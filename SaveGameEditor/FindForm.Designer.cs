namespace SaveGameEditor
{
    partial class FindForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindForm));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxStarts = new System.Windows.Forms.TextBox();
            this.textBoxContains = new System.Windows.Forms.TextBox();
            this.textBoxEnds = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonFind = new System.Windows.Forms.Button();
            this.buttonFindPrev = new System.Windows.Forms.Button();
            this.buttonFindNext = new System.Windows.Forms.Button();
            this.labelResultCount = new System.Windows.Forms.Label();
            this.checkBoxCase = new System.Windows.Forms.CheckBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find in:";
            // 
            // textBoxStarts
            // 
            this.textBoxStarts.Location = new System.Drawing.Point(77, 53);
            this.textBoxStarts.Name = "textBoxStarts";
            this.textBoxStarts.Size = new System.Drawing.Size(134, 20);
            this.textBoxStarts.TabIndex = 4;
            this.textBoxStarts.TextChanged += new System.EventHandler(this.textBoxes_TextChanged);
            // 
            // textBoxContains
            // 
            this.textBoxContains.Location = new System.Drawing.Point(77, 79);
            this.textBoxContains.Name = "textBoxContains";
            this.textBoxContains.Size = new System.Drawing.Size(134, 20);
            this.textBoxContains.TabIndex = 5;
            this.textBoxContains.TextChanged += new System.EventHandler(this.textBoxes_TextChanged);
            // 
            // textBoxEnds
            // 
            this.textBoxEnds.Location = new System.Drawing.Point(77, 105);
            this.textBoxEnds.Name = "textBoxEnds";
            this.textBoxEnds.Size = new System.Drawing.Size(134, 20);
            this.textBoxEnds.TabIndex = 6;
            this.textBoxEnds.TextChanged += new System.EventHandler(this.textBoxes_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Starts with:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Contains:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Ends with:";
            // 
            // buttonFind
            // 
            this.buttonFind.Enabled = false;
            this.buttonFind.Location = new System.Drawing.Point(81, 154);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(60, 25);
            this.buttonFind.TabIndex = 10;
            this.buttonFind.Text = "First";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // buttonFindPrev
            // 
            this.buttonFindPrev.Enabled = false;
            this.buttonFindPrev.Location = new System.Drawing.Point(11, 154);
            this.buttonFindPrev.Name = "buttonFindPrev";
            this.buttonFindPrev.Size = new System.Drawing.Size(60, 25);
            this.buttonFindPrev.TabIndex = 11;
            this.buttonFindPrev.Text = "Prev";
            this.buttonFindPrev.UseVisualStyleBackColor = true;
            this.buttonFindPrev.Click += new System.EventHandler(this.buttonFindPrev_Click);
            // 
            // buttonFindNext
            // 
            this.buttonFindNext.Enabled = false;
            this.buttonFindNext.Location = new System.Drawing.Point(151, 154);
            this.buttonFindNext.Name = "buttonFindNext";
            this.buttonFindNext.Size = new System.Drawing.Size(60, 25);
            this.buttonFindNext.TabIndex = 12;
            this.buttonFindNext.Text = "Next";
            this.buttonFindNext.UseVisualStyleBackColor = true;
            this.buttonFindNext.Click += new System.EventHandler(this.buttonFindNext_Click);
            // 
            // labelResultCount
            // 
            this.labelResultCount.Location = new System.Drawing.Point(11, 186);
            this.labelResultCount.Name = "labelResultCount";
            this.labelResultCount.Size = new System.Drawing.Size(200, 13);
            this.labelResultCount.TabIndex = 13;
            this.labelResultCount.Text = "Found <x> results";
            this.labelResultCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelResultCount.Visible = false;
            // 
            // checkBoxCase
            // 
            this.checkBoxCase.AutoSize = true;
            this.checkBoxCase.Location = new System.Drawing.Point(11, 131);
            this.checkBoxCase.Name = "checkBoxCase";
            this.checkBoxCase.Size = new System.Drawing.Size(96, 17);
            this.checkBoxCase.TabIndex = 14;
            this.checkBoxCase.Text = "Case Sensitive";
            this.checkBoxCase.UseVisualStyleBackColor = true;
            this.checkBoxCase.CheckedChanged += new System.EventHandler(this.checkBoxes_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(77, 30);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(53, 17);
            this.radioButton3.TabIndex = 3;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Floats";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(161, 7);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(50, 17);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Flags";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(77, 7);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(68, 17);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Variables";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(161, 30);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(50, 17);
            this.radioButton4.TabIndex = 15;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Items";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // FindForm
            // 
            this.AcceptButton = this.buttonFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 208);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.checkBoxCase);
            this.Controls.Add(this.labelResultCount);
            this.Controls.Add(this.buttonFindNext);
            this.Controls.Add(this.buttonFindPrev);
            this.Controls.Add(this.buttonFind);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxEnds);
            this.Controls.Add(this.textBoxContains);
            this.Controls.Add(this.textBoxStarts);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FindForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxStarts;
        private System.Windows.Forms.TextBox textBoxContains;
        private System.Windows.Forms.TextBox textBoxEnds;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.Button buttonFindPrev;
        private System.Windows.Forms.Button buttonFindNext;
        private System.Windows.Forms.Label labelResultCount;
        private System.Windows.Forms.CheckBox checkBoxCase;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton4;
    }
}