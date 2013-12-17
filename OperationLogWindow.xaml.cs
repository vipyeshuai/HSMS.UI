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
using System.Data.SqlClient;
using HSMS.Model;
using HSMS.DAL;

namespace HSMS.UI.SystemMgr
{
    /// <summary>
    /// OperationLogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OperationLogWindow : Window
    {
        public OperationLogWindow()
        {
            InitializeComponent();

            ImageBrush b = new ImageBrush();

            b.ImageSource = new BitmapImage(new Uri("pack://application:,,,/backgroundImage/manage1.jpg"));

            b.Stretch = Stretch.Fill;

            this.Background = b;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<string> whereList = new List<string>();
            List<SqlParameter> parametersList = new List<SqlParameter>();

            if (cbSearchByOperator.IsChecked == true)
            {
                if (cmbOperator.SelectedIndex <0)
                {
                    MessageBox.Show("请选择操作员");
                    return;
                }
                whereList.Add("OperatorId=@OperatorId");
                parametersList.Add(new SqlParameter("@OperatorId", cmbOperator.SelectedValue));
            }

            if (cbSearchByMakeDate.IsChecked == true)
            {
                if (dpStartDate.SelectedDate == null || dpEndDate.SelectedDate == null)
                {
                    MessageBox.Show("请选择要查询的日期");
                    return;
                }
                whereList.Add("MakeDate between @StartDate and @EndDate");
                parametersList.Add(new SqlParameter("@StartDate", dpStartDate.SelectedDate));
                parametersList.Add(new SqlParameter("@EndDate", dpEndDate.SelectedDate));
            }
            if (cbSearchByAction.IsChecked == true)
            {
                whereList.Add("ActionDesc like @ActionDesc");
                parametersList.Add(new SqlParameter("@ActionDesc", "%" + txtActionDesc.Text + "%"));
            }

            if (whereList.Count <= 0)
            {
                MessageBox.Show("请至少选择一种查询条件");
                return;
            }

            string sql = "select * from T_OperationLog where " + String.Join(" and ", whereList);

            T_OperationLog[] operationLogs = T_OperationLogDAL.Search(sql, parametersList.ToArray());
            datagrid.ItemsSource = operationLogs;


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Operator[] operators = OperatorDAL.ListAll();
            cmbOperator.ItemsSource = operators;
            operatorColumn.ItemsSource = operators;
        }
    }
}
