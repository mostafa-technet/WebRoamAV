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
    public partial class pPCSchedule : Form
    {
        public pPCSchedule()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            checkR = 0;
            panel3.Enabled = false;
            calendar1.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            checkR = 1;
            panel3.Enabled = true;
            calendar1.Enabled = true;
        }

        private void pPCSchedule_Load(object sender, EventArgs e)
        {
            calendar1.ViewStart = DateTime.Now.AddDays(0 - (DateTime.Now.DayOfWeek));
            calendar1.Font = new Font(calendar1.Font.FontFamily, 7.5f);
            calendar1.AutoScroll = true;
            calendar1.AllowItemEdit = false;
            calendar1.ItemDoubleClick += Calendar1_ItemDoubleClick;
            calendar1.Enabled = false;
            comboBox1.SelectedIndex = 0;
            calendar1.BackColor = Color.Gray;
        }

        private void Calendar1_ItemDoubleClick(object sender, System.Windows.Forms.Calendar.CalendarItemEventArgs e)
        {
            SendKeys.SendWait("{ENTER}");
        }
        int checkR = 0;
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            panel4.Enabled = false;
            calendar1.Enabled = true;
            checkR = 2;
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            panel4.Enabled = true;
            calendar1.Enabled = false;
            checkR = 3;
        }

        private void calendar1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SendKeys.SendWait("{ENTER}");
        }
    }
}
