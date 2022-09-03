using System;
using System.Collections.Generic;
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
    /// Interaction logic for frmVlnr.xaml
    /// </summary>
    /// 
    public static partial class Commands
    {
        public static readonly RoutedCommand btnvStartScanPressed = new RoutedCommand();
    }
    public partial class frmVlnr : Window
    {
        string mw;
        public frmVlnr(string parent)
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
            /*btnRemove.Background = new SolidColorBrush(Colors.Green);
            coloarn = new ColorAnimation();
            coloarn.From = Colors.Green;
            coloarn.To = (Color)ColorConverter.ConvertFromString("#A0008000");
            coloarn.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            coloarn.Completed -= Coloarn_Completed;*/
        }
        public void btnStartScan_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("3");
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
        private void StartScan_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnStartScan.RenderTransformOrigin = new Point(0.5, 0.5);
                btnStartScan.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnStartScan);
                Storyboard.SetTarget(growAnimation2, btnStartScan);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnStartScan.Background = new SolidColorBrush(Colors.Green);
        }

        private void StartScan_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnStartScan.RenderTransformOrigin = new Point(0.5, 0.5);
            btnStartScan.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnStartScan);
            Storyboard.SetTarget(growAnimation2, btnStartScan);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnStartScan_Click(null, null);
        }

        private void StartScan_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnStartScan.RenderTransformOrigin = new Point(0.5, 0.5);
            btnStartScan.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnStartScan);
            Storyboard.SetTarget(growAnimation2, btnStartScan);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void StartScan_MouseEnter(object sender, MouseEventArgs e)
        {
            btnStartScan.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
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
            MessageBox.Show(mi.ToString());
        }
        private void grcl1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void grcl2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void grcl1_MouseEnter(object sender, MouseEventArgs e)
        {
            if(this.lblCount1.Content.ToString() != "0")
            {
                this.grcl1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5D2F1D5"));
                this.grcl1.Cursor = Cursors.Hand;
            }            
        }

        private void grcl1_MouseLeave(object sender, MouseEventArgs e)
        {
            this.grcl1.Background = new SolidColorBrush(Colors.Transparent);
            this.grcl1.Cursor = Cursors.Arrow;
        }

        private void grcl2_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.lblCount2.Content.ToString() != "0")
            {
                this.grcl2.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5D2F1D5"));
                this.grcl2.Cursor = Cursors.Hand;
            }
        }

        private void grcl2_MouseLeave(object sender, MouseEventArgs e)
        {
            this.grcl2.Background = new SolidColorBrush(Colors.Transparent);
            this.grcl2.Cursor = Cursors.Arrow;
        }
    }
}