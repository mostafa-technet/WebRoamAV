using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using EnumerateFile;
using System.Management;

namespace WebRoamAV
{
    /// <summary>
    /// Interaction logic for wndFullScan.xaml
    /// </summary>
    /// 

   
    public static partial class Commands
    {
        public static readonly RoutedCommand btnfPausePressed = new RoutedCommand();
        public static readonly RoutedCommand btnfStopPressed = new RoutedCommand();
        public static readonly RoutedCommand btnfSkipFilePressed = new RoutedCommand();
        public static readonly RoutedCommand btnfSkipFolderPressed = new RoutedCommand();
    }

    public partial class wndFullScan : Window
    {

        private ColorAnimation coloarn;
        string mw;
        private List<GRowsCUS> _files = null;
        bool Nil = true;
        public wndFullScan()
        {
            Nil = true;
            mw = "";
        }
        private static bool _showonly = false;
        private string _finished = "Finished.";
        private string _interrupted = "Interrupted";
        public wndFullScan(string parent, bool showonly = false)
        {
            lock (shl)
            {
                _showonly = showonly;
            }
            InitializeComponent();
            mw = parent;
            this.Top = MainWindow.parentTop;
            this.Left = MainWindow.parentLeft;
            this.MouseDown += MainWindow_MouseDown;
        }
        public wndFullScan(string parent, bool nil, ref List<GRowsCUS> files)
        {
            lock (shl)
            {
                _showonly = false;
            }
            Nil = nil;
            if (!Nil)
                _files = files;
            InitializeComponent();

            mw = parent;
            this.Top = MainWindow.parentTop;
            this.Left = MainWindow.parentLeft;
            this.MouseDown += MainWindow_MouseDown;
        }

