using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static WebRoamAV.App;

namespace WebRoamAV
{
    /// <summary>
    /// Interaction logic for InNets.xaml
    /// </summary>
    public partial class InNets : Window
    {
        private ColorAnimation coloarn;
        string mw;
        public InNets(string parent)
        {
            InitializeComponent();
            mw = parent;
			this.Top = MainWindow.parentTop;
			this.Left = MainWindow.parentLeft;
            if (parent == null)
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            this.MouseDown += MainWindow_MouseDown;
        }

        public void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.GetPosition(this).Y < 70)
                this.DragMove();
        }
        public void buttonMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Type NetFwMgrType = Type.GetTypeFromProgID("HNetCfg.FwMgr", false);
                INetFwMgr mgr = (INetFwMgr)Activator.CreateInstance(NetFwMgrType);

                bool firewallEnabled = mgr.LocalPolicy.CurrentProfile.FirewallEnabled;
                //   MessageBox.Show(firewallEnabled.ToString());
                App.AppSettings.IsFirewallOn = firewallEnabled;
                if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='InNets' AND fieldID=4)") > 0)
                {
                    /*     if (SqlReaderWriter.ExecuteScalar("SELECT fieldOnOff FROM tblOnOff WHERE (frmName='InNets' AND fieldID=4)").ToString().ToLower() != "true")
                         {                                    
                             AppSettings.IsFirewallOn = false;

                 }*/

                    if (firewallEnabled)
                    {
                        /*Process proc = new Process();
                         string top = "netsh.exe";
                         proc.StartInfo.Arguments = " AdvFirewall set allprofiles state on";
                         proc.StartInfo.FileName = top;
                         proc.StartInfo.UseShellExecute = false;
                         proc.StartInfo.RedirectStandardOutput = true;
                         proc.StartInfo.CreateNoWindow = true;
                         proc.Start();
                         proc.WaitForExit();*/
                        SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=1 WHERE (fieldID=4 AND frmName='InNets')");

                    }
                }
                else
                {
                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + $"', 'InNets', 4, {(firewallEnabled ? 1 : 0)})");
                }

                LaOnOff.IsChecked = AppSettings.IPSIDSOn;
            fOnOff.IsChecked = AppSettings.IsFirewallOn;

            SSOnOff.IsChecked = AppSettings.IsBrowsingProtectOn;
            foOnOff.IsChecked = AppSettings.IsNewsAlertOn;
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void ButtonEsc_Click(object sender, KeyEventArgs e){if(e.Key == Key.Escape){Button_Click(null, null);}} private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(true)//if ((Mouse.GetPosition(ButtonMn).X < ButtonMn.Width) && (Mouse.GetPosition(ButtonMn).X > 0) && (Mouse.GetPosition(ButtonMn).Y < ButtonMn.Height) && (Mouse.GetPosition(ButtonMn).Y > 0))
            {
                MainWindow mw = new MainWindow();
                mw.Left = this.Left;
                mw.Top = this.Top;
                mw.WindowStartupLocation = WindowStartupLocation.Manual;
                mw.Show(); this.Close();
               /* Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType((mw.StartsWith("WebRoamAV.") ? "" : "WebRoamAV.") + mw.ToString().Replace(".xaml", ""));
                MainWindow.parentTop = this.Top;	
				MainWindow.parentLeft = this.Left;	
				t.GetMethod("Show").Invoke(Activator.CreateInstance(t, this.ToString()), new object[] { });
                this.Close();*/
            }
        }
        private void link_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType(e.Uri.ToString().StartsWith("WebRoamAV.") ? "" : "WebRoamAV." + e.Uri.ToString().Replace(".xaml", ""));
            MainWindow.parentTop = this.Top;	
				MainWindow.parentLeft = this.Left;	
				t.GetMethod("Show").Invoke(Activator.CreateInstance(t, this.ToString()), new object[] { });
            this.Close();
        }

        private void link4_Click(object sender, RoutedEventArgs e)
        {

            Hyperlink h = sender as Hyperlink;
            ContextMenu contextMenu = (ContextMenu)this.link4.Resources["link41"];
            contextMenu.PlacementTarget = this.tlink4;
            contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
            contextMenu.IsOpen = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.OriginalSource as MenuItem;
           //MessageBox.Show(mi.ToString());
        }
        private void ScrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;

            if (scrollViewer.IsMouseOver)
            {
                Point p = e.MouseDevice.GetPosition(scrollViewer);
                //  MessageBox.Show(scrollViewer.ContentVerticalOffset.ToString());
                if (587 - p.X <= 21 && (p.Y - 21 >= (155 - 21) * (scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight)) && p.Y < ((155 - 21) * (scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight)) + 155)
                {
                    LinearGradientBrush resources = (LinearGradientBrush)this.Resources["GradientBrush"];
                    resources.GradientStops[0].Color = Colors.Green;
                }
                else
                {
                    LinearGradientBrush resources = (LinearGradientBrush)this.Resources["GradientBrush"];
                    resources.GradientStops[0].Color = (Color)ColorConverter.ConvertFromString("#6623AA48");
                }
            }
            else
            {
                LinearGradientBrush resources = (LinearGradientBrush)this.Resources["GradientBrush"];
                resources.GradientStops[0].Color = (Color)ColorConverter.ConvertFromString("#6623AA48");
            }
        }

        private void ScrollViewer_MouseLeave(object sender, MouseEventArgs e)
        {
            LinearGradientBrush resources = (LinearGradientBrush)this.Resources["GradientBrush"];
            resources.GradientStops[0].Color = (Color)ColorConverter.ConvertFromString("#6623AA48");
        }

        private void fOnOff_Checked(object sender, RoutedEventArgs e)
        {
            
            Task.Factory.StartNew(() =>
            {
                try
            { 
                
                Process proc = new Process();
                string top = "netsh.exe";
                proc.StartInfo.Arguments = " AdvFirewall set allprofiles state on";
                proc.StartInfo.FileName = top;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.WaitForExit();
                SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=1 WHERE (fieldID=4 AND frmName='InNets')");
                App.AppSettings.IsFirewallOn = true;
                Dispatcher.Invoke(() => fText.Text = "ON");
            }
            catch (Exception em)
                {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        });
           
        }

        private void fOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            { 
            Task.Factory.StartNew(() =>
            {
               
                Process proc = new Process();
                string top = "netsh.exe";
                proc.StartInfo.Arguments = " AdvFirewall set allprofiles state off";
                proc.StartInfo.FileName = top;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.WaitForExit();
                SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=0 WHERE (fieldID=4 AND frmName='InNets')");
                App.AppSettings.IsFirewallOn = false;
                Dispatcher.Invoke(() => fText.Text = "OFF");
            });
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
        private void sOnOff_Checked(object sender, RoutedEventArgs e)
        {
            sText.Text = "ON";
        }

        private void sOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            sText.Text = "OFF";
        }
        private void tOnOff_Checked(object sender, RoutedEventArgs e)
        {
            //tText.Text = "ON";
        }

        private void tOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
          //  tText.Text = "OFF";
        }
        private void foOnOff_Checked(object sender, RoutedEventArgs e)
        {
            try
            { 
            App.AppSettings.IsNewsAlertOn = true;
            SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=1 WHERE (fieldID=7 AND frmName='InNets')");
            foText.Text = "ON";
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void foOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            try { 
            App.AppSettings.IsNewsAlertOn = false;
            SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=0 WHERE (fieldID=7 AND frmName='InNets')");            
            foText.Text = "OFF";
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }


       

      

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            Control target = e.Source as Control;

            if (target != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }
        private void Coloarn_Completed(object sender, EventArgs e)
        {
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            if (btn.IsEnabled)
            {
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
                coloarn = new ColorAnimation();
                coloarn.To = Colors.Green;
                coloarn.From = (Color)ColorConverter.ConvertFromString("#A0008000");
                coloarn.Duration = new Duration(TimeSpan.FromSeconds(0.1));
                coloarn.Completed += Coloarn_Completed;
                btn.Background.BeginAnimation(SolidColorBrush.ColorProperty, coloarn);
                this.GetType().GetMethod(btn.Name + "_Click").Invoke(this, new object[] { sender, e });

            }


        }

        private void LaOnOff_Checked(object sender, RoutedEventArgs e)
        {
            LaText.Text = "Waiting...";
            Task.Factory.StartNew(() =>
            {
                try
                {
                    App.AppSettings.IPSIDSOn = true;
                    SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=1 WHERE (fieldID=8 AND frmName='InNets')");
                    WebroamIPS.Enable();
                    WebroamIPS.StartService();


                }
                catch (Exception em)
                {
                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new System.Diagnostics.StackFrame(1, true).GetFileName() + " " + new System.Diagnostics.StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                }
            }).ContinueWith((t) => { Dispatcher.Invoke(() => LaText.Text = "ON"); });
           
        }

        private void LaOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                App.AppSettings.IPSIDSOn = false;
                SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=0 WHERE (fieldID=8 AND frmName='InNets')");
                WebroamIPS.StopService();
                WebroamIPS.Disable();
                LaText.Text = "OFF";
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void SSOnOff_Checked(object sender, RoutedEventArgs e)
        {
            try
            { 
            App.AppSettings.IsBrowsingProtectOn = true;
            SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=1 WHERE (fieldID=5 AND frmName='InNets')");          
            SSText.Text = "ON";
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void SSOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            try { 
            App.AppSettings.IsBrowsingProtectOn = false;
            SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=0 WHERE (fieldID=5 AND frmName='InNets')");
            WebroamIPS.StopService();
            SSText.Text = "OFF";
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void TTOnOff_Checked(object sender, RoutedEventArgs e)
        {
           // TTText.Text = "ON";
        }

        private void TTOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            //TTText.Text = "OFF";
        }

        private void Canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try { 
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return; this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wFirewallProtection(this.ToString()).ShowDialog();
            this.Close();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void Canvas_PreviewMouseUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((e.OriginalSource.GetType() == typeof(System.Windows.Controls.Primitives.ToggleButton))) return; this.Hide();
                MainWindow.parentLeft = this.Left;
                MainWindow.parentTop = this.Top;
                new wMalwareProtection(this.ToString()).ShowDialog();
                this.Close();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
            }

        private void Canvas_PreviewMouseUp_2(object sender, MouseButtonEventArgs e)
        {
            try
            { 
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return; this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wSandboxBrowser(this.ToString()).ShowDialog();
            this.Close();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }


        private void Canvas_PreviewMouseUp_3(object sender, MouseButtonEventArgs e)
        {
            try
            { 
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return; this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wSafeBanking(this.ToString()).ShowDialog();
            this.Close();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                try
                {                 
                    MainWindow.parentLeft = this.Left;
                    MainWindow.parentTop = this.Top;
                    new wFirewallProtection(this.ToString()).ShowDialog();
                    this.Close();
                }
                catch (Exception em)
                {
                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                }
            }
        }

        private void fOnOff_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                foOnOff.IsChecked = !foOnOff.IsChecked;
            }
        }

        private void ButtonMn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                Button_Click(null, null);
            }
        }


        private void SSOnOff_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                SSOnOff.IsChecked = !SSOnOff.IsChecked;
            }
        }

        private void foOnOff_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                foOnOff.IsChecked = !foOnOff.IsChecked;
            }
        }

        private void LaOnOff_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                LaOnOff.IsChecked = !LaOnOff.IsChecked;
            }
        }

        private void buttonMin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                this.WindowState = WindowState.Minimized;
            }
        }

        private void buttonClose_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                this.Close();
            }
        }
    }
}
