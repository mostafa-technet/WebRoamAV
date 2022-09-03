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
    public partial class SelectAction : Form
    {
        public SelectAction()
        {
            InitializeComponent();
        }
        protected new void TabStopChanged(object sender, EventArgs e)
        {
            ((RadioButton)sender).TabStop = true;
        }
        private void SelectAction_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in this.Controls)
                {
                    if (item.GetType() == typeof(RadioButton))
                    {
                        ((RadioButton)item).TabStop = true;
                        ((RadioButton)item).TabStopChanged += new System.EventHandler(TabStopChanged);
                    }
                }
              
                try
                {
                    string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
                    IniFile inf = new IniFile(file_ini);

                                        
                    string action;
                    
                    action = inf.Read("SCAN_SELECT_ACTION", "SCAN_SETTINGS");
                    comboBox1.Text = inf.Read("ARCHIVE_SCAN_LEVEL", "SCAN_SETTINGS");                    
                    string[] items = inf.Read("ARCHIVE_TYPES", "SCAN_SETTINGS").Split(',');
                    foreach(var format in items)
                    {
                        for (int i = 0; i < checkedListBox1.Items.Count; i++)
                        {
                            if (checkedListBox1.GetItemText(checkedListBox1.Items[i]) == format)
                            {
                                checkedListBox1.SetItemChecked(i, true);
                                break;
                            }
                        }
                    }
                    
                }
                catch (Exception em)
                {
                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                }
            }
            catch(Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for(int i=0;i<checkedListBox1.Items.Count;i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button6_Click(sender, e);
            checkedListBox1.SetItemChecked(0, true);
            checkedListBox1.SetItemChecked(4, true);
            checkedListBox1.SetItemChecked(7, true);
            checkedListBox1.SetItemChecked(8, true);
            checkedListBox1.SetItemChecked(9, true);
            comboBox1.SelectedIndex = 1;
            numericUpDown1.Value = 100;
            radioButton3.Checked = true;
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
            inf.Write("SCAN_FILE_TYPE", "EXECUTABLE", "SCAN_SETTINGS");
                string action;
                if (radioButton1.Checked)
                {
                    action = "DELETE";
                }
                else if (radioButton2.Checked)
                {
                    action = "REPAIR";
                }
                else
                {
                    action = "SKIP";
                }
                inf.Write("SCAN_SELECT_ACTION", action, "SCAN_SETTINGS");
                inf.Write("ARCHIVE_SCAN_LEVEL", comboBox1.Text, "SCAN_SETTINGS");
                inf.Write("OnlineProtMaxSize", numericUpDown1.Value.ToString(), "SCAN_SETTINGS");
                string formats = String.Join(",", checkedListBox1.CheckedItems.Cast<string>().ToArray());
           // MessageBox.Show(checkedListBox1.CheckedItems.Count.ToString());
                inf.Write("ARCHIVE_TYPES", formats, "SCAN_SETTINGS");
            this.Close();
            }
            catch(Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
    }
}
