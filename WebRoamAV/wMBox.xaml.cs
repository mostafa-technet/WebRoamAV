using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WebRoamAV
{
    /// <summary>
    /// Interaction logic for wMBox.xaml
    /// </summary>
    public partial class wMBox : Window
    {
        private string _text, _title;
        public wMBox():this("","")
        {

        }
        public wMBox(string text, string title)
        {
            InitializeComponent();
            try
            { 
            _text = text;
            _title = title;
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Topmost = true;
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                this.Left = desktopWorkingArea.Right - this.Width;
                this.Top = desktopWorkingArea.Bottom - this.Height;
                tmsg.Text = _text;
                this.Title = _title;
                Timer tmr = new Timer();
                tmr.Elapsed += Tmr_Elapsed;
                tmr.Interval = 2000;
                tmr.Enabled = true;
                tmr.Start();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void Tmr_Elapsed(object sender, ElapsedEventArgs e)
        {          
            this.Dispatcher.Invoke(()=>this.Close());
        }
    }
}
