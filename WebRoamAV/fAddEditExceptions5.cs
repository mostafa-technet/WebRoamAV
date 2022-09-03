using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class fAddEditExceptions5 : Form
    {
        public fAddEditExceptions5()
        {
            InitializeComponent();
        }

        public fAddEditExceptions5(bool Isreadonly)
        {
            InitializeComponent();
            panel1.Enabled = !Isreadonly;
        }

        private void fAddEditExceptions5_Load(object sender, EventArgs e)
        {
            if (AdvanceSettings.strRule[4] == "")
                return;
            string[] fill = AdvanceSettings.strRule[2].Split(';');

            radioButton1.Checked = fill[0] == radioButton1.Text.Replace("&", "");
            radioButton2.Checked = fill[0] == radioButton2.Text.Replace("&", "");
            radioButton3.Checked = fill[0] == radioButton3.Text.Replace("&", "");
            if (!radioButton1.Checked)
                if (radioButton2.Checked)
                {
                    textBox1.Text = fill[1];
                }
                else
                {
                    textBox2.Text = fill[1];
                    textBox3.Text = fill[2];
                }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new fAddEditExceptions4().ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (panel1.Enabled)
            {
                if ((radioButton2.Checked && textBox1.Text.Replace(".", "").Trim() == "") || (radioButton3.Checked && (textBox2.Text.Replace(".", "").Trim() == "" || textBox3.Text.Replace(".", "").Trim() == "")))
                {
                    MessageBox.Show("Fill the form properly!");
                    return;
                }
                if (radioButton1.Checked)
                {
                    AdvanceSettings.strRule[4] = radioButton1.Text.Replace("&", "") + ";";
                }
                else if (radioButton2.Checked)
                {
                    AdvanceSettings.strRule[4] = radioButton2.Text.Replace("&", "") + ";" + textBox1.Text + ";";
                }
                else
                {
                    AdvanceSettings.strRule[4] = radioButton3.Text.Replace("&", "") + ";" + textBox2.Text + ";" + textBox3.Text + ";";
                }
            }
            this.Hide();
            new fAddEditExceptions6(!panel1.Enabled).ShowDialog();
            this.Close();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = radioButton2.Checked;
            textBox2.Enabled = radioButton3.Checked;
            textBox3.Enabled = radioButton3.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = radioButton2.Checked;
            textBox2.Enabled = radioButton3.Checked;
            textBox3.Enabled = radioButton3.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = radioButton2.Checked;
            textBox2.Enabled = radioButton3.Checked;
            textBox3.Enabled = radioButton3.Checked;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
