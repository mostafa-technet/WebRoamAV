using SevenZip;
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
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

namespace WebRoamAV
{
    public partial class tViewQuarantineFiles : Form
    {
        public tViewQuarantineFiles()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tViewQuarantineFiles_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Clear();
                var tblq = SqlReaderWriter.ReadQuery("SELECT Location,QuarantinedOn, VirusName FROM tblQuarantine");
                foreach (DataRow row in tblq.Rows)
                {
                    dataGridView1.Rows.Add(row[0].ToString().Split('\\').Last(), row[0], row[1], row[2]);
                }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.Rows.Count == 0)
                {
                    return;
                }


                var tbl = SqlReaderWriter.ReadQuery("SELECT FileName FROM tblQuarantine");
                foreach (DataRow t in tbl.Rows)
                {
                    try
                    {

                        BackUp.RemoveFile(t[0].ToString());
                    }
                    catch
                    { }
                }

                                
               
            SqlReaderWriter.ExecuteQuery("DELETE FROM tblQuarantine");
                dataGridView1.Rows.Clear();
                tViewQuarantineFiles_Load(null, null);
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
                if(dataGridView1.SelectedRows.Count==0)
                {
                    MessageBox.Show("Please select the row(s) before this action!");
                    return;
                }
            foreach (DataGridViewRow r in dataGridView1.SelectedRows)
            {
                    
                    BackUp.RemoveFile(SqlReaderWriter.ExecuteScalar("SELECT FileName FROM tblQuarantine WHERE Location='" + r.Cells[1].Value.ToString() + "'").ToString());
                SqlReaderWriter.ExecuteQuery("DELETE FROM tblQuarantine WHERE Location='" + r.Cells[1].Value.ToString() + "'");
            }
                dataGridView1.Rows.Clear();
            tViewQuarantineFiles_Load(null, null);
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() != DialogResult.Cancel)
                {
                    //SqlReaderWriter.ExecuteQuery($"INSERT INTO tblQuarantine (FileName,Location,QuarantinedOn) VALUES('{filename}', '{location}', '{Qua}')");                
                    new Quarantine().QuaFile(ofd.FileName);
                    tViewQuarantineFiles_Load(null, null);
                }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select the row(s) before this action!");
                    return;
                }
                foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                {
                    var tblq = SqlReaderWriter.ReadQuery("SELECT FileName FROM tblQuarantine WHERE Location='" + r.Cells[1].Value.ToString() + "'");
                    foreach (DataRow row in tblq.Rows)
                    {
                        string s = r.Cells[1].Value.ToString().Substring(0, r.Cells[1].Value.ToString().LastIndexOf("\\")) +"\\"+ row[0].ToString();
                       // MessageBox.Show(s);
                        BackUp.UnBackUpFile(s);
                        System.IO.File.Move(s, r.Cells[1].Value.ToString());
                        SqlReaderWriter.ExecuteQuery("DELETE FROM tblQuarantine WHERE Location='" + r.Cells[1].Value.ToString() + "'");
                    }
                }
                dataGridView1.Rows.Clear();
                tViewQuarantineFiles_Load(null, null);
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
    
        private void button7_Click(object sender, EventArgs e)
        {
            
           /* MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("tech.webroam.com@gmail.com");
            mail.To.Add("devtech@webroam.com");
            mail.Subject = "Quarantine Mail";
            mail.Body = "Quarantine mail with attachment";            */
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select the row(s) before this action!");
                    return;
                }
                //Scopes for use with the Google Drive API
                MailMessage msg = new MailMessage("tech.webroam.com@outlook.com", "tech.webroam.com@outlook.com", "webroam security attachment", "test");

                // Create an instance of SmtpClient class
                SmtpClient client = new SmtpClient();

                // Specify your mailing Host, Username, Password, Port # and Security option
                client.Host = "smtp-mail.outlook.com";
                client.Username = "tech.webroam.com@outlook.com";
                client.Password = "sandoogheM@n8053";
                client.Port = 587;
                client.SecurityOptions = SecurityOptions.SSLExplicit;
               
               

                var filenm = Path.GetDirectoryName(Application.ExecutablePath) + "\\wrTemp\\" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".bin";
                SevenZipBase.SetLibraryPath(
       Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "7z64.dll"));

                SevenZipCompressor compressor = new SevenZipCompressor();
                compressor.ArchiveFormat = OutArchiveFormat.SevenZip;
                compressor.CompressionMode = CompressionMode.Create;
                List<string> filestoar = new List<string>();
                foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                  {
                      var tblq = SqlReaderWriter.ReadQuery("SELECT ID, FileName FROM tblQuarantine WHERE Location='" + r.Cells[1].Value.ToString() + "'");
                      foreach (DataRow row in tblq.Rows)
                      {
                          string s = r.Cells[1].Value.ToString().Substring(0, r.Cells[1].Value.ToString().LastIndexOf("\\")) + "\\" + row[1].ToString();

                        // MessageBox.Show(s);
                    /*    SqlReaderWriter.ExecuteQuery("DELETE FROM tblQuarantine WHERE ID=" + row[0]);
                        try
                          {
                              BackUp.UnBackUpFile(s);
                          }
                          catch(Exception em)
                          {
                            MessageBox.Show(em.ToString());
                              continue;
                          }
                        /*string rf = r.Cells[1].Value.ToString();
                      if (System.IO.File.Exists(rf)&&File.Exists(s))
                      {
                          System.IO.File.Delete(rf);
                          System.IO.File.Move(s, rf);
                      }*/




                        filestoar.Add(s);
                        


                        //File.Move(s, r.Cells[1].Value.ToString());
                        /*  System.Net.Mail.Attachment attachment;
                          attachment = new System.Net.Mail.Attachment(rf);
                          mail.Attachments.Add(attachment);

                          SmtpServer.Port = 587;
                          SmtpServer.UseDefaultCredentials = false;
                          SmtpServer.Credentials = new System.Net.NetworkCredential("tech.webroam.com@gmail.com", @"vwgvawblprzpluib");
                          SmtpServer.EnableSsl = true;

                          SmtpServer.Send(mail);
                          File.Delete(rf);
                          File.Delete(filenm);*/
                      
                      }
                   
                   
                }
                
                compressor.CompressFiles(filenm, filestoar.ToArray());
                if (new FileInfo(filenm).Length > 50 * 1024 * 1024)
                {
                    MessageBox.Show("Too large file! Contact the administrator, please.");
                    return;
                }

                msg.AddAttachment(new Attachment(filenm));
                // Send this email
                client.Send(msg);
              
                //Upload(dc, "Documents", Path.GetFileName(filenm), new FileStream(filenm, FileMode.Open, FileAccess.Read)).Wait();
                MessageBox.Show("Succcessfully sent the files!");
                dataGridView1.Rows.Clear();
                tViewQuarantineFiles_Load(null, null);
            }
            catch (Exception em)
            {
                MessageBox.Show("Failed to send!");
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
            //string s = r.Cells[1].Value.ToString().Substring(0, r.Cells[1].Value.ToString().LastIndexOf("\\")) + "\\" + row[0].ToString();
            // MessageBox.Show(s);
            

            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
