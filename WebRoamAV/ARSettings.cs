using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class ARSettings : Form
    {
        public ARSettings()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //this.Hide();
            var tc = new TcpClient();
            byte[] buff = Encoding.ASCII.GetBytes("wrMainAntiRansomeware.Form1");
             tc.ConnectAsync("localhost", 2900).ContinueWith(
                 delegate  {
                 var st = tc.GetStream();
                st.WriteAsync(buff, 0, buff.Length);
                st.FlushAsync().ContinueWith(delegate{
                    tc.Close();
                });
            });
            //this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Process.Start($"{wARProtection.ARFolder}\\gui\\webroamransomwgui.exe").WaitForExit();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {       

            var tc = new TcpClient();
            byte[] buff = Encoding.ASCII.GetBytes("wrMainAntiRansomeware.AddBlacklist");
            tc.ConnectAsync("localhost", 2900).ContinueWith(
                delegate {
                    var st = tc.GetStream();
                    st.WriteAsync(buff, 0, buff.Length);
                    st.FlushAsync().ContinueWith(delegate {
                        tc.Close();
                    });
                });

        }


        private void button4_Click(object sender, EventArgs e)
        {
          
            var tc = new TcpClient();
            byte[] buff = Encoding.ASCII.GetBytes("wrMainAntiRansomeware.AddWhiteList");
            tc.ConnectAsync("localhost", 2900).ContinueWith(
                delegate {
                    var st = tc.GetStream();
                    st.WriteAsync(buff, 0, buff.Length);
                    st.FlushAsync().ContinueWith(delegate {
                        tc.Close();
                    });
                });
        }
    }
}
