using System;
using System.Management;
using NetFwTypeLib;
using Microsoft.Win32;
using System.IO;
using SevenZip;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Collections;
using PacketDotNet;
using SharpPcap.WinPcap;
using SharpPcap;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.ServiceProcess;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Data.Sql;
using System.Data;
using System.ComponentModel;
using System.Threading;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Net.Http;
using System.Net.Http.Headers;
using AutoUpdaterDotNET;
using System.Windows.Threading;
using NativeWifi;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.Management.Automation;
using static WebRoamAV.App;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using UIAutomationClient;
using System.Windows.Automation;
using EnumerateFile;

namespace WebRoamAV //unitilities file
{

    public static class ProcessExtension
    {
        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);


        private static void SuspendProcess(int pid)
        {
            var process = Process.GetProcessById(pid); // throws exception if process does not exist

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                SuspendThread(pOpenThread);

                CloseHandle(pOpenThread);
            }
        }

        public static void ResumeProcess(int pid)
        {
            var process = Process.GetProcessById(pid);

            if (process.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                var suspendCount = 0;
                do
                {
                    suspendCount = ResumeThread(pOpenThread);
                } while (suspendCount > 0);

                CloseHandle(pOpenThread);
            }
        }
    }
    //class for configuration
    class BrowserAutm
    {
        private string[] blocked;
        [DllImport("user32.dll")]
        private static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("User32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr windowHandle, StringBuilder stringBuilder, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextLength", SetLastError = true)]
        internal static extern int GetWindowTextLength(IntPtr hwnd);

        private static List<IntPtr> windowList;
        private static string _className;
        private static StringBuilder apiResult = new StringBuilder(256); //256 Is max class name length.
        private delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        public delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        protected static extern bool EnumWindows(Win32Callback enumProc, IntPtr lParam);

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            List<IntPtr> pointers = GCHandle.FromIntPtr(pointer).Target as List<IntPtr>;
            pointers.Add(handle);
            return true;
        }

        public int CheckChrome(string localApp, string[] domain)
        {
            int count = 0;
            // string title = "";
            List<IntPtr> Windows = new List<IntPtr> { GetForegroundWindow() };//GetChildWindows(GetForegroundWindow());//GetAllWindows();

            foreach (IntPtr window in Windows)
            {
                bool b = domain.Any((d) =>
                {
                    return GetTitle(window).ToLower().Contains(localApp.ToLower());
                });

                if (b)
                {
                    count += InspectChromeObject(window, domain) ? 1 : 0;
                    //MessageBox.Show(count.ToString());
                }
            }
            return count;

        }

        public int CheckFirefox(string localApp, string[] domain)
        {
            int count = 0;
            //string title = "";
            List<IntPtr> Windows = new List<IntPtr> { GetForegroundWindow() };//GetChildWindows(GetForegroundWindow());// GetAllWindows();

            foreach (IntPtr window in Windows)
            {
                bool b = domain.Any((d) => { return GetTitle(window).ToLower().Contains(localApp.ToLower()); });
                if (b)
                {
                    // MessageBox.Show(title);
                    count += InspectFireFoxObject(window, domain) ? 1 : 0;
                }
            }

            return count;

        }


        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

        /// <summary>
        /// Returns a list of child windows
        /// </summary>
        /// <param name="parent">Parent of the windows to return</param>
        /// <returns>List of child windows</returns>
        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(chEnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        /// <summary>
        /// Callback method to be used when enumerating windows.
        /// </summary>
        /// <param name="handle">Handle of the next window</param>
        /// <param name="pointer">Pointer to a GCHandle that holds a reference to the list to fill</param>
        /// <returns>True to continue the enumeration, false to bail</returns>
        private static bool chEnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            list.Add(handle);
            //  You can modify this to check to see if you want to cancel the operation, then return a null here
            return true;
        }

        /// <summary>
        /// Delegate for the EnumChildWindows method
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <param name="parameter">Caller-defined variable; we use it for a pointer to our list</param>
        /// <returns>True to continue enumerating, false to bail.</returns>
        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        public static List<IntPtr> GetAllWindows()
        {
            Win32Callback enumCallback = new Win32Callback(EnumWindow);
            List<IntPtr> pointers = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(pointers);
            try
            {
                EnumWindows(enumCallback, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated) listHandle.Free();
            }
            return pointers;
        }
        public static string GetTitle(IntPtr handle)
        {
            int length = GetWindowTextLength(handle);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(handle, sb, sb.Capacity);
            return sb.ToString();
        }

        private List<string> WalkEnabledElements(AutomationElement rootElement, List<string> ret)
        {
            Condition condition1 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            Condition condition2 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            TreeWalker walker = new TreeWalker(new AndCondition(condition1, condition2));
            AutomationElement elementNode = walker.GetFirstChild(rootElement);
            AutomationElement elementNode2 = walker.GetNextSibling(rootElement);
            while (elementNode != null)
            {
                ret.Add(elementNode.Current.ControlType.LocalizedControlType);
                WalkEnabledElements(elementNode, ret);
                elementNode = walker.GetNextSibling(elementNode);
            }
            while (elementNode2 != null)
            {
                ret.Add(elementNode2.Current.ControlType.LocalizedControlType);
                WalkEnabledElements(elementNode2, ret);
                elementNode2 = walker.GetNextSibling(elementNode2);
            }
            return ret;
        }


        private static List<AutomationElement> GetEditElement(AutomationElement rootElement, List<AutomationElement> ret)
        {

            Condition isControlElementProperty = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            Condition isEnabledProperty = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            TreeWalker walker = new TreeWalker(new AndCondition(isControlElementProperty, isEnabledProperty));
            AutomationElement elementNode = walker.GetFirstChild(rootElement);
            while (elementNode != null)
            {
                if (elementNode.Current.ControlType.ProgrammaticName == "ControlType.Edit")
                    ret.Add(elementNode);
                GetEditElement(elementNode, ret);
                elementNode = walker.GetNextSibling(elementNode);
            }
            return ret;
        }
        static Task[] tasks = new Task[4];
        CancellationTokenSource source = new CancellationTokenSource();
        
        public void Start()
        {
            CancellationToken ct = source.Token;
          //  MessageBox.Show(AppSettings.IsParentalProtectOn.ToString());
            blocked = File.ReadAllLines("blockedsites.txt");
            AppSettings.IsParentalProtectOn = blocked.Any(l => !String.IsNullOrWhiteSpace(l));

            Task.Run(() =>
            {
                while (true)
                {
                    lock (lk)
                    {
                        if (!AppSettings.IsParentalProtectOn && tasks[0].Status == TaskStatus.Running)
                        {
                            try
                            {
                                source.Cancel();
                            }
                            catch { }
                        }
                        else
                        {
                            foreach (var t in tasks)
                            {
                                if (t.IsCanceled)
                                {
                                    t.Start();
                                }
                            }
                        }
                    }
                    Thread.Sleep(800);
                }
            });
            // }
            FileSystemWatcher watcher = new FileSystemWatcher(".");
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += Watcher_Changed;
            watcher.EnableRaisingEvents = true;
            try
            {
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add(string.Format("http://+:2005/"));
                tasks[0] = Task.Run(() =>
                {
                    listener.Start();
                    while (!ct.IsCancellationRequested)
                    {
                        // Note: The GetContext method blocks while waiting for a request.
                        HttpListenerContext context = listener.GetContext();

                        // Obtain a response object.
                        HttpListenerResponse response = context.Response;

                        // Construct a response.
                        string responseString = "<HTML><BODY> Forbidden!</BODY></HTML>";
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                        // Get a response stream and write the response to it.
                        response.ContentLength64 = buffer.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);

                        //Close the output stream.
                        output.Close();
                    }
                }
                , ct);

                tasks[1] = Task.Run(() =>
                {
                    BrowserAutm chr = new BrowserAutm();
                    string[] list = blocked;
                    while (!ct.IsCancellationRequested)
                    {
                        try
                        {
                            if (chr.CheckChrome("Google", list) > 0)
                            {
                                SendKeys.SendWait("{ENTER}");
                                //    MessageBox.Show("Enter!");
                            }
                        }
                        catch
                        { }
                        Thread.Sleep(60);
                        lock (lk)
                        {
                            list = blocked;
                        }

                    }
                }, ct);
                tasks[2] = Task.Run(() =>
                {
                    BrowserAutm chr = new BrowserAutm();
                    string[] list = blocked;
                    while (!ct.IsCancellationRequested)
                    {
                        try
                        {
                            if (chr.CheckFirefox("Firefox", list) > 0)
                            {
                                SendKeys.SendWait("{ENTER}");
                            }
                        }
                        catch
                        { }
                        Thread.Sleep(60);
                        lock (lk)
                        {
                            list = blocked;
                        }

                    }
                },ct);

                tasks[3] = Task.Run(() =>
                {
                    BrowserAutm chr = new BrowserAutm();
                    string[] list = blocked;
                    while (!ct.IsCancellationRequested)
                    {
                        try
                        {
                            if (BrowserAutm.Edge.CheckEdge(list))
                                SendKeys.SendWait("{ENTER}");
                        }
                        catch { }
                        Thread.Sleep(60);
                        lock (lk)
                        {
                            list = blocked;
                        }
                    }
                },ct);

            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
    public void Stop()
        {
            try
            {
                source.Cancel();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.Contains("blockedsites.txt"))
            {
                lock (lk)
                {
                    blocked = File.ReadAllLines("blockedsites.txt");
                    AppSettings.IsParentalProtectOn = blocked.Any(l => !String.IsNullOrWhiteSpace(l));
                   // MessageBox.Show(AppSettings.IsParentalProtectOn.ToString());
                }
            }
        }

        object lk = new object();
        public static string GetText(AutomationElement element)
        {
            if (element == null)
                return null;
            object patternObj;
            if (element.TryGetCurrentPattern(ValuePattern.Pattern, out patternObj))
            {
                var valuePattern = (ValuePattern)patternObj;
                return valuePattern.Current.Value;
            }
            else if (element.TryGetCurrentPattern(TextPattern.Pattern, out patternObj))
            {
                var textPattern = (TextPattern)patternObj;
                return textPattern.DocumentRange.GetText(-1).TrimEnd('\r'); // often there is an extra '\r' hanging off the end.
            }
            else
            {
                return element.Current.Name;
            }
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        public static void PressKey(Keys key, bool up)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;
            if (up)
            {
                keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            }
            else
            {
                keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            }
        }

        private static bool InspectChromeObject(IntPtr handle, string[] domain)
        {
            AutomationElement elm = AutomationElement.FromHandle(handle);

            AutomationElement elmUrlBar = null;
            try
            {
                var elm1 = elm.FindFirst(System.Windows.Automation.TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Google Chrome"));

                if (elm1 == null) { return false; }

                List<AutomationElement> ret = new List<AutomationElement>();
                elmUrlBar = GetEditElement(elm1, ret)[0];
                string s = GetText(elmUrlBar);



                if (domain.Any((d) => s.ToLower().Contains(d.ToLower().Replace("https://", "").Replace("www.", "").Replace("http://", "").TrimEnd('/'))))

                {
                    try
                    {
                        SetText(elmUrlBar, "Http://127.0.0.1:2005");
                        elmUrlBar.SetFocus();
                    }
                    catch
                    { }
                    return true;
                }

            }
            catch
            {
                return true;
            }

            if (elmUrlBar == null)
            {
                return false;
            }

            if ((bool)elmUrlBar.GetCurrentPropertyValue(AutomationElement.HasKeyboardFocusProperty))
            {
                return false;
            }
            AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
            if (patterns.Length == 1)
            {
                string ret = "";
                try
                {
                    ret = ((ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0])).Current.Value;
                }
                catch { }

                if (ret != "")
                {
                    if (Regex.IsMatch(ret, @"^(https:\/\/)?[a-zA-Z0-9\-\.]+(\.[a-zA-Z]{2,4}).*$"))
                    {
                        if (!ret.StartsWith("http"))
                        {
                            ret = "http://" + ret;
                        }
                        domain.Any((d) =>
                        {
                            var uri = new Uri(d);
                            var ieUri = new Uri(ret);
                            if (ieUri.Host.Contains(uri.Host.ToString()))
                            {
                                return true;
                            }
                            return false;
                        });
                    }
                }
                return false;
            }

            return false;

        }
        private static void SetText(AutomationElement targetControl,
                                    string value)
        {
            // Validate arguments / initial setup
            if (value == null)
                throw new ArgumentNullException(
                    "String parameter must not be null.");

            if (targetControl == null)
                throw new ArgumentNullException(
                    "AutomationElement parameter must not be null");

            // A series of basic checks prior to attempting an insertion.
            //
            // Check #1: Is control enabled?
            // An alternative to testing for static or read-only controls 
            // is to filter using 
            // PropertyCondition(AutomationElement.IsEnabledProperty, true) 
            // and exclude all read-only text controls from the collection.
            if (!targetControl.Current.IsEnabled)
            {
                throw new InvalidOperationException(
                    "The control is not enabled.\n\n");
            }

            // Check #2: Are there styles that prohibit us 
            //           from sending text to this control?
            /*            if (!targetControl.Current.IsKeyboardFocusable)
                        {
                            throw new InvalidOperationException(
                                "The control is not focusable.\n\n");
                        }*/

            // Once you have an instance of an AutomationElement,  
            // check if it supports the ValuePattern pattern.
            object valuePattern = null;

            if (!targetControl.TryGetCurrentPattern(
                ValuePattern.Pattern, out valuePattern))
            {
                // Elements that support TextPattern 
                // do not support ValuePattern and TextPattern
                // does not support setting the text of 
                // multi-line edit or document controls.
                // For this reason, text input must be simulated.
            }
            // Control supports the ValuePattern pattern so we can 
            // use the SetValue method to insert content.
            else
            {
                if (((ValuePattern)valuePattern).Current.IsReadOnly)
                {
                    throw new InvalidOperationException(
                        "The control is read-only.");
                }
                else
                {
                    ((ValuePattern)valuePattern).SetValue(value);
                }
            }
        }
        private static bool InspectFireFoxObject(IntPtr handle, string[] domain)
        {
            AutomationElement elm = AutomationElement.FromHandle(handle);

            AutomationElement elmUrlBar = null;
            try
            {
                var nameCondition = new PropertyCondition(AutomationElement.NameProperty, "Search with Google or enter address");
                var controlCondition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit);
                var condition = new AndCondition(nameCondition, controlCondition);
                AutomationElement editBox = elm.FindFirst(System.Windows.Automation.TreeScope.Subtree, condition);
                //AutomationPattern[] patterns1 = elm.GetSupportedPatterns();
                string s = GetText(editBox);
                if (domain.Any((d) => s.ToLower().Contains(d.ToLower().Replace("https://", "").Replace("www.", "").Replace("http://", "").TrimEnd('/'))))

                {
                    try
                    {
                        SetText(editBox, "Http://127.0.0.1:2005");
                        editBox.SetFocus();
                    }
                    catch
                    { }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                return false;
            }

        }
        public static class Edge
        {

            [DllImport("user32.dll")]
            private static extern IntPtr GetForegroundWindow();


            public static IEnumerable<AutomationElement> FindInRawView(AutomationElement root)
            {
                TreeWalker rawViewWalker = TreeWalker.RawViewWalker;
                Queue<AutomationElement> queue = new Queue<AutomationElement>();
                queue.Enqueue(root);
                var child = rawViewWalker.GetFirstChild(root);
                while (queue.Count > 0)
                {
                    var ch = rawViewWalker.GetNextSibling(child);
                    if (ch == null)
                        return queue.AsEnumerable();
                    queue.Enqueue(ch);


                }
                return queue.AsEnumerable();
            }
            private static void WalkControlElements(AutomationElement rootElement, TreeNode treeNode)
            {
                AutomationElement elementNode = TreeWalker.ContentViewWalker.GetFirstChild(rootElement); ;

                while (elementNode != null)
                {
                    MessageBox.Show(GetText(elementNode) + " " + elementNode.Current.Name);
                    TreeNode childTreeNode = treeNode.Nodes.Add(elementNode.Current.ControlType.LocalizedControlType);

                    // here I want to get text from 'elementNode'                    
                    WalkControlElements(elementNode, childTreeNode);
                    elementNode = TreeWalker.ControlViewWalker.GetNextSibling(elementNode);
                }
            }
            public static bool CheckEdge(string[] args)
            {
                string oldUrl = "";
                string oldTitle = "";

                //foreach (var win in GetAllWindows())
                //{
                string url = "";
                string title = "";
                try
                {
                    var win = GetForegroundWindow();
                    AutomationElement au = AutomationElement.FromHandle(win);
                    if (GetText(au).Contains("Edge"))
                    {
                        var nameCondition = new PropertyCondition(AutomationElement.NameProperty, "Address and search bar");
                        var controlCondition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit);
                        var condition = new AndCondition(nameCondition, controlCondition);
                        AutomationElement editBox = au.FindFirst(System.Windows.Automation.TreeScope.Subtree, condition);
                        if (args.Any((d) => GetText(editBox).ToLower().Contains(d.ToLower().Replace("https://", "").Replace("www.", "").Replace("http://", "").TrimEnd('/'))))

                        {
                            try
                            {
                                SetText(editBox, "Http://127.0.0.1:2005");
                                editBox.SetFocus();
                            }
                            catch
                            { }
                            return true;
                        }
                        //TreeNode treeNode = new TreeNode();
                        // WalkControlElements(au, treeNode);
                        //FindAll(TreeScope.Subtree, Condition.TrueCondition);
                        //MessageBox.Show(au.GetCachedPattern);
                    }
                }
                catch { }
                if (url != "")
                    MessageBox.Show(String.Format("Page title: {0} \r\nURL: {1}", title, url));


                return false;
            }
        }
    }
    public static class WebroamUtility
    {
        public static void appShortcutToDesktop(string linkName, string app = null)
        {
            if (app == null)
                app = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            using (StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".url"))
            {
                
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + app);
                writer.WriteLine("IconIndex=0");
                string icon = app.Replace('\\', '/');
                writer.WriteLine("IconFile=" + icon);
            }
        }

        static void MakeFolderLink(string link, string target)
        {
            string cmda = $"/c mklink /D \"{link}\" \"{target}\"";
            ProcessStartInfo psi = new ProcessStartInfo($"cmd.exe", cmda);
            Clipboard.SetText(cmda);
            psi.Verb = "runas";
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            var p = Process.Start(psi);
            p.WaitForExit();
        }
    }
    class IniFile   // revision 11
    {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName.ToString();
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            //MessageBox.Show(RetVal.ToString());
            return RetVal.ToString() == "1" || RetVal.ToString().ToLower() == "true" ? "True" : RetVal.ToString();
        }
        public string ReadNoneBool(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            //MessageBox.Show(RetVal.ToString());
            return RetVal.ToString();
        }
        public void Write(string Key, string Value, string Section = null)
        {         

            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
    // get open handles by processes used in memory scan
    class Handle
    {
        [DllImport("ntdll.dll")]
        public static extern uint NtQuerySystemInformation(int
            SystemInformationClass, IntPtr SystemInformation, int SystemInformationLength,
            ref int returnLength);

        [DllImport("kernel32.dll", EntryPoint = "RtlCopyMemory")]
        static extern void CopyMemory(byte[] Destination, IntPtr Source, uint Length);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SYSTEM_HANDLE_INFORMATION
        { // Information Class 16
            public int ProcessID;
            public byte ObjectTypeNumber;
            public byte Flags; // 0x01 = PROTECT_FROM_CLOSE, 0x02 = INHERIT
            public ushort Handle;
            public int Object_Pointer;
            public UInt32 GrantedAccess;
        }

        const int CNST_SYSTEM_HANDLE_INFORMATION = 16;
        const uint STATUS_INFO_LENGTH_MISMATCH = 0xc0000004;


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetFinalPathNameByHandle(IntPtr handle, [In, Out] StringBuilder path, int bufLen, int flags);


        [HandleProcessCorruptedStateExceptionsAttribute()]
        private static List<SYSTEM_HANDLE_INFORMATION> GetHandles(Process process)
        {
            try
            {
                uint nStatus;
                int nHandleInfoSize = Marshal.SizeOf(typeof(SYSTEM_HANDLE_INFORMATION));
                IntPtr ipHandlePointer = Marshal.AllocHGlobal(nHandleInfoSize);
                int nLength = 0;
                IntPtr ipHandle = IntPtr.Zero;

                while ((nStatus = NtQuerySystemInformation(CNST_SYSTEM_HANDLE_INFORMATION, ipHandlePointer, nHandleInfoSize, ref nLength)) == STATUS_INFO_LENGTH_MISMATCH)
                {
                    nHandleInfoSize = nLength;
                    Marshal.FreeHGlobal(ipHandlePointer);
                    ipHandlePointer = Marshal.AllocHGlobal(nLength);
                }

                //byte[] baTemp = new byte[nLength];
                //  CopyMemory(baTemp, ipHandlePointer, (uint)nLength);

                long lHandleCount = 0;
                if (Is64Bits())
                {
                    lHandleCount = Marshal.ReadInt64(ipHandlePointer);
                    ipHandle = new IntPtr(ipHandlePointer.ToInt64() + 8);
                }
                else
                {
                    lHandleCount = Marshal.ReadInt32(ipHandlePointer);
                    ipHandle = new IntPtr(ipHandlePointer.ToInt32() + 4);
                }

                SYSTEM_HANDLE_INFORMATION shHandle;
                List<SYSTEM_HANDLE_INFORMATION> lstHandles = new List<SYSTEM_HANDLE_INFORMATION>();

                for (long lIndex = 0; lIndex < lHandleCount; lIndex++)
                {
                    shHandle = new SYSTEM_HANDLE_INFORMATION();
                    if (Is64Bits())
                    {
                        shHandle = (SYSTEM_HANDLE_INFORMATION)Marshal.PtrToStructure(ipHandle, shHandle.GetType());
                        ipHandle = new IntPtr(ipHandle.ToInt64() + Marshal.SizeOf(shHandle) + 8);
                    }
                    else
                    {
                        ipHandle = new IntPtr(ipHandle.ToInt64() + Marshal.SizeOf(shHandle));
                        shHandle = (SYSTEM_HANDLE_INFORMATION)Marshal.PtrToStructure(ipHandle, shHandle.GetType());
                    }

                    if (shHandle.ProcessID != process.Id) continue;

                    lstHandles.Add(shHandle);
                }
                Marshal.FreeHGlobal(ipHandlePointer);
                return lstHandles;
            }
            catch (AccessViolationException em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);               
                return null;
            }
        }
        public static List<string> getOpenFileNames(Process procs)
        {
            var handles = GetHandles(procs);
            if (handles == null)
            {
                try
                {
                    var info = new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "handle64.exe"));
                    info.CreateNoWindow = true;
                    info.UseShellExecute = false;
                    info.RedirectStandardOutput = true;
                    info.WindowStyle = ProcessWindowStyle.Hidden;
                    var p = Process.Start(info);
                   // p.WaitForExit();
                    var oresults = p.StandardOutput.ReadToEnd().Split('\n').ToList().FindAll(s1 => (s1.Contains(":\\")&&(s1.ToLowerInvariant().EndsWith(".dll"))|| s1.ToLowerInvariant().EndsWith(".exe"))).Select(s2 => s2.Substring(s2.IndexOf(":\\") - 1).ToLowerInvariant()).ToList().Distinct().ToList();
                    p.WaitForExit();
                    return oresults;
                }
                catch
                {
                    return null;
                }
            }
            List<string> re = new List<string>();
            foreach(var v in handles)
            {
                StringBuilder sb = new StringBuilder(512);
                GetFinalPathNameByHandle(new IntPtr(v.Handle), sb, sb.Capacity, 0x8);
                if (sb.ToString() != "" && !new DirectoryInfo(sb.ToString().Replace("\\\\?\\", "")).Exists)
                {
                    re.Add(sb.ToString().Replace("\\\\?\\", ""));
                }
            }
            return re;
        }
        static bool Is64Bits()
        {
            return Marshal.SizeOf(typeof(IntPtr)) == 8 ? true : false;
        }
    }


    // wifi security class
    class WifiSafety
    {
        static WlanClient client = null;
        static bool bstop = false;
        static object alock = new object();
        public static void Start()
        {
            lock (alock)
            {
                bstop = false;


            }
            try
            {
                client = new WlanClient();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            WlanClient.WlanInterface wlanIface = null;
            try
            {
                wlanIface = client.Interfaces.FirstOrDefault();
            }
            catch { }
            if (wlanIface == null)
            {
                Console.WriteLine("No Wifi Interface available!");
                //throw new Exception("No Wifi Interface available!");
                return;
            }

            // Lists all available networks



            /*  Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(0);
              foreach (Wlan.WlanAvailableNetwork network in networks)
              {

                  Console.WriteLine("Found network with SSID {0} || Secured : {1}.", GetStringForSSID(network.dot11Ssid), network.securityEnabled);
              }*/
            if (wlanIface.InterfaceState == Wlan.WlanInterfaceState.Connected && !wlanIface.CurrentConnection.wlanSecurityAttributes.securityEnabled)
                new gMbox(String.Format("Current Wifi [{0}] Network Not secured", wlanIface.CurrentConnection.profileName), "Alert").Show();

            wlanIface.WlanConnectionNotification += Program_WlanConnectionNotification;
            Task.Factory.StartNew(() =>{while (true) {
                    lock (alock)
                    {
                        if (bstop)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(900);
                } }).Wait();
        }
        public static void Stop()
        {
            lock (alock)
            {
                bstop = true;


            }
        }
        private static void Program_WlanConnectionNotification(Wlan.WlanNotificationData notifyData, Wlan.WlanConnectionNotificationData connNotifyData)
        {
            // WlanClient client = null;
            Console.WriteLine("connection!!");
            if (client == null)
            {
                try
                {
                    client = new WlanClient();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            WlanClient.WlanInterface wlanIface = client.Interfaces.FirstOrDefault();

            if (wlanIface == null)
            {
                Console.WriteLine("No Wifi Interface available!");
                //throw new Exception("No Wifi Interface available!");
                return;
            }

            // Lists all available networks



            /*  Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(0);
              foreach (Wlan.WlanAvailableNetwork network in networks)
              {

                  Console.WriteLine("Found network with SSID {0} || Secured : {1}.", GetStringForSSID(network.dot11Ssid), network.securityEnabled);
              }*/
            if (wlanIface.InterfaceState == Wlan.WlanInterfaceState.Connected)
                Console.WriteLine("Current Wifi [{0}] Network secured? {1}", wlanIface.CurrentConnection.profileName, wlanIface.CurrentConnection.wlanSecurityAttributes.securityEnabled);

        }

        static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
        }
    }

    // Auto update class
    public static class wrUpdate
    {
        private static System.Timers.Timer timer;
        private static void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args != null)
            {
                if (args.IsUpdateAvailable)
                {
                    if (args.Mandatory)
                    {
                    }
                    else
                    {
                        if (MessageBox.Show($@"There is new version {args.CurrentVersion} available. You are using version {args.InstalledVersion}. Do you want to update the application now?", @"Update Available",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        {
                            timer.Stop();
                            return;
                        }
                    }
                    AutoUpdater.Mandatory = true;
                    AutoUpdater.UpdateMode = Mode.Forced;


                    if (AutoUpdater.DownloadUpdate())
                    {
                        ProcessProtector.isOn = false;
                        //System.Windows.Application.Current.Shutdown();
                        Environment.Exit(0);
                    }
                }
            }
        }
        private static bool ofUp = false;
        public static void Update()
        {
            if (!IsLicenseValid && (DateTime.Now.Month > 11 && DateTime.Now.Year > 2022))
                    return;
            timer = new System.Timers.Timer(100);
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            AutoUpdater.DownloadPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            BasicAuthentication basicAuthentication = new BasicAuthentication("wrupdat1av", "Mv5T@fa.Up1");
            AutoUpdater.BasicAuthXML = AutoUpdater.BasicAuthDownload = basicAuthentication;
            timer.Elapsed += delegate
            {
                AutoUpdater.Start(File.ReadAllText(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\update.txt").Trim().Replace("[product]", Assembly.GetExecutingAssembly().FullName));
                // timer.Stop();
                int count = SqlReaderWriter.MaxofRow("tblReportFor");
                int rpfID = 9;
                int iDate = Int32.Parse(string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                string sTime = DateTime.Now.ToLongTimeString();
                string repfor = "Update Check";
                SqlReaderWriter.ExecuteQuery("INSERT INTO tblReportFor (ID, ReportFID, Date, Time, ReportFor) VALUES (" + (count + 1) + ", " + rpfID + ", " + iDate + ", '" + sTime + "', '" + repfor + "\n')");
                if (App.AppSettings.AU_PICK_PATH == "")
                {
                    AutoUpdater.Start(File.ReadAllText(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\update.txt").Replace("[product]", Assembly.GetExecutingAssembly().FullName));
                }
                else if (!ofUp)
                {
                    FileSystemWatcher watcher = new FileSystemWatcher(App.AppSettings.AU_PICK_PATH, "*.*");
                    watcher.Changed += Watcher_Changed;
                    watcher.IncludeSubdirectories = true;
                    watcher.NotifyFilter = NotifyFilters.LastWrite;
                    watcher.EnableRaisingEvents = true;
                    ofUp = true;
                }
                timer.Interval = 16 * 60 * 1000;
            };
            timer.Start();
           
            if (App.AppSettings.AU_PICK_PATH != "")
            {
                FileSystemWatcher watcher = new FileSystemWatcher(App.AppSettings.AU_PICK_PATH, "*.*");
                watcher.Changed += Watcher_Changed;
                watcher.IncludeSubdirectories = true;
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                watcher.EnableRaisingEvents = true;
                ofUp = true;
            }
           
                timer.Start();
            
            
        }

        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (MessageBox.Show($@"There is new version available. Do you want to update the application now?", @"Offline Update Available",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (e.FullPath.ToLower().EndsWith(".wup"))
            {
                Process.Start(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+ "\\ZipExtractor.exe", $"\"{e.FullPath}\" \"{Process.GetCurrentProcess().MainModule.FileName}\"");
                //AppSettings.IsSelfProtected = false;
                ProcessProtector.isOn = false;

                Environment.Exit(0);
            }
        }
    }

    // Licensing class
    public static class LicenseClass
    {
        public static byte[] Aes256Decrypt(byte[] v)
        {
            var cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
            var key = ASCIIEncoding.ASCII.GetBytes(("Th@Wbroam$K12317" + "1001" + "    " + "12345678"));

            /* //pad key out to 32 bytes (256bits) if its too short
             if (key.Length < 32)
             {
                 var paddedkey = new byte[32];
                 Buffer.BlockCopy(key, 0, paddedkey, 0, key.Length);
                 key = paddedkey;
             }*/

            //setup an empty iv
            // var iv = new byte[16];

            //get the encrypted data and decrypt
            //byte[] encryptedBytes = SHexToByteArray(StrToHex("VDFUUR-JJEURA-IGFRAR-TTGJTJ-HBSFVB"));
            var crte = new KCtrBlockCipher(new AesEngine());
            var encryptKeyParameter = new KeyParameter(key);
            cipher.Init(false, new ParametersWithIV(ParameterUtilities.CreateKeyParameter("AES", key), ASCIIEncoding.ASCII.GetBytes("1234567812345678")));
            byte[] decr = new byte[1024];
            int len = cipher.ProcessBytes(v, decr, 0);
            len += cipher.DoFinal(decr, len);
            Array.Resize(ref decr, len);
            return decr;
        }
        private static string unpack(string p1, string input)
        {
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                string a = Convert.ToInt32(input[i]).ToString("X");
                output.Append(a);
            }

            return output.ToString();
        }

        public static byte[] SHexToByteArray(String hex)
        {

            hex = hex.Length % 2 != 0 ? "0" + hex : hex;
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        public static string StrToHex(string str)
        {
            var st = str.Replace("-", "");
            //mysqli_close();
            // MessageBox.Show(st);
            StringBuilder rstr = new StringBuilder();
            for (int i = 0; i < st.Length; ++i)
            {
                // MessageBox.Show(st[i]+" "+ Int32.Parse(((int)(st[i] - 'A')).ToString()));
                rstr.Append(Convert.ToString(Int32.Parse(((int)(st[i] - 'A')).ToString()), 16));
            }
            //MessageBox.Show(rstr.ToString());
            return rstr.ToString();
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

    }



    // Registry monitor class
    public class RegistryEventWatcher
    {
        public delegate void RegistryWatcherEvent(object sender, EventArrivedEventArgs e);
        public ManagementEventWatcher WatchForthisKeyModification(string hive, string key, RegistryWatcherEvent myevent)
        {
            ManagementScope Scope = new ManagementScope("\\\\.\\root\\default");
            EventQuery Query = new EventQuery(@"SELECT * FROM RegistryKeyChangeEvent WHERE Hive='" + hive + "' AND KeyPath='SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall'");
            ManagementEventWatcher watcher = new ManagementEventWatcher(Scope, Query);
            watcher.EventArrived += new EventArrivedEventHandler(myevent);
            watcher.Start();
            return watcher;
        }

        public ManagementEventWatcher WatchForthisKeyModification(string key, bool isFullRegEntry, RegistryWatcherEvent myevent)
        {
            string hive = "";
            if (!isFullRegEntry) hive = "HKEY_LOCAL_MACHINE";
            return WatchForthisKeyModification(hive, key, myevent);
        }

    }


    //Startup protector class monitors if AV is in windows startup not removed by malwares
    public class StartUpProtector
    {
        private string m_appname = "WebRoamAV.exe";
        private string m_path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "WebRoamAV.exe");
        void RegistryWatcherEvent(object sender, EventArrivedEventArgs e)
        {
            RegEdit re = new RegEdit();
            re.baseRegistryKey = Registry.LocalMachine;
            re.subKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            if (re.RegRead(m_appname) != m_path)
            {
                re.RegWrite("webroam_app", m_path);
            }
        }
        public void Protect(string appname, string path)
        {
            m_appname = appname;
            m_path = path;
            RegistryEventWatcher rew = new RegistryEventWatcher();
            rew.WatchForthisKeyModification("HKEY_CURRENT_USER", "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", RegistryWatcherEvent);
        }
    }


    //Access control of files & controls class we need it in AntiRansomeware too
    public static class AccessControl
    {
        public static void AddFileSecurity(string fileName, string account,
              FileSystemRights rights, AccessControlType controlType)
        {

            // Get a FileSecurity object that represents the
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Add the FileSystemAccessRule to the security settings.
            fSecurity.AddAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);
        }

        // Removes an ACL entry on the specified file for the specified account.
        public static void RemoveFileSecurity(string fileName, string account,
            FileSystemRights rights, AccessControlType controlType)
        {

            // Get a FileSecurity object that represents the
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Remove the FileSystemAccessRule from the security settings.
            fSecurity.RemoveAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);
        }


        public static void SetAccessFileDenyOrAllow(string arg1, FileSystemRights ef = FileSystemRights.ExecuteFile, AccessControlType act = AccessControlType.Deny)
        {
            AccessControl.AddFileSecurity(arg1, System.Security.Principal.WindowsIdentity.GetCurrent().Name, ef, act);
        }
    }

    //self protection class
    public class ProcessProtector
    {
        public static object oluck = new object();
        public static bool isOn = true;
        private string MySecondProcess = "onlineProtector";
        public static ClamAVEngine cen = null;
        public ProcessProtector()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }
        public void ProcessProtect()
        {
            //var info = new System.Diagnostics.ProcessStartInfo(Application.ExecutablePath);
            cen = new ClamAVEngine();
            lock (oluck)
            {
                if (isOn && Process.GetProcessesByName(MySecondProcess).Length < 1)
                {
                    var info = new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), MySecondProcess + ".exe"));
                    info.CreateNoWindow = true;
                    info.UseShellExecute = false;
                    info.RedirectStandardOutput = true;
                    info.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.Start(info);
                }
            }
           
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if(isOn)
            Process.Start(Process.GetCurrentProcess().MainModule.FileVersionInfo.FileName);
        }

        public ManagementEventWatcher WatchForProcessStart(string processName)
        {
            string queryString =
                "SELECT TargetInstance" +
                "  FROM __InstanceCreationEvent " +
                "WITHIN  10 " +
                " WHERE TargetInstance ISA 'Win32_Process' " +
                "   AND TargetInstance.Name = '" + processName + "'";

            // The dot in the scope means use the current machine
            string scope = @"\\.\root\CIMV2";

            // Create a watcher and listen for events
            ManagementEventWatcher watcher = new ManagementEventWatcher(scope, queryString);
            watcher.EventArrived += ProcessStarted;
            watcher.Start();
            return watcher;
        }

        public ManagementEventWatcher WatchForProcessEnd(string processName)
        {
            string queryString =
                "SELECT TargetInstance" +
                "  FROM __InstanceDeletionEvent " +
                "WITHIN  10 " +
                " WHERE TargetInstance ISA 'Win32_Process' " +
                "   AND TargetInstance.Name = '" + processName + "'";

            // The dot in the scope means use the current machine
            string scope = @"\\.\root\CIMV2";

            // Create a watcher and listen for events
            ManagementEventWatcher watcher = new ManagementEventWatcher(scope, queryString);
            watcher.EventArrived += ProcessEnded;
            watcher.Start();
            return watcher;
        }

        public void ProcessEnded(object sender, EventArrivedEventArgs e)
        {
            if (!isOn)
                return;
            var eventName = e.NewEvent.ClassPath.ClassName;
            if (eventName.CompareTo("__InstanceDeletionEvent") != 0)
                return;
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
            string processName = targetInstance.Properties["Name"].Value.ToString();
        //    MessageBox.Show(processName);
            if (processName == MySecondProcess)
            {
                Process.Start(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),MySecondProcess+".exe"));
                StartUpProtector su = new StartUpProtector();
                su.Protect(Process.GetCurrentProcess().MainModule.FileVersionInfo.FileName, Process.GetCurrentProcess().MainModule.FileName);
            }
            Console.WriteLine(String.Format("{0} process ended", processName));
        }

        public void ProcessStarted(object sender, EventArrivedEventArgs e)
        {
            var eventName = e.NewEvent.ClassPath.ClassName;
            if (eventName.CompareTo("__InstanceCreationEvent") != 0)
                return;
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
            string processName = targetInstance.Properties["Name"].Value.ToString();
            string pid= targetInstance.Properties["ProcessId"].Value.ToString();
            if (processName == MySecondProcess)
            {
                FileStream fstream = new FileStream(BackUp.destinationFile[0], FileMode.Open);
                fstream.Lock(0, fstream.Length);
            }
            //Console.WriteLine(String.Format("{0} process started", processName));
            var t = Task.Factory.StartNew(() => {
                TcpListener tcpl = new TcpListener(IPAddress.Any, 2552);
               tcpl.Start();
                Socket s;

                while (true)
                {

                    try
                    {
                        s = tcpl.AcceptSocket();
                        byte[] buf = new byte[1024];
                        if (s.Receive(buf) > 0)
                        {
                           
                            if(!String.IsNullOrWhiteSpace(Encoding.ASCII.GetString(buf)))
                                {
                                Task.Factory.StartNew(new Action(() =>
                                {
                                    Process.GetProcessById(Int32.Parse(pid)).Kill();
                                    new wMBox(Encoding.ASCII.GetString(buf), "Infected file!").Show();
                                }));
                                break;
                            }
                        }
                    }
                    catch
                    {
                        break;
                    }
                    }
            });
            //IntPtr parg = Marshal.StringToHGlobalUni(Process.GetProcessById(Int32.Parse(pid)).MainModule.FileName);            
            if(cen!=null)
            cen.Scan(new GRowsCUS[] { new GRowsCUS() { PathF = Process.GetProcessById(Int32.Parse(pid)).MainModule.FileName, SubF = "No" } },5);            
           // Marshal.FreeHGlobal(parg);
        }
    }

    //Firewall control class
    public class FWCtrl
    {
        const string guidFWPolicy2 = "{E2B3C97F-6AE1-41AC-817A-F6F92166D7DD}";
        const string guidRWRule = "{2C5BC43E-3369-4C33-AB0C-BE9469677AF4}";
   
        /*static void Main(string[] args)
        {
        Process proc = new Process();
                string top = "netsh.exe";
                proc.StartInfo.Arguments = "Firewall set opmode enable";
                proc.StartInfo.FileName = top;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.WaitForExit();
            FWCtrl ctrl = new FWCtrl();
            ctrl.Setup("192.168.0.2", "4000", NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN); //NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT
        }*/
        public static List<List<string>> ListFirewallRules()
        {
            List<List<string>> result = new List<List<string>>();
            try
            {
                Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
                var currentProfiles = fwPolicy2.CurrentProfileTypes;

                // List of rules
                // List<INetFwRule> RuleList = new List<INetFwRule>();

                foreach (INetFwRule rule in fwPolicy2.Rules)
                {
                    string prf = (((rule.Profiles & (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE) != 0 ? "Private," : "") + ((rule.Profiles & (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_DOMAIN) != 0 ? "Domain," : "") + ((rule.Profiles & (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC) != 0 ? "Public" : ""));
                    prf = prf.EndsWith(",") ? prf.Remove(prf.Length - 1) : prf;
                    List<string> temp = new List<string>();
                    Dictionary<int, string> prots = new Dictionary<int, string>();
                    prots[1] = "ICMPv4";
                    prots[2] = "IGMP";
                    prots[4] = "IPv4";
                    prots[6] = "TCP";
                    prots[17] = "UDP";
                    prots[41] = "IPv6";
                    prots[256] = "Any";
                    temp.Add(rule.Name);                    
                    temp.Add(prots[(int)rule.Protocol]);
                    temp.Add(rule.Action.ToString().Replace("NET_FW_ACTION_", ""));
                    temp.Add(prf);
                    result.Add(temp);
                    //Console.WriteLine(rule.Name+" "+rule.Protocol+" "+rule.Action+" "+prf);
                }
            }
            catch (Exception ex)
            { }
            return result;
        }

      
        public static void MoveUpFirewallRule(int index)
        {
            try
            {
                Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
                var currentProfiles = fwPolicy2.CurrentProfileTypes;

                // List of rules
                // List<INetFwRule> RuleList = new List<INetFwRule>();
                int findex = 1, findex2 = 1, findex3 = 1;
                string t_rule = "", t_rule2 = "";
                string rulename = "";
                foreach (INetFwRule rule in fwPolicy2.Rules)
                {
                    if (findex == index - 1)
                    {
                        t_rule = rule.Name;
                    }
                    else if (findex == index)
                    {
                        t_rule2 = rule.Name;
                        break;
                    }
                    findex++;
                }
                findex = 1;
                string nt_rule="", nt_rule2="";
                foreach (INetFwRule rule in fwPolicy2.Rules)
                {
                    // Add a rule to list
                    // RuleList.Add(rule);
                    // Console.WriteLine(rule.Name);
                    if (rule.Name== t_rule)
                    {
                        INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                        INetFwRule r = firewallPolicy.Rules.Item(t_rule);
                        r.Name = r.Name + "@#$%" + findex;
                        nt_rule = r.Name;
                        findex++;
                    }
                    if(rule.Name == t_rule2)
                    {
                        INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                        INetFwRule r = firewallPolicy.Rules.Item(t_rule2);
                        r.Name = r.Name + "@#$%" + findex2;
                        nt_rule2 = r.Name;
                        findex2++;
                    }
                    if (index == findex3)
                        break;
                    // Console.WriteLine(rule.Name + " has been deleted from the Firewall Policy");
                    findex3++;

                }

                t_rule = nt_rule;
                t_rule2 = nt_rule2;

                INetFwPolicy2 fp = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                INetFwRule gr1 = fp.Rules.Item(t_rule);                
                INetFwRule gr2 = fp.Rules.Item(t_rule2);
 
                //   deepCopy(firewallRule, gr2);
                var _rule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                _rule.Action = (NET_FW_ACTION_)((int)gr2.Action);//NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                _rule.ApplicationName = gr2.ApplicationName;
                _rule.Description = gr2.Description;
                _rule.Direction = (NET_FW_RULE_DIRECTION_)((int)gr2.Direction);//NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
                _rule.EdgeTraversal = gr2.EdgeTraversal;
                _rule.Enabled = gr2.Enabled;
                _rule.Grouping = gr2.Grouping;
                _rule.Name = gr2.Name;
                _rule.Protocol = (int)gr2.Protocol;
                _rule.Profiles = gr2.Profiles;

                //var _rule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                gr2.Action = (NET_FW_ACTION_)((int)gr1.Action);//NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                gr2.ApplicationName = gr1.ApplicationName;
                gr2.Description = gr1.Description;
                gr2.Direction = (NET_FW_RULE_DIRECTION_)((int)gr1.Direction);//NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
                gr2.EdgeTraversal = gr1.EdgeTraversal;
                gr2.Enabled = gr1.Enabled;
                gr2.Grouping = gr1.Grouping;
                gr2.Name = gr1.Name;
                gr2.Protocol = (int)gr1.Protocol;
                gr2.Profiles = gr1.Profiles;

                gr1.Action = (NET_FW_ACTION_)((int)_rule.Action);//NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                gr1.ApplicationName = _rule.ApplicationName;
                gr1.Description = _rule.Description;
                gr1.Direction = (NET_FW_RULE_DIRECTION_)((int)_rule.Direction);//NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
                gr1.EdgeTraversal = _rule.EdgeTraversal;
                gr1.Enabled = _rule.Enabled;
                gr1.Grouping = _rule.Grouping;
                gr1.Name = _rule.Name;
                gr1.Protocol = (int)_rule.Protocol;
                gr1.Profiles = _rule.Profiles;

                Type tNetFwPolicy21 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                INetFwPolicy2 fwPolicy21 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
                var currentProfiles1 = fwPolicy2.CurrentProfileTypes;

                foreach (INetFwRule rule in fwPolicy21.Rules)
                {
                   // MessageBox.Show(rule.Name);
                    if (rule.Name.IndexOf("@#$%")!= -1)
                    {
                       // MessageBox.Show(rule.Name);
                        INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                        INetFwRule r2 = firewallPolicy.Rules.Item(rule.Name);
                        r2.Name = rule.Name.Substring(0, rule.Name.IndexOf("@#$%"));
                     //   MessageBox.Show(": "+r2.Name);
                    }
                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
        }
        public static void RemoveFirewallRules(string RuleName, int index)
        {
            try
            {
                Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
                var currentProfiles = fwPolicy2.CurrentProfileTypes;

                // List of rules
                // List<INetFwRule> RuleList = new List<INetFwRule>();
                int findex = 1;
                foreach (INetFwRule rule in fwPolicy2.Rules)
                {
                    // Add a rule to list
                    // RuleList.Add(rule);
                    // Console.WriteLine(rule.Name);
                    
                    if (rule.Name == RuleName)
                    {
                        
                        // Remove a rule
                        INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                        INetFwRule r = firewallPolicy.Rules.Item(RuleName);
                        r.Name = RuleName + "@#$%" + findex.ToString();
                        firewallPolicy.Rules.Remove(RuleName+"@#$%"+index);
                        INetFwRule r2 = firewallPolicy.Rules.Item(RuleName + "@#$%" + index);
                        if (r2 != null)
                            r2.Name = RuleName;


                        findex++;
                        // Console.WriteLine(rule.Name + " has been deleted from the Firewall Policy");
                    
                    }
                }
            }
            catch (Exception r)
            {
                Console.WriteLine("Error deleting a Firewall rule");
            }
        }
        public void Setup(string name, NET_FW_IP_PROTOCOL_ protocol, string ip, string port, string remote_ip, string remote_port, NET_FW_ACTION_ action, int profiles, NET_FW_RULE_DIRECTION_ direction)
        {
            Type typeFWPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");//Type.GetTypeFromCLSID(new Guid(guidFWPolicy2));
            Type typeFWRule = Type.GetTypeFromProgID("HNetCfg.FWRule");//Type.GetTypeFromCLSID(new Guid(guidRWRule));
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(typeFWPolicy2);
            INetFwRule newRule = (INetFwRule)Activator.CreateInstance(typeFWRule);
            newRule.Name = name;
            newRule.Description = "rule by webroam";
            newRule.Protocol = (int)protocol;
            newRule.LocalPorts = port;
            newRule.RemoteAddresses = ip; //
            newRule.Direction = direction;
            newRule.Enabled = true;
            newRule.Grouping = "@firewallapi.dll,-23255";
            newRule.Profiles = profiles;
            newRule.Action = action;            
            fwPolicy2.Rules.Add(newRule);
        }
    }

    //Registry utility class
    public class RegEdit
    {
        public RegistryKey baseRegistryKey = Registry.LocalMachine;
        public string subKey = "";
        public bool RegDeleteKey(string KeyName)
        {
            try
            {
                // Setting
                RegistryKey rk = baseRegistryKey;
                RegistryKey sk1 = rk.CreateSubKey(subKey);
                // If the RegistrySubKey doesn't exists -> (true)
                if (sk1 == null)
                    return true;
                else
                    sk1.DeleteValue(KeyName);

                return true;
            }
            catch
            {
                // AAAAAAAAAAARGH, an error!
                //ShowErrorMessage(e, "Deleting SubKey " + subKey);                
                return false;
            }
        }

        public string RegRead(string KeyName)
        {
            // Opening the registry key
            RegistryKey rk = baseRegistryKey;
            // Open a subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey(subKey);
            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                return null;
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value
                    // or null is returned.
                    return (string)sk1.GetValue(KeyName.ToUpper());
                }
                catch
                {
                    // AAAAAAAAAAARGH, an error!
                    //ShowErrorMessage(e, "Reading registry " + KeyName.ToUpper());
                    return null;
                }
            }
        }
        public bool RegWrite(string KeyName, object Value)
        {
            try
            {
                // Setting
                RegistryKey rk = baseRegistryKey;
                // I have to use CreateSubKey 
                // (create or open it if already exits), 
                // 'cause OpenSubKey open a subKey as read-only
                RegistryKey sk1 = rk.CreateSubKey(subKey);
                // Save the value
                sk1.SetValue(KeyName.ToUpper(), Value);

                return true;
            }
            catch
            {
                // AAAAAAAAAAARGH, an error!
                //ShowErrorMessage(e, "Writing registry " + KeyName.ToUpper());
                return false;
            }
        }
    }

    //Quarantine class
    public class Quarantine
    {
        public void KillProcessesAssociatedToFile(string file)
        {
            GetProcessesAssociatedToFile(file).ForEach(x =>
            {
                x.Kill();
                x.WaitForExit(10000);
            });
        }

        public List<Process> GetProcessesAssociatedToFile(string file)
        {
            var pses = Process.GetProcesses();
            List<Process> result = new List<Process>();
            foreach(var p in pses)
            {
                if(!p.HasExited && p.Modules.Cast<ProcessModule>().Any(a=>a.FileName.ToLowerInvariant()==file.ToLowerInvariant()))
                result.Add(p);
            }
            return result;/* Process.GetProcesses()
                .Where(x => !x.HasExited
                    && x.Modules.Cast<ProcessModule>().ToList()
                        .Exists(y => y.FileName.ToLowerInvariant() == file.ToLowerInvariant())
                    ).ToList();*/
        }
        public void QuaFile(string file, string vname="")
        {
            try
            {
                string file1;
                if (file.Split('.').Last().StartsWith("Zz"))
                    return;



                /*  if ((file.Split('\\').Last()).Any(f => Path.GetInvalidFileNameChars().Contains(f)))
                  {
                      MessageBox.Show("Yes");*/
                string fname = new Random().Next().ToString() + "." + "Zz";
                    file1 = file.Substring(0,file.LastIndexOf("\\"))+"\\"+fname;
               // MessageBox.Show(file1);
                int count = SqlReaderWriter.MaxofRow("tblQuarantine") +1;
               
                string location = file;
                string dttm = DateTime.Now.ToString();
                string virnm = vname;
                SqlReaderWriter.ExecuteQuery($"INSERT INTO tblQuarantine (ID,FileName,Location,QuarantinedOn,VirusName,SendStatus,Type) VALUES ({count}, '{fname}','{location}','{dttm}', '{virnm}', 0, 0)");
                //MessageBox.Show(file1);
                var psi1 = new ProcessStartInfo("CMD.exe", " /C Ren \"" + file +"\" \""+file1.Split('\\').Last() + "\"");
                    psi1.CreateNoWindow = true;
                    psi1.WindowStyle = ProcessWindowStyle.Hidden;

                    Process.Start(psi1).WaitForExit();
                  //  MessageBox.Show(psi1.FileName+" "+psi1.Arguments);
           //     }

                    BackUp.destinationFile[0] = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "wrTemp\\Qs.dat");
                file = file1;
                //   MessageBox.Show(file);
                try
                {
                    BackUp.getBackUpfromFile(file);
                }
                catch
                { }

                // MessageBox.Show(file);


                try
                    {
                    //Process.GetProcesses().ToList().FindAll((p) => p.MainModule.FileName.ToLowerInvariant() == file.ToLowerInvariant()).ForEach((p)=>p.Kill());
                        var ps = GetProcessesAssociatedToFile(file);
                        foreach (var v in ps)
                        {
                            v.Kill();
                        }
                    }
                    catch
                    { }
                string file2 = file.Replace("\\", "\\\\").Replace(",", "%");
                var psi = new ProcessStartInfo("wmic.exe", " process where \"ExecutablePath LIKE '" + file2 + "'\" delete");
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.RedirectStandardOutput = true;
                psi.UseShellExecute = false;
                var p1 = Process.Start(psi);
                //MessageBox.Show(psi.FileName+" "+psi.Arguments+Environment.NewLine+p1.StandardOutput.ReadToEnd());
                p1.WaitForExit();
                
                            var psi2 = new ProcessStartInfo("CMD.exe", " /C attrib -H -S \"" + file + "\"");
                            psi2.CreateNoWindow = true;
                            psi2.WindowStyle = ProcessWindowStyle.Hidden;                
                var p = Process.Start(psi2);
                p.WaitForExit();
                var psi3 = new ProcessStartInfo("CMD.exe", " /C del /F \"" + file + "\"");
                psi3.CreateNoWindow = true;
                psi3.WindowStyle = ProcessWindowStyle.Hidden;
               
                Process.Start(psi3).WaitForExit();
                // MessageBox.Show(psi3.FileName + " " + psi.Arguments);

                //MessageBox.Show(ex.Message); 
                /*if (Path.GetExtension(file).ToLower()==".exe")
                {
                    //File.Delete(file);

                }*/

            }                
            catch(Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
                 
        }
        public void ExtractQuaFile(string file)
        {
            

        }
        public void StopExeFile(string file)
        {
            RegEdit re = new RegEdit();
            re.subKey = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Image File Execution Options\\"+Path.GetFileName(file);
            re.RegWrite("Debugger", file);
        }
        public void UnStopExeFile(string file)
        {
            RegEdit re = new RegEdit();
            re.subKey = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Image File Execution Options\\";
            re.RegDeleteKey(Path.GetFileName(file));
        }

    }

    public static class gSettings
    {
        public static string Password = "";
    }


    //USB protection class
    public class USBStore
    {

        static void usbInsertion(object sender, EventArrivedEventArgs e)
        {
            try
            {
                string driveName = e.NewEvent.Properties["DriveName"].Value.ToString();

                // Console.WriteLine(driveName);

                foreach (ManagementObject drive in new ManagementObjectSearcher("select * from Win32_LogicalDisk where Name='" + driveName + "'").Get())
                {
                    // associate physical disks with partitions
                    foreach (ManagementObject b in drive.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementObject c in b.GetRelated("Win32_DiskDrive"))
                        {
                            // Console.WriteLine(c["DeviceID"].ToString());
                            Task.Factory.StartNew(() => Block(c["DeviceID"].ToString().Replace(@"\", @"\\"), c["PNPDeviceID"].ToString()));
                        }
                    }
                    //doBlkOp(drive["DeviceID"].ToString(), drive["PNPDeviceID"].ToString());

                }
                if (DriveInfo.GetDrives().Contains(new DriveInfo(e.NewEvent.Properties["DriveName"].Value.ToString())))
                {
                    Task.Run(() => App.func(null, null));
                }
                Thread.Sleep(1500);
                string filename = "secureUSB.txt";
                if (File.Exists(filename))
                {

                    if (File.ReadAllText(filename).Contains(e.NewEvent.Properties["DriveName"].Value.ToString()))
                    {
                        AutorunFunctionality(false);
                        return;
                    }
                }
                AutorunFunctionality(!AppSettings.IsAutoRunProtectOn);

                lock (EDDevices.DevLock)
                {
                    if (AppSettings.IsScanExternalDevsOn)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(() => new wMBox("External Device Scan Running Now! Time is " + DateTime.Now.ToString(), "Info").Show());
                        // MessageBox.Show(ProcessProtector.cen.ToString());
                        string s = "";


                        //         bool end = false;
                        // MessageBox.Show(":" + myEnum1.SearchPath);

                        foreach (var strFileName in new DirectoryInfo(e.NewEvent.Properties["DriveName"].Value.ToString()).GetFiles("*", (AppSettings.IsScanFullDrive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)))
                        { //MessageBox.Show(strDirName);
                          // ilsdr++;                                    

                            string fscn = strFileName.FullName;
                            foreach (var t in ProcessProtector.cen.Scan(new GRowsCUS[] { new GRowsCUS() { PathF = fscn, SubF = "Yes" } }, 5))
                            {
                                if (!String.IsNullOrWhiteSpace(t) && (t.Contains("$")))
                                {
                                    s += t;

                                }
                            }
                            // MessageBox.Show(fscn);
                            // new Quarantine().QuaFile(infs[0].Split('$')[1]);
                            if(!String.IsNullOrWhiteSpace(s))
                            System.Windows.Application.Current.Dispatcher.Invoke(() => new wMBox(s.Replace("$", ":"), "Alert").Show());
                        }
                    }
                }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
            // Block(driveName);
        }

        public static void AutorunFunctionality(bool enable)
        {
            RegistryKey Rkey;

            Rkey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
            if (!enable)
                Rkey.SetValue("NoDriveTypeAutoRun", 255); //disable for all media types, recommended 
            else
                Rkey.SetValue("NoDriveTypeAutoRun", 145); //enable

        }
        public static void Run(string[] args)
        {
            try
            {
                if (ProcessProtector.cen == null)
                    ProcessProtector.cen = new ClamAVEngine();
                AutorunFunctionality(false);
                ManagementEventWatcher watcher = new ManagementEventWatcher();
                WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2");
                watcher.EventArrived += new EventArrivedEventHandler(usbInsertion);
                watcher.Query = query;
                watcher.Start();
                while (true)
                {
                    watcher.WaitForNextEvent();
                }
                watcher.Stop();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
        private static void doBlkOp(string deviceid, string pnpdeviceid)
        {

            foreach (ManagementObject partition in new ManagementObjectSearcher(
                    "ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + deviceid
                      + "'}").Get())
            {
                Console.WriteLine("Partition=" + partition["Name"]);

                // associate partitions with logical disks (drive letter volumes)

                foreach (ManagementObject disk in new ManagementObjectSearcher(
                    "ASSOCIATORS OF {Win32_DiskPartition.DeviceID='"
                      + partition["DeviceID"]
                      + "'} WHERE AssocClass = Win32_LogicalDiskToPartition").Get())
                {
                    //Console.WriteLine("Disk=" + drive["PNPDeviceID"]);

                    string pwrargs = ((File.Exists("permListdevs.conf") ? !File.ReadAllLines("permListdevs.conf").Contains(pnpdeviceid) : false) ? "Disable-PnpDevice" : "Enable-PnpDevice") + "  -Confirm:$false -InstanceId '" + pnpdeviceid + "'"; //"Enable-PnpDevice  -Confirm:$false -InstanceId '" + drive["PNPDeviceID"]+"'"
                    var powershell = PowerShell.Create().AddScript(pwrargs);
                    powershell.Invoke();
                    powershell.Dispose();
                }
            }

        }
        public static void DisEnRemovable(bool Disable)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\RemovableStorageDevices", true))
            {
                if (key != null)
                {
                    key.SetValue("Deny_All", Disable ? "1" : "0", RegistryValueKind.DWord);
                }
            }

        }
        public static void DisEnCD(bool Disable)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\cdrom", true))
            {
                if (key != null)
                {
                    key.SetValue("Start", Disable ? "4" : "1", RegistryValueKind.DWord);
                    key.SetValue("AutoRun", Disable ? "0" : "1", RegistryValueKind.DWord);
                }
               // Console.WriteLine("Restart the PC!!");
                if (MessageBox.Show("System needs restart in order that changes to be affected. \nDo you want to restart the system now?", "Webroam Security", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var psi = new ProcessStartInfo("shutdown", "/r /t 0");
                    psi.CreateNoWindow = true;
                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.Start(psi);
                }
            }
            /*  var drives = from d in DriveInfo.GetDrives() where d.DriveType == DriveType.CDRom select d;
              foreach(var dd in drives)
              {
                  var dr = dd.Name.Replace("\\", "");
                  var path = @"\\.\" + dr;
                  Console.WriteLine(path);
                 // const int FSCTL_DISMOUNT_VOLUME = 0x00090020;

                  var u = new UnmanagedFileLoader(path);
                  u.Load(path);//, FileAccess.Read, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero)
                  uint byteReturned = 0;
                  //const uint FSCTL_LOCK_VOLUME = 0x00090018;
                 // UnmanagedFileLoader.DeviceIoControl(u.Handle, Disable ? UnmanagedFileLoader.EIOControlCode.StorageEjectMedia : UnmanagedFileLoader.EIOControlCode.FsctlUnlockVolume, null, 0, null, 0, ref byteReturned, IntPtr.Zero);                
                 // UnmanagedFileLoader.DeviceIoControl(u.Handle, Disable ? UnmanagedFileLoader.EIOControlCode.FsctlLockVolume : UnmanagedFileLoader.EIOControlCode.FsctlUnlockVolume, null, 0, null, 0, ref byteReturned, IntPtr.Zero);
                  UnmanagedFileLoader.DeviceIoControl(u.Handle, Disable ? UnmanagedFileLoader.EIOControlCode.FsctlDismountVolume : UnmanagedFileLoader.EIOControlCode.FsctlUnlockVolume, null, 0, null, 0, ref byteReturned, IntPtr.Zero);
              }
           */

        }
        public static void MakeReadOnly(bool isreadonly)
        {
            //var redit = new RegEdit();
            var baseRegistryKey = Registry.LocalMachine;
            var subKey = @"SYSTEM\CurrentControlSet\Control";
            if (!baseRegistryKey.OpenSubKey(subKey).GetValueNames().Contains("StorageDevicePolicies"))
            {
                baseRegistryKey.OpenSubKey(subKey, true).CreateSubKey("StorageDevicePolicies");
            }
            subKey += @"\StorageDevicePolicies";
            baseRegistryKey.OpenSubKey(subKey, true).SetValue("WriteProtect", isreadonly ? 1 : 0);

        }
        public static void Block(string deviceid = "", string pnpdeviceid = "", bool disableAll = false)
        {
            //string query= "select * from Win32_DiskDrive where InterfaceType='USB'";

            bool bnblk = (File.Exists("permListdevs.conf") ? !File.ReadAllLines("permListdevs.conf").Contains(pnpdeviceid) : false);
            if (deviceid != "" && pnpdeviceid != "")
            {
                string pwrargs = (bnblk ? "Disable-PnpDevice" : "Enable-PnpDevice") + "  -Confirm:$false -InstanceId '" + pnpdeviceid + "'"; //"Enable-PnpDevice  -Confirm:$false -InstanceId '" + drive["PNPDeviceID"]+"'"
                Console.WriteLine(pwrargs);
                var powershell = PowerShell.Create().AddScript(pwrargs);
                powershell.Invoke();
                powershell.Dispose();
                // return;
            }
            else
            {
                string pwrargs = "Set-ItemProperty  \"HKLM:\\SYSTEM\\CurrentControlSet\\services\\USBSTOR\" -name start -Value " + (disableAll ? "4" : "3"); //"Enable-PnpDevice  -Confirm:$false -InstanceId '" + drive["PNPDeviceID"]+"'"
                Console.WriteLine(pwrargs);
                var powershell = PowerShell.Create().AddScript(pwrargs);
                powershell.Invoke();
                powershell.Dispose();
                //return;
            }/*
            foreach (ManagementObject drive in new ManagementObjectSearcher(
     query).Get())
            {
                // associate physical disks with partitions

                doBlkOp(drive["DeviceID"].ToString(), drive["PNPDeviceID"].ToString());
        // this may display nothing if the physical disk

        // does not have a hardware serial number

        Console.WriteLine("Serial="
     + new ManagementObject("Win32_PhysicalMedia.Tag='"
     + drive["DeviceID"] + "'")["SerialNumber"]);
}*/
        }
    }

    //static site blocker class
    public class BlockSite
    {
        public static void BlockAddress(string address)
        {
            using (StreamWriter sw = File.AppendText(System.Environment.SystemDirectory + "\\drivers\\etc\\hosts"))
            {
                sw.WriteLine("127.0.0.1 " + address);
            }
        }

        public static void RemoveAddress(string address)
        {
            File.AppendAllText(System.Environment.SystemDirectory + "\\drivers\\etc\\hosts", File.ReadAllText(System.Environment.SystemDirectory + "\\drivers\\etc\\hosts").Replace("127.0.0.1 " + address, ""));
        }

        public static void Referesh()
        {
            System.Diagnostics.Process pr = new System.Diagnostics.Process();
            pr.StartInfo.CreateNoWindow = true;
            pr.StartInfo.FileName = "ipconfig";
            pr.StartInfo.Arguments = "/flushdns";
            pr.Start();
            pr.WaitForExit();
        }
    }

    //Backup class
    public class BackUp
    {
        private static FileStream fileStream = null;
        public static string[] destinationFile = { Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "wrTemp\\Qs.dat")    };
        static string password = @"mywrpw1799";
        public static void getBackUpfromFile(string filename)
        {
            //MessageBox.Show(filename.Split('\\').Last());
           
            //MessageBox.Show(destinationFile[0]+Environment.NewLine+filename);
            SevenZipBase.SetLibraryPath(
        Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
        "7z64.dll"));
            
            SevenZipCompressor compressor = new SevenZipCompressor();
            compressor.ArchiveFormat = OutArchiveFormat.SevenZip;
            if (File.Exists(destinationFile[0]))
                compressor.CompressionMode = CompressionMode.Append;
            else
            {
                
                if(!Directory.Exists(Path.GetDirectoryName(destinationFile[0])))
                Directory.CreateDirectory(Path.GetDirectoryName(destinationFile[0]));
                //File.Create(destinationFile[0]);
                compressor.CompressionMode = CompressionMode.Create;
            }
           /* if (!File.Exists(destinationFile[0]))
                File.Create(destinationFile[0]);*/
            compressor.Compressing += Compressor_Compressing;
            compressor.FileCompressionStarted += Compressor_FileCompressionStarted;
            compressor.CompressionFinished += Compressor_CompressionFinished;

     /*       if (fileStream != null)
            {
                try
                {
                    fileStream.Unlock(0, fileStream.Length);
                }
                catch
                { }
            }
            if (fileStream != null)
            {
                fileStream.Close();
                fileStream = null;
            }*/
            string[] sourceFiles = new string[] { filename };//Directory.GetFiles(@"C:\Temp\YourFiles\");

            if (String.IsNullOrWhiteSpace(password))
            {
                compressor.CompressFiles(destinationFile[0], sourceFiles);
            }
            else
            {
                //optional
                compressor.CompressionMethod = CompressionMethod.Deflate;
                compressor.CompressionLevel = CompressionLevel.High;
                compressor.ZipEncryptionMethod = ZipEncryptionMethod.Aes256;
                compressor.ArchiveFormat = OutArchiveFormat.Zip;
                if (!File.Exists(destinationFile[0]))
                    compressor.CompressionMode = CompressionMode.Create;
                else
                    compressor.CompressionMode = CompressionMode.Append;
                compressor.EventSynchronization = EventSynchronizationStrategy.AlwaysAsynchronous;
                compressor.FastCompression = false;
                compressor.EncryptHeaders = true;
                compressor.ScanOnlyWritable = true;
                compressor.CompressFilesEncrypted(destinationFile[0], password, sourceFiles);
                
            }
         /*   if (fileStream != null)
                fileStream.Lock(0, fileStream.Length);
        */}
        public static void RemoveFile(string filename)
        {
            string cmd = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\7z\\7z.exe";
            string args = $"-pmywrpw1799 d {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\wrTemp\\Qs.dat \"{filename}\" -r";
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(cmd+" "+ args);//"netsh", "interface set interface name=\"" + name + "\" admin=DISABLE");
            psi.CreateNoWindow = true;
            psi.Verb = "runas";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.FileName = cmd;
            psi.Arguments = args;
            Process.Start(psi).WaitForExit();
           // Clipboard.SetText(cmd+" "+args);           
        }


            public static void UnBackUpFile(string filename)
        {
            SevenZipBase.SetLibraryPath(Path.Combine(
        Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
        "7z64.dll"));
            SevenZip.SevenZipExtractor extractor = new SevenZipExtractor(destinationFile[0], password);     
            if (File.Exists(destinationFile[0]))
            {
                FileStream fs = new FileStream(filename, FileMode.Create);
                extractor.ExtractFile(filename.Split('\\').Last(), fs);
                fs.Flush();
                fs.Close();
            }
        }

                private static void Compressor_FileCompressionStarted(object sender, FileNameEventArgs e)
        {
          /*  if (fileStream != null)
                fileStream.Unlock(0, fileStream.Length);*/
        }

        private static void Compressor_CompressionFinished(object sender, EventArgs e)
        {
            /*  if (fileStream == null)
              {
                  fileStream = new FileStream(destinationFile[0], FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
              }
              FileSecurity fsecurity = new FileSecurity(destinationFile[0], AccessControlSections.Owner);*/
            //MessageBox.Show("");
            //fsecurity.SetOwner()

        }

        private static void Compressor_Compressing(object sender, SevenZip.ProgressEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }

   
    public class BackUpSettings
    {
        private static FileStream fileStream = null;
        //public static string[] destinationFile = { Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "wrTemp\\Qs.dat") };

        public static void getBackUpfromFiles(string destination, string[] filename)
        {
            //MessageBox.Show(filename.Split('\\').Last());

            //MessageBox.Show(destinationFile[0]+Environment.NewLine+filename);
            SevenZipBase.SetLibraryPath(
        Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
        "7z64.dll"));

            SevenZipCompressor compressor = new SevenZipCompressor();
            if (File.Exists(destination))
                compressor.CompressionMode = CompressionMode.Append;
            else
            {

                if (!Directory.Exists((destination)))
                    Directory.CreateDirectory(Path.GetDirectoryName(destination));
                //File.Create(destinationFile[0]);
                compressor.CompressionMode = CompressionMode.Create;
            }
            /* if (!File.Exists(destinationFile[0]))
                 File.Create(destinationFile[0]);*/
            compressor.Compressing += Compressor_Compressing;
            compressor.FileCompressionStarted += Compressor_FileCompressionStarted;
            compressor.CompressionFinished += Compressor_CompressionFinished;

            /*       if (fileStream != null)
                   {
                       try
                       {
                           fileStream.Unlock(0, fileStream.Length);
                       }
                       catch
                       { }
                   }
                   if (fileStream != null)
                   {
                       fileStream.Close();
                       fileStream = null;
                   }*/
            string[] sourceFiles =  filename;//Directory.GetFiles(@"C:\Temp\YourFiles\");

     
                compressor.CompressFiles(destination, sourceFiles);
           

            
            /*   if (fileStream != null)
                   fileStream.Lock(0, fileStream.Length);
           */
        }
       
        public static void UnBackUpFiles(string destination, string[] filenames)
        {
            SevenZipBase.SetLibraryPath(Path.Combine(
        Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
        "7z64.dll"));
            SevenZip.SevenZipExtractor extractor = new SevenZipExtractor(destination);
            extractor.ExtractArchive(Path.GetDirectoryName(Application.ExecutablePath));
            /*foreach (var file in filenames)
            {
                FileStream fs = new FileStream(file, FileMode.Create);
                extractor.ExtractFile(file, fs);
                fs.Flush();
                fs.Close();
            }*/
        }

        private static void Compressor_FileCompressionStarted(object sender, FileNameEventArgs e)
        {
            /*  if (fileStream != null)
                  fileStream.Unlock(0, fileStream.Length);*/
        }

        private static void Compressor_CompressionFinished(object sender, EventArgs e)
        {
            /*  if (fileStream == null)
              {
                  fileStream = new FileStream(destinationFile[0], FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
              }
              FileSecurity fsecurity = new FileSecurity(destinationFile[0], AccessControlSections.Owner);*/
            //MessageBox.Show("");
            //fsecurity.SetOwner()

        }

        private static void Compressor_Compressing(object sender, SevenZip.ProgressEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }

    // IPS/IDS controller class
    public static class WebroamIPS
    {
        public static void Disable()
        {
            /*   string name = "";
               foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
               {

                       foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                       {
                           if (ip.Address.ToString() == IP)
                           {
                           name = ni.Name;
                           break;
                           }
                       }

               }*/

            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\wrIPS\\";
            string content = File.ReadAllText(path + "wripsTemp.conf");
            content = content.Replace("{STATISENABLED}", "no");
            File.WriteAllText(path + "wrips.conf", content);
            //set interface name="Ethernet" admin=DISABLE
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("sc.exe", " config WRIPS start=disabled");//"netsh", "interface set interface name=\"" + name + "\" admin=DISABLE");
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            System.Diagnostics.Process p = new System.Diagnostics.Process();

            p.StartInfo = psi;
            p.Start();
            p.WaitForExit();
        }
        public static void Enable()
        {
            /*   string name = "";
               foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
               {

                       foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                       {
                           if (ip.Address.ToString() == IP)
                           {
                           name = ni.Name;
                           break;
                           }
                       }

               }*/

            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\wrIPS\\";
            string content = File.ReadAllText(path + "wrips.conf");
            content = content.Replace("{STATISENABLED}", "yes");
            File.WriteAllText(path + "wrips.conf", content);
            //set interface name="Ethernet" admin=DISABLE
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("sc.exe", " config WRIPS start=auto");//"netsh", "interface set interface name=\"" + name + "\" admin=DISABLE");
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            System.Diagnostics.Process p = new System.Diagnostics.Process();

            p.StartInfo = psi;
            p.Start();
            p.WaitForExit();
        }
        public static void ConfigIP(string ip)
        {


            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("sc.exe", " config \"WRIPS\" binpath= " + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\"\\wrIPS\\wrips.exe -c wrips.conf -i " + ip + "\"");//"netsh", "interface set interface name=\"" + name + "\" admin=DISABLE");
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            System.Diagnostics.Process p = new System.Diagnostics.Process();

            p.StartInfo = psi;
            p.Start();
            p.WaitForExit();
        }
        public static void InstallService()
        {
            IPAddress ip = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList.Where(i => i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault();


            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("sc.exe", " CREATE \"WRIPS\" binpath= \""+ System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\wrIPS\\wrips.exe -c wrips.conf -i "+ip+"\"");//"netsh", "interface set interface name=\"" + name + "\" admin=DISABLE");
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.WorkingDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\wrIPS\\";
            System.Diagnostics.Process p = new System.Diagnostics.Process();
           // MessageBox.Show(psi.Arguments);
            p.StartInfo = psi;
            p.Start();
            p.WaitForExit();
           
        }
        public static string GetIPSubnetMask(IPAddress address)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (address.Equals(unicastIPAddressInformation.Address))
                        {
                            var ip = unicastIPAddressInformation.Address.ToString().Split('.');
                            return String.Join(".", unicastIPAddressInformation.IPv4Mask.ToString().Split('.').Select((a, i)=>a=a.Replace("255", ip[i]))) + "/"+ String.Join(".", (unicastIPAddressInformation.IPv4Mask.ToString().Split('.').Select(x => Convert.ToString(Int32.Parse(x), 2).PadLeft(8, '0'))).ToArray()).Count(c => c == '1').ToString();
                        }
                    }
                }
            }

            return null;
            
        }

        public static void StartService()
        {
            string netad = GetIPSubnetMask(System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList.Where(i => i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault());
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\wrIPS";
           string content = File.ReadAllText(path + "\\wripsTemp.conf");
            content = content.Replace("{STATISENABLED}", "yes").Replace("{S_IPS_PATH}", path).Replace("{IPS_PATH}", path.Replace("\\", "\\\\")).Replace("{HOMENET}", netad);
            File.WriteAllText(path + "\\wrips.conf", content);
            ServiceController controller;
            if (!ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals("WRIPS")))
            {
           
                InstallService();
            }
            IPAddress ip = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList.Where(i => i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault();

            ConfigIP(ip.ToString());
            Enable();
                controller = new ServiceController("WRIPS");
                if (controller.Status == ServiceControllerStatus.Running)
                    return;

           

            if (controller.Status != ServiceControllerStatus.Running)
                    controller.Start();
                StartIPS();
            
        }

        public static void StopService()
        {
            ServiceController controller = new ServiceController("WRIPS");
            if (controller.Status == ServiceControllerStatus.Running)
                controller.Stop();
           
        }

       
        public static void StartIPS()
        {
           
            System.Diagnostics.ProcessStartInfo sinfo = new System.Diagnostics.ProcessStartInfo();
            sinfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //sinfo.Arguments = @"-i 1 -c "+ System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Snort\etc\snort.conf -A console";
            sinfo.Verb = "runas";
            sinfo.FileName = "sc.exe";
            sinfo.Arguments = "Start WRIPS";
            System.Diagnostics.Process.Start(sinfo).WaitForExit();
           

            /*System.Diagnostics.ProcessStartInfo sinfo2 = new System.Diagnostics.ProcessStartInfo();
            int i = 1; // set ID/Number of NIC ??????? USE ListDevs 
            sinfo2.FileName = @System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Snort\windump.exe"; //windump.exe -D lists
            //sinfo2.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            sinfo2.Arguments = @"-i " + i + " -q -w log/tcp_dump.log -n -C 30 -W 10 -U -s 0";
            sinfo2.Verb = "runas";


            System.Diagnostics.Process.Start(sinfo2).WaitForExit();*/
        }
    }
    //start of safebanking***************************

        //

   
       //DB (sql seerver ce 4.0) operations class
    public static class SqlReaderWriter
    {
        static string password = @"mywrpw1797";
        public static void ExecuteQuery(string insertorUpdq)
        {
            try
            {
                // MessageBox.Show(insertorUpdq);
                SqlCeConnection m_dbConnection = new SqlCeConnection(@"Data Source=" + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam.sdf;Password=" + password);
                //string sql2 = "insert into highscores (name, family, score) values ('Me', 'Asghari', 3000)";
                SqlCeCommand command2 = new SqlCeCommand(insertorUpdq, m_dbConnection);
                m_dbConnection.Open();
                command2.ExecuteNonQuery();
                m_dbConnection.Close();
            }
            catch(Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
        public static DataTable ReadQuery(string selectq)
        {
            try
            { 
            SqlCeConnection m_dbConnection = new SqlCeConnection("Data Source="+System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+"\\webroam.sdf;Password="+password);
            m_dbConnection.Open();
            SqlCeCommand command = new SqlCeCommand(selectq,m_dbConnection);
            command.CommandText = selectq;//"SELECT score, name, family FROM highscores";
                
            //DataSet DST = new DataSet();
            DataTable DT = new DataTable();
            SqlCeDataAdapter SDA = new SqlCeDataAdapter(command);

            SDA.Fill(DT);
            m_dbConnection.Close();
            return DT;
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
            return new DataTable();
        }
        public static object ExecuteScalar(string squery)
        {
            try
            { 
            object rt;
            SqlCeConnection m_dbConnection = new SqlCeConnection("Data Source=" + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+ "\\webroam.sdf;Password=" + password);
            //string sql2 = "insert into highscores (name, family, score) values ('Me', 'Asghari', 3000)";
            SqlCeCommand command2 = new SqlCeCommand(squery, m_dbConnection);
            SqlCeEngine en = new SqlCeEngine(m_dbConnection.ConnectionString);
            //en.CreateDatabase();
            //en.Upgrade();
            m_dbConnection.Open();
            rt = command2.ExecuteScalar();
            m_dbConnection.Close();
            return rt;
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
            return 0;
        }
        public static int MaxofRow(string tableName, string Where)
        {
            try
            { 
            int count = 0;       
            if(CountOfRow(tableName, Where) >0)
           count = Int32.Parse(ExecuteScalar("SELECT Max(ID) FROM " + tableName + " " + Where).ToString());
            count++;
            return count;
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
            return 0;
        }
        public static int MaxofRow(string tableName)
        {
            return MaxofRow(tableName, "");
        }

         public static int CountOfRow(string tableName, string Where)
        {
            int count = 0;          
           count = Int32.Parse(ExecuteScalar("SELECT COUNT(*) FROM " + tableName + " " + Where).ToString());
           
            return count;
        }
        public static int CountOfRow(string tableName)
        {
            return CountOfRow(tableName, "");
        }
    }
}

