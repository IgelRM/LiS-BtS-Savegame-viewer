using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaveGameEditor
{
    public partial class FindForm : Form
    {
        public FindForm()
        {
            InitializeComponent();
        }

        public MainForm form1;
        public int tab_num;

        private void buttonFind_Click(object sender, EventArgs e)
        {
            form1.find_results.Clear();
            form1.ResetFindStrings();

            if (!String.IsNullOrWhiteSpace(textBoxStarts.Text))
            {
                form1.find_Starts = textBoxStarts.Text;
            }
            if (!String.IsNullOrWhiteSpace(textBoxContains.Text))
            {
                form1.find_Contains = textBoxContains.Text;
            }
            if (!String.IsNullOrWhiteSpace(textBoxEnds.Text))
            {
                form1.find_Ends = textBoxEnds.Text;
            }

            form1.FindFirst(tab_num, checkBoxCase.Checked);
            int res_count = form1.find_results.Count;

            if (res_count == 0)
            {
                labelResultCount.Visible = false;
                MessageBox.Show("Can not find any cells! Try a different table or search terms.");
            }
            else if (res_count == 1)
            {
                labelResultCount.Visible = true;
                labelResultCount.Text = "Found 1 result.";
            }
            else
            {
                labelResultCount.Visible = true;
                labelResultCount.Text = "Found " + res_count + " results.";
                buttonFindPrev.Enabled = true;
                buttonFindNext.Enabled = true;
                this.AcceptButton = buttonFindNext;
            }
        }

        private void buttonFindPrev_Click(object sender, EventArgs e)
        {
            form1.FindPrev(tab_num);
        }

        private void buttonFindNext_Click(object sender, EventArgs e)
        {
            form1.FindNext(tab_num);
        }

        private void textBoxes_TextChanged (object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(textBoxStarts.Text) || !String.IsNullOrWhiteSpace(textBoxContains.Text) || !String.IsNullOrWhiteSpace(textBoxEnds.Text))
            {
                buttonFind.Enabled = true;
                
            }
            else
            {
                buttonFind.Enabled = false;
            }

            ResetSearchState();
        } 

        private void radioButtons_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) tab_num = 0;
            else if (radioButton2.Checked) tab_num = 1;
            else if (radioButton3.Checked) tab_num = 2;
            else if (radioButton4.Checked) tab_num = 3;

            ResetSearchState();
        }

        public void UpdateRadioChoice()
        {
            if (tab_num == 0) radioButton1.Checked = true;
            else if (tab_num == 1) radioButton2.Checked = true;
            else if (tab_num == 2) radioButton3.Checked = true;
            else if (tab_num == 3) radioButton4.Checked = true;
        }

        private void FindForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            SendKeys.Send("{ESC}"); //fix against the cell's contents being selected
        }

        private void checkBoxes_CheckedChanged(object sender, EventArgs e)
        {
            ResetSearchState();
        }

        private void ResetSearchState ()
        {
            buttonFindPrev.Enabled = false;
            buttonFindNext.Enabled = false;
            labelResultCount.Visible = false;
            this.AcceptButton = buttonFind;
        }
    }
}
