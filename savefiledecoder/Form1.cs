using System;

using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Linq;
using System.Reflection;

namespace savefiledecoder
{
    public partial class Form1 : Form
    {
        GameData m_GameData = new GameData();
        GameSave m_GameSave;
        const string c_DataPath = @"Life is Strange - Before the Storm_Data\StreamingAssets\Data\InitialData.et.bytes";
        const string c_AssemblyPath = @"Life is Strange - Before the Storm_Data\Managed\Assembly-CSharp.dll";
        string point_id = "", var_name = "";

        public Form1()
        {
            InitializeComponent();
            ValidatePaths();
        }
         
        private void button1_Click(object sender, EventArgs e)
        {
            byte[] key = ReadKey(Path.Combine(textBoxLisPath.Text, c_AssemblyPath));

            DecodeEncode.SetKey(key);
            string initiDataPath = Path.Combine(textBoxLisPath.Text, c_DataPath);
            m_GameData.Read(initiDataPath);
            m_GameSave = new GameSave(m_GameData);
            m_GameSave.Read(textBoxSavePath.Text);
            if (!GameSave.SaveEmpty) //handles the "Just Started" state.
            {
                UpdateEpsiodeBoxes();
                UpdateDataGrid();
                label4.Visible = false; //hide save file warning
                button2.Enabled = true; //allow exporting
                checkBoxEditMode.Enabled = true;
            }
            else
            {
                MessageBox.Show("Save file is empty or corrupt! Please specify a different one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //textBoxRawJson.Text = m_GameSave.Raw.Replace("\n", Environment.NewLine);

        }

        // only enable boxes for episodes the player has already finished or is currently playing
        private void UpdateEpsiodeBoxes()
        {
            // E1
            if(m_GameSave.EpisodePlayed["E1"])
            {
                checkBoxE1.Enabled = true;
            }
            else
            {
                checkBoxE1.Enabled = false;
                checkBoxE1.Checked = false;
            }
            // E2
            if (m_GameSave.EpisodePlayed["E2"])
            {
                checkBoxE2.Enabled = true;
            }
            else
            {
                checkBoxE2.Enabled = false;
                checkBoxE2.Checked = false;
            }
            // E3
            if (m_GameSave.EpisodePlayed["E3"])
            {
                checkBoxE3.Enabled = true;
            }
            else
            {
                checkBoxE3.Enabled = false;
                checkBoxE3.Checked = false;
            }
            // E4
            if (m_GameSave.EpisodePlayed["E4"])
            {
                checkBoxE4.Enabled = true;
            }
            else
            {
                checkBoxE4.Enabled = false;
                checkBoxE4.Checked = false;
            }
        }

        private void UpdateDataGrid()
        {
            if (m_GameSave == null)
                return;
            dataGridView1.Columns.Clear();
            DataTable table = BuildDataTable();
            dataGridView1.DataSource = table.DefaultView;
            dataGridView1.Columns["Key"].Frozen = true;
            dataGridView1.Columns["Key"].ReadOnly = true;
            dataGridView1.Rows[0].Frozen = true;
            dataGridView1.Rows[1].Frozen = true;
            dataGridView1.Rows[0].ReadOnly = true;
            dataGridView1.Rows[1].ReadOnly = true;
            dataGridView1.Columns[2].HeaderText = "CurrentCheckpoint";

            //temporary code
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++) //do two things in 1 cycle - set the non-variable cells to read only and set all read only cells to gray color
                {
                    if (dataGridView1.Rows[i].Cells[j].Value.ToString() == String.Empty)
                    {
                        dataGridView1.Rows[i].Cells[j].ReadOnly = true;
                    }
                    if (dataGridView1.Rows[i].Cells[j].ReadOnly && editModeActive)
                    {
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.LightGray;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.White;
                    }
                }
            }
            //end temporary

            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }
        private DataTable BuildDataTable()
        {
            DataTable t = new DataTable();
            t.Columns.Add("Key");
            bool first = true;
            for (int i = m_GameSave.Checkpoints.Count - 1; i >= 0; i--)
            {
                if (first)
                {
                    t.Columns.Add("Global");
                    first = false;
                }
                else
                {
                    t.Columns.Add("Checkpoint " + (i + 1).ToString());
                }
            }

            // current point
            object[] row = new object[t.Columns.Count];
            row[0] = "PointIdentifier";
            for (int i = m_GameSave.Checkpoints.Count - 1; i >= 0; i--)
            {
                row[m_GameSave.Checkpoints.Count - i] = m_GameSave.Checkpoints[i].PointIdentifier;
            }
            t.Rows.Add(row);
            // current objective
            row[0] = "Objective";
            for (int i = m_GameSave.Checkpoints.Count - 1; i >= 0; i--)
            {
                row[m_GameSave.Checkpoints.Count - i] = m_GameSave.Checkpoints[i].Objective;
            }
            t.Rows.Add(row);


            // variables 
            foreach (var varType in m_GameData.Variables.OrderBy((v) => v.Value.name))
            {
                string varName = varType.Value.name.ToUpper();
                if (!checkBoxE1.Checked && varName.StartsWith("E1_") && editModeActive == false)
                {
                    continue;
                }
                if (!checkBoxE2.Checked && varName.StartsWith("E2_") && editModeActive == false)
                {
                    continue;
                }
                if (!checkBoxE3.Checked && varName.StartsWith("E3_") && editModeActive == false)
                {
                    continue;
                }
                if (!checkBoxE4.Checked && varName.StartsWith("E4_") && editModeActive == false)
                {
                    continue;
                }

                row[0] = varType.Value.name;
                for (int i = m_GameSave.Checkpoints.Count - 1; i >= 0; i--)
                {
                    var checkpoint = m_GameSave.Checkpoints[i];
                    VariableState state;
                    bool found = checkpoint.Variables.TryGetValue(varType.Value.name, out state);
                    if (found)
                    {
                        row[m_GameSave.Checkpoints.Count - i] = state.Value;
                    }
                    else
                    {
                        row[m_GameSave.Checkpoints.Count - i] = null;
                    }
                }
                t.Rows.Add(row);
            }

            return t;
        }

