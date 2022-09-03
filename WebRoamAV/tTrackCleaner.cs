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
    public partial class tTrackCleaner : Form
    {
        public tTrackCleaner()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tTrackCleaner_Load(object sender, EventArgs e)
        {
            FormattedcheckedListBox flb = new FormattedcheckedListBox();
            flb.Width = 500;
            flb.Height = 200;
            flb.Location = new Point(50,100);
            fItem fi = new fItem() { Text = "text", Description = "this is a test"};
            flb.AddItem(fi);
            flb.AddItem(new fItem() { Text = "subject is another", Description = "this is the second time test" });
            this.Controls.Add(flb);
            flb.AddItem(new fItem() { Text = "subject is another3", Description = "this is the second time test3" });
            this.Controls.Add(flb);
        }
    }
}
