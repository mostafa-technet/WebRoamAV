using NetFwTypeLib;
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
    public partial class ConfigPrgRules : Form
    {      
        public ConfigPrgRules()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        DataTable newdt;
        private void ConfigPrgRules_Load(object sender, EventArgs e)
        {
            try
            {

                if (SqlReaderWriter.CountOfRow("tblFirewalProt") > 0)
                {
                    string s = SqlReaderWriter.ExecuteScalar("SELECT rFLevel FROM tblFirewalProt WHERE rFLevel IS NOT NULL ORDER BY ID DESC")?.ToString();
                    if (!String.IsNullOrEmpty(s))
                    {
                        comboBox1.SelectedIndex = Int32.Parse(s);
                    }
                }
                else
                {
                    checkBox2.Checked = true;
                    /* var tb = SqlReaderWriter.ReadQuery("SELECT  rProgramP, rAccess FROM tblFirewalProt WHERE rProgramP IS NOT NULL");
                     newdt = tb.Clone();

                     newdt.Columns[1].DataType = typeof(string);

                     foreach (DataRow dr in tb.Rows)
                     {
                         newdt.Rows.Add(dr[0], dr[1].ToString().ToUpper() == "TRUE" ? "Allow" : "Block");
                     }
                     dataGridView1.DataSource = newdt;*/
                }
                /*
       if (SqlReaderWriter.ExecuteScalar("SELECT rAllowT FROM tblFirewalProt WHERE rAllowT IS NOT NULL") != null)
           checkBox2.Checked = SqlReaderWriter.ExecuteScalar("SELECT rAllowT FROM tblFirewalProt WHERE rAllowT IS NOT NULL").ToString().ToUpper() == "TRUE";
       */
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
         //   SqlReaderWriter.ExecuteQuery("DELETE FROM tblFirewalProt WHERE rFLevel IS NOT NULL");
           // SqlReaderWriter.ExecuteQuery("DELETE FROM tblFirewalProt WHERE rAllowT IS NOT NULL");
            if (checkBox2.Checked)
            {
                System.Diagnostics.ProcessStartInfo sinfo1 = new System.Diagnostics.ProcessStartInfo();
                sinfo1.FileName = "netsh.exe";
                sinfo1.RedirectStandardOutput = true;
                sinfo1.CreateNoWindow = true;
                sinfo1.UseShellExecute = false;
                sinfo1.Arguments = " advfirewall export " + @System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\fw-oldr.dat";
                System.Diagnostics.Process.Start(sinfo1);

                System.Diagnostics.ProcessStartInfo sinfo2 = new System.Diagnostics.ProcessStartInfo();
                sinfo2.FileName = "netsh.exe";
                sinfo2.RedirectStandardOutput = true;
                sinfo2.CreateNoWindow = true;
                sinfo2.UseShellExecute = false;
                sinfo2.Arguments = " advfirewall import "+ @System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\fw-rules.dat";
                System.Diagnostics.Process.Start(sinfo2).WaitForExit();
                int myid = SqlReaderWriter.MaxofRow("tblFirewalProt");
               /* if (SqlReaderWriter.CountOfRow("tblFirewalProt") == 0)
                    myid = 1;
                else
                    myid = Int32.Parse(SqlReaderWriter.ExecuteScalar("SELECT TOP(1) ID FROM tblFirewalProt ORDER BY ID DESC").ToString()) + 1;
                    */
                //string myquery = "INSERT INTO tblFirewalProt (ID, rAllowT) VALUES (" + myid + ",1)";
              //  SqlReaderWriter.ExecuteQuery(myquery);
            }
            else if(comboBox1.SelectedIndex>=0)
            {
                System.Diagnostics.ProcessStartInfo sinfo2 = new System.Diagnostics.ProcessStartInfo();
                sinfo2.FileName = "netsh.exe";
                sinfo2.RedirectStandardOutput = true;
                sinfo2.CreateNoWindow = true;
                sinfo2.UseShellExecute = false;
                sinfo2.Arguments = " advfirewall import " + @System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\fwLevels\\fw-"+comboBox1.Text+".dat";
                System.Diagnostics.Process.Start(sinfo2).WaitForExit();


                int myid = SqlReaderWriter.MaxofRow("tblFirewalProt");
              /*  if (SqlReaderWriter.CountOfRow("tblFirewalProt") == 0)
                    myid = 1;
                else
                    myid = Int32.Parse(SqlReaderWriter.ExecuteScalar("SELECT TOP(1) ID FROM tblFirewalProt ORDER BY ID DESC").ToString()) + 1;
                    */
               // string myquery = "INSERT INTO tblFirewalProt (ID, rFLevel) VALUES (" + myid+","+comboBox1.SelectedIndex+")";
               // SqlReaderWriter.ExecuteQuery(myquery);

            }



          //  SqlReaderWriter.ExecuteQuery("DELETE FROM tblFirewalProt WHERE rProgramP IS NULL");
            int id = SqlReaderWriter.MaxofRow("tblFirewalProt");
            /*if (SqlReaderWriter.CountOfRow("tblFirewalProt") == 0)
                id = 1;
            else
            id = Int32.Parse(SqlReaderWriter.ExecuteScalar("SELECT TOP(1) ID FROM tblFirewalProt ORDER BY ID DESC").ToString()) + 1;*/
            for (int j=0;j<dataGridView1.Rows.Count;j++)
            {
                if (dataGridView1.Rows[j].Cells[0].Value == null)
                    break;
                string query;
                if (checkBox2.Checked)
                {
                    query = "INSERT INTO tblFirewalProt (ID, rProgramP, rAccess) VALUES (" + id + ",'" + dataGridView1.Rows[j].Cells[0].Value + "','" + (dataGridView1.Rows[j].Cells[1].Value.ToString() == "Allow" ? "TRUE" : "FALSE") + "')";
                    SqlReaderWriter.ExecuteQuery("DELETE FROM tblFirewalProt WHERE rFLevel IS NULL");
                }
                else
                {
                    query = "INSERT INTO tblFirewalProt (ID, rProgramP, rAccess, rFLevel) VALUES (" + id + ",'" + dataGridView1.Rows[j].Cells[0].Value + "','" + (dataGridView1.Rows[j].Cells[1].Value.ToString() == "Allow" ? "TRUE" : "FALSE") + $"',{comboBox1.SelectedIndex})";
                    SqlReaderWriter.ExecuteQuery("DELETE FROM tblFirewalProt WHERE rFLevel IS NOT NULL");
                }
                Type typeFWPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");//Type.GetTypeFromCLSID(new Guid(guidFWPolicy2));
                Type typeFWRule = Type.GetTypeFromProgID("HNetCfg.FWRule");//Type.GetTypeFromCLSID(new Guid(guidRWRule));
                INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(typeFWPolicy2);
                INetFwRule newRule = (INetFwRule)Activator.CreateInstance(typeFWRule);
                newRule.Name = "webroam"+(j+1).ToString();
                newRule.Description = "rule by webroam";
                newRule.InterfaceTypes = "All";
                newRule.Enabled = true;
                newRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
                newRule.Action = dataGridView1.Rows[j].Cells[1].Value.ToString() == "Allow" ? NET_FW_ACTION_.NET_FW_ACTION_ALLOW : NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                newRule.ApplicationName = dataGridView1.Rows[j].Cells[0].Value.ToString();
                fwPolicy2.Rules.Add(newRule);
                SqlReaderWriter.ExecuteQuery(query);
                id++;
            }
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {     
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.exe|*.exe";
            if(ofd.ShowDialog() != DialogResult.Cancel)
            {
                newdt.Rows.Add(ofd.FileName, "Allow");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {                    
            newdt.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);          
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = !checkBox2.Checked;
            dataGridView1.DataSource = null;
            if (checkBox2.Checked)
            {
                var tb = SqlReaderWriter.ReadQuery("SELECT  rProgramP, rAccess FROM tblFirewalProt WHERE rFLevel IS NULL ORDER BY ID DESC");
                if (tb == null)
                    return;
                newdt = tb.Clone();

                newdt.Columns[1].DataType = typeof(string);

                foreach (DataRow dr in tb.Rows)
                {
                    newdt.Rows.Add(dr[0], dr[1]?.ToString().ToUpper() == "TRUE" ? "Allow" : "Block");
                }
                dataGridView1.DataSource = newdt;
            }
            else
            {
                comboBox1_SelectedIndexChanged(null, null);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0) return;
            var tb = SqlReaderWriter.ReadQuery("SELECT  rProgramP, rAccess FROM tblFirewalProt WHERE rFLevel="+comboBox1.SelectedIndex+ " ORDER BY ID DESC");
            if (tb == null)
                return;
            newdt = tb.Clone();

            newdt.Columns[1].DataType = typeof(string);

            foreach (DataRow dr in tb.Rows)
            {
                newdt.Rows.Add(dr[0], dr[1]?.ToString().ToUpper() == "TRUE" ? "Allow" : "Block");
            }
            dataGridView1.DataSource = newdt;
       
            if (SqlReaderWriter.ExecuteScalar("SELECT rAllowT FROM tblFirewalProt WHERE rFLevel="+comboBox1.SelectedIndex+ " ORDER BY ID DESC") != null)
                checkBox2.Checked = SqlReaderWriter.ExecuteScalar("SELECT rAllowT FROM tblFirewalProt WHERE rFLevel="+comboBox1.SelectedIndex).ToString().ToUpper() == "TRUE"+ " ORDER BY ID DESC";

        }
    }
}
