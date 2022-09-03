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
    /// Interaction logic for pApplicationCategory.xaml
    /// </summary>
    /// 
    public static partial class Commands
    {
        public static readonly RoutedCommand btnp2ExcludePressed = new RoutedCommand();
        public static readonly RoutedCommand btnp2DefaultPressed = new RoutedCommand();
        public static readonly RoutedCommand btnp2OKPressed = new RoutedCommand();
        public static readonly RoutedCommand btnp2CancelPressed = new RoutedCommand();
    }
    public partial class pApplicationCategory : Window
    {
        public pApplicationCategory()
        {
            InitializeComponent();
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
                this.GetType().GetMethod(btn.Name + "_Click").Invoke(this, new object[] { sender, e });

            }

        }
        public void btnExclude_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("1");
        }

        private void btnDefault_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

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

        private void aOnOff_Checked0(object sender, RoutedEventArgs e)
        {
            aText0.Text = "ALLOW";
        }

        private void aOnOff_Unchecked0(object sender, RoutedEventArgs e)
        {
            aText0.Text = "DENY";
        }
        private void aOnOff_Checked1(object sender, RoutedEventArgs e)
        {
            aText1.Text = "ALLOW";
        }

        private void aOnOff_Unchecked1(object sender, RoutedEventArgs e)
        {
            aText1.Text = "DENY";
        }
        private void aOnOff_Checked2(object sender, RoutedEventArgs e)
        {
            aText2.Text = "ALLOW";
        }

        private void aOnOff_Unchecked2(object sender, RoutedEventArgs e)
        {
            aText2.Text = "DENY";
        }
        private void aOnOff_Checked3(object sender, RoutedEventArgs e)
        {
            aText3.Text = "ALLOW";
        }

        private void aOnOff_Unchecked3(object sender, RoutedEventArgs e)
        {
            aText3.Text = "DENY";
        }
        private void aOnOff_Checked4(object sender, RoutedEventArgs e)
        {
            aText4.Text = "ALLOW";
        }

        private void aOnOff_Unchecked4(object sender, RoutedEventArgs e)
        {
            aText4.Text = "DENY";
        }
        private void aOnOff_Checked5(object sender, RoutedEventArgs e)
        {
            aText5.Text = "ALLOW";
        }

        private void aOnOff_Unchecked5(object sender, RoutedEventArgs e)
        {
            aText5.Text = "DENY";
        }
        private void aOnOff_Checked6(object sender, RoutedEventArgs e)
        {
            aText6.Text = "ALLOW";
        }

        private void aOnOff_Unchecked6(object sender, RoutedEventArgs e)
        {
            aText6.Text = "DENY";
        }
        private void aOnOff_Checked7(object sender, RoutedEventArgs e)
        {
            aText7.Text = "ALLOW";
        }

        private void aOnOff_Unchecked7(object sender, RoutedEventArgs e)
        {
            aText7.Text = "DENY";
        }
        private void aOnOff_Checked8(object sender, RoutedEventArgs e)
        {
            aText8.Text = "ALLOW";
        }

        private void aOnOff_Unchecked8(object sender, RoutedEventArgs e)
        {
            aText8.Text = "DENY";
        }
        private void aOnOff_Checked9(object sender, RoutedEventArgs e)
        {
            aText9.Text = "ALLOW";
        }

        private void aOnOff_Unchecked9(object sender, RoutedEventArgs e)
        {
            aText9.Text = "DENY";
        }
        private void aOnOff_Checked10(object sender, RoutedEventArgs e)
        {
            aText10.Text = "ALLOW";
        }

        private void aOnOff_Unchecked10(object sender, RoutedEventArgs e)
        {
            aText10.Text = "DENY";
        }
    }
}
