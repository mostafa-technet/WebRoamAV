using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO.Pipes;
using System.Diagnostics;
using System.Net.NetworkInformation;
using EnumerateFile;
using nClam;
using System.Windows.Threading;
using System.ServiceProcess;
using System.Data;

namespace WebRoamAV
{
    // class for native calls p/invoke. scanner/call to engine is in this file

        //Native API like loading DLLs dynamically
    public static class PInvoke
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)] 
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);



        //API to get list of connections 
        [DllImport("iphlpapi.dll")]
        public static extern int GetTcpTable(IntPtr pTcpTable, ref int pdwSize, bool bOrder);

        //API to change status of connection 
        [DllImport("iphlpapi.dll")]
        public static extern int SetTcpEntry(IntPtr pTcprow);

        //Convert 16-bit value from network to host byte order 
        [DllImport("wsock32.dll")]
        public static extern int ntohs(int netshort);

        //Convert 16-bit value back again 
        [DllImport("wsock32.dll")]
        public static extern int htons(int netshort);

        public static T load_function<T>(IntPtr dll, string functionname) where T : class
        {
            IntPtr address = GetProcAddress(dll, functionname);
            System.Delegate fn_ptr = Marshal.GetDelegateForFunctionPointer(address, typeof(T));
            return fn_ptr as T;
        }

    }

 //engine of webroam Antivirus
    public class ClamAVEngine
    {
        static ClamClient clam = null;
        static bool started = false;
        public void init()
        {
            try
            {
                if (!started)
                {
                    started = true;

                    ServiceController controller;
                    if (!ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals("ClamD")))
                    {
                        //    InstallService();
                        // MessageBox.Show(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\clamav-x64\\clamd.exe --install");
                        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\bin\\installer.bat");
                        psi.CreateNoWindow = true;
                        psi.WindowStyle = ProcessWindowStyle.Hidden;
                        psi.Verb = "runas";
                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        // MessageBox.Show(psi.Arguments);
                        p.StartInfo = psi;
                        p.Start();
                        p.WaitForExit();
                    }
                    controller = new ServiceController("ClamD");


                    if (controller.Status != ServiceControllerStatus.Running)
                    {
                        controller.Start();
                        controller.WaitForStatus(ServiceControllerStatus.Running);
                    }
                }

                /*
                ProcessStartInfo processStart = new ProcessStartInfo("sc.exe", "start clamd");
                processStart.CreateNoWindow = true;
                processStart.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(processStart).WaitForExit();*/
                clam = new ClamClient("localhost", 3310);
            }
            catch (Exception em)
            {            
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
        public ClamAVEngine()
        {
           init();
        }
        

       
        public TcpState GetState(TcpClient tcpClient)
        {
            var foo = IPGlobalProperties.GetIPGlobalProperties()
              .GetActiveTcpConnections()
              .SingleOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint));
            return foo != null ? foo.State : TcpState.Unknown;
        }
        IniFile inif = new IniFile(".\\webroam_conf.ini");
        IniFile inif2 = new IniFile(".\\AV_config.ini");
        [System.Runtime.InteropServices.DllImport("shlwapi.dll", EntryPoint = "PathIsDirectoryW", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PathIsDirectory([MarshalAs(UnmanagedType.LPTStr)] string pszPath);


        public IEnumerable<string> Scan(string[] path)
        {
            if (!started)
                init();
            //MessageBox.Show(path.Length.ToString());
            int iCount = 0;
            /* */
            //var clam = new ClamClient("localhost", 3310);
            string s1 = inif.Read("SCAN_ARCHIVE_FILES", "SCAN_SETTINGS").ToLower();
            string s2 = inif.Read("SCAN_FILE_TYPE", "SCAN_SETTINGS").ToLower();
            string s3 = inif.Read("ARCHIVE_TYPES", "SCAN_SETTINGS").ToLower();
            string s4 = inif2.Read("EXE", "WR_AV_CONF").ToLower();
            List<string> lpaths = new List<string>();
            for (int i = 0; i < path.Length; i++)
            {
                //  MessageBox.Show(path[i].PathF);
                /*  if (!path[i].PathF.Any(f => Path.GetInvalidFileNameChars().Contains(f)))
                  {*/

                bool b = true;


                //

                //

                try
                {
                    b = (path[i].Split('\\').Last()).Any(f => Path.GetInvalidFileNameChars().Contains(f));
                    if (b)
                    {
                        try
                        {
                            new Quarantine().QuaFile(path[i]);
                        }
                        catch
                        { }
                    }
                }
                catch (Exception ex)
                {
                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + ex.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                }
                DataTable dt;
                
                    dt = SqlReaderWriter.ReadQuery($"SELECT Subfolders FROM tblExcludeFF WHERE CHARINDEX(PATH, '{path[i]}')>0 AND ExclusionFor LIKE '%Behavior detection%';");

                if (dt.Rows.Count > 0)
                {
                    /*foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[0].ToString().ToLower() == "1" || dr[0].ToString().ToLower() == "true")
                        {
                            yield return "File\r\n";
                        }
                        else
                        {
                            if (SqlReaderWriter.CountOfRow("tblExcludeFF", $" WHERE Path='{path[i].PathF.Replace("'", "''")}'") > 0)
                            {
                                yield return "File\r\n";
                            }
                        }
                    }*/
                    yield return "File\r\n";
                    //  yield break;
                }
                else
                {








                    /* if (s2 == "false")
                     {
                         if (!s4.Contains(Path.GetExtension(path[i].PathF)) && (s1 == "false" || !s3.Contains(Path.GetExtension(path[i].PathF))))
                         {
                             yield return scanreport + Environment.NewLine;
                             continue;
                         }
                     }*/
                    // var clam = new ClamClient("localhost", 3310);
                    string strin = path[i];

                    DataTable dt2;
                    //  if (true)
                    //{
                    dt2 = SqlReaderWriter.ReadQuery($"SELECT * FROM tblExcludeFF WHERE PATH='{path[i]}' AND ExclusionFor LIKE '%Behavior detection%';");
                    if (dt2.Rows.Count > 0)
                    {

                        yield return "File\r\n";
                        //  yield break;

                    }
                    else
                    {
                        lpaths.Add(strin);
                    }
                    //}

                }
            }
            ClamScanResult[] scanResult = null;
            try
            {
                var t = clam.ScanFileOnServerMultithreadedAsync(lpaths.ToArray(), CancellationToken.None);
                t.Wait();
                if (t.IsCompleted)
                    scanResult = t.Result;
            }
            catch (Exception ex)
            {
                //  MessageBox.Show(strin);
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + ex.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
            if (scanResult != null)
            {
                for (int i = 0; i < lpaths.Count; i++)
                {
                    string scanreport = "";


                    scanreport = "";
                    //MessageBox.Show(strin);

                    if (scanResult[i].Result == ClamScanResults.VirusDetected)
                    {
                        //tasks.Add(Task.Factory.StartNew(() => {
                        try
                        {
                            new Quarantine().QuaFile(lpaths[i], scanResult[i].InfectedFiles?.First().VirusName);
                        }
                        catch
                        { }
                        //  }));
                        string virname = scanResult[i].InfectedFiles?.First().VirusName;
                        scanreport = "";
                        scanreport += (("\r\n"));
                        scanreport += (("File$"));
                        scanreport += (lpaths[i]);
                        scanreport += (("\r\n"));
                        scanreport += (("Folder$"));
                        scanreport += Path.GetDirectoryName(lpaths[i]);
                        scanreport += (("\r\n"));
                        scanreport += (("MType$"));
                        scanreport += (("Malware/Virus"));
                        scanreport += (("\r\n"));
                        scanreport += (("Malware$"));
                        scanreport += (virname);
                        scanreport += (("\r\n"));
                        scanreport += (("***\r\n"));
                    }
                    scanreport += "\r\nFinished\r\n";
                    iCount++;
                    //MessageBox.Show(scanreport);
                    TextReader sr = new StreamReader(new MemoryStream(ASCIIEncoding.ASCII.GetBytes(scanreport)));
                    string s = "";
                    do
                    {
                        s = sr.ReadLine();
                        // MessageBox.Show(s);
                        yield return s + Environment.NewLine;
                    } while (sr.Peek() != -1);




                    //MessageBox.Show("7");
                    //Task.WaitAll(tasks.ToArray());
                    //yield return "File\r\n";
                  //  yield break;


                }
            }
        }

        //SELECT * FROM tblExcludeFF WHERE CHARINDEX(PATH, 'C:\Users\Mostafa\Desktop\.vs')>0 AND ExclusionFor LIKE '%Known virus detection%';
        public IEnumerable<string> Scan(GRowsCUS[] path, int priority, bool OnlinePr = false)
        {
            if(!started)
            init();
            //MessageBox.Show(path.Length.ToString());
            int iCount = 0;
            /* */
            //var clam = new ClamClient("localhost", 3310);
            string s1 = inif.Read("SCAN_ARCHIVE_FILES", "SCAN_SETTINGS").ToLower();
            string s2 = inif.Read("SCAN_FILE_TYPE", "SCAN_SETTINGS").ToLower();
            string s3 = inif.Read("ARCHIVE_TYPES", "SCAN_SETTINGS").ToLower();
            string s4 = inif2.Read("EXE", "WR_AV_CONF").ToLower();
            // List<Task> tasks=new List<Task>();
            for (int i = 0; i < path.Length; i++)
            {
                //  MessageBox.Show(path[i].PathF);
                /*  if (!path[i].PathF.Any(f => Path.GetInvalidFileNameChars().Contains(f)))
                  {*/

                bool b = true;

                string scanreport = "";
                //

                //


                b = (path[i].PathF.Split('\\').Last()).Any(f => Path.GetInvalidFileNameChars().Contains(f));
                    if (b)
                    {
                        try
                        {
                            new Quarantine().QuaFile(path[i].PathF);
                        }
                        catch
                        { }
                    scanreport = (("File$"));
                    //scanreport += (strin);
                    scanreport += (("\r\n"));
                    scanreport += (("Folder$"));
                    scanreport += (path[i]);
                    scanreport += (("\r\n"));
                    scanreport += (("MType$"));
                    scanreport += (("Malware/Virus"));
                    scanreport += (("\r\n"));
                    scanreport += (("Malware$"));
                    //  scanreport += (virname);
                    scanreport += (("\r\n"));
                    scanreport += (("***\r\n"));


                    TextReader sr = new StreamReader(new MemoryStream(ASCIIEncoding.ASCII.GetBytes(scanreport)));
                    string s = "";
                    do
                    {
                        s = sr.ReadLine();
                        // MessageBox.Show(s);
                        yield return s + Environment.NewLine;
                    } while (sr.Peek() != -1);
                    continue;
                }
               
                DataTable dt;
                if (false)
                {
                    dt = SqlReaderWriter.ReadQuery($"SELECT Subfolders FROM tblExcludeFF WHERE CHARINDEX(PATH, '{path[i].PathF.Replace("'", "''")}')>0 AND ExclusionFor LIKE '%Behavior detection%';");

                    if (dt.Rows.Count > 0)
                    {
                        /*foreach (DataRow dr in dt.Rows)
                        {
                            if (dr[0].ToString().ToLower() == "1" || dr[0].ToString().ToLower() == "true")
                            {
                                yield return "File\r\n";
                            }
                            else
                            {
                                if (SqlReaderWriter.CountOfRow("tblExcludeFF", $" WHERE Path='{path[i].PathF.Replace("'", "''")}'") > 0)
                                {
                                    yield return "File\r\n";
                                }
                            }
                        }*/
                       // yield return "File$\r\n";
                        yield break;
                    }
                }

                else
                {
                    dt = SqlReaderWriter.ReadQuery($"SELECT Subfolders FROM tblExcludeFF WHERE CHARINDEX(PATH, '{path[i].PathF.Replace("'", "''").Replace("'", "''")}')>0 AND ExclusionFor LIKE '%Known virus detection%';");
                    if (dt.Rows.Count > 0)
                    {
                        /*foreach (DataRow dr in dt.Rows)
                        {
                            if (dr[0].ToString().ToLower() == "1" || dr[0].ToString().ToLower() == "true")
                            {
                                yield return "File\r\n";
                            }
                            else
                            {
                                if (SqlReaderWriter.CountOfRow("tblExcludeFF", $" WHERE Path='{path[i].PathF.Replace("'", "''")}'") > 0)
                                {
                                    yield return "File\r\n";
                                }
                            }
                        }*/
                       // yield return "File$\r\n";
                    }
                }



               
                if (!b && PathIsDirectory(path[i].PathF))
                {


                    string strPath = path[i].PathF;
                    //    MessageBox.Show(strPath);

                    FileDirectoryEnumerable myEnum = new FileDirectoryEnumerable();
                    myEnum.SearchPath = strPath;
                    myEnum.SearchPattern = "*.*";
                    myEnum.ReturnStringType = true;
                    myEnum.SearchForFile = true;
                    myEnum.SearchForDirectory = false;
                    myEnum.ReturnStringType = true;

                    // System.Console.WriteLine("----------- search ALL file name ------------------");

                    List<string> filestoScan = new List<string>();
                  
                    foreach (string strFileName in myEnum.AsParallel())
                    {
                        if (!(s2 != "all" && strFileName.Contains(".") && s4.Contains(strFileName.Substring(strFileName.LastIndexOf('.')).ToLower())) || s2 == "all")
                        {
                            yield return "File\r\n";
                            continue;
                        }
                        scanreport = "";
                        if (strFileName.Any(f => Path.GetInvalidFileNameChars().Contains(f)))
                        {
                            scanreport = "";
                            scanreport += (("\r\n"));
                            scanreport += (("File$"));
                            //scanreport += (strin);
                            scanreport += (("\r\n"));
                            scanreport += (("Folder$"));
                            scanreport += (strPath);
                            scanreport += (("\r\n"));
                            scanreport += (("MType$"));
                            scanreport += (("Malware/Virus"));
                            scanreport += (("\r\n"));
                            scanreport += (("Malware$"));
                            //  scanreport += (virname);
                            scanreport += (("\r\n"));
                            scanreport += (("***\r\n"));


                            TextReader sr = new StreamReader(new MemoryStream(ASCIIEncoding.ASCII.GetBytes(scanreport)));
                            string s = "";
                            do
                            {
                                s = sr.ReadLine();
                                // MessageBox.Show(s);
                                yield return s;// + Environment.NewLine;
                            } while (sr.Peek() != -1);
                            continue;
                        }

                        var strin1 = strPath + "\\" + strFileName;

                        filestoScan.Add(strin1);
                    }
                    
                    /*       if(path[i].SubF=="No")
                           {
                               if(Path.GetDirectoryName(strFileName).ToLower()!= Path.GetDirectoryName(path[i].PathF).ToLower())
                               {
                                   break;
                               }
                           }*/

                    /*    if (s2 == "false")
                        {
                            if (!s4.Contains(strFileName.Split('.').Last()) && (s1 == "false" || !s3.Contains(strFileName.Split('.').Last())))
                            {
                                yield return scanreport + Environment.NewLine;
                                continue;
                            }
                        }*/
                    // var clam = new ClamClient("localhost", 3310);

                    //  var strin = strPath + "\\" + strFileName;

                    // if(strFileName.Contains(".")) 
                    //MessageBox.Show(strFileName);
                    ClamScanResult[] scanResult = null;

                    //MessageBox.Show(strin);
                    try
                    {
                        Task<ClamScanResult[]> myr = clam.ScanFileOnServerMultithreadedAsync(filestoScan.ToArray(), CancellationToken.None);
                        myr.Wait();
                        //if(myr.IsCompleted)
                        scanResult = myr.Result;
                    }
                    catch(Exception em)
                    {
                        ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                    }
                    int c = 0;
                    if (scanResult != null)
                    {
                        foreach (var strin in filestoScan)
                        {
                            if (scanResult[c].Result != ClamScanResults.VirusDetected)
                            {
                                yield return "File$\r\n";
                                continue;
                            }
                            scanreport = "";
                            scanreport += (("\r\n"));
                            scanreport += (("File$"));
                            scanreport += (strin);
                            scanreport += (("\r\n"));
                            scanreport += (("Folder$"));
                            scanreport += (strPath);
                            scanreport += (("\r\n"));
                            scanreport += (("MType$"));
                            scanreport += (("Malware/Virus"));
                            scanreport += (("\r\n"));
                            scanreport += (("Malware$"));
                            scanreport += (scanResult[c].InfectedFiles?.First().VirusName);
                            scanreport += (("\r\n"));
                            scanreport += (("***\r\n"));
                            // c++;

                            //  MessageBox.Show("");

                            // MessageBox.Show("/");
                           

                            TextReader sr = new StreamReader(new MemoryStream(ASCIIEncoding.ASCII.GetBytes(scanreport)));
                            string s = "";
                            do
                            {
                                s = sr.ReadLine();
                                // MessageBox.Show(s);
                                yield return s + Environment.NewLine;                                
                            } while (sr.Peek() != -1);

                            c++;

                        }
                    }
                }
                else
                {
                    scanreport = "";

                    /* if (s2 == "false")
                     {
                         if (!s4.Contains(Path.GetExtension(path[i].PathF)) && (s1 == "false" || !s3.Contains(Path.GetExtension(path[i].PathF))))
                         {
                             yield return scanreport + Environment.NewLine;
                             continue;
                         }
                     }*/
                    // var clam = new ClamClient("localhost", 3310);
                    string strin = path[i].PathF;
                    DataTable dt2;
                    if (false)
                    {
                        dt2 = SqlReaderWriter.ReadQuery($"SELECT * FROM tblExcludeFF WHERE PATH='{path[i].PathF.Replace("'", "''")}' AND ExclusionFor LIKE '%Behavior detection%';");
                        if (dt2.Rows.Count > 0)
                        {

                            //yield return "File$\r\n";
                            yield break;

                        }
                    }
                    else
                    {
                        dt2 = SqlReaderWriter.ReadQuery($"SELECT * FROM tblExcludeFF WHERE PATH='{path[i].PathF.Replace("'", "''")}' AND ExclusionFor LIKE '%Known virus detection%';");
                        if (dt2.Rows.Count > 0)
                        {

                           // yield return "File$\r\n";
                        }



                        //MessageBox.Show(strin);
                        ClamScanResult scanResult2 = new ClamScanResult("Clean");
                        try
                        {
                            var t = clam.ScanFileOnServerMultithreadedAsync(strin);
                            t.Wait();
                            if (t.IsCompleted)
                                scanResult2 = t.Result;
                        }
                        catch (Exception ex)
                        {
                            //  MessageBox.Show(strin);
                            ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + ex.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                        }
                        if (scanResult2.Result == ClamScanResults.VirusDetected)
                        {
                            //tasks.Add(Task.Factory.StartNew(() => {
                            try
                            {
                                new Quarantine().QuaFile(strin, scanResult2.InfectedFiles?.First().VirusName);
                            }
                            catch
                            { }
                            //  }));
                            string virname = scanResult2.InfectedFiles?.First().VirusName;
                            scanreport = "";
                            scanreport += (("\r\n"));
                            scanreport += (("File$"));
                            scanreport += (strin);
                            scanreport += (("\r\n"));
                            scanreport += (("Folder$"));
                            scanreport += (path[i].PathF);
                            scanreport += (("\r\n"));
                            scanreport += (("MType$"));
                            scanreport += (("Malware/Virus"));
                            scanreport += (("\r\n"));
                            scanreport += (("Malware$"));
                            scanreport += (virname);
                            scanreport += (("\r\n"));
                            scanreport += (("***\r\n"));
                        }
                       // scanreport += "\r\nFinished\r\n";
                        iCount++;
                        //MessageBox.Show(scanreport);
                        TextReader sr = new StreamReader(new MemoryStream(ASCIIEncoding.ASCII.GetBytes(scanreport)));
                        string s = "";
                        do
                        {
                            s = sr.ReadLine();
                            // MessageBox.Show(s);
                            yield return s + Environment.NewLine;
                        } while (sr.Peek() != -1);


                    }
                }
            }
            //MessageBox.Show("7");
            //Task.WaitAll(tasks.ToArray());
            //yield return "File$\r\n";
            yield break;
        

            }
        
    }

   
}
