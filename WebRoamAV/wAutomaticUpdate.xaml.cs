using System;
using System.Collections.Generic;
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
    /// Interaction logic for wAutomaticUpdate.xaml
    /// </summary>
    /// 

    public static partial class Commands
    {
        public static readonly RoutedCommand btnuDefaultPressed = new RoutedCommand();
        public static readonly RoutedCommand btnuSaveChangesPressed = new RoutedCommand();
        public static readonly RoutedCommand btnuCancelPressed = new RoutedCommand();
        public static readonly RoutedCommand btnuBrs1Pressed = new RoutedCommand();
        public static readonly RoutedCommand btnuBrs2Pressed = new RoutedCommand();
    }
    public partial class wAutomaticUpdate : Window
    {
        string mw;
        public wAutomaticUpdate(string parent)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnDefault_Click(null, null);
            string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
            IniFile inf = new IniFile(file_ini);
            if (File.Exists(file_ini))
            {
                if (inf.KeyExists("AU_PICK_PATH", "AUTOUPDATE"))
                {
                    txtBrs1.Text = inf.Read("AU_PICK_PATH", "AUTOUPDATE");
                    r2.IsChecked = true;
                }
           
                if (inf.KeyExists("AU_SAVE_PATH", "AUTOUPDATE"))
                {
                    txtBrs2.Text = inf.Read("AU_SAVE_PATH", "AUTOUPDATE");
                    ckUp.IsChecked = true;
                }
                else
                {
                    ckUp.IsChecked = false;
                }
                if (inf.KeyExists("AU_SHOW_NOTIF", "AUTOUPDATE"))
                {
                    ckSho.IsChecked = inf.Read("AU_SHOW_NOTIF", "AUTOUPDATE").ToLower() == "true";
                }
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
            //MessageBox.Show(mi.ToString());
        }
        public void btnBtnBrs1_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                var dir = dlg.SelectedPath;
                r2.IsChecked = true;
                txtBrs1.Text = dir;

               // string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
                //IniFile inf = new IniFile(file_ini);

                //fill config of our system/AV with default values if the config file didn't exist
              
            }
        }

        private void BtnBrs1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnBtnBrs1.RenderTransformOrigin = new Point(0.5, 0.5);
            btnBtnBrs1.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnBtnBrs1);
            Storyboard.SetTarget(growAnimation2, btnBtnBrs1);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void BtnBrs1_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnBtnBrs1.RenderTransformOrigin = new Point(0.5, 0.5);
                btnBtnBrs1.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnBtnBrs1);
                Storyboard.SetTarget(growAnimation2, btnBtnBrs1);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnBtnBrs1.Background = new SolidColorBrush(Colors.Green);
        }

        private void BtnBrs1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnBtnBrs1.RenderTransformOrigin = new Point(0.5, 0.5);
            btnBtnBrs1.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnBtnBrs1);
            Storyboard.SetTarget(growAnimation2, btnBtnBrs1);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnBtnBrs1_Click(null, null);
        }

        private void BtnBrs1_MouseEnter(object sender, MouseEventArgs e)
        {
            btnBtnBrs1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }
        public void btnBtnBrs2_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                var dir = dlg.SelectedPath;
                ckUp.IsChecked = true;
                txtBrs2.Text = dir;
              //  string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
                //IniFile inf = new IniFile(file_ini);

                //fill config of our system/AV with default values if the config file didn't exist
                
            }
        
        }

        private void BtnBrs2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnBtnBrs2.RenderTransformOrigin = new Point(0.5, 0.5);
            btnBtnBrs2.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnBtnBrs2);
            Storyboard.SetTarget(growAnimation2, btnBtnBrs2);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void BtnBrs2_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnBtnBrs2.RenderTransformOrigin = new Point(0.5, 0.5);
                btnBtnBrs2.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnBtnBrs2);
                Storyboard.SetTarget(growAnimation2, btnBtnBrs2);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnBtnBrs2.Background = new SolidColorBrush(Colors.Green);
        }

        private void BtnBrs2_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnBtnBrs2.RenderTransformOrigin = new Point(0.5, 0.5);
            btnBtnBrs2.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnBtnBrs2);
            Storyboard.SetTarget(growAnimation2, btnBtnBrs2);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnBtnBrs2_Click(null, null);
        }

        private void BtnBrs2_MouseEnter(object sender, MouseEventArgs e)
        {
            btnBtnBrs2.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }
        private void r1_Checked(object sender, RoutedEventArgs e)
        {
            txtBrs1.IsReadOnly = true;
            txtBrs1.IsEnabled = false;
            txtBrs1.IsTabStop = false;
            btnBtnBrs1.IsEnabled = false;
            btnBtnBrs1.Opacity = 0.5;
            txtBrs1.Opacity = 0.5;
        }
        private void r2_Checked(object sender, RoutedEventArgs e)
        {
            txtBrs1.IsReadOnly = false;
            txtBrs1.IsEnabled = true;
            txtBrs1.IsTabStop = true;
            btnBtnBrs1.IsEnabled = true;
            btnBtnBrs1.Opacity = 1;
            txtBrs1.Opacity = 1;

        }

        public void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        public void btnDefault_Click(object sender, RoutedEventArgs e)
        {
            ckSho.IsChecked = true;
            ckUp.IsChecked = true;
            r1.IsChecked = true;
            txtBrs1.Text = "";
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms) + "\\Webroam";
            if(!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
            txtBrs2.Text = dir;
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

        public void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string file_ini = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\webroam_conf.ini";
            IniFile inf = new IniFile(file_ini);

            //fill config of our system/AV with default values if the config file didn't exist
            if (File.Exists(file_ini))
            {
                if (inf.KeyExists("AU_SHOW_NOTIF", "AUTOUPDATE"))
                {
                    inf.DeleteKey("AU_SHOW_NOTIF", "AUTOUPDATE");
                }
                inf.Write("AU_SHOW_NOTIF", ckSho.IsChecked==true?"TRUE":"FALSE", "AUTOUPDATE");
              
                    if (inf.KeyExists("AU_PICK_PATH", "AUTOUPDATE"))
                    {
                        inf.DeleteKey("AU_PICK_PATH", "AUTOUPDATE");
                    }
                    if(r2.IsChecked == true)
                    inf.Write("AU_PICK_PATH", txtBrs1.Text, "AUTOUPDATE");
                if (inf.KeyExists("AU_SAVE_PATH", "AUTOUPDATE"))
                {
                    inf.DeleteKey("AU_SAVE_PATH", "AUTOUPDATE");
                }
                if(ckUp.IsChecked == true)
                inf.Write("AU_SAVE_PATH", txtBrs2.Text, "AUTOUPDATE");
            }
            App.AppSettings.AU_PICK_PATH = App.AppSettings.AU_SAVE_PATH = "";
            if (inf.KeyExists("AU_PICK_PATH", "AUTOUPDATE"))
            {
                App.AppSettings.AU_PICK_PATH = inf.Read("AU_PICK_PATH", "AUTOUPDATE");
            }

            if (inf.KeyExists("AU_SAVE_PATH", "AUTOUPDATE"))
            {
                App.AppSettings.AU_SAVE_PATH = inf.Read("AU_SAVE_PATH", "AUTOUPDATE");
            }

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

        private void SaveChanges_MouseLeave_1(object sender, MouseEventArgs e)
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

        private void SaveChanges_MouseEnter(object sender, MouseEventArgs e)
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

        private void Cancel_MouseLeave_1(object sender, MouseEventArgs e)
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

        private void Cancel_MouseEnter(object sender, MouseEventArgs e)
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

        private void ckUp_Checked(object sender, RoutedEventArgs e)
        {
            txtBrs2.IsReadOnly = false;
            txtBrs2.IsTabStop = true;
            txtBrs2.Opacity = 1;
            btnBtnBrs2.IsEnabled = true;
            btnBtnBrs2.IsTabStop = true;
            btnBtnBrs2.Opacity = 1;
           
        }

        private void ckUp_Unchecked(object sender, RoutedEventArgs e)
        {
            txtBrs2.IsReadOnly = true;
            txtBrs2.IsEnabled = false;
            txtBrs2.IsTabStop = false;
            txtBrs2.Opacity = 0.5;
            btnBtnBrs2.Opacity = 0.5;
            btnBtnBrs2.IsEnabled = false;
            
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

        private void ButtonMn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                Button_Click(null, null);
            }
        }

        private void btnBtnBrs1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnBtnBrs1_Click(null, null);
            }
        }

        private void btnBtnBrs2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnBtnBrs2_Click(null, null);
            }
        }

        private void btnDefault_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnDefault_Click(null, null);
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
                btnCancel_Click(null, null);
            }
        }
    }
}