        private void ValidatePaths()
        {
            bool successDataPath = false;
            try
            {
                string dataPath = Path.Combine(textBoxLisPath.Text, c_DataPath);
                successDataPath = File.Exists(dataPath);
            }
            catch
            {

            }
            if (successDataPath)
            {
                textBoxLisPath.BackColor = System.Drawing.SystemColors.Window;
            }
            else
            {
                textBoxLisPath.BackColor = System.Drawing.Color.Red;
            }

            bool successSavePath = false;
            try
            {
                string savePath = Path.Combine(textBoxSavePath.Text);
                successSavePath = File.Exists(savePath);
            }
            catch
            {

            }
            if (successSavePath)
            {
                textBoxSavePath.BackColor = System.Drawing.SystemColors.Window;
            }
            else
            {
                textBoxSavePath.BackColor = System.Drawing.Color.Red;
            }
            if (successDataPath && successSavePath)
            {
                button1.Enabled = true;
                button2.Enabled = false;
                checkBoxEditMode.Enabled = false;
                label4.Text = "Save file changed! Press Show Content to update.";
                label4.Visible = true; //shows warning about save file
                SaveFileViewer.Properties.Settings.Default.Save();
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = false;
                checkBoxEditMode.Enabled = false;
            }
        }


        private byte[] ReadKey(string assemblyPath)
        {
            var ass = Assembly.LoadFile(assemblyPath);
            Type t = ass.GetType("T_3EF937CB");
            FieldInfo keyField = t.GetField("_18AFCD9AB", BindingFlags.Static | BindingFlags.NonPublic);
            return (byte[])keyField.GetValue(null);

        }

        private void checkBoxEpisodes_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }

        private void textBoxSavePath_TextChanged(object sender, EventArgs e)
        {
            ValidatePaths();
        }

        private void textBoxLisPath_TextChanged(object sender, EventArgs e)
        {
            ValidatePaths();
        }

