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
        }

        public List<string> SteamIDFolders = new List<string>();
        public int savenumber=0;
        public string steamid;

        public GameSave m_GameSave;

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
            if (radioButton1.Checked) savenumber = 0;
            else if (radioButton2.Checked) savenumber = 1;
            else if (radioButton3.Checked) savenumber = 2;
            else
            {
                MessageBox.Show("Please select a slot!");
                e.Cancel = true;
            }

            Form1.selectedSavePath = SteamIDFolders[comboBox1.SelectedIndex].ToString() + @"\SLOT_0" + savenumber.ToString() + @"\Data.Save";
        }

        private void UpdateStatus()
        {
            labelStatus.Visible = false;

            bool[] status = new bool[3];

            for (int i = 0; i < 3; i++)
            {
                status[i] = File.Exists(SteamIDFolders[comboBox1.SelectedIndex].ToString() + @"\SLOT_0" + i.ToString() + @"\Data.Save");
            }
            radioButton1.Enabled = status[0];
            radioButton2.Enabled = status[1];
            radioButton3.Enabled = status[2];
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = steamid;
            if (savenumber == 0 && radioButton1.Enabled) radioButton1.Checked = true;
            else if (savenumber == 1) radioButton2.Checked = true;
            else  if (radioButton3.Enabled) radioButton3.Checked = true;
        }

        private void InterpretHeader(int number)
        {
            string h_path = SteamIDFolders[comboBox1.SelectedIndex].ToString() + @"\SLOT_0" + number.ToString() + @"\Header.Save";
            if (m_GameSave == null || !File.Exists(h_path))
            {
                labelStatus.Text = "";
                return;
            }

            m_GameSave.ReadHeader(h_path);
            int ep = 0;
            string text = "";
            labelStatus.ForeColor = SystemColors.ControlText;
            for (int i = 0; i < m_GameSave.list_EpStates.Count; i++)
            {
                if (m_GameSave.list_EpStates[i] == "kInProgress" || m_GameSave.list_EpStates[i] == "kFinished")
                {
                    ep = i;
                }
                else break;
            }

            if (m_GameSave.m_Header.currentEpisode == "GLOBAL_CODE_READYTOSTARTEPISODE")
            {
                text += "Ready to start Episode " + (ep+2);
            }
            else if (m_GameSave.m_Header.currentEpisode == "GLOBAL_CODE_STORYCOMPLETE")
            {
                text += "Story Complete";
                labelStatus.ForeColor = Color.Green;
            }
            else if (m_GameSave.m_Header.currentEpisode == "GLOBAL_CODE_SAVEJUSTSTARTED")
            {
                text += "Just Started";
                labelStatus.ForeColor = Color.Red;
            }
            else
            {
                text += m_GameSave.episodeNames[ep];
            }
            text += "\n";

            if  (m_GameSave.m_Header.currentScene != "GLOBAL_CODE_READYTOSTARTEPISODE")
            {
                text += m_GameSave.pointNames[m_GameSave.m_Header.currentScene.Value.ToUpper()];
            }
            text += "\n";
            text += String.Format("{1}/{0}/{2}", m_GameSave.dateofSave[0], m_GameSave.dateofSave[1], m_GameSave.dateofSave[2]);

            labelStatus.Text = text;
            labelStatus.Visible = true;
        }

        private void radioButtons_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
               InterpretHeader(0);
            }
            else if (radioButton2.Checked)
            {
                InterpretHeader(1);
            }
            else if (radioButton3.Checked)
            {
                 InterpretHeader(2);
            }
        }
    }
}
