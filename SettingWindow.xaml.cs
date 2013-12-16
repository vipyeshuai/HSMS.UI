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
using HSMS.DAL;

namespace HSMS.UI.SystemMgr
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SettingDAL.SetValue("公司名称", txtCompanyName.Text);
            SettingDAL.SetValue("公司网站", txtCompanySite.Text);
            SettingDAL.SetValue("生日提醒天数",txtBirthdayCount.Text);
            SettingDAL.SetValue("员工工号前缀", txtNumberPrefix.Text);
            SettingDAL.SetValue("启用生日提醒", (bool)cbStartBirthdayRemind.IsChecked);

            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtCompanyName.Text = SettingDAL.GetValue("公司名称");
            txtCompanySite.Text = SettingDAL.GetValue("公司网站");
            txtBirthdayCount.Text = SettingDAL.GetValue("生日提醒天数");
            cbStartBirthdayRemind.IsChecked = SettingDAL.GetBoolValue("启用生日提醒");
            txtNumberPrefix.Text = SettingDAL.GetValue("员工工号前缀");
        }
    }
}
