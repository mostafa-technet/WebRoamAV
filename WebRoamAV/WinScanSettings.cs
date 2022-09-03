using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class WinScanSettings : Form
    {
        public WinScanSettings()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = radioButton4.Checked = true;
            checkBox1.Checked = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            { 
            string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
            IniFile inf = new IniFile(file_ini);

            //fill config of our system/AV with default values if the config file didn't exist
            if (!File.Exists(file_ini))
            {
                File.Create(file_ini);
            }
            inf.Write("SCAN_FILE_TYPE","EXECUTABLE", "SCAN_SETTINGS");
                inf.Write("SCAN_GET_BACKUP", checkBox1.Checked.ToString().ToUpper(), "SCAN_SETTINGS");
                string action;
                if(radioButton3.Checked)
                {
                    action = "DELETE";
                }
                else if(radioButton4.Checked)
                {
                    action = "REPAIR";
                }
                else
                {
                    action = "SKIP";
                }
                inf.Write("SCAN_SELECT_ACTION", action, "SCAN_SETTINGS");
            this.Close();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button5.Enabled = radioButton2.Checked;
        }
        /*
         *
         
[SCAN_SETTINGS]
SCAN_FILE_TYPE=EXECUTABLE|ALL
SCAN_MODE=DEFAULT|CUSTOM
SCAN_ARCHIVE_FILES=TRUE
SCAN_SELECT_ACTION=REPAIR|DELETE|SKIP
SCAN_GET_BACKUP=TRUE
SCAN_MAILBOX=QUICK|FULL
ARCHIVE_TYPES=ARJ,RAR,ZIP,SIS,MSEXPAND
ARCHIVE_SCAN_LEVEL=2

         * */
        private void button5_Click(object sender, EventArgs e)
        {
            var sitm = new SelectAction();
            sitm.ShowDialog();
        }

        private void WinScanSettings_Load(object sender, EventArgs e)
        {
            try
            {
                string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
                IniFile inf = new IniFile(file_ini);

                //fill config of our system/AV with default values if the config file didn't exist


                checkBox1.Checked = inf.Read("SCAN_GET_BACKUP", "SCAN_SETTINGS") == "TRUE";
                string action;
               
                action = inf.Read("SCAN_SELECT_ACTION", "SCAN_SETTINGS");
                if (action == "DELETE")
                {
                    radioButton3.Checked = true;
                }
                else if (action == "REPAIR")
                {
                    radioButton4.Checked = true;
                }
                else
                {
                    radioButton5.Checked = true;
                }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
    }
}
