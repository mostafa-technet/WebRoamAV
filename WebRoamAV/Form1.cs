using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(string batchfile):this()
        {
            
            batchFile = batchfile;
        }
        string batchFile = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    var processInfo = new ProcessStartInfo("cmd.exe", "/c " + "\""+batchFile+"\"");

                    //Do not create command propmpt window 
                    processInfo.CreateNoWindow = true;

                    //Do not use shell execution
                    processInfo.UseShellExecute = false;

                    //Redirects error and output of the process (command prompt).
                    processInfo.RedirectStandardError = true;
                    processInfo.RedirectStandardOutput = true;

                    //start a new process
                    var process = Process.Start(processInfo);
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.OutputDataReceived += (sender1, args) =>
                    {
                        var outputData = args.Data;
                        textBox1.Invoke(new Action(() => textBox1.AppendText(outputData+"\r\n")));
                        // ...
                    };
                    process.ErrorDataReceived += (sender2, args) =>
                    {
                        var errorData = args.Data;
                        textBox1.Invoke(new Action(() => textBox1.AppendText(errorData+"\r\n")));

                        // ...
                    };
                    process.WaitForExit();

                    //wait until process is running                
                    //reads output and error of command prompt to string.
                  
                }catch(Exception ex)
                { MessageBox.Show(ex.ToString()); }
            }).ContinueWith((t) =>
           this.Close());
        }
    }
}
