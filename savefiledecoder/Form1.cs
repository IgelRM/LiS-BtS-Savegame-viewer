using System;

using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Web.Helpers;

namespace savefiledecoder
{
    public partial class Form1 : Form
    {
        Form2 browseForm = new Form2();
        GameData m_GameData = new GameData();
        GameSave m_GameSave;
        const string c_DataPath = @"Life is Strange - Before the Storm_Data\StreamingAssets\Data\InitialData.et.bytes";
        const string c_AssemblyPath = @"Life is Strange - Before the Storm_Data\Managed\Assembly-CSharp.dll";
        string point_id = "", var_name = "";
        List<string> SteamIDFolders = new List<string>();
        public static string selectedSavePath = SaveFileViewer.Properties.Settings.Default.SavePath;
        dynamic appSettings = Json.Decode("{}");

        public Form1()
        {
            InitializeComponent();
            ValidatePaths();
        }

        bool resizeHelpShown = false;
        private void buttonShowContent_Click(object sender, EventArgs e)
        {
            byte[] key = ReadKey(Path.Combine(textBoxLisPath.Text, c_AssemblyPath));

            DecodeEncode.SetKey(key);
            string initiDataPath = Path.Combine(textBoxLisPath.Text, c_DataPath);
            m_GameData.Read(initiDataPath);
            m_GameSave = new GameSave(m_GameData);
            m_GameSave.Read(textBoxSavePath.Text);
#if DEBUG
            if (Form.ModifierKeys == Keys.Control)
            {
                File.WriteAllText(textBoxSavePath.Text + @".txt", m_GameSave.Raw);
                File.WriteAllText(textBoxSavePath.Text + @"-initialdata.txt", m_GameData.Raw);
                if (m_GameSave.m_Header != null)
                {
                    File.WriteAllText(textBoxSavePath.Text + @"-header.txt", m_GameSave.h_Raw);
                }
            }
#endif
            if (!m_GameSave.SaveEmpty) //handles the "Just Started" state.
            {
                UpdateEpsiodeBoxes();
                UpdateFlagGrid();
                UpdateFloatGrid();
                UpdateDataGrid();
                label4.Visible = false; //hide save file warning
                buttonExport.Enabled = true; //allow exporting
                buttonExtras.Enabled = true;
                checkBoxEditMode.Enabled = true;
                SaveFileViewer.Properties.Settings.Default.BTSpath = textBoxLisPath.Text;
                SaveFileViewer.Properties.Settings.Default.SavePath = textBoxSavePath.Text;

                if (!resizeHelpShown)
                {
                    ToolTip tt = new ToolTip();
                    tt.IsBalloon = true;
                    tt.Show("Drag here to resize", this, 140, 115, 2000);
                    resizeHelpShown = true;
                }  
            }
            else
            {
                MessageBox.Show("Save file is empty or corrupt! Please specify a different one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
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
            }
            // E2
            if (m_GameSave.EpisodePlayed["E2"])
            {
                checkBoxE2.Enabled = true;
            }
            else
            {
                checkBoxE2.Enabled = false;
            }
            // E3
            if (m_GameSave.EpisodePlayed["E3"])
            {
                checkBoxE3.Enabled = true;
            }
            else
            {
                checkBoxE3.Enabled = false;
            }
            // E4
            if (m_GameSave.EpisodePlayed["E4"])
            {
                checkBoxE4.Enabled = true;
            }
            else
            {
                checkBoxE4.Enabled = false;
            }
        }

        int visible_row = 2, visible_column = 1;
        int f_visible_row = 0, f_visible_column = 1;

        private void UpdateDataGrid()
        {
            if (m_GameSave == null)
                return;

            int keyColWidth = 100;
            try
            {
                keyColWidth = dataGridView1.Columns[0].Width;
            }
            catch
            {

            }

            if (dataGridView1.FirstDisplayedScrollingRowIndex <= 2)
            {
                visible_row = 2;
            }
            else
            {
                visible_row = dataGridView1.FirstDisplayedScrollingRowIndex;
            }
            if (dataGridView1.FirstDisplayedScrollingColumnIndex <= 1)
            {
                visible_column = 1;
            }
            else if (dataGridView1.FirstDisplayedScrollingColumnHiddenWidth > 60 )
            {
                visible_column = dataGridView1.FirstDisplayedScrollingColumnIndex+1;
            }
            else
            {
                visible_column = dataGridView1.FirstDisplayedScrollingColumnIndex;
            }

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

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                { 
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
            
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            try
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = visible_row;
                dataGridView1.FirstDisplayedScrollingColumnIndex = visible_column;
            }
            catch
            {

            }
            dataGridView1.Columns[0].Width = keyColWidth;

        }

        private void UpdateFloatGrid()
        {
            if (m_GameSave == null)
                return;

            int keyColWidth = 100;
            try
            {
                keyColWidth = dataGridViewFloats.Columns[0].Width;
            }
            catch
            {

            }

            if (dataGridViewFloats.FirstDisplayedScrollingRowIndex <= 0)
            {
                visible_row = 0;
            }
            else
            {
                visible_row = dataGridViewFloats.FirstDisplayedScrollingRowIndex;
            }
            if (dataGridViewFloats.FirstDisplayedScrollingColumnIndex <= 1)
            {
                visible_column = 1;
            }
            else if (dataGridViewFloats.FirstDisplayedScrollingColumnHiddenWidth > 60)
            {
                visible_column = dataGridViewFloats.FirstDisplayedScrollingColumnIndex + 1;
            }
            else
            {
                visible_column = dataGridViewFloats.FirstDisplayedScrollingColumnIndex;
            }

            dataGridViewFloats.Columns.Clear();
            DataTable table = BuildFloatTable();
            dataGridViewFloats.DataSource = table.DefaultView;
            dataGridViewFloats.Columns["Key"].Frozen = true;
            dataGridViewFloats.Columns["Key"].ReadOnly = true;
            dataGridViewFloats.Rows[0].Frozen = true;
            dataGridViewFloats.Rows[0].ReadOnly = true;
            dataGridViewFloats.Columns[2].HeaderText = "CurrentCheckpoint";

            for (int i = 0; i < dataGridViewFloats.RowCount; i++)
            {
                for (int j = 0; j < dataGridViewFloats.ColumnCount; j++)
                {
                    if (dataGridViewFloats.Rows[i].Cells[j].ReadOnly && editModeActive)
                    {
                        dataGridViewFloats.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.LightGray;
                    }
                    else
                    {
                        dataGridViewFloats.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.White;
                    }
                }
            }

            for (int i = 0; i < dataGridViewFloats.ColumnCount; i++)
            {
                dataGridViewFloats.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            try
            {
                dataGridViewFloats.FirstDisplayedScrollingRowIndex = visible_row;
                dataGridViewFloats.FirstDisplayedScrollingColumnIndex = visible_column;
            }
            catch
            {

            }
            dataGridViewFloats.Columns[0].Width = keyColWidth;

        }

        private void UpdateFlagGrid()
        {
            if (m_GameSave == null)
                return;

            int keyColWidth = 100;
            try
            {
                keyColWidth = dataGridViewFlags.Columns[0].Width;
            }
            catch
            {

            }

            if (dataGridViewFlags.FirstDisplayedScrollingRowIndex <= 0)
            {
                f_visible_row = 0;
            }
            else
            {
                f_visible_row = dataGridViewFlags.FirstDisplayedScrollingRowIndex;
            }
            if (dataGridViewFlags.FirstDisplayedScrollingColumnIndex <= 1)
            {
                f_visible_column = 1;
            }
            else if (dataGridViewFlags.FirstDisplayedScrollingColumnHiddenWidth > 60)
            {
                f_visible_column = dataGridViewFlags.FirstDisplayedScrollingColumnIndex + 1;
            }
            else
            {
                f_visible_column = dataGridViewFlags.FirstDisplayedScrollingColumnIndex;
            }
            dataGridViewFlags.Columns.Clear();
            DataTable table = BuildFlagTable();
            dataGridViewFlags.DataSource = table.DefaultView;
            dataGridViewFlags.Columns["Key"].Frozen = true;
            dataGridViewFlags.Columns["Key"].ReadOnly = true;
            dataGridViewFlags.Rows[0].Frozen = true;
            dataGridViewFlags.Rows[0].ReadOnly = true;
            dataGridViewFlags.Columns[1].HeaderText = "CurrentCheckpoint";

            for (int i = 0; i < dataGridViewFlags.RowCount; i++)
            {
                for (int j = 0; j < dataGridViewFlags.ColumnCount; j++)
                {
                    if (dataGridViewFlags.Rows[i].Cells[j].ReadOnly && editModeActive)
                    {
                        dataGridViewFlags.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.LightGray;
                    }
                    else
                    {
                        dataGridViewFlags.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.White;
                    }
                }
            }

            for (int i = 1; i < dataGridViewFlags.RowCount; i++)
            {
                for (int j = 1; j < dataGridViewFlags.ColumnCount; j++)
                {
                    DataGridViewCheckBoxCell CheckBoxCell = new DataGridViewCheckBoxCell();
                    CheckBoxCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewFlags.Rows[i].Cells[j] = CheckBoxCell;
                }
            }

            for (int i = 0; i < dataGridViewFlags.ColumnCount; i++)
            {
                dataGridViewFlags.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            try
            {
                dataGridViewFlags.FirstDisplayedScrollingRowIndex = f_visible_row;
                dataGridViewFlags.FirstDisplayedScrollingColumnIndex = f_visible_column;
            }
            catch
            {

            }
            dataGridViewFlags.Columns[0].Width = keyColWidth;
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

        private DataTable BuildFloatTable()
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

            // floats
            foreach (var flt in m_GameSave.m_Data.floatValuesDict)
            {
                if (flt.Name == "$type") continue;
                row[0] = flt.Name;
                for (int i = m_GameSave.Checkpoints.Count - 1; i >= 0; i--)
                {
                    var checkpoint = m_GameSave.Checkpoints[i];
                    FloatState state;
                    bool found = checkpoint.Floats.TryGetValue(flt.Name, out state);
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

        private DataTable BuildFlagTable()
        {
            DataTable t = new DataTable();
            t.Columns.Add("Key");
            for (int i = m_GameSave.Checkpoints.Count - 2; i >= 0; i--)
            {
                t.Columns.Add("Checkpoint " + (i+1).ToString());
            }

            // current point
            object[] row = new object[t.Columns.Count];
            row[0] = "PointIdentifier";
            for (int i = m_GameSave.Checkpoints.Count - 2; i >= 0; i--)
            {
                row[m_GameSave.Checkpoints.Count - i-1] = m_GameSave.Checkpoints[i].PointIdentifier;
            }
            t.Rows.Add(row);

            // flags
            foreach (var flag in m_GameSave.m_Data.flags)
            {
                row[0] = flag.Value;
                for (int i = m_GameSave.Checkpoints.Count - 2; i >=0; i--)
                {
                    int rownum = m_GameSave.Checkpoints.Count - i;
                    row[rownum-1] = m_GameSave.Checkpoints[i].Flags.Contains(flag.Value);
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
                if (File.Exists(savePath) && Path.GetFileName(savePath) == "Data.Save")
                {
                    successSavePath = true;
                }
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
                buttonShowContent.Enabled = true;
                buttonExport.Enabled = false;
                buttonExtras.Enabled = false;
                checkBoxEditMode.Enabled = false;
                try
                {
                    string saveRoot = Directory.GetParent(Directory.GetParent(Directory.GetParent(textBoxSavePath.Text).ToString()).ToString()).ToString().ToLowerInvariant();
                    string testRoot = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\LocalLow\Square Enix\Life Is Strange_ Before The Storm\Saves").ToLowerInvariant();
                    if (saveRoot != testRoot)
                    {
                        buttonSaveSelector.Enabled = false;
                    }
                    else
                    {
                        buttonSaveSelector.Enabled = true;
                    }
                }
                catch
                {
                    buttonSaveSelector.Enabled = false;
                }
                
                label4.Text = "Save file changed! Press 'Show Content' to update.";
                label4.Visible = true; //shows warning about save file
            }
            else
            {
                buttonShowContent.Enabled = false;
                buttonExport.Enabled = false;
                buttonExtras.Enabled = false;
                buttonSaveSelector.Enabled = false;
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
            if (!editModeActive)
            {
                UpdateDataGrid();
            }
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
        private void buttonExport_Click(object sender, EventArgs e)
        {
            using (StreamWriter file = new StreamWriter("objectives.txt"))
                for (int i = m_GameSave.Checkpoints.Count - 1; i >= 0; i--)
                {
                    file.WriteLine("\"{0}\"", m_GameSave.Checkpoints[i].Objective);
                }

            using (StreamWriter file = new StreamWriter("checkpoints.txt"))
                for (int i = m_GameSave.Checkpoints.Count - 1; i >= 0; i--)
                {
                    file.WriteLine("\"{0}\"", m_GameSave.Checkpoints[i].PointIdentifier);
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
            if (ModifierKeys == Keys.Control && File.Exists(textBoxSavePath.Text))
            {
                System.Diagnostics.Process.Start(Directory.GetParent(textBoxSavePath.Text).ToString());
            }
            else
            {
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    SaveFileViewer.Properties.Settings.Default.SavePath = openFileDialog1.FileName;
                    textBoxSavePath.Text = openFileDialog1.FileName;
                }
            }
            
        }
        //browse for BTS install directory
        private void button6_Click(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Control && Directory.Exists(textBoxLisPath.Text))
            {
                System.Diagnostics.Process.Start(textBoxLisPath.Text);
            }
            else
            {
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    SaveFileViewer.Properties.Settings.Default.BTSpath = folderBrowserDialog1.SelectedPath;
                    textBoxLisPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Version 0.7\nTool by /u/DanielWe\nModified by Ladosha and IgelRM\nhttps://github.com/IgelRM/LiS-BtS-Savegame-viewer", "About Savegame Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("settings.json"))
            {
                string file = File.ReadAllText("settings.json");
                try
                {
                    appSettings = Json.Decode(file);
                    SaveFileViewer.Properties.Settings.Default.SavePath = appSettings.SavePath;
                    SaveFileViewer.Properties.Settings.Default.BTSpath = appSettings.BTSpath;
                    SaveFileViewer.Properties.Settings.Default.rewindNotesShown = appSettings.rewindNotesShown;
                    SaveFileViewer.Properties.Settings.Default.editModeIntroShown = appSettings.editModeIntroShown;
                }
                catch
                {

                }
            }

            ToolTip toolTip = new ToolTip();
            toolTip.BackColor = System.Drawing.SystemColors.InfoText;
            toolTip.IsBalloon = true;
            toolTip.SetToolTip(buttonExport, "Click to export variables with a value into a text file.\nCtrl+Click to export all variables.");

            if (SaveFileViewer.Properties.Settings.Default.BTSpath == "Undefined")
            {
                DetectBtsPath();
            }
            else
            {
                textBoxLisPath.Text = SaveFileViewer.Properties.Settings.Default.BTSpath;
                folderBrowserDialog1.SelectedPath = SaveFileViewer.Properties.Settings.Default.BTSpath;
            }

            DetectSavePath();
            label4.Visible = false;
            tabControl1.SelectedTab = tabPageFlags;
            tabControl1.SelectedTab = tabPageFloats;
            tabControl1.SelectedTab = tabPageVars;

            //double buffering
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridViewFlags, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridViewFloats, new object[] { true });

        }

        private void DetectBtsPath ()
        {
            RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64); //https://social.msdn.microsoft.com/Forums/vstudio/en-US/ef0de98a-18db-43e1-b9b9-b52c3b5f3d4c/registry-issue-getting-install-location-and-saving-its-path-c?forum=csharpgeneral
            localKey = localKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 554620");
            try 
            {
                textBoxLisPath.Text = localKey.GetValue("InstallLocation").ToString();
                //Console.WriteLine(localKey.GetValue("InstallLocation").ToString());
            }
            catch
            {
                textBoxLisPath.Text = "Auto-detection failed! Please select the path manually.";
            }
        }

        private void DetectSavePath ()
        {
            try
            {
                SteamIDFolders = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\LocalLow\Square Enix\Life Is Strange_ Before The Storm\Saves").ToList<string>();
            }
            catch
            {

            }
            if (SteamIDFolders.Count != 0)
            {
                SteamIDFolders.RemoveAt(SteamIDFolders.Count - 1); //remove the preferences from the list
            }
            if (SaveFileViewer.Properties.Settings.Default.SavePath == "Undefined")
            {
                if (SteamIDFolders.Count == 1)
                {
                    bool found = false;
                    for (int i=0; i<3; i++)
                    {
                        if (File.Exists(SteamIDFolders[0].ToString() + @"\SLOT_0" + i.ToString() + @"\Data.Save"))
                        {
                            textBoxSavePath.Text = SteamIDFolders[0].ToString() + @"\SLOT_0" + i.ToString() + @"\Data.Save";
                            SaveFileViewer.Properties.Settings.Default.SavePath = textBoxSavePath.Text;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        textBoxSavePath.Text = "Auto-detection failed! Please select the path manually.";
                    }
                }
                else if (SteamIDFolders.Count > 1)
                {
                    browseForm.SteamIDFolders = this.SteamIDFolders;
                    browseForm.updateComboBox1();
                    browseForm.savenumber = 0;
                    browseForm.steamid = SteamIDFolders[0];
                    byte[] key = ReadKey(Path.Combine(textBoxLisPath.Text, c_AssemblyPath));
                    DecodeEncode.SetKey(key);
                    string initiDataPath = Path.Combine(textBoxLisPath.Text, c_DataPath);
                    m_GameData.Read(initiDataPath);
                    if (m_GameSave == null)
                    {
                        m_GameSave = new GameSave(m_GameData);
                    }
                    browseForm.m_GameSave = m_GameSave;
                    browseForm.ShowDialog();
                    updateSavePath();
                }
                else
                {
                    textBoxSavePath.Text = "Auto-detection failed! Please select the path manually.";
                }
            }
            else
            {
                textBoxSavePath.Text = SaveFileViewer.Properties.Settings.Default.SavePath;
            }
        }

        public void updateSavePath()
        {
            textBoxSavePath.Text = selectedSavePath;
            SaveFileViewer.Properties.Settings.Default.SavePath = textBoxSavePath.Text;
        }

        private void buttonSaveEdits_Click(object sender, EventArgs e)
        {
            m_GameSave.WriteData(textBoxSavePath.Text, m_GameSave.m_Data);
            if (m_GameSave.editsSaved) MessageBox.Show("Saved successfully!", "Savegame Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            label4.Visible = false;

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
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

            for (int i = 0; i < dataGridViewFloats.RowCount; i++)
            {
                for (int j = 0; j < dataGridViewFloats.ColumnCount; j++)
                {
                    if (dataGridViewFloats.Rows[i].Cells[j].ReadOnly && editModeActive)
                    {
                        dataGridViewFloats.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.LightGray;
                    }
                    else
                    {
                        dataGridViewFloats.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.White;
                    }
                }
            }

            for (int i = 0; i < dataGridViewFlags.RowCount; i++)
            {
                for (int j = 0; j < dataGridViewFlags.ColumnCount; j++)
                {
                    if (dataGridViewFlags.Rows[i].Cells[j].ReadOnly && editModeActive)
                    {
                        dataGridViewFlags.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.LightGray;
                    }
                    else
                    {
                        dataGridViewFlags.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.White;
                    }
                }
            }
        }

        public int? origCellValue, newCellValue;
        public float? origFloatValue, newFloatValue;
        public bool origFlagState, newFlagState;
        private string cellType = "";

        public bool editModeActive = false;

        private void checkBoxEditMode_MouseUp(object sender, MouseEventArgs e)
        {
            if (!SaveFileViewer.Properties.Settings.Default.editModeIntroShown)
            {
                MessageBox.Show("Note that the 'Edit Mode' is experimental. In some cases, it might make the game crash unexpectedly, or even completely refuse to save to or load from the modified file, not to mention causing tornados in and around Arcadia Bay.\n\nVariables/Floats: Select a cell (or a range of cells) using the mouse or the arrow keys, and type in the new value. If you accidentally selected the wrong cell(s), then press ESC to cancel the edit.\n\nFlags: Simply check or uncheck the respective boxes in the table. You can use the mouse or the arrow keys and Spacebar. To edit multiple flags at once, select them and press Shift+T (True) of Shift+F (False).\n\nNewly edited but unsaved cells are marked with yellow. Editing of gray-colored cells is not permitted.", "Savegame Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SaveFileViewer.Properties.Settings.Default.editModeIntroShown = true;
            }

            if (checkBoxEditMode.Checked)
            {
                enableEditMode();
            }
            else
            {
                if (!m_GameSave.editsSaved)
                {
                    DialogResult answer = MessageBox.Show("There are unsaved edits left!\nExit 'Edit Mode' without saving?", "Savegame Viewer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            dataGridViewFlags.ReadOnly = false;
            dataGridViewFloats.ReadOnly = false;
            buttonShowContent.Enabled = false;
            buttonManualBrowseBts.Enabled = false;
            buttonManualBrowseSave.Enabled = false;
            buttonSaveSelector.Enabled = false;
            textBoxLisPath.Enabled = false;
            textBoxSavePath.Enabled = false;
            checkBoxE1.Enabled = false;
            checkBoxE2.Enabled = false;
            checkBoxE3.Enabled = false;
            checkBoxE4.Enabled = false;
            buttonSaveEdits.Enabled = true;
            buttonExtras.Enabled = false;
            UpdateDataGrid();
            UpdateFlagGrid();
            UpdateFloatGrid();
        }
        private void disableEditMode()
        {
            editModeActive = false;
            m_GameSave.editsSaved = true;
            dataGridView1.ReadOnly = true;
            dataGridViewFlags.ReadOnly = true;
            dataGridViewFloats.ReadOnly = true;
            buttonShowContent.Enabled = true;
            buttonManualBrowseBts.Enabled = true;
            buttonManualBrowseSave.Enabled = true;
            buttonSaveSelector.Enabled = true;
            textBoxLisPath.Enabled = true;
            textBoxSavePath.Enabled = true;
            UpdateEpsiodeBoxes();
            if (checkBoxE1.Enabled) checkBoxE1.Checked = true;
            if (checkBoxE2.Enabled) checkBoxE2.Checked = true;
            if (checkBoxE3.Enabled) checkBoxE3.Checked = true;
            if (checkBoxE4.Enabled) checkBoxE4.Checked = true;
            buttonSaveEdits.Enabled = false;
            buttonExtras.Enabled = true;
            label4.Visible = false;
            m_GameSave.Read(textBoxSavePath.Text);
            UpdateDataGrid();
            UpdateFlagGrid();
            UpdateFloatGrid();
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
            }
            switch (e.ColumnIndex)
            {
                case 1: cellType = "global"; break;
                case 2: cellType = "current"; break;
                case 3: cellType = "last"; break;
                default: cellType = "normal"; break;
            }
        }

        private void dataGridViewFloats_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dataGridViewFloats.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == String.Empty) //if the cell was originally empty
            {
                origFloatValue = null;
            }
            else
            {
                origFloatValue = float.Parse(dataGridViewFloats.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace('.', ','));
            }
            switch (e.ColumnIndex)
            {
                case 1: cellType = "global"; break;
                case 2: cellType = "current"; break;
                case 3: cellType = "last"; break;
                default: cellType = "normal"; break;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Variables/Floats: Select a cell (or a range of cells) using the mouse or the arrow keys, and type in the new value. If you accidentally selected the wrong cell(s), then press ESC to cancel the edit.\n\nFlags: Simply check or uncheck the respective boxes in the table. You can use the mouse or the arrow keys and Spacebar. To edit multiple flags at once, select them and press Shift+T (True) of Shift+F (False).\n\nNewly edited but unsaved cells are marked with yellow. Editing of gray-colored cells is not permitted.", "Help", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void dataGridViewFlags_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            origFlagState = Convert.ToBoolean(dataGridViewFlags.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            if (e.ColumnIndex == 1)
            {
                cellType = "current";
            }
            else
            {
                cellType = "normal";
            }
        }

        private void dataGridViewFlags_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            newFlagState = Convert.ToBoolean(dataGridViewFlags.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

            if (newFlagState != origFlagState)
            {
                point_id = dataGridViewFlags.Rows[0].Cells[e.ColumnIndex].Value.ToString();
                var_name = dataGridViewFlags.Rows[e.RowIndex].Cells[0].Value.ToString();
                //MessageBox.Show("Finished Editing of Cell on Column " + e.ColumnIndex.ToString() + " and Row " + e.RowIndex.ToString() + "\n Value of the cell is " + newFlagState.ToString(), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show("The Identifier of edited cell is " + point_id  + "\n and the flag name is " + flag_name, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (m_GameSave.FindAndUpdateFlagValue(point_id, var_name, origFlagState, cellType))
                {
                    dataGridViewFlags.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                    label4.Text = "Press 'Save' to write changes to the save file.";
                    label4.Visible = true;
                }
                else dataGridViewFlags.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = origFlagState;
            }
        }
        bool firstEdit = true;
        private void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            int number;
            if (firstEdit && (String.IsNullOrWhiteSpace(e.Value.ToString()) || int.TryParse(e.Value.ToString(), out number)))
            {
                firstEdit = false;
                fillCellsWithValue(dataGridView1.SelectedCells, e.Value);
                firstEdit = true;
            }
        }

        private void dataGridViewFloats_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            float number;
            if (firstEdit && (String.IsNullOrWhiteSpace(e.Value.ToString()) || float.TryParse(e.Value.ToString().Replace('.', ','), out number)))
            {
                firstEdit = false;
                fillCellsWithValue(dataGridViewFloats.SelectedCells, e.Value);
                firstEdit = true;
            }
        }

        private void dataGridViewFlags_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'T')
            {
                fillCellsWithValue(dataGridViewFlags.SelectedCells, true);
            }
            else if (e.KeyChar == 'F')
            {
                fillCellsWithValue(dataGridViewFlags.SelectedCells, false);
            }
        }

        private void buttonSaveSelector_Click(object sender, EventArgs e)
        {
            try
            {
                SteamIDFolders = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\LocalLow\Square Enix\Life Is Strange_ Before The Storm\Saves").ToList<string>();
                if (SteamIDFolders.Count != 0)
                {
                    SteamIDFolders.RemoveAt(SteamIDFolders.Count - 1); //remove the preferences from the list
                }
            }
            catch
            {
                MessageBox.Show("Could not find save folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            browseForm.SteamIDFolders = this.SteamIDFolders;
            browseForm.updateComboBox1();
            string folder = Directory.GetParent(textBoxSavePath.Text).ToString();
            browseForm.savenumber = int.Parse(folder.Substring(folder.Length - 1));
            browseForm.steamid = Path.GetDirectoryName(folder).Remove(0, Path.GetDirectoryName(folder).LastIndexOf('\\')+1);

            byte[] key = ReadKey(Path.Combine(textBoxLisPath.Text, c_AssemblyPath));
            DecodeEncode.SetKey(key);
            string initiDataPath = Path.Combine(textBoxLisPath.Text, c_DataPath);
            m_GameData.Read(initiDataPath);
            if (m_GameSave == null)
            {
                m_GameSave = new GameSave(m_GameData);
            }
            m_GameSave.Read(textBoxSavePath.Text);

            browseForm.m_GameSave = m_GameSave;
            browseForm.ShowDialog();
            updateSavePath();
        }

        private void buttonExtras_Click(object sender, EventArgs e)
        {
            FormExtras formExtras = new FormExtras();
            formExtras.savePath = textBoxSavePath.Text;
            formExtras.headerPath = Path.GetDirectoryName(textBoxSavePath.Text) + @"\Header.Save";
            formExtras.m_GameSave = m_GameSave;
            formExtras.ShowDialog(); //prevent the user from chaging things in main form while the extras is open
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
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
                if (m_GameSave.FindAndUpdateVarValue(point_id, var_name, origCellValue, newCellValue, cellType))
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                    label4.Text = "Press 'Save' to write changes to the save file.";
                    label4.Visible = true;
                }
                else dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = origCellValue;
            }
        }

        private void fillCellsWithValue (DataGridViewSelectedCellCollection selectedCells, object value)
        {
            foreach (DataGridViewCell cell in selectedCells)
            {
                if (!cell.ReadOnly)
                {
                    cell.DataGridView.CurrentCell = cell;
                    cell.DataGridView.BeginEdit(false);
                    cell.Value = value;
                    cell.DataGridView.EndEdit();
                }
            }
        }

        private void dataGridViewFloats_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dataGridViewFloats.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
            {
                newFloatValue = null;
            }
            else
            {
                float result; //result of parsing
                if (float.TryParse(dataGridViewFloats.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace('.', ','), out result))
                {
                    newFloatValue = result;
                }
                else
                {
                    MessageBox.Show("Variable value contains non-numeric characters! Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    newFloatValue = origFloatValue;
                    dataGridViewFloats.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = origFloatValue;
                }
            }

            if (newFloatValue != origFloatValue)
            {
                point_id = dataGridViewFloats.Rows[0].Cells[e.ColumnIndex].Value.ToString();
                var_name = dataGridViewFloats.Rows[e.RowIndex].Cells[0].Value.ToString();
                //MessageBox.Show("Finished Editing of Cell on Column " + e.ColumnIndex.ToString() + " and Row " + e.RowIndex.ToString() + "\n Value of the cell is " + newFloatValue.ToString(), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show("The Identifier of edited cell is " + point_id  + "\n and the variable name is " + var_name, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (m_GameSave.FindAndUpdateFloatValue(point_id, var_name, origFloatValue, newFloatValue, cellType))
                {
                    dataGridViewFloats.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                    label4.Text = "Press 'Save' to write changes to the save file.";
                    label4.Visible = true;
                }
                else dataGridViewFloats.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = origFloatValue;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (appSettings.BTSpath != SaveFileViewer.Properties.Settings.Default.BTSpath ||
            appSettings.SavePath != SaveFileViewer.Properties.Settings.Default.SavePath ||
            appSettings.editModeIntroShown != SaveFileViewer.Properties.Settings.Default.editModeIntroShown ||
            appSettings.rewindNotesShown != SaveFileViewer.Properties.Settings.Default.rewindNotesShown)
            {
                appSettings.BTSpath = SaveFileViewer.Properties.Settings.Default.BTSpath;
                appSettings.SavePath = SaveFileViewer.Properties.Settings.Default.SavePath;
                appSettings.editModeIntroShown = SaveFileViewer.Properties.Settings.Default.editModeIntroShown;
                appSettings.rewindNotesShown = SaveFileViewer.Properties.Settings.Default.rewindNotesShown;
                File.WriteAllText("settings.json", Newtonsoft.Json.JsonConvert.SerializeObject(appSettings, Newtonsoft.Json.Formatting.Indented));
            }
            
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



