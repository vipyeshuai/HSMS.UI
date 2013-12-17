using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HSMS.Model;
using HSMS.DAL;
using HSMS.UI.SystemMgr;
using HSMS.UI.softWHelpWindow;

namespace HSMS.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ImageBrush b = new ImageBrush();

            b.ImageSource = new BitmapImage(new Uri("pack://application:,,,/backgroundImage/main2.jpg"));   

            b.Stretch = Stretch.Fill;

            this.Background = b;

        }

        private Login login;

        public Login Login
        {
            get { return login; }
            set { login = value; }
        }

        private void miOperatormng_Click(object sender, RoutedEventArgs e)
        {

            OperatorListUI listUi = new OperatorListUI();
            listUi.ShowDialog();
            //Operator op = new Operator();
            //op.UserName = "aaa";
            //op.Password = "123";
            //OperatorDAL.Insert(op);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Login lg = new Login();
            //if (lg.ShowDialog() != true)
            //{
            //    Application.Current.Shutdown();
            //}
            
            this.Title = SettingDAL.GetValue("公司名称") + "人事管理系统";

            //查询是否有员工过生日

            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            if (SettingDAL.GetBoolValue("启用生日提醒"))
            {
                List<Employee> list = EmployeeDAL.ListAll().ToList();
                foreach (Employee item in list)
                {
                    
                    int empMonth = item.BirthDay.Month;
                    int empDay = item.BirthDay.Day;
                    if (month == empMonth && empDay == day)
                    {
                        MessageBox.Show("今天是你的同事" + item.Name + "的生日，赶快送上祝福吧！");
                        continue;
                    }

                    if (month == empMonth && (empDay - day) <= SettingDAL.GetIntValue("生日提醒天数") && (empDay - day) > 0)//有人要过生日
                    {
                        MessageBox.Show("还有"+(empDay-day)+"天是你的同事" + item.Name + "的生日，赶快送上祝福吧！");
                        continue;
                    }
                }
            }

        }

        private void miEmployeeMng_Click(object sender, RoutedEventArgs e)
        {
            EmployeeListWindow empListWin = new EmployeeListWindow();
            empListWin.ShowDialog();
        }

       

        private void miSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow win = new SettingWindow();
            win.ShowDialog();
        }

        private void miViewLog_Click(object sender, RoutedEventArgs e)
        {
            OperationLogWindow win = new OperationLogWindow();
            win.ShowDialog();
        }

        private void miCreateSalarySheet_Click(object sender, RoutedEventArgs e)
        {
            SalarySheetWindow win = new SalarySheetWindow();
            win.ShowDialog();
        }

        private void miPrintSalarySheet_Click(object sender, RoutedEventArgs e)
        {
            PrintSalarySheetWindow win = new PrintSalarySheetWindow();
            win.ShowDialog();
        }

        private void miCancel_Click(object sender, RoutedEventArgs e)
        {
            this.login.Show();
            this.Close();
        }

        private void miQuit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void aboutUs_Click(object sender, RoutedEventArgs e)
        {
            aboutUsWindow aus = new aboutUsWindow();
            aus.Show();
        }

        private void aboutSoftWare_Click(object sender, RoutedEventArgs e)
        {
            aboutSoftwWindow asw = new aboutSoftwWindow();
            asw.Show();
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnOperatorManage_Click(object sender, RoutedEventArgs e)
        {
            OperatorListUI listUi = new OperatorListUI();
            listUi.ShowDialog();
        }

        private void btnViewLog_Click(object sender, RoutedEventArgs e)
        {
            OperationLogWindow win = new OperationLogWindow();
            win.ShowDialog();
        }

        private void btnSystemSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow win = new SettingWindow();
            win.ShowDialog();
        }

        private void btnEmployeeManage_Click(object sender, RoutedEventArgs e)
        {
            EmployeeListWindow empListWin = new EmployeeListWindow();
            empListWin.ShowDialog();
        }

        private void btnCreateSalSheet_Click(object sender, RoutedEventArgs e)
        {
            SalarySheetWindow win = new SalarySheetWindow();
            win.ShowDialog();
        }

        private void btnprintSalSheet_Click(object sender, RoutedEventArgs e)
        {
            PrintSalarySheetWindow win = new PrintSalarySheetWindow();
            win.ShowDialog();
        }

      

       

        private void btnViewLog_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ImageBrush b = new ImageBrush();

            b.ImageSource = new BitmapImage(new Uri("pack://application:,,,/backgroundImage/manage1.jpg"));

            b.Stretch = Stretch.Fill;

            //this.Background = b;
            btnViewLog.Background = b;
        }
    }
}
