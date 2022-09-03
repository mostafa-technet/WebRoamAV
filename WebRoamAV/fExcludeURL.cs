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
    public partial class fExcludeURL : Form
    {
        public fExcludeURL()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(textBox1.Text, checkBox1.Checked);
            textBox1.Text = "";
            checkBox1.Checked = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow c in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.Remove(c);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
