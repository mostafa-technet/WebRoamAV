using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class bootscanmode : Form
    {
        public bootscanmode()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            Task.Factory.StartNew(() =>
            { try
                {
                    using (Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService())
                    {
                        // Create a new task definition and assign properties
                        Microsoft.Win32.TaskScheduler.TaskDefinition td = ts.NewTask();
                        td.RegistrationInfo.Description = "Scan Boot Time";
                        td.Principal.LogonType = Microsoft.Win32.TaskScheduler.TaskLogonType.S4U;
                        td.Settings.Priority = ProcessPriorityClass.High;
                        td.Principal.RunLevel = Microsoft.Win32.TaskScheduler.TaskRunLevel.Highest;
                        // Create a trigger that will fire after the system boot
                        td.Triggers.Add(new Microsoft.Win32.TaskScheduler.BootTrigger());
                        // Create an action that will launch Notepad whenever the trigger fires
                        td.Actions.Add(new Microsoft.Win32.TaskScheduler.ExecAction(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\boottime.bat", radioButton1.Checked ? "quick" : "full", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
                        // Register the task in the root folder
                        if(ts.FindTask("BootTimeWRoamTask") !=null)
                        ts.RootFolder.DeleteTask("BootTimeWRoamTask");
                        ts.RootFolder.RegisterTaskDefinition(@"BootTimeWRoamTask", td, Microsoft.Win32.TaskScheduler.TaskCreation.CreateOrUpdate, "NT AUTHORITY\\SYSTEM", null, Microsoft.Win32.TaskScheduler.TaskLogonType.S4U);


                        // Remove the task we just created
                        // ts.RootFolder.DeleteTask("BootTimeWRoamTask");
                    }
                }
                catch (Exception em)
                {
                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                }
            }).ContinueWith((t) =>
            {
                try
                {
                    File.Create("bootS.dat");
                    int count = SqlReaderWriter.MaxofRow("tblReportFor");
                    int rpfID = 6;
                    int iDate = Int32.Parse(string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                    string sTime = DateTime.Now.ToLongTimeString();
                    string repfor = "Boot Scan scheduled";
                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblReportFor (ID, ReportFID, Date, Time, ReportFor) VALUES (" + (count + 100000 * rpfID) + ", " + rpfID + ", " + iDate + ", '" + sTime + "', '" + repfor + "\n')");

                    if (MessageBox.Show("Boot Time Scan is scheduled to run on next boot to clean infection from your system. It is recommended that you restart your system. Before restarting your system, save any open files and close all programs.\n\nDo you want to restart the system now?", "Webroam Security", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var psi = new ProcessStartInfo("shutdown", "/r /t 0");
                        psi.CreateNoWindow = true;
                        psi.WindowStyle = ProcessWindowStyle.Hidden;
                        Process.Start(psi);
                    }
                    this.Close();
                }
                catch (Exception em)
                {
                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                }
            });
        }
    }
}
