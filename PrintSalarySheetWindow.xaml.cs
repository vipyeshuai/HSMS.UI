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
    /// PrintSalarySheetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PrintSalarySheetWindow : Window
    {
        public PrintSalarySheetWindow()
        {
            InitializeComponent();

            ImageBrush b = new ImageBrush();

            b.ImageSource = new BitmapImage(new Uri("pack://application:,,,/backgroundImage/manage1.jpg"));

            b.Stretch = Stretch.Fill;

            this.Background = b;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbDept.ItemsSource = DepartmentDAL.ListALL();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            int year = Convert.ToInt32(txtYear.Text);
            int month = Convert.ToInt32(txtMonth.Text);
            Guid DeptId = (Guid)cmbDept.SelectedValue;

            if (SalarySheetDAL.IsExist(year, month, DeptId) == false)
            {
                MessageBox.Show("还没有生成工资表！");
                return;
            }

            SalarySheetItem[] salarySheetItems = SalarySheetDAL.GetSalarySheetItem(year, month, DeptId);

            SalarySheetItemrpt[] salarySheetItemrpts = new SalarySheetItemrpt[salarySheetItems.Length];

            for (int i = 0; i < salarySheetItems.Length; i++)
            {
                SalarySheetItem item = salarySheetItems[i];
                SalarySheetItemrpt itemRpt = new SalarySheetItemrpt();
                itemRpt.BaseSalary = item.BaseSalary;
                itemRpt.Bonus = item.Bonus;
                itemRpt.Fine = item.Fine;
                itemRpt.Other = item.Other;

                itemRpt.EmployeeName = EmployeeDAL.GetById(item.EmployeeId).Name;
                salarySheetItemrpts[i] = itemRpt;
            }

            SalarySheetItemCrystalReport rpt = new SalarySheetItemCrystalReport();
            rpt.SetDataSource(salarySheetItemrpts);
            rpt.SetParameterValue("年", year);
            rpt.SetParameterValue("月", month);
            rpt.SetParameterValue("部门", cmbDept.Text);

            crystalReportsViewer1.ViewerCore.ReportSource = rpt;


        }
    }
}
