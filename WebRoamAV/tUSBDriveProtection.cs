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
    public partial class tUSBDriveProtection : Form
    {
        public tUSBDriveProtection()
        {
            InitializeComponent();
        }

        private void tUSBDriveProtection_Load(object sender, EventArgs e)
        {
            var drives = from d in DriveInfo.GetDrives() where d.DriveType==DriveType.Removable select d;
            foreach (var dr in drives)
                comboBox1.Items.Add(dr.Name);
            comboBox1.SelectedIndex = 0;
            if (comboBox1.Items.Count > 1)
            {
                comboBox1.Items.RemoveAt(0);
                comboBox1.Enabled = true;
                button2.Enabled = true;
                comboBox1.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "&Secure Removable Drive")
            {
                string filename = "secureUSB.txt";
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                File.WriteAllText(filename, comboBox1.Text);
                button2.Text = "&Unsecure Removable Drive";
            }
            else
            {
                string filename = "secureUSB.txt";
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                button2.Text = "&Secure Removable Drive";
            }

        }
    }
}
