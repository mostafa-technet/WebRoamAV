using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
    public partial class wReportFor : Form
    {
        string _date, _time;
        int _titleGin = 0;
        System.Windows.Controls.DataGrid _gridMain;
        public static string ReportText;
        public wReportFor(string date, string time, int titleGIndex, System.Windows.Controls.DataGrid grid)
        {
            _date = date;
            _time = time;
            _titleGin = titleGIndex;
            _gridMain = grid;
            InitializeComponent();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {

            _gridMain.SelectedIndex++;
                btnPrev.Enabled = true;
                GRowsR gr = (GRowsR)_gridMain.SelectedItem;
                lblReportFor.Text = gr.ReportFor.Split('\n')[0];
                lblDate.Text = _date;
                lblTime.Text = _time;
                textBox1.Text = gr.ReportFor.Split('\n')[1];//ReportText;
            if (_gridMain.SelectedIndex == _gridMain.Items.Count-1)
            {
                btnNext.Enabled = false;
            }


        }

        private void btnPrev_Click(object sender, EventArgs e)
        {   _gridMain.SelectedIndex--; 
            if (_gridMain.SelectedIndex == 0)
            {
                btnPrev.Enabled = false;
            }
           
          
            btnNext.Enabled = true;
            GRowsR gr = (GRowsR)_gridMain.SelectedItem;
            lblReportFor.Text = gr.ReportFor.Split('\n')[0];
            lblDate.Text = _date;
            lblTime.Text = _time;
            textBox1.Text = gr.ReportFor.Split('\n')[1];//ReportText;
           
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument pdoc = new PrintDocument();
            pdoc.PrintPage += Pdoc_PrintPage;
            PrintDialog pdlg = new PrintDialog();
            if (pdlg.ShowDialog() == DialogResult.OK)
            {
                pdoc.Print();
            }
        }

        private void Pdoc_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 20;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;
            var printFont = new Font("Arial", 20, FontStyle.Regular);
            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);
            var streamreader = new StringReader(textBox1.Text);
            // Print each line of the file. 
            while (count < linesPerPage &&
               ((line = streamreader.ReadLine()) != null))
            {
                yPos = topMargin + (count *
                   printFont.GetHeight(ev.Graphics));

                ev.Graphics.DrawString(line, printFont, Brushes.Black,new RectangleF(leftMargin, yPos,ev.PageBounds.Width-ev.MarginBounds.X, ev.PageBounds.Height - ev.MarginBounds.Y), new StringFormat());
                count++;
            }

            // If more lines exist, print another page. 
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*.txt|*.txt";
            if(sfd.ShowDialog()!= DialogResult.Cancel)
            {
                File.WriteAllText(sfd.FileName, textBox1.Text);
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void wReportFor_Load(object sender, EventArgs e)
        {
            try
            {
                if (_gridMain.SelectedIndex > 0)
                    btnPrev.Enabled = true;
                if (_gridMain.SelectedIndex == _gridMain.Items.Count-1)
                    btnNext.Enabled = false;
                if (_titleGin == 5 && wpfReports.contents!= null && wpfReports.contents.Length > _titleGin)
                {
                    lblReportFor.Text = wpfReports.contents[_titleGin];
                    lblDate.Text = _date;
                    lblTime.Text = _time;
                    textBox1.Text = ReportText;
                }
                else
                {
                  
                    GRowsR gr = (GRowsR)_gridMain.SelectedItem;
                    lblReportFor.Text = gr.ReportFor.Split('\n')[0];
                    lblDate.Text = _date;
                    lblTime.Text = _time;
                    textBox1.Text = gr.ReportFor.Split('\n')[1];//ReportText;
                }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }

        }
    }
}
