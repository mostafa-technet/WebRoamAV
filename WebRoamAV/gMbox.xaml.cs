using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for gMbox.xaml
    /// </summary>
    public partial class gMbox : Window
    {
        string _text, _title;
        TimeSpan _tmspan;
        public gMbox(string text, string title, double tm = 2000)
        {
            InitializeComponent();
            _tmspan = new TimeSpan(0,0,2);
            _text = text;
            _title = title;
            _tmspan = TimeSpan.FromMilliseconds(tm);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ShowInTaskbar = false;
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                this.Left = desktopWorkingArea.Right - this.Width;
                this.Top = desktopWorkingArea.Bottom - this.Height;
                tmsg.Text = _text;
                this.Title = _title;
                Timer tmr = new Timer();
                tmr.Elapsed += Tmr_Elapsed;
                tmr.Interval = _tmspan.TotalMilliseconds;
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
            try
            {
                this.Dispatcher.Invoke(() => { this.Close(); this.Visibility = Visibility.Hidden; });
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
    }
}