        public void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.GetPosition(this).Y < 70)
                this.DragMove();
        }
        public void buttonMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Coloarn_Completed(object sender, EventArgs e)
        {
            /*btnDeleteAll.Background = new SolidColorBrush(Colors.Green);
            coloarn = new ColorAnimation();
            coloarn.From = Colors.Green;
            coloarn.To = (Color)ColorConverter.ConvertFromString("#A0008000");
            coloarn.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            coloarn.Completed -= Coloarn_Completed;*/
        }


        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            Control target = e.Source as Control;

            if (target != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            if (e.Parameter != null && e.Parameter.ToString() == "Resume")
            {
                if (lblPause.Content.ToString() == "_Pause")
                {
                    System.Media.SystemSounds.Beep.Play();
                    return;
                }
                else
                {
                    btn = btnPause;
                    lblPause.Content = "_Pause";
                }
            }
            else if (btn == btnPause)
            {
                if (lblPause.Content.ToString() == "_Pause")
                    lblPause.Content = "_Resume";
                else
                {
                    System.Media.SystemSounds.Beep.Play();
                    return;
                }

            }

            if (btn.IsEnabled)
            {
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
                coloarn = new ColorAnimation();
                coloarn.To = Colors.Green;
                coloarn.From = (Color)ColorConverter.ConvertFromString("#A0008000");
                coloarn.Duration = new Duration(TimeSpan.FromSeconds(0.1));
                coloarn.Completed += Coloarn_Completed;
                btn.Background.BeginAnimation(SolidColorBrush.ColorProperty, coloarn);
                this.GetType().GetMethod(btn.Name + "_Click").Invoke(this, new object[] { sender, e });

            }


        }


        public class Item
        {
            public string File { get; set; }
            public string Status { get; set; }
            public string Action { get; set; }
        }

        ManualResetEvent _wait = new ManualResetEvent(true);
        static CancellationTokenSource ts = null;
        static int bskipfilef = 0;
        static object lk;
        CancellationTokenSource ts2;
        CancellationToken ct2;
        Task tGlobal;
        public static object shl = new object();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            link1.IsEnabled = false;
            link2.IsEnabled = false;
            link3.IsEnabled = false;
            link4.IsEnabled = false;
            btnPause.IsEnabled = btnSkipFile.IsEnabled = btnSkipFolder.IsEnabled = btnStop.IsEnabled = false;
            lblbtnSkipFile.Foreground = lblbtnSkipFolder.Foreground = lblbtnStop.Foreground
     = lblPause.Foreground = System.Windows.Media.Brushes.LightGray;
            ButtonMn.Foreground = System.Windows.Media.Brushes.Gray;
            ButtonMn.IsEnabled = false;

            prgbar.Foreground = new SolidColorBrush(Colors.Orange);
            lblExtracting.Content = "Loading..";
            //Ld();
            lock (shl)
            {
                if (_showonly)
                {
                    _showonly = false;
                    return;
                }
            }

            tGlobal = Task.Factory.StartNew(() =>
            Ld());

        }
        public void LoadNow(List<GRowsCUS> files)
        {
            lock (shl)
            {

                _files = files;
                _showonly = false;
            }
            this.Dispatcher.Invoke(() => Window_Loaded(null, null));
        }

        // Adds an ACL entry on the specified directory for the specified account.
        public static void AddDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            // Create a new DirectoryInfo object.
            DirectoryInfo dInfo = new DirectoryInfo(FileName);

            // Get a DirectorySecurity object that represents the 
            // current security settings.
            DirectorySecurity dSecurity = dInfo.GetAccessControl();

            // Add the FileSystemAccessRule to the security settings. 
            dSecurity.AddAccessRule(new FileSystemAccessRule(Account,
                                                            Rights,
                                                            ControlType));

            // Set the new access settings.
            dInfo.SetAccessControl(dSecurity);

        }

        // Removes an ACL entry on the specified directory for the specified account.
        public static void RemoveDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            // Create a new DirectoryInfo object.
            DirectoryInfo dInfo = new DirectoryInfo(FileName);

            // Get a DirectorySecurity object that represents the 
            // current security settings.
            DirectorySecurity dSecurity = dInfo.GetAccessControl();

            // Add the FileSystemAccessRule to the security settings. 
            dSecurity.RemoveAccessRule(new FileSystemAccessRule(Account,
                                                            Rights,
                                                            ControlType));

            // Set the new access settings.
            dInfo.SetAccessControl(dSecurity);

        }
        static bool isReceiving = false;
        static TcpListener tc;
        static Socket s;
        Process prc = null;
        int itrts = 0, ireprd = 0, iqarn = 0;
        float percent = 0;
        void Receive(string result, int dircount, object obj, CancellationTokenSource cts)
        {
            try
            {
                // int iscn = 0, iarc = 0, idel = 0, ierr = 0;

                /* if (isReceiving)
                 {
                     return;
                 }
                 isReceiving = true;
                 CancellationToken ct = (CancellationToken)obj;




                 tc = new TcpListener(IPAddress.Any, 2551);
                     tc.Start();


                    while (!ct.IsCancellationRequested)
                     {

                     try
                     {
                         s = tc.AcceptSocket();
                         byte[] buf = new byte[1024 * 20];
                         if (s.Receive(buf) > 0)
                         {
                         */
                string texts = result;// System.Text.Encoding.Unicode.GetString(buf);
                                      //MessageBox.Show(texts);
                                      // Task.Factory.StartNew(delegate { this.Dispatcher.Invoke(() => MessageBox.Show(texts)); });
                /* var mchs = Regex.Matches(texts, @"[\r\n]+[a-zA-Z]+\*f\b");
                 itrts += mchs.Count;
                 foreach (Match v in mchs)
                 {*/
                string[] infs = texts.Split('\n').Where(a => a.Contains("$")).ToArray();// new string[] { "Infected" }, 1000, StringSplitOptions.RemoveEmptyEntries);
                                                                                        // MessageBox.Show(infs.Length.ToString());
                for (int j = 0; j < (infs.Length - (infs.Length % 4)); j += 4)
                {
                    //string[] str = s.Split('\n');
                    string[] pref = new string[] { "File", "Folder", "Malware", "MType" };
                    List<KeyValuePair<string, string>> dbk = new List<KeyValuePair<string, string>>();

                    int i = 0;
                    for (; i < 4; i++)
                    {

                        string[] kv = infs[j + i].Split('$');
                        //dbk = kv.ToLookup<string, string>()
                        if ((kv == null || kv.Length < 2) || string.IsNullOrWhiteSpace(kv[0]))
                            continue;
                        dbk.Add(new KeyValuePair<string, string>(kv[0], kv[1]));
                    }

                    var lkdbk = dbk.ToLookup(p => p.Key);
                    int count = 0;
                    Dictionary<string, string> dc = new Dictionary<string, string>();
                    foreach (var g in lkdbk)
                    {
                        dc.Add(g.Key, g.ElementAt(count / 4).Value);



                        count++;
                        if (count % 4 == 0)
                        {
                            string rd = "";

                            rd = dc["Folder"].EndsWith("\\") ? dc["Folder"] : (dc["Folder"] + "\\") + (dc["File"].LastIndexOf('\\') == -1 ? dc["File"] : dc["File"].Substring(dc["File"].LastIndexOf('\\') + 1));
                            bool contains = false;
                            foreach (var item in this.DataGridMain.Items)
                            {
                                if (((Item)item).File == rd)
                                {
                                    contains = true;
                                    break;
                                }
                            }
                            //   MessageBox.Show(infs.Length.ToString());
                            /*       if (!File.Exists(rd))
                               {
                                   //iqarn--;
                                   continue;
                               }*/
                            if (contains)
                                continue;
                            //}
                            itrts++;
                            this.Dispatcher.Invoke(() =>
                                        {
                                            this.DataGridMain.Items.Add(new Item() { File = rd, Status = dc["Malware"] + "/" + dc["MType"], Action = "Quarantined" });
                                        });

                            new Quarantine().QuaFile(rd, dc["Malware"]);
                            iqarn++;
                            //dc = new Dictionary<string, string>();

                        }

                    }
                    // v.Value.Replace("*f", ""));
                    //MessageBox.Show("");     


                }
                //Console.WriteLine(System.Text.Encoding.Unicode.GetString(buf));

                /* string scn = iscn.ToString(), arc = iarc.ToString(), trts = itrts.ToString(), reprd = ireprd.ToString(), qarn = iqarn.ToString(), del = idel.ToString(), err = ierr.ToString();
                 string status = String.Format("File Scanned\t\t\t{0}\t\t\tFiles Deleted\t\t\t{5}\nArchive/Packed\t\t\t{1}\t\t\tI/O errors\t\t\t{6}\nThreats Detected\t\t\t{2}\nFiles repaired\t\t\t{3}\nFile Quarantined\t\t\t{4}",
                     scn, arc, trts, reprd, qarn, del, err);
                 this.Dispatcher.Invoke(() =>
                 {
                     lblStatus.Content = status;
                 });*/
                /* percent += 20 * 100 / (float)dircount;
                 this.Dispatcher.BeginInvoke(new Action(() =>
                 {
                     this.prgbar.Value = percent;
                 }));*/
                /*    }
                }
                catch { }
                }*/

                // Console.WriteLine(System.Text.Encoding.ASCII.GetString(buf));
                //Console.ReadLine();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
        Thread gt1 = null;
        object lkDr = new object();
        string gDr = "";
        static object glk = new object();
        List<GRowsCUS> files2 = new List<GRowsCUS>();
        string msg([CallerLineNumber]
        int number = 0)
        {
            return number.ToString();
        }
        public static long GetDirectorySize(string parentDirectory)
        {
            return new DirectoryInfo(parentDirectory).GetFiles("*.*", SearchOption.AllDirectories).Sum(file => file.Length);
        }
        public static string GetFreeSpace(string driveLetter)
        {
            ManagementObject disk = new ManagementObject($"select * from Win32_Volume where DriveLetter=\"{driveLetter}:\"");
            disk.Get();
            string freespace = disk["Capacity"].ToString();
            return freespace;
        }
    async   void Ld()
            {

            try
            {
                if (ProcessProtector.cen == null)
                    ProcessProtector.cen = new ClamAVEngine();
                //var cen = ProcessProtector.cen;
                lk = new object();
                ts2 = new CancellationTokenSource();
                CancellationToken ct2 = ts2.Token;
                //mre.Set();

                //string tid = NanoScan.Scan("E:\\\\AV\\\\DLP\\\\Nano\\\\nanoavsdk\\\\files\\\\clean.txt").Result;
                List<GRowsCUS> files2;
                lock (shl)
                {
                    if (_files == null)
                    {
                        files2 = new List<GRowsCUS>();
                        var drvs = Environment.GetLogicalDrives().ToList();
                        foreach (var v in drvs)
                            files2.Add(new GRowsCUS { PathF = v, SubF = Directory.Exists(v)?"Yes":"No" });
                    }
                    else
                        files2 = _files;
                }
               // MessageBox.Show(files2.Count.ToString());
                ts = new CancellationTokenSource();
                CancellationToken ct = ts.Token;
                this.Dispatcher.Invoke(() =>
                {
                    this.prgbar.Value = 0;
                    ButtonMn.IsEnabled = btnPause.IsEnabled = btnSkipFile.IsEnabled = btnSkipFolder.IsEnabled = btnStop.IsEnabled = true;
                    lblbtnSkipFile.Foreground = lblbtnSkipFolder.Foreground = lblbtnStop.Foreground
         = lblPause.Foreground = System.Windows.Media.Brushes.White;
                    ButtonMn.Foreground = System.Windows.Media.Brushes.Green;
                });
                // List<GRowsCUS> fls = new List<GRowsCUS>();


                /* var act=((Action)delegate ()
                 {*/
                int iscn = 0, iarc = 0, idel = 0, ierr = 0;
                /* for (int i = 0; i < files2.Count; i++)
                 {

                     lock (glk)
                     {
                         if (brk)
                         {
                             brk = false;
                             Dispatcher.Invoke(() => { prgbar.Value = 100; });
                                 return;
                         }
                     }

                     if ((!string.IsNullOrEmpty((string)files2.ElementAt(i).PathF)))
                     {
                         try
                         {
                             if (System.IO.Directory.Exists((string)files2.ElementAt(i).PathF) || System.IO.File.Exists((string)files2.ElementAt(i).PathF))
                                 fls.Add(files2.ElementAt(i));
                         }
                         catch (Exception em)
                         {
                             ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName()+" "+new StackFrame(1, true).GetFileLineNumber()+Environment.NewLine+em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                         }

                     }
                 }*/

                Dispatcher.Invoke(() => { lblExtracting.Content = "Waiting..."; prgbar.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8D429A4E")); });

                string zfrmts = "7Z.BZ2.GZ.RAR.TAR.ZIP".ToLower();
                List<string> zlist = zfrmts.Split('.').ToList();
                float maxlevel = 4.0f;
                int ifs = 0;
                if (files2 != null && files2.Count > 0)
                    foreach (var f1 in files2.ToList())
                    {
                        Dispatcher.Invoke(() =>
                        {
                            lblFolder.Content = System.IO.Path.GetDirectoryName(f1.PathF);
                        });
                        lock (glk)
                        {
                            if (brk)
                            {
                                brk = false;
                                Dispatcher.Invoke(() =>
                                {
                                    lblFolder.Content = System.IO.Path.GetDirectoryName(f1.PathF);
                                    //this.prgbar.Value = 100;
                                    //lblExtracting.Content = _finished;
                                    // MessageBox.Show("Success!");
                                });
                                //return;
                                break;
                            }
                        }
                        GRowsCUS rf = f1;//.Replace(@"\\", @"\").Replace(@"\\", @"\");
                                         //this.Dispatcher.Invoke(() => { this.prgbar.Value = 100; });
                        float percent = 0;
                        List<string> Listdirs = new List<string>();
                        //List<string> mydirs = new List<string>();
                        if (System.IO.Directory.Exists(rf.PathF) || Environment.GetLogicalDrives().Contains(rf.PathF))
                        {
                            //AddDirectorySecurity(rf, Environment.UserDomainName+@"\"+Environment.UserName, FileSystemRights.ReadData, AccessControlType.Allow);
                            //AddDirectorySecurity(rf, Environment.UserDomainName + @"\" + "Administrator", FileSystemRights.ReadData, AccessControlType.Allow);

                            //List<string> l3 = new List<string>();
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                lblFolder.Content = rf.PathF;
                            }));
                            //File.Create("e:\\test6.txt").Close();
                            //var mydirs = System.IO.Directory.GetDirectories(rf, "*", SearchOption.AllDirectories);
                           
                          //  mydirs.Add(rf.PathF);
                            int ilsdr = 0;
                            if (rf.SubF == "Yes")
                            // mydirs.AddRange(Directory.GetDirectories(rf.PathF, "*", SearchOption.TopDirectoryOnly).Where((a) => !new DirectoryInfo(a).Attributes.HasFlag(FileAttributes.Hidden) && !new DirectoryInfo(a).Attributes.HasFlag(FileAttributes.System)));
                            {

                                //float Listdirs_count = 1000.0f;
                                Listdirs.Add(rf.PathF);
                                double iprcnt = 1;
                                long sz = 1;
                                
                                if (rf.PathF.Count(c=>c=='\\') < 2)
                                {
                                    var searcher = new ManagementObjectSearcher(@"select * from Win32_Volume");
                                    foreach (ManagementObject obj in searcher.Get())
                                    {                                      
                     //                   MessageBox.Show("Name: "+ obj["DriveLetter"]);
                       //                 MessageBox.Show("Size: "+ (((obj["Capacity"].ToString()))));

                                        if (obj["DriveLetter"].ToString() == rf.PathF.Substring(2))
                                        {
                                            sz = ((Int32.Parse(obj["Capacity"].ToString())));
                                            break;
                                        }
                                    }

                                 //   MessageBox.Show(rf.PathF);
                                    
                                 
                                }
                                else
                                {
                                    try
                                    {
                                        sz = GetDirectorySize(rf.PathF);
                                    }
                                    catch { }

                                }
                                iprcnt = (165000 * (sz / 1000000000.0) / 500.0);
                                Dispatcher.Invoke(() => { lblExtracting.Content = "Scanning...";/* prgbar.Value = 0;*/ });
                                FileDirectoryEnumerable myEnum1 = new FileDirectoryEnumerable();
                                do
                                {
                                    lock (lk)
                                    {
                                        if (Listdirs.Count == ilsdr)
                                            break;

                                        myEnum1.SearchPath = Listdirs[ilsdr];
                                    }
                                    myEnum1.SearchPattern = "*";
                                    myEnum1.ReturnStringType = true;
                                    myEnum1.SearchForFile = false;
                                    myEnum1.SearchForDirectory = true;
                                    myEnum1.ReturnStringType = true;
                                    myEnum1.ThrowIOException = false;
                                    
                               //         bool end = false;
                                    // MessageBox.Show(":" + myEnum1.SearchPath);
                                    try
                                    { 
                                        foreach (string strDirName in myEnum1.AsParallel())
                                        { //MessageBox.Show(strDirName);
                                            Listdirs.Add(myEnum1.SearchPath+"\\"+strDirName);
                                           // ilsdr++;                                    
                                          await Task.Run(() =>
                                            {
                                                try
                                                {
                                                    string fscn = "";
                                                    string Listdirs_ilsdr = "";
                                                    lock (lk)
                                                    {
                                                        fscn = (Listdirs[ilsdr] + "\\" + strDirName);
                                                        Listdirs_ilsdr = Listdirs[ilsdr];
                                                    }
                                                    float level = 0;
                                                    //  mydirs = Listdirs;
                                                    int directoryCount = Listdirs.Count + 1;//mydirs.Count + 1;
                                                    float cprg = 100 / (float)directoryCount;
                                                    //var tr1 = new Thread(new ThreadStart(() => Receive(directoryCount, ct, ts)));
                                                    //  gt1 = tr1;

                                                    //  tr1.Start();

                                                    //   for (int ic = 0; ic < mydirs.Count; ic++)
                                                    //{

                                                    _wait.WaitOne();
                                                    lock (glk)
                                                    {
                                                        if (brk)
                                                        {
                                                            // Dispatcher.Invoke(() => { prgbar.Value = 100; });
                                                            return;
                                                            // break;
                                                        }
                                                    }

                                                    string rd = fscn;// mydirs[ic];
                                                    /*Environment.CurrentDirectory= @"C:\Users\Tech4\Desktop\savapi-sdk-win64\bin\";
                                                      var pinfo = new ProcessStartInfo();
                                                      pinfo.WindowStyle = ProcessWindowStyle.Hidden;
                                                      pinfo.FileName = "ConsoleApp7.exe";
                                                      pinfo.Arguments = "\"" + sav.instance.ToInt64() + "&" + rd + "\"";
                                                     // MessageBox.Show(pinfo.Arguments);
                                                   //   pinfo.CreateNoWindow = true;
                                                      pinfo.WorkingDirectory= @"C:\Users\Tech4\Desktop\savapi-sdk-win64\bin\";
                                                      pinfo.UseShellExecute = false;
                                                      prc = Process.Start(pinfo);
                                                      prc.WaitForExit();*/
                                                    lock (lkDr)
                                                    {
                                                        gDr = rd;
                                                    }
                                                    if (level < maxlevel)
                                                    {
                                                        level++;
                                                    }
                                                    else
                                                        level = maxlevel;


                                                    //   Listdirs_count+= Listdirs_count/(1000.0f*Listdirs_count);
                                                    string thrs = "";
                                                    //   MessageBox.Show("*"); 
                                                    var rgs = new GRowsCUS[] { new GRowsCUS { PathF = rd, SubF = rf.SubF } };
                                                    //var rtt = ProcessProtector.cen.Scan(rgs, 1);
                                                   // if(rtt!=null && rtt.Count()>0)
                                                    foreach (var reslt in ProcessProtector.cen.Scan(rgs, 1))
                                                    {
                                                            iscn++;
                                                            if (reslt == null)
                                                                continue;
                                                        _wait.WaitOne();
                                                        lock (glk)
                                                        {
                                                            if (brk)
                                                            {
                                                                Dispatcher.Invoke(() =>
                                                                {
                                                                    //  this.prgbar.Value = 100;
                                                                    // lblExtracting.Content = _finished;
                                                                    //MessageBox.Show("Success!");
                                                                });
                                                                //return;
                                                                break;
                                                            }
                                                        }
                                                        if (string.IsNullOrWhiteSpace(reslt))
                                                            continue;
                                                            
                                                        if (reslt.Contains("$"))
                                                        {
                                                            thrs += reslt;
                                                            //  MessageBox.Show(thrs);
                                                            //MessageBox.Show(thrs);
                                                            //Receive(reslt, directoryCount, null, null);
                                                            //
                                                        }
                                                        else if (reslt.Contains("***"))
                                                        {
                                                            // MessageBox.Show(thrs);
                                                            Receive(thrs, directoryCount, null, null);
                                                            thrs = "";
                                                        }

                                                        //iscn += Directory.GetFiles(rd, "*.*", SearchOption.TopDirectoryOnly).Length;
                                                        // MessageBox.Show(thrs);
                                                        //MessageBox.Show(iscn.ToString());
                                                      //  iscn++;

                                                        int ti = Regex.Matches(reslt, "Malware$").Count;
                                                      // iscn -= ti;
                                                        itrts += ti;

                                                        if (percent < 95)
                                                        {
                                                            percent = iscn / 1000.0f;
                                                        }
                                                        zlist.ForEach((f) => { if (reslt.ToLower().Contains("." + f)) { iarc++; } });
                                                        //iscn -= ti;
                                                        //= Regex.Matches(reslt, "Finish").Count;
                                                        //iscn += Regex.Matches(reslt, "Finish").Count;
                                                        //iscn--;                                
                                                        string scn = iscn.ToString(), arc = iarc.ToString(), trts = itrts.ToString(), reprd = ireprd.ToString(), qarn = iqarn.ToString(), del = idel.ToString(), err = ierr.ToString();
                                                        string status = String.Format("Files Scanned\t\t\t{0}\t\t\tFiles Deleted\t\t\t{5}\nArchive/Packed\t\t\t{1}\t\t\tI/O errors\t\t\t{6}\nThreats Detected\t\t\t{2}\nFiles repaired\t\t\t{3}\nFile Quarantined\t\t\t{4}",
                                                            scn, arc, trts, reprd, qarn, del, err);
                                                        string sc = Listdirs_ilsdr;// mydirs[ic];
                                                        //MessageBox.Show(sc);
                                                        this.Dispatcher.BeginInvoke(new Action(() =>
                                                        {
                                                            var mys = System.IO.Path.GetFileName(sc);
                                                            lblExtracting.Content = mys.Length > 65 ? mys.Substring(0, 65) + "..." : mys;
                                                            prgbar.Value = percent;
                                                            lblStatus.Content = status;
                                                        }));
                                                        if (String.IsNullOrWhiteSpace(reslt))
                                                        {
                                                            //break;
                                                        }
                                                    }
                                                    //MessageBox.Show("**");
                                                    //  MessageBox.Show("x");
                                                    /*lock (glk)
                                                    {
                                                        if (brk)
                                                        {
                                                            brk = false;
                                                            break;
                                                        }
                                                    }*/
                                                    try
                                                    {
                                                        int icu = Listdirs.Count;
                                                        //mydirs.AddRange(Directory.GetDirectories(rd, "*", SearchOption.TopDirectoryOnly).Where((a) => (!mydirs.Contains(a) && /*!new DirectoryInfo(a).Attributes.HasFlag(FileAttributes.Hidden) &&*/ !new DirectoryInfo(a).Attributes.HasFlag(FileAttributes.System))));
                                                        /*   FileDirectoryEnumerable myEnum = new FileDirectoryEnumerable();
                                                           myEnum.SearchPath = rf.PathF;
                                                           myEnum.SearchPattern = "*";
                                                           myEnum.ReturnStringType = true;
                                                           myEnum.SearchForFile = false;
                                                           myEnum.SearchForDirectory = true;
                                                           myEnum.ReturnStringType = true;

                                                           // System.Console.WriteLine("----------- search ALL file name ------------------");


                                                           foreach (string strDirName in myEnum.AsParallel())
                                                           {
                                                               mydirs.Add(strDirName);
                                                           }*/
                                                        if (icu == Listdirs.Count)
                                                        {
                                                            //percent -= percent % (int)cprg;
                                                            //percent += percent / (int)cprg;
                                                            level = 0;
                                                        }
                                                    }
                                                    catch (Exception em)
                                                    {
                                                        ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                                                    }
                                                    /* lock (glk)
                                                     {
                                                         if (brk)
                                                         {
                                                             brk = false;
                                                             break;
                                                         }
                                                     }*/
                                                    // }



                                                    //           end = true;
                                                    //   MessageBox.Show("::" + strDirName);
                                                    // 
                                                }
                                                catch (Exception em)
                                                {
                                                    var st = new StackTrace(em, true);
                                                    //MessageBox.Show(st.GetFrame(st.FrameCount-1).GetFileLineNumber().ToString());
                                                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + "Line: " + st.GetFrame(st.FrameCount - 1).GetFileLineNumber().ToString() + Environment.NewLine + msg() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                                                }
                                            });
                                        }
                                        //myEnum1.Close();
                                    }
                                    catch
                                    {
                                       // ilsdr++;
                                    }
                                    //  myEnum1.Close();
                                    //       ilsdr++;
                                    lock (lk)
                                    {
                                        ilsdr++;
                                    }
                                } 
                                while (true);

                           /*     FileDirectoryEnumerable myEnum = new FileDirectoryEnumerable();
                                    myEnum.SearchPath = rf.PathF;
                                    myEnum.SearchPattern = "*";
                                    myEnum.ReturnStringType = true;
                                    myEnum.SearchForFile = false;
                                    myEnum.SearchForDirectory = true;
                                    myEnum.ReturnStringType = true;
                                */
                                    // System.Console.WriteLine("----------- search ALL file name ------------------");


                                

                            }
                  

                        }
                        else
                        {
                            /* try
                             {*/
                           /* bool bEx = false;
                            try
                            {
                                bEx = File.Exists(rf.PathF);
                            }
                            catch (Exception em)
                            {
                                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                            }
                           */
                            if (!string.IsNullOrEmpty(rf.PathF))// && bEx)
                            {

                                // string argv = System.AppDomain.CurrentDomain.FriendlyName + "$11488" + "$" + rf + "$0" + "$1";
                                //  IntPtr parg = Marshal.StringToHGlobalUni(@"E:\AV\Virus sample\file1.exe");

                                //string repair = "0";
                                //IntPtr r2 = Marshal.StringToHGlobalUni(repair);
                                //sav.func_ScanFile(new string[] { rf });
                                // files2.Add(rf);

                                // Marshal.FreeHGlobal(parg);
                                /*if (r != 0)
                                {
                                    new Quarantine().QuaFile(rf);
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        this.DataGridMain.Items.Add(new Item() { File = rf, Status = "Infected", Action = "Quarantined" });
                                    });
                                    itrts++;
                                    iqarn++;
                                }*/
                            }
                            //                            iscn++;



                            /*
                        }

                        catch (ThreadAbortException)
                        { }
                        catch (Exception ex) {  }
                        */
                        }

                        //  MessageBox.Show(files2.Count.ToString()+" "+ msg());
                        if (ifs < files2.Count)
                        {

                            string thrs = "";
                            var tmp = files2.ToArray();
                            int p = 0;
                            if (tmp.Length > 0)
                            {
                               // var tar = ProcessProtector.cen.Scan(tmp, 1);
                               // if(tar!=null && tar.Count()>0)
                                foreach (var reslt in ProcessProtector.cen.Scan(tmp, 1))
                                {
                                        if(reslt.Contains("File$"))
                                        iscn++;
                                    //MessageBox.Show(reslt);
                                    lock (glk)
                                    {
                                        if (brk)
                                        {
                                            brk = false;
                                            //  Dispatcher.Invoke(() => { prgbar.Value = 100; });
                                            //return;
                                            break;
                                        }
                                    }
                                    //var curfl = files2[ifs];
                                    _wait.WaitOne();
                                      //  MessageBox.Show(reslt);
                                   // if(reslt.Contains("File$"))
                                     //   iscn++;
                                    if (reslt.Contains("$"))
                                    {
                                        thrs += reslt;
                                            //iscn--;
                                        //  MessageBox.Show(thrs);
                                        //MessageBox.Show(thrs);
                                        //Receive(reslt, directoryCount, null, null);
                                        //MessageBox.Show(reslt);
                                    }
                                    else if (reslt.Contains("***"))
                                    {
                                        //MessageBox.Show(thrs);
                                        Receive(thrs, 1, null, null);
                                        thrs = "";

                                    }

                                    string scn = iscn.ToString(), arc = iarc.ToString(), trts = itrts.ToString(), reprd = ireprd.ToString(), qarn = iqarn.ToString(), del = idel.ToString(), err = ierr.ToString();
                                    string status = String.Format("Files Scanned\t\t\t{0}\t\t\tFiles Deleted\t\t\t{5}\nArchive/Packed\t\t\t{1}\t\t\tI/O errors\t\t\t{6}\nThreats Detected\t\t\t{2}\nFiles repaired\t\t\t{3}\nFile Quarantined\t\t\t{4}",
                                        scn, arc, trts, reprd, qarn, del, err);
                                    //string sc = "";
                                    // MessageBox.Show(scn);
                                    this.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        p = (int)(100 * ifs / (float)(files2.Count));

                                        //   lblFolder.Content = "";
                                        if (files2.Count > ifs)
                                        {
                                            var mys = (files2[ifs].PathF).Split('\\').Last();
                                            lblExtracting.Content = mys.Length > 65 ? mys.Substring(0, 65) + "..." : mys;
                                        }
                                        prgbar.Value = p;
                                        lblStatus.Content = status;
                                    }));


                                    if (reslt.Trim() == ("File"))
                                        ifs++;
                                    // MessageBox.Show((ifs / (float)(files2.Count / 100)).ToString());

                                    // if (ifs >= files2.Count)
                                    //   break;
                                }
                            }
                        }
                    }
                string scn2 = iscn.ToString(), arc2 = iarc.ToString(), trts2 = itrts.ToString(), reprd2 = ireprd.ToString(), qarn2 = iqarn.ToString(), del2 = idel.ToString(), err2 = ierr.ToString();
                string status2 = String.Format("Files Scanned\t\t\t{0}\t\t\tFiles Deleted\t\t\t{5}\nArchive/Packed\t\t\t{1}\t\t\tI/O errors\t\t\t{6}\nThreats Detected\t\t\t{2}\nFiles repaired\t\t\t{3}\nFile Quarantined\t\t\t{4}",
                        scn2, arc2, trts2, reprd2, qarn2, del2, err2);

                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    //    lblFolder.Content = "";
                    //  lblExtracting.Content = "";
                    // prgbar.Value = 100;
                    lblStatus.Content = status2;
                }));

                this.Dispatcher.Invoke(() =>
                            {
                                link1.IsEnabled = true;
                                link2.IsEnabled = true;
                                link3.IsEnabled = true;
                                link4.IsEnabled = true;
                            });
                            this.Dispatcher.Invoke(() =>
                            {
                               // this.prgbar.Value = 100;
                                lblExtracting.Content = _finished;
                                 // MessageBox.Show("Success!");
                                
                            });//return;
                            /*
                            lock (glk)
                            {
                                if (!brk)
                                    this.Dispatcher.Invoke(() => { MessageBox.Show("Success!"); });

                            }*/
                        
                    
                /*  }
                 );

                  act();*/


                // float prc = 0;
                /*System.Timers.Timer tmr = new System.Timers.Timer(500);
                tmr.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
                {
                  
                    prc += new Random().Next(1, 8);
                    
                    Dispatcher.Invoke(() => { prgbar.Value = prc; });
                    lock (glk)
                    {
                        if (prc >= 90 || brk)
                        {
                            tmr.Stop();
                            if (brk)
                            {

                                Dispatcher.Invoke(() => { 
                                    this.prgbar.Value = 100;
                                    lblExtracting.Content = _finished;
                                   // MessageBox.Show("Success!");
                                });
                            }
                        }
                    }
                   
                };*/
                // tmr.Start();
                this.Dispatcher.Invoke(() =>
                {
                    btnPause.IsEnabled = btnSkipFile.IsEnabled = btnSkipFolder.IsEnabled = false;
                    lblbtnSkipFile.Foreground = lblbtnSkipFolder.Foreground
                     = lblPause.Foreground = System.Windows.Media.Brushes.LightGray;
                    ButtonMn.Foreground = System.Windows.Media.Brushes.LightGray;
                    ButtonMn.IsEnabled = false;
                });
                //MessageBox.Show("/");
             
                    /*  string scn2 = iscn.ToString(), arc2 = iarc.ToString(), trts2 = itrts.ToString(), reprd2 = ireprd.ToString(), qarn2 = iqarn.ToString(), del2 = idel.ToString(), err2 = ierr.ToString();
                      string status2 = String.Format("File Scanned\t\t\t{0}\t\t\tFiles Deleted\t\t\t{5}\nArchive/Packed\t\t\t{1}\t\t\tI/O errors\t\t\t{6}\nThreats Detected\t\t\t{2}\nFiles repaired\t\t\t{3}\nFile Quarantined\t\t\t{4}",
                          scn2, arc2, trts2, reprd2, qarn2, del2, err2);
                      this.Dispatcher.Invoke(() =>
                      {
                          lblStatus.Content = status2;
                      });*/
                    //percent = 100;
                    //tmr.Stop();
                    this.Dispatcher.Invoke(() =>
                    {
                        link1.IsEnabled = true;
                        link2.IsEnabled = true;
                        link3.IsEnabled = true;
                        link4.IsEnabled = true;
                    });
                    this.Dispatcher.Invoke(() =>
                    {
                        this.prgbar.Value = 100;
                        lblExtracting.Content = _finished;
                        ButtonMn.Foreground = System.Windows.Media.Brushes.Green;
                        ButtonMn.IsEnabled = true;
                        MessageBox.Show("Success!");
                        
                    });
