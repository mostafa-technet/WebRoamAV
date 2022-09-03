using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WebRoamAV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static double parentTop = 0, parentLeft = 0;
        string mw;
       // ColorAnimation coloarn;
        public MainWindow():
            this(null)
        {
        }
        public MainWindow(string parent)
        {
            InitializeComponent();
            try
            { 
            mw = parent;            
			this.Top = MainWindow.parentTop;
			this.Left = MainWindow.parentLeft;
            if (parent == null)
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            this.MouseDown += MainWindow_MouseDown;
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName()+" "+new StackFrame(1, true).GetFileLineNumber()+Environment.NewLine+em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        public void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left &&  e.GetPosition(this).Y < 70)
                  this.DragMove();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            /*if (rect.Visibility == System.Windows.Visibility.Collapsed)
            {
                rect.Visibility = System.Windows.Visibility.Visible;
                (sender as Button).Content = "<";
            }
            else
            {
                rect.Visibility = System.Windows.Visibility.Collapsed;
                (sender as Button).Content = ">";
            }*/
        }

        public void buttonMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void image_MouseEnter(object sender, MouseEventArgs e)
        {
            image.Source = new BitmapImage(new Uri(@"Pictures\Untitled-32.png", UriKind.RelativeOrAbsolute));
        }

        public void image_MouseLeave(object sender, MouseEventArgs e)
        {
            image.Source = new BitmapImage(new Uri(@"Pictures\Untitled-33.png", UriKind.RelativeOrAbsolute));
        }

        public void image_Copy_MouseLeave(object sender, MouseEventArgs e)
        {
            image_Copy.Source = new BitmapImage(new Uri(@"Pictures\Untitled-31.png", UriKind.RelativeOrAbsolute));
        }

        public void image_Copy_MouseEnter(object sender, MouseEventArgs e)
        {
            image_Copy.Source = new BitmapImage(new Uri(@"Pictures\Untitled-30.png", UriKind.RelativeOrAbsolute));
        }

        public void image_Copy1_MouseEnter(object sender, MouseEventArgs e)
        {
            image_Copy1.Source = new BitmapImage(new Uri(@"Pictures\Untitled-28.png", UriKind.RelativeOrAbsolute));
        }

        public void image_Copy1_MouseLeave(object sender, MouseEventArgs e)
        {
            image_Copy1.Source = new BitmapImage(new Uri(@"Pictures\Untitled-29.png", UriKind.RelativeOrAbsolute));
        }

        public void image_Copy2_MouseEnter(object sender, MouseEventArgs e)
        {
            image_Copy2.Source = new BitmapImage(new Uri(@"Pictures\Untitled-20.png", UriKind.RelativeOrAbsolute));
        }

        public void image_Copy2_MouseLeave(object sender, MouseEventArgs e)
        {
            image_Copy2.Source = new BitmapImage(new Uri(@"Pictures\Untitled-21.png", UriKind.RelativeOrAbsolute));
        }

        public unsafe void image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        
        public unsafe void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           

        }
        

        private void image_Copy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
          
        }

        private void image_Copy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            InNets inn = new InNets(this.ToString());
            this.Hide();
            inn.Left = this.Left;
            inn.Top = this.Top;
            inn.ShowDialog();
            this.Close();

        }

        Image image_Copy3;
        Storyboard sb = null;
        private void image_Copy1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            wARProtection inn = new wARProtection(this.ToString());
            this.Hide();
            inn.Left = this.Left;
            inn.Top = this.Top;
            inn.ShowDialog();
            this.Close();
        }

        private void Image_Copy3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        void AnimateMyUIObject(UIElement element, Thickness from, Thickness to)
        {
           
            var ta = new ThicknessAnimation();
            ta.BeginTime = new TimeSpan(0);
            ta.SetValue(Storyboard.TargetProperty, element);
            Storyboard.SetTargetProperty(ta, new PropertyPath(MarginProperty));
            ta.DecelerationRatio = 0.1;
            ta.From = from;
            ta.To = to;
            ta.Duration = new Duration(TimeSpan.FromSeconds(0.9));
            ta.DecelerationRatio = 0.85;
            ta.AccelerationRatio = 0;
            ta.SpeedRatio = 1;
            sb.Children.Add(ta);
            

        }

        private void image_Copy3_MouseEnter(object sender, MouseEventArgs e)
        {
            if (image_Copy3 != null)
                image_Copy3.Source = new BitmapImage(new Uri(@"Pictures\Untitled-34.png", UriKind.RelativeOrAbsolute));
        }

        private void image_Copy3_MouseLeave(object sender, MouseEventArgs e)
        {
            if (image_Copy3 != null)
                image_Copy3.Source = new BitmapImage(new Uri(@"Pictures\Untitled-35.png", UriKind.RelativeOrAbsolute));
        }

        public static List<GRowsCUS> mfiles = null;
        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try { 
            MenuItem mi = e.OriginalSource as MenuItem;
            if (mi.Name == "m1")
            {
                int count = SqlReaderWriter.MaxofRow("tblReportFor");
                int rpfID = 2;
                int iDate = Int32.Parse(string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                string sTime = DateTime.Now.ToLongTimeString();
                string repfor = "Full Scan";
                SqlReaderWriter.ExecuteQuery("INSERT INTO tblReportFor (ID, ReportFID, Date, Time, ReportFor) VALUES (" + (count + 100000 * rpfID) + ", " + rpfID + ", " + iDate + ", '" + sTime + "', '" + repfor + "')");
                
                MainWindow.parentLeft = this.Left;
                MainWindow.parentTop = this.Top;
                new wndFullScan(this.ToString(), true, ref mfiles).ShowDialog();
            }
            else if (mi.Name == "m2")
            {
                wndCustomScan inn = new wndCustomScan(this.ToString());
                this.Hide();
                inn.Left = this.Left;
                inn.Top = this.Top;
                inn.ShowDialog();
                this.Close();
            }
            else if (mi.Name == "m3")
            {
                var wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
                var searcher = new ManagementObjectSearcher(wmiQueryString);
               var results = searcher.Get();


                   
                    int count = SqlReaderWriter.MaxofRow("tblReportFor");
                    int rpfID = 3;
                    int iDate = Int32.Parse(string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                    string sTime = DateTime.Now.ToLongTimeString();
                    string repfor = "Scan Memory";
                    SqlReaderWriter.ExecuteQuery("INSERT INTO tblReportFor (ID, ReportFID, Date, Time, ReportFor) VALUES (" + (count + 100000 * rpfID) + ", " + rpfID + ", " + iDate + ", '" + sTime + "', '" + repfor + "\n')");
                    this.Hide();
                    MainWindow.parentLeft = this.Left;
                    MainWindow.parentTop = this.Top;
                    wndFullScan wnd=new wndFullScan("", true);
                    wnd.Show();
                    if (mfiles == null)
                    {
                        await Task.Run((Action)delegate ()
                        {
                            mfiles = new List<GRowsCUS>();
                            bool isex = false;
                            foreach (var mo in results)
                            {
                                try
                                {
                                    if (mo["ExecutablePath"] != null && File.Exists((string)mo["ExecutablePath"]) && Directory.GetParent(mo["ExecutablePath"].ToString()).FullName != System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().FullName))
                                    {
                                        mfiles.Add(new GRowsCUS { PathF = (string)mo["ExecutablePath"], SubF = "No" });
                                        Process prc;
                                        try
                                        {
                                            prc = Process.GetProcessById(Int32.Parse(mo["ProcessId"].ToString()));
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                        try
                                        {
                                            if (isex)
                                                continue;
                                            var ofiles = Handle.getOpenFileNames(prc);
                                            if(ofiles!=null)
                                            foreach (var v in ofiles)
                                                mfiles.Add(new GRowsCUS { PathF = v, SubF = "No" });
                                            ofiles.Clear();
                                            ofiles = null;
                                        }
                                        catch (Exception em)
                                        {
                                            ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                                            isex = true;
                                            //  MessageBox.Show(ex.ToString());
                                            // return;
                                        }
                                    }
                                }
                                catch (Exception em)
                                {
                                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);

                                //  MessageBox.Show(ex.ToString());
                                // return;
                            }
                            }
                        });
                    }
                        wnd.Hide();
                    // MessageBox.Show(mfiles.Count.ToString());
                    //mfiles.Remove(mfiles.Find(f=> System.IO.Path.GetFileName(f.PathF)== "lsass.exe"));
                  //  MessageBox.Show(mfiles.Count.ToString());
                        wnd = new wndFullScan("",false, ref mfiles);
                        wnd.ShowDialog();                   
                    mfiles.Clear();
                    mfiles = null;
                    GC.Collect();
                   // GC.WaitForFullGCComplete();
                    wnd.Close();

            }
            else if (mi.Name == "m4")
            {
                    new bootscanmode().ShowDialog();
                   
                }
            else if (mi.Name == "m5")
            {
                MainWindow.parentLeft = this.Left;
                MainWindow.parentTop = this.Top;
                new frmVlApplication().ShowDialog();
            }
            else if (mi.Name == "m6")
            {
               
            }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName()+" "+new StackFrame(1, true).GetFileLineNumber()+Environment.NewLine+em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void image_Scan_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Image image = sender as Image;
                ContextMenu contextMenu = (ContextMenu)this.image_Scan.Resources["scanContext"];
                contextMenu.PlacementTarget = image;
                contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
                contextMenu.IsOpen = true;
            }
        }

        private void image_Scan_Copy_MouseEnter(object sender, MouseEventArgs e)
        {
            if (image_Scan_Copy != null)
                image_Scan_Copy.Source = new BitmapImage(new Uri(@"Pictures\Untitled-18.png", UriKind.RelativeOrAbsolute));
        }

        private void image_Scan_Copy_MouseLeave(object sender, MouseEventArgs e)
        {
            if (image_Scan_Copy != null)
                image_Scan_Copy.Source = new BitmapImage(new Uri(@"Pictures\Untitled-8.png", UriKind.RelativeOrAbsolute));
        }

        private void image_Scan_MouseLeave(object sender, MouseEventArgs e)
        {
            if (image_Scan != null)
                image_Scan.Source = new BitmapImage(new Uri(@"Pictures\Untitled-9.png", UriKind.RelativeOrAbsolute));
        }

        private void image_Scan_MouseEnter(object sender, MouseEventArgs e)
        {
            if (image_Scan != null)
                image_Scan.Source = new BitmapImage(new Uri(@"Pictures\Untitled-19.png", UriKind.RelativeOrAbsolute));
        }
        private void link_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try { 
            Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType(e.Uri.ToString().StartsWith("WebRoamAV.") ? "" : "WebRoamAV." + e.Uri.ToString().Replace(".xaml", ""));

            MainWindow.parentTop = this.Top;	
				MainWindow.parentLeft = this.Left;	
				t.GetMethod("Show").Invoke(Activator.CreateInstance(t, this.ToString()), new object[] { });           
            this.Close();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName()+" "+new StackFrame(1, true).GetFileLineNumber()+Environment.NewLine+em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        private void link4_Click(object sender, RoutedEventArgs e)
        {

            Hyperlink h = sender as Hyperlink;
            ContextMenu contextMenu = (ContextMenu)this.link4.Resources["link41"];
            contextMenu.PlacementTarget = this.tlink4;
            contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
            contextMenu.IsOpen = true;
        }

        private void GoRight_Click(object sender, RoutedEventArgs e)
        {
            if (Grid1.Children.IndexOf(image_Copy2) == -1)
                return;
            if (sb != null)
                sb.Stop();
            sb = new Storyboard();


            double w1 = image_Copy1.Width;
            double w2 = image_Copy.Width;


            image_Copy3 = new Image();
            image_Copy3.Source = new BitmapImage(new Uri(@"Pictures\Untitled-35.png", UriKind.RelativeOrAbsolute));
            image_Copy3.MouseEnter += image_Copy3_MouseEnter;
            image_Copy3.MouseLeave += image_Copy3_MouseLeave;
            image_Copy3.MouseLeftButtonDown += Image_Copy3_MouseLeftButtonDown;
            image_Copy3.KeyDown += Image_Copy3_KeyDown;
            image_Copy3.Focusable = true;
            KeyboardNavigation.SetTabIndex(image_Copy3, 12);
            KeyboardNavigation.SetIsTabStop(image_Copy3, true);
            Grid.SetColumn(image_Copy3, 2);
            Grid.SetRow(image_Copy3, 1);

            Grid.SetColumnSpan(image_Copy1, 2);
            Grid.SetColumnSpan(image_Copy, 1);

            Grid.SetColumn(image_Copy1, 1);
            double w3 = image.Width;
            image_Copy3.Margin = image_Copy1.Margin;
            image_Copy3.Width = w3;
            image_Copy3.Height = 143;
            image_Copy3.VerticalAlignment = VerticalAlignment.Top;
            image_Copy3.HorizontalAlignment = HorizontalAlignment.Left;
            image_Copy3.Cursor = Cursors.Hand;
            image_Copy3.Visibility = Visibility.Visible;

            double dw1 = image_Copy2.Width;
            image_Copy3.MouseLeftButtonUp += Image_Copy3_MouseLeftButtonUp;

            Grid1.Children.Add(image_Copy3);
            ((FrameworkElement)Grid1.Children[Grid1.Children.Count - 1]).Name = "image_Copy3";


            Grid.SetColumn(image_Copy3, Grid.GetColumn(image));
            Grid.SetColumnSpan(image_Copy3, Grid.GetColumnSpan(image));
            AnimateMyUIObject(image_Copy3, new Thickness(163 * 4, image.Margin.Top, 0, image.Margin.Bottom), image.Margin);

            image.Width = w2;
            image_Copy.Width = w1;

            image_Copy1.Width = dw1;

            IEnumerable<Image> uicol = from uic in Grid1.Children.OfType<Image>() where uic.Name.StartsWith("image") orderby uic.Name ascending select uic;
            IEnumerable<Image> uicol2 = from uic in Grid1.Children.OfType<Image>() where uic.Name.StartsWith("image") orderby uic.Name ascending select uic;
            int i = 0;
            foreach (var ui in uicol)
            {
                i++;
                if (i < 4)
                {
                    AnimateMyUIObject(ui, ui.Margin, uicol2.ElementAt<Image>(i).Margin);
                }
            }

            Grid1.Children.Remove(image_Copy2);
           
            sb.Completed += delegate 
            {
                elLeft.Fill = new SolidColorBrush(Colors.White);
                elRight.Fill = new SolidColorBrush(Colors.Green);
            };
            GoLeft.IsEnabled = true;
            GoRight.IsEnabled = false;
            LEc.Stroke = new SolidColorBrush(Colors.Green);
            LTb.Foreground = new SolidColorBrush(Colors.Green);
            REc.Stroke = new SolidColorBrush(Colors.Gray);
            RTb.Foreground = new SolidColorBrush(Colors.Gray);
            sb.Begin();
            /*  image.Margin = tc2;            
              image_Copy.Margin = tc1; */






            //this.Content = scrollviewer1.Content;
            /* wnd1 = new Window1();
            wnd1.Top = this.Top;
            wnd1.Left = this.Left;
            wnd1.Show();
            Task.Factory.StartNew(() => {
                System.Threading.Thread.Sleep(400);
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    this.Close();
                }));                
            });*/

        }

        private void Image_Copy3_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Image_Copy3_MouseLeftButtonUp(null, null);
            }
        }

        private void Image_Copy3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var edv = new EDDevices(this.ToString());
            this.Hide();
            edv.Left = this.Left;
            edv.Top = this.Top;
            edv.ShowDialog();
            this.Close();
        }

        private void GoLeft_Click(object sender, RoutedEventArgs e)
        {
            if (Grid1.Children.IndexOf(image_Copy3) == -1)
                return;
            if (sb != null)
                sb.Stop();
            sb = new Storyboard();


            double dw1 = image_Copy.Width;
            Grid.SetColumn(image_Copy1, 2);
            Grid1.Children.Remove(image_Copy3);
            Grid1.Children.Add(image_Copy2);
            ((FrameworkElement)Grid1.Children[Grid1.Children.Count - 1]).Name = "image_Copy2";

            image_Copy2.Margin = new Thickness(-163, image.Margin.Top, 0, image.Margin.Bottom);
            AnimateMyUIObject(image_Copy2, image_Copy2.Margin, image_Copy1.Margin);



            IEnumerable<Image> uicol = from uic in Grid1.Children.OfType<Image>() where uic.Name.StartsWith("image") orderby uic.Name descending select uic;
            IEnumerable<Image> uicol2 = from uic in Grid1.Children.OfType<Image>() where uic.Name.StartsWith("image") orderby uic.Name descending select uic;
            int i = 0;
            foreach (var ui in uicol)
            {

                i++;
                if (i <= 5 && i >= 1)
                {
                    if (ui.Name.IndexOf("Scan") != -1)
                        continue;
                    ui.Width = uicol2.ElementAt<Image>(i).Width;
                    AnimateMyUIObject(ui, ui.Margin, uicol2.ElementAt<Image>(i).Margin);
                }
            }
            image.Width = image_Copy3.Width;
            AnimateMyUIObject(image, image.Margin, image_Copy3.Margin);

            sb.Completed += delegate
            {
                elLeft.Fill = new SolidColorBrush(Colors.Green);
                elRight.Fill = new SolidColorBrush(Colors.White);
            };
            GoLeft.IsEnabled = false;
            GoRight.IsEnabled = true;
            REc.Stroke = new SolidColorBrush(Colors.Green);
            RTb.Foreground = new SolidColorBrush(Colors.Green);
            LEc.Stroke = new SolidColorBrush(Colors.Gray);
            LTb.Foreground = new SolidColorBrush(Colors.Gray);

            sb.Begin();
        }

        private void btnRegisterNow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnRegisterNow.RenderTransformOrigin = new Point(0.5, 0.5);
            btnRegisterNow.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnRegisterNow);
            Storyboard.SetTarget(growAnimation2, btnRegisterNow);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void btnRegisterNow_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnRegisterNow.RenderTransformOrigin = new Point(0.5, 0.5);
            btnRegisterNow.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnRegisterNow);
            Storyboard.SetTarget(growAnimation2, btnRegisterNow);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnRegisterNow_Click(null, null);
        }

        private void btnRegisterNow_Click(object p1, object p2)
        {
            // new pInternetSchedule().Show();
            Process.Start("..\\gui\\wrMainAntiRansomeware.exe", "wrMainAntiRansomeware.ProductKeyForm");
        }

        private void btnRegisterNow_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnRegisterNow.RenderTransformOrigin = new Point(0.5, 0.5);
                btnRegisterNow.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnRegisterNow);
                Storyboard.SetTarget(growAnimation2, btnRegisterNow);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnRegisterNow.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF726B6B"));
        }

        private void btnRegisterNow_MouseEnter(object sender, MouseEventArgs e)
        {
            btnRegisterNow.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#808080"));
        }

        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var wpn = new SimpleParental();//wndParental(this.ToString());
           /* this.Hide();
            wpn.Left = (int)this.Left;
            wpn.Top = (int)this.Top;*/
            wpn.ShowDialog();
            //this.Close();
        }

        private void image_Copy2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //WebRoamAV.SecureDesktop.Run(null);
            var wff = new winFilesFolders(this.ToString());
            this.Hide();
            wff.Left = this.Left;
            wff.Top = this.Top;
            wff.ShowDialog();
            this.Close();
        }
        BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
      //      var margin = new Thickness(this.image_Copy2.Margin.Left, this.image_Copy2.Margin.Top, this.image_Copy2
        //        .Margin.Right, this.Margin.Bottom);
            this.image_Copy1.Visibility = Visibility.Visible;
            if (App.IsLicenseValid || (DateTime.Now.Month <= 11 && DateTime.Now.Year == 2022))
            {
                BimageNR.Source = BitmapToImageSource(Properties.Resources.ok_icon_3099);
                LbNR1.Content = "System is secure";
                LbNR2.Content = "System is being actively protected";
                LbNR3.Content = "No Action required";
               // MessageBox.Show(App.IsLicenseValid.ToString());
                if (App.IsLicenseValid)
                {
                    btnRegisterNow.Visibility = Visibility.Hidden;
                }
            }
            
            /*this.image_Copy2.Margin = new Thickness(this.image_Copy2.Margin.Left-this.image_Copy1.Width,this.image_Copy2.Margin.Top,this.image_Copy2
                .Margin.Right, this.Margin.Bottom);*/
                //GoRight_Click(null, null);
                //this.image_Copy2.Margin = margin;
        }



        private void image_Scan_Copy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var edv = new EDDevices(this.ToString());
            this.Hide();
            edv.Left = this.Left;
            edv.Top = this.Top;
            edv.ShowDialog();
            this.Close();
        }

        private void image_Scan_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Image image = sender as Image;
                ContextMenu contextMenu = (ContextMenu)this.image_Scan.Resources["scanContext"];
                contextMenu.PlacementTarget = image;
                contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
                contextMenu.IsOpen = true;
            }
        }

        private void btnRegisterNow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                Process.Start("..\\gui\\wrMainAntiRansomeware.exe", "wrMainAntiRansomeware.ProductKeyForm");

            }
        }

        private void image_Copy2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                image_Copy2_MouseLeftButtonUp(null, null);
            }
        }

        private void image_Copy1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                image_Copy1_MouseLeftButtonUp(null, null);
            }
        }

        private void image_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                image_MouseLeftButtonUp(null, null);
            }
        }

        private void image_Copy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                image_Copy_MouseLeftButtonUp(null, null);
            }
        }

        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {
            //MenuItem mi = e.OriginalSource as MenuItem;
           // MessageBox.Show(mi.ToString(), "m2");
        }
    }
    }