        //export
        private void button2_Click(object sender, EventArgs e)
        {
            using (StreamWriter file = new StreamWriter("objectives.txt"))
                for (int i = m_GameSave.Checkpoints.Count - 1; i >= 0; i--)
                {
                    file.WriteLine("\"{0}\"", m_GameSave.Checkpoints[i].Objective);
                }

            using (StreamWriter file = new StreamWriter("checkpoints.txt"))
                for (int i = m_GameSave.Checkpoints.Count - 1; i >= 0; i--)
                {
                    file.WriteLine("\"{0}\"", m_GameSave.Checkpoints[i].Objective);
                }

            using (StreamWriter file = new StreamWriter("variables.txt"))
                foreach (var entry in m_GameData.Variables.OrderBy((v) => v.Value.name))
                {
                    var checkpoint = m_GameSave.Checkpoints[m_GameSave.Checkpoints.Count - 1];
                    VariableState state;
                    bool valFound = checkpoint.Variables.TryGetValue(entry.Value.name, out state);
                    if (valFound)
                    {
                        file.WriteLine("\"{0}\", {1}", entry.Value.name.ToUpper(), state.Value);
                    }
                    else if (Form.ModifierKeys == Keys.Control)
                    {
                        file.WriteLine("\"{0}\"", entry.Value.name.ToUpper());
                    }
                }
            System.Diagnostics.Process.Start("variables.txt"); //open the text file

        }
        //browse for Data.Save
        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                SaveFileViewer.Properties.Settings.Default.SavePath = openFileDialog1.FileName;
                textBoxSavePath.Text = openFileDialog1.FileName;
            }
        }
        //browse for BTS install directory
        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                SaveFileViewer.Properties.Settings.Default.BTSpath = folderBrowserDialog1.SelectedPath;
                textBoxLisPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Version 0.4\nTool by /u/DanielWe\nModified by Ladosha and IgelRM", "About Savegame Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxSavePath.Text = SaveFileViewer.Properties.Settings.Default.SavePath;
            textBoxLisPath.Text = SaveFileViewer.Properties.Settings.Default.BTSpath;
            folderBrowserDialog1.SelectedPath = SaveFileViewer.Properties.Settings.Default.BTSpath;
            label4.Visible = false;
        }

        private void buttonSaveEdits_Click(object sender, EventArgs e)
        {
            m_GameSave.Write(textBoxSavePath.Text, m_GameSave.m_Data);
            if (m_GameSave.editsSaved) MessageBox.Show("Saved successfully!", "Savegame Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            label4.Visible = false;  
        }

        public int? origCellValue, newCellValue;
        private string cellType = "";

        public bool editModeActive = false;
        bool editModeIntroShown = false;
        private void checkBoxEditMode_MouseUp(object sender, EventArgs e)
        {
            if (!editModeIntroShown)
            {
                MessageBox.Show("Note that the Edit Mode is highly experimental. It has not been extensively tested and might make the game crash unexpectedly, or even completely refuse to save to or load from the modified file, not to mention causing tornados in and around Arcadia Bay.\n\nTo start editing a cell, double click on it, or select it with the mouse/arrow keys and press F2. Editing of gray-colored cells is not permitted.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                editModeIntroShown = true;
            }

            if (checkBoxEditMode.Checked)
            {
                enableEditMode();
            }
            else
            {
                if (!m_GameSave.editsSaved)
                {
                    DialogResult answer = MessageBox.Show("There are unsaved edits left!\nExit Edit Mode without saving?", "Savegame Viewer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        disableEditMode();
                    }
                    else
                    {
                        checkBoxEditMode.Checked = true;
                    }
                }
                else disableEditMode();
            }
        }

        private void enableEditMode()
        {
            editModeActive = true;
            dataGridView1.ReadOnly = false;
            //checkBoxE1.Checked = true;
            //checkBoxE2.Checked = true;
            //checkBoxE3.Checked = true;
            //checkBoxE4.Checked = true;
            checkBoxE1.Enabled = false;
            checkBoxE2.Enabled = false;
            checkBoxE3.Enabled = false;
            checkBoxE4.Enabled = false;
            buttonSaveEdits.Enabled = true;
            buttonExtras.Enabled = true;
            UpdateDataGrid();
        }
        private void disableEditMode()
        {
            editModeActive = false;
            dataGridView1.ReadOnly = true;
            UpdateEpsiodeBoxes();
            buttonSaveEdits.Enabled = false;
            buttonExtras.Enabled = false;
            UpdateDataGrid();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == String.Empty) //if the cell was originally empty
            {
                origCellValue = null;
            }
            else
            {
                origCellValue = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                switch (e.ColumnIndex)
                {
                    case 1: cellType = "global"; break;
                    case 2: cellType = "current"; break;
                    case 3: cellType = "last"; break;
                    default: cellType = "normal"; break;
                }
            }
        }

        private void buttonExtras_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming soon!", "Savegame Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == String.Empty)
            {
                newCellValue = null;
            }
            else
            {
                int result; //result of parsing
                if (int.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out result))
                {
                    newCellValue = result;
                }
                else
                {
                    MessageBox.Show("Variable value contains non-numeric characters! Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    newCellValue = origCellValue;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = origCellValue;
                }
            }

            if (newCellValue != origCellValue)
            {
                point_id = dataGridView1.Rows[0].Cells[e.ColumnIndex].Value.ToString();
                var_name = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                //MessageBox.Show("Finished Editing of Cell on Column " + e.ColumnIndex.ToString() + " and Row " + e.RowIndex.ToString() + "\n Value of the cell is " + newCellValue.ToString(), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show("The Identifier of edited cell is " + point_id  + "\n and the variable name is " + var_name, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (m_GameSave.FindAndUpdateVarValue(point_id, var_name, newCellValue, cellType))
                {
                    label4.Text = "Press Save to write changes to the save file.";
                    label4.Visible = true;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_GameSave != null && !m_GameSave.editsSaved)
            {
                DialogResult answer = MessageBox.Show("There are unsaved edits left! Exit without saving?", "Savegame Viewer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer == DialogResult.Yes)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else e.Cancel = false;
        }
    }
}



