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
using Microsoft.WindowsAPICodePack.Dialogs;

namespace WebRoamAV
{
    public partial class WScanSchedStep : Form
    {
        public static Form AForm;
        BindingList<string> binding = new BindingList<string>();
        public WScanSchedStep()
        {
            InitializeComponent();
            AForm = this;
        }
    

        private void button5_Click(object sender, EventArgs e)
        {
            if (WScanSchedule.SchItems.ContainsKey("F2_text"))
            {
                WScanSchedule.SchItems["F2_text"] = String.Join(",", listBox1.Items.OfType<string>().ToArray());
            }
            else
            {
                WScanSchedule.SchItems.Add("F2_text", String.Join(",", listBox1.Items.OfType<string>().ToArray()));
            }
            this.Hide();
            var ssf = new ScanSchedFinish();
            ssf.ShowDialog();          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try { 
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();            
            dialog.InitialDirectory = "C:\\";
            dialog.IsFolderPicker = true;
            dialog.Multiselect = true;                        
            if (dialog.ShowDialog(this.Handle) == CommonFileDialogResult.Ok)
            {
                //MessageBox.Show("You selected: " + dialog.FileName);
                foreach (var it in dialog.FileNames)
                {
                    binding.Add(it);
                    button2.Enabled = button3.Enabled = true;
                    button2.ForeColor = button3.ForeColor = Color.Black;
                }
            }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try { 
            if(listBox1.SelectedIndex>-1)
            {
                binding.RemoveAt(listBox1.SelectedIndex);
                if(binding.Count==0)
                {
                    button2.Enabled = button3.Enabled = false;
                    button2.ForeColor = button3.ForeColor = Color.Gray;
                }
            }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            { 
            binding.Clear();
            button2.Enabled = button3.Enabled = false;
            button2.ForeColor = button3.ForeColor = Color.Gray;
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            WScanSchedule.AForm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure want to cancel the wizard?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                this.Close();
        }
        
        private void WScanSchedStep_Load(object sender, EventArgs e)
        {
            try { 
            if(WScanSchedule.EditMode)
            {
                string selc = "SELECT ScanLocation FROM tblScanSchedule WHERE ScheduleItem='" + wScanSchedule.SchItems["F1_textBox1"].ToString() + "';";
                DataTable dt = SqlReaderWriter.ReadQuery(selc);
                if (dt.Rows.Count == 1)
                {
                    DataRow d = dt.Rows[0];
                    string[] result;
                    listBox1.DataSource = binding;
                    binding.ListChanged += Binding_ListChanged;
                    binding.RaiseListChangedEvents = true;                    
                    if (d[0].ToString().Contains(","))
                    {
                        result = d[0].ToString().Split(',');
                    }
                    else
                    {
                        result = new string[] { d[0].ToString()};
                    }
                    foreach (string r in result)
                    {
                        if(!String.IsNullOrWhiteSpace(r))
                        binding.Add(r);
                    }
                }
                    return;
            }
            listBox1.DataSource = binding;
            binding.ListChanged += Binding_ListChanged;
            binding.RaiseListChangedEvents = true;
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }

        }

        private void Binding_ListChanged(object sender, ListChangedEventArgs e)
        {
            if(binding.Count==0)
            {
                button5.Enabled = false;
                button5.ForeColor = Color.Gray;
            }
            else
            {

                button5.Enabled = true;
                button5.ForeColor = Color.Black;
            }
        }
    }
}
