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
using HSMS.Model;

namespace HSMS.UI.SystemMgr
{
    /// <summary>
    /// OperatorListUI.xaml 的交互逻辑
    /// </summary>
    public partial class OperatorListUI : Window
    {
        public OperatorListUI()
        {
            InitializeComponent();

            ImageBrush b = new ImageBrush();

            b.ImageSource = new BitmapImage(new Uri("pack://application:,,,/backgroundImage/manage1.jpg"));

            b.Stretch = Stretch.Fill;

            this.Background = b;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            OperatorEditUI opEdit = new OperatorEditUI();
            opEdit.IsInsert = true;
            if (opEdit.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Operator op = (Operator)gridOperator.SelectedItem;
            if (op == null)
            {
                MessageBox.Show("请选择要删除的对象！");
                return;
            }
            if (MessageBox.Show("你真的要删除" + op.RealName + "吗？", "提醒", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Guid OperatorId = CommonHelper.GetOperatorId();
                T_OperationLogDAL.Insert(OperatorId, "删除" + op.UserName);
                OperatorDAL.DeleteById(op.Id);
                LoadData();
            }
        }

        private void btnEidt_Click(object sender, RoutedEventArgs e)
        {
            Operator op = (Operator)gridOperator.SelectedItem;
            if (op == null)
            {
                MessageBox.Show("请选择要编辑的对象！");
                return;
            }
            OperatorEditUI EditUI = new OperatorEditUI();
            EditUI.IsInsert=false;
            EditUI.EditId=op.Id;
            if (EditUI.ShowDialog() == true)
            {
                LoadData();
            }
        }
        private void LoadData()
        {
            gridOperator.ItemsSource = OperatorDAL.ListAll();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void gridOperator_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
