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


            UpdateEpsiodeBoxes();
            UpdateDataGrid();

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
            DataTable table = BuildDataTable();
            dataGridView1.DataSource = table.DefaultView;
            dataGridView1.Columns["Key"].Frozen = true;
            dataGridView1.Rows[0].Frozen = true;
            dataGridView1.Rows[1].Frozen = true;
            foreach(var column in dataGridView1.Columns)
            {
                        
            }
        }

        private DataTable BuildDataTable()
        {
            DataTable t = new DataTable();
            t.Columns.Add("Key");
            bool first = true;
            for(int i= m_GameSave.Checkpoints.Count-1; i>=0; i--)
            {
                if(first)
                {
                    t.Columns.Add("Global");
                    first = false;
                }
                else
                    t.Columns.Add("Checkpoint "+(i+1).ToString());
            }

            // current point
            object[] row = new object[t.Columns.Count];
            row[0] = "PointIdentifier";
            for (int i = m_GameSave.Checkpoints.Count-1; i >= 0; i--)
            {
                row[m_GameSave.Checkpoints.Count-i] = m_GameSave.Checkpoints[i].PointIdentifier;
            }
            t.Rows.Add(row);
            // current objective
            row[0] = "Objective";
            for (int i = m_GameSave.Checkpoints.Count-1; i >= 0; i--)
            {
                row[m_GameSave.Checkpoints.Count-i] = m_GameSave.Checkpoints[i].Objective;
            }
            t.Rows.Add(row);


            // variables 
            foreach (var varType in m_GameData.Variables.OrderBy( (v)=>v.Value.name) )
            {
                string varName = varType.Value.name.ToUpper();
                if (!checkBoxE1.Checked && varName.StartsWith("E1_"))
                {
                    continue;
                }
                if (!checkBoxE2.Checked && varName.StartsWith("E2_"))
                {
                    continue;
                }
                if (!checkBoxE3.Checked && varName.StartsWith("E3_"))
                {
                    continue;
                }
                if (!checkBoxE4.Checked && varName.StartsWith("E4_"))
                {
                    continue;
                }
                
                row[0] = varType.Value.name;
                for (int i = m_GameSave.Checkpoints.Count-1; i >= 0; i--)
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
            if(successDataPath)
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
            if(successDataPath&&successSavePath)
            {
                button1.Enabled = true;
                button2.Enabled = true;
                SaveFileViewer.Properties.Settings.Default.Save();
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }
        }


        private byte[] ReadKey(string assemblyPath)
        {
            var ass = Assembly.LoadFile(assemblyPath);
            Type t = ass.GetType("T_3EF937CB");
            FieldInfo keyField = t.GetField("_18AFCD9AB", BindingFlags.Static|BindingFlags.NonPublic);
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
                    bool found = checkpoint.Variables.TryGetValue(entry.Value.name, out state);
                    file.WriteLine("\"{0}\", {1}", entry.Value.name.ToUpper(), state.Value);


                }
            System.Diagnostics.Process.Start("variables.txt"); //open the text file

        }
        //export checkpoints
        private void button3_Click(object sender, EventArgs e)
        {
            
        }
        //export variables
        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        //browse for Data.Save
        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if(result == DialogResult.OK)
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
            MessageBox.Show("Tool by /u/DanielWe\nModified by Ladosha2 and IgelRM", "About Savegame Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxSavePath.Text = SaveFileViewer.Properties.Settings.Default.SavePath;
            textBoxLisPath.Text = SaveFileViewer.Properties.Settings.Default.BTSpath;
            folderBrowserDialog1.SelectedPath = SaveFileViewer.Properties.Settings.Default.BTSpath;
        }
    }
}
