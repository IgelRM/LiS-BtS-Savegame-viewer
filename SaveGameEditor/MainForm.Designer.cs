namespace SaveGameEditor
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSaveEdits = new System.Windows.Forms.Button();
            this.buttonExtras = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxSavePath = new System.Windows.Forms.TextBox();
            this.textBoxLisPath = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelEpisodes = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxE1 = new System.Windows.Forms.CheckBox();
            this.checkBoxE2 = new System.Windows.Forms.CheckBox();
            this.checkBoxE3 = new System.Windows.Forms.CheckBox();
            this.checkBoxE4 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBoxEditMode = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonManualBrowseBts = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageVars = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPageFlags = new System.Windows.Forms.TabPage();
            this.dataGridViewFlags = new System.Windows.Forms.DataGridView();
            this.tabPageFloats = new System.Windows.Forms.TabPage();
            this.dataGridViewFloats = new System.Windows.Forms.DataGridView();
            this.tabPageItems = new System.Windows.Forms.TabPage();
            this.dataGridViewItems = new System.Windows.Forms.DataGridView();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonShowContent = new System.Windows.Forms.Button();
            this.buttonSaveSelector = new System.Windows.Forms.Button();
            this.buttonManualBrowseSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanelEpisodes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageVars.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPageFlags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFlags)).BeginInit();
            this.tabPageFloats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFloats)).BeginInit();
            this.tabPageItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItems)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Data.Save file|Data.Save";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.button1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.button2, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(918, 44);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(210, 35);
            this.tableLayoutPanel4.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(2, 2);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.MinimumSize = new System.Drawing.Size(90, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 31);
            this.button1.TabIndex = 1;
            this.button1.Text = "Show Content";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonShowContent_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(107, 2);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 31);
            this.button2.TabIndex = 6;
            this.button2.Text = "Export";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(915, 82);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(216, 35);
            this.tableLayoutPanel2.TabIndex = 11;
            // 
            // buttonSaveEdits
            // 
            this.buttonSaveEdits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveEdits.Enabled = false;
            this.buttonSaveEdits.Location = new System.Drawing.Point(986, 60);
            this.buttonSaveEdits.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSaveEdits.Name = "buttonSaveEdits";
            this.buttonSaveEdits.Size = new System.Drawing.Size(90, 25);
            this.buttonSaveEdits.TabIndex = 12;
            this.buttonSaveEdits.Text = "Save";
            this.buttonSaveEdits.UseVisualStyleBackColor = true;
            this.buttonSaveEdits.Click += new System.EventHandler(this.buttonSaveEdits_Click);
            // 
            // buttonExtras
            // 
            this.buttonExtras.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExtras.Enabled = false;
            this.buttonExtras.Location = new System.Drawing.Point(1080, 60);
            this.buttonExtras.Margin = new System.Windows.Forms.Padding(2);
            this.buttonExtras.Name = "buttonExtras";
            this.buttonExtras.Size = new System.Drawing.Size(110, 25);
            this.buttonExtras.TabIndex = 13;
            this.buttonExtras.Text = "Extras";
            this.buttonExtras.UseVisualStyleBackColor = true;
            this.buttonExtras.Click += new System.EventHandler(this.buttonExtras_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label1.Size = new System.Drawing.Size(143, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "Path to savefile (Data.Save):";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label3.Size = new System.Drawing.Size(143, 35);
            this.label3.TabIndex = 1;
            this.label3.Text = "Show variables from:";
            // 
            // textBoxSavePath
            // 
            this.textBoxSavePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSavePath.Location = new System.Drawing.Point(151, 4);
            this.textBoxSavePath.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxSavePath.Name = "textBoxSavePath";
            this.textBoxSavePath.Size = new System.Drawing.Size(765, 20);
            this.textBoxSavePath.TabIndex = 0;
            this.textBoxSavePath.Text = "C: \\Users\\[UserName]\\AppData\\LocalLow\\Square Enix\\Life is Strange_ Before the Sto" +
    "rm\\Saves\\[id]\\SLOT_00\\Data.Save";
            this.textBoxSavePath.TextChanged += new System.EventHandler(this.textBoxSavePath_TextChanged);
            // 
            // textBoxLisPath
            // 
            this.textBoxLisPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLisPath.Location = new System.Drawing.Point(152, 33);
            this.textBoxLisPath.Name = "textBoxLisPath";
            this.textBoxLisPath.Size = new System.Drawing.Size(763, 20);
            this.textBoxLisPath.TabIndex = 2;
            this.textBoxLisPath.Text = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Life is Strange - Before the Storm";
            this.textBoxLisPath.TextChanged += new System.EventHandler(this.textBoxLisPath_TextChanged);
            // 
            // tableLayoutPanelEpisodes
            // 
            this.tableLayoutPanelEpisodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelEpisodes.ColumnCount = 7;
            this.tableLayoutPanelEpisodes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEpisodes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEpisodes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEpisodes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEpisodes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEpisodes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanelEpisodes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEpisodes.Controls.Add(this.checkBoxE1, 0, 0);
            this.tableLayoutPanelEpisodes.Controls.Add(this.checkBoxE2, 1, 0);
            this.tableLayoutPanelEpisodes.Controls.Add(this.checkBoxE3, 2, 0);
            this.tableLayoutPanelEpisodes.Controls.Add(this.checkBoxE4, 3, 0);
            this.tableLayoutPanelEpisodes.Controls.Add(this.label4, 6, 0);
            this.tableLayoutPanelEpisodes.Controls.Add(this.pictureBox1, 5, 0);
            this.tableLayoutPanelEpisodes.Controls.Add(this.checkBoxEditMode, 4, 0);
            this.tableLayoutPanelEpisodes.Location = new System.Drawing.Point(152, 61);
            this.tableLayoutPanelEpisodes.Name = "tableLayoutPanelEpisodes";
            this.tableLayoutPanelEpisodes.RowCount = 1;
            this.tableLayoutPanelEpisodes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEpisodes.Size = new System.Drawing.Size(763, 29);
            this.tableLayoutPanelEpisodes.TabIndex = 1;
            // 
            // checkBoxE1
            // 
            this.checkBoxE1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxE1.Enabled = false;
            this.checkBoxE1.Location = new System.Drawing.Point(3, 3);
            this.checkBoxE1.Name = "checkBoxE1";
            this.checkBoxE1.Size = new System.Drawing.Size(73, 23);
            this.checkBoxE1.TabIndex = 1;
            this.checkBoxE1.Text = "Episode 1";
            this.checkBoxE1.UseVisualStyleBackColor = true;
            this.checkBoxE1.CheckedChanged += new System.EventHandler(this.checkBoxEpisodes_CheckedChanged);
            // 
            // checkBoxE2
            // 
            this.checkBoxE2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxE2.Enabled = false;
            this.checkBoxE2.Location = new System.Drawing.Point(82, 3);
            this.checkBoxE2.Name = "checkBoxE2";
            this.checkBoxE2.Size = new System.Drawing.Size(73, 23);
            this.checkBoxE2.TabIndex = 5;
            this.checkBoxE2.Text = "Episode 2";
            this.checkBoxE2.UseVisualStyleBackColor = true;
            this.checkBoxE2.CheckedChanged += new System.EventHandler(this.checkBoxEpisodes_CheckedChanged);
            // 
            // checkBoxE3
            // 
            this.checkBoxE3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxE3.Enabled = false;
            this.checkBoxE3.Location = new System.Drawing.Point(161, 3);
            this.checkBoxE3.Name = "checkBoxE3";
            this.checkBoxE3.Size = new System.Drawing.Size(73, 23);
            this.checkBoxE3.TabIndex = 6;
            this.checkBoxE3.Text = "Episode 3";
            this.checkBoxE3.UseVisualStyleBackColor = true;
            this.checkBoxE3.CheckedChanged += new System.EventHandler(this.checkBoxEpisodes_CheckedChanged);
            // 
            // checkBoxE4
            // 
            this.checkBoxE4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxE4.Enabled = false;
            this.checkBoxE4.Location = new System.Drawing.Point(240, 3);
            this.checkBoxE4.Name = "checkBoxE4";
            this.checkBoxE4.Size = new System.Drawing.Size(97, 23);
            this.checkBoxE4.TabIndex = 7;
            this.checkBoxE4.Text = "Bonus Episode";
            this.checkBoxE4.UseVisualStyleBackColor = true;
            this.checkBoxE4.CheckedChanged += new System.EventHandler(this.checkBoxEpisodes_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(436, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(324, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Save file changed! Press Show Content to update.";
            this.label4.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox1.Image = global::SaveGameEditor.Properties.Resources.Help;
            this.pictureBox1.Location = new System.Drawing.Point(417, 6);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBoxHelp_Click);
            // 
            // checkBoxEditMode
            // 
            this.checkBoxEditMode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxEditMode.AutoSize = true;
            this.checkBoxEditMode.Location = new System.Drawing.Point(343, 3);
            this.checkBoxEditMode.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.checkBoxEditMode.Name = "checkBoxEditMode";
            this.checkBoxEditMode.Size = new System.Drawing.Size(74, 23);
            this.checkBoxEditMode.TabIndex = 9;
            this.checkBoxEditMode.Text = "Edit Mode";
            this.checkBoxEditMode.UseVisualStyleBackColor = true;
            this.checkBoxEditMode.MouseUp += new System.Windows.Forms.MouseEventHandler(this.checkBoxEditMode_MouseUp);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            this.tableLayoutPanel1.Controls.Add(this.buttonExtras, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonManualBrowseBts, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanelEpisodes, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLisPath, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxSavePath, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonAbout, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonSaveEdits, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonExport, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonShowContent, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonSaveSelector, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonManualBrowseSave, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1192, 552);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // buttonManualBrowseBts
            // 
            this.buttonManualBrowseBts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonManualBrowseBts.Location = new System.Drawing.Point(920, 31);
            this.buttonManualBrowseBts.Margin = new System.Windows.Forms.Padding(2);
            this.buttonManualBrowseBts.Name = "buttonManualBrowseBts";
            this.buttonManualBrowseBts.Size = new System.Drawing.Size(62, 25);
            this.buttonManualBrowseBts.TabIndex = 9;
            this.buttonManualBrowseBts.Text = "Browse";
            this.buttonManualBrowseBts.UseVisualStyleBackColor = true;
            this.buttonManualBrowseBts.Click += new System.EventHandler(this.button6_Click);
            // 
            // tabControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 5);
            this.tabControl1.Controls.Add(this.tabPageVars);
            this.tabControl1.Controls.Add(this.tabPageFlags);
            this.tabControl1.Controls.Add(this.tabPageFloats);
            this.tabControl1.Controls.Add(this.tabPageItems);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 96);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1189, 453);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPageVars
            // 
            this.tabPageVars.Controls.Add(this.dataGridView1);
            this.tabPageVars.Location = new System.Drawing.Point(4, 22);
            this.tabPageVars.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageVars.Name = "tabPageVars";
            this.tabPageVars.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.tabPageVars.Size = new System.Drawing.Size(1181, 427);
            this.tabPageVars.TabIndex = 0;
            this.tabPageVars.Text = "Variables";
            this.tabPageVars.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Location = new System.Drawing.Point(1, 2);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(1178, 425);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.dataGridView1_CellParsing);
            // 
            // tabPageFlags
            // 
            this.tabPageFlags.Controls.Add(this.dataGridViewFlags);
            this.tabPageFlags.Location = new System.Drawing.Point(4, 22);
            this.tabPageFlags.Name = "tabPageFlags";
            this.tabPageFlags.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFlags.Size = new System.Drawing.Size(1181, 427);
            this.tabPageFlags.TabIndex = 1;
            this.tabPageFlags.Text = "Flags";
            this.tabPageFlags.UseVisualStyleBackColor = true;
            // 
            // dataGridViewFlags
            // 
            this.dataGridViewFlags.AllowUserToAddRows = false;
            this.dataGridViewFlags.AllowUserToDeleteRows = false;
            this.dataGridViewFlags.AllowUserToResizeRows = false;
            this.dataGridViewFlags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewFlags.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewFlags.Location = new System.Drawing.Point(1, 2);
            this.dataGridViewFlags.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridViewFlags.Name = "dataGridViewFlags";
            this.dataGridViewFlags.ReadOnly = true;
            this.dataGridViewFlags.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewFlags.ShowEditingIcon = false;
            this.dataGridViewFlags.Size = new System.Drawing.Size(1178, 425);
            this.dataGridViewFlags.TabIndex = 2;
            this.dataGridViewFlags.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewFlags_CellBeginEdit);
            this.dataGridViewFlags.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFlags_CellEndEdit);
            this.dataGridViewFlags.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dataGridViewFlags_KeyPress);
            // 
            // tabPageFloats
            // 
            this.tabPageFloats.Controls.Add(this.dataGridViewFloats);
            this.tabPageFloats.Location = new System.Drawing.Point(4, 22);
            this.tabPageFloats.Name = "tabPageFloats";
            this.tabPageFloats.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFloats.Size = new System.Drawing.Size(1181, 427);
            this.tabPageFloats.TabIndex = 2;
            this.tabPageFloats.Text = "Floats";
            this.tabPageFloats.UseVisualStyleBackColor = true;
            // 
            // dataGridViewFloats
            // 
            this.dataGridViewFloats.AllowUserToAddRows = false;
            this.dataGridViewFloats.AllowUserToDeleteRows = false;
            this.dataGridViewFloats.AllowUserToResizeRows = false;
            this.dataGridViewFloats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewFloats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewFloats.Location = new System.Drawing.Point(1, 2);
            this.dataGridViewFloats.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridViewFloats.Name = "dataGridViewFloats";
            this.dataGridViewFloats.ReadOnly = true;
            this.dataGridViewFloats.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewFloats.ShowEditingIcon = false;
            this.dataGridViewFloats.Size = new System.Drawing.Size(1178, 425);
            this.dataGridViewFloats.TabIndex = 3;
            this.dataGridViewFloats.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewFloats_CellBeginEdit);
            this.dataGridViewFloats.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFloats_CellEndEdit);
            this.dataGridViewFloats.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.dataGridViewFloats_CellParsing);
            // 
            // tabPageItems
            // 
            this.tabPageItems.Controls.Add(this.dataGridViewItems);
            this.tabPageItems.Location = new System.Drawing.Point(4, 22);
            this.tabPageItems.Name = "tabPageItems";
            this.tabPageItems.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageItems.Size = new System.Drawing.Size(1181, 427);
            this.tabPageItems.TabIndex = 3;
            this.tabPageItems.Text = "Items";
            this.tabPageItems.UseVisualStyleBackColor = true;
            // 
            // dataGridViewItems
            // 
            this.dataGridViewItems.AllowUserToAddRows = false;
            this.dataGridViewItems.AllowUserToDeleteRows = false;
            this.dataGridViewItems.AllowUserToResizeRows = false;
            this.dataGridViewItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewItems.Location = new System.Drawing.Point(1, 2);
            this.dataGridViewItems.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridViewItems.Name = "dataGridViewItems";
            this.dataGridViewItems.ReadOnly = true;
            this.dataGridViewItems.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewItems.ShowEditingIcon = false;
            this.dataGridViewItems.Size = new System.Drawing.Size(1178, 425);
            this.dataGridViewItems.TabIndex = 3;
            this.dataGridViewItems.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewItems_CellBeginEdit);
            this.dataGridViewItems.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewItems_CellEndEdit);
            this.dataGridViewItems.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dataGridViewItems_KeyPress);
            // 
            // buttonAbout
            // 
            this.buttonAbout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbout.Location = new System.Drawing.Point(920, 60);
            this.buttonAbout.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(62, 25);
            this.buttonAbout.TabIndex = 10;
            this.buttonAbout.Text = "About";
            this.buttonAbout.UseVisualStyleBackColor = true;
            this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Enabled = false;
            this.buttonExport.Location = new System.Drawing.Point(1080, 31);
            this.buttonExport.Margin = new System.Windows.Forms.Padding(2);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(110, 25);
            this.buttonExport.TabIndex = 5;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // buttonShowContent
            // 
            this.buttonShowContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShowContent.Location = new System.Drawing.Point(986, 31);
            this.buttonShowContent.Margin = new System.Windows.Forms.Padding(2);
            this.buttonShowContent.MinimumSize = new System.Drawing.Size(90, 0);
            this.buttonShowContent.Name = "buttonShowContent";
            this.buttonShowContent.Size = new System.Drawing.Size(90, 25);
            this.buttonShowContent.TabIndex = 1;
            this.buttonShowContent.Text = "Show Content";
            this.buttonShowContent.UseVisualStyleBackColor = true;
            this.buttonShowContent.Click += new System.EventHandler(this.buttonShowContent_Click);
            // 
            // buttonSaveSelector
            // 
            this.buttonSaveSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveSelector.Location = new System.Drawing.Point(1080, 2);
            this.buttonSaveSelector.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSaveSelector.Name = "buttonSaveSelector";
            this.buttonSaveSelector.Size = new System.Drawing.Size(110, 25);
            this.buttonSaveSelector.TabIndex = 14;
            this.buttonSaveSelector.Text = "Select save";
            this.buttonSaveSelector.UseVisualStyleBackColor = true;
            this.buttonSaveSelector.Click += new System.EventHandler(this.buttonSaveSelector_Click);
            // 
            // buttonManualBrowseSave
            // 
            this.buttonManualBrowseSave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.buttonManualBrowseSave, 2);
            this.buttonManualBrowseSave.Location = new System.Drawing.Point(920, 2);
            this.buttonManualBrowseSave.Margin = new System.Windows.Forms.Padding(2);
            this.buttonManualBrowseSave.Name = "buttonManualBrowseSave";
            this.buttonManualBrowseSave.Size = new System.Drawing.Size(156, 25);
            this.buttonManualBrowseSave.TabIndex = 8;
            this.buttonManualBrowseSave.Text = "Browse manually";
            this.buttonManualBrowseSave.UseVisualStyleBackColor = true;
            this.buttonManualBrowseSave.Click += new System.EventHandler(this.button5_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 29);
            this.label2.TabIndex = 15;
            this.label2.Text = "Path to LiS BtS:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 550);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(1020, 450);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanelEpisodes.ResumeLayout(false);
            this.tableLayoutPanelEpisodes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageVars.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPageFlags.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFlags)).EndInit();
            this.tabPageFloats.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFloats)).EndInit();
            this.tabPageItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonSaveEdits;
        private System.Windows.Forms.Button buttonExtras;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEpisodes;
        private System.Windows.Forms.CheckBox checkBoxE1;
        private System.Windows.Forms.CheckBox checkBoxE2;
        private System.Windows.Forms.CheckBox checkBoxE3;
        private System.Windows.Forms.CheckBox checkBoxE4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonShowContent;
        private System.Windows.Forms.Button buttonManualBrowseSave;
        private System.Windows.Forms.Button buttonManualBrowseBts;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox textBoxSavePath;
        public System.Windows.Forms.TextBox textBoxLisPath;
        private System.Windows.Forms.CheckBox checkBoxEditMode;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonSaveSelector;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageVars;
        private System.Windows.Forms.TabPage tabPageFlags;
        private System.Windows.Forms.DataGridView dataGridViewFlags;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPageFloats;
        private System.Windows.Forms.DataGridView dataGridViewFloats;
        private System.Windows.Forms.TabPage tabPageItems;
        private System.Windows.Forms.DataGridView dataGridViewItems;
    }
}

