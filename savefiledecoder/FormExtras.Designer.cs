namespace savefiledecoder
{
    partial class FormExtras
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExtras));
            this.buttonManualBkpSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonManualBkpHeader = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxPoint = new System.Windows.Forms.ComboBox();
            this.labelPointType = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.comboBoxHeaderEp = new System.Windows.Forms.ComboBox();
            this.buttonRewindCheckpoint = new System.Windows.Forms.Button();
            this.buttonLoadHeader = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.labelHeaderNotFound = new System.Windows.Forms.Label();
            this.checkBoxSkipIntro = new System.Windows.Forms.CheckBox();
            this.checkBoxSkipCutscenes = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonSaveDLL = new System.Windows.Forms.Button();
            this.buttonLoadDLL = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonManualBkpSave
            // 
            this.buttonManualBkpSave.Location = new System.Drawing.Point(6, 22);
            this.buttonManualBkpSave.Name = "buttonManualBkpSave";
            this.buttonManualBkpSave.Size = new System.Drawing.Size(100, 25);
            this.buttonManualBkpSave.TabIndex = 0;
            this.buttonManualBkpSave.Text = "Save File";
            this.buttonManualBkpSave.UseVisualStyleBackColor = true;
            this.buttonManualBkpSave.Click += new System.EventHandler(this.buttonManualBkpSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonManualBkpHeader);
            this.groupBox1.Controls.Add(this.buttonManualBkpSave);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(115, 87);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Manual Backup";
            // 
            // buttonManualBkpHeader
            // 
            this.buttonManualBkpHeader.Location = new System.Drawing.Point(6, 50);
            this.buttonManualBkpHeader.Name = "buttonManualBkpHeader";
            this.buttonManualBkpHeader.Size = new System.Drawing.Size(100, 25);
            this.buttonManualBkpHeader.TabIndex = 2;
            this.buttonManualBkpHeader.Text = "Header File";
            this.buttonManualBkpHeader.UseVisualStyleBackColor = true;
            this.buttonManualBkpHeader.Click += new System.EventHandler(this.buttonManualBkpHeader_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.labelPointType);
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Controls.Add(this.comboBoxHeaderEp);
            this.groupBox2.Controls.Add(this.buttonRewindCheckpoint);
            this.groupBox2.Controls.Add(this.buttonLoadHeader);
            this.groupBox2.Location = new System.Drawing.Point(133, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(178, 157);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Restart from Checkpoint";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox1.Image = global::savefiledecoder.Properties.Resources.Help;
            this.pictureBox1.Location = new System.Drawing.Point(80, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.comboBoxPoint);
            this.panel1.Location = new System.Drawing.Point(5, 80);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(169, 25);
            this.panel1.TabIndex = 4;
            // 
            // comboBoxPoint
            // 
            this.comboBoxPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPoint.Enabled = false;
            this.comboBoxPoint.FormattingEnabled = true;
            this.comboBoxPoint.Location = new System.Drawing.Point(2, 2);
            this.comboBoxPoint.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxPoint.Name = "comboBoxPoint";
            this.comboBoxPoint.Size = new System.Drawing.Size(165, 21);
            this.comboBoxPoint.TabIndex = 7;
            this.comboBoxPoint.SelectedIndexChanged += new System.EventHandler(this.comboBoxPoint_SelectedIndexChanged);
            // 
            // labelPointType
            // 
            this.labelPointType.AutoSize = true;
            this.labelPointType.Location = new System.Drawing.Point(7, 135);
            this.labelPointType.Name = "labelPointType";
            this.labelPointType.Size = new System.Drawing.Size(37, 13);
            this.labelPointType.TabIndex = 9;
            this.labelPointType.Text = "Type: ";
            this.labelPointType.Visible = false;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "dd MMMM yyyy";
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(6, 108);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(166, 20);
            this.dateTimePicker1.TabIndex = 8;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // comboBoxHeaderEp
            // 
            this.comboBoxHeaderEp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHeaderEp.Enabled = false;
            this.comboBoxHeaderEp.FormattingEnabled = true;
            this.comboBoxHeaderEp.Location = new System.Drawing.Point(6, 54);
            this.comboBoxHeaderEp.Name = "comboBoxHeaderEp";
            this.comboBoxHeaderEp.Size = new System.Drawing.Size(166, 21);
            this.comboBoxHeaderEp.TabIndex = 6;
            this.comboBoxHeaderEp.Tag = "";
            this.comboBoxHeaderEp.SelectedIndexChanged += new System.EventHandler(this.comboBoxHeaderEp_SelectedIndexChanged);
            // 
            // buttonRewindCheckpoint
            // 
            this.buttonRewindCheckpoint.Enabled = false;
            this.buttonRewindCheckpoint.Location = new System.Drawing.Point(112, 19);
            this.buttonRewindCheckpoint.Name = "buttonRewindCheckpoint";
            this.buttonRewindCheckpoint.Size = new System.Drawing.Size(60, 25);
            this.buttonRewindCheckpoint.TabIndex = 5;
            this.buttonRewindCheckpoint.Text = "Save";
            this.buttonRewindCheckpoint.UseVisualStyleBackColor = true;
            this.buttonRewindCheckpoint.Click += new System.EventHandler(this.buttonRewindCheckpoint_Click);
            // 
            // buttonLoadHeader
            // 
            this.buttonLoadHeader.Location = new System.Drawing.Point(6, 19);
            this.buttonLoadHeader.Name = "buttonLoadHeader";
            this.buttonLoadHeader.Size = new System.Drawing.Size(60, 25);
            this.buttonLoadHeader.TabIndex = 4;
            this.buttonLoadHeader.Text = "Load";
            this.buttonLoadHeader.UseVisualStyleBackColor = true;
            this.buttonLoadHeader.Click += new System.EventHandler(this.buttonLoadHeader_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "Save";
            this.saveFileDialog1.Filter = "Before the Storm save file|*.Save";
            this.saveFileDialog1.Title = "Backup the save file";
            // 
            // labelHeaderNotFound
            // 
            this.labelHeaderNotFound.AutoSize = true;
            this.labelHeaderNotFound.ForeColor = System.Drawing.Color.Red;
            this.labelHeaderNotFound.Location = new System.Drawing.Point(168, 172);
            this.labelHeaderNotFound.Name = "labelHeaderNotFound";
            this.labelHeaderNotFound.Size = new System.Drawing.Size(105, 26);
            this.labelHeaderNotFound.TabIndex = 4;
            this.labelHeaderNotFound.Text = "Header file was not \r\nfound near the save!";
            this.labelHeaderNotFound.Visible = false;
            // 
            // checkBoxSkipIntro
            // 
            this.checkBoxSkipIntro.AutoSize = true;
            this.checkBoxSkipIntro.Enabled = false;
            this.checkBoxSkipIntro.Location = new System.Drawing.Point(6, 43);
            this.checkBoxSkipIntro.Name = "checkBoxSkipIntro";
            this.checkBoxSkipIntro.Size = new System.Drawing.Size(71, 17);
            this.checkBoxSkipIntro.TabIndex = 5;
            this.checkBoxSkipIntro.Text = "Skip Intro";
            this.checkBoxSkipIntro.UseVisualStyleBackColor = true;
            this.checkBoxSkipIntro.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxSkipIntro_MouseUp);
            // 
            // checkBoxSkipCutscenes
            // 
            this.checkBoxSkipCutscenes.AutoSize = true;
            this.checkBoxSkipCutscenes.Enabled = false;
            this.checkBoxSkipCutscenes.Location = new System.Drawing.Point(6, 62);
            this.checkBoxSkipCutscenes.Name = "checkBoxSkipCutscenes";
            this.checkBoxSkipCutscenes.Size = new System.Drawing.Size(100, 30);
            this.checkBoxSkipCutscenes.TabIndex = 6;
            this.checkBoxSkipCutscenes.Text = "Skip Cutscenes\r\nin Normal Mode";
            this.checkBoxSkipCutscenes.UseVisualStyleBackColor = true;
            this.checkBoxSkipCutscenes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxSkipCutscenes_MouseUp);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonSaveDLL);
            this.groupBox3.Controls.Add(this.buttonLoadDLL);
            this.groupBox3.Controls.Add(this.checkBoxSkipIntro);
            this.groupBox3.Controls.Add(this.checkBoxSkipCutscenes);
            this.groupBox3.Location = new System.Drawing.Point(12, 100);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(115, 98);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "DLL Mods";
            // 
            // buttonSaveDLL
            // 
            this.buttonSaveDLL.Location = new System.Drawing.Point(61, 14);
            this.buttonSaveDLL.Name = "buttonSaveDLL";
            this.buttonSaveDLL.Size = new System.Drawing.Size(49, 25);
            this.buttonSaveDLL.TabIndex = 8;
            this.buttonSaveDLL.Text = "Save";
            this.buttonSaveDLL.UseVisualStyleBackColor = true;
            this.buttonSaveDLL.Click += new System.EventHandler(this.buttonSaveDLL_Click);
            // 
            // buttonLoadDLL
            // 
            this.buttonLoadDLL.Location = new System.Drawing.Point(6, 14);
            this.buttonLoadDLL.Name = "buttonLoadDLL";
            this.buttonLoadDLL.Size = new System.Drawing.Size(49, 25);
            this.buttonLoadDLL.TabIndex = 7;
            this.buttonLoadDLL.Text = "Load";
            this.buttonLoadDLL.UseVisualStyleBackColor = true;
            this.buttonLoadDLL.Click += new System.EventHandler(this.buttonLoadDLL_Click);
            // 
            // FormExtras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 203);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.labelHeaderNotFound);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormExtras";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Extras";
            this.Load += new System.EventHandler(this.FormExtras_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ComboBoxHeaderEp_SelectedValueChanged(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Button buttonManualBkpSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonManualBkpHeader;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonRewindCheckpoint;
        private System.Windows.Forms.Button buttonLoadHeader;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox comboBoxPoint;
        private System.Windows.Forms.ComboBox comboBoxHeaderEp;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label labelPointType;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelHeaderNotFound;
        private System.Windows.Forms.CheckBox checkBoxSkipIntro;
        private System.Windows.Forms.CheckBox checkBoxSkipCutscenes;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonSaveDLL;
        private System.Windows.Forms.Button buttonLoadDLL;
    }
}