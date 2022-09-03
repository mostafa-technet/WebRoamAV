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
    public partial class ScanItem : Form
    {
        public ScanItem()
        {
            InitializeComponent();
        }
        protected new void TabStopChanged(object sender, EventArgs e)
        {
            ((RadioButton)sender).TabStop = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = radioButton4.Checked = true;
            checkBox1.Checked = checkBox2.Checked = checkBox3.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
            IniFile inf = new IniFile(file_ini);

            //fill config of our system/AV with default values if the config file didn't exist
            if (!File.Exists(file_ini))
            {
                inf.Write("IsAutoUpdateOn", "True", "SCANNER");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            var sa = new SelectAction();
            sa.ShowDialog();
        }

        private void ScanItem_Load(object sender, EventArgs e)
        {
            foreach (var item in this.groupBox1.Controls)
            {
                if (item.GetType() == typeof(RadioButton))
                {
                    ((RadioButton)item).TabStop = true;
                    ((RadioButton)item).TabStopChanged += new System.EventHandler(TabStopChanged);
                }
            }
            foreach (var item in this.groupBox2.Controls)
            {
                if (item.GetType() == typeof(RadioButton))
                {
                    ((RadioButton)item).TabStop = true;
                    ((RadioButton)item).TabStopChanged += new System.EventHandler(TabStopChanged);
                }
            }
        }
    }
}
