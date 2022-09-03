using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
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
    /// Interaction logic for wARProtection.xaml
    /// </summary>
    /// 
    public static partial class Commands
    {
        public static readonly RoutedCommand btnspDefaultPressed = new RoutedCommand();
        public static readonly RoutedCommand btnspSaveChangesPressed = new RoutedCommand();
        public static readonly RoutedCommand btnspCancelPressed = new RoutedCommand();
        public static readonly RoutedCommand btnAntiRansomewareSPressed = new RoutedCommand();
        public static readonly RoutedCommand btnBackupSettingsPressed = new RoutedCommand();
    }
    public partial class wARProtection : Window
    {
        string mw;
        public static readonly string ARFolder = Directory.GetParent(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName.TrimEnd('\\');
        public wARProtection(string parent)
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
            chkAl.IsChecked = true;
            r2.IsChecked = true;

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
            try
            {
                setDefault();
               
                IniFile inf = new IniFile($"{ARFolder}\\gui\\app_config.ini");
                
                string key = "AlertBlock";
                if (inf.KeyExists(key, "RANSOME"))
                {                    
                    chkAl.IsChecked = inf.Read(key, "RANSOME").ToLowerInvariant() == "true";
                }
                key = "SecurityLevel";
                if (inf.KeyExists(key))
                {
                    r1.IsChecked = inf.Read(key).ToLowerInvariant() == "3";
                }
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
            ContextMenu contextMenu = (ContextMenu)this.link4.Resources["link41"];
            contextMenu.PlacementTarget = this.tlink4;
            contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
            contextMenu.IsOpen = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.OriginalSource as MenuItem;
         //   MessageBox.Show(mi.ToString());
        }
        public void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void btnDefault_Click(object sender, RoutedEventArgs e)
        {
            chkAl.IsChecked = true;
            r2.IsChecked = true;
           // btnSaveChanges_Click(null, null);
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

      
        public void btnPExEx_Click(object sender, RoutedEventArgs e)
        {

           // System.Windows.MessageBox.Show("2");
        }

       
        public void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string[] lines = File.ReadAllLines($"{ARFolder}\\gui\\app_config.ini");
            int i = 0;
            int selec = 0;
            if (r1.IsChecked==true)
                selec = 1;
            else if (r2.IsChecked==true)
                selec = 2;
            else if (r3.IsChecked==true)
                selec = 3;
            foreach (var l in lines)
            {
                if (l.Contains("SecurityLevel"))
                {
                    lines[i] = "SecurityLevel=" + selec.ToString();
                    break;
                }
                i++;
            }
            File.WriteAllLines($"{ARFolder}\\gui\\app_config.ini", lines, Encoding.Unicode);

            string[] lines2 = File.ReadAllLines($"{ARFolder}\\gui\\app_config.ini");
            int i2 = 0;
            foreach (var l in lines2)
            {
                if (l.Contains("AlertBlock"))
                {
                    lines2[i2] = "AlertBlock=" + (chkAl.IsChecked==true ? "1" : "0");
                    break;
                }
                i2++;
            }
            File.WriteAllLines($"{ARFolder}\\gui\\app_config.ini", lines2, Encoding.Unicode);
            Button_Click(null, null);
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
        }
        private void r2_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void r3_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnAntiRansomewareS_MouseEnter(object sender, MouseEventArgs e)
        {
            btnAntiRansomewareS.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }

        private void btnAntiRansomewareS_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnAntiRansomewareS.RenderTransformOrigin = new Point(0.5, 0.5);
                btnAntiRansomewareS.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnAntiRansomewareS);
                Storyboard.SetTarget(growAnimation2, btnAntiRansomewareS);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnAntiRansomewareS.Background = new SolidColorBrush(Colors.Green);
        }

        private void btnAntiRansomewareS_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnAntiRansomewareS.RenderTransformOrigin = new Point(0.5, 0.5);
            btnAntiRansomewareS.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnAntiRansomewareS);
            Storyboard.SetTarget(growAnimation2, btnAntiRansomewareS);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void btnAntiRansomewareS_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnAntiRansomewareS.RenderTransformOrigin = new Point(0.5, 0.5);
            btnAntiRansomewareS.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnAntiRansomewareS);
            Storyboard.SetTarget(growAnimation2, btnAntiRansomewareS);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnAntiRansomewareS_Click(null, null);
        }

        private void btnAntiRansomewareS_Click(object p1, object p2)
        {
            new ARSettings().ShowDialog();
        }

        private void btnBackupSetting_MouseEnter(object sender, MouseEventArgs e)
        {
            btnBackupSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }

        private void btnBackupSetting_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnBackupSettings.RenderTransformOrigin = new Point(0.5, 0.5);
                btnBackupSettings.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnBackupSettings);
                Storyboard.SetTarget(growAnimation2, btnBackupSettings);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnBackupSettings.Background = new SolidColorBrush(Colors.Green);
        }

        private void btnBackupSetting_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnBackupSettings.RenderTransformOrigin = new Point(0.5, 0.5);
            btnBackupSettings.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnBackupSettings);
            Storyboard.SetTarget(growAnimation2, btnBackupSettings);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void btnBackupSetting_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnBackupSettings.RenderTransformOrigin = new Point(0.5, 0.5);
            btnBackupSettings.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnBackupSettings);
            Storyboard.SetTarget(growAnimation2, btnBackupSettings);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnBackupSetting_Click(null, null);
        }

        private void btnBackupSetting_Click(object p1, object p2)
        {
            var tc = new TcpClient();
            byte[] buff = Encoding.ASCII.GetBytes("wrMainAntiRansomeware.bkupSetting");
            tc.ConnectAsync("localhost", 2900).ContinueWith(
                delegate {
                    var st = tc.GetStream();
                    st.WriteAsync(buff, 0, buff.Length);
                    st.FlushAsync().ContinueWith(delegate {
                        tc.Close();
                    });
                });

        }

        private void buttonClose_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                this.Close();
            }
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
            if(e.Key== Key.Enter)
            {
                this.WindowState = WindowState.Minimized;

            }
        }

        private void btnAntiRansomewareS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnAntiRansomewareS_Click(null, null);
            }
        }

        private void btnBackupSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnBackupSetting_Click(null, null);

            }
        }

        private void btnDefault_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                chkAl.IsChecked = true;
                r2.IsChecked = true;
            }
        }

        private void btnSaveChanges_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnSaveChanges_Click(null, null);
            }
        }

        private void btnCancel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                this.Close();
            }
        }
    }
}

