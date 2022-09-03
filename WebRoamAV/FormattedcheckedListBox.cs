using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows;
using System.Windows.Forms;
//using System.Windows.Media;

namespace WebRoamAV
{
    public class fItem
    {
        public string Text
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }

    }
    public partial class FormattedcheckedListBox : Panel
    {
        public FormattedcheckedListBox()
        {
            InitializeComponent();
            this.BackColor = Color.White;
            DeFont = SystemFonts.DefaultFont;
            TeFont = new Font(SystemFonts.DefaultFont.FontFamily, 15);
            LeTPadding = 10;
            LeDPadding = 20;
            TopPadding = 10;
            ItemsCount = 0;
            ItemHeight = 70;
            this.BorderStyle = BorderStyle.Fixed3D;
            this.AutoScroll = true;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public Font DeFont
        {
            get;
            set;
        }
        public Font TeFont
        {
            get;
            set;
        }
        public int LeTPadding
        {
            get;
            set;
        }
        public int LeDPadding
        {
            get;
            set;
        }
        public int TopPadding
        {
            get;
            set;
        }
        public int ItemsCount
        {
            get;
            set;
        }
        public int ItemHeight
        {
            get;
            set;
        }
        public void AddItem(fItem item)
        {
            //new fItem() { Text = "", Description = ""};

            CheckBox checkB = new CheckBox();
            Label lblT = new Label();
            Label lblD = new Label();
            Panel p = new Panel();
            lblT.MouseDown += delegate (object sender, MouseEventArgs e) {
                for (int i = 0; i < this.Controls.Count; i++)
                    this.Controls[i].BackColor = Color.White;
                p.BackColor = Color.FromArgb(100, 100, 50, 250);
            };
            checkB.MouseDown += delegate (object sender, MouseEventArgs e) {
                for (int i = 0; i < this.Controls.Count; i++)
                    this.Controls[i].BackColor = Color.White;
                p.BackColor = Color.FromArgb(100, 100, 50, 250);
            };
            p.MouseDown += delegate (object sender, MouseEventArgs e) {
                for (int i = 0; i < this.Controls.Count; i++)
                    this.Controls[i].BackColor = Color.Transparent;
                p.BackColor = Color.FromArgb(100,100,50,250); };
            checkB.Text = " ";
            lblT.Text = item.Text;
            lblT.Font = TeFont;            
            p.Location = new Point(0, ItemsCount * (ItemHeight));
            checkB.Location = new Point(LeTPadding, TopPadding);
            lblT.Location = new Point(checkB.Location.X + 15 + (int)new System.Windows.Media.FormattedText(checkB.Text, CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new System.Windows.Media.Typeface(new System.Windows.Media.FontFamily(checkB.Font.FontFamily.Name), System.Windows.FontStyles.Normal, System.Windows.FontWeights.Normal, System.Windows.FontStretches.Normal), checkB.Font.Size, System.Windows.Media.Brushes.Black).Width + LeTPadding, checkB.Location.Y-(int)checkB.Height/4);
            lblD.Location = new Point(lblT.Location.X + LeDPadding, TopPadding + lblT.Height + checkB.Location.Y);
            if (item.Description != string.Empty)
            {                
                lblD.Text = item.Description;
                lblD.Font = DeFont;
            }
            checkB.AutoSize = lblT.AutoSize = lblD.AutoSize = true;
            checkB.BackColor = lblT.BackColor = lblD.BackColor = Color.Transparent;
            p.AutoSize = true;
            p.Height = ItemHeight;
            p.Width = this.Width;
            p.Controls.Add(checkB);
            p.Controls.Add(lblT);
            p.Controls.Add(lblD);
            this.Controls.Add(p);
            ItemsCount++;
        }
    }
}
