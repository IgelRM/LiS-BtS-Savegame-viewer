using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SaveGameEditor
{
    public partial class ExtrasForm : Form
    {
        private SettingManager _settingManager;

        public ExtrasForm(SettingManager settingManager)
        {
            InitializeComponent();

            _settingManager = settingManager;
        }
        public string savePath, headerPath;

        public GameSave m_GameSave;
        public AssFile m_assFile;

        private void buttonManualBkpHeader_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Backup the header file";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.Copy(headerPath, saveFileDialog1.FileName);
                MessageBox.Show("The backup was successful!", "Savegame Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            saveFileDialog1.FileName = String.Empty;
        }

        private void buttonLoadHeader_Click(object sender, EventArgs e)
        {
            comboBoxHeaderEp.Enabled = true;
            comboBoxPoint.Enabled = true;
            dateTimePicker1.Enabled = true;
            comboBoxHeaderEp.Items.Clear();
            comboBoxPoint.Items.Clear();
            
            if (_settingManager.Settings.RewindNotesShown)
            {
                MessageBox.Show("A few notes on the Checkpoint Rewind feature:\n\n1. It has not been extensively tested and may cause unintended consequences. Users are advised to make backups of their data before proceeding further.\n2. If you are on a mid-level checkpoint, then selecting the last item in the checkpoint list will still cause the game to start from the beginning of that point.\n3. If you want to change only the date of the save, don't touch the checkpoint list at all. To reset the state of the list, click \"Load\" again.", "Savegame Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _settingManager.Settings.RewindNotesShown = true;
            }
            m_GameSave.ReadSaveFromFile(savePath);

            if (!m_GameSave.SaveIsEmpty) //handles the "Just Started" state.
            {
                for (int i=0; i<m_GameSave.EpisodeStates.Count; i++)
                {
                    if (m_GameSave.EpisodeStates[i] == Consts.EpisodeStates.InProgress || 
                        m_GameSave.EpisodeStates[i] == Consts.EpisodeStates.Finished)
                    {
                        comboBoxHeaderEp.Items.Add(Consts.EpisodeNames[(Episode) i]);
                    }
                }
                comboBoxHeaderEp.SelectedIndex = comboBoxHeaderEp.Items.Count - 1; //autoselect the last item
                autoChange = true;
                dateTimePicker1.Value = new DateTime(m_GameSave.SaveDate[2], m_GameSave.SaveDate[1], m_GameSave.SaveDate[0]);
                autoChange = false;
                dateSelected = false; pointSelected = false;
                if (m_GameSave.IsAtMidLevel)
                {
                    labelPointType.Text = "Type: Mid-level checkpoint";
                    labelPointType.ForeColor = Color.Red;
                }
                else
                {
                    labelPointType.Text = "Type: Normal checkpoint";
                    labelPointType.ForeColor = Color.Green;
                }
                labelPointType.Visible = true;
            }
        }

        private void buttonManualBkpSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Backup the save file";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.Copy(savePath, saveFileDialog1.FileName, true);
                MessageBox.Show("The backup was successful!", "Savegame Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            saveFileDialog1.FileName = String.Empty;
        }

        private void comboBoxHeaderEp_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePointList();
        }

        bool autoChange;
        bool dateSelected = false, pointSelected = false;
        private void comboBoxPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!autoChange)
            {
                pointSelected = true;
                buttonRewindCheckpoint.Enabled = true;
                panel1.BackColor = Color.Green;

            }
            else
            {
                pointSelected = false;
                buttonRewindCheckpoint.Enabled = false;
                panel1.BackColor = SystemColors.Control;
            }
        }

        private void dateTimePicker1_ValueChanged (object sender, EventArgs e)
        {
            if (!autoChange)
            {
                dateSelected = true;
                buttonRewindCheckpoint.Enabled = true;
            }
            else buttonRewindCheckpoint.Enabled = false;
        }

        private void FormExtras_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.BackColor = SystemColors.InfoText;
            toolTip.IsBalloon = true;
            toolTip.SetToolTip(buttonLoadHeader, "Load the checkpoint data from selected save");
            toolTip.SetToolTip(buttonRewindCheckpoint, "Write changes to the save files");

            if (m_GameSave.Header == null)
            {
                buttonManualBkpHeader.Enabled = false;
                buttonLoadHeader.Enabled = false;
                labelHeaderNotFound.Visible = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Press Load to display the list of available checkpoints. Select the one that you would like to restart from and press Save. You can optionally change the \"save date\" that is displayed when switching between save slots.\n\nIf you are on a mid-level checkpoint, then selecting the last item in the checkpoint list will still cause the game to start from the beginning of that point.\n\nIf you want to change only the date of the save, don't touch the checkpoint list at all. To reset the state of the list, click \"Load\" again.", "Help", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void buttonRewindCheckpoint_Click(object sender, EventArgs e)
        {
            DialogResult answer = DialogResult.No;
            if (pointSelected)
            {
                answer = MessageBox.Show("Warning! ALL your progress after the checkpoint specified\nbelow will be lost! The target checkpoint is:\n\n" + comboBoxHeaderEp.SelectedItem.ToString() + "\n" + comboBoxPoint.SelectedItem.ToString() + "\n" + dateTimePicker1.Value.ToShortDateString() + "\n\nAre you sure you want to continue?", "Savegame Viewer", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            }
            else
            {
                answer = MessageBox.Show("You have chosen the following date:\n\n" + dateTimePicker1.Value.ToLongDateString() + "\n\nDo you want to proceed with the changes?", "Savegame Viewer", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            }
            
            if (answer == DialogResult.Yes)
            {
                int dest_epNumber = comboBoxHeaderEp.SelectedIndex;
                int pointOffset = 0;
                switch (dest_epNumber)
                {
                    case 0: pointOffset = 0; break;
                    case 1: pointOffset = 14; break;
                    case 2: pointOffset = 27; break;
                    case 3: break;
                }
                dynamic dest_point = m_GameSave.Data.checkpoints[comboBoxPoint.SelectedIndex + pointOffset]; //the point that we ARE reverting to
                var variablePrefix = "";
                m_GameSave.PointVariablePrefixes.TryGetValue(dest_point.pointIdentifier.Value, out variablePrefix);

                m_GameSave.SaveDate[0] = dateTimePicker1.Value.Day;
                m_GameSave.SaveDate[1] = dateTimePicker1.Value.Month;
                m_GameSave.SaveDate[2] = dateTimePicker1.Value.Year;

                if (dateSelected && pointSelected)
                {
                    m_GameSave.RestartFromCheckpoint(variablePrefix, dest_point, dest_epNumber);
                    m_GameSave.WriteSaveToFile(savePath, m_GameSave.Data);
                    m_GameSave.WriteHeaderToFile(headerPath, m_GameSave.Header);
                }
                else if (!dateSelected && pointSelected)
                {
                    m_GameSave.RestartFromCheckpoint(variablePrefix, dest_point, dest_epNumber);
                    m_GameSave.WriteSaveToFile(savePath, m_GameSave.Data);
                    m_GameSave.WriteHeaderToFile(headerPath, m_GameSave.Header);
                }
                else if (dateSelected && !pointSelected)
                {
                    m_GameSave.Header.saveDate = JArray.FromObject(m_GameSave.SaveDate);
                    m_GameSave.WriteHeaderToFile(headerPath, m_GameSave.Header);
                }
                else
                {
                    MessageBox.Show("You didn't change anything!"); //this should never execute in normal circumstances
                }
                DialogResult dontQuit = MessageBox.Show("The changes have been successfully written to the save files! Do you want to do something else?", "Savegame Viewer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if ( dontQuit == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void UpdatePointList()
        {
            comboBoxPoint.Items.Clear();
            Episode selectedEpisode = 0;
            switch(comboBoxHeaderEp.SelectedIndex)
            {
                case 0:
                    selectedEpisode = Episode.First;
                    break;
                case 1:
                    selectedEpisode = Episode.Second;
                    break;
                case 2:
                    selectedEpisode = Episode.Third;
                    break;
            }
            foreach (var checkpoint in Consts.CheckPointDescriptorCollection.GetCheckPointDescriptors(selectedEpisode))
            {
                comboBoxPoint.Items.Add(checkpoint.Name);
                if (checkpoint.Code == m_GameSave.Data.checkpoints.Last.pointIdentifier.Value)
                {
                    break;
                }
            }
            autoChange = true;
            comboBoxPoint.SelectedIndex = comboBoxPoint.Items.Count - 1;
            autoChange = false;
        }

        private void buttonLoadDLL_Click(object sender, EventArgs e)
        {
            m_assFile.Read();
            switch (m_assFile.CheckIntroSkip())
            {
                case true: checkBoxSkipIntro.Checked = true; checkBoxSkipIntro.Enabled = true; break;
                case false: checkBoxSkipIntro.Checked = false; checkBoxSkipIntro.Enabled = true; break;
                case null: checkBoxSkipIntro.Enabled = false; break;
            }
            switch (m_assFile.CheckCutsceneSkip())
            {
                case true: checkBoxSkipCutscenes.Checked = true; checkBoxSkipCutscenes.Enabled = true; break;
                case false: checkBoxSkipCutscenes.Checked = false; checkBoxSkipCutscenes.Enabled = true; break;
                case null: checkBoxSkipCutscenes.Enabled = false; break;
            }
        }

        private void buttonSaveDLL_Click(object sender, EventArgs e)
        {
            if(m_assFile.Write())
            {
                MessageBox.Show("Saved successfully!", "Savegame Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error while saving DLL!", "Savegame Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckBoxSkipIntro_MouseUp(object sender, MouseEventArgs e)
        {
            m_assFile.ChangeIntroSkip(checkBoxSkipIntro.Checked);
        }

        private void CheckBoxSkipCutscenes_MouseUp(object sender, MouseEventArgs e)
        {
            m_assFile.ChangeCutsceneSkip(checkBoxSkipCutscenes.Checked);
        }

    }
}
