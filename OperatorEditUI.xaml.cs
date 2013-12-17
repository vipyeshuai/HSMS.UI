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

namespace HSMS.UI.SystemMgr
{
    /// <summary>
    /// OperatorEditUI.xaml 的交互逻辑
    /// </summary>
    public partial class OperatorEditUI : Window
    {
        public OperatorEditUI()
        {
            InitializeComponent();
        }
        public bool IsInsert { set; get; }
        public  Guid EditId { set; get; }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Guid operatorId = CommonHelper.GetOperatorId();
            if (IsInsert)
            {
                Operator op = new Operator();
                op.UserName = txtUserName.Text;
                op.RealName = txtRealName.Text;
                op.Password = pdxPassword.Password;
                OperatorDAL.Insert(op);
               
                T_OperationLogDAL.Insert(operatorId, "插入操作员" + op.UserName);
            }
            else 
            {
                string password = pdxPassword.Password;
                string userName = txtUserName.Text;
                if (password.Length <= 0)
                {
                    OperatorDAL.Update(EditId, txtUserName.Text, txtRealName.Text);
                    //OperatorDAL.Update(EditId, null, txtRealName.Text);
                }
                else
                {
                    //OperatorDAL.Update(EditId, txtUserName.Text, txtRealName.Text, pdxPassword.Password);
                    OperatorDAL.Update(EditId, null, txtRealName.Text, pdxPassword.Password);

                }
                T_OperationLogDAL.Insert(operatorId, "修改操作员" + userName);
            }
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsInsert)
            {
            }
            else
            {
                Operator op = OperatorDAL.GetById(EditId);
                txtUserName.Text = op.UserName;
                txtRealName.Text = op.RealName;
                pdxPassword.Password = op.Password;
            }
        }
    }
}
