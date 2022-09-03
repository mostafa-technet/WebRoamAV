using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class WindowsSpyDetails : Form
    {
        Process mPr;
        public WindowsSpyDetails(Process p)
        {
            InitializeComponent();
            mPr = p;
        }

        private void WindowsSpyDetails_Load(object sender, EventArgs e)
        {
            label2.Text = mPr.ProcessName;
            dataGridView1.Rows.Add("Application Path", mPr.MainModule.FileName);
            dataGridView1.Rows.Add("Application Name", mPr.ProcessName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mPr.Kill();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
