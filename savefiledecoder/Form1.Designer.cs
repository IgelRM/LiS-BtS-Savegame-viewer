namespace savefiledecoder
{
    partial class Form1
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
            this.textBoxSavePath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBoxRawJson = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelEpisodes = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxE4 = new System.Windows.Forms.CheckBox();
            this.checkBoxE1 = new System.Windows.Forms.CheckBox();
            this.checkBoxE2 = new System.Windows.Forms.CheckBox();
            this.checkBoxE3 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxLisPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanelEpisodes.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxSavePath
            // 
            this.textBoxSavePath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSavePath.Location = new System.Drawing.Point(121, 2);
            this.textBoxSavePath.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxSavePath.Name = "textBoxSavePath";
            this.textBoxSavePath.Size = new System.Drawing.Size(774, 20);
            this.textBoxSavePath.TabIndex = 0;
            this.textBoxSavePath.Text = "C:\\Users\\[User]\\AppData\\LocalLow\\Square Enix\\Life Is Strange_ Before The Storm\\Sa" +
    "ves\\[ID]\\SLOT_00\\Data.Save";
            this.textBoxSavePath.TextChanged += new System.EventHandler(this.textBoxSavePath_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(899, 66);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 27);
            this.button1.TabIndex = 1;
            this.button1.Text = "Show content";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label1.Size = new System.Drawing.Size(113, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Path to savefile (.data)";
            // 
            // tabControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 99);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1126, 514);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1118, 488);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Variables";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1112, 482);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBoxRawJson);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1118, 488);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "page2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBoxRawJson
            // 
            this.textBoxRawJson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRawJson.Location = new System.Drawing.Point(3, 3);
            this.textBoxRawJson.Multiline = true;
            this.textBoxRawJson.Name = "textBoxRawJson";
            this.textBoxRawJson.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxRawJson.Size = new System.Drawing.Size(1112, 482);
            this.textBoxRawJson.TabIndex = 0;
            this.textBoxRawJson.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanelEpisodes, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLisPath, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxSavePath, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.button3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.button4, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.button1, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1129, 616);
            this.tableLayoutPanel1.TabIndex = 5;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // tableLayoutPanelEpisodes
            // 
            this.tableLayoutPanelEpisodes.ColumnCount = 5;
            this.tableLayoutPanelEpisodes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEpisodes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEpisodes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEpisodes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEpisodes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEpisodes.Controls.Add(this.checkBoxE4, 3, 0);
            this.tableLayoutPanelEpisodes.Controls.Add(this.checkBoxE1, 0, 0);
            this.tableLayoutPanelEpisodes.Controls.Add(this.checkBoxE2, 1, 0);
            this.tableLayoutPanelEpisodes.Controls.Add(this.checkBoxE3, 2, 0);
            this.tableLayoutPanelEpisodes.Controls.Add(this.button2, 4, 0);
            this.tableLayoutPanelEpisodes.Location = new System.Drawing.Point(122, 67);
            this.tableLayoutPanelEpisodes.Name = "tableLayoutPanelEpisodes";
            this.tableLayoutPanelEpisodes.RowCount = 1;
            this.tableLayoutPanelEpisodes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEpisodes.Size = new System.Drawing.Size(772, 26);
            this.tableLayoutPanelEpisodes.TabIndex = 1;
            // 
            // checkBoxE4
            // 
            this.checkBoxE4.AutoSize = true;
            this.checkBoxE4.Enabled = false;
            this.checkBoxE4.Location = new System.Drawing.Point(240, 3);
            this.checkBoxE4.Name = "checkBoxE4";
            this.checkBoxE4.Size = new System.Drawing.Size(97, 17);
            this.checkBoxE4.TabIndex = 7;
            this.checkBoxE4.Text = "Bonus Episode";
            this.checkBoxE4.UseVisualStyleBackColor = true;
            this.checkBoxE4.CheckedChanged += new System.EventHandler(this.checkBoxEpisodes_CheckedChanged);
            // 
            // checkBoxE1
            // 
            this.checkBoxE1.AutoSize = true;
            this.checkBoxE1.Enabled = false;
            this.checkBoxE1.Location = new System.Drawing.Point(3, 3);
            this.checkBoxE1.Name = "checkBoxE1";
            this.checkBoxE1.Size = new System.Drawing.Size(73, 17);
            this.checkBoxE1.TabIndex = 1;
            this.checkBoxE1.Text = "Episode 1";
            this.checkBoxE1.UseVisualStyleBackColor = true;
            this.checkBoxE1.CheckedChanged += new System.EventHandler(this.checkBoxEpisodes_CheckedChanged);
            // 
            // checkBoxE2
            // 
            this.checkBoxE2.AutoSize = true;
            this.checkBoxE2.Enabled = false;
            this.checkBoxE2.Location = new System.Drawing.Point(82, 3);
            this.checkBoxE2.Name = "checkBoxE2";
            this.checkBoxE2.Size = new System.Drawing.Size(73, 17);
            this.checkBoxE2.TabIndex = 5;
            this.checkBoxE2.Text = "Episode 2";
            this.checkBoxE2.UseVisualStyleBackColor = true;
            this.checkBoxE2.CheckedChanged += new System.EventHandler(this.checkBoxEpisodes_CheckedChanged);
            // 
            // checkBoxE3
            // 
            this.checkBoxE3.AutoSize = true;
            this.checkBoxE3.Enabled = false;
            this.checkBoxE3.Location = new System.Drawing.Point(161, 3);
            this.checkBoxE3.Name = "checkBoxE3";
            this.checkBoxE3.Size = new System.Drawing.Size(73, 17);
            this.checkBoxE3.TabIndex = 6;
            this.checkBoxE3.Text = "Episode 3";
            this.checkBoxE3.UseVisualStyleBackColor = true;
            this.checkBoxE3.CheckedChanged += new System.EventHandler(this.checkBoxEpisodes_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(343, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(103, 20);
            this.button2.TabIndex = 5;
            this.button2.Text = "Export";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxLisPath
            // 
            this.textBoxLisPath.Location = new System.Drawing.Point(122, 35);
            this.textBoxLisPath.Name = "textBoxLisPath";
            this.textBoxLisPath.Size = new System.Drawing.Size(771, 20);
            this.textBoxLisPath.TabIndex = 2;
            this.textBoxLisPath.Text = "C:\\Program Files (x86)\\Steam\\SteamApps\\common\\Life is Strange - Before the Storm\\" +
    "";
            this.textBoxLisPath.TextChanged += new System.EventHandler(this.textBoxLisPath_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 64);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label3.Size = new System.Drawing.Size(105, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "Show variables from:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label2.Size = new System.Drawing.Size(79, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Path to LiS BtS";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(900, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(103, 26);
            this.button3.TabIndex = 6;
            this.button3.Text = "Open file";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(900, 35);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(103, 26);
            this.button4.TabIndex = 7;
            this.button4.Text = "Open file";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 616);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "LiS BtS Save File Viewer Version 0.2";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanelEpisodes.ResumeLayout(false);
            this.tableLayoutPanelEpisodes.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSavePath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBoxRawJson;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBoxLisPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEpisodes;
        private System.Windows.Forms.CheckBox checkBoxE4;
        private System.Windows.Forms.CheckBox checkBoxE1;
        private System.Windows.Forms.CheckBox checkBoxE2;
        private System.Windows.Forms.CheckBox checkBoxE3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

