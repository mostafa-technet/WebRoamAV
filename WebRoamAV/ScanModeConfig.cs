using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace WebRoamAV
{
    public partial class ScanModeConfig : Form
    {
        public ScanModeConfig()
        { 
           

            InitializeComponent();
        }

        protected new void TabStopChanged(object sender, EventArgs e)
        {
            ((RadioButton)sender).TabStop = true;
        }
        private void ScanModeConfig_Load(object sender, EventArgs e)
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

            IDS_OPT_SCNSTDLG_SELECT_ITEMS_TO_SCAN.Text = WebRoamAV.Resources.res2.IDS_OPT_SCNSTDLG_SELECT_ITEMS_TO_SCAN;
            IDS_OPT_SCNSTDLG_SCAN_EXECUTABLES.Text = WebRoamAV.Resources.res2.IDS_OPT_SCNSTDLG_SCAN_EXECUTABLES;
            IDS_OPT_SCNSTDLG_SCAN_ALL.Text = WebRoamAV.Resources.res2.IDS_OPT_SCNSTDLG_SCAN_ALL;
         //   IDS_OPT_SCNSTDLG_SCAN_ARCHIVE.Text = WebRoamAV.Resources.res2.IDS_OPT_SCNSTDLG_SCAN_ARCHIVE;
            IDS_EMLPROSET_CONFIGURE.Text = WebRoamAV.Resources.res2.IDS_EMLPROSET_CONFIGURE;
            IDS_OPT_SCNSTDLG_SCAN_ARCHIVE.Text = WebRoamAV.Resources.res2.IDS_OPT_SCNSTDLG_SCAN_ARCHIVE;
            IDS_OPT_SCNSTDLG_SCAN_PACKED.Text = WebRoamAV.Resources.res2.IDS_OPT_SCNSTDLG_SCAN_PACKED;
            IDS_OPT_SCNSTDLG_SCAN_MAILBOXES.Text = WebRoamAV.Resources.res2.IDS_OPT_SCNSTDLG_SCAN_MAILBOXES;
            IDS_OPT_SCNSTDLG_SCAN_MAILBOX_QUICK_SCAN.Text = WebRoamAV.Resources.res2.IDS_OPT_SCNSTDLG_SCAN_MAILBOX_QUICK_SCAN;
            IDS_OPT_SCNSTDLG_SCAN_MAILBOX_THROUGH_SCAN.Text = WebRoamAV.Resources.res2.IDS_OPT_SCNSTDLG_SCAN_MAILBOX_THROUGH_SCAN;
            IDS_EXCLUSION_BTN_OK.Text = WebRoamAV.Resources.res2.IDS_EXCLUSION_BTN_OK;
            IDS_ONLINEEXE_ONLNALRT_BTN_CANCEL.Text = WebRoamAV.Resources.res2.IDS_ONLINEEXE_ONLNALRT_BTN_CANCEL;
            IDS_DEFAULT.Text = WebRoamAV.Resources.res2.IDS_DEFAULT;
            IDS_ECD_HELP.Text = WebRoamAV.Resources.res2.IDS_ECD_HELP;
           
            try
            {
                string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
                IniFile inf = new IniFile(file_ini);

                //fill config of our system/AV with default values if the config file didn't exist

                IDS_OPT_SCNSTDLG_SCAN_EXECUTABLES.Checked = inf.Read("SCAN_FILE_TYPE",  "SCAN_SETTINGS").ToUpper() == "EXECUTABLE" ? true : false;
                IDS_OPT_SCNSTDLG_SCAN_ARCHIVE.Checked = inf.Read("SCAN_ARCHIVE_FILES", "SCAN_SETTINGS").ToUpperInvariant() == "TRUE" ? true : false;
                IDS_OPT_SCNSTDLG_SCAN_PACKED.Checked = inf.Read("SCAN_PACKED_FILES", "SCAN_SETTINGS") == "TRUE" ? true : false;
                string mailscan;
                mailscan = inf.Read("SCAN_MAILBOX", "SCAN_SETTINGS");
                if (mailscan == "NONE")
                {
                    
                    IDS_OPT_SCNSTDLG_SCAN_MAILBOXES.Checked = IDS_OPT_SCNSTDLG_SCAN_MAILBOX_QUICK_SCAN.Checked = false;
                }
                else if (mailscan == "QUICK")
                {
                    IDS_OPT_SCNSTDLG_SCAN_MAILBOX_QUICK_SCAN.Checked = true;
                }
                else
                {
                    IDS_OPT_SCNSTDLG_SCAN_ALL.Checked = true;
                }
                
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }


        }

        private void IDS_EMLPROSET_CONFIGURE_Click(object sender, EventArgs e)
        {
            var sit = new WinScanSettings();
            sit.ShowDialog();

        }

        private void IDS_ONLINEEXE_ONLNALRT_BTN_CANCEL_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void IDS_DEFAULT_Click(object sender, EventArgs e)
        {
            IDS_OPT_SCNSTDLG_SCAN_EXECUTABLES.Checked = IDS_OPT_SCNSTDLG_SCAN_ARCHIVE.Checked = IDS_OPT_SCNSTDLG_SCAN_MAILBOX_QUICK_SCAN.Checked
                = IDS_OPT_SCNSTDLG_SCAN_PACKED.Checked = IDS_OPT_SCNSTDLG_SCAN_MAILBOXES.Checked
                = true;           
        }

        private void IDS_ECD_HELP_Click(object sender, EventArgs e)
        {

        }

        /*
         * [SCAN_SETTINGS]
SCAN_FILE_TYPE=EXECUTABLE|ALL
SCAN_MODE=DEFAULT|CUSTOM
SCAN_ARCHIVE_FILES=TRUE
SCAN_PACKED_FILES=TRUE
SCAN_SELECT_ACTION=REPAIR|DELETE|SKIP
SCAN_GET_BACKUP=TRUE
SCAN_MAILBOX=QUICK|FULL|NONE        
ARCHIVE_TYPES=ARJ,RAR,ZIP,SIS,MSEXPAND
ARCHIVE_SCAN_LEVEL=2

         * */

        private void IDS_EXCLUSION_BTN_OK_Click(object sender, EventArgs e)
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
                inf.Write("SCAN_FILE_TYPE", IDS_OPT_SCNSTDLG_SCAN_EXECUTABLES.Checked ? "EXECUTABLE":"ALL", "SCAN_SETTINGS");
            inf.Write("SCAN_ARCHIVE_FILES", IDS_OPT_SCNSTDLG_SCAN_ARCHIVE.Checked.ToString().ToUpperInvariant(), "SCAN_SETTINGS");
            inf.Write("SCAN_PACKED_FILES", IDS_OPT_SCNSTDLG_SCAN_PACKED.Checked.ToString().ToUpperInvariant(), "SCAN_SETTINGS");
            string mailscan;
            if(!IDS_OPT_SCNSTDLG_SCAN_MAILBOXES.Checked)
            {
                mailscan = "NONE";
            }
            else if(IDS_OPT_SCNSTDLG_SCAN_MAILBOX_QUICK_SCAN.Checked)
            {
                mailscan = "QUICK";
            }
            else
            {
                mailscan = "FULL";
            }
            inf.Write("SCAN_MAILBOX", mailscan, "SCAN_SETTINGS");
            this.Close();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
    }
}
