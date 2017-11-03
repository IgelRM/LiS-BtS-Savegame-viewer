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

        OpenFileDialog OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        OpenFileDialog OpenFileDialog2 = new System.Windows.Forms.OpenFileDialog();

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

            textBoxRawJson.Text = m_GameSave.Raw.Replace("\n", Environment.NewLine);

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

        private void button2_Click(object sender, EventArgs e)
        {
            //writes savegame variable names to file
            using (StreamWriter file = new StreamWriter("savegamedata_variables.txt"))
                foreach (var entry in m_GameData.Variables.OrderBy((v) => v.Value.name))
                    file.WriteLine("[{0}]", entry.Value.name.ToUpper());
                    //file.WriteLine("[{0} {1}]", entry.Key, entry.Value.name.ToUpper());

            //writes savegame checkpoints to file
            using (StreamWriter file = new StreamWriter("savegamedata_checkpoints.txt"))
                for (int i = m_GameSave.Checkpoints.Count - 1; i >= 0; i--)
                {
                    file.WriteLine("[{0}]", m_GameSave.Checkpoints[i].PointIdentifier);
                }
             
             //writes objectives to file 
            using (StreamWriter file = new StreamWriter("savegamedata_objectives.txt"))
                for (int i = m_GameSave.Checkpoints.Count - 1; i >= 0; i--)
                {
                    file.WriteLine(("[{0}]", m_GameSave.Checkpoints[i].Objective);
                }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = OpenFileDialog1.FileName;
                try
                {
                    //string text = File.ReadAllText(file);
                    textBoxSavePath.Text = file;
                }
                catch (IOException)
                {
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenFileDialog2.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = OpenFileDialog2.FileName;
                try
                {
                    //string text = File.ReadAllText(file);
                    textBoxLisPath.Text = file;
                }
                catch (IOException)
                {
                }
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
