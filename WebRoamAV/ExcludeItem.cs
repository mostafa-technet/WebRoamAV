using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class ExcludeItem : Form
    {
        private bool _edit = false;
        public static string _lastString = "";
        public static List<string> _statement = new List<string>();
        public static bool[] MyOptions = new bool[4];
        public ExcludeItem(bool Edit = false, string myText = "")
        {
            InitializeComponent();       
            _edit = Edit;
            //MessageBox.Show(myText);
            _lastString = myText;
        }
        private void ExcludeItem_Load(object sender, EventArgs e)
        {
            if(_edit)
            {
                
                textBox1.Text = _lastString;
                checkBox2.Checked = MyOptions[0];
                checkBox3.Checked = MyOptions[1];
                checkBox4.Checked = MyOptions[2];
                checkBox5.Checked = MyOptions[3];
            }
            else
            {
                textBox1.Text = "";
            }
            textBox1.Focus();
            Task.Run(() =>
            {
              
                Thread.Sleep(50);
                for (int i = 0; i < 10; i++)
                {
                    //MessageBox.Show(this.Controls[i].GetType().ToString()+" "+this.Controls[i].Name+" "+this.Controls[i].TabIndex+" "+i+" "+index);
                    SendKeys.SendWait("{TAB}");
                     System.Threading.Thread.Sleep(10);
                }
                this.Opacity = 1;
            });
            // textBox1.Focus();
          //  textBox1.Select();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            { 
            if(String.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter the file or folder name!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
            }
            MyOptions[0] = checkBox2.Checked;
            MyOptions[1] = checkBox3.Checked;
            MyOptions[2] = checkBox4.Checked;
            MyOptions[3] = checkBox5.Checked;
           // _lastString = textBox1.Text;

            string excludes = "";
            if (ExcludeItem.MyOptions[0])
                excludes += "Known virus detection, ";
            if (ExcludeItem.MyOptions[1])
                excludes += "DNAScan, ";
            if (ExcludeItem.MyOptions[2])
                excludes += "Suspected packed files scan, ";
            if (ExcludeItem.MyOptions[3])
                excludes += "Behavior detection, ";

            excludes = excludes.Substring(0, excludes.Length - 2);
            bool isDir = Directory.Exists(textBox1.Text.Replace("\\*.*", ""));

            if (_edit)
            {
                //repfor = repfor.Remove(repfor.Length - 2, 2);
                string stmt = $"UPDATE tblExcludeFF SET Path='{textBox1.Text.Replace("\\*.*", "")}', Subfolders='{(isDir.ToString().ToUpperInvariant() == "TRUE" ? "1" : "0")}', ExclusionFor='{excludes}' WHERE Path='{ExcludeItem._lastString.Replace("\\*.*", "")}';";
               // SqlReaderWriter.ExecuteQuery(stmt);
                _statement.Add(stmt);
            }
            else
            {
                int count = SqlReaderWriter.MaxofRow("tblExcludeFF");
                //repfor = repfor.Remove(repfor.Length - 2, 2);
                string stmt = "INSERT INTO tblExcludeFF (ID, Path, Subfolders, ExclusionFor) VALUES (" + count.ToString() + $",'{textBox1.Text.Replace("\\*.*", "")}', {(isDir.ToString().ToUpperInvariant() == "TRUE" ? "1" : "0")}, '{excludes}'" + ");";
             //   SqlReaderWriter.ExecuteQuery(stmt);
                _statement.Add(stmt);
            }
                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        void doBtnUpd()
        {
            if (!checkBox2.Checked && !checkBox3.Checked && !checkBox4.Checked && !checkBox5.Checked)
            {
                button5.Enabled = false;
            }
            else
            {
                button5.Enabled = true;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            PicIndex = ((CheckBox)sender).TabIndex;
            if(checkBox2.Checked)
            checkBox3.Checked = checkBox4.Checked = checkBox2.Checked;
            checkBox3.Enabled = checkBox4.Enabled = !checkBox2.Checked;

            doBtnUpd();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();            
            if(fbd.ShowDialog() != DialogResult.Cancel)
            {
                textBox1.Text = fbd.SelectedPath+"\\*.*";
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select File to Exclude";
            if (ofd.ShowDialog() != DialogResult.Cancel)
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            PicIndex = ((CheckBox)sender).TabIndex;
            doBtnUpd();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            PicIndex = ((CheckBox)sender).TabIndex;
            doBtnUpd();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            PicIndex = ((CheckBox)sender).TabIndex;
            doBtnUpd();
        }
        int PicIndex = 0;        
        private void button4_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                PicIndex++;
                int index = PicIndex % 10;

                if (index == 1)
                {
                    pictureBox1.BorderStyle = BorderStyle.Fixed3D;
                    pictureBox2.BorderStyle = BorderStyle.FixedSingle;

                }
                else if (index == 2)
                {
                    pictureBox2.BorderStyle = BorderStyle.Fixed3D;
                    pictureBox1.BorderStyle = BorderStyle.FixedSingle;
                    
                }
                else
                {
                    pictureBox1.BorderStyle = BorderStyle.FixedSingle;
                    pictureBox2.BorderStyle = BorderStyle.FixedSingle;
                    
                    int i = 0;
                    for (; i < this.Controls.Count; i++)
                    {
                        //MessageBox.Show(this.Controls[i].GetType().ToString()+" "+this.Controls[i].Name+" "+this.Controls[i].TabIndex+" "+i+" "+index);
                        if (this.Controls[i].GetType() == typeof(TextBox) || this.Controls[i].GetType() == typeof(CheckBox) || this.Controls[i].GetType() == typeof(Button))
                        {
                            if (this.Controls[i].TabIndex == index)
                            {
                                this.Controls[i].Select();
                                break;
                            }
                        }
                    }
                  
                }
            }
            if(e.KeyCode== Keys.Enter)
            {
                int index = PicIndex %  10;
                if (index == 1)
                {
                 
                    pictureBox1_Click(null, null);
                }
                else if (index == 2)
                {
                   
                    pictureBox2_Click(null, null);
                }
                
            }
        }
    }
}
