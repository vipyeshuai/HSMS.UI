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
using System.Windows.Shapes;
using HSMS.Model;
using HSMS.DAL;

namespace HSMS.UI
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            ImageBrush b = new ImageBrush();

            b.ImageSource = new BitmapImage(new Uri("pack://application:,,,/backgroundImage/Login1.jpg"));

            b.Stretch = Stretch.Fill;

            this.Background = b;


        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userName = tbxUserName.Text;
            string password = pdxPassword.Password;

            Operator op = OperatorDAL.GetByUserName(userName);

            if (op == null)
            {
                MessageBox.Show("用户名或密码错误");
                return;
            }
            else if (op.Password == password)
            {
                T_OperationLogDAL.Insert(op.Id, op.RealName + "登录成功！");
                Application.Current.Properties["OperatorID"] = op.Id;
                // MessageBox.Show("登录成功！");
                //DialogResult = true;
                MainWindow mainWin = new MainWindow();
                mainWin.Login = this;
                this.Hide();
                mainWin.Show();
            }
            else
            {
                T_OperationLogDAL.Insert(op.Id, op.RealName + "登录失败！");

                MessageBox.Show("用户名或密码错误");
            }
              //  DialogResult = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
