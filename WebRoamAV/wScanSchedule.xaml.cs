using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Diagnostics;
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
    /// Interaction logic for wScanSchedule.xaml
    /// </summary>
    /// 
    public static partial class Commands
    {
        public static readonly RoutedCommand btnssNewPressed = new RoutedCommand();
        public static readonly RoutedCommand btnssRemovePressed = new RoutedCommand();
        public static readonly RoutedCommand btnssEditPressed = new RoutedCommand();
        public static readonly RoutedCommand btnssClosePressed = new RoutedCommand();
    }
    internal class GRowSS
    {
        public string scheduleItem { get; set; }
        public string frequency { get; set; }
        public string scanlocation { get; set; }
    }
    public partial class wScanSchedule : Window
    {
        string mw;
        public static Dictionary<string, object> SchItems = new Dictionary<string, object>();
        public wScanSchedule(string parent)
        {
            InitializeComponent();
            mw = parent;
			this.Top = MainWindow.parentTop;
			this.Left = MainWindow.parentLeft;
            this.MouseDown += WpfReports_MouseDown;
        }

        private void WpfReports_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left && e.GetPosition(this).Y < 70)
                this.DragMove();
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

        private void New_MouseEnter(object sender, MouseEventArgs e)
        {
            btnNew.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
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
            try { 
            DataGridMain.Items.Clear();
            string selc = "SELECT ScheduleItem, Frequency, FreqTime, FreqRepeat, FreqPriority, FreqUserName, FreqPassword, FreqRunIfmissed, ScanLocation FROM tblScanSchedule";
            DataTable dt = SqlReaderWriter.ReadQuery(selc);
            if (dt.Rows.Count > 0)
            {                
                btnEdit.IsEnabled = true;
                btnRemove.IsEnabled = true;
                DataGridMain.SelectedIndex = 0;
                foreach (DataRow d in dt.Rows)
                {
                    string freq;
                    if (((int)Int32.Parse(d[1].ToString()) / 100) == 0)
                    {
                        freq = "Every " + (d[2].ToString().Contains(",") ? d[2].ToString().Split(',')[1] : "") + " Day(s) at " + (d[2].ToString().Contains(",") ? d[2].ToString().Split(',')[0] : " First Boot");
                    }
                    else
                    {
                        freq = $"Every {(d[2].ToString().Contains(",") ? d[2].ToString().Split(',')[1] : "") } Week(s) on " + System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName((DayOfWeek)(Int32.Parse(d[1].ToString()) % 100)) + " at " + (d[2].ToString().Contains(",") ? d[2].ToString().Split(',')[0] : " First Boot");
                    }
                    this.DataGridMain.Items.Add(new GRowSS() { scheduleItem = d[0].ToString(), frequency = freq, scanlocation = d[8].ToString() });
                }
            }

            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }

        }

        public void btnNew_Click(object sender, RoutedEventArgs e)
        {

            var ws = new WScanSchedule();
            ws.ShowDialog();          
        }
        public void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try { 
            if(DataGridMain.SelectedIndex >= 0)
            {
                if(MessageBox.Show("Do you want to remove the Scheduled Scan?","Webroam Security", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    string selc = "DELETE FROM tblScanSchedule WHERE ScheduleItem='" + (DataGridMain.SelectedCells[0].Column.GetCellContent(DataGridMain.SelectedItem) as TextBlock).Text + "';";
                    SqlReaderWriter.ExecuteQuery(selc);
                    DataGridMain.Items.RemoveAt(DataGridMain.SelectedIndex);
                }
                if(DataGridMain.Items.Count==0)
                {
                    btnEdit.IsEnabled = btnRemove.IsEnabled = false;
                }
            }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
        public void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try { 
            if (DataGridMain.SelectedCells.Count > 0)
            {
                wScanSchedule.SchItems.Clear();
                wScanSchedule.SchItems.Add("F1_textBox1", (DataGridMain.SelectedCells[0].Column.GetCellContent(DataGridMain.SelectedItem) as TextBlock).Text);

                var ws = new WScanSchedule(true);
                ws.ShowDialog();
            }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }

        }
        public void btnClose_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
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

        private void DeleteAll_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnRemove.RenderTransformOrigin = new Point(0.5, 0.5);
            btnRemove.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnRemove);
            Storyboard.SetTarget(growAnimation2, btnRemove);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void New_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnNew.RenderTransformOrigin = new Point(0.5, 0.5);
            btnNew.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnNew);
            Storyboard.SetTarget(growAnimation2, btnNew);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnNew_Click(null, null);
        }



        private void New_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnNew.RenderTransformOrigin = new Point(0.5, 0.5);
                btnNew.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnNew);
                Storyboard.SetTarget(growAnimation2, btnNew);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnNew.Background = new SolidColorBrush(Colors.Green);

        }

        private void DeleteAll_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnRemove.RenderTransformOrigin = new Point(0.5, 0.5);
                btnRemove.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnRemove);
                Storyboard.SetTarget(growAnimation2, btnRemove);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnRemove.Background = new SolidColorBrush(Colors.Green);
        }

        private void DeleteAll_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnRemove.RenderTransformOrigin = new Point(0.5, 0.5);
            btnRemove.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnRemove);
            Storyboard.SetTarget(growAnimation2, btnRemove);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnRemove_Click(null, null);
        }

        private void New_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnNew.RenderTransformOrigin = new Point(0.5, 0.5);
            btnNew.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnNew);
            Storyboard.SetTarget(growAnimation2, btnNew);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void DeleteAll_MouseEnter(object sender, MouseEventArgs e)
        {
            btnRemove.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }

        private void Edit_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnEdit.RenderTransformOrigin = new Point(0.5, 0.5);
                btnEdit.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnEdit);
                Storyboard.SetTarget(growAnimation2, btnEdit);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnEdit.Background = new SolidColorBrush(Colors.Green);
        }

        private void Edit_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnEdit.RenderTransformOrigin = new Point(0.5, 0.5);
            btnEdit.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnEdit);
            Storyboard.SetTarget(growAnimation2, btnEdit);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnEdit_Click(null, null);
        }

        private void Edit_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnEdit.RenderTransformOrigin = new Point(0.5, 0.5);
            btnEdit.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnEdit);
            Storyboard.SetTarget(growAnimation2, btnEdit);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void Edit_MouseEnter(object sender, MouseEventArgs e)
        {
            btnEdit.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }


        private void Close_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnClose.RenderTransformOrigin = new Point(0.5, 0.5);
                btnClose.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnClose);
                Storyboard.SetTarget(growAnimation2, btnClose);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnClose.Background = new SolidColorBrush(Colors.Green);
        }

        private void Close_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnClose.RenderTransformOrigin = new Point(0.5, 0.5);
            btnClose.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnClose);
            Storyboard.SetTarget(growAnimation2, btnClose);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnClose_Click(null, null);
        }

        private void Close_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnClose.RenderTransformOrigin = new Point(0.5, 0.5);
            btnClose.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnClose);
            Storyboard.SetTarget(growAnimation2, btnClose);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void Close_MouseEnter(object sender, MouseEventArgs e)
        {
            btnClose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
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

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            Window_Loaded(sender, e);
        }

        private void buttonMin_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                buttonMin_Click(null, null);
            }
        }

        private void buttonClose_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                buttonClose_Click(null, null);
            }
        }

        private void btnNew_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnNew_Click(null, null);
            }
        }

        private void btnRemove_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnRemove_Click(null, null);
            }
        }

        private void btnEdit_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnEdit_Click(null, null);
            }
        }

        private void btnClose_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnClose_Click(null, null);
            }
        }

        private void ButtonMn_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                Button_Click(null, null);
            }
        }
    }
}
