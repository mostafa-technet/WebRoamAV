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
    /// Interaction logic for wExcludeFF.xaml
    /// </summary>
    /// 
    public static partial class Commands
    {
        public static readonly RoutedCommand btneffAddPressed = new RoutedCommand();
        public static readonly RoutedCommand btneffRemovePressed = new RoutedCommand();
        public static readonly RoutedCommand btneffEditPressed = new RoutedCommand();
        public static readonly RoutedCommand btneffSaveChangesPressed = new RoutedCommand();
        public static readonly RoutedCommand btneffCancelPressed = new RoutedCommand();
    }
    public class GRowEXC
    {
        public string PathF { get; set; }
        public string SubF { get; set; }
        public string Exclusions { get; set; }
    }
    public partial class wExcludeFF : Window
    {
        string mw;
        public wExcludeFF(string parent)
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

        private void Edit_MouseEnter(object sender, MouseEventArgs e)
        {
            btnEdit.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
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
              SqlDataAdapter SDA = new SqlDataAdapter(command);f

              SDA.Fill(DT);

              this.DataGridMain.ItemsSource = DT.AsDataView();
              m_dbConnection.Close();*/
            try
            { 
            string selc = "SELECT Path, Subfolders, ExclusionFor FROM tblExcludeFF;";
            //MessageBox.Show(selc);
            DataTable dt = SqlReaderWriter.ReadQuery(selc);
            int count = SqlReaderWriter.MaxofRow("tblExcludeFF");
            
            if (dt.Rows.Count > 0)
                foreach (DataRow d in dt.Rows)
                {
                    string subf;
                    if (Directory.Exists(d[0].ToString()))
                    {
                        subf = d[1].ToString().ToUpperInvariant() == "TRUE" ? "Yes" : "No";
                    }
                    else
                    {
                        subf = "N/A";
                    }
                    DataGridMain.Items.Add(new GRowEXC() { PathF=d[0].ToString(), SubF=subf, Exclusions=d[2].ToString()});
                }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }

        public void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = new ExcludeItem().ShowDialog();
                if (result == System.Windows.Forms.DialogResult.Cancel)
                    return;
                string excludes = "";
                if (ExcludeItem.MyOptions[0])
                    excludes += "Known virus detection, ";
                if (ExcludeItem.MyOptions[1])
                    excludes += "DNAScan, ";
                if (ExcludeItem.MyOptions[2])
                    excludes += "Suspected packed files scan, ";
                if (ExcludeItem.MyOptions[3])
                    excludes += "Behavior detection, ";

                excludes = excludes.Substring(0, excludes.Length - 2);
                bool isDir = Directory.Exists(ExcludeItem._lastString.Replace("\\*.*", ""));
                DataGridMain.Items.Add(new GRowEXC() { PathF = ExcludeItem._lastString.Replace("\\*.*", ""), SubF = ExcludeItem._lastString.Contains("\\*.*") ? "Yes" : !isDir ? "N/A" : "No", Exclusions = excludes });
            }
            catch(Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
        public void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
            if (DataGridMain.Items.Count > 0 && DataGridMain.SelectedIndex >= 0)
            {
                var row = DataGridMain.Items.Cast<GRowEXC>().ElementAt(DataGridMain.SelectedIndex);
                var stext = row.PathF + (row.SubF == "Yes" ? "\\*.*" : "");
                string excl = row.Exclusions;
                DataGridMain.Items.RemoveAt(DataGridMain.SelectedIndex);
                ExcludeItem.MyOptions[0] = false;
                ExcludeItem.MyOptions[1] = false;
                ExcludeItem.MyOptions[2] = false;
                ExcludeItem.MyOptions[3] = false;


                if (excl.Contains("Known virus detection"))
                {
                    ExcludeItem.MyOptions[0] = true;

                }
                if (excl.Contains("DNAScan"))
                {
                    ExcludeItem.MyOptions[1] = true;

                }
                if (excl.Contains("Suspected packed files scan"))
                {
                    ExcludeItem.MyOptions[2] = true;

                }
                if (excl.Contains("Behavior detection"))
                {
                    ExcludeItem.MyOptions[3] = true;

                }
                

                new ExcludeItem(true, stext).ShowDialog();
                string excludes = "";
                if (ExcludeItem.MyOptions[0])
                    excludes += "Known virus detection, ";
                if (ExcludeItem.MyOptions[1])
                    excludes += "DNAScan, ";
                if (ExcludeItem.MyOptions[2])
                    excludes += "Suspected packed files scan, ";
                if (ExcludeItem.MyOptions[3])
                    excludes += "Behavior detection, ";

                excludes = excludes.Substring(0, excludes.Length - 2);
                bool isDir = Directory.Exists(ExcludeItem._lastString.Replace("\\*.*", ""));
                DataGridMain.Items.Add(new GRowEXC() { PathF = ExcludeItem._lastString.Replace("\\*.*", ""), SubF = ExcludeItem._lastString.Contains("\\*.*") ? "Yes" : !isDir ? "N/A" : "No", Exclusions = excludes });
                DataGridMain.SelectedIndex = DataGridMain.Items.Count - 1;
            }
            }
            catch(Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
        List<string> statement = new List<string>();
        public void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try { 
            if (DataGridMain.Items.Count > 0 && DataGridMain.SelectedIndex >= 0)
            {
                statement.Add("DELETE FROM tblExcludeFF WHERE Path='" + ((GRowEXC)DataGridMain.SelectedItem).PathF + "';");
                DataGridMain.Items.RemoveAt(DataGridMain.SelectedIndex);
            }
            }
            catch(Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
        public void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var st in ExcludeItem._statement)
                {
                 //   MessageBox.Show(st);
                    if (!string.IsNullOrEmpty(st))
                        SqlReaderWriter.ExecuteQuery(st);
                }
                foreach (var st in statement)
                {
                    if (!string.IsNullOrEmpty(st))

                        SqlReaderWriter.ExecuteQuery(st);
                }
            }
            catch (Exception em)
            {
                ActivateForm.FAppendAllText("wrlog.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
            }
            MessageBox.Show("Updated Successfully!");
            this.Close();
        }
        public void btnCancel_Click(object sender, RoutedEventArgs e)
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

        private void Add_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void Add_MouseLeave_1(object sender, MouseEventArgs e)
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

        private void Add_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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

        private void Add_MouseEnter(object sender, MouseEventArgs e)
        {
            btnAdd.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
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

        private void SaveChanges_MouseEnter(object sender, MouseEventArgs e)
        {
            btnSaveChanges.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
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

        private void Cancel_MouseEnter(object sender, MouseEventArgs e)
        {
            btnCancel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
        }

        private void Remove_MouseLeave_1(object sender, MouseEventArgs e)
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

        private void Remove_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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

        private void Remove_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void Remove_MouseEnter(object sender, MouseEventArgs e)
        {
            btnRemove.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0008000"));
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
            //MenuItem mi = e.OriginalSource as MenuItem;
           // MessageBox.Show(mi.ToString());
        }

        private void buttonMin_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void buttonClose_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ButtonMn_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnAdd_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnRemove_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnEdit_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnSaveChanges_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnCancel_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
