using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics;
using System.Management;
using System.Management.Automation;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Threading;
using NetFwTypeLib;

namespace WebRoamAV
{
   
    public partial class AdvanceSettings : Form
    {

        // Returns the list of Network Interfaces installed
        internal class GRules
        {
            public bool Checked;
            public string ExceptionName;
            public string Protocol;
            public string Action;
            public string NetworkProfile;
        }
        public AdvanceSettings()
        {
            InitializeComponent();
        }

        Dictionary<string, string> confs;
        private void AdvanceSettings_Load(object sender, EventArgs e)
        {
            /*NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
            var sel = from a in nis where a.OperationalStatus == OperationalStatus.Up && a.NetworkInterfaceType == NetworkInterfaceType.Ethernet select a;
            List<UnicastIPAddressInformation> l = new List<UnicastIPAddressInformation>(); 
            foreach(var nic in sel)
            {
                l.AddRange(nic.GetIPProperties().UnicastAddresses);
            }            
            var ips = from ifs in l where ifs.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork select ifs;

           

             foreach(var n in GetNicNames())
             MessageBox.Show(n);*/
           // dataGridView3.ReadOnly = true;
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.MultiSelect = false;
            timer1.Start();
            List<List<string>> rules = FWCtrl.ListFirewallRules();
            dataGridView3.Columns[1].Name = "ExceptionName";
            foreach (var r in rules)
            {
                dataGridView3.Rows.Add(new object[] { false, r.ElementAt(0), r.ElementAt(1), r.ElementAt(2), r.ElementAt(3) });                
            }
            var ta3 = Task.Factory.StartNew(delegate
            {
                Process p3 = new System.Diagnostics.Process();
                ProcessStartInfo psi3 = new ProcessStartInfo(@"Powershell.exe");
                psi3.WindowStyle = ProcessWindowStyle.Hidden;
                psi3.CreateNoWindow = true;
                p3.StartInfo = psi3;
                psi3.Arguments = " -WindowStyle Hidden Get-NetFirewallProfile";
                psi3.RedirectStandardOutput = true;
                psi3.UseShellExecute = false;
                p3.Start();
                string output3 = "";


                output3 = p3.StandardOutput.ReadToEnd();
                string[] o1r = output3.Split(new char[] { ':', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < o1r.Length; i++)
                {
                    o1r[i] = o1r[i].Trim();
                }
                var temp = new List<string>(o1r);
                temp.RemoveAll((t) => t.Length == 0);
                o1r = temp.ToArray();
                confs = new Dictionary<string, string>();
                for (int ij = 0; ij < o1r.Length; ij += 2)
                {
                    confs.Add(o1r[ij] + ij.ToString(), o1r[ij + 1]);
                }
                var profiles = from a in confs where a.Key.StartsWith("Name") select a;
                var stealth = from a in confs where a.Key.StartsWith("EnableStealthModeForIPsec") select a;
                Process p12 = new System.Diagnostics.Process();
                ProcessStartInfo psi12 = new ProcessStartInfo(@"Powershell.exe");
                psi12.WindowStyle = ProcessWindowStyle.Hidden;
                psi12.CreateNoWindow = true;
                p12.StartInfo = psi12;
                psi12.Arguments = "  -WindowStyle Hidden Get-NetFirewallRule -DisplayGroup 'File and Printer Sharing' |Where Enabled -eq True| select Profile";
                psi12.RedirectStandardOutput = true;
                psi12.UseShellExecute = false;
                p12.Start();
                string cprofile = p12.StandardOutput.ReadToEnd();
                for (int cc = 0; cc < profiles.Count(); cc++)
                {
                    //MessageBox.Show(stealth.ElementAt(cc).Value);
                    dataGridView1.Invoke((MethodInvoker)delegate { ((DataGridViewComboBoxColumn)dataGridView1.Columns[3]).Items.Add(profiles.ElementAt(cc).Value); });
                    dataGridView2.Invoke((MethodInvoker)delegate { dataGridView2.Rows.Add(new object[] { profiles.ElementAt(cc).Value, stealth.ElementAt(cc).Value == "True" ? "ON" : "OFF", (cprofile.Contains(profiles.ElementAt(cc).Value)?"ON":"OFF") }); });
                }
            });
                    Process p2 = new System.Diagnostics.Process();
            ProcessStartInfo psi2 = new ProcessStartInfo(@"Powershell.exe");
            psi2.WindowStyle = ProcessWindowStyle.Hidden;
            psi2.CreateNoWindow = true;
            p2.StartInfo = psi2;
            psi2.Arguments = " -WindowStyle Hidden Get-NetIPAddress -AddressFamily IPv4";
            psi2.RedirectStandardOutput = true;
            psi2.UseShellExecute = false;
            p2.Start();
           confs = new Dictionary<string, string>();
            string output1 = "";

            Task ta = Task.Factory.StartNew(() => {
                {
                    ta3.Wait();
                    output1 = p2.StandardOutput.ReadToEnd();

                    string[] to1r = output1.Split(new char[] { ':', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < to1r.Length; i++)
                    {
                        to1r[i] = to1r[i].Trim();
                    }
                    var ttemp = new List<string>(to1r);
                    ttemp.RemoveAll((t) => t.Length == 0);
                    to1r = ttemp.ToArray();
                    confs = new Dictionary<string, string>();
                    for (int ij = 0; ij < to1r.Length; ij += 2)
                    {
                        confs.Add(to1r[ij] + ij.ToString(), to1r[ij + 1]);
                    }
                    var ips = from a in confs where a.Key.StartsWith("IPAddress") select a;
                    var eths = from a in confs where a.Key.StartsWith("InterfaceAlias") select a;
                }
            });
            Process p = new System.Diagnostics.Process();
            
            string output = "";
            int l = 0;
            ProcessStartInfo psi = new ProcessStartInfo(@"Powershell.exe");
            Task ta0 = Task.Factory.StartNew(() =>
            {
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.CreateNoWindow = true;
                p.StartInfo = psi;
                psi.Arguments = " -WindowStyle Hidden Get-NetconnectionProfile";
                psi.RedirectStandardOutput = true;
                psi.UseShellExecute = false;
                p.Start();
                output = p.StandardOutput.ReadToEnd();

                l = Regex.Matches(output, "InterfaceIndex").Count;
            });
            ta0.ContinueWith((g) => completed(l, output, ta));
        }
        void completed(int l, string output, Task tsk)
        {
            int it = 0, it2 = 0;
            MemoryStream mst = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(output));
            StreamReader ms = new StreamReader(mst);
            for (int j = 0; j < l; j++)
            {
                it = output.IndexOf("InterfaceAlias", it) + "InterfaceAlias ".Length;
                ms.BaseStream.Seek(it, SeekOrigin.Begin);
                string rs = ms.ReadLine();
                ms.DiscardBufferedData();
                rs = rs.Replace(":", "").Trim();
                it2 = output.IndexOf("NetworkCategory", it2) + "NetworkCategory".Length;
                ms.BaseStream.Seek(it2, SeekOrigin.Begin);
                string rs2 = ms.ReadLine();
                ms.DiscardBufferedData();
                rs2 = rs2.Replace(":", "").Trim();
                while (!tsk.IsCompleted)
                    Thread.Sleep(100);
                string ip = confs.ElementAt(confs.ToList().IndexOf((new KeyValuePair<string, string>(confs.FirstOrDefault((kv) => kv.Value == rs).Key, rs))) - 2).Value;
                this.dataGridView1.Invoke((MethodInvoker)delegate 
                {
                    this.dataGridView1.Rows.Add(new string[] { rs, ip, "Connected", rs2 });
                 }
                );
                pictureBox1.Invoke((MethodInvoker)delegate
                {
                    this.pictureBox1.Visible = false;
                    this.pictureBox1.Enabled = false;
                });
                
            }
    
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.pictureBox1.Enabled)
                this.pictureBox1.Visible = true;
            timer1.Stop();
            timer1.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Visible = true;
            this.pictureBox1.Enabled = true;
            Task.Factory.StartNew(() =>
            {
                for (int index = 0; index < dataGridView2.Rows.Count; index++)
                {
                    DataGridViewRow v0 = dataGridView2.Rows[index];
                    if (v0.Cells.Count == 3 && v0.Cells[0].Value != null && v0.Cells[1].Value != null && v0.Cells[2].Value != null)
                    {
                        //   Process p = new System.Diagnostics.Process();
                        //string output = "";
                        //  int l = 0;

                        string args = "Set-NetFirewallProfile -Name " + v0.Cells[0].Value + " " + (v0.Cells[1].Value.ToString() == "ON" ? " -EnableStealthModeForIPsec True" : " -EnableStealthModeForIPsec False");
                        var powershell = PowerShell.Create().AddScript(args);
                        powershell.Invoke();
                        powershell.Dispose();
                        //var p2 = new System.Diagnostics.Process();
                        //string output2 = "";
                        // int l2 = 0;


                    }
                }
                var v = dataGridView2.Rows;
                string enbl = "";
                if (v[0].Cells[2].Value.ToString() == "ON")
                {
                    enbl += v[0].Cells[0].Value.ToString();
                }
                if (v[1].Cells[2].Value.ToString() == "ON")
                {
                    if (enbl != "")
                        enbl += ", ";
                    enbl += v[1].Cells[0].Value.ToString();
                }
                if (v[2].Cells[2].Value.ToString() == "ON")
                {
                    if (enbl != "")
                        enbl += ", ";
                    enbl += v[2].Cells[0].Value.ToString();
                }
                string args02 = "Set-NetFirewallRule -DisplayGroup 'File And Printer Sharing' -Enabled False";
                var powershell02 = PowerShell.Create().AddScript(args02);
                powershell02.Invoke();

                string args2 = "Set-NetFirewallRule -DisplayGroup 'File And Printer Sharing' -Enabled True -Profile " + enbl;
                // Clipboard.SetText(psi2.FileName + " " + psi2.Arguments);
                var powershell2 = PowerShell.Create().AddScript(args2);
                powershell2.Invoke();

                powershell2.Dispose();

            });/*.ContinueWith((t) =>
            {*/
                this.Close();
            //});
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to restore firewall rules to default?", "Webroam", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                ProcessStartInfo psi = new ProcessStartInfo(@"netsh.exe");
                Process p = new Process();
                SqlReaderWriter.ExecuteQuery("DELETE FROM tblFirewalProt WHERE rFLevel IS NOT NULL");
                Task ta0 = Task.Factory.StartNew(() =>
                {
                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                    psi.CreateNoWindow = true;
                    p.StartInfo = psi;
                    psi.Arguments = " advfirewall import " + Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().FullName) + "\\fwLevels\\fw-medium.dat";
                    psi.RedirectStandardOutput = true;
                    psi.UseShellExecute = false;
                    p.Start();
                    int myid = SqlReaderWriter.MaxofRow("tblFirewalProt");
                   /* if (SqlReaderWriter.CountOfRow("tblFirewalProt") == 0)
                        myid = 1;
                    else
                        myid = Int32.Parse(SqlReaderWriter.ExecuteScalar("SELECT TOP(1) ID FROM tblFirewalProt ORDER BY ID DESC").ToString()) + 1;
                        */
                    string myquery = "INSERT INTO tblFirewalProt (ID, rFLevel) VALUES (" + myid + ", 2)";
                    SqlReaderWriter.ExecuteQuery(myquery);
                    SqlReaderWriter.ExecuteQuery("DELETE FROM tblFirewalProt WHERE rProgramP IS NOT NULL");
                });

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int c = dataGridView3.Rows.Count, i = 0;
            int index = 1;
            while (i < c)
            {                
                if (dataGridView3.Rows[i]!= null && dataGridView3.Rows[i].Cells[0].Value!=null&& dataGridView3.Rows[i].Cells[0].Value.ToString().ToLower() == "true")
                {
                    FWCtrl.RemoveFirewallRules(dataGridView3.Rows[i].Cells[1].Value.ToString(), index);
                    index = 1;

                    while (i - index > 0 && dataGridView3.Rows[i - index].Cells[0].Value.ToString() == dataGridView3.Rows[i].Cells[0].Value.ToString())
                            index++;
                         dataGridView3.Rows.RemoveAt(i);

                      
                        i = 0;
                        c--;
                    
                }
                else
                i++;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int i = dataGridView3.SelectedCells[0].RowIndex;
            int scrollPosition = dataGridView3.FirstDisplayedScrollingRowIndex;
            FWCtrl.MoveUpFirewallRule(i+1);
            dataGridView3.ReadOnly = false;
            dataGridView3.Rows.Clear();
            timer1.Start();
            List<List<string>> rules = FWCtrl.ListFirewallRules();
            dataGridView3.Columns[1].Name = "ExceptionName";
            foreach (var r in rules)
            {
                dataGridView3.Rows.Add(new object[] { false, r.ElementAt(0), r.ElementAt(1), r.ElementAt(2), r.ElementAt(3) });
            }
            dataGridView3.ReadOnly = true;            
            dataGridView3.Rows[i - 1].Selected = true;
            if (i - 1 < scrollPosition)
                dataGridView3.FirstDisplayedScrollingRowIndex = i - 1;
            else
            dataGridView3.FirstDisplayedScrollingRowIndex = scrollPosition;


        }

