using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class frmVlApplication : Form
    {
        public frmVlApplication()
        {
            InitializeComponent();
        }

        private void frmVlApplication_Load(object sender, EventArgs e)
        {
            try {
                lblDate2.Text = DateTime.Now.ToLongDateString();
            lblTime2.Text = DateTime.Now.ToLongTimeString();
            Uri uri = new Uri(@"E:\Program Files\OVAL\ovaldi-5.10.1.7\results.html");
            if(!File.Exists(@"E:\Program Files\OVAL\ovaldi-5.10.1.7\results.html"))
            {
                btnReferesh_Click(null, null);
            }
            System.IO.File.WriteAllText(@"E:\Program Files\OVAL\ovaldi-5.10.1.7\results.html", Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(System.IO.File.ReadAllText(@"E:\Program Files\OVAL\ovaldi-5.10.1.7\results.html"), "ovald", "", RegexOptions.IgnoreCase), "oval", "", RegexOptions.IgnoreCase), "org.mitre", "", RegexOptions.IgnoreCase), "mitre", "", RegexOptions.IgnoreCase));
            webBrowser1.Navigate(uri.AbsoluteUri);
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void btnReferesh_Click(object sender, EventArgs e)
        {
            try
            {
                lblTimer1.Visible = lblTimer2.Visible = true;
                timer1.Start();
                Task.Factory.StartNew(() =>
                {
                    string exepath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\vscan\\vscan.exe";
                    var inf = new ProcessStartInfo(exepath, " -m -o windows.xml");
                    inf.CreateNoWindow = true;
                    inf.WorkingDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\vscan\";
                    inf.WindowStyle = ProcessWindowStyle.Hidden;
                    inf.UseShellExecute = false;
                    btnReferesh.Text = "Waiting...";
                    System.Diagnostics.Process.Start(inf).WaitForExit();
                }).ContinueWith((t)=>
                {
                    timer1.Stop();
                    btnReferesh.Text = "Refresh";
                    lblDate2.Text = DateTime.Now.ToLongDateString();
                    lblTime2.Text = DateTime.Now.ToLongTimeString();

                    Uri uri = new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\vscan\results.html");
                    System.IO.File.WriteAllText(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\vscan\results.html", Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(System.IO.File.ReadAllText(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\vscan\results.html"), "ovald", "", RegexOptions.IgnoreCase), "oval", "", RegexOptions.IgnoreCase), "org.mitre", "", RegexOptions.IgnoreCase), "mitre", "", RegexOptions.IgnoreCase));
                    webBrowser1.Navigate(uri.AbsoluteUri);
                });
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        int sec = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimer2.Text = (sec/60) + ":" + (sec%60);
            sec++;
        }
    }
}
