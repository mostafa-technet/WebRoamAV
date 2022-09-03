using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class fAddEditExceptions6 : Form
    {
        public fAddEditExceptions6()
        {
            InitializeComponent();
        }

        public fAddEditExceptions6(bool Isreadonly)
        {
            InitializeComponent();
            panel1.Enabled = !Isreadonly;
            checkBox1.Enabled = checkBox2.Enabled = checkBox3.Enabled = !Isreadonly;
        }

        private void fAddEditExceptions6_Load(object sender, EventArgs e)
        {
            if (AdvanceSettings.strRule[5] == "")
                return;
            string[] form = AdvanceSettings.strRule[5].Split(';');
            if(form[0] == radioButton1.Text.Replace("&", ""))
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }
            if(form[1].IndexOf(checkBox1.Text.Replace("&", ""))!=-1)
            {
                checkBox1.Checked = true;
            }
            if (form[1].IndexOf(checkBox2.Text.Replace("&", "")) != -1)
            {
                checkBox2.Checked = true;
            }
            if (form[1].IndexOf(checkBox3.Text.Replace("&", "")) != -1)
            {
                checkBox3.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!panel1.Enabled)
            {
                this.Close();
                return;
            }
            if (radioButton1.Checked)
            {
                AdvanceSettings.strRule[5] = radioButton1.Text.Replace("&", "") + ";";
            }
            else if (radioButton2.Checked)
            {
                AdvanceSettings.strRule[5] = radioButton2.Text.Replace("&", "") + ";";
            }
            if(checkBox1.Checked)
                AdvanceSettings.strRule[5] += checkBox1.Text.Replace("&", "") + ",";
            if (checkBox2.Checked)
                AdvanceSettings.strRule[5] += checkBox2.Text.Replace("&", "") + ",";
            if (checkBox3.Checked)
                AdvanceSettings.strRule[5] += checkBox3.Text.Replace("&", "") + ",";
            AdvanceSettings.strRule[5] += ";";
            FWCtrl f = new FWCtrl();
            List<string> values = new List<string>();
            for(int i = 0; i < AdvanceSettings.strRule.Length; i++)
            {
                string[] form = AdvanceSettings.strRule[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                values.AddRange(form);                
            }


            int index = 0;
            string name = values.ElementAt(index);

            index++;
            bool dirIn = true;
            if (values.ElementAt(index) == "Out")
            {
                dirIn = false;
            }


            index++;
            int protocol;
            if (values.ElementAt(index) == "TCP")
            {
                protocol = 6;
            }
            else if(values.ElementAt(index)=="UDP")
            {
                protocol = 17;
            }
            else
            {
                protocol = 1;
            }
            
            index++;
            string ip;
            if(values.ElementAt(index)== "Any IP Address")
            {
                ip = "";
            }
            else if(values.ElementAt(index)== "IP Address Range")
            {
                index++;
                ip = values.ElementAt(index).Replace(" ", "") + "-";
                index++;
                ip += values.ElementAt(index).Replace(" ", "");
            }
            else
            {
                index++;
                ip = values.ElementAt(index).Replace(" ", "");
            }

            index++;
            string port;
            if (values.ElementAt(index) == "All Ports")
            {
                port = "";
            }
            else if (values.ElementAt(index) == "Port Range")
            {
                index++;
                port = values.ElementAt(index) + "-";
                index++;
                port += values.ElementAt(index);
            }
            else
            {
                index++;
                port = values.ElementAt(index);
            }

            index++;
            string remote_ip;
            if (values.ElementAt(index) == "Any IP Address")
            {
                remote_ip = "";
            }
            else if (values.ElementAt(index) == "IP Address Range")
            {
                index++;
                remote_ip = values.ElementAt(index).Replace(" ", "") + "-";
                index++;
                remote_ip += values.ElementAt(index).Replace(" ", "");
            }
            else
            {
                index++;
                remote_ip = values.ElementAt(index).Replace(" ", "");
            }

            index++;
            string remote_port;
            if (values.ElementAt(index) == "All Ports")
            {
                remote_port = "";
            }
            else if (values.ElementAt(index) == "Port Range")
            {
                index++;
                remote_port = values.ElementAt(index) + "-";
                index++;
                remote_port += values.ElementAt(index);
            }
            else
            {
                index++;
                remote_port = values.ElementAt(index);
            }
            string action, profiles;
            index++;
            action = values.ElementAt(index);
            index++;
            profiles = values.ElementAt(index).TrimEnd(',');
            int vprofile = 0;
            if(profiles.IndexOf("Domain")!=-1)
            {
                vprofile |= (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_DOMAIN;
            }
            if (profiles.IndexOf("Private") != -1)
            {
                vprofile |= (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE;
            }
            if (profiles.IndexOf("Public") != -1)
            {
                vprofile |= (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC;
            }            
            f.Setup(name, (NetFwTypeLib.NET_FW_IP_PROTOCOL_)protocol, ip, port, remote_ip, remote_port, action=="Allow"?NetFwTypeLib.NET_FW_ACTION_.NET_FW_ACTION_ALLOW:NetFwTypeLib.NET_FW_ACTION_.NET_FW_ACTION_BLOCK, vprofile, dirIn?NetFwTypeLib.NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN:NetFwTypeLib.NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT);
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                AdvanceSettings.strRule[5] = radioButton1.Text.Replace("&", "") + ";";
            }
            else if (radioButton2.Checked)
            {
                AdvanceSettings.strRule[5] = radioButton2.Text.Replace("&", "") + ";";
            }
            if (checkBox1.Checked)
                AdvanceSettings.strRule[5] += checkBox1.Text.Replace("&", "") + ",";
            if (checkBox2.Checked)
                AdvanceSettings.strRule[5] += checkBox2.Text.Replace("&", "") + ",";
            if (checkBox3.Checked)
                AdvanceSettings.strRule[5] += checkBox3.Text.Replace("&", "") + ",";
            AdvanceSettings.strRule[5] += ";";
            this.Hide();
            new fAddEditExceptions5().ShowDialog();
            this.Close();
        }
    }
}
