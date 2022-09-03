using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Sql;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
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
    public class GRowsCUS
    {
        public string PathF { get; set; }
        public string SubF { get; set; }
    }
    public static partial class Commands
    {
        public static readonly RoutedCommand btncAddPressed = new RoutedCommand();
        public static readonly RoutedCommand btncRemovePressed = new RoutedCommand();
        public static readonly RoutedCommand btncStartScanPressed = new RoutedCommand();
        public static readonly RoutedCommand btncClosePressed = new RoutedCommand();
    }
   


    public partial class wndCustomScan : Window
    {
        public string SubF { get; set; }
        List<string> SubFs = new List<string>();
        string mw;
        //public static SAVAPI sav;
        public wndCustomScan(string parent, string path):this()
        {
            if(path!="")
            
            this.DataGridMain.Items.Add(new GRowsCUS { PathF = path, SubF = "Yes" });
           // btnStartScan_Click(null, null);
        }

        ObservableCollection<GRowsCUS> mycol = new ObservableCollection<GRowsCUS>();
            public wndCustomScan(string parent)
        {

            InitializeComponent();
            try
            {
               // DataGridMain.ItemsSource = mycol;
            mw = parent;
			this.Top = MainWindow.parentTop;
			this.Left = MainWindow.parentLeft;
            this.MouseDown += WpfReports_MouseDown;
                //    DataGridMain.BeginningEdit += (s, ss) => ss.Cancel = true;
                

                //DataGridMain.DataContext = mycol;
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName()+" "+new StackFrame(1, true).GetFileLineNumber()+Environment.NewLine+em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
        public wndCustomScan():this(null)
        {
        }
            private void WpfReports_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try { 
            if (e.ChangedButton == MouseButton.Left && e.GetPosition(this).Y < 70)
                this.DragMove();
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName()+" "+new StackFrame(1, true).GetFileLineNumber()+Environment.NewLine+em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
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

        private void Details_MouseEnter(object sender, MouseEventArgs e)
        {
            btnAdd.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
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
            btnStartScan.IsEnabled = false;
            ContentControl c = ((ContentControl)btnStartScan.Content);
            btnStartScan.Content = "Waiting...";
            /* SqlConnection.CreateFile("MyDatabase.Sql");
             SqlConnection m_dbConnection = new SqlConnection("Data Source=MyDatabase.Sql;Version=3;");
             string sql = "CREATE TABLE highscores (name VARCHAR(20), family VARCHAR(30), score INT)";
             SqlCommand command = new SqlCommand(sql, m_dbConnection);
             m_dbConnection.Open();
             command.ExecuteNonQuery();
             m_dbConnection.Close();

             string sql2 = "insert into highscores (name, family, score) values ('Me', 'Asghari', 3000)";
             SqlCommand command2 = new SqlCommand(sql2, m_dbConnection);
             m_dbConnection.Open();
             command2.ExecuteNonQuery();
             m_dbConnection.Close();*/
            /*  SqlConnection m_dbConnection = new SqlConnection("Data Source=MyDatabase.Sql;Version=3;");
              m_dbConnection.Open();
              SqlCommand command = new SqlCommand(m_dbConnection);
              command.CommandText = "SELECT score, name, family FROM highscores";

              DataSet DST = new DataSet();
              DataTable DT = new DataTable();
              SqlDataAdapter SDA = new SqlDataAdapter(command);

              SDA.Fill(DT);

              this.DataGridMain.ItemsSource = DT.AsDataView();
              m_dbConnection.Close();*/
            Task.Factory.StartNew(() =>
          { if (ProcessProtector.cen == null) ProcessProtector.cen = new ClamAVEngine(); }).ContinueWith((a) => Dispatcher.Invoke(() => { btnStartScan.IsEnabled = true; btnStartScan.Content = c; }));

            
        }
        private void DataGridCell_Selected(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
               // Starts the Edit on the row;
                //DataGrid grd = (DataGrid)sender;
              //  this.DataGridMain.BeginEdit(e);
            
        }
        public void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            var fbd = new System.Windows.Forms.FolderBrowserDialog(); 
            
            if(fbd.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                DataGridMain.Items.Add(new GRowsCUS{ PathF=fbd.SelectedPath, SubF="Yes" });
                //DataGridMain.ItemsSource = mycol;
            }
        }
        public void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataGridMain.SelectedIndex > -1)
                this.DataGridMain.Items.RemoveAt(this.DataGridMain.SelectedIndex);
            else
                MessageBox.Show("Select the row you want to remove!");
        }
        List<GRowsCUS> v = null;
        public void btnStartScan_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                this.btnStartScan.Focus();
             /*   BindingExpression binex = DataGridMain.GetBindingExpression(DataGrid.ItemsSourceProperty);
                binex.UpdateSource();*/
                Dispatcher.BeginInvoke((Action)delegate(){
                    DataGridMain.CommitEdit();
                        }
                );
              //  DataGridMain.SelectAllCells();
               // MessageBox.Show(((GRowsCUS)DataGridMain.Items[0]).PathF.ToString());
                //MessageBox.Show(mycol[0].PathF);
                foreach (GRowsCUS c in this.DataGridMain.Items)
                {
                    if (!Directory.Exists(c.PathF))
                    {
                        MessageBox.Show(c.PathF + "  Directory Doesn't Exist!");
                        return;
                    }

                }
                        if (this.DataGridMain.Items.Count == 0)
                        {
                            MessageBox.Show("Add a Folder to Scan!");
                            return;
                        }
                
                int count = SqlReaderWriter.MaxofRow("tblReportFor");
                //   MessageBox.Show(count.ToString());
                /*foreach (var item in this.DataGridMain.Items)
                {
                    MessageBox.Show(NanoScan.Scan(((GRows)item).PathF));
                }*/
                MainWindow.parentLeft = this.Left;
                MainWindow.parentTop = this.Top;
                v = this.DataGridMain.Items.Cast<GRowsCUS>().ToList();
                
                for(int i =0;i<v.Count;i++)
                {
                    v[i].SubF = Cmbvalues[i];
                }
                //MessageBox.Show(v[0].SubF);
                wndFullScan w = new wndFullScan(this.ToString(),false, ref v);
              

                int rpfID = 1;
                int iDate = Int32.Parse(string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                string sTime = DateTime.Now.ToLongTimeString();
                string repfor = "Custom Scan:\n";
                for(int i=0;i<DataGridMain.Items.Count;i++)
                {                    
                    repfor += ((GRowsCUS)DataGridMain.Items[i]).PathF;
                    repfor += ",";
                }
                //repfor = repfor.Remove(repfor.Length - 2, 2);
                SqlReaderWriter.ExecuteQuery("INSERT INTO tblReportFor (ID, ReportFID, Date, Time, ReportFor) VALUES (" + (count + 100000*rpfID) + ", " + rpfID + ", " + iDate + ", '" + sTime + "', '" + repfor + "')");
                this.Hide();
                w.ShowDialog();
                this.Close();
                /*  string arg = ((GRows)DataGridMain.Items[0]).PathF;
                  string repair = "0";
                  IntPtr parg = Marshal.StringToHGlobalUni(System.AppDomain.CurrentDomain.FriendlyName + "$11488" + arg +"$1");            
                  IntPtr parg2 = Marshal.StringToHGlobalUni(repair);
                  SAVAPI sav = new SAVAPI();
                  int r = sav.func_ScanFile(parg);
                  //return;
                  Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
                  {
                      MessageBox.Show(r.ToString());
                  }));
                  Marshal.FreeHGlobal(parg);*/
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName()+" "+new StackFrame(1, true).GetFileLineNumber()+Environment.NewLine+em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
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

        private void Details_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnAdd.RenderTransformOrigin = new Point(0.5, 0.5);
            btnAdd.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnAdd);
            Storyboard.SetTarget(growAnimation2, btnAdd);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
            btnAdd_Click(null, null);
        }



        private void Details_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Storyboard storyboard = new Storyboard();

                ScaleTransform scale = new ScaleTransform(1.0, 1.0);
                btnAdd.RenderTransformOrigin = new Point(0.5, 0.5);
                btnAdd.RenderTransform = scale;

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
                Storyboard.SetTarget(growAnimation, btnAdd);
                Storyboard.SetTarget(growAnimation2, btnAdd);
                //storyboard.AutoReverse = true;
                storyboard.Begin();
            }
            btnAdd.Background = new SolidColorBrush(Colors.Green);

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

        private void Details_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            btnAdd.RenderTransformOrigin = new Point(0.5, 0.5);
            btnAdd.RenderTransform = scale;

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
            Storyboard.SetTarget(growAnimation, btnAdd);
            Storyboard.SetTarget(growAnimation2, btnAdd);
            //storyboard.AutoReverse = true;
            storyboard.Begin();
        }

        private void DeleteAll_MouseEnter(object sender, MouseEventArgs e)
        {
            btnRemove.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
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
            try
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
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName()+" "+new StackFrame(1, true).GetFileLineNumber()+Environment.NewLine+em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
        private void link_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            { 
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.OriginalSource as MenuItem;
            MessageBox.Show(mi.ToString());
        }
        Dictionary<int, string> Cmbvalues = new Dictionary<int, string>();
        DataRowView mrowView = null;
        private void myCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke( System.Windows.Threading.DispatcherPriority.Normal,(Action)(() =>
            {
                try { 
                var mycmb = ((ComboBox)e.Source);
                DataGridRow row = (DataGridRow)DataGridMain.ContainerFromElement(mycmb);
                int rowIndex = row.GetIndex();
                Cmbvalues[rowIndex] = mycmb.Text;
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }));
           
        }
        private bool isManualEditCommit = false;
        private void DataGridMain_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!isManualEditCommit)
            {
                try
                { 
                isManualEditCommit = true;
                DataGrid grid = (DataGrid)sender;
                this.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    grid.CommitEdit();
                  //  grid.Items.Refresh();
                });
                
                isManualEditCommit = false;
                }
                catch (Exception em)
                {
                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                }
            }
            DataRowView rowView = e.Row.Item as DataRowView;
            //btnAdd.Focus();
        }

        private void DataGridMain_CurrentCellChanged(object sender, EventArgs e)
        {
            if (mrowView != null)
            {
                this.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    try { 
                    mrowView.EndEdit();
                        //btnAdd.Focus();
                    }
                    catch (Exception em)
                    {
                        ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                    }
                }
            );
            }
        }

        private void DataGridMain_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)delegate ()
            {
                try
                { 
                DataGridMain.CommitEdit();
                    //btnAdd.Focus();
                }
                catch (Exception em)
                {
                    ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                }
            });
           
        }
    }
}
