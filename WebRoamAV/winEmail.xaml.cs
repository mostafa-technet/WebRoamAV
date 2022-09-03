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
    /// Interaction logic for winEmail.xaml
    /// </summary>
    public partial class winEmail : Window
    {
        private ColorAnimation coloarn;
        string mw;
        public winEmail(string parent)
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
             /*   Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType((mw.StartsWith("WebRoamAV.") ? "" : "WebRoamAV.") + mw.ToString().Replace(".xaml", ""));
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
            fText.Text = "ON";
        }

        private void fOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            fText.Text = "OFF";
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

        private void SSOnOff_Checked(object sender, RoutedEventArgs e)
        {
            SSText.Text = "ON";
        }

        private void SSOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            SSText.Text = "OFF";
        }

        private void TTOnOff_Checked(object sender, RoutedEventArgs e)
        {
            TTText.Text = "ON";
        }

        private void TTOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            TTText.Text = "OFF";
        }

        private void Canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return; this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wEmailProtection(this.ToString()).ShowDialog();
            this.Close();
        }

        private void Canvas_PreviewMouseUp_1(object sender, MouseButtonEventArgs e)
        {
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return; this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wTrustedEmailClients(this.ToString()).ShowDialog();
            this.Close();
        }

        private void Canvas_PreviewMouseUp_2(object sender, MouseButtonEventArgs e)
        {
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return; this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wARProtection(this.ToString()).ShowDialog();
            this.Close();
        }
    }
}

