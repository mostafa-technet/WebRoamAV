using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class fAddEditExceptions4 : Form
    {
        public fAddEditExceptions4()
        {
            InitializeComponent();
        }
        public fAddEditExceptions4(bool Isreadonly)
        {
            InitializeComponent();
            panel1.Enabled = !Isreadonly;
        }

        private void mtbIP_Leave(object sender, EventArgs e)
        {
            IPAddress iad;
            if (!IPAddress.TryParse(mtbIP.Text.Replace(" ", ""), out iad))
            {
                //                System.Windows.MessageBox.Show("Bad IP address!!");
                //              mtbIP.Focus();
            }
        }
        string s = "";
        int pd = 0;
        private void mtbIP_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            int k = e.KeyValue;

            if (k == 110 || k == 190)
            {
                s = mtbIP.Text;
                char ch = s.LastOrDefault(f => char.IsDigit(f));
                int dig = s.LastIndexOf(ch);
                pd += 3;

                if (dig > 0)
                {
                    pd = (s.Substring(0, dig)).LastIndexOf('.') + 4 + 1;
                }
                mtbIP.Select(pd, 1);
            }

        }

        private void mtbIP_GotFocus(object sender, EventArgs e)
        {

            mtbIP.Select(0, 1);
        }

        private void mtbIP2_Leave(object sender, EventArgs e)
        {
            IPAddress iad;
            if (!IPAddress.TryParse(mtbIP2.Text.Replace(" ", ""), out iad))
            {
                //                System.Windows.MessageBox.Show("Bad IP address!!");
                //              mtbIP2.Focus();
            }
        }
        string s2 = "";
        int pd2 = 0;
        private void mtbIP2_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            int k = e.KeyValue;

            if (k == 110 || k == 190)
            {
                s = mtbIP2.Text;
                char ch = s.LastOrDefault(f => char.IsDigit(f));
                int dig = s.LastIndexOf(ch);
                pd += 3;

                if (dig > 0)
                {
                    pd = (s.Substring(0, dig)).LastIndexOf('.') + 4 + 1;
                }
                mtbIP2.Select(pd, 1);
            }

        }

        private void mtbIP2_GotFocus(object sender, EventArgs e)
        {

            mtbIP2.Select(0, 1);
        }
        private void mtbIP3_Leave(object sender, EventArgs e)
        {
            IPAddress iad;
            if (!IPAddress.TryParse(mtbIP3.Text.Replace(" ", ""), out iad))
            {
                //                System.Windows.MessageBox.Show("Bad IP address!!");
                //              mtbIP3.Focus();
            }
        }
        string s3 = "";
        int pd3 = 0;
        private void mtbIP3_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            int k = e.KeyValue;

            if (k == 110 || k == 190)
            {
                s = mtbIP3.Text;
                char ch = s.LastOrDefault(f => char.IsDigit(f));
                int dig = s.LastIndexOf(ch);
                pd += 3;

                if (dig > 0)
                {
                    pd = (s.Substring(0, dig)).LastIndexOf('.') + 4 + 1;
                }
                mtbIP3.Select(pd, 1);
            }

        }

        private void mtbIP3_GotFocus(object sender, EventArgs e)
        {

            mtbIP3.Select(0, 1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fAddEditExceptions4_Load(object sender, EventArgs e)
        {
            if (AdvanceSettings.strRule[3] == "")
                return;
            string[] fill = AdvanceSettings.strRule[3].Split(';');

            radioButton1.Checked = fill[0] == radioButton1.Text.Replace("&", "");
            radioButton2.Checked = fill[0] == radioButton2.Text.Replace("&", "");
            radioButton3.Checked = fill[0] == radioButton3.Text.Replace("&", "");
            if (!radioButton1.Checked)
                if (radioButton2.Checked)
                {
                    mtbIP.Text = fill[1];
                }
                else
                {
                    mtbIP2.Text = fill[1];
                    mtbIP3.Text = fill[2];
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (panel1.Enabled)
            {
                if ((radioButton2.Checked && mtbIP.Text.Replace(".", "").Trim() == "") || (radioButton3.Checked && (mtbIP2.Text.Replace(".", "").Trim() == "" || mtbIP3.Text.Replace(".", "").Trim() == "")))
                {
                    MessageBox.Show("Fill the form properly!");
                    return;
                }
                if (radioButton1.Checked)
                {
                    AdvanceSettings.strRule[3] = radioButton1.Text.Replace("&", "") + ";";
                }
                else if (radioButton2.Checked)
                {
                    AdvanceSettings.strRule[3] = radioButton2.Text.Replace("&", "") + ";" + mtbIP.Text + ";";
                }
                else
                {
                    AdvanceSettings.strRule[3] = radioButton3.Text.Replace("&", "") + ";" + mtbIP2.Text + ";" + mtbIP3.Text + ";";
                }
            }
            this.Hide();
            new fAddEditExceptions5(!panel1.Enabled).ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new fAddEditExceptions3().ShowDialog();
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            mtbIP.Enabled = radioButton2.Checked;
            mtbIP2.Enabled = radioButton3.Checked;
            mtbIP3.Enabled = radioButton3.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            mtbIP.Enabled = radioButton2.Checked;
            mtbIP2.Enabled = radioButton3.Checked;
            mtbIP3.Enabled = radioButton3.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            mtbIP.Enabled = radioButton2.Checked;
            mtbIP2.Enabled = radioButton3.Checked;
            mtbIP3.Enabled = radioButton3.Checked;
        }
    }
}
