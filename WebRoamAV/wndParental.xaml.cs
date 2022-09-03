using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for wndParental.xaml
    /// </summary>
    /// 


    public static partial class Commands
    {
        public static readonly RoutedCommand btnDefTPressed = new RoutedCommand();
        public static readonly RoutedCommand btnSavTPressed = new RoutedCommand();
        public static readonly RoutedCommand btnCanTPressed = new RoutedCommand();
    }

        public partial class wndParental : Window
    {
        string mw;
        int ir = 0;
        public wndParental(string parent)
        {
            InitializeComponent();
            mw = parent;
			this.Top = MainWindow.parentTop;
			this.Left = MainWindow.parentLeft;
            this.MouseDown += MainWindow_MouseDown;
        }
       
        ColorAnimation coloarn;
        private void Coloarn_Completed(object sender, EventArgs e)
        {
            /*btnDeleteAll.Background = new SolidColorBrush(Colors.Green);
            coloarn = new ColorAnimation();
            coloarn.From = Colors.Green;
            coloarn.To = (Color)ColorConverter.ConvertFromString("#A0008000");
            coloarn.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            coloarn.Completed -= Coloarn_Completed;*/
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
        }

        private void ButtonEsc_Click(object sender, KeyEventArgs e){if(e.Key == Key.Escape){Button_Click(null, null);}} private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(true)//if ((Mouse.GetPosition(ButtonMn).X < ButtonMn.Width) && (Mouse.GetPosition(ButtonMn).X > 0) && (Mouse.GetPosition(ButtonMn).Y < ButtonMn.Height) && (Mouse.GetPosition(ButtonMn).Y > 0))
            {
                MainWindow mw = new MainWindow();
                mw.Left = this.Left;
                mw.Top = this.Top;
                mw.WindowStartupLocation = WindowStartupLocation.Manual;
                mw.Show(); this.Close();/*
                Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType((mw.StartsWith("WebRoamAV.") ? "" : "WebRoamAV.") + mw.ToString().Replace(".xaml", ""));
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
            MessageBox.Show(mi.ToString());
        }
        public void btnDefault_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("2");
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

            MessageBox.Show("1");
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

        public void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("3");
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

        private void r2_Checked(object sender, RoutedEventArgs e)
        {
            ir = 2;
            this.cvAllU.Visibility = Visibility.Collapsed;
            SelectQuery query = new SelectQuery("Win32_UserAccount");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            stUsers.Children.Clear();


            foreach (ManagementObject envVar in searcher.Get())
            {
                TextBlock tb = new TextBlock();
                tb.Text = envVar["Name"].ToString();
                tb.Margin = new Thickness(10, 10, 0, 0);
                tb.FontWeight = FontWeights.Bold;
                TextBlock tb2 = new TextBlock();
                tb2.Text = "Not Configured";
                tb2.Margin = new Thickness(400, 10, 0, 0);
                Grid cv = new Grid();
                cv.Height = 60;
                cv.ColumnDefinitions.Add(new ColumnDefinition());
                
                cv.VerticalAlignment = VerticalAlignment.Center;
                
                cv.Children.Add(tb);
                cv.Children.Add(tb2);
                cv.Cursor = Cursors.Hand;
                //this.rect.RowDefinitions[1].Height = new GridLength(180);
                
                cv.MouseEnter += delegate (object sen, MouseEventArgs eve) 
                {

                    cv.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5D2F1D5"));
                 
                  
                };
                cv.MouseLeave += delegate (object sen, MouseEventArgs eve)
                {

                    cv.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E7F3E8"));

                };
                stUsers.Children.Add(cv);
                Line line = new Line();
                line.X1 = 0;
                line.Y1 = 0;
                line.Y2 = 0;
                line.X2 = 600;
                SolidColorBrush sbr = new SolidColorBrush(Colors.White);
                line.Stroke = sbr;
                line.StrokeThickness = 3;
                stUsers.Children.Add(line);
            }
          
        }

        
        private void r1_Checked(object sender, RoutedEventArgs e)
        {
            this.cvAllU.Visibility = Visibility.Visible;
            this.stUsers.Children.Clear();
            this.stUsers.Children.Add(this.cvAllU);
            ir = 1;
        }

        private void cvAllU_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            new ParentalControl().ShowDialog();
        }
    }
}

