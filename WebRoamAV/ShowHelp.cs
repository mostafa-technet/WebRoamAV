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
    public partial class ShowHelp : Form
    {
        public ShowHelp()
        {
            InitializeComponent();
        }

        private void ShowHelp_Load(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, AppDomain.CurrentDomain.BaseDirectory + "\\test.chm", HelpNavigator.Topic, "Welcome.htm");
        }
    }
}
