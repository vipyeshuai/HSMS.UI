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

namespace HSMS.UI
{
    /// <summary>
    /// SalarySheetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SalarySheetWindow : Window
    {
        public SalarySheetWindow()
        {
            InitializeComponent();

            ImageBrush b = new ImageBrush();

            b.ImageSource = new BitmapImage(new Uri("pack://application:,,,/backgroundImage/manage1.jpg"));

            b.Stretch = Stretch.Fill;

            this.Background = b;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<int> listYear = new List<int>();
            for (int i = DateTime.Now.Year - 5; i <= DateTime.Now.Year + 5; i++)
            {
                listYear.Add(i);
            }
            List<int> listMonth = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                listMonth.Add(i);
            }

            cmbYear.ItemsSource = listYear;
            cmbYear.SelectedIndex = 4;
            cmbMonth.ItemsSource = listMonth;
            cmbMonth.SelectedIndex = 11;


            cmbDept.ItemsSource = DepartmentDAL.ListALL();
            cmbDept.SelectedIndex = 3;
        }

        private void btnCreateSalarySheet_Click(object sender, RoutedEventArgs e)
        {
            int year=(int)cmbYear.SelectedItem;
            int month=(int)cmbMonth.SelectedItem;
            Guid DeptId=(Guid)cmbDept.SelectedValue;
            if (SalarySheetDAL.IsExist(year, month, DeptId))
            {
                if (MessageBox.Show("工资表已存在，是否重新生成？", "提示",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    SalarySheetDAL.DeleteSalarySheet(year, month, DeptId);
                }
                SalarySheetDAL.Build(year, month, DeptId);
                //MessageBox.Show("生成成功！");
                colEmployee.ItemsSource = EmployeeDAL.GetEmployeeByDept(DeptId);
                dataGridSalaryItems.ItemsSource = SalarySheetDAL.GetSalarySheetItem(year, month, DeptId);
            }
        }

        private void dataGridSalaryItems_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            SalarySheetItem item = (SalarySheetItem)e.Row.DataContext;

            SalarySheetDAL.UpdateSalarySheet(item);
        }
    }
}
