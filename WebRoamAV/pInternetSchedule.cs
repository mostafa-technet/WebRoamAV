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

    public partial class pInternetSchedule : Form
    {
        public pInternetSchedule()
        {
            InitializeComponent();
        }

        private void pInternetSchedule_Load(object sender, EventArgs e)
        {
            calendar1.ViewStart = DateTime.Now.AddDays(0 - (DateTime.Now.DayOfWeek));
            calendar1.Font = new Font(calendar1.Font.FontFamily, 7.5f);
            calendar1.AutoScroll = true;
            calendar1.AllowItemEdit = false;
            calendar1.ItemDoubleClick += Calendar1_ItemDoubleClick;
            calendar1.Enabled = false;
            label1.Text = Resources.Resources.pInternetSchedulL1;
        }

        private void Calendar1_ItemDoubleClick(object sender, System.Windows.Forms.Calendar.CalendarItemEventArgs e)
        {

            SendKeys.SendWait("{ENTER}");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            calendar1.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            calendar1.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
           
            MessageBox.Show(calendar1.Items[0].StartDate.ToString());
        }

        private void calendar1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SendKeys.SendWait("{ENTER}");
        }
    }
}