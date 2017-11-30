using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace savefiledecoder
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; //https://stackoverflow.com/questions/5169131/c-making-a-form-non-resizable
        }

        public List<string> SteamIDFolders = new List<string>();
        public int savenumber=0;
        public string steamid;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        public void updateComboBox1()
        {
            comboBox1.Items.Clear();
            for (int i=0; i<SteamIDFolders.Count; i++)
            {
                comboBox1.Items.Add(SteamIDFolders[i].Remove(0, SteamIDFolders[i].LastIndexOf('\\')+1));
            }
            comboBox1.SelectedIndex = 0;
            
        }

        private void browseForm_FormClosing (object sender, FormClosingEventArgs e)
        {
            savenumber=0;
            if (radioButton2.Checked) savenumber = 1;
            else if (radioButton3.Checked) savenumber = 2;
            Form1.selectedSavePath = SteamIDFolders[comboBox1.SelectedIndex].ToString() + @"\SLOT_0" + savenumber.ToString() + @"\Data.Save";
        }

        private void UpdateStatus()
        {
            bool[] status = new bool[3];

            for (int i = 0; i < 3; i++)
            {
                status[i] = File.Exists(SteamIDFolders[comboBox1.SelectedIndex].ToString() + @"\SLOT_0" + i.ToString() + @"\Data.Save");
            }
            radioButton1.Enabled = status[0];
            radioButton2.Enabled = status[1];
            radioButton3.Enabled = status[2];
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (savenumber == 0 && radioButton1.Enabled) radioButton1.Checked = true;
            else if (savenumber == 1) radioButton2.Checked = true;
            else  if (radioButton3.Enabled) radioButton3.Checked = true;
            comboBox1.SelectedItem = steamid;
        }
    }
}
