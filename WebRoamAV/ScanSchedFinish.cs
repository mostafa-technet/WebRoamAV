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
    public partial class ScanSchedFinish : Form
    {
        public ScanSchedFinish()
        {
            InitializeComponent();
        }
        protected new void TabStopChanged(object sender, EventArgs e)
        {
            ((RadioButton)sender).TabStop = true;
        }
        private void ScanSchedFinish_Load(object sender, EventArgs e)
        {
            try { 
            string scanname = WScanSchedule.SchItems["F1_textBox1"].ToString();
            string freq = WScanSchedule.SchItems["F1_comboBox1"].ToString()=="0"?"Daily":"Weekly";
            string scansched = DateTime.Now.ToString();
            string selectfd = WScanSchedule.SchItems["F2_text"].ToString();
            string repeat = WScanSchedule.SchItems["F1_checkBox2"].ToString() != "TRUE" ? "Repeat":"No Repeat";
            string startt = WScanSchedule.SchItems["F1_dateTimePicker1"].ToString();
            string text = String.Format(
                "Scan Name:\t{0}\r\n\r\nFrequency:\t{1}\r\n\r\nScan Scheduled On:\t{2}\r\n\r\nSelected Folder & Drives:\t{3}\r\n\r\nRepeat Every:\t{4}\r\n\r\nStart Time:\t{5}",
               scanname, freq, scansched, selectfd, repeat, startt);
            textBox1.Text = text;
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try {
                string v0 = (Int32.Parse(WScanSchedule.SchItems["F1_comboBox2"].ToString()) + 100 * Int32.Parse(WScanSchedule.SchItems["F1_comboBox1"].ToString())).ToString();  
                
                string v1 = WScanSchedule.SchItems["F1_radioButton3"].ToString() != "TRUE" ? WScanSchedule.SchItems["F1_dateTimePicker1"].ToString() + "," + WScanSchedule.SchItems["F1_numericUpDown1"] + "," : "";
                string v2 = WScanSchedule.SchItems["F1_checkBox2"].ToString() == "TRUE" ? (100000 + 10000 * Int32.Parse(WScanSchedule.SchItems["F1_comboBox3"].ToString()) + Int32.Parse(WScanSchedule.SchItems["F1_numericUpDown2"].ToString())).ToString() : WScanSchedule.SchItems["F1_numericUpDown2"].ToString();
                string v3 = WScanSchedule.SchItems["F1_radioButton1"].ToString() == "TRUE" ? "1" : "0";
                string v4 = WScanSchedule.SchItems["F1_textBox2"].ToString();
                string v5 = WScanSchedule.SchItems["F1_textBox3"].ToString();
                string v6 = WScanSchedule.SchItems["F1_checkBox1"].ToString() == "TRUE" ? "1" : "0";
                string v7 = WScanSchedule.SchItems["F2_text"].ToString();
                string id = SqlReaderWriter.MaxofRow("tblScanSchedule").ToString();
                if (WScanSchedule.EditMode)
                {
                    string selc = "DELETE FROM tblScanSchedule WHERE ScheduleItem='" + WScanSchedule.SchItems["F1_ID"].ToString() + "';";
                    //MessageBox.Show(selc);
                    SqlReaderWriter.ExecuteQuery(selc);
                }
                string scmd = $"INSERT INTO [tblScanSchedule] (ID, ScheduleItem, Frequency, FreqTime, FreqRepeat, FreqPriority, FreqUserName, FreqPassword, FreqRunIfmissed, ScanLocation) VALUES ({id}, '{WScanSchedule.SchItems["F1_textBox1"].ToString()}', " +
                    $"'{v0}'," +
                    $"'{v1}', '{v2}', '{v3}','{v4}', '{v5}', '{v6}', '{v7}')";
                SqlReaderWriter.ExecuteQuery(scmd);
                //  Clipboard.SetText(scmd);
                // MessageBox.Show(scmd);
                this.Close();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            WScanSchedStep.AForm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure want to cancel the wizard?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                this.Close();
        }
    }
}
