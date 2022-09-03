using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WebRoamAV
{
    /// <summary>
    /// Interaction logic for pWebCategory.xaml
    /// </summary>
    /// 
    public static partial class Commands
    {
        public static readonly RoutedCommand btnpExcludePressed = new RoutedCommand();
        public static readonly RoutedCommand btnpDefaultPressed = new RoutedCommand();
        public static readonly RoutedCommand btnpOKPressed = new RoutedCommand();
        public static readonly RoutedCommand btnpCancelPressed = new RoutedCommand();
    }
    public partial class pWebCategory : Window
    {        
            public pWebCategory()
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
            fExcludeURL fex = new fExcludeURL();
            fex.ShowDialog();
        }

        private void btnDefault_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
           // this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            //this.Close();
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
        private void aOnOff_Checked11(object sender, RoutedEventArgs e)
        {
            aText11.Text = "ALLOW";
        }

        private void aOnOff_Unchecked11(object sender, RoutedEventArgs e)
        {
            aText11.Text = "DENY";
        }
        private void aOnOff_Checked12(object sender, RoutedEventArgs e)
        {
            aText12.Text = "ALLOW";
        }

        private void aOnOff_Unchecked12(object sender, RoutedEventArgs e)
        {
            aText12.Text = "DENY";
        }
        private void aOnOff_Checked13(object sender, RoutedEventArgs e)
        {
            aText13.Text = "ALLOW";
        }

        private void aOnOff_Unchecked13(object sender, RoutedEventArgs e)
        {
            aText13.Text = "DENY";
        }
        private void aOnOff_Checked14(object sender, RoutedEventArgs e)
        {
            aText14.Text = "ALLOW";
        }

        private void aOnOff_Unchecked14(object sender, RoutedEventArgs e)
        {
            aText14.Text = "DENY";
        }
        private void aOnOff_Checked15(object sender, RoutedEventArgs e)
        {
            aText15.Text = "ALLOW";
        }

        private void aOnOff_Unchecked15(object sender, RoutedEventArgs e)
        {
            aText15.Text = "DENY";
        }
        private void aOnOff_Checked16(object sender, RoutedEventArgs e)
        {
            aText16.Text = "ALLOW";
        }

        private void aOnOff_Unchecked16(object sender, RoutedEventArgs e)
        {
            aText16.Text = "DENY";
        }
        private void aOnOff_Checked17(object sender, RoutedEventArgs e)
        {
            aText17.Text = "ALLOW";
        }

        private void aOnOff_Unchecked17(object sender, RoutedEventArgs e)
        {
            aText17.Text = "DENY";
        }
        private void aOnOff_Checked18(object sender, RoutedEventArgs e)
        {
            aText18.Text = "ALLOW";
        }

        private void aOnOff_Unchecked18(object sender, RoutedEventArgs e)
        {
            aText18.Text = "DENY";
        }
        private void aOnOff_Checked19(object sender, RoutedEventArgs e)
        {
            aText19.Text = "ALLOW";
        }

        private void aOnOff_Unchecked19(object sender, RoutedEventArgs e)
        {
            aText19.Text = "DENY";
        }
        private void aOnOff_Checked20(object sender, RoutedEventArgs e)
        {
            aText20.Text = "ALLOW";
        }

        private void aOnOff_Unchecked20(object sender, RoutedEventArgs e)
        {
            aText20.Text = "DENY";
        }
        private void aOnOff_Checked21(object sender, RoutedEventArgs e)
        {
            aText21.Text = "ALLOW";
        }

        private void aOnOff_Unchecked21(object sender, RoutedEventArgs e)
        {
            aText21.Text = "DENY";
        }
        private void aOnOff_Checked22(object sender, RoutedEventArgs e)
        {
            aText22.Text = "ALLOW";
        }

        private void aOnOff_Unchecked22(object sender, RoutedEventArgs e)
        {
            aText22.Text = "DENY";
        }
        private void aOnOff_Checked23(object sender, RoutedEventArgs e)
        {
            aText23.Text = "ALLOW";
        }

        private void aOnOff_Unchecked23(object sender, RoutedEventArgs e)
        {
            aText23.Text = "DENY";
        }
        private void aOnOff_Checked24(object sender, RoutedEventArgs e)
        {
            aText24.Text = "ALLOW";
        }

        private void aOnOff_Unchecked24(object sender, RoutedEventArgs e)
        {
            aText24.Text = "DENY";
        }
        private void aOnOff_Checked25(object sender, RoutedEventArgs e)
        {
            aText25.Text = "ALLOW";
        }

        private void aOnOff_Unchecked25(object sender, RoutedEventArgs e)
        {
            aText25.Text = "DENY";
        }
        private void aOnOff_Checked26(object sender, RoutedEventArgs e)
        {
            aText26.Text = "ALLOW";
        }

        private void aOnOff_Unchecked26(object sender, RoutedEventArgs e)
        {
            aText26.Text = "DENY";
        }
        private void aOnOff_Checked27(object sender, RoutedEventArgs e)
        {
            aText27.Text = "ALLOW";
        }

        private void aOnOff_Unchecked27(object sender, RoutedEventArgs e)
        {
            aText27.Text = "DENY";
        }
        private void aOnOff_Checked28(object sender, RoutedEventArgs e)
        {
            aText28.Text = "ALLOW";
        }

        private void aOnOff_Unchecked28(object sender, RoutedEventArgs e)
        {
            aText28.Text = "DENY";
        }
        private void aOnOff_Checked29(object sender, RoutedEventArgs e)
        {
            aText29.Text = "ALLOW";
        }

        private void aOnOff_Unchecked29(object sender, RoutedEventArgs e)
        {
            aText29.Text = "DENY";
        }
        private void aOnOff_Checked30(object sender, RoutedEventArgs e)
        {
            aText30.Text = "ALLOW";
        }

        private void aOnOff_Unchecked30(object sender, RoutedEventArgs e)
        {
            aText30.Text = "DENY";
        }
        private void aOnOff_Checked31(object sender, RoutedEventArgs e)
        {
            aText31.Text = "ALLOW";
        }

        private void aOnOff_Unchecked31(object sender, RoutedEventArgs e)
        {
            aText31.Text = "DENY";
        }
        private void aOnOff_Checked32(object sender, RoutedEventArgs e)
        {
            aText32.Text = "ALLOW";
        }

        private void aOnOff_Unchecked32(object sender, RoutedEventArgs e)
        {
            aText32.Text = "DENY";
        }
        private void aOnOff_Checked33(object sender, RoutedEventArgs e)
        {
            aText33.Text = "ALLOW";
        }

        private void aOnOff_Unchecked33(object sender, RoutedEventArgs e)
        {
            aText33.Text = "DENY";
        }
        private void aOnOff_Checked34(object sender, RoutedEventArgs e)
        {
            aText34.Text = "ALLOW";
        }

        private void aOnOff_Unchecked34(object sender, RoutedEventArgs e)
        {
            aText34.Text = "DENY";
        }
        private void aOnOff_Checked35(object sender, RoutedEventArgs e)
        {
            aText35.Text = "ALLOW";
        }

        private void aOnOff_Unchecked35(object sender, RoutedEventArgs e)
        {
            aText35.Text = "DENY";
        }
        private void aOnOff_Checked36(object sender, RoutedEventArgs e)
        {
            aText36.Text = "ALLOW";
        }

        private void aOnOff_Unchecked36(object sender, RoutedEventArgs e)
        {
            aText36.Text = "DENY";
        }
        private void aOnOff_Checked37(object sender, RoutedEventArgs e)
        {
            aText37.Text = "ALLOW";
        }

        private void aOnOff_Unchecked37(object sender, RoutedEventArgs e)
        {
            aText37.Text = "DENY";
        }
        private void aOnOff_Checked38(object sender, RoutedEventArgs e)
        {
            aText38.Text = "ALLOW";
        }

        private void aOnOff_Unchecked38(object sender, RoutedEventArgs e)
        {
            aText38.Text = "DENY";
        }
        private void aOnOff_Checked39(object sender, RoutedEventArgs e)
        {
            aText39.Text = "ALLOW";
        }

        private void aOnOff_Unchecked39(object sender, RoutedEventArgs e)
        {
            aText39.Text = "DENY";
        }
        private void aOnOff_Checked40(object sender, RoutedEventArgs e)
        {
            aText40.Text = "ALLOW";
        }

        private void aOnOff_Unchecked40(object sender, RoutedEventArgs e)
        {
            aText40.Text = "DENY";
        }
        private void aOnOff_Checked41(object sender, RoutedEventArgs e)
        {
            aText41.Text = "ALLOW";
        }

        private void aOnOff_Unchecked41(object sender, RoutedEventArgs e)
        {
            aText41.Text = "DENY";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
