using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WebRoamAV
{
    /// <summary>
    /// Interaction logic for wSafeBanking.xaml
    /// </summary>
    /// 

    public static partial class Commands
    {
        public static readonly RoutedCommand btnsbDefaultPressed = new RoutedCommand();
        public static readonly RoutedCommand btnsbSaveChangesPressed = new RoutedCommand();
        public static readonly RoutedCommand btnsbCancelPressed = new RoutedCommand();
    }

    public partial class wSafeBanking : Window
    {
        string mw;
        public wSafeBanking(string parent)
        {
            InitializeComponent();
            mw = parent;
			this.Top = MainWindow.parentTop;
			this.Left = MainWindow.parentLeft;
            this.MouseDown += MainWindow_MouseDown;
        }
        ColorAnimation coloarn;
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


        void setDefault()
        {
            chkProtDNS.IsChecked = true;
            cmbDNS.Text = "8.8.8.8";
            mtbIP.Text = "  8.  8.  8.  8";
            cCtrl.IsChecked = cAlt.IsChecked = true;
            cmbAlphabet.Text = "A";
            r1.IsChecked = true;
            FromDesktop.IsChecked = FromSafe.IsChecked = false;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*  Storyboard sb = new Storyboard();

              /**ScaleTransform scale = new ScaleTransform(1.0, 1.0);
              mnw.RenderTransformOrigin = new Point(0.5, 0.5);
              mnw.RenderTransform = scale;*/


            /*  var ta = new ThicknessAnimation();
              ta.BeginTime = new TimeSpan(0);
              SetValue(Storyboard.TargetNameProperty, "Window1");
              ta.SetValue(Storyboard.TargetPropertyProperty, "Margin");
              Storyboard.SetTargetProperty(ta, new PropertyPath(MarginProperty));

              ta.From = new Thickness(0);
              ta.To = new Thickness(0, 0, 0, 482);
              ta.Duration = new Duration(TimeSpan.FromSeconds(3));

              sb.Children.Add(ta);
              sb.Begin(this);
              this.Close();*/
            //Dispatcher.BeginInvoke((Action)delegate ()
            //{
            try
            { 
                setDefault();

                string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
                IniFile inf = new IniFile(file_ini);

                //fill config of our system/AV with default values if the config file didn't exist
                if (File.Exists(file_ini))
                {  
                    if (inf.KeyExists("ProtectDNS", "WRSafeBanking"))
                        chkProtDNS.IsChecked = inf.Read("ProtectDNS", "WRSafeBanking").ToLower() == "True".ToLower();

                    if (inf.KeyExists("AllowClipboardFromDesktop", "WRSafeBanking"))
                        FromDesktop.IsChecked = inf.Read("AllowClipboardFromDesktop", "WRSafeBanking").ToLower() == "True".ToLower();

                    if (inf.KeyExists("AllowClipboardFromSafeBanking", "WRSafeBanking"))                                         
                        FromSafe.IsChecked = inf.Read("AllowClipboardFromSafeBanking", "WRSafeBanking").ToLower() == "True".ToLower();
                    

                    if (inf.KeyExists("Ctrl", "WRSafeBanking"))
                        cCtrl.IsChecked = inf.Read("Ctrl", "WRSafeBanking").ToLower() == "True".ToLower();

                    if (inf.KeyExists("Alt", "WRSafeBanking"))
                        cAlt.IsChecked = inf.Read("Alt", "WRSafeBanking").ToLower() == "True".ToLower();

                    if (inf.KeyExists("Shift", "WRSafeBanking"))
                        cShift.IsChecked = inf.Read("Shift", "WRSafeBanking").ToLower() == "True".ToLower();

                    if (inf.KeyExists("Win", "WRSafeBanking"))
                        cWin.IsChecked = inf.Read("Win", "WRSafeBanking").ToLower() == "True".ToLower();

                    if (inf.KeyExists("Alphabet", "WRSafeBanking"))
                        cmbAlphabet.Text = inf.Read("Alphabet", "WRSafeBanking");

                    if (inf.KeyExists("SecureDNS", "WRSafeBanking"))
                    {

                        string dip = inf.Read("SecureDNS", "WRSafeBanking");
                        if (cmbDNS.Items.Contains(dip))
                        {
                            cmbDNS.Text = dip;
                            r1.IsChecked = true;
                        }
                        else
                        {
                            r2.IsChecked = true;
                            mtbIP.Text = dip;
                        }
                    }

                }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
            // });
        }

        private void ButtonEsc_Click(object sender, System.Windows.Input.KeyEventArgs e){if(e.Key == Key.Escape){Button_Click(null, null);}} private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(true)//if ((Mouse.GetPosition(ButtonMn).X < ButtonMn.Width) && (Mouse.GetPosition(ButtonMn).X > 0) && (Mouse.GetPosition(ButtonMn).Y < ButtonMn.Height) && (Mouse.GetPosition(ButtonMn).Y > 0))
            {
                /*MainWindow mw = new MainWindow();
                mw.Left = this.Left;
                mw.Top = this.Top;
                mw.WindowStartupLocation = WindowStartupLocation.Manual;
                mw.Show(); this.Close();*/
                Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType((mw.StartsWith("WebRoamAV.") ? "" : "WebRoamAV.") + mw.ToString().Replace(".xaml", ""));
                MainWindow.parentTop = this.Top;	
				MainWindow.parentLeft = this.Left;	
				t.GetMethod("Show").Invoke(Activator.CreateInstance(t, this.ToString()), new object[] { });
                this.Close();
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
            System.Windows.Controls.ContextMenu contextMenu = (System.Windows.Controls.ContextMenu)this.link4.Resources["link41"];
            contextMenu.PlacementTarget = this.tlink4;
            contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
            contextMenu.IsOpen = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem mi = e.OriginalSource as System.Windows.Controls.MenuItem;
           // System.Windows.MessageBox.Show(mi.ToString());
        }
        public void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void btnDefault_Click(object sender, RoutedEventArgs e)
        {
            setDefault();
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

        private void Default_MouseLeave_1(object sender, System.Windows.Input.MouseEventArgs e)
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

        private void Default_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnDefault.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }
  

        public void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
            string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
            IniFile inf = new IniFile(file_ini);

            //fill config of our system/AV with default values if the config file didn't exist
            if (File.Exists(file_ini))
            {                
                inf.Write("ProtectDNS", chkProtDNS.IsChecked==true?"True":"False", "WRSafeBanking");
                inf.Write("AllowClipboardFromDesktop", FromDesktop.IsChecked == true ? "True" : "False", "WRSafeBanking");
                inf.Write("AllowClipboardFromSafeBanking", FromSafe.IsChecked == true ? "True" : "False", "WRSafeBanking");
               
                inf.Write("Ctrl", cCtrl.IsChecked == true ? "True" : "False", "WRSafeBanking");
                inf.Write("Alt", cAlt.IsChecked == true ? "True" : "False", "WRSafeBanking");
                inf.Write("Shift", cShift.IsChecked == true ? "True" : "False", "WRSafeBanking");
                inf.Write("Win", cWin.IsChecked == true ? "True" : "False", "WRSafeBanking");
                inf.Write("Alphabet", cmbAlphabet.Text, "WRSafeBanking");

                inf.Write("SecureDNS", r1.IsChecked == true ? cmbDNS.Text : mtbIP.Text, "WRSafeBanking");
                Button_Click(null, null);
            }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void SaveChanges_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnSaveChanges.RenderTransformOrigin = new Point(0.5, 0.5);
            btnSaveChanges.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnSaveChanges);
            Storyboard.SetTarget(growAnimation2, btnSaveChanges);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void SaveChanges_MouseLeave_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnSaveChanges.RenderTransformOrigin = new Point(0.5, 0.5);
                btnSaveChanges.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnSaveChanges);
                Storyboard.SetTarget(growAnimation2, btnSaveChanges);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnSaveChanges.Background = new SolidColorBrush(Colors.Green);
        }

        private void SaveChanges_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnSaveChanges.RenderTransformOrigin = new Point(0.5, 0.5);
            btnSaveChanges.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnSaveChanges);
            Storyboard.SetTarget(growAnimation2, btnSaveChanges);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnSaveChanges_Click(null, null);
        }

        private void SaveChanges_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnSaveChanges.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }


        private void Cancel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnCancel.RenderTransformOrigin = new Point(0.5, 0.5);
            btnCancel.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnCancel);
            Storyboard.SetTarget(growAnimation2, btnCancel);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void Cancel_MouseLeave_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnCancel.RenderTransformOrigin = new Point(0.5, 0.5);
                btnCancel.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnCancel);
                Storyboard.SetTarget(growAnimation2, btnCancel);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnCancel.Background = new SolidColorBrush(Colors.Green);
        }

        private void Cancel_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnCancel.RenderTransformOrigin = new Point(0.5, 0.5);
            btnCancel.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnCancel);
            Storyboard.SetTarget(growAnimation2, btnCancel);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnCancel_Click(null, null);
        }

        private void Cancel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnCancel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }
        private void Coloarn_Completed(object sender, EventArgs e)
        {
            /*btnDeleteAll.Background = new SolidColorBrush(Colors.Green);
            coloarn = new ColorAnimation();
            coloarn.From = Colors.Green;
            coloarn.To = (Color)ColorConverter.ConvertFromString("#A0008000");
            coloarn.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            coloarn.Completed -= Coloarn_Completed;*/
        }
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            System.Windows.Controls.Control target = e.Source as System.Windows.Controls.Control;

            if (target != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = e.Source as System.Windows.Controls.Button;

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

        private void r1_Checked(object sender, RoutedEventArgs e)
        {
            cmbDNS.IsEnabled = true;
            mtbIP.Enabled = false;
        }
        private void r2_Checked(object sender, RoutedEventArgs e)
        {
            cmbDNS.IsEnabled = false;
            mtbIP.Enabled = true;
        }

        private void mtbIP_Leave(object sender, EventArgs e)
        {
            IPAddress iad;
            if(!IPAddress.TryParse(mtbIP.Text.Replace(" ", ""), out iad))
            {
//                System.Windows.MessageBox.Show("Bad IP address!!");
  //              mtbIP.Focus();
            }
        }
        string s = "";
        int pd = 0;
        private void mtbIP_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            int k = e.KeyValue;

            if (k == 110 || k == 190)
            {
                s = mtbIP.Text;
                // mtbIP.Select(mtbIP.SelectionStart + mtbIP.SelectionStart % 3+1,1);
                while (!mtbIP.SelectedText.StartsWith(".") && !mtbIP.SelectedText.StartsWith(" ") && mtbIP.Text.Substring(mtbIP.SelectionStart).Contains("."))
                {
                    if (mtbIP.Text.Substring(mtbIP.SelectionStart).Length > 0 && mtbIP.Text.Substring(mtbIP.SelectionStart)[0] == '.')
                    {
                        mtbIP.Select(mtbIP.SelectionStart + 1, 1);
                        break;
                    }
                        mtbIP.Select(mtbIP.SelectionStart + 1, 1);
                }
               /*  char ch = s.LastOrDefault(f => char.IsDigit(f));
                int dig = s.LastIndexOf(ch);
                pd += 3;
           
                if (dig > 0)
                {
                    pd = (s.Substring(0, dig)).LastIndexOf('.') + 4 + 1;
                }
                mtbIP.Select(pd, 1);*/
            }
            
        }

        private void mtbIP_GotFocus(object sender, EventArgs e)
        {

            mtbIP.Select(0,1);
        }

        private void chkProtDNS_Checked(object sender, RoutedEventArgs e)
        {
            r1.IsEnabled = r2.IsEnabled = true;
            if (r1.IsChecked==true)
                cmbDNS.IsEnabled = true;
            if(r2.IsChecked==true)
                mtbIP.Enabled = true;

        }

        private void chkProtDNS_Unchecked(object sender, RoutedEventArgs e)
        {
            r1.IsEnabled = r2.IsEnabled = false;
            mtbIP.Enabled = false;
            cmbDNS.IsEnabled = false;
        }
    }
}