//return;
                    lock (glk)
                    {
                        if (MainWindow.mfiles != null)
                            MainWindow.mfiles.Clear();
                        MainWindow.mfiles = null;
                        /*GC.Collect();
                       // GC.WaitForFullGCComplete();
                        if (!brk)
                        this.Dispatcher.Invoke(() => { MessageBox.Show("Success!"); });
                        */
                    }
                    /*Task.Factory.StartNew(act, ct)
                    .ContinueWith((t) => {

                    });*/

                }
    
            catch (Exception em)
            {
                var st = new StackTrace(em, true);
                //MessageBox.Show(st.GetFrame(st.FrameCount-1).GetFileLineNumber().ToString());
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName()+" "+"Line: "+st.GetFrame(st.FrameCount-1).GetFileLineNumber().ToString()+Environment.NewLine+msg()+Environment.NewLine+em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void ButtonEsc_Click(object sender, KeyEventArgs e){if(e.Key == Key.Escape){Button_Click(null, null);}} private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lblExtracting.Content.ToString() != _finished && lblExtracting.Content.ToString() != _interrupted)
            {
                if (MessageBox.Show("Do you really want to stop the Scan?", "Alert", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    prgbar.Value = 100;
                    ts?.Cancel();
                    ProcessStartInfo pis = new ProcessStartInfo(Environment.SystemDirectory + "\\TaskKill.exe", $" /F /IM {ConsoleApp13}.exe");
                    pis.CreateNoWindow = true;
                    pis.WindowStyle = ProcessWindowStyle.Hidden;
                    pis.UseShellExecute = false;
                    Process.Start(pis);
                    lock (glk)
                    {
                        brk = true;
                    }

                    gt1?.Abort();



                    if (prgbar.Value != 100)
                        return;
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    /*
                    Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType((mw.StartsWith("WebRoamAV.") ? "" : "WebRoamAV.") + mw.ToString().Replace(".xaml", ""));
                    MainWindow.parentTop = this.Top;
                    MainWindow.parentLeft = this.Left;
                    t.GetMethod("Show").Invoke(Activator.CreateInstance(t, this.ToString()), new object[] { });*/
                    this.Close();
                }
            }
            else
            {
                MainWindow mw = new MainWindow();
                mw.Show();
                /*
                Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType((mw.StartsWith("WebRoamAV.") ? "" : "WebRoamAV.") + mw.ToString().Replace(".xaml", ""));
                MainWindow.parentTop = this.Top;
                MainWindow.parentLeft = this.Left;
                t.GetMethod("Show").Invoke(Activator.CreateInstance(t, this.ToString()), new object[] { });*/
                this.Close();
            }
        }
        private void link_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType(e.Uri.ToString().StartsWith("WebRoamAV.") ? "" : "WebRoamAV." + e.Uri.ToString().Replace(".xaml", ""));
            MainWindow.parentTop = this.Top;	
				MainWindow.parentLeft = this.Left;	
				t.GetMethod("Show").Invoke(Activator.CreateInstance(t, this.ToString()), new object[] { });
            this.Close();
        }

        private void link4_Click(object sender, RoutedEventArgs e)
        {

            Hyperlink h = sender as Hyperlink;
            ContextMenu contextMenu = (ContextMenu)this.link4.Resources["link41"];
            contextMenu.PlacementTarget = this.tlink4;
            contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
            contextMenu.IsOpen = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.OriginalSource as MenuItem;
            MessageBox.Show(mi.ToString());
        }
        bool brk = false;
        public void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you really want to stop the Scan?", "Alert", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                ts?.Cancel();

                ProcessStartInfo pis = new ProcessStartInfo(Environment.SystemDirectory + "\\TaskKill.exe", $" /F /IM {ConsoleApp13}.exe");
                pis.CreateNoWindow = true;
                pis.WindowStyle = ProcessWindowStyle.Hidden;
                pis.UseShellExecute = false;
                Process.Start(pis);
                lock (glk)
                {
                    brk = true;
                }
                lblExtracting.Content = _interrupted;
                gt1?.Abort();
                btnStop.IsEnabled = false;
                lblbtnStop.Foreground = Brushes.LightGray;
                Task.Factory.StartNew(() => Thread.Sleep(2500)).ContinueWith((t) => {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.prgbar.Value = 100;
                        lblExtracting.Content = _interrupted;
                        ButtonMn.Foreground = System.Windows.Media.Brushes.Green;
                        ButtonMn.IsEnabled = true;
                    });
                });
            }
        }
        private void Stop_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnStop.RenderTransformOrigin = new Point(0.5, 0.5);
                btnStop.RenderTransform = scale;

                DoubleAnimation growAnimation = new DoubleAnimation();
                growAnimation.Duration = TimeSpan.FromMilliseconds(100);
                growAnimation.To = 1;
                growAnimation.From = 0.9;
                storyboard.Children.Add(growAnimation);

                DoubleAnimation growAnimation2 = new DoubleAnimation();
                growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
                growAnimation2.To = 1;
                growAnimation2.From = 0.9;
                storyboard.Children.Add(growAnimation2);

                Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
                Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
                Storyboard.SetTarget(growAnimation, btnStop);
                Storyboard.SetTarget(growAnimation2, btnStop);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnStop.Background = new SolidColorBrush(Colors.Green);
        }

        private void Stop_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnStop.RenderTransformOrigin = new Point(0.5, 0.5);
            btnStop.RenderTransform = scale;

            DoubleAnimation growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation.To = 1;
            growAnimation.From = 0.9;
            storyboard.Children.Add(growAnimation);

            DoubleAnimation growAnimation2 = new DoubleAnimation();
            growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation2.To = 1;
            growAnimation2.From = 0.9;
            storyboard.Children.Add(growAnimation2);

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growAnimation, btnStop);
            Storyboard.SetTarget(growAnimation2, btnStop);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnStop_Click(null, null);
        }

        private void Stop_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnStop.RenderTransformOrigin = new Point(0.5, 0.5);
            btnStop.RenderTransform = scale;

            DoubleAnimation growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation.From = 1;
            growAnimation.To = 0.9;
            storyboard.Children.Add(growAnimation);

            DoubleAnimation growAnimation2 = new DoubleAnimation();
            growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation2.From = 1;
            growAnimation2.To = 0.9;
            storyboard.Children.Add(growAnimation2);

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growAnimation, btnStop);
            Storyboard.SetTarget(growAnimation2, btnStop);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void Stop_MouseEnter(object sender, MouseEventArgs e)
        {
            btnStop.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }

        private void Pause_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnPause.RenderTransformOrigin = new Point(0.5, 0.5);
                btnPause.RenderTransform = scale;

                DoubleAnimation growAnimation = new DoubleAnimation();
                growAnimation.Duration = TimeSpan.FromMilliseconds(100);
                growAnimation.To = 1;
                growAnimation.From = 0.9;
                storyboard.Children.Add(growAnimation);

                DoubleAnimation growAnimation2 = new DoubleAnimation();
                growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
                growAnimation2.To = 1;
                growAnimation2.From = 0.9;
                storyboard.Children.Add(growAnimation2);

                Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
                Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
                Storyboard.SetTarget(growAnimation, btnPause);
                Storyboard.SetTarget(growAnimation2, btnPause);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnPause.Background = new SolidColorBrush(Colors.Green);
        }

        private void Pause_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnPause.RenderTransformOrigin = new Point(0.5, 0.5);
            btnPause.RenderTransform = scale;

            DoubleAnimation growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation.To = 1;
            growAnimation.From = 0.9;
            storyboard.Children.Add(growAnimation);

            DoubleAnimation growAnimation2 = new DoubleAnimation();
            growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation2.To = 1;
            growAnimation2.From = 0.9;
            storyboard.Children.Add(growAnimation2);

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growAnimation, btnPause);
            Storyboard.SetTarget(growAnimation2, btnPause);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            if (lblPause.Content.ToString() == "_Pause")
            {
                lblPause.Content = "_Resume";
            }
            else
            {
                lblPause.Content = "_Pause";
            }
            btnPause_Click(null, null);
        }

        private void Pause_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnPause.RenderTransformOrigin = new Point(0.5, 0.5);
            btnPause.RenderTransform = scale;

            DoubleAnimation growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation.From = 1;
            growAnimation.To = 0.9;
            storyboard.Children.Add(growAnimation);

            DoubleAnimation growAnimation2 = new DoubleAnimation();
            growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation2.From = 1;
            growAnimation2.To = 0.9;
            storyboard.Children.Add(growAnimation2);

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growAnimation, btnPause);
            Storyboard.SetTarget(growAnimation2, btnPause);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void Pause_MouseEnter(object sender, MouseEventArgs e)
        {
            btnPause.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }
        bool bpause = true;
        public void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (!bpause)
                _wait.Set();
            else
                _wait.Reset();
            bpause = !bpause;
           
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            if (this.prgbar.Value > 0 && this.prgbar.Value < 100)
            {
                if (MessageBox.Show("Are you sure want to cancel the scan task?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.None) == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            }
            else
                this.Close();
        }
        private string ConsoleApp13 = "wravrun";
        public void btnSkipFolder_Click(object sender, RoutedEventArgs e)
        {
            // if (MessageBox.Show("Do you really want to stop the Scan?", "Alert", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            // {
            ts?.Cancel();
           
             /*
              *OLD CODE
              * ProcessStartInfo pis = new ProcessStartInfo(Environment.SystemDirectory + "\\TaskKill.exe", $" /F /IM {ConsoleApp13}.exe");
                pis.CreateNoWindow = true;
                pis.UseShellExecute = false;
                pis.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(pis);*/
                lock (glk)
                {
                    brk = false;
                }

                
                    gt1?.Abort();
               // prgbar.Value = 100;

        //    }
          /*  lock (lk)
            {
                bskipfilef = 2;
            }*/
        }
        private void SkipFolder_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnSkipFolder.RenderTransformOrigin = new Point(0.5, 0.5);
                btnSkipFolder.RenderTransform = scale;

                DoubleAnimation growAnimation = new DoubleAnimation();
                growAnimation.Duration = TimeSpan.FromMilliseconds(100);
                growAnimation.To = 1;
                growAnimation.From = 0.9;
                storyboard.Children.Add(growAnimation);

                DoubleAnimation growAnimation2 = new DoubleAnimation();
                growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
                growAnimation2.To = 1;
                growAnimation2.From = 0.9;
                storyboard.Children.Add(growAnimation2);

                Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
                Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
                Storyboard.SetTarget(growAnimation, btnSkipFolder);
                Storyboard.SetTarget(growAnimation2, btnSkipFolder);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnSkipFolder.Background = new SolidColorBrush(Colors.Green);
        }

        private void SkipFolder_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnSkipFolder.RenderTransformOrigin = new Point(0.5, 0.5);
            btnSkipFolder.RenderTransform = scale;

            DoubleAnimation growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation.To = 1;
            growAnimation.From = 0.9;
            storyboard.Children.Add(growAnimation);

            DoubleAnimation growAnimation2 = new DoubleAnimation();
            growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation2.To = 1;
            growAnimation2.From = 0.9;
            storyboard.Children.Add(growAnimation2);

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growAnimation, btnSkipFolder);
            Storyboard.SetTarget(growAnimation2, btnSkipFolder);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnSkipFolder_Click(null, null);
        }

        private void SkipFolder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnSkipFolder.RenderTransformOrigin = new Point(0.5, 0.5);
            btnSkipFolder.RenderTransform = scale;

            DoubleAnimation growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation.From = 1;
            growAnimation.To = 0.9;
            storyboard.Children.Add(growAnimation);

            DoubleAnimation growAnimation2 = new DoubleAnimation();
            growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation2.From = 1;
            growAnimation2.To = 0.9;
            storyboard.Children.Add(growAnimation2);

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growAnimation, btnSkipFolder);
            Storyboard.SetTarget(growAnimation2, btnSkipFolder);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void SkipFolder_MouseEnter(object sender, MouseEventArgs e)
        {
            btnSkipFolder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }

        public void btnSkipFile_Click(object sender, RoutedEventArgs e)
        {
            bskipfilef = 1;
            ts2.Cancel();
        }
        private void SkipFile_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnSkipFile.RenderTransformOrigin = new Point(0.5, 0.5);
                btnSkipFile.RenderTransform = scale;

                DoubleAnimation growAnimation = new DoubleAnimation();
                growAnimation.Duration = TimeSpan.FromMilliseconds(100);
                growAnimation.To = 1;
                growAnimation.From = 0.9;
                storyboard.Children.Add(growAnimation);

                DoubleAnimation growAnimation2 = new DoubleAnimation();
                growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
                growAnimation2.To = 1;
                growAnimation2.From = 0.9;
                storyboard.Children.Add(growAnimation2);

                Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
                Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
                Storyboard.SetTarget(growAnimation, btnSkipFile);
                Storyboard.SetTarget(growAnimation2, btnSkipFile);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnSkipFile.Background = new SolidColorBrush(Colors.Green);
        }

        private void SkipFile_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnSkipFile.RenderTransformOrigin = new Point(0.5, 0.5);
            btnSkipFile.RenderTransform = scale;

            DoubleAnimation growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation.To = 1;
            growAnimation.From = 0.9;
            storyboard.Children.Add(growAnimation);

            DoubleAnimation growAnimation2 = new DoubleAnimation();
            growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation2.To = 1;
            growAnimation2.From = 0.9;
            storyboard.Children.Add(growAnimation2);

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growAnimation, btnSkipFile);
            Storyboard.SetTarget(growAnimation2, btnSkipFile);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnSkipFile_Click(null, null);
        }

        private void SkipFile_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnSkipFile.RenderTransformOrigin = new Point(0.5, 0.5);
            btnSkipFile.RenderTransform = scale;

            DoubleAnimation growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation.From = 1;
            growAnimation.To = 0.9;
            storyboard.Children.Add(growAnimation);

            DoubleAnimation growAnimation2 = new DoubleAnimation();
            growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation2.From = 1;
            growAnimation2.To = 0.9;
            storyboard.Children.Add(growAnimation2);

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growAnimation, btnSkipFile);
            Storyboard.SetTarget(growAnimation2, btnSkipFile);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

      
        private void SkipFile_MouseEnter(object sender, MouseEventArgs e)
        {
            btnSkipFile.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                isReceiving = false;
                s?.Close();
                ts?.Cancel();
                if(tc!=null)
                tc.Stop();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName()+" "+new StackFrame(1, true).GetFileLineNumber()+Environment.NewLine+em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
            finally
            {
                this.Close();
            }
        }
    }
}
