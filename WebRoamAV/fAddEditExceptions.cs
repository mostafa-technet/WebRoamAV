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
    public partial class fAddEditExceptions : Form
    {
        public fAddEditExceptions()
        {
            InitializeComponent();
        }
        public fAddEditExceptions(bool Isreadonly)
        {
            InitializeComponent();
            textBox1.ReadOnly = Isreadonly;
            panel1.Enabled = !Isreadonly;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (panel1.Enabled)
            {
                if ((!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked) || textBox1.Text == "")
                {
                    MessageBox.Show("You need to fill the form completely!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                AdvanceSettings.strRule[0] = textBox1.Text + ";" + (radioButton5.Checked ? "Out" : "In") + ";" + (radioButton1.Checked ? radioButton1.Text.Replace("&", "") : (radioButton2.Checked ? radioButton2.Text.Replace("&", "") : radioButton3.Text.Replace("&", "")));
            }
            this.Hide();
            new fAddEditExceptions2(!panel1.Enabled).ShowDialog();
            this.Close();
        }

        private void fAddEditExceptions_Load(object sender, EventArgs e)
        {
            if (AdvanceSettings.strRule[0] == "")
                return;
            string[] fill = AdvanceSettings.strRule[0].Split(';');

            textBox1.Text = fill[0];
            radioButton5.Checked = fill[1] == "Out";
            radioButton1.Checked = fill[2] == radioButton1.Text.Replace("&", "");
            radioButton2.Checked = fill[2] == radioButton2.Text.Replace("&", "");            
            radioButton3.Checked = fill[2] == radioButton3.Text.Replace("&", "");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
