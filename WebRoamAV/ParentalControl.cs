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
    public partial class ParentalControl : Form
    {
        public ParentalControl()
        {
            InitializeComponent();
        }
        string expath = "";
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox4.Enabled = checkBox5.Enabled = checkBox6.Enabled = checkBox1.Checked;
            button5.Enabled = checkBox4.Checked && checkBox4.Enabled;
            button1.Enabled = checkBox5.Checked && checkBox5.Enabled;
            button2.Enabled = checkBox6.Checked && checkBox6.Enabled;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            button5.Enabled = checkBox4.Checked;
            var pw = new pWebCategory();
            pw.ShowDialog();
            if (pw.DialogResult != false)
                button5.Enabled = false;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = checkBox5.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = checkBox6.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox7.Enabled = checkBox8.Enabled = checkBox2.Checked;
            button3.Enabled = checkBox7.Checked && checkBox7.Enabled;
            button4.Enabled = checkBox8.Checked && checkBox8.Enabled;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            button9.Enabled = checkBox3.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            button3.Enabled = checkBox7.Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            button4.Enabled = checkBox8.Checked;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new pInternetSchedule().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new pWebBlockList().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new pApplicationCategory().ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new pWebCategory().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new pApplicationBlockList().ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            new pPCSchedule().ShowDialog();
        }
    }
}
