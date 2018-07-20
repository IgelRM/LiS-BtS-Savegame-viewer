using System;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using SaveGameEditor.Properties;
using System.Drawing;

namespace SaveGameEditor
{
    public partial class MainForm : Form
    {
        private readonly SettingManager _settingManager = new SettingManager();
        private readonly GameData _initialData = new GameData();
        private GameSave _gameSave;
        private string _saveDataFilePath;

        string point_id = "", var_name = "";
        private List<string> _steamIdFolders = new List<string>();

        public string SaveDataFilePath
        {
            get
            {
                return _saveDataFilePath;
                
            }
            set
            {
                _saveDataFilePath = value;
                textBoxSavePath.Text = SaveDataFilePath;
                _settingManager.Settings.SavePath = textBoxSavePath.Text;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            ValidatePaths();
        }

        bool resizeHelpShown = false, bonusWarningShown = false;
        private void buttonShowContent_Click(object sender, EventArgs e)
        {
            DeckNineXorEncoder.ReadKeyFromFile(PathHelper.GetCSharpAssemblyPath(textBoxLisPath.Text));
            _initialData.ReadFromFile(PathHelper.GetInitialDataFilePath(textBoxLisPath.Text));
            _gameSave = new GameSave(_initialData);
            _gameSave.ReadMainSaveFromFile(textBoxSavePath.Text);
#if DEBUG
            if (Form.ModifierKeys == Keys.Control)
            {
                if (!_gameSave.MainSaveIsEmpty)
                {
                    File.WriteAllText(textBoxSavePath.Text + @".txt", _gameSave.RawMainSave);
                }
                File.WriteAllText(textBoxSavePath.Text + @"-initialdata.txt", _initialData.Raw);
                if (_gameSave.Header != null)
                {
                    File.WriteAllText(textBoxSavePath.Text + @"-header.txt", _gameSave.RawHeader);
                }
                if (!_gameSave.FarewellSaveIsEmpty)
                {
                    File.WriteAllText(textBoxSavePath.Text + @"-bonus.txt", _gameSave.RawFarewellSave);
                }
            }
#endif
            if (!_gameSave.MainSaveIsEmpty || !_gameSave.FarewellSaveIsEmpty) //handles the "Just Started" state.
            {
                rbMain.Enabled = !_gameSave.MainSaveIsEmpty;
                rbBonus.Enabled = !_gameSave.FarewellSaveIsEmpty;

                if (rbMain.Enabled) rbMain.Checked = true;
                else if (rbBonus.Enabled) rbBonus.Checked = true;

                if (findForm != null)
                {
                    findForm.ResetSearchState();
                }
                UpdateEpisodeBoxes();
                UpdateFlagGrid();
                UpdateFloatGrid();
                UpdateItemGrid();
                UpdateDataGrid();
                label4.Visible = false; //hide save file warning
                _settingManager.Settings.GamePath = textBoxLisPath.Text;

                if (!_gameSave.MainSaveIsEmpty || _gameSave.MainSaveHasFarewellData)
                {
                    buttonExport.Enabled = true;
                    buttonExtras.Enabled = true;
                    checkBoxEditMode.Enabled = true;
                    _settingManager.Settings.SavePath = textBoxSavePath.Text;
                }
                else
                {
                    if (!bonusWarningShown)
                    {
                        MessageBox.Show(Resources.BonusSaveOpened, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bonusWarningShown = true;
                    }
                    buttonExport.Enabled = true;
                    buttonExtras.Enabled = true;
                }

                if (!resizeHelpShown)
                {
                    ToolTip tt = new ToolTip();
                    tt.Show("Drag here to resize", this, 150, 145, 2000);
                    resizeHelpShown = true;
                }

                if (!_settingManager.Settings.FindHintShown)
                {
                    MessageBox.Show(Resources.SearchHelpMessage, "Hint", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _settingManager.Settings.FindHintShown = true;
                }
            }
            else
            {
                MessageBox.Show(Resources.CorruptSaveMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        // only enable boxes for episodes the player has already finished or is currently playing
        private void UpdateEpisodeBoxes()
        {
            if (rbMain.Checked)
            {

                // E1
                if (_gameSave.PlayedEpisodes["E1"])
                {
                    checkBoxE1.Enabled = true;
                }
                else
                {
                    checkBoxE1.Enabled = false;
                }
                // E2
                if (_gameSave.PlayedEpisodes["E2"])
                {
                    checkBoxE2.Enabled = true;
                }
                else
                {
                    checkBoxE2.Enabled = false;
                }
                // E3
                if (_gameSave.PlayedEpisodes["E3"])
                {
                    checkBoxE3.Enabled = true;
                }
                else
                {
                    checkBoxE3.Enabled = false;
                }
            }
            else
            {
                checkBoxE1.Enabled = false;
                checkBoxE1.Checked = false;
                checkBoxE2.Enabled = false;
                checkBoxE2.Checked = false;
                checkBoxE3.Enabled = false;
                checkBoxE3.Checked = false;
            }

        }

        int visible_row = 2, visible_column = 1;

        private void UpdateDataGrid()
        {
            if (_gameSave == null)
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
            DataTable table = BuildMainDataTable();
            dataGridView1.DataSource = table.DefaultView;
            dataGridView1.Columns["Key"].Frozen = true;
            dataGridView1.Columns["Key"].ReadOnly = true;
            dataGridView1.Rows[0].Frozen = true;
            dataGridView1.Rows[1].Frozen = true;
            dataGridView1.Rows[0].ReadOnly = true;
            dataGridView1.Rows[1].ReadOnly = true;
            int current = rbBonus.Checked ? 1 : 2;
            dataGridView1.Columns[current].HeaderText = "CurrentCheckpoint";

            dataGridView1.Rows[0].Cells[current].ToolTipText = (rbBonus.Checked && _gameSave.IsFarewellAtMidLevel) || (rbMain.Checked && _gameSave.IsMainAtMidLevel)
                ? "Middle of " + Consts.CheckPointDescriptorCollection.GetCheckPointDescriptorByCode(dataGridView1.Rows[0].Cells[current+1].Value.ToString())?.Name 
                : Consts.CheckPointDescriptorCollection.GetCheckPointDescriptorByCode(dataGridView1.Rows[0].Cells[current].Value.ToString())?.Name;
            for (int i = current+1; i < dataGridView1.Rows[0].Cells.Count; i++)
            {
                dataGridView1.Rows[0].Cells[i].ToolTipText = Consts.CheckPointDescriptorCollection.GetCheckPointDescriptorByCode(dataGridView1.Rows[0].Cells[i].Value.ToString())?.Name;
            }

            if (editModeActive)
            {
                dataGridView1.Columns["Key"].DefaultCellStyle.BackColor = Color.LightGray;
                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
                dataGridView1.Rows[1].DefaultCellStyle.BackColor = Color.LightGray;
            }
            else
            {
                dataGridView1.Columns["Key"].DefaultCellStyle.BackColor = Color.White;
                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.White;
                dataGridView1.Rows[1].DefaultCellStyle.BackColor = Color.White;
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
            if (_gameSave == null)
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
            DataTable table = BuildMainFloatTable();
            dataGridViewFloats.DataSource = table.DefaultView;
            dataGridViewFloats.Columns["Key"].Frozen = true;
            dataGridViewFloats.Columns["Key"].ReadOnly = true;
            dataGridViewFloats.Rows[0].Frozen = true;
            dataGridViewFloats.Rows[0].ReadOnly = true;
            int current = rbBonus.Checked ? 1 : 2;
            dataGridViewFloats.Columns[current].HeaderText = "CurrentCheckpoint";


            dataGridViewFloats.Rows[0].Cells[current].ToolTipText = (rbBonus.Checked && _gameSave.IsFarewellAtMidLevel) || (rbMain.Checked && _gameSave.IsMainAtMidLevel)
                ? "Middle of " + Consts.CheckPointDescriptorCollection.GetCheckPointDescriptorByCode(dataGridViewFloats.Rows[0].Cells[current + 1].Value.ToString())?.Name
                : Consts.CheckPointDescriptorCollection.GetCheckPointDescriptorByCode(dataGridViewFloats.Rows[0].Cells[current].Value.ToString())?.Name;
            for (int i = current+1; i < dataGridViewFloats.Rows[0].Cells.Count; i++)
            {
                dataGridViewFloats.Rows[0].Cells[i].ToolTipText = Consts.CheckPointDescriptorCollection.GetCheckPointDescriptorByCode(dataGridViewFloats.Rows[0].Cells[i].Value.ToString())?.Name;
            }

            if (editModeActive)
            {
                dataGridViewFloats.Columns["Key"].DefaultCellStyle.BackColor = Color.LightGray;
                dataGridViewFloats.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
            }
            else
            {
                dataGridViewFloats.Columns["Key"].DefaultCellStyle.BackColor = Color.White;
                dataGridViewFloats.Rows[0].DefaultCellStyle.BackColor = Color.White;
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
            if (_gameSave == null)
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
                visible_row = 0;
            }
            else
            {
                visible_row = dataGridViewFlags.FirstDisplayedScrollingRowIndex;
            }
            if (dataGridViewFlags.FirstDisplayedScrollingColumnIndex <= 1)
            {
                visible_column = 1;
            }
            else if (dataGridViewFlags.FirstDisplayedScrollingColumnHiddenWidth > 60)
            {
                visible_column = dataGridViewFlags.FirstDisplayedScrollingColumnIndex + 1;
            }
            else
            {
                visible_column = dataGridViewFlags.FirstDisplayedScrollingColumnIndex;
            }
            dataGridViewFlags.Columns.Clear();
            DataTable table = BuildMainFlagTable();
            dataGridViewFlags.DataSource = table.DefaultView;
            dataGridViewFlags.Columns["Key"].Frozen = true;
            dataGridViewFlags.Columns["Key"].ReadOnly = true;
            dataGridViewFlags.Rows[0].Frozen = true;
            dataGridViewFlags.Rows[0].ReadOnly = true;
            int current = rbBonus.Checked ? 1 : 2;
            dataGridViewFlags.Columns[current].HeaderText = "CurrentCheckpoint";

            dataGridViewFlags.Rows[0].Cells[current].ToolTipText = (rbBonus.Checked && _gameSave.IsFarewellAtMidLevel) || (rbMain.Checked && _gameSave.IsMainAtMidLevel)
                ? "Middle of " + Consts.CheckPointDescriptorCollection.GetCheckPointDescriptorByCode(dataGridViewFlags.Rows[0].Cells[current + 1].Value.ToString())?.Name
                : Consts.CheckPointDescriptorCollection.GetCheckPointDescriptorByCode(dataGridViewFlags.Rows[0].Cells[current].Value.ToString())?.Name;
            for (int i = current+1; i < dataGridViewFlags.Rows[0].Cells.Count; i++)
            {
                dataGridViewFlags.Rows[0].Cells[i].ToolTipText = Consts.CheckPointDescriptorCollection.GetCheckPointDescriptorByCode(dataGridViewFlags.Rows[0].Cells[i].Value.ToString())?.Name;
            }

            if (editModeActive)
            {
                dataGridViewFlags.Columns["Key"].DefaultCellStyle.BackColor = Color.LightGray;
                dataGridViewFlags.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
            }
            else
            {
                dataGridViewFlags.Columns["Key"].DefaultCellStyle.BackColor = Color.White;
                dataGridViewFlags.Rows[0].DefaultCellStyle.BackColor = Color.White;
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
                dataGridViewFlags.FirstDisplayedScrollingRowIndex = visible_row;
                dataGridViewFlags.FirstDisplayedScrollingColumnIndex = visible_column;
            }
            catch
            {

            }
            dataGridViewFlags.Columns[0].Width = keyColWidth;
        }

        private void UpdateItemGrid()
        {
            if (_gameSave == null)
                return;

            int keyColWidth = 100;
            try
            {
                keyColWidth = dataGridViewItems.Columns[0].Width;
            }
            catch
            {

            }

            if (dataGridViewItems.FirstDisplayedScrollingRowIndex <= 0)
            {
                visible_row = 0;
            }
            else
            {
                visible_row = dataGridViewItems.FirstDisplayedScrollingRowIndex;
            }
            if (dataGridViewItems.FirstDisplayedScrollingColumnIndex <= 1)
            {
                visible_column = 1;
            }
            else if (dataGridViewItems.FirstDisplayedScrollingColumnHiddenWidth > 60)
            {
                visible_column = dataGridViewItems.FirstDisplayedScrollingColumnIndex + 1;
            }
            else
            {
                visible_column = dataGridViewItems.FirstDisplayedScrollingColumnIndex;
            }
            dataGridViewItems.Columns.Clear();
            DataTable table = BuildMainItemTable();
            dataGridViewItems.DataSource = table.DefaultView;
            dataGridViewItems.Columns["Key"].Frozen = true;
            dataGridViewItems.Columns["Key"].ReadOnly = true;
            dataGridViewItems.Rows[0].Frozen = true;
            dataGridViewItems.Rows[0].ReadOnly = true;
            dataGridViewItems.Columns[1].HeaderText = "CurrentCheckpoint";

            dataGridViewItems.Rows[0].Cells[1].ToolTipText = (rbBonus.Checked && _gameSave.IsFarewellAtMidLevel) || (rbMain.Checked && _gameSave.IsMainAtMidLevel)
                ? "Middle of " + Consts.CheckPointDescriptorCollection.GetCheckPointDescriptorByCode(dataGridViewItems.Rows[0].Cells[2].Value.ToString())?.Name
                : Consts.CheckPointDescriptorCollection.GetCheckPointDescriptorByCode(dataGridViewItems.Rows[0].Cells[1].Value.ToString())?.Name;
            for (int i = 2; i < dataGridViewItems.Rows[0].Cells.Count; i++)
            {
                dataGridViewItems.Rows[0].Cells[i].ToolTipText = Consts.CheckPointDescriptorCollection.GetCheckPointDescriptorByCode(dataGridViewItems.Rows[0].Cells[i].Value.ToString())?.Name;
            }

            if (editModeActive)
            {
                dataGridViewItems.Columns["Key"].DefaultCellStyle.BackColor = Color.LightGray;
                dataGridViewItems.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
            }
            else
            {
                dataGridViewItems.Columns["Key"].DefaultCellStyle.BackColor = Color.White;
                dataGridViewItems.Rows[0].DefaultCellStyle.BackColor = Color.White;
            }

            for (int i = 1; i < dataGridViewItems.RowCount; i++)
            {
                for (int j = 1; j < dataGridViewItems.ColumnCount; j++)
                {
                    DataGridViewCheckBoxCell CheckBoxCell = new DataGridViewCheckBoxCell();
                    CheckBoxCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewItems.Rows[i].Cells[j] = CheckBoxCell;
                }
            }

            for (int i = 0; i < dataGridViewItems.ColumnCount; i++)
            {
                dataGridViewItems.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            try
            {
                dataGridViewItems.FirstDisplayedScrollingRowIndex = visible_row;
                dataGridViewItems.FirstDisplayedScrollingColumnIndex = visible_column;
            }
            catch
            {

            }
            dataGridViewItems.Columns[0].Width = keyColWidth;
        }

        #region Main table-building functions
        private DataTable BuildMainDataTable()
        {
            if (rbBonus.Checked)
            {
                return BuildFarewellDataTable();
            }

            DataTable t = new DataTable();
            t.Columns.Add("Key");
            bool first = true;
            for (int i = _gameSave.MainCheckpoints.Count - 1; i >= 0; i--)
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
            for (int i = _gameSave.MainCheckpoints.Count - 1; i >= 0; i--)
            {
                row[_gameSave.MainCheckpoints.Count - i] = _gameSave.MainCheckpoints[i].PointIdentifier;
            }
            t.Rows.Add(row);
            // current objective
            row[0] = "Objective";
            for (int i = _gameSave.MainCheckpoints.Count - 1; i >= 0; i--)
            {
                row[_gameSave.MainCheckpoints.Count - i] = _gameSave.MainCheckpoints[i].Objective;
            }
            t.Rows.Add(row);

            // variables 
            foreach (var varType in _initialData.GetVariables().OrderBy((v) => v.Value.Name))
            {
                string varName = varType.Value.Name.ToUpper();
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
                if (varName.StartsWith("E4_"))
                {
                    continue;
                }

                row[0] = varType.Value.Name;
                for (int i = _gameSave.MainCheckpoints.Count - 1; i >= 0; i--)
                {
                    var checkpoint = _gameSave.MainCheckpoints[i];
                    VariableState state;
                    bool found = checkpoint.Variables.TryGetValue(varType.Value.Name, out state);
                    if (found)
                    {
                        row[_gameSave.MainCheckpoints.Count - i] = state.Value;
                    }
                    else
                    {
                        row[_gameSave.MainCheckpoints.Count - i] = null;
                    }
                }
                t.Rows.Add(row);
            }

            return t;
        }

        private DataTable BuildMainFloatTable()
        {
            if (rbBonus.Checked)
            {
                return BuildFarewellFloatTable();
            }

            DataTable t = new DataTable();
            t.Columns.Add("Key");
            bool first = true;
            for (int i = _gameSave.MainCheckpoints.Count - 1; i >= 0; i--)
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
            for (int i = _gameSave.MainCheckpoints.Count - 1; i >= 0; i--)
            {
                row[_gameSave.MainCheckpoints.Count - i] = _gameSave.MainCheckpoints[i].PointIdentifier;
            }
            t.Rows.Add(row);

            // floats
            foreach (var flt in Resources.FloatList_main.Trim().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                row[0] = flt;
                for (int i = _gameSave.MainCheckpoints.Count - 1; i >= 0; i--)
                {
                    var checkpoint = _gameSave.MainCheckpoints[i];
                    FloatState state;
                    bool found = checkpoint.Floats.TryGetValue(flt, out state);
                    if (found)
                    {
                        row[_gameSave.MainCheckpoints.Count - i] = state.Value;
                    }
                    else
                    {
                        row[_gameSave.MainCheckpoints.Count - i] = null;
                    }
                }
                t.Rows.Add(row);
            }

            return t;
        }

        private DataTable BuildMainFlagTable()
        {
            if (rbBonus.Checked)
            {
                return BuildFarewellFlagTable();
            }
            
            DataTable t = new DataTable();
            t.Columns.Add("Key");

            bool first = true;
            for (int i = _gameSave.MainCheckpoints.Count - 1; i >= 0; i--)
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
            for (int i = _gameSave.MainCheckpoints.Count - 1; i >= 0; i--)
            {
                row[_gameSave.MainCheckpoints.Count - i] = _gameSave.MainCheckpoints[i].PointIdentifier;
            }
            t.Rows.Add(row);

            // flags
            foreach (var flag in Resources.FlagList_main.Trim().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                row[0] = flag;
                for (int i = _gameSave.MainCheckpoints.Count - 1; i >=0; i--)
                {
                    int rownum = _gameSave.MainCheckpoints.Count - i;
                    row[rownum] = _gameSave.MainCheckpoints[i].Flags.Contains(flag);
                }
                t.Rows.Add(row);
            }

            return t;
        }

        private DataTable BuildMainItemTable()
        {
            if (rbBonus.Checked)
            {
                return BuildFarewellItemTable();
            }

            DataTable t = new DataTable();
            t.Columns.Add("Key");

            for (int i = _gameSave.MainCheckpoints.Count - 2; i >= 0; i--)
            {
                t.Columns.Add("Checkpoint " + (i + 1).ToString());
            }

            // current point
            object[] row = new object[t.Columns.Count];
            row[0] = "PointIdentifier";
            for (int i = _gameSave.MainCheckpoints.Count - 2; i >= 0; i--)
            {
                row[_gameSave.MainCheckpoints.Count - i-1] = _gameSave.MainCheckpoints[i].PointIdentifier;
            }
            t.Rows.Add(row);

            // items
            foreach (var kvp in _initialData.GetItems().OrderBy((v) => v.Value.Name))
            {
                string itemName = kvp.Value.Name.ToUpper();
                if (itemName.StartsWith("E4_"))
                {
                    continue;
                }

                row[0] = kvp.Value.Name;
                for (int i = _gameSave.MainCheckpoints.Count - 2; i >= 0; i--)
                {
                    var checkpoint = _gameSave.MainCheckpoints[i];
                    ItemState state;
                    bool found = checkpoint.Items.TryGetValue(kvp.Value.Name, out state);
                    if (found)
                    {
                        row[_gameSave.MainCheckpoints.Count - i - 1] = (state.Owner == Consts.Uids.Chloe);
                    }
                    else
                    {
                        row[_gameSave.MainCheckpoints.Count - i - 1] = false;
                    }
                    
                }
                t.Rows.Add(row);
            }

            return t;
        }

        #endregion

        #region Farewell table-building functions

        private DataTable BuildFarewellDataTable()
        {
            DataTable t = new DataTable();
            t.Columns.Add("Key");

            for (int i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
            {
                t.Columns.Add("Checkpoint " + (i + 1).ToString());
            }

            // current point
            object[] row = new object[t.Columns.Count];
            row[0] = "PointIdentifier";
            for (int i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
            {
                row[_gameSave.FarewellCheckpoints.Count - i] = _gameSave.FarewellCheckpoints[i].PointIdentifier;
            }
            t.Rows.Add(row);
            // current objective
            row[0] = "Objective";
            for (int i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
            {
                row[_gameSave.FarewellCheckpoints.Count - i] = _gameSave.FarewellCheckpoints[i].Objective;
            }
            t.Rows.Add(row);

            // variables 
            foreach (var varType in _initialData.GetVariables().OrderBy((v) => v.Value.Name))
            {
                string varName = varType.Value.Name.ToUpper();
                if (!varName.StartsWith("E4_") && varName != "HINT_PROGRESS")
                {
                    continue;
                }

                row[0] = varType.Value.Name;
                for (int i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
                {
                    var checkpoint = _gameSave.FarewellCheckpoints[i];
                    VariableState state;
                    bool found = checkpoint.Variables.TryGetValue(varType.Value.Name, out state);
                    if (found)
                    {
                        row[_gameSave.FarewellCheckpoints.Count - i] = state.Value;
                    }
                    else
                    {
                        row[_gameSave.FarewellCheckpoints.Count - i] = null;
                    }
                }
                t.Rows.Add(row);
            }

            return t;
        }

        private DataTable BuildFarewellFlagTable()
        {
            DataTable t = new DataTable();
            t.Columns.Add("Key");

            for (int i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
            {
                t.Columns.Add("Checkpoint " + (i + 1).ToString());
            }

            // current point
            object[] row = new object[t.Columns.Count];
            row[0] = "PointIdentifier";
            for (int i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
            {
                row[_gameSave.FarewellCheckpoints.Count - i] = _gameSave.FarewellCheckpoints[i].PointIdentifier;
            }
            t.Rows.Add(row);

            // flags
            foreach (var flag in Resources.FlagList_farewell.Trim().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                row[0] = flag;
                for (int i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
                {
                    int rownum = _gameSave.FarewellCheckpoints.Count - i;
                    row[rownum] = _gameSave.FarewellCheckpoints[i].Flags.Contains(flag);
                }
                t.Rows.Add(row);
            }

            return t;
        }

        private DataTable BuildFarewellFloatTable()
        {
            DataTable t = new DataTable();
            t.Columns.Add("Key");
            for (int i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
            {
                t.Columns.Add("Checkpoint " + (i + 1).ToString());
            }

            // current point
            object[] row = new object[t.Columns.Count];
            row[0] = "PointIdentifier";
            for (int i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
            {
                row[_gameSave.FarewellCheckpoints.Count - i] = _gameSave.FarewellCheckpoints[i].PointIdentifier;
            }
            t.Rows.Add(row);

            // floats
            foreach (var flt in Resources.FloatList_farewell.Trim().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                row[0] = flt;
                for (int i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
                {
                    var checkpoint = _gameSave.FarewellCheckpoints[i];
                    FloatState state;
                    bool found = checkpoint.Floats.TryGetValue(flt, out state);
                    if (found)
                    {
                        row[_gameSave.FarewellCheckpoints.Count - i] = state.Value;
                    }
                    else
                    {
                        row[_gameSave.FarewellCheckpoints.Count - i] = null;
                    }
                }
                t.Rows.Add(row);
            }

            return t;
        }

        private DataTable BuildFarewellItemTable()
        {
            DataTable t = new DataTable();
            t.Columns.Add("Key");

            for (int i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
            {
                t.Columns.Add("Checkpoint " + (i + 1).ToString());
            }

            // current point
            object[] row = new object[t.Columns.Count];
            row[0] = "PointIdentifier";
            for (int i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
            {
                row[_gameSave.FarewellCheckpoints.Count - i] = _gameSave.FarewellCheckpoints[i].PointIdentifier;
            }
            t.Rows.Add(row);

            // items
            foreach (var kvp in _initialData.GetItems().OrderBy((v) => v.Value.Name))
            {
                string itemName = kvp.Value.Name.ToUpper();
                if (!itemName.StartsWith("E4_"))
                {
                    continue;
                }

                row[0] = kvp.Value.Name;
                for (int i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
                {
                    var checkpoint = _gameSave.FarewellCheckpoints[i];
                    ItemState state;
                    bool found = checkpoint.Items.TryGetValue(kvp.Value.Name, out state);
                    if (found)
                    {
                        row[_gameSave.FarewellCheckpoints.Count - i] = (state.Owner == Consts.Uids.Maxine);
                    }
                    else
                    {
                        row[_gameSave.FarewellCheckpoints.Count - i] = false;
                    }

                }
                t.Rows.Add(row);
            }

            return t;
        }

        #endregion

        private void ValidatePaths()
        {
            bool successDataPath = false;
            try
            {
                successDataPath = File.Exists(PathHelper.GetInitialDataFilePath(textBoxLisPath.Text));
            }
            catch
            {

            }
            if (successDataPath)
            {
                textBoxLisPath.BackColor = SystemColors.Window;
            }
            else
            {
                textBoxLisPath.BackColor = Color.Red;
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
                textBoxSavePath.BackColor = SystemColors.Window;
            }
            else
            {
                textBoxSavePath.BackColor = Color.Red;
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
            var ass = Assembly.Load(File.ReadAllBytes(assemblyPath));
            Type t = ass.GetType("T_3EF937CB");
            FieldInfo keyField = t.GetField("_18AFCD9AB", BindingFlags.Static | BindingFlags.NonPublic);
            return (byte[])keyField.GetValue(null);
        }

        private void checkBoxEpisodes_CheckedChanged(object sender, EventArgs e)
        {
            if (!editModeActive)
            {
                if (findForm != null)
                {
                    findForm.ResetSearchState();
                }
                UpdateDataGrid();
            }
        }

        private void EpisodeRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (findForm != null)
            {
                findForm.ResetSearchState();
            }
            UpdateEpisodeBoxes();
            UpdateDataGrid();
            UpdateFlagGrid();
            UpdateFloatGrid();
            UpdateItemGrid();
        }

        private void textBoxSavePath_TextChanged(object sender, EventArgs e)
        {
            ValidatePaths();
        }

        private void textBoxLisPath_TextChanged(object sender, EventArgs e)
        {
            ValidatePaths();
        }

        // Export
        private void buttonExport_Click(object sender, EventArgs e)
        {
            using (var file = new StreamWriter(PathHelper.ExportObjectivesFileName))
            {
                if (!_gameSave.MainSaveIsEmpty)
                {
                    for (var i = _gameSave.MainCheckpoints.Count - 1; i >= 0; i--)
                    {
                        file.WriteLine("\"{0}\"", _gameSave.MainCheckpoints[i].Objective);
                    }
                }
                if (!_gameSave.FarewellSaveIsEmpty)
                {
                    for (var i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
                    {
                        file.WriteLine("\"{0}\"", _gameSave.FarewellCheckpoints[i].Objective);
                    }
                }
            }

            using (var file = new StreamWriter(PathHelper.ExportCheckpointsFileName))
            {
                if (!_gameSave.MainSaveIsEmpty)
                {
                    for (var i = _gameSave.MainCheckpoints.Count - 1; i >= 0; i--)
                    {
                        file.WriteLine("\"{0}\"", _gameSave.MainCheckpoints[i].PointIdentifier);
                    }
                }
                if (!_gameSave.FarewellSaveIsEmpty)
                {
                    for (var i = _gameSave.FarewellCheckpoints.Count - 1; i >= 0; i--)
                    {
                        file.WriteLine("\"{0}\"", _gameSave.FarewellCheckpoints[i].PointIdentifier);
                    }
                }
            }

            using (var file = new StreamWriter(PathHelper.ExportVariablesFileName))
            {
                var mainPoint = _gameSave.MainSaveIsEmpty ? null : _gameSave.MainCheckpoints[_gameSave.MainCheckpoints.Count-2];
                var  farewellPoint = _gameSave.FarewellSaveIsEmpty ? null : _gameSave.FarewellCheckpoints.Last();
                foreach (var entry in _initialData.GetVariables().OrderBy(v => v.Value.Name))
                {
                    VariableState state = null;
                    if ((mainPoint != null && mainPoint.Variables.TryGetValue(entry.Value.Name, out state)) ||
                        (farewellPoint != null && farewellPoint.Variables.TryGetValue(entry.Value.Name, out state)))
                    {
                        file.WriteLine("\"{0}\", {1}", entry.Value.Name.ToUpper(), state.Value);
                    }
                    else if (ModifierKeys == Keys.Control)
                    {
                        file.WriteLine("\"{0}\"", entry.Value.Name.ToUpper());
                    }
                }
            }

            using (var file = new StreamWriter(PathHelper.ExportFlagsFileName))
            {
                if (!_gameSave.MainSaveIsEmpty)
                {
                    foreach (var flag in _gameSave.MainCheckpoints[_gameSave.MainCheckpoints.Count - 2].Flags)
                    {
                        file.WriteLine("\"{0}\"", flag);
                    }
                }
                if (!_gameSave.FarewellSaveIsEmpty)
                {
                    foreach (var flag in _gameSave.FarewellData.checkpoints.Last.flags)
                    {
                        file.WriteLine("\"{0}\"", flag.Value);
                    }
                }
            }

            using (var file = new StreamWriter(PathHelper.ExportFloatsFileName))
            {
                if (!_gameSave.MainSaveIsEmpty)
                {
                    foreach (var floatValue in _gameSave.MainCheckpoints[_gameSave.MainCheckpoints.Count - 2].Floats)
                    {
                        file.WriteLine("\"{0}\", {1}", floatValue.Key, floatValue.Value.Value);
                    }
                }
                if (!_gameSave.FarewellSaveIsEmpty)
                {
                    foreach (var floatValue in _gameSave.FarewellData.checkpoints.Last.floatValuesDict)
                    {
                        if (floatValue.Name == "$type")
                        {
                            continue;
                        }

                        file.WriteLine("\"{0}\", {1}", floatValue.Name, floatValue.Value);
                    }
                }
            }

            MessageBox.Show("The following files were created in application folder:" +
                            Environment.NewLine + 
                            Environment.NewLine +
                            "* " + PathHelper.ExportObjectivesFileName +
                            Environment.NewLine +
                            "* " + PathHelper.ExportCheckpointsFileName +
                            Environment.NewLine +
                            "* " + PathHelper.ExportVariablesFileName +
                            Environment.NewLine +
                            "* " + PathHelper.ExportFlagsFileName +
                            Environment.NewLine +
                            "* " + PathHelper.ExportFloatsFileName,
                "Export completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            System.Diagnostics.Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
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
                    _settingManager.Settings.SavePath = openFileDialog1.FileName;
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
                    _settingManager.Settings.GamePath = folderBrowserDialog1.SelectedPath;
                    textBoxLisPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            using (var aboutForm = new AboutForm())
            {
                aboutForm.ShowDialog();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            #region Update checking at startup
            if (_settingManager.Settings.CheckForUpdatesAtStartup)
            {
                Task.Run(async () =>
                {
                    var result = await UpdateChecker.CheckForUpdates();
                    if (result == null || !result.CanBeUpdated)
                    {
                        return;
                    }

                    this.InvokeEx(() =>
                    {
                        using (var updateForm = new UpdateForm(result))
                        {
                            if (updateForm.ShowDialog() == DialogResult.Yes)
                            {
                                UpdateChecker.VisitDownloadPage();
                            }

                            if (updateForm.DontShowAgainIsChecked)
                            {
                                _settingManager.Settings.CheckForUpdatesAtStartup = false;
                            }
                        }
                    });
                });
            }
            #endregion

            Text = $"LiS BtS Savegame Editor v{Program.GetApplicationVersionStr()}";

            SaveDataFilePath = _settingManager.Settings.SavePath;

            ToolTip toolTip = new ToolTip();
            toolTip.BackColor = SystemColors.InfoText;
            toolTip.IsBalloon = true;
            toolTip.SetToolTip(buttonExport, "Click to export variables with a value into a text file.\nCtrl+Click to export all variables.");

            if (_settingManager.Settings.GamePath == null)
            {
                DetectGamePath();
            }
            else
            {
                textBoxLisPath.Text = _settingManager.Settings.GamePath;
                folderBrowserDialog1.SelectedPath = _settingManager.Settings.GamePath;
            }

            DetectSavePath();
            label4.Visible = false;
            tabControl1.SelectedTab = tabPageFlags;
            tabControl1.SelectedTab = tabPageFloats;
            tabControl1.SelectedTab = tabPageItems;
            tabControl1.SelectedTab = tabPageVars;

            //double buffering
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridViewFlags, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridViewFloats, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridViewItems, new object[] { true });

        }

        private void DetectGamePath ()
        {
            RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64); //https://social.msdn.microsoft.com/Forums/vstudio/en-US/ef0de98a-18db-43e1-b9b9-b52c3b5f3d4c/registry-issue-getting-install-location-and-saving-its-path-c?forum=csharpgeneral
            localKey = localKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 554620");
            try 
            {
                textBoxLisPath.Text = localKey.GetValue("InstallLocation").ToString();
            }
            catch
            {
                textBoxLisPath.Text = "Auto-detection failed! Please select the path manually.";
            }
        }

        private void DetectSavePath()
        {
            try
            {
                _steamIdFolders = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\LocalLow\Square Enix\Life Is Strange_ Before The Storm\Saves").ToList<string>();
            }
            catch
            {

            }
            if (_steamIdFolders.Count != 0)
            {
                _steamIdFolders.RemoveAt(_steamIdFolders.Count - 1); //remove the preferences from the list
            }
            if (String.IsNullOrEmpty(_settingManager.Settings.SavePath))
            {
                if (_steamIdFolders.Count == 1)
                {
                    bool found = false;
                    for (int i=0; i<3; i++)
                    {
                        if (File.Exists(_steamIdFolders[0].ToString() + @"\SLOT_0" + i.ToString() + @"\Data.Save"))
                        {
                            textBoxSavePath.Text = _steamIdFolders[0].ToString() + @"\SLOT_0" + i.ToString() + @"\Data.Save";
                            _settingManager.Settings.SavePath = textBoxSavePath.Text;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        textBoxSavePath.Text = "Auto-detection failed! Please select the path manually.";
                    }
                }
                else if (_steamIdFolders.Count > 1 && Directory.Exists(textBoxLisPath.Text))
                {
                    DeckNineXorEncoder.ReadKeyFromFile(PathHelper.GetCSharpAssemblyPath(textBoxLisPath.Text));
                    _initialData.ReadFromFile(PathHelper.GetInitialDataFilePath(textBoxLisPath.Text));
                    if (_gameSave == null)
                    {
                        _gameSave = new GameSave(_initialData);
                    }

                    using (var saveSelectionForm = new SaveSelectionForm(_gameSave))
                    {
                        if (saveSelectionForm.ShowDialog() == DialogResult.OK)
                        {
                            SaveDataFilePath = saveSelectionForm.SaveDataFilePath;
                        }
                    }
                }
                else
                {
                    textBoxSavePath.Text = "Auto-detection failed! Please select the path manually.";
                }
            }
            else
            {
                textBoxSavePath.Text = _settingManager.Settings.SavePath;
            }
        }

        private void buttonSaveEdits_Click(object sender, EventArgs e)
        {
            if (!_gameSave.MainSaveChangesSaved)
            {
                _gameSave.WriteSaveToFile(textBoxSavePath.Text, _gameSave.MainData, SaveType.Regular);
            }
            if (!_gameSave.FarewellSaveChangesSaved)
            {
                _gameSave.WriteSaveToFile(textBoxSavePath.Text.Replace("SLOT_", "Bonus"), _gameSave.FarewellData, SaveType.Bonus);
            }
           
            if (_gameSave.MainSaveChangesSaved && _gameSave.FarewellSaveChangesSaved)
            {
                MessageBox.Show(Resources.EditsSuccessfullySavedMessage, "Savegame Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                label4.Visible = false;

                //Reset the colors of all cells in all DataGrids
                foreach (TabPage tab in tabControl1.Controls)
                {
                    DataGridView grid = (DataGridView)tab.Controls[0];
                    for (int i = 0; i < grid.RowCount; i++)
                    {
                        for (int j = 0; j < grid.ColumnCount; j++)
                        {
                            if (grid.Rows[i].Cells[j].ReadOnly)
                            {
                                grid.Rows[i].Cells[j].Style.BackColor = Color.LightGray;
                            }
                            else
                            {
                                grid.Rows[i].Cells[j].Style.BackColor = Color.White;
                            }
                        }
                    }
                }
            }  
        }

        public int? origCellValue, newCellValue;
        public float? origFloatValue, newFloatValue;
        public bool origFlagState, newFlagState;
        private VariableScope _editingVariableScope;

        public bool editModeActive = false;

        private void checkBoxEditMode_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_settingManager.Settings.EditModeIntroShown)
            {
                MessageBox.Show(Resources.EditModeHelpFirst, "Savegame Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _settingManager.Settings.EditModeIntroShown = true;
            }

            if (checkBoxEditMode.Checked)
            {
                enableEditMode();
            }
            else
            {
                if (!_gameSave.MainSaveChangesSaved || !_gameSave.FarewellSaveChangesSaved)
                {
                    DialogResult answer = MessageBox.Show(Resources.UnsavedEditsWarningMessage.Insert(34, " Edit Mode"), 
                        "Savegame Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            dataGridViewItems.ReadOnly = false;
            buttonShowContent.Enabled = false;
            buttonManualBrowseBts.Enabled = false;
            buttonManualBrowseSave.Enabled = false;
            buttonSaveSelector.Enabled = false;
            textBoxLisPath.Enabled = false;
            textBoxSavePath.Enabled = false;
            rbMain.Enabled = false;
            checkBoxE1.Enabled = false;
            checkBoxE2.Enabled = false;
            checkBoxE3.Enabled = false;
            rbBonus.Enabled = false;
            buttonSaveEdits.Enabled = true;
            buttonExtras.Enabled = false;
            if (findForm != null)
            {
                findForm.ResetSearchState();
            }
            UpdateDataGrid();
            UpdateFlagGrid();
            UpdateFloatGrid();
            UpdateItemGrid();
        }
        private void disableEditMode()
        {
            editModeActive = false;
            _gameSave.MainSaveChangesSaved = true;
            _gameSave.FarewellSaveChangesSaved = true;
            dataGridView1.ReadOnly = true;
            dataGridViewFlags.ReadOnly = true;
            dataGridViewFloats.ReadOnly = true;
            dataGridViewItems.ReadOnly = true;
            buttonShowContent.Enabled = true;
            buttonManualBrowseBts.Enabled = true;
            buttonManualBrowseSave.Enabled = true;
            buttonSaveSelector.Enabled = true;
            textBoxLisPath.Enabled = true;
            textBoxSavePath.Enabled = true;
            UpdateEpisodeBoxes();
            rbMain.Enabled = !_gameSave.MainSaveIsEmpty;
            rbBonus.Enabled = !_gameSave.FarewellSaveIsEmpty;

            if (rbMain.Checked)
            {
                if (checkBoxE1.Enabled) checkBoxE1.Checked = true;
                if (checkBoxE2.Enabled) checkBoxE2.Checked = true;
                if (checkBoxE3.Enabled) checkBoxE3.Checked = true;
            }
            buttonSaveEdits.Enabled = false;
            buttonExtras.Enabled = true;
            label4.Visible = false;
            _gameSave.ReadMainSaveFromFile(textBoxSavePath.Text);
            if (findForm != null)
            {
                findForm.ResetSearchState();
            }
            UpdateDataGrid();
            UpdateFlagGrid();
            UpdateFloatGrid();
            UpdateItemGrid();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())) //if the cell was originally empty
            {
                origCellValue = null;
            }
            else
            {
                origCellValue = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            }

            if (rbMain.Checked)
            {
                switch (e.ColumnIndex)
                {
                    case 1:
                        _editingVariableScope = VariableScope.Global;
                        break;
                    case 2:
                        _editingVariableScope = VariableScope.CurrentMainCheckpoint;
                        break;
                    case 3:
                        _editingVariableScope = VariableScope.LastCheckpoint;
                        break;
                    default:
                        _editingVariableScope = VariableScope.RegularCheckpoint;
                        break;
                }
            }
            else if (rbBonus.Checked)
            {
                switch (e.ColumnIndex)
                {
                    case 1:
                        _editingVariableScope = VariableScope.CurrentFarewellCheckpoint;
                        break;
                    case 2:
                        _editingVariableScope = VariableScope.LastCheckpoint;
                        break;
                    default:
                        _editingVariableScope = VariableScope.RegularCheckpoint;
                        break;
                }
            }
            
        }

        private void dataGridViewFloats_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dataGridViewFloats.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())) //if the cell was originally empty
            {
                origFloatValue = null;
            }
            else
            {
                origFloatValue = float.Parse(dataGridViewFloats.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace('.', ','));
            }

            if (rbMain.Checked)
            {
                switch (e.ColumnIndex)
                {
                    case 1:
                        _editingVariableScope = VariableScope.Global;
                        break;
                    case 2:
                        _editingVariableScope = VariableScope.CurrentMainCheckpoint;
                        break;
                    case 3:
                        _editingVariableScope = VariableScope.LastCheckpoint;
                        break;
                    default:
                        _editingVariableScope = VariableScope.RegularCheckpoint;
                        break;
                }
            }
            else if (rbBonus.Checked)
            {
                switch (e.ColumnIndex)
                {
                    case 1:
                        _editingVariableScope = VariableScope.CurrentFarewellCheckpoint;
                        break;
                    case 2:
                        _editingVariableScope = VariableScope.LastCheckpoint;
                        break;
                    default:
                        _editingVariableScope = VariableScope.RegularCheckpoint;
                        break;
                }
            }
        }

        private void dataGridViewFlags_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            origFlagState = Convert.ToBoolean(dataGridViewFlags.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

            if (rbMain.Checked)
            {
                switch (e.ColumnIndex)
                {
                    case 1:
                        _editingVariableScope = VariableScope.Global;
                        break;
                    case 2:
                        _editingVariableScope = VariableScope.CurrentMainCheckpoint;
                        break;
                    case 3:
                        _editingVariableScope = VariableScope.LastCheckpoint;
                        break;
                    default:
                        _editingVariableScope = VariableScope.RegularCheckpoint;
                        break;
                }
            }
            else if (rbBonus.Checked)
            {
                switch (e.ColumnIndex)
                {
                    case 1:
                        _editingVariableScope = VariableScope.CurrentFarewellCheckpoint;
                        break;
                    case 2:
                        _editingVariableScope = VariableScope.LastCheckpoint;
                        break;
                    default:
                        _editingVariableScope = VariableScope.RegularCheckpoint;
                        break;
                }
            }
        }

        private void dataGridViewItems_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            origFlagState = Convert.ToBoolean(dataGridViewItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

            if (rbMain.Checked)
            {
                switch (e.ColumnIndex)
                {
                    case 1:
                        _editingVariableScope = VariableScope.CurrentMainCheckpoint;
                        break;
                    case 2:
                        _editingVariableScope = VariableScope.LastCheckpoint;
                        break;
                    default:
                        _editingVariableScope = VariableScope.RegularCheckpoint;
                        break;
                }
            }
            else if (rbBonus.Checked)
            {
                switch (e.ColumnIndex)
                {
                    case 1:
                        _editingVariableScope = VariableScope.CurrentFarewellCheckpoint;
                        break;
                    case 2:
                        _editingVariableScope = VariableScope.LastCheckpoint;
                        break;
                    default:
                        _editingVariableScope = VariableScope.RegularCheckpoint;
                        break;
                }
            }
        }


        #region Batch editing

        bool firstEdit = true;
        private void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            int number;
            if (dataGridView1.SelectedCells.Count > 1 && firstEdit && (String.IsNullOrWhiteSpace(e.Value.ToString()) || int.TryParse(e.Value.ToString(), out number)))
            {
                firstEdit = false;
                fillCellsWithValue(dataGridView1.SelectedCells, e.Value);
                firstEdit = true;
            }
        }

        private void dataGridViewFloats_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            float number;
            if (dataGridViewFloats.SelectedCells.Count > 1 && firstEdit && (String.IsNullOrWhiteSpace(e.Value.ToString()) || float.TryParse(e.Value.ToString().Replace('.', ','), out number)))
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

        private void dataGridViewItems_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'T')
            {
                fillCellsWithValue(dataGridViewItems.SelectedCells, true);
            }
            else if (e.KeyChar == 'F')
            {
                fillCellsWithValue(dataGridViewItems.SelectedCells, false);
            }
        }

        private void fillCellsWithValue(DataGridViewSelectedCellCollection selectedCells, object value)
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
        #endregion

        private void pictureBoxHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.EditModeHelpIconMessage, "Help", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void buttonSaveSelector_Click(object sender, EventArgs e)
        {
            DeckNineXorEncoder.ReadKeyFromFile(PathHelper.GetCSharpAssemblyPath(textBoxLisPath.Text));
            _initialData.ReadFromFile(PathHelper.GetInitialDataFilePath(textBoxLisPath.Text));
            if (_gameSave == null)
            {
                _gameSave = new GameSave(_initialData);
            }

            
            using (var saveSelectionForm = new SaveSelectionForm(_gameSave, textBoxSavePath.Text))
            {
                if (saveSelectionForm.ShowDialog() == DialogResult.OK)
                {
                    SaveDataFilePath = saveSelectionForm.SaveDataFilePath;
                }
            }
        }

        private void buttonExtras_Click(object sender, EventArgs e)
        {
            ExtrasForm formExtras = new ExtrasForm(_settingManager);
            formExtras.savePath = textBoxSavePath.Text;
            formExtras.headerPath = Path.GetDirectoryName(textBoxSavePath.Text) + @"\Header.Save";
            formExtras.m_GameSave = _gameSave;
            formExtras.m_assFile = new AssFile(PathHelper.GetCSharpAssemblyPath(textBoxLisPath.Text));
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
                    MessageBox.Show(Resources.BadVariableValueMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if ( (rbMain.Checked && _gameSave.FindAndUpdateMainVarValue(point_id, var_name, origCellValue, newCellValue, _editingVariableScope)) ||
                     (rbBonus.Checked && _gameSave.FindAndUpdateFarewellVarValue(point_id, var_name, origCellValue, newCellValue, _editingVariableScope)) )
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightGoldenrodYellow;
                    label4.Text = "Press 'Save' to write changes to the save file.";
                    label4.Visible = true;
                }
                else dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = origCellValue;
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
                if ( (rbMain.Checked && _gameSave.FindAndUpdateMainFlagValue(point_id, var_name, origFlagState, _editingVariableScope)) ||
                     (rbBonus.Checked && _gameSave.FindAndUpdateFarewellFlagValue(point_id, var_name, origFlagState, _editingVariableScope)) )
                {
                    dataGridViewFlags.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightGoldenrodYellow;
                    label4.Text = "Press 'Save' to write changes to the save file.";
                    label4.Visible = true;
                }
                else dataGridViewFlags.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = origFlagState;
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
                    MessageBox.Show(Resources.BadVariableValueMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if ( (rbMain.Checked && _gameSave.FindAndUpdateMainFloatValue(point_id, var_name, origFloatValue, newFloatValue, _editingVariableScope)) ||
                     (rbBonus.Checked && _gameSave.FindAndUpdateFarewellFloatValue(point_id, var_name, origFloatValue, newFloatValue, _editingVariableScope)) )
                {
                    dataGridViewFloats.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightGoldenrodYellow;
                    label4.Text = "Press 'Save' to write changes to the save file.";
                    label4.Visible = true;
                }
                else dataGridViewFloats.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = origFloatValue;
            }
        }

        private void dataGridViewItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            newFlagState = Convert.ToBoolean(dataGridViewItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

            if (newFlagState != origFlagState)
            {
                point_id = dataGridViewItems.Rows[0].Cells[e.ColumnIndex].Value.ToString();
                var_name = dataGridViewItems.Rows[e.RowIndex].Cells[0].Value.ToString();
                //MessageBox.Show("Finished Editing of Cell on Column " + e.ColumnIndex.ToString() + " and Row " + e.RowIndex.ToString() + "\n Value of the cell is " + newFlagState.ToString(), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show("The Identifier of edited cell is " + point_id  + "\n and the flag name is " + flag_name, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if ( (rbMain.Checked && _gameSave.FindAndUpdateMainItemValue(point_id, var_name, origFlagState, _editingVariableScope)) ||
                    (rbBonus.Checked && _gameSave.FindAndUpdateFarewellItemValue(point_id, var_name, origFlagState, _editingVariableScope)))
                {
                    dataGridViewItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightGoldenrodYellow;
                    label4.Text = "Press 'Save' to write changes to the save file.";
                    label4.Visible = true;
                }
                else dataGridViewItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = origFlagState;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _settingManager.SaveSettings();

            if (_gameSave != null && !_gameSave.MainSaveChangesSaved)
            {
                DialogResult answer = MessageBox.Show(Resources.UnsavedEditsWarningMessage, 
                    "Savegame Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        #region Search functions
        FindForm findForm;
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dataGridView1.DataSource != null && ModifierKeys == Keys.Control && (int)e.KeyChar == 6)
            {
                if (findForm == null)
                {
                    findForm = new FindForm();
                    findForm.form1 = this;
                }
                findForm.tab_num = tabControl1.SelectedIndex;
                findForm.UpdateRadioChoice();
                if (findForm.Visible)
                {
                    findForm.WindowState = FormWindowState.Normal;
                    findForm.Activate();
                }
                else
                {
                    findForm.Show(this);
                }
                ((DataGridView)tabControl1.SelectedTab.Controls[0]).CancelEdit();
            }
        }

        public string find_Starts = "", find_Contains = "", find_Ends = "";

        

        public List<DataGridViewCell> find_results = new List<DataGridViewCell>();

        public void ResetFindStrings()
        {
            find_Starts = "";
            find_Contains = "";
            find_Ends = "";
        }

        DataGridView target_grid;
        int res_index = 0;
        public void FindFirst(int tab_num, bool CaseSensitive)
        {
            tabControl1.SelectTab(tab_num);
            target_grid = (DataGridView)tabControl1.SelectedTab.Controls[0];

            StringComparison strcomp = CaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;

            for (int i = (tab_num == 0) ? 2 : 1; i < target_grid.RowCount; i++)
            {
                string value = target_grid[0, i].Value.ToString();
                if (value.StartsWith(find_Starts, !CaseSensitive, CultureInfo.InvariantCulture) &&
                    (value.IndexOf(find_Contains, strcomp) != -1) &&
                    value.EndsWith(find_Ends, !CaseSensitive, CultureInfo.InvariantCulture))
                {
                    find_results.Add(target_grid[0, i]);
                }
            }

            if (find_results.Count > 0)
            {
                target_grid.CurrentCell = find_results[0];
                res_index = 0;
            } 
        }

        public void FindPrev(int tab_num)
        {
            tabControl1.SelectTab(tab_num);
            res_index--;
            if (res_index < 0) res_index = find_results.Count - 1;
            target_grid.CurrentCell = find_results[res_index];
        }

        public void FindNext(int tab_num)
        {
            tabControl1.SelectTab(tab_num);
            res_index++;
            if (res_index == find_results.Count) res_index = 0;
            target_grid.CurrentCell = find_results[res_index];
        }
        #endregion
    }
}



