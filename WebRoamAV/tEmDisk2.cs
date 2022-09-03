using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class tEmDisk2 : Form
    {
        public tEmDisk2()
        {
            InitializeComponent();
        }

        private void tEmDisk2_Load(object sender, EventArgs e)
        {
            var drives = from d in DriveInfo.GetDrives() where d.DriveType == DriveType.Removable select d;
            foreach (var dr in drives)
                comboBox1.Items.Add(dr.Name);

            if (comboBox1.Items.Count > 1)
                comboBox1.Items.RemoveAt(0);
            var drives2 = from d in DriveInfo.GetDrives() where d.DriveType == DriveType.CDRom select d;
            foreach (var dr2 in drives2)
                comboBox2.Items.Add(dr2.Name);
            if (comboBox2.Items.Count > 1)
                comboBox2.Items.RemoveAt(0);

            comboBox2.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new tEmDisk3().ShowDialog();
            this.Close();
        }

        private void tEmDisk2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.U:
                        comboBox1.Focus();
                        break;
                    case Keys.D:
                        comboBox2.Focus();
                        break;
                }
            
            }
        }
    }
}
