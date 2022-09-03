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
    public partial class WScanSchedule : Form
    {
        public static bool EditMode = false;
        public static Form AForm;
        public static Dictionary<string, object> SchItems = null;
        public WScanSchedule()
        {
            InitializeComponent();
            SchItems = new Dictionary<string, object>();
            EditMode = false;
            AForm = this;
    }
        public WScanSchedule(bool editMode = false)
        {
            InitializeComponent();
            SchItems = new Dictionary<string, object>();
            EditMode = editMode;
            AForm = this;
        }
        protected new void TabStopChanged(object sender, EventArgs e)
        {
            ((RadioButton)sender).TabStop = true;
        }
        private void WScanSchedule_Load(object sender, EventArgs e)
        {
            try {
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
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "HH:mm";
            if (EditMode)
            {                
                string selc = "SELECT ScheduleItem, Frequency, FreqTime, FreqRepeat, FreqPriority, FreqUserName, FreqPassword, FreqRunIfmissed, ScanLocation FROM tblScanSchedule WHERE ScheduleItem='" + wScanSchedule.SchItems["F1_textBox1"].ToString() + "';";
                //MessageBox.Show(selc);
                DataTable dt = SqlReaderWriter.ReadQuery(selc);
                
                if (dt.Rows.Count==1)
                {
                        DataRow d = dt.Rows[0];
                        textBox1.Text = d[0].ToString();
                        textBox2.Text = d[5].ToString();
                        textBox3.Text = d[6].ToString();
                        //     MessageBox.Show("1");
                        int d1 = Int32.Parse(d[1].ToString());
                        if (d1 > 100000)
                        {
                            d1 -= 100000;
                        }

                        int d3 = Int32.Parse(d[3].ToString());
                        if (d3 > 100000)
                        {
                            checkBox2.Checked = true;
                            d3 -= 100000;
                        }
                        comboBox1.SelectedIndex = Int32.Parse(d1.ToString()) / 100;
                        comboBox2.SelectedIndex = Int32.Parse(d1.ToString()) % 100;
                        comboBox3.SelectedIndex = Int32.Parse(d3.ToString()) / 10000;
                        //   MessageBox.Show("1");
                        numericUpDown2.Value = Int32.Parse(d3.ToString()) % 10000;
                        if(String.IsNullOrEmpty(d[2].ToString()))
                        {
                            radioButton3.Checked = true;
                        }
                        if (!string.IsNullOrWhiteSpace(d[2].ToString()))
                        {
                            string[] spt = d[2].ToString().Split(',');
                            dateTimePicker1.Text = spt[0];
                            numericUpDown1.Value = Int32.Parse(spt[1]);
                        }
                    
                  //  MessageBox.Show("1");
                 /*   checkBox1.Checked = WScanSchedule.SchItems["F1_checkBox1"].ToString() == "TRUE";
                    checkBox2.Checked = WScanSchedule.SchItems["F1_checkBox2"].ToString() == "TRUE";
                    
                    radioButton1.Checked = WScanSchedule.SchItems["F1_radioButton1"].ToString() == "TRUE";
                    radioButton2.Checked = WScanSchedule.SchItems["F1_radioButton2"].ToString() == "TRUE";
                    MessageBox.Show("1");

                    radioButton3.Checked = WScanSchedule.SchItems["F1_radioButton3"].ToString() == "TRUE";
                    radioButton4.Checked = WScanSchedule.SchItems["F1_radioButton4"].ToString() == "TRUE";
                 */ 
                }
                SchItems.Add("F1_ID", textBox1.Text);
                return;
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            textBox2.Text = Environment.UserDomainName + "\\" + Environment.UserName;
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }

        }
        bool IsInList(string name)
        {
            string selc = "SELECT COUNT(*) FROM tblScanSchedule WHERE ScheduleItem='"+textBox1.Text+"'";
            int cnt = (int)SqlReaderWriter.ExecuteScalar(selc);
            return cnt>0;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try { 
            if(textBox1.Text=="")
            {
                MessageBox.Show("Please provide valid Schedule Scan Name", "Webroam Security", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(!EditMode && IsInList(textBox1.Text))
            {
                MessageBox.Show("Scan already scheduled", "Webroam Security", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Please provide valid User Name", "Webroam Security", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string text = "";
            if (EditMode)
            {
               text = SchItems["F1_ID"].ToString();               
            }
            SchItems.Clear();
            if (EditMode)
            {
                SchItems.Add("F1_ID", text);
            }
            SchItems.Add("F1_" + textBox1.Name, textBox1.Text);
            SchItems.Add("F1_" + textBox2.Name, textBox2.Text);
            SchItems.Add("F1_" + textBox3.Name, textBox3.Text);
            SchItems.Add("F1_" + comboBox1.Name, comboBox1.SelectedIndex);
            SchItems.Add("F1_" + comboBox2.Name, comboBox2.SelectedIndex);
            SchItems.Add("F1_" + comboBox3.Name, comboBox3.SelectedIndex);
            SchItems.Add("F1_" + numericUpDown1.Name, numericUpDown1.Text);
            SchItems.Add("F1_" + numericUpDown2.Name, numericUpDown2.Text);
            SchItems.Add("F1_" + dateTimePicker1.Name, dateTimePicker1.Text);
            SchItems.Add("F1_" + checkBox1.Name, checkBox1.Checked ? "TRUE" : "FALSE");
            SchItems.Add("F1_" + checkBox2.Name, checkBox2.Checked ? "TRUE" : "FALSE");
            //if(radioButton1.Checked)
            SchItems.Add("F1_" + radioButton1.Name, radioButton1.Checked ? "TRUE" : "FALSE");
            //if (radioButton2.Checked)
            SchItems.Add("F1_" + radioButton2.Name, radioButton2.Checked ? "TRUE" : "FALSE");
            //if (radioButton3.Checked)
            SchItems.Add("F1_" + radioButton3.Name, radioButton3.Checked ? "TRUE" : "FALSE");
            //if (radioButton4.Checked)
            SchItems.Add("F1_" + radioButton4.Name, radioButton4.Checked ? "TRUE" : "FALSE");
            this.Hide();
            var stp = new WScanSchedStep();
            stp.Show();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }

            // this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure want to cancel the wizard?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
             var sm = new ScanModeConfig();
             sm.ShowDialog();            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = (comboBox1.SelectedIndex == 1);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = comboBox3.Enabled = checkBox2.Checked;
        }
    }
    
}
