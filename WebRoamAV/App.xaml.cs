using NetFwTypeLib;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WebRoamAV
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application    //startup class
    {
        public static readonly int apPort = 2559;
        public static readonly byte apBYTERep = 105;
        public class AppSettings //  Enable/Disable
        {
            public static bool IsSelfProtected = false;     // self-protection 
            public static bool IPSIDSOn = false;            // IPS-IDS
            public static bool IsWifiProtOn = false;        // Wifi Protection
            public static bool IsFirewallOn = true;        // Firewall

            public static bool IsBrowsingProtectOn = true;  // Browsing Protection
            public static bool IsVirusProtectOn = true;     // Realtime virus protection

            public static bool IsAutoUpdateOn = false;      //Auto-Update
            public static bool IsPasswProtectOn = false;    //Password Protection
            public static bool IsVirusReportSendOn = true; //send reprot
            public static bool IsPhishingProtectOn = false; //anti-phisihing *
            public static bool IsParentalProtectOn = false; //parental protection
            public static bool IsNewsAlertOn = false;       //news

            public static bool IsAutoRunProtectOn = false;  //auto-run protection
            public static bool IsScanExternalDevsOn = true; //scan external devices
            public static bool IsBlockUSBsOn = false; //data-theft/ blocks usb, cdrom, etc
            public static bool IsReadOnlyUSB = false;
            public static bool IsDataTheftOn = false;
            public static bool IsScanFullDrive = true; // scan full removable drives or root
            Dictionary<string, Tuple<Window, Action>> sectUpd; //windows automation
            public static bool IsBoot = false;
            public static string ExcludedExtensions = "";
            public static string AU_SAVE_PATH = "";
            public static string AU_PICK_PATH = "";
            public static int OnlineProtMaxSize = 100;
            public static int tblFirewalProtLevel = 2;
            //update features on/off periodically by this function or all
            public void UpdateAll()
            {
                try
                {
                    foreach (var a in sectUpd)
                    {
                        Tuple<Window, Action> ac = a.Value;
                        ac.Item2();
                    }
                    List<string> list = new List<string>();
                    foreach (var a in sectUpd)
                    {
                        Tuple<Window, Action> ac = a.Value;
                        list.Add(ac.Item1.ToString());
                        if (!list.Contains(ac.Item1.ToString()))
                            ac.Item1.Close();
                    }
                }
                catch (Exception em)
                {
                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                }
            }

            //update one feature on/off
            public void UpdateThis(string s)
            {
                sectUpd[s].Item2();
            }

            //is currently checked or unchecked
            private bool CheckedThis(string action)
            {
              
                return (bool)(this.GetType().GetField(action).GetValue(action));
            }
            Window mainwindow;
            
            //Constructor
            public AppSettings()
            {
                try
                {
                    //config section
                   
                   
                        string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
                    IniFile inf = new IniFile(file_ini);

                    //fill config of our system/AV with default values if the config file didn't exist
                    if (!File.Exists(file_ini))
                    {
                       // inf.DeleteSection("WebroamAV");
                        /*inf.Write("IsAutoUpdateOn", "True", "WebroamAV");
                        inf.Write("IsPasswProtectOn", "False", "WebroamAV");*/
                        inf.Write("IsVirusReportSendOn", "True", "WebroamAV");
                        /*inf.Write("IsPhishingProtectOn", "True", "WebroamAV");*/
                       // inf.Write("IsParentalProtectOn", "True", "WebroamAV");
                        /*inf.Write("IsNewsAlertOn", "True", "WebroamAV");
                        inf.Write("IsAutoRunProtectOn", "True", "WebroamAV");
                        inf.Write("IsScanExternalDevsOn", "True", "WebroamAV");
                        inf.Write("IsBlockUSBsOn", "False", "WebroamAV");
                        inf.Write("IsDataTheftOn", "False", "WebroamAV");
                        inf.Write("IsReadOnlyUSB", "False", "WebroamAV");                    
                        inf.Write("IsVirusProtectOn", "True", "WebroamAV");
                        inf.Write("IsBrowsingProtectOn", "True", "WebroamAV");*/
                    }


                    if (inf.KeyExists("AU_PICK_PATH", "AUTOUPDATE"))
                    {
                        App.AppSettings.AU_PICK_PATH = inf.Read("AU_PICK_PATH", "AUTOUPDATE");
                    }

                    if (inf.KeyExists("AU_SAVE_PATH", "AUTOUPDATE"))
                    {
                        App.AppSettings.AU_SAVE_PATH = inf.Read("AU_SAVE_PATH", "AUTOUPDATE");
                    }
                    //update settings of app from config file ********
                    /* if (inf.KeyExists("IsAutoUpdateOn"))
                         AppSettings.IsAutoUpdateOn = inf.Read("IsAutoUpdateOn").ToLower() == "true";
                     if (inf.KeyExists("IsPasswProtectOn"))
                         AppSettings.IsPasswProtectOn = inf.Read("IsPasswProtectOn").ToLower() == "true";*/
                    if (inf.KeyExists("IsVirusReportSendOn", "WebroamAV"))
                    {
                        AppSettings.IsVirusReportSendOn = inf.Read("IsVirusReportSendOn", "WebroamAV").ToLower() == "true";
                    }
                    if (inf.KeyExists("OnlineProtMaxSize", "SCAN_SETTINGS"))
                    {
                        AppSettings.OnlineProtMaxSize = Int32.Parse(inf.Read("OnlineProtMaxSize", "SCAN_SETTINGS"));
                    }
                    /*if (inf.KeyExists("IsPhishingProtectOn"))
                        AppSettings.IsPhishingProtectOn = inf.Read("IsPhishingProtectOn").ToLower() == "true";*/
                    //          if (inf.KeyExists("IsParentalProtectOn"))
                    //            AppSettings.IsParentalProtectOn = inf.Read("IsParentalProtectOn", "WebroamAV").ToLower() == "true";
                    /* if (inf.KeyExists("IsNewsAlertOn"))
                         AppSettings.IsNewsAlertOn = inf.Read("IsNewsAlertOn").ToLower() == "true";
                     if (inf.KeyExists("IsAutoRunProtectOn"))
                         AppSettings.IsAutoRunProtectOn = inf.Read("IsAutoRunProtectOn").ToLower() == "true";
                     if (inf.KeyExists("IsScanExternalDevsOn"))
                     lock (EDDevices.DevLock)
                         AppSettings.IsScanExternalDevsOn = inf.Read("IsScanExternalDevsOn").ToLower() == "true";
                     if (inf.KeyExists("IsBlockUSBsOn"))//in data theft prot.
                         AppSettings.IsBlockUSBsOn = inf.Read("IsBlockUSBsOn").ToLower() == "true";
                 if (inf.KeyExists("IsDataTheftOn"))//data theft prot.
                     AppSettings.IsDataTheftOn = inf.Read("IsDataTheftOn").ToLower() == "true";
                // if (inf.KeyExists("IsVirusProtectOn"))
                  //       AppSettings.IsVirusProtectOn = inf.Read("IsVirusProtectOn").ToLower() == "true";
                     if (inf.KeyExists("IsBrowsingProtectOn"))
                         AppSettings.IsBrowsingProtectOn = inf.Read("IsBrowsingProtectOn").ToLower() == "true";*/

                    mainwindow = new MainWindow(); // Main Window initialization
                    sectUpd = new Dictionary<string, Tuple<Window, Action>>(); //features stored var
                                                                               //sect.Add("All", new Tuple<Window, Action>(null, ()=>UpdateAll()));

                    var frmSett = new frmSettings("MainWindow"); // Create frmSettings, we need some of its functionalities
                    var frmNets = new InNets("MainWindow"); // Create InNets, we need some of its functionalities
                    var frmEdDev = new EDDevices("MainWindow"); // Create EDDevices, we need some of its functionalities
                    var frmEDTheftP = new EDDTheftP("MainWindow");
                    //add selfprotection functionality
                    sectUpd.Add("IsSelfProtected", new Tuple<Window, Action>(frmSett, () => UpdateSettings("IsSelfProtected", "sOnOff" + (CheckedThis("IsSelfProtected") ? "_Checked" : "_Unchecked"))));

                    //add ips/ids functionality
                    sectUpd.Add("IPSIDSOn", new Tuple<Window, Action>(frmNets, () => UpdateSettings("IPSIDSOn", "LaOnOff" + (CheckedThis("IPSIDSOn") ? "_Checked" : "_Unchecked"))));

                    //add wifi protection functionality
                    sectUpd.Add("IsWifiProtOn", new Tuple<Window, Action>(new wFirewallProtection(), () => UpdateSettings("IsWifiProtOn", "fOnOff" + (CheckedThis("IsWifiProtOn") ? "_Checked" : "_Unchecked"))));

                    //add firewall functionality
                    

                    //sectUpd.Add("IsFirewallOn", new Tuple<Window, Action>(frmNets, () => UpdateSettings("IsFirewallOn", "fOnOff" + (CheckedThis("IsFirewallOn") ? "_Checked" : "_Unchecked"))));

                    //add browsing protection functionality
                    sectUpd.Add("IsBrowsingProtectOn", new Tuple<Window, Action>(frmNets, () => UpdateSettings("IsBrowsingProtectOn", "SSOnOff" + (CheckedThis("IsBrowsingProtectOn") ? "_Checked" : "_Unchecked"))));

                    //add realtime virus protection functionality
                    sectUpd.Add("IsVirusProtectOn", new Tuple<Window, Action>(new winFilesFolders("MainWindow"), () => UpdateSettings("IsVirusProtectOn", "fOnOff" + (CheckedThis("IsVirusProtectOn") ? "_Checked" : "_Unchecked"))));

                    //add autoupdate functionality
                    sectUpd.Add("IsAutoUpdateOn", new Tuple<Window, Action>(frmSett, () => UpdateSettings("IsAutoUpdateOn", "fOnOff" + (CheckedThis("IsAutoUpdateOn") ? "_Checked" : "_Unchecked"))));

                    //add password-protection functionality
                    sectUpd.Add("IsPasswProtectOn", new Tuple<Window, Action>(frmSett, () => UpdateSettings("IsPasswProtectOn", "tOnOff" + (CheckedThis("IsPasswProtectOn") ? "_Checked" : "_Unchecked"))));

                    //add sendreport functionality
                    sectUpd.Add("IsVirusReportSendOn", new Tuple<Window, Action>(frmSett, () => UpdateSettings("IsVirusReportSendOn", "foOnOff" + (CheckedThis("IsVirusReportSendOn") ? "_Checked" : "_Unchecked"))));

                    //add phishing protection functionality *
                    sectUpd.Add("IsPhishingProtectOn", new Tuple<Window, Action>(frmNets, () => UpdateSettings("IsPhishingProtectOn", "sOnOff" + (CheckedThis("IsPhishingProtectOn") ? "_Checked" : "_Unchecked"))));

                    // sect.Add("IsParentalProtectOn", new Tuple<Window, Action>(new wndParental("MainWindow"), () => UpdateSettings("IsParentalProtectOn", (CheckedThis("IsParentalProtectOn") ? "" : ""))));

                    //add news-alert functionality
                    sectUpd.Add("IsNewsAlertOn", new Tuple<Window, Action>(frmNets, () => UpdateSettings("IsNewsAlertOn", "foOnOff" + (CheckedThis("IsNewsAlertOn") ? "_Checked" : "_Unchecked"))));

                    //add autorun-protection functionality
                    sectUpd.Add("IsAutoRunProtectOn", new Tuple<Window, Action>(frmEdDev, () => UpdateSettings("IsAutoRunProtectOn", "fOnOff" + (CheckedThis("IsAutoRunProtectOn") ? "_Checked" : "_Unchecked"))));

                    //add scan-external-devices functionality
                    sectUpd.Add("IsScanExternalDevsOn", new Tuple<Window, Action>(frmEdDev, () => UpdateSettings("IsScanExternalDevsOn", "SSOnOff" + (CheckedThis("IsScanExternalDevsOn") ? "_Checked" : "_Unchecked"))));

                    //add data-theft functionality
                    // sectUpd.Add("IsBlockUSBsOn", new Tuple<Window, Action>(frmEdDev, () => UpdateSettings("IsBlockUSBsOn", "TTOnOff" + (CheckedThis("IsBlockUSBsOn") ? "_Checked" : "_Unchecked"))));
                    
                    sectUpd.Add("IsDataTheftOn", new Tuple<Window, Action>(frmEdDev, () => UpdateSettings("IsDataTheftOn", "TTOnOff" + (CheckedThis("IsDataTheftOn") ? "_Checked" : "_Unchecked"))));
                    sectUpd.Add("IsReadOnlyUSB", new Tuple<Window, Action>(frmEDTheftP, () => UpdateSettings("IsReadOnlyUSB", "r1" + (CheckedThis("IsReadOnlyUSB") ? "_Checked" : "_Unchecked"))));
                    sectUpd.Add("IsBlockUSBsOn", new Tuple<Window, Action>(frmEDTheftP, () => UpdateSettings("IsBlockUSBsOn", "r2" + (CheckedThis("IsReadOnlyUSB") ? "_Checked" : "_Unchecked"))));

                }
                catch (Exception em)
                {
                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                }
            }



            // update setings on/off check buttons checked/unchecked
            private void UpdateSettings(string Section, string OnOff)
            {
                try
                {
                    var sc = sectUpd[Section];
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MethodInfo dynMethod = sc.Item1.GetType().GetMethod(OnOff,
            BindingFlags.NonPublic | BindingFlags.Instance);
                        dynMethod.Invoke(sc.Item1, new object[] { null, null });
                        Application.Current.MainWindow = mainwindow;

                    }));


                }
                catch (Exception em)
                {
                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                }
            }
        }

            //notification icon on tray of system
            private System.Windows.Forms.NotifyIcon _notifyIcon;
            private bool _isExit;

            //Use this var to determin largest free space drive
            public static DriveInfo maxfreesize;


            // Engine Executable file
            public static string ConsoleApplication11 = "webroamave";
            //private TcpListener myTC;

            //cancellation feature of 
            private CancellationTokenSource ctsNetw;

        Socket listener;

            //Make sure it runs Only once
        bool isFirstRun = true;


            private static void KillProcessAndChildren(int pid)
            {
                try
                {
                    // Cannot close 'system idle process'.
                    if (pid == 0)
                    {
                        return;
                    }
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher
                            ("Select * From Win32_Process Where ParentProcessID=" + pid);
                    ManagementObjectCollection moc = searcher.Get();
                    foreach (ManagementObject mo in moc)
                    {
                        KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
                    }
                    try
                    {
                        Process proc = Process.GetProcessById(pid);
                        proc.Kill();
                    }
                    catch (ArgumentException)
                    {
                        // Process already exited.
                    }
                }
                catch (Exception em)
                {
                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                }
            }


            //monitor file system by this watcher
            FileSystemWatcher fwIOMain;
        public static FileSystemEventHandler func;
            //grid items list of scanned files/folders
            List<GRowsCUS> lstGrMain;
        public static bool IsLicenseValid = false;

        // 1st way
        private const int MAX_PATH = 260;
        private const int CSIDL_DESKTOP = 0x0000;
        private const int CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019; // Can get to All Users desktop even on .NET 2.0,
                                                                  // where Environment.SpecialFolder.CommonDesktopDirectory is not available
        [DllImport("shell32.dll")]
        static extern int SHGetKnownFolderPath(
      [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
      uint dwFlags,
      IntPtr hToken,
      out IntPtr pszPath  // API uses CoTaskMemAlloc
      );


        // 2nd way
        static string GetDesktopPath(bool iscommonDesktop)
        {
            if (iscommonDesktop)
                return Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            else
                return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            /*Guid PublicDesktop = new Guid("C4AA340D-F20F-4863-AFEF-F87EF2E6BA25");
            IntPtr pPath;

            if (SHGetKnownFolderPath(PublicDesktop, 0, IntPtr.Zero, out pPath) == 0)
            {
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(pPath);
            }

            return string.Empty;*/
        }
        void CreateDesktopShortcuts(bool iscommonDesktop)
        {

            var file1 = Path.Combine(GetDesktopPath(iscommonDesktop), "WebroamAV.exe.lnk");

            var powershell = PowerShell.Create();
            //  Clipboard.SetText(file1);

            //    MessageBox.Show(file1);

            if (!File.Exists(file1))
                return;
            File.Delete(file1);


            powershell.Commands.AddScript("$WshShell = New-Object -comObject WScript.Shell");
            powershell.Commands.AddScript($"$Shortcut = $WshShell.CreateShortcut(\"{file1}\")");
            powershell.Commands.AddScript($"$Shortcut.TargetPath = \"{Assembly.GetExecutingAssembly().Location}\"");
            powershell.Commands.AddScript($"$Shortcut.WorkingDirectory = \"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\"");
            powershell.Commands.AddScript("$Shortcut.Save()");
            powershell.Invoke();
            var powershell2 = PowerShell.Create();
            var file2 = Path.Combine(GetDesktopPath(iscommonDesktop), "webroamAntiRansomeware.exe.lnk");
            if (File.Exists(file2))
                File.Delete(file2);
            // MessageBox.Show(file2);
            powershell2.Commands.AddScript("$WshShell = New-Object -comObject WScript.Shell");
            powershell2.Commands.AddScript($"$Shortcut = $WshShell.CreateShortcut(\"{file2}\")");
            powershell2.Commands.AddScript($"$Shortcut.TargetPath = \"{Path.Combine(new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent.FullName + "\\gui\\wrMainAntiRansomeware.exe")}\"");
            powershell2.Commands.AddScript($"$Shortcut.WorkingDirectory = \"{Path.Combine(new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent.FullName + "\\gui\\")}\"");
            powershell2.Commands.AddScript("$Shortcut.Save()");
            powershell2.Invoke();

        }
        static readonly int PCOUNT = 4;
        public static object LK_VP = new object();
        private void Application_Startup(object sender, StartupEventArgs e)
            {
                try
                {
                 Directory.SetCurrentDirectory(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                    //Auto-Update
                    wrUpdate.Update();
                    string fr = "first_run";
                    string fro = "first_run.old";
                // MessageBox.Show((Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+ "\\setups"));
                // check if we need to add to right click menue of windows 
                if (Directory.Exists(fr) || File.Exists((new DirectoryInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).Parent.FullName + "\\setups\\deploy.bat")))
                {
                    try
                    {

                        CreateDesktopShortcuts(true);
                        CreateDesktopShortcuts(false);
                        /* var fcon = File.ReadAllText("first_run\\addmenuitem.reg");
                         fcon = fcon.Replace("[DBL_PATH_SCANNER]", System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("\\", "\\\\"));
                         File.WriteAllText("first_run\\addmenuitem.reg", fcon);*/
                        //var pinfo = new ProcessStartInfo();
                        //pinfo.Verb = "runas";
                        //pinfo.WindowStyle = ProcessWindowStyle.Hidden;
                        /*pinfo.FileName = */
                        new Form1(new DirectoryInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).Parent.FullName + "\\installer.bat").ShowDialog();
                        //Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\regedit.exe";
                        //pinfo.Arguments = $"/s {System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\\first_run\\addmenuitem.reg";
                        //pinfo.CreateNoWindow = true;
                        //pinfo.WorkingDirectory = new DirectoryInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).Parent.FullName;
                        // pinfo.UseShellExecute = false;
                        //Process.Start(pinfo).WaitForExit();
                        //  if (!Directory.Exists(fr))
                        //    Directory.CreateDirectory(fr);
                        if (Directory.Exists(fr))
                        {
                            try
                            {
                                Directory.Delete(fr);
                            }
                            catch { }
                        }
                        Process.Start("sc.exe config wrardriver start=auto");
                        Process.Start("sc.exe start wrardriver");



                        Process p12 = new System.Diagnostics.Process();
                        ProcessStartInfo psi12 = new ProcessStartInfo(@"Powershell.exe");
                        psi12.WindowStyle = ProcessWindowStyle.Hidden;
                        psi12.CreateNoWindow = true;
                        p12.StartInfo = psi12;
                        psi12.Arguments = $"  -WindowStyle Hidden Remove-Item -LiteralPath '{new DirectoryInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).Parent.FullName }\\setups' -Force -Recurse";
                        psi12.RedirectStandardOutput = true;
                        psi12.UseShellExecute = false;
                        p12.Start();
                        p12.WaitForExit();

                        Process[] instances = Process.GetProcessesByName("wrMainAntiRansomeware");

                        if (instances.Length > 1)
                        {
                            foreach (var i in instances)
                            {
                                if (i.Id != Process.GetCurrentProcess().Id)
                                {
                                    i.Kill();
                                }
                            }
                        }
                      
                    }
                    catch { }
                    finally
                    {
                        Process p121 = new System.Diagnostics.Process();
                        ProcessStartInfo psi121 = new ProcessStartInfo(@"cscript.exe");
                        psi121.WindowStyle = ProcessWindowStyle.Hidden;
                        psi121.CreateNoWindow = true;
                        p121.StartInfo = psi121;
                        psi121.Arguments = $"\"{new DirectoryInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).Parent.FullName }\\gui\\start.vbs\"";
                        psi121.RedirectStandardOutput = true;
                        psi121.UseShellExecute = false;
                        p121.Start();
                        Environment.Exit(0);

                    }
                }
                
                ////*******




                // We need cmd Arg to run AV
                if (Environment.GetCommandLineArgs().Length >= 1)
                    {                   
                        // Not Main as arg means some path (if not start which is startup of window)
                        if (Environment.GetCommandLineArgs().Length>1&&Environment.GetCommandLineArgs()[1].ToLower() != "main"&& Environment.GetCommandLineArgs()[1].ToLower() != "start")
                        {
                        var parg = Environment.GetCommandLineArgs()[1].TrimStart('\'').TrimEnd('\'');

                       
                            try
                            {
                                // we need absolute
                                if (!Path.IsPathRooted(parg))
                                    return;


                                // ok??
                                //Path.GetFullPath(parg);
                            }
                            catch
                            {
                                // the path is not ok/legal
                                return;
                            }


                        /*  lstGrMain = new List<GRowsCUS>();

                          lstGrMain.Add(new GRowsCUS { PathF = parg, SubF = Directory.Exists(parg)?"Yes": "No" });
                          wndFullScan w = new wndFullScan("MainWindow", false, ref lstGrMain);


                          int rpfID = 1;
                          int iDate = Int32.Parse(string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                          string sTime = DateTime.Now.ToLongTimeString();
                          string repfor = "Custom Scan:\n" + parg;



                          int count = SqlReaderWriter.MaxofRow("tblReportFor");
                          //repfor = repfor.Remove(repfor.Length - 2, 2);
                          SqlReaderWriter.ExecuteQuery("INSERT INTO tblReportFor (ID, ReportFID, Date, Time, ReportFor) VALUES (" + (count + 1) + ", " + rpfID + ", " + iDate + ", '" + sTime + "', '" + repfor + "')");

                          w.ShowDialog();*/
                        if(String.Join(" ", Environment.GetCommandLineArgs()).Split('\'').Length>1)
                        using (var tc = new TcpClient()) // bring main window to front
                        {
                            try
                            {
                                    for (int cc = 0; cc < PCOUNT; cc++)
                                    {
                                        try
                                        {
                                            tc.Connect("127.0.0.1", App.apPort + cc);
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                        var strm = tc.GetStream();
                                        string swi = "CustomScan*" + String.Join(" ", Environment.GetCommandLineArgs()).Split('\'')[1];
                                        byte[] swibuf = new byte[1024];
                                        ASCIIEncoding.ASCII.GetBytes(swi).CopyTo(swibuf, 0);
                                        strm.Write(swibuf, 0, swibuf.Length);
                                        strm.Flush();
                                        if (strm.ReadByte() == apBYTERep)
                                        {
                                            strm.Close();

                                            //   tc.Client.Shutdown(SocketShutdown.Both);
                                            // tc.Client.Disconnect(false);
                                            //tc.GetStream().Close();
                                            tc.Close();
                                            break;
                                        }
                                        strm.Close();

                                        //   tc.Client.Shutdown(SocketShutdown.Both);
                                        // tc.Client.Disconnect(false);
                                        //tc.GetStream().Close();
                                        tc.Close();
                                       // break;
                                    }
                            }
                            catch (Exception en){ MessageBox.Show(en.ToString()); }
                        }
                        Environment.Exit(0);
                            return;
                        }
                        else  // add listener to bring window to front or run custom scan from windows (right click or cmd or others) in first run
                        {
                        ctsNetw = new CancellationTokenSource(); // initialize cancellation tocket of network for re-run of program                            

                        var pinfo = new ProcessStartInfo();
                        pinfo.WindowStyle = ProcessWindowStyle.Hidden;
                        pinfo.FileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + "webroam_diver_packets.exe";
                        pinfo.CreateNoWindow = true;
                        pinfo.WorkingDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        pinfo.UseShellExecute = false;
                        pinfo.Verb = "runas";
                       // Process.Start(pinfo);


                        var trd = new Thread(delegate ()

                        {  byte[] bytes;

                            /*     listener = new Socket(AddressFamily.InterNetwork,
                                                             SocketType.Stream, ProtocolType.Tcp);
                                 listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                                 listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                                 listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1500);
                                 listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 1500);
                                 // Data buffer for incoming data.
                               
                                 // Establish the local endpoint for the socket.
                                 // Dns.GetHostName returns the name of the 
                                 // host running the application.
                                 IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, App.apPort);

                                 // Create a TCP/IP socket.
                                 //MessageBox.Show("0");

                                 // Bind the socket to the local endpoint and 
                                 // listen for incoming connections.

                                 listener.Bind(localEndPoint);
                                 listener.Listen(10);*/
                          
                            // MessageBox.Show("1");
                            // Start listening for connections.
                             Task.Run(() =>
                                    {
                                        try
                                        {
                                            for (int cc = 0; cc < PCOUNT; cc++)
                                {
                                                              
                                        TcpListener listener;
                                        try
                                        {
                                                var tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                                tcpClient.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), (App.apPort + cc)));
                                                tcpClient.Close();

                                            }
                                            catch (SocketException em)
                                        {                                                
                                                continue;
                                        } 
                                            listener = new TcpListener(IPAddress.Any, (App.apPort + cc)); 
                                                listener.Start();
                                           
                                       // MessageBox.Show(App.apBYTERep.ToString());
                                        while (!ctsNetw.IsCancellationRequested)
                                        {

                                            // MessageBox.Show("");
                                            //cts.Token.ThrowIfCancellationRequested();
                                            //Console.WriteLine("Waiting for a connection...");
                                            // Program is suspended while waiting for an incoming connection.
                                            //MessageBox.Show("");
                                            /*      if (!listener.Poll(8000, SelectMode.SelectRead))
                                                  {
                                                      if (!listener.Connected)
                                                          Environment.Exit(1);

                                                      continue;
                                                  }
                                                 /* var mytr = new Thread(delegate ()

                                                  {*/
                                            string data = "";
                                            TcpClient handler = null;
                                            try
                                            {
                                                   // MessageBox.Show((App.apPort+cc).ToString());
                                                handler = listener.AcceptTcpClient();
   
//MessageBox.Show("4");

                                                int bytesRec = 0;
                                                var nts = handler.GetStream();
                                                do
                                                {
                                                    bytes = new byte[1024];

                                                    bytesRec += nts.Read(bytes, bytesRec, 1024);
                                                     data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                                                }
                                                while (bytesRec > 0);
                                            
                                                nts.WriteByte(App.apBYTERep);
                                                nts.Flush();
                                                nts.Close();
                                            }
                                            catch
                                            {
                                            }
                                            finally
                                            {
                                                handler?.Close();
                                            }
                                            if (!String.IsNullOrWhiteSpace(data))
                                            {
                                                if (data.IndexOf("MainWindow") != -1)
                                                {
                                                    Dispatcher.Invoke(() =>
                                                    {
                                                        ShowMainWindow();

                                                    });
                                                }
                                                else if (data.IndexOf("CustomScan") != -1)
                                                {
                                                    Dispatcher.Invoke(() =>
                                                    {

                                                        lstGrMain = new List<GRowsCUS>();
                                                        if (data.Contains("*"))
                                                        {
                                                            string parg = data.Split('*')[1].TrimEnd((char)0).TrimEnd(' ', '\n');
                                                            // MessageBox.Show(Directory.Exists(parg).ToString()+parg+" "+parg.Last().ToString()+"\n"+parg.Length.ToString());
                                                            lstGrMain.Add(new GRowsCUS { PathF = parg, SubF = Directory.Exists(parg) ? "Yes" : "No" });
                                                            wndFullScan w = new wndFullScan("MainWindow", false, ref lstGrMain);


                                                            int rpfID = 1;
                                                            int iDate = Int32.Parse(string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                                                            string sTime = DateTime.Now.ToLongTimeString();
                                                            string repfor = "Custom Scan:\n" + parg;



                                                            int count = SqlReaderWriter.MaxofRow("tblReportFor");
                                                            //repfor = repfor.Remove(repfor.Length - 2, 2);
                                                            SqlReaderWriter.ExecuteQuery("INSERT INTO tblReportFor (ID, ReportFID, Date, Time, ReportFor) VALUES (" + (count + 1) + ", " + rpfID + ", " + iDate + ", '" + sTime + "', '" + repfor + "')");

                                                            w.ShowDialog();
                                                        }
                                                    });
                                                }
                                            }

                                            /*  });
                                              //mytr.ApartmentState = ApartmentState.STA;
                                              mytr.Start();
                                              mytr.Join();*/
                                        }
                                   
                                    break;
                                }
                                    }

                            catch (Exception em)
                            {
                                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                                //Environment.Exit(1);
                            }
                        });

                           
                        });
                        //if(isFirstRun)
                        
                        // Main as cmd arg show Main Window 
                        if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1) //if program already run
                        {
                            try
                            { 
                                for (int cc = 0; cc < PCOUNT; cc++)
                                {using (var tc = new TcpClient()) // bring main window to front
                                    {
                                   
                                        try
                                        {
                                            try
                                            {
                                                tc.Connect("127.0.0.1", App.apPort + cc);
                                            }catch
                                            {  }
                                            var strm = tc.GetStream();
                                            string swi = "MainWindow\n";
                                            byte[] swibuf = new byte[1024];
                                            ASCIIEncoding.ASCII.GetBytes(swi).CopyTo(swibuf, 0);
                                            strm.Write(swibuf, 0, swibuf.Length);
                                            strm.Flush();
                                            //MessageBox.Show((App.apPort+cc).ToString());
                                            /*if(strm.ReadByte()== apBYTERep)
                                            {
                                                MessageBox.Show("2");

                                                strm.Close();
                                            // tc.Client.Shutdown(SocketShutdown.Both);
                                            // tc.Client.Disconnect(false);
                                            //tc.GetStream().Close();
                                            tc.Close();
                                                break;
                                            }*/
                                            strm.Close();
                                            // tc.Client.Shutdown(SocketShutdown.Both);
                                            // tc.Client.Disconnect(false);
                                            //tc.GetStream().Close();
                                            tc.Close();
                                           // break;
                                        }
                                        catch { }

                                    }

                                }


                                Environment.Exit(0);
                                //  ;

                                //                                return;
                            }
                            catch(Exception em)
                            {
                                try
                                {
                                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                                }
                                catch { }
                                finally
                                {
                                    Environment.Exit(0);
                                }
                            }
                           
                        }

                        trd.Start();





                        /*  myTC = new TcpListener(IPAddress.Any, App.apPort);

                              try
                              {
                                  myTC.Start();
                              }
                              catch
                              {
                              Environment.Exit(1);
                              }
                          // Socket s;


                             */




                        /*    Process curr = Process.GetCurrentProcess();
                            Process[] procs = Process.GetProcessesByName(curr.ProcessName);
                            foreach (Process p in procs)
                            {
                                if ((p.Id != curr.Id) &&
                                    (p.MainModule.FileName == curr.MainModule.FileName))
                                {
                                    ProcessStartInfo psi = new ProcessStartInfo();
                                    psi.FileName = Environment.SystemDirectory + "\\Taskkill.exe";
                                    psi.Arguments = " /PID " + p.Id.ToString();
                                    psi.CreateNoWindow = true;
                                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                                    psi.UseShellExecute = false;
                                    psi.Verb = "runas";
                                    Process.Start(psi);
                                }
                                    //p.Kill();
                            }*/
                        Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        /*int isrun = Process.GetProcessesByName(ConsoleApplication11).Length;
                        if (isrun == 0)
                        {
                            var pinfo = new ProcessStartInfo();
                            pinfo.WindowStyle = ProcessWindowStyle.Hidden;
                            pinfo.FileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + ConsoleApplication11 + ".exe";
                            pinfo.CreateNoWindow = true;
                            pinfo.WorkingDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                            pinfo.UseShellExecute = false;
                            Process.Start(pinfo);

                        }*/
                        //Task.Run(() => WebroamIPS.StartService());
                        if (true)
                        {
                            BrowserAutm nrAu = new BrowserAutm();
                            try
                            {
                                Task.Run(() => nrAu.Start());
                            }
                            catch { }
                        }
                                //ClamAVEngine cen = null; // av engine
                                Task.Factory.StartNew(() =>
                                {
                                    if (ProcessProtector.cen == null)
                                        ProcessProtector.cen = new ClamAVEngine();
                                //    cen = ProcessProtector.cen;

                                  //  cen = new ClamAVEngine();
                                }).ContinueWith(delegate
                                {
                                    BlockingCollection<string> myqs = new BlockingCollection<string>();
                                    Task.Factory.StartNew(() =>
                                    {
                                        List<string> alreadyScanned = new List<string>();



                                        while (true)
                                        {
                                            bool isVPOn = false;
                                            lock(LK_VP)
                                            {
                                                isVPOn = App.AppSettings.IsVirusProtectOn;
                                            }
                                            if(!isVPOn)
                                            {
                                                Thread.Sleep(5000);
                                                continue;
                                            }    
                                            List<string> mwfn = new List<string>();
                                            {
                                                string itm = myqs.Take();
                                                if (!alreadyScanned.Contains(itm))
                                                {
                                                    mwfn.Add(itm);
                                                    alreadyScanned.Add(itm);
                                                }
                                            }
                                            Thread.Sleep(20000);
                                            bool hasit = true;
                                            while (hasit)
                                            {
                                                string itm;
                                                hasit = myqs.TryTake(out itm, 100);
                                                if (hasit && (!alreadyScanned.Contains(itm)))
                                                {
                                                    mwfn.Add(itm);
                                                    alreadyScanned.Add(itm);
                                                }
                                            }
                                            // Dispatcher.Invoke(() => MessageBox.Show(wfn));
                                            //mwfn.Add(Process.GetCurrentProcess().MainModule.FileName);
                                            Task.Factory.StartNew(() =>
                                            {
                                                try
                                                {
                                                    var mitm = mwfn.ToArray();
                                                    //  string wfn = ob;

                                                    //Dispatcher.Invoke(() => MessageBox.Show(wfn));
                                                    //var t = ProcessProtector.cen.Scan(mitm);

                                                    //                    Dispatcher.Invoke(() => MessageBox.Show(s));
                                                   
                                                    if (mitm.Count() > 0)
                                                    {string St = "";
                                                        foreach (var s in ProcessProtector.cen.Scan(mitm))
                                                        {
                                                            St += s;
                                                            // string.Join("\r\n", s);
                                                           // if(s.Contains("$"))
                                                            //MessageBox.Show(s);
                                                            if (!String.IsNullOrWhiteSpace(St) && (St.Contains("$")) && St.Contains("Malware"))
                                                            {
                                                                if (!St.Contains("***"))
                                                                    continue;
                                                                
                                                                   
                                                                Dispatcher.Invoke(() => new wMBox(St.Replace("$", ": ").Replace("Finished",""), "Alert").Show());
                                                                string[] infs = St.Split('\n').Where(a => a.Contains("$") && a.Contains("File$")).ToArray();
                                                                //   Dispatcher.Invoke(() => MessageBox.Show(infs[0]));
                                                                //     new Quarantine().QuaFile(infs[0].Split('$')[1]);
                                                                Task.Factory.StartNew(() =>
                                                                    {
                                                                        int rpfID = 4;
                                                                        int iDate = Int32.Parse(string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                                                                        string sTime = DateTime.Now.ToLongTimeString();
                                                                        string repfor = "Online Protection\n" + infs[0].Split('$')[1];

                                                                        int count = SqlReaderWriter.MaxofRow("tblReportFor");
                                                                    //repfor = repfor.Remove(repfor.Length - 2, 2);
                                                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblReportFor (ID, ReportFID, Date, Time, ReportFor) VALUES (" + (count + 1) + ", " + rpfID + ", " + iDate + ", '" + sTime + "', '" + repfor + "')");
                                                                    });
                                                            }
                                                            if(St.Contains("***"))
                                                            St = "";
                                                        }
                                                    }
                                                }catch (Exception em)
                                                {
                                                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                                                }
                                                // Dispatcher.Invoke(() => MessageBox.Show("3"));
                                            });
                                          //  Thread.Sleep(300);
                                           // mwfn.Clear();


                                        }
                                        /*else
                                        {
                                            if (SAVAPI.TestOpenPort(2555))
                                            {
                                                var sc = cen.Scan(new string[] { wfilename });
                                                string jsc = string.Join(Environment.NewLine, sc);
                                                if (!String.IsNullOrWhiteSpace(jsc))
                                                {
                                                    Dispatcher.Invoke(() => new wMBox(jsc.Replace("$", ":"), "Alert").Show());
                                                    string[] infs = jsc.Split('\n').Where(a => a.Contains("$") && a.Contains("File")).ToArray();
                                                    lock (this)
                                                    {
                                                        new Quarantine().QuaFile(infs[0].Split('$')[1]);
                                                    }
                                                }
                                            }
                                        }*/
                                    });

                                    // Online Protection Functionality fill the queue for scanner engine
                                    // Quarantine in drive with most free space. for now, skip it
                                    Task.Factory.StartNew(() =>
                                        {
                                            try
                                            {
                                                //wndCustomScan.sav = new SAVAPI();
                                                var dinf = DriveInfo.GetDrives().Where(a => a.DriveType != DriveType.CDRom && a.IsReady);
                                                var formats = File.ReadAllLines("AV_config.ini").ToList().FindAll(s => s.ToUpperInvariant().Contains("EXE") && s.Contains("=")).Select(s => s.Replace(" =", "=").ToUpperInvariant().Replace("EXE=", "")).ToArray();
                                                string sfrmt = String.Join(Environment.NewLine, formats).ToLowerInvariant();


                                                maxfreesize = dinf.ElementAt(0);
                                                for (int i = 0; i < dinf.Count(); i++)
                                                {
                                                    if (maxfreesize.AvailableFreeSpace < dinf.ElementAt(i).AvailableFreeSpace)
                                                        maxfreesize = dinf.ElementAt(i);
                                                    fwIOMain = new FileSystemWatcher(dinf.ElementAt(i).Name, "*.*");
                                                    fwIOMain.IncludeSubdirectories = true;
                                                    fwIOMain.EnableRaisingEvents = true;
                                                    fwIOMain.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.FileName;

                                                    var tnow = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                                    //  int wt = 0;

                                                    func = delegate (object sn, FileSystemEventArgs ev)
                                                        {
                                                            try
                                                            {
                                                                if (new DriveInfo(ev.FullPath.Substring(0, 2)).DriveType == DriveType.Removable)
                                                                {
                                                                    lock (EDDevices.DevLock)
                                                                    {
                                                                        if (!AppSettings.IsScanExternalDevsOn)
                                                                        {
                                                                            return;
                                                                        }
                                                                    }
                                                                    if (!AppSettings.IsScanFullDrive && ev.FullPath.Count((a) => a == '\\') > 1)
                                                                    {
                                                                        return;
                                                                    }
                                                                }
                                                                
                                                                string wfilename = ev.FullPath;
                                                                if ((!ev.FullPath.Split('\\').Last().Contains(".")) || (!sfrmt.Contains(Path.GetExtension(ev.FullPath).ToLowerInvariant())) || (ev.FullPath.Split('.').Last().StartsWith("Zz")))
                                                                    return;
                                                                if (!File.Exists(wfilename))
                                                                    return;
                                                                    if(new FileInfo(wfilename).Length / (1024 * 1024) > AppSettings.OnlineProtMaxSize)
                                                                    return;
                                                                //MessageBox.Show(ev.FullPath);
                                                                //fw.EnableRaisingEvents = false;
                                                                

                                                                if (App.AppSettings.ExcludedExtensions != "" && App.AppSettings.ExcludedExtensions.Contains(wfilename.Substring(wfilename.LastIndexOf("."))))
                                                                {
                                                                    return;
                                                                }
                                                                myqs.Add(wfilename);
                                                            }
                                                            catch(Exception em)
                                                            {
                                                                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                                                            }
                                                            // MessageBox.Show(wfilename);
                                                            //fw.EnableRaisingEvents = true;
                                                        };

                                                    fwIOMain.Changed += func;
                                                }

                                                while (true)
                                                {
                                                    try
                                                    {
                                                        fwIOMain.WaitForChanged(WatcherChangeTypes.Created);
                                                    }
                                                    catch { }
                                                }
                                            }
                                            catch (Exception em)
                                            {
                                                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                                            }

                                        });
                                    Task.Factory.StartNew(() =>
                                    {
                                        try
                                        {
                                            ManagementEventWatcher startWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
                                            startWatch.EventArrived
                                                                += delegate (object sender2, EventArrivedEventArgs e2)
                                                                {
                                                                    if (Int32.Parse(e2.NewEvent.Properties["ProcessID"].Value.ToString()) == Process.GetCurrentProcess().Id)
                                                                        return;
                                                                // startWatch.Stop();

                                                                myqs.Add(Process.GetProcessById(Int32.Parse(e2.NewEvent.Properties["ProcessID"].Value.ToString())).MainModule.FileName);

                                                                };
                                            startWatch.Start();
                                            //watcher.Start();
                                            while (true)
                                            {
                                                startWatch.WaitForNextEvent();
                                            }
                                        }catch (Exception em)
                                        {
                                            ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                                        }
                                    });
                                });
                        try
                        {
                            // Boot Time Scanner                        
                            if (File.Exists("bootS.dat"))
                            {
                                File.Delete("bootS.dat");
                                int count = SqlReaderWriter.MaxofRow("tblReportFor");
                                int rpfID = 8;
                                int iDate = Int32.Parse(string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                                string sTime = DateTime.Now.ToLongTimeString();
                                string repfor = "Boot Scan Run!";
                                SqlReaderWriter.ExecuteQuery("INSERT INTO tblReportFor (ID, ReportFID, Date, Time, ReportFor) VALUES (" + (count + 1) + ", " + rpfID + ", " + iDate + ", '" + sTime + "', '" + repfor + "\n')");

                                new gMbox("Boot Time Scan is on Proress!", "Information", 20000).Show();
                            }

                            bool waspp = AppSettings.IsSelfProtected;
                            AppSettings.IsSelfProtected = false;

                            //kill other instnaces of the program
                            Process[] instances = Process.GetProcessesByName("WebRoamAV");

                            if (instances.Length > 1)
                            {
                                foreach (var i in instances)
                                {
                                    if (i.Id != Process.GetCurrentProcess().Id)
                                    {
                                        i.Kill();
                                    }
                                }
                            }
                          
                                AppSettings.IsSelfProtected = waspp;

                            //Fill settings from config table (default value if not exist fill db with it)

                        


                            if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE frmName='Settings'") > 0)
                            {
                                if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='Settings' AND fieldID=4)") > 0)
                                {
                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='Settings' AND fieldID=4)").ToString() == "True")
                                    {
                                        AppSettings.IsSelfProtected = true;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'Settings', 4, 0)");
                                }

                                if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='Settings' AND fieldID=5)") > 0)
                                {
                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='Settings' AND fieldID=5)").ToString() == "True")
                                    {
                                        AppSettings.IsAutoUpdateOn = true;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'Settings', 5, 0)");
                                }


                                if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='Settings' AND fieldID=6)") > 0)
                                {
                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='Settings' AND fieldID=6)").ToString() == "True")
                                    {
                                        AppSettings.IsPasswProtectOn = true;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'Settings', 6, 0)");
                                }


                                if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='Settings' AND fieldID=7)") > 0)
                                {
                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='Settings' AND fieldID=7)").ToString() == "True")
                                    {
                                        AppSettings.IsAutoUpdateOn = true;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'Settings', 7, 0)");
                                }

                                if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='InNets' AND fieldID=8)") > 0)
                                {
                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='InNets' AND fieldID=8)").ToString() == "True")
                                    {
                                        AppSettings.IPSIDSOn = true;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'InNets', 8, 0)");
                                }

                                if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='InNets' AND fieldID=7)") > 0)
                                {

                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='InNets' AND fieldID=7)").ToString().ToLower() == "true")
                                    {
                                        AppSettings.IsNewsAlertOn = true;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'InNets', 7, 0)");
                                }

                                if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='InNets' AND fieldID=6)") > 0)
                                {
                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='InNets' AND fieldID=6)").ToString() == "True")
                                    {
                                        AppSettings.IsPhishingProtectOn = true;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'InNets', 6, 0)");
                                }



                                if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='InNets' AND fieldID=5)") > 0)
                                {
                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='InNets' AND fieldID=5)").ToString() == "True")
                                    {
                                        AppSettings.IsBrowsingProtectOn = true;
                                    }
                                    else
                                    {
                                        AppSettings.IsBrowsingProtectOn = false;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'InNets', 5, 1)");
                                }

                                /*if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='EDDv' AND fieldID=2)") > 0)
                                {
                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='EDDv' AND fieldID=2)").ToString() == "True")
                                    {
                                        AppSettings.IsAutoRunProtectOn = true;
                                    }
                                }*/
                                if (SqlReaderWriter.CountOfRow("tblExternalDevs") > 0)
                                {
                                    string rs = SqlReaderWriter.ExecuteScalar("SELECT AutProt FROM tblExternalDevs WHERE ID=1").ToString().ToLower();
                                    if (rs == "true" || rs == "1")
                                    {
                                        AppSettings.IsAutoRunProtectOn = true;
                                    }
                                    else if (rs != "")
                                    {
                                        AppSettings.IsAutoRunProtectOn = false;
                                    }

                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblExternalDevs (ID, AutProt) VALUES (1, 'FALSE')");
                                    //SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'EDDv', 2, 0)");
                                }
                                if (SqlReaderWriter.CountOfRow("tblExternalDevs") > 0)
                                {
                                    string rs = SqlReaderWriter.ExecuteScalar("SELECT ScanWmob FROM tblExternalDevs WHERE ID=1").ToString().ToLower();
                                    if (rs == "true" || rs == "1")
                                    {
                                        lock (EDDevices.DevLock)
                                            AppSettings.IsScanExternalDevsOn = true;
                                    }
                                    else if (rs != "")
                                    {
                                        lock (EDDevices.DevLock)
                                            AppSettings.IsScanExternalDevsOn = false;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblExternalDevs (ID, ScanWmob) VALUES (1, 'FALSE')");
                                }
                                App.AppSettings.tblFirewalProtLevel = 2;
                                if (SqlReaderWriter.CountOfRow("tblFirewalProt")>0)
                                {
                                    string s = SqlReaderWriter.ExecuteScalar("SELECT rFLevel FROM tblFirewalProt WHERE rFLevel IS NOT NULL ORDER BY ID DESC")?.ToString();
                                    if (!String.IsNullOrEmpty(s))
                                        App.AppSettings.tblFirewalProtLevel = Int32.Parse(s);
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteScalar("INSERT INTO tblFirewalProt (ID , rFLevel) VALUES (1, 2)");
                                }

                                /*if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='EDDv' AND fieldID=4)") > 0)
                                {
                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='EDDv' AND fieldID=4)").ToString() == "True")
                                    {
                                lock (EDDevices.DevLock)
                                    AppSettings.IsScanExternalDevsOn = true;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'EDDv', 4, 0)");
                                }*/


                                if (SqlReaderWriter.CountOfRow("tblExternalDevs") > 0)
                                {
                                    int? rt = Int32.Parse(SqlReaderWriter.ExecuteScalar("SELECT ScanExDrChoice FROM tblExternalDevs WHERE (ID='1')").ToString());
                                    if (rt == 1)
                                    {
                                        AppSettings.IsScanFullDrive = false;
                                    }
                                }
                                else
                                {
                                    AppSettings.IsScanFullDrive = true;
                                }

                                if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='EDDv' AND fieldID=4)") > 0)
                                {
                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='EDDv' AND fieldID=4)").ToString() == "True")
                                    {
                                        AppSettings.IsBlockUSBsOn = true;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'EDDv', 4, 0)");
                                }
                                string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
                                IniFile inf = new IniFile(file_ini);

                                if (inf.KeyExists("EXCLUDED_EXTENSION"))
                                {
                                    App.AppSettings.ExcludedExtensions = inf.Read("EXCLUDED_EXTENSION");
                                }

                                if (SqlReaderWriter.CountOfRow("tblExternalDevs") > 0)
                                {
                                    string rs = SqlReaderWriter.ExecuteScalar("SELECT DataThfProt FROM tblExternalDevs WHERE ID=1").ToString().ToLower();
                                    if (rs == "true" || rs == "1")
                                    {
                                        AppSettings.IsDataTheftOn = true;
                                    }
                                    else if (rs != "")
                                    {
                                        AppSettings.IsDataTheftOn = false;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblExternalDevs (ID, DataThfProt) VALUES (1, 'TRUE')");
                                }
                                /*if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='EDDv' AND fieldID=5)") > 0)
                                {
                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='EDDv' AND fieldID=5)").ToString() == "True")
                                    {
                                        AppSettings.IsDataTheftOn = true;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'EDDv', 5, 0)");
                                }*/

                                if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='EDDv' AND fieldID=1)") > 0)
                                {
                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='EDDv' AND fieldID=1)").ToString() == "True")
                                    {
                                        AppSettings.IsReadOnlyUSB = true;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'EDDv', 1, 0)");
                                }


                                if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='FFl' AND fieldID=4)") > 0)
                                {

                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='FFl' AND fieldID=4)").ToString().ToLower() != "true")
                                    {

                                        lock (App.LK_VP)
                                        {
                                            App.AppSettings.IsVirusProtectOn = false;
                                        }
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'FFl', 4, 1)");
                                }


                                AppSettings.IsWifiProtOn = false;
                                if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='fwSettings' AND fieldID=1)") > 0)
                                {
                                    if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='fwSettings' AND fieldID=1)").ToString() == "True")
                                    {
                                        AppSettings.IsWifiProtOn = true;
                                    }
                                }
                                else
                                {
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'fwSettings', 1, 0)");
                                }
                            }
                            else
                            {
                                SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'Settings', 4, 0)");
                                SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'InNets', 8, 0)");
                                SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'fwSettings', 1, 0)");
                            }

                            //wifi security
                            if (AppSettings.IsWifiProtOn)
                            {
                                Task.Factory.StartNew(() => WifiSafety.Start());
                            }
                            else
                            {
                                WifiSafety.Stop();
                            }
                            //Self protection
                            ProcessProtector.isOn = AppSettings.IsSelfProtected;
                            ProcessProtector pp = new ProcessProtector();
                            ProcessProtector.oluck = new object();
                            Task.Factory.StartNew(() => { do { Thread.Sleep(100); pp.ProcessProtect(); } while (true); });
                            // Set settings on/off in their windows & in our stored var
                            if (isFirstRun)
                            {
                                AppSettings apset = new AppSettings();

                                Task.Factory.StartNew(() =>
                                {
                                    Task.Delay(8000);

                                    apset.UpdateAll();
                                });
                                isFirstRun = false;
                            }
                        }
                        catch (Exception em)
                        {
                            ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                        }



                        //AppSettings.IsProtected = true;
                        /* if (Environment.GetCommandLineArgs().Length == 1)
                         {
                             new wndCustomScan().Show();
                             return;
                         }
                         if (Environment.GetCommandLineArgs()[1].ToLower() == "main")*/

                        /* else
                             new wndCustomScan(null, Environment.GetCommandLineArgs()[1]).Show();
                             */
                        //  if(isFirstRun)
                        //    trd.Join();
                    }







                     

                      /*  if (Environment.GetCommandLineArgs().Length < 2)
                        {
                            Environment.Exit(0);
                        }*/
                    // ProcessProtector.isOn = true;
                    // 
                    System.Windows.Forms.Timer tmr0 = new System.Windows.Forms.Timer();
                    tmr0.Interval = 60 * 1000*60;
                    tmr0.Tick += delegate (object sender1, EventArgs e1)
                    {
                        try
                        {
                            tmr0.Enabled = false;
                            tmr0.Stop();
                            const string fn0 = ".\\lsqdel.txt";
                            //  int sleep = 0;
                            string formt0 = "MM/dd/yyyy HH:mm tt";

                            if (!File.Exists(fn0))
                            {
                                File.WriteAllText(fn0, "0");
                            }
                            string last_read = File.ReadAllText(fn0).Trim();
                            DateTime date1 = DateTime.Now;


                            if (last_read == "0")
                            {
                                File.Delete(fn0);
                                File.WriteAllText(fn0, DateTime.Now.ToString(formt0));
                                tmr0.Enabled = true;
                                tmr0.Start();
                                return;
                            }
                            else
                            {
                                //  MessageBox.Show(date1.ToString()+Environment.NewLine+last_scan);
                                date1 = DateTime.ParseExact(last_read, formt0, CultureInfo.InvariantCulture);

                            }
                            string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
                            IniFile inf = new IniFile(file_ini);

                            int days = 30;
                            if (inf.KeyExists("DELETE_QUARANTINE_AFTER"))
                            {
                                int[] daray = {0, 10, 30, 90};
                                days = daray[Int32.Parse(inf.Read("DELETE_QUARANTINE_AFTER"))];
                            }
                            if (days>0 && (date1.Date - DateTime.Now.Date).TotalDays >= days)
                            {
                                File.Delete("wrTemp\\Qs.dat");
                                File.Delete(fn0);
                                File.WriteAllText(fn0, "0");                                
                            }

                            tmr0.Enabled = true;
                            tmr0.Start();
                        }
                        catch (Exception em)
                        {
                            ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                        }
                    };



                        System.Windows.Forms.Timer tmr = new System.Windows.Forms.Timer();
                        tmr.Interval = 60 * 1000;
                        tmr.Tick += delegate (object sender1, EventArgs e1)
                        {
                            try
                            {
                                tmr.Enabled = false;
                                tmr.Stop();                                
                                const string fn = ".\\lschscan.txt";
                                int sleep = 0;
                                string formt = "MM/dd/yyyy HH:mm tt";
                                if (!File.Exists(fn))
                                {
                                    File.WriteAllText(fn, "0");
                                }
                                string last_scan = File.ReadAllText(fn);
                                DateTime date1 = DateTime.Now;


                                if (last_scan == "0")
                                {

                                }
                                else
                                {
                                    //  MessageBox.Show(date1.ToString()+Environment.NewLine+last_scan);
                                    date1 = DateTime.ParseExact(last_scan, formt, CultureInfo.InvariantCulture);

                                }
                                string selc = "SELECT ScheduleItem, Frequency, FreqTime, FreqRepeat, FreqPriority, FreqUserName, FreqPassword, FreqRunIfmissed, ScanLocation FROM tblScanSchedule;";
                                //MessageBox.Show(selc);
                                DataTable dt = SqlReaderWriter.ReadQuery(selc);
                                int count = SqlReaderWriter.MaxofRow("tblReportFor", " WHERE ReportFID=7");
                                if (dt.Rows.Count == 0)
                                {
                                    tmr.Enabled = true;
                                    tmr.Start();
                                    return;
                                }
                                bool scanN = false;
                                List<string> paths = new List<string>();
                                foreach (DataRow d in dt.Rows)
                                {
                                    //var s0 = d[0].ToString();
                                    //   var s5 = d[5].ToString();
                                    // var s6 = d[6].ToString();
                                    //   MessageBox.Show(d[1] + Environment.NewLine + d[3]);
                                    var s1 = Int32.Parse(d[1].ToString()) / 100;
                                    var s1_2 = Int32.Parse(d[1].ToString()) % 100;
                                    var s3 = (Int32.Parse(d[3].ToString()) - 100000);
                                    if (s3 < 0)
                                        s3 += 100000;
                                    s3 /= 10000;
                                    //   MessageBox.Show("1");
                                    var s3_2 = (Int32.Parse(d[3].ToString()) - 100000);
                                    if (s3_2 < 0)
                                        s3_2 += 100000;
                                    s3_2 %= 10000;
                                    string[] dds = d[2].ToString().Split(',');
                                    string[] hm = dds[0].Split(':');
                                    var s2 = (Int32.Parse(hm[0]));
                                    var s2_1 = (Int32.Parse(hm[1]));
                                    Console.WriteLine(s3 + " " + (s2_1 % s3_2) + " " + (DateTime.Now.Minute % s3_2));

                                    if (s1 == 0) // daily
                                    {
                                        if ((s3 == 1 ? DateTime.Now.Hour % s3_2 == s2 % s3_2 : true) && (DateTime.Now.Minute.ToString() == hm[1] || (s3 == 0 ? (DateTime.Now.Minute - s3_2 > 0 && DateTime.Now.Minute % s3_2 == s2_1 % s3_2) : false)))
                                        {
                                            //DateTimeFormatInfo dinfo = DateTimeFormatInfo.CurrentInfo;
                                            //DateTime date2 = dinfo.Calendar.AddDays(date1, Int32.Parse(dds[1]));
                                            if (last_scan == "0" || (date1.Day - DateTime.Now.Day) % Int32.Parse(dds[1]) == 0)
                                            {
                                                scanN = true;
                                                if (!String.IsNullOrWhiteSpace(d[8].ToString()))
                                                    paths.Add(d[8].ToString());
                                                if (s3_2 != 0) //repeat after
                                                {
                                                    sleep = 1;
                                                    if (s3 == 1)
                                                        sleep *= 60;
                                                    sleep *= 60000;
                                                }
                                                //continue;
                                            }
                                            if ((String.IsNullOrEmpty(d[2].ToString())))
                                            {
                                                if (AppSettings.IsBoot)
                                                {
                                                    scanN = true;
                                                    if (!String.IsNullOrWhiteSpace(d[8].ToString()))
                                                        paths.Add(d[8].ToString());
                                                    if (s3_2 != 0) //repeat after
                                                    {
                                                        sleep = 1;
                                                        if (s3 == 1)
                                                            sleep *= 60;
                                                        sleep *= 60000;
                                                    }
                                                    //continue;
                                                }
                                            }

                                        }


                                    }
                                    else //weekly
                                    {
                                        if (DateTime.Now.DayOfWeek == (DayOfWeek)(s1_2) && ((DateTime.Now.DayOfYear - date1.DayOfYear) % (7 * Int32.Parse(dds[1]))) == 0)
                                        {
                                            if ((String.IsNullOrEmpty(d[2].ToString())))
                                            {
                                                if (AppSettings.IsBoot)
                                                {
                                                    scanN = true;
                                                    if (!String.IsNullOrWhiteSpace(d[8].ToString()))
                                                        paths.Add(d[8].ToString());
                                                    if (s3_2 != 0) //repeat after
                                                    {
                                                        sleep = 1;
                                                        if (s3 == 1)
                                                            sleep *= 60;
                                                        sleep *= 60000;
                                                    }
                                                    //continue;
                                                }
                                            }
                                            if ((s3 == 1 ? DateTime.Now.Hour % s3_2 == s2 % s3_2 : true) && ((s3 == 0 ? (DateTime.Now.Minute - s3_2 > 0 && DateTime.Now.Minute % s3_2 == s2_1 % s3_2) : false) || DateTime.Now.Minute.ToString() == hm[1]))
                                            {
                                                scanN = true;
                                                if (!String.IsNullOrWhiteSpace(d[8].ToString()))
                                                    paths.Add(d[8].ToString());
                                                if (s3_2 != 0) //repeat after
                                                {
                                                    sleep = 1;
                                                    if (s3 == 1)
                                                        sleep *= 60;
                                                    sleep *= 60000;
                                                }
                                                //continue;
                                            }
                                        }

                                    }
                                    /*if (!string.IsNullOrWhiteSpace(d[2].ToString()))
                                    {
                                        string[] spt = d[2].ToString().Split(',');
                                        var s2 = spt[0];
                                        var s2_2 = Int32.Parse(spt[1]);
                                    }*/
                                }
                                if (scanN)
                                {
                                    int rpfID = 7;
                                    int iDate = Int32.Parse(string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                                    string sTime = DateTime.Now.ToLongTimeString();
                                    string repfor = "Scan Scheduler";
                                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblReportFor (ID, ReportFID, Date, Time, ReportFor) VALUES (" + (count + 1) + ", " + rpfID + ", " + iDate + ", '" + sTime + "', '" + repfor + "\n')");
                                    File.Delete(fn);
                                    File.WriteAllText(fn, DateTime.Now.ToString(formt));
                                    int rcount = 1;
                                    GRowsCUS[] rows = new GRowsCUS[rcount];
                                    //var ce = new ClamAVEngine();
                                    for (int i = 0; i < rcount; i++)
                                    {
                                        rows[i] = new GRowsCUS { PathF = paths[i], SubF = "No" };
                                    }
                                    Task.Factory.StartNew(() =>
                                    {
                                        while (true)
                                        {
                                            Task.Factory.StartNew(() =>
                                            {
                                                Dispatcher.Invoke(() => new wMBox("Scan Schedule Running Now! Time is " + DateTime.Now.ToString(), "Info").Show());

                                                string s = "";
                                                var ta = ProcessProtector.cen.Scan(rows, 10);
                                                if(ta.Count()>0)
                                                foreach (var t in ta)
                                                {

                                                    s += t;
                                                }

                                                string St = s;

                                                if (!String.IsNullOrWhiteSpace(St) && (St.Contains("$")) && St.Contains("Malware"))
                                                {
                                                    Dispatcher.Invoke(() => new wMBox(St.Replace("$", ":"), "Alert").Show());
                                                    string[] infs = St.Split('\n').Where(a => a.Contains("$") && a.Contains("Malware")).ToArray();
                                                }
                                            });
                                            if (sleep == 0)
                                                break;
                                            Thread.Sleep(sleep);
                                        }
                                    });
                                }
                                tmr.Enabled = true;
                                tmr.Start();
                            }


                            catch (Exception em)
                            {
                                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                            }
        };
                        tmr.Enabled = true;
                        tmr.Start();

                        Task.Factory.StartNew(() => { USBStore.Run(null); });
                        /*Timer tmr = new Timer((o) => {

                            if (AppSettings.IsBlockUSBsOn)
                                File.Create("permListdevs.conf");
                            else
                            {
                                if(File.Exists("permListdevs.conf"))
                                File.Delete("permListdevs.conf");
                            }
                        },null, 0, 3000);*/

                    }
                }


                catch (Exception em)
                {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(em, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + trace.GetFrame(trace.FrameCount - 1).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                }
            
        }


        bool _isExplicitClose = false;
        protected override void OnStartup(StartupEventArgs e)
            {
                try
                {
                    base.OnStartup(e);
                //MessageBox.Show(Environment.CommandLine);
                    bool registered = false;
                    string[] license;
                    string lictx = "";
                    string[] lfile = null;
                string licenseF = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent.FullName + "\\gui\\license";
                if (File.Exists(licenseF))
                    {
                        lfile = File.ReadAllLines(licenseF);
                        lictx = lfile[0].Trim();
                    }
                    if (lfile == null || lfile.Count() < 3)
                    {
                        lictx = "";
                        MessageBox.Show("Invalid License!");
                        _isExplicitClose = true;
                        return;
                    }

                    string[] licontent = new string[3];
                    licontent[0] = lfile.ElementAt(0);
                    licontent[2] = lfile.ElementAt(2);
                    string macAddr =
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where (nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet || nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                select nic?.GetPhysicalAddress().ToString()
            )?.FirstOrDefault();
                    if (macAddr != null)
                    {
                        licontent[1] = LicenseClass.CreateMD5(macAddr);
                        File.WriteAllLines(licenseF, licontent);
                        license = Encoding.UTF8.GetString(LicenseClass.Aes256Decrypt(LicenseClass.SHexToByteArray(LicenseClass.StrToHex(lictx)))).Split(' ');
                        //1 product id
                        //4 license version
                        if (!(license?.Length == 6 && license[1] == "1001" && license[4] == "1"))
                        {
                            MessageBox.Show("Invalid License!");
                            _isExplicitClose = true;
                            return;
                        }
                        /*string mc = (from nic2 in NetworkInterface.GetAllNetworkInterfaces()
                                     where nic2.OperationalStatus == OperationalStatus.Up
                                     select nic2?.GetPhysicalAddress().ToString()
                )?.FirstOrDefault();*/


                        if (lfile != null && lfile?.Length > 2 && lfile[2] != "")
                        {
                            string regs = lfile[2];

                            string[] regsarr = Encoding.UTF8.GetString(LicenseClass.Aes256Decrypt(LicenseClass.SHexToByteArray(LicenseClass.StrToHex(regs)))).Split(' ');
                           //  MessageBox.Show((regsarr[5]) +Environment.NewLine+ macAddr);
                          //  MessageBox.Show(regsarr.Length.ToString());
                            if (regsarr?.Length == 6 && regsarr[1] == "1001" && regsarr[4] == "1" && LicenseClass.CreateMD5(regsarr[5]) == licontent[1])
                            {
                            string year = DateTime.Now.Year.ToString();//"20" + regsarr[2].Substring(0, 2);
                            string month = DateTime.Now.Month.ToString("D2");// regsarr[2].Substring(2, 2);
                                string period = regsarr[2];
                                DateTime dt = new DateTime(Int32.Parse(year), Int32.Parse(month), DateTime.Now.Day);
                                dt = dt.AddMonths(Int32.Parse(period));
                            //    MessageBox.Show(DateTime.Now.Subtract(dt).Days.ToString());
                                if (DateTime.Now.Subtract(dt).Days <= 0)
                                    registered = true;
                            }

                        
                    }
                    IsLicenseValid = registered;

                if (!IsLicenseValid && (DateTime.Now.Month > 11 && DateTime.Now.Year > 2022))
                    return;
                    // Main window of our program
                    var ww = new MainWindow();

                    // show main window run from desktop
                    if (Environment.GetCommandLineArgs().Length > 1)
                    {

                        if (Environment.GetCommandLineArgs()[1].ToLower() == "main")
                        {
                            ww.Show();
                        }
                        else
                        {
                            // windows startup run
                            if (Environment.GetCommandLineArgs().Select((a) => a.ToLower()).Equals("start"))
                            {
                                ww.Hide();
                                AppSettings.IsBoot = true;
                            }
                        }
                    }
                    else
                    {
                        ww.Show();
                    }
                    // we need a tray 
                    //  if (Environment.GetCommandLineArgs().Length < 2||Environment.GetCommandLineArgs()[1].ToLower() == "main" || Environment.GetCommandLineArgs()[1].ToLower() == "start")
                    //{
                    MainWindow.Closing += MainWindow_Closing;

                        _notifyIcon = new System.Windows.Forms.NotifyIcon();
                        _notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
                        _notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(
                         System.Reflection.Assembly.GetEntryAssembly().ManifestModule.Name);
                        _notifyIcon.Visible = true;
                   // }
                    if (!registered)
                    {
                        var contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip();
                        contextMenuStrip2.Items.Add("Activate...").Click += (s, e2) => { Process.Start("https://webroam.com/renew"); };

                        if ((DateTime.Now.Month > 11 && DateTime.Now.Year > 2022))
                        {
                            _notifyIcon.ContextMenuStrip = contextMenuStrip2;
                            _notifyIcon.Visible = true;
                          /*  string[] flines = File.ReadAllLines(".\\app_config.ini");

                            int slevel = int.Parse(flines?.FirstOrDefault((a) => a.Contains("SecurityLevel")).Replace("SecurityLevel", "").Replace("=", "").Trim());
                            var s = (from content in flines where !content.Contains("SecurityLevel") select content)?.FirstOrDefault();
                            string filecntnt = string.IsNullOrEmpty(s) ? "" : String.Join(Environment.NewLine, s);
                            filecntnt += "\nSecurityLevel=3";
                            File.WriteAllText(".\\app_config.ini", filecntnt, new UTF8Encoding(false));*/
                            /*try
                            {
                                    Process p = Process.GetProcessesByName("WrArServ")[0];
                                    FreeConsole();
                                    if(AttachConsole((uint)p.Id)){
                                    //uint spid= p.SessionId;

                                    // MessageBox.Show(Process.GetProcessesByName("WRAREngine").Length.ToString() + Environment.NewLine + spid);
                                    SetConsoleCtrlHandler(null, true);

                                    //                            GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, p.SessionId);
                                    GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, 0);
                                    //
                                    Thread.Sleep(2000);
                                    FreeConsole();
                                    SetConsoleCtrlHandler(null, false);}
                                }
                            catch (Exception em) { ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
                            */
                            new ProductKeyForm().Show();
                            return;

                        }                        
                       // new ProductKeyForm().Show();
                    }
                    if (Environment.GetCommandLineArgs().Length<2||Environment.GetCommandLineArgs()[1].ToLower() == "main" || Environment.GetCommandLineArgs()[1].ToLower() == "start")
                    {
                        CreateContextMenu();
                    }
                    }
                }
                catch (Exception em)
                {
                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                }
            }

        private void CreateContextMenu()
        {
            try
            { 
            _notifyIcon.ContextMenuStrip =
              new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("MainWindow...").Click += (s, e) => ShowMainWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Create Password...").Click += (s, e) => {
                var itm = s as System.Windows.Forms.ToolStripItem;
                if (itm.Text.Contains("Create"))
                {
                    ShowCreatePassword();
                    itm.Text = "Remove Password";
                }
                else
                {
                    
                    string[] flines = File.ReadAllLines(".\\app_config.ini");
                    var lines = flines.Where(st => !st.Contains("Password"));
                    File.WriteAllLines(".\\app_config.ini", lines.ToArray());
                    gSettings.Password = "";
                    itm.Text = "Create Password...";
                }
            };
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication(); //must be changed when production is ready ******
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void ExitApplication()
        {
            
            if (MessageBox.Show("Are you sure you want to exit?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ProcessProtector.isOn = false;
                _isExit = true;

                foreach(var s in ActivateForm.sw.Values)
                {
                    s.Close();
                }
                
                ctsNetw?.Cancel();
                try
                {
                    try { listener.Disconnect(true); } catch { } finally { listener.Close(); }
                }
                finally
                {
                    Dispatcher.Invoke(() =>
                    {
                        MainWindow.Close();
                        _notifyIcon.Dispose();
                        _notifyIcon = null;
                    });
                    KillProcessAndChildren(Process.GetCurrentProcess().Id);
                }


                // myTC.Stop();



                //Environment.Exit(0);
                //myTC.Stop();
                /*myTC.Server.Shutdown(SocketShutdown.Both);
                try
                {
                    myTC.Server.Disconnect(true);
                }
                catch
                { }
                
                myTC.Stop();
                */

            }
        }
        // show main window
        private void ShowMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
                bool topm = MainWindow.Topmost;
                MainWindow.Topmost = true;
                MainWindow.Focus();
                MainWindow.Topmost = topm;
            }
            else
            {
                MainWindow.Show();
                MainWindow.Focus();
                bool topm = MainWindow.Topmost;
                MainWindow.Topmost = true;
                MainWindow.Focus();
                MainWindow.Topmost = topm;                
            }
        }
        CreatePassword createPassword = null;
        private void ShowCreatePassword()
        {
            if (createPassword!=null)
            {
                if (createPassword.WindowState == System.Windows.Forms.FormWindowState.Minimized)
                {
                    createPassword.WindowState = System.Windows.Forms.FormWindowState.Normal;
                }
                createPassword.Activate();
                bool topm = createPassword.TopMost;
                createPassword.TopMost = true;
                createPassword.Focus();
                createPassword.TopMost = topm;
            }
            else
            {
                createPassword = new CreatePassword();
                createPassword.Show();
                createPassword.Focus();
                bool topm = createPassword.TopMost;
                createPassword.TopMost = true;
                createPassword.Focus();
                createPassword.TopMost = topm;
                createPassword.FormClosed += CreatePassword_FormClosed;
            }
        }

        private void CreatePassword_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            createPassword = null;
        }

        //  hide if NOT exist Only close
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                MainWindow.Hide(); // A hidden window can be shown again, a closed one not
            }
         
        }
        
    }
}
