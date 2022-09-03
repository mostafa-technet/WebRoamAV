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
    public partial class EnterPassword : Form
    {
        public string wrPaswd = "";
       

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string md5i = CreateMD5(textBox1.Text);
            var text = from s in File.ReadAllLines(".\\app_config.ini", new UTF8Encoding(false)) where s.Replace(" ", string.Empty).Contains("Password=") select s;
            string rs = text.ElementAt(0).Replace(" ", string.Empty).Replace("Password=", "");
            if (md5i != rs)
            {
                MessageBox.Show("Invalid Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
            }
            wrPaswd = textBox1.Text;
        //    MessageBox.Show("The password was saved successfully!");

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void EnterPassword_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                this.button1_Click(null, null);
            }
        }

      
    }
}
