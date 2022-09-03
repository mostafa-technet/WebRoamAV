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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace WebRoamAV
{
    /// <summary>
    /// Interaction logic for wndTools.xaml
    /// </summary>
    public partial class wndTools : Window
    {
        string mw;
    
        public wndTools(string parent)
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
              /*  Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType((mw.StartsWith("WebRoamAV.") ? "" : "WebRoamAV.") + mw.ToString().Replace(".xaml", ""));
                MainWindow.parentTop = this.Top;	
				MainWindow.parentLeft = this.Left;	
				t.GetMethod("Show").Invoke(Activator.CreateInstance(t, this.ToString()), new object[] { });*/
                this.Close();
            }
        }
        private void link_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            if (e.Uri.ToString() == "") return;
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
        private void Hyperlink_RequestNavigate1(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            new tHiJackRestore().ShowDialog();
        }
        private void Hyperlink_RequestNavigate2(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            new tTrackCleaner().ShowDialog();
        }
        private void Hyperlink_RequestNavigate3(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            new tEmDisk1().ShowDialog();
        }
        private void Hyperlink_RequestNavigate4(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            MessageBox.Show("No AntiMalware Installed!!");
        }
        private void Hyperlink_RequestNavigate5(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            new tViewQuarantineFiles().ShowDialog();
        }
        private void Hyperlink_RequestNavigate6(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            new tUSBDriveProtection().ShowDialog();
        }
        private void Hyperlink_RequestNavigate7(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            new tSystemExplorer().ShowDialog();
        }
        private void Hyperlink_RequestNavigate8(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            new tWindowsSpy(this).ShowDialog();
        }
        private void Hyperlink_RequestNavigate9(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            new tExcludeFileExtension().ShowDialog();
        }

        private void buttonMin_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void buttonClose_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ButtonMn_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
