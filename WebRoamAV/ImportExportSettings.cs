using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class ImportExportSettings : Form
    {
        public ImportExportSettings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var allfiles1 = Directory.GetFiles(Environment.CurrentDirectory).Where(f=> ".ini|.sfd|.conf|.wfw".Split('|').Any(f.ToLower().EndsWith));
            var allfiles2 = Directory.GetFiles(Environment.CurrentDirectory + "\\wrIPS", "*.conf");
            var allfiles3 = Directory.GetFiles(Environment.CurrentDirectory + "\\fwLevels", "*.dat");
           
            if (radioButton1.Checked)
            {
                List<string> allfiles = new List<string>();
                allfiles.AddRange(allfiles1);
                allfiles.AddRange(allfiles2);
                allfiles.AddRange(allfiles3);
                var sfd = new SaveFileDialog();
                sfd.Filter = "*.dat|*.dat";
                if (sfd.ShowDialog() != DialogResult.Cancel)
                {
                    if (sfd.FileName != "")
                    {
                        BackUpSettings.getBackUpfromFiles(sfd.FileName, allfiles.ToArray());
                        MessageBox.Show("Settings exported successfully.", "Import/Export Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
            }
            else
            {
                List<string> allfiles = new List<string>();
                allfiles.AddRange(allfiles1);//.Select(s=>s.Split('\\').Last()));
                allfiles.AddRange(allfiles2);//.Select(s => s.Substring(s.LastIndexOf("\\wrIPS"))));
                allfiles.AddRange(allfiles3);//.Select(s => s.Substring(s.LastIndexOf("\\fwLevels"))));
                allfiles.ForEach(f => File.Delete(f));
                var ofd = new OpenFileDialog();
                ofd.Filter = "*.dat|*.dat";
                if(ofd.ShowDialog()!= DialogResult.Cancel)
                {
                    if(ofd.FileName!="")
                    {
                        if (MessageBox.Show("This will overwrite all settings that you have configured.\nDo you want to continue?", "Import/Export Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.No)
                        {
                            BackUpSettings.UnBackUpFiles(ofd.FileName, allfiles.ToArray());
                            MessageBox.Show("Settings imported successfully.", "Import/Export Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();

                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
