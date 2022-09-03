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
    public partial class wFolders : Form
    {
        public wFolders()
        {
            InitializeComponent();
        }

        private void wFolders_Load(object sender, EventArgs e)
        {
            MessageBox.Show(dataGridView1.Rows[0].Cells[0].ToString());
        }
    }
}