        private void button7_Click(object sender, EventArgs e)
        {
            int i = dataGridView3.SelectedCells[0].RowIndex;
            int scrollPosition = dataGridView3.FirstDisplayedScrollingRowIndex;
            FWCtrl.MoveUpFirewallRule(i + 2);
            dataGridView3.ReadOnly = false;
            dataGridView3.Rows.Clear();
            timer1.Start();
            List<List<string>> rules = FWCtrl.ListFirewallRules();
            dataGridView3.Columns[1].Name = "ExceptionName";
            foreach (var r in rules)
            {
                dataGridView3.Rows.Add(new object[] { false, r.ElementAt(0), r.ElementAt(1), r.ElementAt(2), r.ElementAt(3) });
            }
            dataGridView3.ReadOnly = true;
            dataGridView3.Rows[i + 1].Selected = true;
            if (i - 1 < scrollPosition)
                dataGridView3.FirstDisplayedScrollingRowIndex = i + 1;
            else
                dataGridView3.FirstDisplayedScrollingRowIndex = scrollPosition;
        }
        public static string[] strRule = new string[7];
        private void button8_Click(object sender, EventArgs e)
        {
            for(int i=0;i<strRule.Length;i++)
            strRule[i] = "";
            new fAddEditExceptions().ShowDialog();
            
        }

