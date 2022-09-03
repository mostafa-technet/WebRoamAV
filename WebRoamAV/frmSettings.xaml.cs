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

namespace WebRoamAV
{
    /// <summary>
    /// Interaction logic for frmSettings.xaml
    /// </summary>
    /// 
    public static partial class Commands
    {
        public static readonly RoutedCommand btnDflPressed = new RoutedCommand();
    }
        public partial class frmSettings : Window
    {
        string mw;
        private ColorAnimation coloarn;
        public frmSettings(string parent)
        {
            InitializeComponent();
            mw = parent;
			this.Top = MainWindow.parentTop;
			this.Left = MainWindow.parentLeft;
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
            sOnOff.IsChecked = App.AppSettings.IsSelfProtected;
            fOnOff.IsChecked = App.AppSettings.IsAutoUpdateOn;
            tOnOff.IsChecked = gSettings.Password != "";
            foOnOff.IsChecked = App.AppSettings.IsVirusReportSendOn;
        }

        private void ButtonEsc_Click(object sender, KeyEventArgs e){if(e.Key == Key.Escape){Button_Click(null, null);}} private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(true)//if ((Mouse.GetPosition(ButtonMn).X < ButtonMn.Width) && (Mouse.GetPosition(ButtonMn).X > 0) && (Mouse.GetPosition(ButtonMn).Y < ButtonMn.Height) && (Mouse.GetPosition(ButtonMn).Y > 0))
            {
                MainWindow mw = new MainWindow();
                mw.Left = this.Left;
                mw.Top = this.Top;
                mw.WindowStartupLocation = WindowStartupLocation.Manual;
                
               /* Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType((mw.StartsWith("WebRoamAV.") ? "" : "WebRoamAV.") + mw.ToString().Replace(".xaml", ""));                
                MainWindow.parentTop = this.Top;	
				MainWindow.parentLeft = this.Left;	
				t.GetMethod("Show").Invoke(Activator.CreateInstance(t, this.ToString()), new object[] { });*/               
                mw.Show(); this.Close();
                this.Close();
            }
        }
        private void link_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            if (e.Uri.ToString() == "")
                return;
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
           // MessageBox.Show(mi.ToString());
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
            try
            { 
                fText.Text = "ON";
                lock (App.LK_VP)
                {
                    App.AppSettings.IsVirusProtectOn = true;
                }
                SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=1 WHERE (fieldID=5 AND frmName='Settings')");
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void fOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            { 
            fText.Text = "OFF";
                lock (App.LK_VP)
                {
                    App.AppSettings.IsVirusProtectOn = false;
                }
                SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=0 WHERE (fieldID=5 AND frmName='Settings')");
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
        private void sOnOff_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                App.AppSettings.IsSelfProtected = true;
                SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=1 WHERE (fieldID=4 AND frmName='Settings')");
                lock (ProcessProtector.oluck)
                {
                    ProcessProtector.isOn = true;
                }
                sText.Text = "ON";
            }
            catch(Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void sOnOff_Unchecked(object sender, RoutedEventArgs e)
        {      
            try
            { 
            App.AppSettings.IsSelfProtected = false;
            SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=0 WHERE (fieldID=4 AND frmName='Settings')");
            lock (ProcessProtector.oluck)
            {
                ProcessProtector.isOn = false;
            }
            sText.Text = "OFF";
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        CreatePassword createPassword = null;
        private bool ShowCreatePassword()
        {
            System.Windows.Forms.DialogResult dl = System.Windows.Forms.DialogResult.Cancel;
            try
            { 
            if (createPassword != null)
            {
                if (createPassword.WindowState == System.Windows.Forms.FormWindowState.Minimized)
                {
                    createPassword.WindowState = System.Windows.Forms.FormWindowState.Normal;
                }
                createPassword.Activate();
                bool topm = createPassword.TopMost;
                createPassword.TopMost = true;
                createPassword.Focus();
                createPassword.TopMost = topm;
            }
            else
            {
                createPassword = new CreatePassword();
               
               // createPassword.Focus();
                bool topm = createPassword.TopMost;
                createPassword.TopMost = true;
                //createPassword.Focus();
                createPassword.TopMost = topm;
                createPassword.FormClosed += CreatePassword_FormClosed; 
                dl = createPassword.ShowDialog();
            }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
            return dl == System.Windows.Forms.DialogResult.OK;
        }

        private void CreatePassword_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            createPassword = null;
        }
        private void tOnOff_Checked(object sender, RoutedEventArgs e)
        {             
            try
            { 
            if (gSettings.Password=="")
            {
                bool b = ShowCreatePassword();
                    tText.Text = b ? "ON" : "OFF";
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    tOnOff.IsChecked = b;
                }));

            }

            if (gSettings.Password != "")
            {
                tText.Text = "ON";
                App.AppSettings.IsPasswProtectOn = true;
                SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=1 WHERE (fieldID=6 AND frmName='Settings')");
            }
                /* else
                 {
                     Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                     {
                         tOnOff.IsChecked = false;
                     }));
                 }*/
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void tOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            { 
            string[] flines = File.ReadAllLines(".\\app_config.ini");
            var lines = flines.Where(st => !st.Contains("Password"));
            File.WriteAllLines(".\\app_config.ini", lines.ToArray());
            gSettings.Password = "";            
            tText.Text = "OFF";
            App.AppSettings.IsPasswProtectOn = false;
            SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=0 WHERE (fieldID=6 AND frmName='Settings')");
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
        private void foOnOff_Checked(object sender, RoutedEventArgs e)
        {
            try
            { 
            foText.Text = "ON";
            App.AppSettings.IsVirusReportSendOn = true;
            //SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=1 WHERE (fieldID=7 AND frmName='Settings')");
            string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
            IniFile inf = new IniFile(file_ini);

            //fill config of our system/AV with default values if the config file didn't exist
            if (File.Exists(file_ini))
            {
                /*inf.Write("IsAutoUpdateOn", "True", "WebroamAV");
                inf.Write("IsPasswProtectOn", "False", "WebroamAV");*/
                if (inf.KeyExists("WebroamAV"))
                    inf.DeleteKey("IsVirusReportSendOn", "WebroamAV");
                inf.Write("IsVirusReportSendOn", "True", "WebroamAV");
            }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void foOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            { 
            foText.Text = "OFF";
            App.AppSettings.IsVirusReportSendOn = false;
           // SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=0 WHERE (fieldID=7 AND frmName='Settings')");
            string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
            IniFile inf = new IniFile(file_ini);

            //fill config of our system/AV with default values if the config file didn't exist
            if (File.Exists(file_ini))
            {
                /*inf.Write("IsAutoUpdateOn", "True", "WebroamAV");
                inf.Write("IsPasswProtectOn", "False", "WebroamAV");*/
                if (inf.KeyExists("WebroamAV"))
                    inf.DeleteKey("IsVirusReportSendOn", "WebroamAV");
                inf.Write("IsVirusReportSendOn", "False", "WebroamAV");
            }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        public void btnDefault_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("This action will revert all settings to default, do you wish to continue?", "Webroam EPS", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                sOnOff.IsChecked = true;
                foOnOff.IsChecked = true;
                gSettings.Password = "";
                fOnOff.IsChecked = false;
                tOnOff.IsChecked = false;
            }
        }

        private void Default_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnDefault.RenderTransformOrigin = new Point(0.5, 0.5);
            btnDefault.RenderTransform = scale;

            DoubleAnimation growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation.To = 1;
            growAnimation.From = 0.9;
            storyboard.Children.Add(growAnimation);

            DoubleAnimation growAnimation2 = new DoubleAnimation();
            growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation2.To = 1;
            growAnimation2.From = 0.9;
            storyboard.Children.Add(growAnimation2);

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growAnimation, btnDefault);
            Storyboard.SetTarget(growAnimation2, btnDefault);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnDefault_Click(null, null);
        }


        private void Default_MouseEnter(object sender, MouseEventArgs e)
        {
            btnDefault.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }
        private void Default_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnDefault.RenderTransformOrigin = new Point(0.5, 0.5);
                btnDefault.RenderTransform = scale;

                DoubleAnimation growAnimation = new DoubleAnimation();
                growAnimation.Duration = TimeSpan.FromMilliseconds(100);
                growAnimation.To = 1;
                growAnimation.From = 0.9;
                storyboard.Children.Add(growAnimation);

                DoubleAnimation growAnimation2 = new DoubleAnimation();
                growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
                growAnimation2.To = 1;
                growAnimation2.From = 0.9;
                storyboard.Children.Add(growAnimation2);

                Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
                Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
                Storyboard.SetTarget(growAnimation, btnDefault);
                Storyboard.SetTarget(growAnimation2, btnDefault);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnDefault.Background = new SolidColorBrush(Colors.Green);

        }

        private void Default_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnDefault.RenderTransformOrigin = new Point(0.5, 0.5);
            btnDefault.RenderTransform = scale;

            DoubleAnimation growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation.From = 1;
            growAnimation.To = 0.9;
            storyboard.Children.Add(growAnimation);

            DoubleAnimation growAnimation2 = new DoubleAnimation();
            growAnimation2.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation2.From = 1;
            growAnimation2.To = 0.9;
            storyboard.Children.Add(growAnimation2);

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growAnimation, btnDefault);
            Storyboard.SetTarget(growAnimation2, btnDefault);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
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

        private void Canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {            
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return;
            this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wAutomaticUpdate(this.ToString()).ShowDialog();
            this.Close();
        }

        private void Canvas_PreviewMouseUp_1(object sender, MouseButtonEventArgs e)
        {
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return;
            this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wInternetSettings(this.ToString()).ShowDialog();
            this.Close();
        }

        private void Canvas_PreviewMouseUp_2(object sender, MouseButtonEventArgs e)
        {
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return;
            this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wRegisterRestore(this.ToString()).ShowDialog();
            this.Close();
        }

        private void Canvas_PreviewMouseUp_3(object sender, MouseButtonEventArgs e)
        {
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return;
            this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wReportSettings(this.ToString()).ShowDialog();
            this.Close();
        }

        private void Buttonimex_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new ImportExportSettings().ShowDialog();
        }

        private void ButtonMn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                Button_Click(null, null);
            }
        }

        private void buttonMin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                buttonMin_Click(null, null);
            }
        }

        private void buttonClose_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                buttonClose_Click(null, null);
            }
        }

        private void Buttonimex_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                Buttonimex_PreviewMouseLeftButtonUp(null, null);
            }
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                this.Hide();
                MainWindow.parentLeft = this.Left;
                MainWindow.parentTop = this.Top;
                new wAutomaticUpdate(this.ToString()).ShowDialog();
                this.Close();
            }
        }

        private void fOnOff_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                fOnOff.IsChecked = !fOnOff.IsChecked;
            }
        }

        private void Canvas_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                this.Hide();
                MainWindow.parentLeft = this.Left;
                MainWindow.parentTop = this.Top;
                new wInternetSettings(this.ToString()).ShowDialog();
                this.Close();
            }
        }

        private void sOnOff_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                sOnOff.IsChecked = !sOnOff.IsChecked;
            }
        }

        private void tOnOff_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                tOnOff.IsChecked = !tOnOff.IsChecked;
            }
        }

        private void Canvas_KeyDown_2(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                this.Hide();
                MainWindow.parentLeft = this.Left;
                MainWindow.parentTop = this.Top;
                new wReportSettings(this.ToString()).ShowDialog();
                this.Close();
            }
        }

        private void foOnOff_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                foOnOff.IsChecked = !foOnOff.IsChecked;
            }
        }

        private void btnDefault_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnDefault_Click(null, null);
            }
        }
    }
}
