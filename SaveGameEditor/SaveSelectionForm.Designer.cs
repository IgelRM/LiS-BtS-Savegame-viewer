namespace SaveGameEditor
{
    partial class SaveSelectionForm
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveSelectionForm));
            this.rbSlot1 = new System.Windows.Forms.RadioButton();
            this.rbSlot2 = new System.Windows.Forms.RadioButton();
            this.rbSlot3 = new System.Windows.Forms.RadioButton();
            this.cbSteamIds = new System.Windows.Forms.ComboBox();
            this.lblSteamId = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.gbActiveSlots = new System.Windows.Forms.GroupBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbActiveSlots.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbSlot1
            // 
            this.rbSlot1.AutoSize = true;
            this.rbSlot1.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.rbSlot1.Location = new System.Drawing.Point(12, 19);
            this.rbSlot1.Name = "rbSlot1";
            this.rbSlot1.Size = new System.Drawing.Size(52, 17);
            this.rbSlot1.TabIndex = 0;
            this.rbSlot1.TabStop = true;
            this.rbSlot1.Text = "Slot 1";
            this.rbSlot1.UseVisualStyleBackColor = true;
            this.rbSlot1.CheckedChanged += new System.EventHandler(this.rbSlot_CheckedChanged);
            // 
            // rbSlot2
            // 
            this.rbSlot2.AutoSize = true;
            this.rbSlot2.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.rbSlot2.Location = new System.Drawing.Point(12, 42);
            this.rbSlot2.Name = "rbSlot2";
            this.rbSlot2.Size = new System.Drawing.Size(52, 17);
            this.rbSlot2.TabIndex = 1;
            this.rbSlot2.TabStop = true;
            this.rbSlot2.Text = "Slot 2";
            this.rbSlot2.UseVisualStyleBackColor = true;
            this.rbSlot2.CheckedChanged += new System.EventHandler(this.rbSlot_CheckedChanged);
            // 
            // rbSlot3
            // 
            this.rbSlot3.AutoSize = true;
            this.rbSlot3.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.rbSlot3.Location = new System.Drawing.Point(12, 65);
            this.rbSlot3.Name = "rbSlot3";
            this.rbSlot3.Size = new System.Drawing.Size(52, 17);
            this.rbSlot3.TabIndex = 2;
            this.rbSlot3.TabStop = true;
            this.rbSlot3.Text = "Slot 3";
            this.rbSlot3.UseVisualStyleBackColor = true;
            this.rbSlot3.CheckedChanged += new System.EventHandler(this.rbSlot_CheckedChanged);
            // 
            // cbSteamIds
            // 
            this.cbSteamIds.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbSteamIds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSteamIds.FormattingEnabled = true;
            this.cbSteamIds.Location = new System.Drawing.Point(12, 25);
            this.cbSteamIds.Name = "cbSteamIds";
            this.cbSteamIds.Size = new System.Drawing.Size(127, 21);
            this.cbSteamIds.TabIndex = 3;
            this.cbSteamIds.SelectedIndexChanged += new System.EventHandler(this.cbSteamIds_SelectedIndexChanged);
            // 
            // lblSteamId
            // 
            this.lblSteamId.AutoSize = true;
            this.lblSteamId.Location = new System.Drawing.Point(12, 9);
            this.lblSteamId.Name = "lblSteamId";
            this.lblSteamId.Size = new System.Drawing.Size(51, 13);
            this.lblSteamId.TabIndex = 2;
            this.lblSteamId.Text = "SteamID:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 49);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(47, 13);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "<status>";
            // 
            // gbActiveSlots
            // 
            this.gbActiveSlots.Controls.Add(this.rbSlot1);
            this.gbActiveSlots.Controls.Add(this.rbSlot2);
            this.gbActiveSlots.Controls.Add(this.rbSlot3);
            this.gbActiveSlots.Location = new System.Drawing.Point(145, 12);
            this.gbActiveSlots.Name = "gbActiveSlots";
            this.gbActiveSlots.Size = new System.Drawing.Size(80, 90);
            this.gbActiveSlots.TabIndex = 5;
            this.gbActiveSlots.TabStop = false;
            this.gbActiveSlots.Text = "Active slot";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(69, 108);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(150, 108);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // SaveSelectionForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(237, 142);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.gbActiveSlots);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblSteamId);
            this.Controls.Add(this.cbSteamIds);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SaveSelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select save";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SaveSelectionForm_FormClosing);
            this.Load += new System.EventHandler(this.SaveSelectionForm_Load);
            this.gbActiveSlots.ResumeLayout(false);
            this.gbActiveSlots.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton rbSlot1;
        private System.Windows.Forms.RadioButton rbSlot2;
        private System.Windows.Forms.RadioButton rbSlot3;
        private System.Windows.Forms.ComboBox cbSteamIds;
        private System.Windows.Forms.Label lblSteamId;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox gbActiveSlots;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}