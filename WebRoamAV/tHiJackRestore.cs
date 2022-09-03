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
    public partial class tHiJackRestore : Form
    {
        public tHiJackRestore()
        {
            InitializeComponent();
        }

        private void tHiJackRestore_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(true, "hi", "this is second", "this is third");
           // dataGridView1.Rows[0].SetValues(true, "hi");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
