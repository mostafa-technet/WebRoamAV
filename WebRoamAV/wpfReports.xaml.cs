using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace WebRoamAV
{
    public static partial class Commands
    {
        public static readonly RoutedCommand btnDtlPressed = new RoutedCommand();
        public static readonly RoutedCommand btnDelAllPressed = new RoutedCommand();
        public static readonly RoutedCommand btnDelPressed = new RoutedCommand();
        public static readonly RoutedCommand btnClosePressed = new RoutedCommand();
    }
    internal class GRowsR
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string ReportFor { get; set; }
    }
    /// <summary>
    /// Interaction logic for wpfReports.xaml
    /// </summary>
    public partial class wpfReports : Window
    {
        string mw;
        public wpfReports(string parent)
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
            /*btnDeleteAll.Background = new SolidColorBrush(Colors.Green);
            coloarn = new ColorAnimation();
            coloarn.From = Colors.Green;
            coloarn.To = (Color)ColorConverter.ConvertFromString("#A0008000");
            coloarn.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            coloarn.Completed -= Coloarn_Completed;*/            
        }

        private void Details_MouseEnter(object sender, MouseEventArgs e)
        {
            btnDetails.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
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

            lstBox.Items.Add("Virus Protection");
            //lstBox.Items.Add("Email Protection");
            lstBox.Items.Add("Scan Schedule");
            //lstBox.Items.Add("Behavior Detection");
            lstBox.Items.Add("Quick Update");
            lstBox.Items.Add("Memory Scan");
           // lstBox.Items.Add("Phishing Protection");
           // lstBox.Items.Add("Registry Restore");
            lstBox.Items.Add("Boot Time Scanner");
            //lstBox.Items.Add("AntiMalware Scan");
          //  lstBox.Items.Add("Firewall Protection");
            //lstBox.Items.Add("Parental Control");
            lstBox.Items.Add("IDS & IPS");
            //lstBox.Items.Add("Browsing Protection");
           // lstBox.Items.Add("PC2Mobile Scan");
            //lstBox.Items.Add("Vulnerability Scan");
         //   lstBox.Items.Add("Safe Banking");
           // lstBox.Items.Add("Anti-Keylogger");
            lstBox.SelectionChanged += LstBox_SelectionChanged;
        }
        public static string[] contents;
        private void LstBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridMain.Items.Clear();
            switch (lstBox.SelectedIndex)
            {
                case 0:
                    DataGridMain.IsReadOnly = false;
                    var dt = SqlReaderWriter.ReadQuery("SELECT * FROM tblReportFor WHERE (ReportFID=4 or ReportFID=1 or ReportFID=1) ORDER BY ID DESC");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");
                    for(int i=0;i<dt.Rows.Count;i++)
                    {
                        DataGridMain.Items.Add(new GRowsR{  Date= String.Format("{0:####/##/##}", dt.Rows[i]["Date"] == null ? 0 : Int32.Parse(dt.Rows[i]["Date"].ToString())), Time = dt.Rows[i]["Time"].ToString(), ReportFor = dt.Rows[i]["ReportFor"].ToString() });
                    }
                    DataGridMain.IsReadOnly = true;
                                      break;

                case 1:                    
                    DataGridMain.IsReadOnly = false;
                    var dt1 = SqlReaderWriter.ReadQuery("SELECT * FROM tblReportFor WHERE (ReportFID=7) ORDER BY ID DESC");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataGridMain.Items.Add(new GRowsR { Date = String.Format("{0:####/##/##}", dt1.Rows[i]["Date"] == null ? 0 : Int32.Parse(dt1.Rows[i]["Date"].ToString())), Time = dt1.Rows[i]["Time"].ToString(), ReportFor = dt1.Rows[i]["ReportFor"].ToString() });
                    }
                    DataGridMain.IsReadOnly = true;
                    break;

                case 2:
                    DataGridMain.IsReadOnly = false;
                    var dt4 = SqlReaderWriter.ReadQuery("SELECT * FROM tblReportFor WHERE (ReportFID=9) ORDER BY ID DESC");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        DataGridMain.Items.Add(new GRowsR { Date = String.Format("{0:####/##/##}", dt4.Rows[i]["Date"] == null ? 0 : Int32.Parse(dt4.Rows[i]["Date"].ToString())), Time = dt4.Rows[i]["Time"].ToString(), ReportFor = dt4.Rows[i]["ReportFor"].ToString() });
                    }
                    DataGridMain.IsReadOnly = true;
                    break;

             

                case 3:
                   
                    DataGridMain.IsReadOnly = false;
                    var dt2 = SqlReaderWriter.ReadQuery("SELECT * FROM tblReportFor WHERE (ReportFID=2) ORDER BY ID DESC");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        DataGridMain.Items.Add(new GRowsR { Date = String.Format("{0:####/##/##}", dt2.Rows[i]["Date"] == null ? 0 : Int32.Parse(dt2.Rows[i]["Date"].ToString())), Time = dt2.Rows[i]["Time"].ToString(), ReportFor = dt2.Rows[i]["ReportFor"].ToString() });
                    }
                    DataGridMain.IsReadOnly = true;
                    break;

                case 4:
                    DataGridMain.IsReadOnly = false;
                    var dt3 = SqlReaderWriter.ReadQuery("SELECT * FROM tblReportFor WHERE (ReportFID=8) ORDER BY ID DESC");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        DataGridMain.Items.Add(new GRowsR { Date = String.Format("{0:####/##/##}", dt3.Rows[i]["Date"] == null ? 0 : Int32.Parse(dt3.Rows[i]["Date"].ToString())), Time = dt3.Rows[i]["Time"].ToString(), ReportFor = dt3.Rows[i]["ReportFor"].ToString() });
                    }
                    DataGridMain.IsReadOnly = true;
                    break;

               

                
                

                case 5: // 7: //9: // 12: 
                    contents = File.ReadAllLines(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\wrIPS\\log\\wralert.log").Where(a=>(a.IndexOf("[**]") !=-1)).ToArray().Reverse().ToArray();
                    foreach(var s in contents)
                    {
                        string[] dtm = s.Split(new char[] { '-', '.' });
                        string rep = s.Substring(s.IndexOf("[**]"), s.LastIndexOf("[**]")- s.IndexOf("[**]"));
                        rep = rep.Substring(rep.IndexOf("ET ")+3);
                        DataGridMain.Items.Add(new GRowsR { Date = dtm[0], Time = dtm[1], ReportFor=rep });
                    }
                    break;
                   

               
                    
            }
        }

        public void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GRowsR gr = (GRowsR)DataGridMain.SelectedItem;
                if (gr != null)
                    new wReportFor(gr.Date, gr.Time, lstBox.SelectedIndex, DataGridMain).Show();
            }catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);

            }
        }   
        public void btnDeleteAll_Click(object sender, RoutedEventArgs e)
        {

            DataGridMain.Items.Clear();
            switch (lstBox.SelectedIndex)
            {
                case 0:
                    // DataGridMain.Items.Clear();

                    SqlReaderWriter.ExecuteQuery("DELETE FROM tblReportFor WHERE (ReportFID=4 or ReportFID=1 or ReportFID=1)");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");
                    
                    break;

                case 1:
                    SqlReaderWriter.ExecuteQuery("DELETE FROM tblReportFor WHERE (ReportFID=7)");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");
                   
                    break;

                case 2:

                    SqlReaderWriter.ExecuteQuery("DELETE FROM tblReportFor WHERE (ReportFID=9)");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");
                   
                    break;



                case 3:

                    SqlReaderWriter.ExecuteQuery("DELETE FROM tblReportFor WHERE (ReportFID=2)");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");
                   
                    break;

                case 4:
                   
                    SqlReaderWriter.ExecuteQuery("DELETE FROM tblReportFor WHERE (ReportFID=8)");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");
                   
                    break;






                case 5: // 7: //9: // 12: 
                 
                    break;




            }
         

        }
        public void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int n = DataGridMain.SelectedIndex;
            if (n < 0)
                return;
            switch (lstBox.SelectedIndex)
            {
                case 0:
                    // DataGridMain.Items.Clear();
               
                    
                    SqlReaderWriter.ExecuteQuery($"DELETE FROM tblReportFor WHERE (ReportFID=4 or ReportFID=1 or ReportFID=1)  ORDER BY ID DESC LIMIT {n.ToString()},1");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");

                    break;

                case 1:
                    SqlReaderWriter.ExecuteQuery($"DELETE FROM tblReportFor WHERE (ReportFID=7)");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");

                    break;

                case 2:

                    SqlReaderWriter.ExecuteQuery($"DELETE FROM tblReportFor WHERE (ReportFID=9) ORDER BY ID DESC LIMIT {n.ToString()},1");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");

                    break;



                case 3:

                    SqlReaderWriter.ExecuteQuery($"DELETE FROM tblReportFor WHERE (ReportFID=2) ORDER BY ID DESC LIMIT {n.ToString()},1");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");

                    break;

                case 4:

                    SqlReaderWriter.ExecuteQuery($"DELETE FROM tblReportFor WHERE (ReportFID=8) ORDER BY ID DESC LIMIT {n.ToString()},1");// ReportFor LIKE '%Scan%' ORDER BY ID DESC");

                    break;






                case 5: // 7: //9: // 12: 

                    break;






            }
            if (DataGridMain.SelectedIndex > -1)
            DataGridMain.Items.RemoveAt(DataGridMain.SelectedIndex);
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
                this.GetType().GetMethod(btn.Name + "_Click").Invoke(this, new object[] { sender, e});
      
                }
            
       
        }

        private void DeleteAll_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnDeleteAll.RenderTransformOrigin = new Point(0.5, 0.5);
            btnDeleteAll.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnDeleteAll);
            Storyboard.SetTarget(growAnimation2, btnDeleteAll);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void Details_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnDetails.RenderTransformOrigin = new Point(0.5, 0.5);
            btnDetails.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnDetails);
            Storyboard.SetTarget(growAnimation2, btnDetails);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnDetails_Click(null, null);
        }



        private void Details_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnDetails.RenderTransformOrigin = new Point(0.5, 0.5);
                btnDetails.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnDetails);
                Storyboard.SetTarget(growAnimation2, btnDetails);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
                btnDetails.Background = new SolidColorBrush(Colors.Green);
            
        }

        private void DeleteAll_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnDeleteAll.RenderTransformOrigin = new Point(0.5, 0.5);
                btnDeleteAll.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnDeleteAll);
                Storyboard.SetTarget(growAnimation2, btnDeleteAll);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnDeleteAll.Background = new SolidColorBrush(Colors.Green);
        }

        private void DeleteAll_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnDeleteAll.RenderTransformOrigin = new Point(0.5, 0.5);
            btnDeleteAll.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnDeleteAll);
            Storyboard.SetTarget(growAnimation2, btnDeleteAll);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnDeleteAll_Click(null, null);
        }

        private void Details_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnDetails.RenderTransformOrigin = new Point(0.5, 0.5);
            btnDetails.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnDetails);
            Storyboard.SetTarget(growAnimation2, btnDetails);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void DeleteAll_MouseEnter(object sender, MouseEventArgs e)
        {
            btnDeleteAll.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }

        private void Delete_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnDelete.RenderTransformOrigin = new Point(0.5, 0.5);
                btnDelete.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnDelete);
                Storyboard.SetTarget(growAnimation2, btnDelete);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnDelete.Background = new SolidColorBrush(Colors.Green);
        }

        private void Delete_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnDelete.RenderTransformOrigin = new Point(0.5, 0.5);
            btnDelete.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnDelete);
            Storyboard.SetTarget(growAnimation2, btnDelete);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnDelete_Click(null, null);
        }

        private void Delete_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnDelete.RenderTransformOrigin = new Point(0.5, 0.5);
            btnDelete.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnDelete);
            Storyboard.SetTarget(growAnimation2, btnDelete);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void Delete_MouseEnter(object sender, MouseEventArgs e)
        {
            btnDelete.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
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
            if (true)//(Mouse.GetPosition(ButtonMn).X < ButtonMn.Width)&& (Mouse.GetPosition(ButtonMn).X > 0) && (Mouse.GetPosition(ButtonMn).Y < ButtonMn.Height) && (Mouse.GetPosition(ButtonMn).Y > 0))
            {
                MainWindow mw = new MainWindow();
                mw.Left = this.Left;
                mw.Top = this.Top;
                mw.WindowStartupLocation = WindowStartupLocation.Manual;
                mw.Show(); this.Close();
                /*Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType((mw.StartsWith("WebRoamAV.") ? "" : "WebRoamAV.") + mw.ToString().Replace(".xaml", ""));
                MainWindow.parentTop = this.Top;	
				MainWindow.parentLeft = this.Left;	
				t.GetMethod("Show").Invoke(Activator.CreateInstance(t, this.ToString()), new object[] { });*/
                this.Close();
            }
        }
        private void link_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            if (e.Uri.ToString() == "")
                return;
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
        /*public static string getDate(int index)
        {
            GRowR row = 
        }

        public static string getTime(int index)
        {

        }*/
        private void DataGridMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                btnDetails_Click(null, null);
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);

            }
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

        private void ButtonMn_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                Button_Click(null, null);
            }
        }

        private void lstBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                LstBox_SelectionChanged(null, null);
            }
        }

        private void DataGridMain_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                DataGridMain_MouseDoubleClick(null, null);
            }
        }

        private void lblBtnDetails_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnDetails_Click(null, null);
            }
        }

        private void btnDeleteAll_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnDeleteAll_Click(null, null);
            }
        }

        private void btnDelete_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnDelete_Click(null, null);
            }
        }

        private void btnClose_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnClose_Click(null, null);
            }
            
        }
    }
}
