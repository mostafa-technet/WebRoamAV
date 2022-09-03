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
    public partial class tExcludeFileExtension : Form
    {
        public tExcludeFileExtension()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(!textBox1.Text.StartsWith("."))
            {
                listBox1.Items.Add("." + textBox1.Text);
            }    
            else
            {
                listBox1.Items.Add(textBox1.Text);
            }
            button3.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = textBox1.Text != "";
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
            IniFile inf = new IniFile(file_ini);

            if (inf.KeyExists("EXCLUDED_EXTENSION"))
            {
                inf.DeleteKey("EXCLUDED_EXTENSION");
            }
            string extensions = "";
            foreach (var item in listBox1.Items)
                extensions += item.ToString();
            inf.Write("EXCLUDED_EXTENSION",extensions);
            App.AppSettings.ExcludedExtensions = extensions;
            this.Close();
        }

        private void tExcludeFileExtension_Load(object sender, EventArgs e)
        {
            if(App.AppSettings.ExcludedExtensions!="")
            foreach (var item in App.AppSettings.ExcludedExtensions.Split('.'))
                listBox1.Items.Add(item);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
            if (listBox1.Items.Count == 0)
                button3.Enabled = false;
        }
    }
}
