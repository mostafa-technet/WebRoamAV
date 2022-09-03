using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace WebRoamAV
{
    public partial class SimpleParental : Form
    {
        public SimpleParental()
        {
            InitializeComponent();
        }
        const string filename = ".\\blockedsites.txt";
        private void SimpleParental_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(filename))
                {
                    listBox1.Items.AddRange(File.ReadAllLines(filename).Select((s) => s = (s.StartsWith(".") ? s = "*" + s : s)).Where((s) => !String.IsNullOrWhiteSpace(s)).ToArray());
                }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(textBox1.Text))
                    listBox1.Items.Add(textBox1.Text);
                File.WriteAllLines(filename, listBox1.Items.OfType<string>().Select((s) => s = s.Replace("*", "")).ToArray());
                MessageBox.Show("You added entry to be blocked successfully", "Webroam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(textBox1.Text))
                    listBox1.Items.Add(textBox1.Text);
                textBox1.Text = "";
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    if (listBox1.SelectedIndex >= 0)
                        listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
    }
}
