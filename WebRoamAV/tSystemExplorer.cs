using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class tSystemExplorer : Form
    {
        public tSystemExplorer()
        {
            InitializeComponent();  
            comboBox1.SelectedIndex = 0;
        }
        class PrcType
        {
            public Process Process { get; set; }    
            public string Path { get; set; }

            public string CommandLine { get; set; }
        }

        IEnumerable<PrcType> query = null;
        ManagementObjectSearcher searcher;
        ManagementObjectCollection results;
        List<string> strt_paths = new List<string>();
        ManagementClass mangnmt;
        ManagementObjectCollection mcol;
        private void tSystemExplorer_Load(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                listView1.Scrollable = true;
                listView1.View = View.Details;
                listView1.Items.Clear();
                imageList1.Images.Clear();
                ColumnHeader header1 = new ColumnHeader();
                header1.Text = "";
                header1.Name = "col1";
                header1.Width = 120;
                listView1.Columns.Add(header1);
                if (query != null)
                {
                    searcher.Dispose();
                    results.Dispose();
                }
                query = null;
                var wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
                /*using (*/
                searcher = new ManagementObjectSearcher(wmiQueryString);//)
                /*using (*/
                results = searcher.Get();//)
                                         //{
                query = from p in Process.GetProcesses()
                        join mo in results.Cast<ManagementObject>()
                            on p.Id equals (int)(uint)mo["ProcessId"]
                        select new PrcType
                        {
                            Process = p,
                            Path = (string)mo["ExecutablePath"],
                            CommandLine = (string)mo["CommandLine"],
                        };
                int i = 0;

                foreach (var item in query)
                {
                    // Do what you want with the Process, Path, and CommandLine
                    if (item.Path != "" && File.Exists(item.Path))
                    {

                        var icon = Icon.ExtractAssociatedIcon(item.Path);
                        imageList1.Images.Add(icon);
                        listView1.Items.Add(item.Process.ProcessName, i);
                        i++;
                    }
                }
                //}




                listView1.SmallImageList = imageList1;
             
            }
            else if(comboBox1.SelectedIndex == 1)
            {
                int i = 0;
                mangnmt =
 new ManagementClass("Win32_StartupCommand");

                mcol = mangnmt.GetInstances();                
                listView1.Scrollable = true;
                listView1.View = View.Details;
                listView1.Items.Clear();
                imageList1.Images.Clear();
                var header1 = new ColumnHeader();
                header1.Text = "";
                header1.Name = "col1";
                header1.Width = 120;
                listView1.Columns.Add(header1);
                try
                {
                    foreach (ManagementObject strt in mcol)
                    {
                        int len = strt["Command"].ToString().IndexOf('"', 1);
                        var path = strt["Command"].ToString().Substring(len < 0 ? 0 : 1, len < 0 ? strt["Command"].ToString().IndexOf(".exe") + 4 : len - 1);
                        var icon = Icon.ExtractAssociatedIcon(path);
                        imageList1.Images.Add(icon);
                        listView1.Items.Add(strt["Name"].ToString(), i);
                        i++;
                        strt_paths.Add(strt["Command"].ToString());
                    }
                }
                catch
                { }
                listView1.SmallImageList = imageList1;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                try
                {
                    label5.Text = ((PrcType)query.ElementAt(listView1.SelectedIndices[0])).Path;

                    button1.Enabled = !((PrcType)query.ElementAt(listView1.SelectedIndices[0])).Process.MainModule.FileName.Contains(Environment.GetFolderPath(Environment.SpecialFolder.System));


                }
                catch { }
            }
            else if(comboBox1.SelectedIndex == 1)
            {
                try
                {
                    if (strt_paths.Count > 0)
                        label5.Text = strt_paths[listView1.SelectedIndices[0]];
                }
                catch { }
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tSystemExplorer_Load(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {                
                ((PrcType)query.ElementAt(listView1.SelectedIndices[0])).Process.Kill();
            }
            catch
            { }
            tSystemExplorer_Load(null, null);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tSystemExplorer_Load(null, null);
        }
    }
}
