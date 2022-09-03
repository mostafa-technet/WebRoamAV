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
    /// Interaction logic for winFilesFolders.xaml
    /// </summary>
    public partial class winFilesFolders : Window
    {
        private ColorAnimation coloarn;
        string mw;
        public winFilesFolders(string parent)
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
            int count = SqlReaderWriter.CountOfRow("tblQuarantine");
            La2Text.Text = count + " file(s) quarantined/backup";
            fOnOff.IsChecked = App.AppSettings.IsVirusProtectOn == true;
        }

        private void ButtonEsc_Click(object sender, KeyEventArgs e){if(e.Key == Key.Escape){Button_Click(null, null);}} private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(true)//if ((Mouse.GetPosition(ButtonMn).X < ButtonMn.Width) && (Mouse.GetPosition(ButtonMn).X > 0) && (Mouse.GetPosition(ButtonMn).Y < ButtonMn.Height) && (Mouse.GetPosition(ButtonMn).Y > 0))
            {
                MainWindow mw = new MainWindow();
                mw.Left = this.Left;
                mw.Top = this.Top;
                mw.WindowStartupLocation = WindowStartupLocation.Manual;
                mw.Show(); 
                /*Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType((mw.StartsWith("WebRoamAV.") ? "" : "WebRoamAV.") + mw.ToString().Replace(".xaml", ""));
                MainWindow.parentTop = this.Top;	
				MainWindow.parentLeft = this.Left;	
				t.GetMethod("Show").Invoke(Activator.CreateInstance(t, this.ToString()), new object[] { });*/
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
          //  MessageBox.Show(mi.ToString());
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
            App.AppSettings.IsVirusProtectOn = true;
            if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='FFl' AND fieldID=4)") > 0)
            {

                SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=1  WHERE (frmName='FFl' AND fieldID=4)");
                
            }
            else
            {
                SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'FFl', 4, 1)");
            }

        }

        private void fOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            fText.Text = "OFF";
            App.AppSettings.IsVirusProtectOn = false;
            if (SqlReaderWriter.CountOfRow("tblOnOff", "WHERE (frmName='FFl' AND fieldID=4)") > 0)
            {

                SqlReaderWriter.ExecuteQuery("UPDATE tblOnOff SET fieldOnOff=0  WHERE (frmName='FFl' AND fieldID=4)");

            }
            else
            {
                SqlReaderWriter.ExecuteQuery("INSERT INTO tblOnOff (ID, frmName, fieldID, fieldOnOff) VALUES ('" + (Int32.Parse(SqlReaderWriter.MaxofRow("tblOnOff").ToString()) + 1) + "', 'FFl', 4, 0)");
            }
        }
        private void sOnOff_Checked(object sender, RoutedEventArgs e)
        {
            //sText.Text = "ON";
        }

        private void sOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
           // sText.Text = "OFF";
        }
        private void tOnOff_Checked(object sender, RoutedEventArgs e)
        {
          //  tText.Text = "ON";
        }

        private void tOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            //tText.Text = "OFF";
        }
    

        public void btnDefault_Click(object sender, RoutedEventArgs e)
        {

            //MessageBox.Show("1");
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
  

        private void t2OnOff_Checked(object sender, RoutedEventArgs e)
        {
         //   t2Text.Text = "ON";
        }

        private void t2OnOff_Unchecked(object sender, RoutedEventArgs e)
        {
           // t2Text.Text = "OFF";
        }

        private void Canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return; this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wScanSettings(this.ToString()).ShowDialog();
            this.Close();
        }

        private void Canvas_PreviewMouseUp_1(object sender, MouseButtonEventArgs e)
        {
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return; this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wVirusProtection(this.ToString()).ShowDialog();
            this.Close();
        }

        private void Canvas_PreviewMouseUp_2(object sender, MouseButtonEventArgs e)
        {
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return; this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wAdvancedDNAScan(this.ToString()).ShowDialog();
            this.Close();
        }

        private void Canvas_PreviewMouseUp_3(object sender, MouseButtonEventArgs e)
        {
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return; this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wScreenLockerProtection(this.ToString()).ShowDialog();
            this.Close();
        }

        private void Canvas_PreviewMouseUp_4(object sender, MouseButtonEventArgs e)
        {
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return; this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wScanSchedule(this.ToString()).ShowDialog();
            this.Close();
        }

        private void Canvas_PreviewMouseUp_5(object sender, MouseButtonEventArgs e)
        {
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return; this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wExcludeFF(this.ToString()).ShowDialog();
            this.Close();
        }

        private void Canvas_PreviewMouseUp_6(object sender, MouseButtonEventArgs e)
        {
            if((e.OriginalSource.GetType()==typeof(System.Windows.Controls.Primitives.ToggleButton)))  return; this.Hide();
            MainWindow.parentLeft = this.Left;
            MainWindow.parentTop = this.Top;
            new wQuaBa(this.ToString()).ShowDialog();
            this.Close();
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                this.Hide();
                MainWindow.parentLeft = this.Left;
                MainWindow.parentTop = this.Top;
                new wScanSettings(this.ToString()).ShowDialog();
                this.Close();
            }
        }

        private void Canvas_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                MainWindow.parentLeft = this.Left;
                MainWindow.parentTop = this.Top;
                new wVirusProtection(this.ToString()).ShowDialog();
                this.Close();
            }
        }

        private void Canvas_KeyDown_2(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                MainWindow.parentLeft = this.Left;
                MainWindow.parentTop = this.Top;
                new wScanSchedule(this.ToString()).ShowDialog();
                this.Close();
            }
        }

        private void Canvas_KeyDown_3(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                MainWindow.parentLeft = this.Left;
                MainWindow.parentTop = this.Top;
                new wExcludeFF(this.ToString()).ShowDialog();
                this.Close();
            }
        }

        private void Canvas_KeyDown_4(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                MainWindow.parentLeft = this.Left;
                MainWindow.parentTop = this.Top;
                new wQuaBa(this.ToString()).ShowDialog();
                this.Close();
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

        private void ButtonMn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                MainWindow mw = new MainWindow();
                mw.Left = this.Left;
                mw.Top = this.Top;
                mw.WindowStartupLocation = WindowStartupLocation.Manual;
                mw.Show(); 
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
    }
}

