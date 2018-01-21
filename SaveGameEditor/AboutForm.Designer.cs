namespace SaveGameEditor
{
    partial class AboutForm
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
            this.btnOk = new System.Windows.Forms.Button();
            this.lblCurrentVersion = new System.Windows.Forms.Label();
            this.lblCredits1 = new System.Windows.Forms.Label();
            this.llProjectHomepage = new System.Windows.Forms.LinkLabel();
            this.lblCredits2 = new System.Windows.Forms.Label();
            this.llNewVersionIsAvailable = new System.Windows.Forms.LinkLabel();
            this.lblNoUpdatesWereFound = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(285, 165);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // lblCurrentVersion
            // 
            this.lblCurrentVersion.AutoSize = true;
            this.lblCurrentVersion.Location = new System.Drawing.Point(28, 18);
            this.lblCurrentVersion.Name = "lblCurrentVersion";
            this.lblCurrentVersion.Size = new System.Drawing.Size(53, 13);
            this.lblCurrentVersion.TabIndex = 1;
            this.lblCurrentVersion.Text = "<version>";
            // 
            // lblCredits1
            // 
            this.lblCredits1.AutoSize = true;
            this.lblCredits1.Location = new System.Drawing.Point(28, 77);
            this.lblCredits1.Name = "lblCredits1";
            this.lblCredits1.Size = new System.Drawing.Size(157, 13);
            this.lblCredits1.TabIndex = 2;
            this.lblCredits1.Text = "Initially created by /u/DanielWe";
            // 
            // llProjectHomepage
            // 
            this.llProjectHomepage.AutoSize = true;
            this.llProjectHomepage.Location = new System.Drawing.Point(28, 133);
            this.llProjectHomepage.Name = "llProjectHomepage";
            this.llProjectHomepage.Size = new System.Drawing.Size(69, 13);
            this.llProjectHomepage.TabIndex = 3;
            this.llProjectHomepage.TabStop = true;
            this.llProjectHomepage.Text = "<homepage>";
            this.llProjectHomepage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDownload_LinkClicked);
            // 
            // lblCredits2
            // 
            this.lblCredits2.AutoSize = true;
            this.lblCredits2.Location = new System.Drawing.Point(28, 100);
            this.lblCredits2.Name = "lblCredits2";
            this.lblCredits2.Size = new System.Drawing.Size(238, 13);
            this.lblCredits2.TabIndex = 4;
            this.lblCredits2.Text = "Modified by Ladosha, IgelRM and VakhtinAndrey";
            // 
            // llNewVersionIsAvailable
            // 
            this.llNewVersionIsAvailable.AutoSize = true;
            this.llNewVersionIsAvailable.Location = new System.Drawing.Point(28, 41);
            this.llNewVersionIsAvailable.Name = "llNewVersionIsAvailable";
            this.llNewVersionIsAvailable.Size = new System.Drawing.Size(73, 13);
            this.llNewVersionIsAvailable.TabIndex = 6;
            this.llNewVersionIsAvailable.TabStop = true;
            this.llNewVersionIsAvailable.Text = "<newversion>";
            this.llNewVersionIsAvailable.Visible = false;
            this.llNewVersionIsAvailable.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llNewVersionIsAvailable_LinkClicked);
            // 
            // lblNoUpdatesWereFound
            // 
            this.lblNoUpdatesWereFound.AutoSize = true;
            this.lblNoUpdatesWereFound.Location = new System.Drawing.Point(28, 41);
            this.lblNoUpdatesWereFound.Name = "lblNoUpdatesWereFound";
            this.lblNoUpdatesWereFound.Size = new System.Drawing.Size(118, 13);
            this.lblNoUpdatesWereFound.TabIndex = 7;
            this.lblNoUpdatesWereFound.Text = "No updates were found";
            this.lblNoUpdatesWereFound.Visible = false;
            // 
            // AboutForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 200);
            this.Controls.Add(this.lblNoUpdatesWereFound);
            this.Controls.Add(this.llNewVersionIsAvailable);
            this.Controls.Add(this.lblCredits2);
            this.Controls.Add(this.llProjectHomepage);
            this.Controls.Add(this.lblCredits1);
            this.Controls.Add(this.lblCurrentVersion);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About Program";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblCurrentVersion;
        private System.Windows.Forms.Label lblCredits1;
        private System.Windows.Forms.LinkLabel llProjectHomepage;
        private System.Windows.Forms.Label lblCredits2;
        private System.Windows.Forms.LinkLabel llNewVersionIsAvailable;
        private System.Windows.Forms.Label lblNoUpdatesWereFound;
    }
}