        private void dataGridView3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            INetFwRule r = firewallPolicy.Rules.Item(dataGridView3.Rows[dataGridView3.SelectedCells[0].RowIndex].Cells[1].Value.ToString());

            AdvanceSettings.strRule[0] = r.Name+";"+(r.Direction == NetFwTypeLib.NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN?"In":"Out")+";"
                +((int)NetFwTypeLib.NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP==r.Protocol?"TCP": ((int)NetFwTypeLib.NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP == r.Protocol?"UDP": (r.Protocol== (int)NetFwTypeLib.NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_ANY ?"Any":"ICMP")));
            
            AdvanceSettings.strRule[1] = r.LocalAddresses==""||r.LocalAddresses=="*"? "Any IP Address" : (r.LocalAddresses.IndexOf("-") == -1 ? "IP Address" : "IP Address Range")+";"+(r.LocalAddresses=="*"?"":r.LocalAddresses);

            AdvanceSettings.strRule[2] = (String.IsNullOrWhiteSpace(r.LocalPorts) || r.LocalPorts == "*") ? "All Ports" : (r.LocalPorts.IndexOf("-")==-1? "Specific Port(s)" : "Port Range")+";"+(r.LocalPorts=="*"?"":r.LocalPorts);

            AdvanceSettings.strRule[3] = (r.RemoteAddresses == "" || r.RemoteAddresses == "*") ? "Any IP Address" : (r.RemoteAddresses.IndexOf("-") == -1 ? "IP Address" : "IP Address Range") + ";" + (r.RemoteAddresses == "*" ? "" : r.RemoteAddresses);

            AdvanceSettings.strRule[4] = (String.IsNullOrWhiteSpace(r.RemotePorts) || r.RemotePorts == "*") ? "All Ports" : (r.RemotePorts.IndexOf("-") == -1 ? "Specific Port(s)" : "Port Range") + ";" + (r.RemotePorts == "*" ? "" : r.RemotePorts);

            AdvanceSettings.strRule[5] = (r.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW ? "Allow" : "Block") + ";" + (((r.Profiles & (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC) != 0 ? "Public" : "") + "," + ((r.Profiles & (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE) != 0 ? "Private" : "") + "," + ((r.Profiles & (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_DOMAIN) != 0 ? "Domain" : "") + ",");


            new fAddEditExceptions(true).ShowDialog();
        }
    }
}
