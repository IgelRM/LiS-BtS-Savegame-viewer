using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace savefiledecoder
{
    public partial class SaveSelectionForm : Form
    {
        private readonly string[] _steamIdFolders;

        private readonly string _selectedDataFilePath;

        private GameSave _gameSave;

        private string SelectedSteamId { get; set; }

        private SaveSlot? SelectedSlot { get; set; }

        private readonly Dictionary<SaveSlot, bool> _saveSlotAvailability = new Dictionary<SaveSlot, bool>();

        public string SaveDataFilePath { get; private set; }

        public SaveSelectionForm(GameSave gameSave, string selectedDataFilePath = null)
        {
            _steamIdFolders = PathHelper.GetSteamIdFolders();
            if (_steamIdFolders == null || _steamIdFolders.Length == 0)
            {
                throw new Exception($"Could not find any save folder ({nameof(_steamIdFolders)} is empty).");
            }

            _selectedDataFilePath = selectedDataFilePath;
            _gameSave = gameSave;

            InitializeComponent();
        }

        private void SaveSelectionForm_Load(object sender, EventArgs e)
        {
            if (_selectedDataFilePath != null)
            {
                SelectedSteamId = PathHelper.GetSteamIdFromPath(_selectedDataFilePath);
                SelectedSlot = PathHelper.GetSlotFromPath(_selectedDataFilePath);
            }

            cbSteamIds.Items.Clear();
            foreach (var folder in _steamIdFolders.OrderBy(f => f))
            {
                cbSteamIds.Items.Add(PathHelper.GetSteamIdFromPath(folder));
            }

            if (SelectedSteamId != null && cbSteamIds.Items.Contains(SelectedSteamId))
            {
                cbSteamIds.SelectedItem = SelectedSteamId;
            }
            else
            {
                cbSteamIds.SelectedIndex = 0;
            }
        }

        private void SaveSelectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If slot is not selected and at least one slot is available
            if (SelectedSlot == null && _saveSlotAvailability.Any(o => o.Value))
            {
                MessageBox.Show("Please select a slot!");
                e.Cancel = true;
            }

            if (SelectedSlot != null)
            {
                SaveDataFilePath = PathHelper.GetSaveDataFilePath(SelectedSteamId, SelectedSlot.Value);
            }
        }

        private void cbSteamIds_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedSteamId = cbSteamIds.SelectedItem.ToString();

            lblStatus.Visible = false;
            
            foreach (SaveSlot slot in Enum.GetValues(typeof(SaveSlot)))
            {
                _saveSlotAvailability[slot] =
                    File.Exists(PathHelper.GetSaveDataFilePath(SelectedSteamId, slot));
            }

            rbSlot1.Enabled = _saveSlotAvailability[SaveSlot.First];
            rbSlot2.Enabled = _saveSlotAvailability[SaveSlot.Second];
            rbSlot3.Enabled = _saveSlotAvailability[SaveSlot.Third];

            rbSlot1.Checked = false;
            rbSlot2.Checked = false;
            rbSlot3.Checked = false;

            if (SelectedSlot != null)
            {
                switch (SelectedSlot)
                {
                    case SaveSlot.First:
                        rbSlot1.Checked = true;
                        break;
                    case SaveSlot.Second:
                        rbSlot2.Checked = true;
                        break;
                    default:
                        rbSlot3.Checked = true;
                        break;
                }
            }
            else
            {
                if (_saveSlotAvailability[SaveSlot.First])
                {
                    rbSlot1.Checked = true;
                }
                else if (_saveSlotAvailability[SaveSlot.Second])
                {
                    rbSlot2.Checked = true;
                }
                else if (_saveSlotAvailability[SaveSlot.Third])
                {
                    rbSlot3.Checked = true;
                }
            }
        }

        private void rbSlot_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSlot1.Checked)
            {
                SelectedSlot = SaveSlot.First;
                InterpretHeader();
            }
            else if (rbSlot2.Checked)
            {
                SelectedSlot = SaveSlot.Second;
                InterpretHeader();
            }
            else if (rbSlot3.Checked)
            {
                SelectedSlot = SaveSlot.Third;
                InterpretHeader();
            }
        }

        private void InterpretHeader()
        {
            // Should not be triggered in any case but still need to check
            if (SelectedSlot == null)
            {
                lblStatus.Text = "";
                return;
            }

            var headerPath = PathHelper.GetSaveHeaderFilePath(cbSteamIds.SelectedItem.ToString(), SelectedSlot.Value);
            if (_gameSave == null || !File.Exists(headerPath))
            {
                lblStatus.Text = "";
                return;
            }

            _gameSave.ReadHeaderFromFile(headerPath);
            var activeEpisode = 0;
            var text = new StringBuilder();
            lblStatus.ForeColor = SystemColors.ControlText;
            for (var i = 0; i < _gameSave.EpisodeStates.Count; i++)
            {
                if (_gameSave.EpisodeStates[i] == Consts.EpisodeStates.InProgress ||
                    _gameSave.EpisodeStates[i] == Consts.EpisodeStates.Finished)
                {
                    activeEpisode = i;
                }
                else
                {
                    break;
                }
            }

            if (_gameSave.Header.currentEpisode == Consts.GlobalCodes.GlobalCodeReadyToStartEpisode)
            {
                text.Append("Ready to start Episode " + (activeEpisode + 2));
            }
            else if (_gameSave.Header.currentEpisode == Consts.GlobalCodes.GlobalCodeStoryComplete)
            {
                text.Append("Story Complete");
                lblStatus.ForeColor = Color.Green;
            }
            else if (_gameSave.Header.currentEpisode == Consts.GlobalCodes.GlobalCodeSaveJustStarted)
            {
                text.Append("Just Started");
                lblStatus.ForeColor = Color.Red;
            }
            else
            {
                text.Append(_gameSave.EpisodeNames[activeEpisode]);
            }
            text.Append(Environment.NewLine);

            if (_gameSave.Header.currentScene != Consts.GlobalCodes.GlobalCodeReadyToStartEpisode &&
                _gameSave.Header.currentScene != Consts.GlobalCodes.GlobalCodeStoryComplete)
            {
                text.Append(_gameSave.PointNames[_gameSave.Header.currentScene.Value.ToUpper()]);
            }
            text.Append(Environment.NewLine);
            text.Append($"{_gameSave.SaveDate[0]}/{_gameSave.SaveDate[1]}/{_gameSave.SaveDate[2]}");

            lblStatus.Text = text.ToString();
            lblStatus.Visible = true;
        }
    }
}
