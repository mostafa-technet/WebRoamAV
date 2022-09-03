using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class CreatePassword : Form
    {
        public CreatePassword()
        {
            InitializeComponent(); //AForm = this;
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length<6)
            {
                MessageBox.Show("Password length should be greather than or equal to 6 characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox1.Text != textBox2.Text)
            {
                MessageBox.Show("Passwords do not match", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);                
                return;
            }
            if(textBox3.Text == "")
            {
                MessageBox.Show("Enter the Captcha please.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox3.Text != captcha.CaptchaValue)
            {
                MessageBox.Show("Captcha is wrong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            try
            {
                string[] lines = File.ReadAllLines(".\\app_config.ini");
                int i = 0;
                foreach (var l in lines)
                {
                    if (l.Contains("Password"))
                    {
                        lines[i] = "Password=" + CreateMD5(textBox1.Text);
                        break;
                    }
                    i++;
                }
                File.WriteAllLines(".\\app_config.ini", lines);
                if (i == lines.Length)
                {
                    File.AppendAllLines(".\\app_config.ini", new string[] { "Password=" + CreateMD5(textBox1.Text) }, new UTF8Encoding(false));
                }
                button1.Enabled = false;
            
                Task.Factory.StartNew((Action)delegate ()
                {
                    try
                    {
                        /*TcpClient client = new TcpClient("localhost", 2555);
                        var netstrm = client.GetStream();
                        string s = "UPDATECONF";
                        byte[] buffer = ASCIIEncoding.ASCII.GetBytes(s);

                        netstrm.Write(buffer, 0, buffer.Length);
                        netstrm.Flush();
                        client.Close();*/
                       /* try
                        {
                           Form1.FreeConsole();
                            Process[] p = Process.GetProcessesByName("WrArServ");
                            if (p.Length > 0 && Form1.AttachConsole((uint)p[0].Id))
                            {
                                //uint spid= p.SessionId;

                                // MessageBox.Show(Process.GetProcessesByName("WRAREngine").Length.ToString() + Environment.NewLine + spid);
                                Form1.SetConsoleCtrlHandler(null, true);

                                //                            GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, p.SessionId);
                                Form1.GenerateConsoleCtrlEvent(Form1.ConsoleCtrlEvent.CTRL_C, 0);
                                //
                                Thread.Sleep(2000);
                                Form1.FreeConsole();
                                Form1.SetConsoleCtrlHandler(null, false);
                            }
                        }
                        catch (Exception em) { ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
*/
                    }
                    catch (Exception em) { ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
                    
                }).ContinueWith((a) =>
                {
                    gSettings.Password = textBox1.Text;
                    MessageBox.Show("The password was saved successfully!");
                    this.BeginInvoke((Action)delegate ()
                    {
                        DialogResult = DialogResult.OK;
                        button1.Enabled = true;
                        this.Close();
                    });
                });
            }
            catch (Exception em) { ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

            //this.Close();
        }
        Captcha captcha = null;
        private void CreatePassword_Load(object sender, EventArgs e)
        {
            captcha = new Captcha();
            pictureBox1.Image = Image.FromStream(captcha.ProcessRequest());
        }

       
    }
}
