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
using System.Data.SqlClient;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace HSMS.UI
{
    /// <summary>
    /// EmployeeListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EmployeeListWindow : Window
    {
        public EmployeeListWindow()
        {
            InitializeComponent();

            ImageBrush b = new ImageBrush();

            b.ImageSource = new BitmapImage(new Uri("pack://application:,,,/backgroundImage/manage1.jpg"));

            b.Stretch = Stretch.Fill;

            this.Background = b;
        }

        private  void LoadData()
        {
            datagrid.ItemsSource = EmployeeDAL.ListAll();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            columnEducationId.ItemsSource = IdNameDAL.GetByCatagory("学历");
            columnDepartmentId.ItemsSource = DepartmentDAL.ListALL();
            LoadData();

            cbSearchByName.IsChecked = true;
            dpInDateStart.SelectedDate = DateTime.Today.AddMonths(-1);
            dpEndDate.SelectedDate = DateTime.Today;

            cmbDept.ItemsSource = DepartmentDAL.ListALL();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EmployeeEidtWindow edit = new EmployeeEidtWindow();
            edit.IsAddNew = true;
            if (edit.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            EmployeeEidtWindow edit = new EmployeeEidtWindow();
            Employee employee=(Employee)datagrid.SelectedItem;
            if (employee == null)
            {
                MessageBox.Show("请选择要编辑的对象");
                return;
            }
            edit.IsAddNew = false;
            edit.EditingId = employee.Id;
            if (edit.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = (Employee)datagrid.SelectedItem;
            if (employee == null)
            {
                MessageBox.Show("请选择要删除的对象");
                return;
            }
            else
            {
                Guid operatorId = CommonHelper.GetOperatorId();
                T_OperationLogDAL.Insert(operatorId, "删除职员" + employee.Name);
                EmployeeDAL.Delete(employee.Id);
                LoadData();
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<string> whereList = new List<string>();
            List<SqlParameter> sqlList = new List<SqlParameter>();

            if (cbSearchByName.IsChecked == true)
            {
                whereList.Add("Name=@Name");
                sqlList.Add(new SqlParameter("@Name", txtName.Text));
            }
            if (cbSearchByInDate.IsChecked == true)
            {
                whereList.Add("InDate>=@InDateStart and InDate<=@InDateEnd");
                sqlList.Add(new SqlParameter("@InDateStart", dpInDateStart.SelectedDate));
                sqlList.Add(new SqlParameter("@InDateEnd", dpEndDate.SelectedDate));

            }
            if (cbSearchByDept.IsChecked == true)
            {
                whereList.Add("DepartmentId=@DepartmentId");
                sqlList.Add(new SqlParameter("@DepartmentId", cmbDept.SelectedValue));
            }

            string whereSql = string.Join(" and ", whereList);
            string sql = "select * from T_Employee";
            if (whereSql.Length > 0)
            {
                sql = sql + " where " + whereSql;
            }
            datagrid.ItemsSource = EmployeeDAL.Search(sql, sqlList);
        }

        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "excel文件|*.xls";
            if (sfd.ShowDialog() != true)
            {
                return;
            }
            string fileName = sfd.FileName;
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("员工信息表");
            IRow rowHeader = sheet.CreateRow(0);
            rowHeader.CreateCell(0, CellType.STRING).SetCellValue("姓名");
            rowHeader.CreateCell(1, CellType.STRING).SetCellValue("工号");
            rowHeader.CreateCell(2, CellType.STRING).SetCellValue("入职时间");
            rowHeader.CreateCell(3, CellType.STRING).SetCellValue("学历");
            rowHeader.CreateCell(4, CellType.STRING).SetCellValue("毕业院校");
            rowHeader.CreateCell(5, CellType.STRING).SetCellValue("基本工资");
            rowHeader.CreateCell(6, CellType.STRING).SetCellValue("部门");
            rowHeader.CreateCell(7, CellType.STRING).SetCellValue("职位");
            rowHeader.CreateCell(8, CellType.STRING).SetCellValue("合同签订日期");
            rowHeader.CreateCell(9, CellType.STRING).SetCellValue("合同到期日期");

            Employee[] employees = (Employee[])datagrid.ItemsSource;

            ICellStyle cellStyle = workbook.CreateCellStyle();
            IDataFormat dataFormat = workbook.CreateDataFormat();

            cellStyle.DataFormat = dataFormat.GetFormat("yyyy\"年\"m\"月\"d\"日\"");

            for (int i = 0; i < employees.Length; i++)
            {
                Employee employee = employees[i];
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0, CellType.STRING).SetCellValue(employee.Name);
                row.CreateCell(1, CellType.STRING).SetCellValue(employee.Number);
                ICell dateCell = row.CreateCell(2, CellType.NUMERIC);
                dateCell.CellStyle = cellStyle;
                dateCell.SetCellValue(employee.InDate);

                row.CreateCell(3, CellType.STRING).SetCellValue(IdNameDAL.GetEducationNameById(employee.EducationId));
                row.CreateCell(4, CellType.STRING).SetCellValue(employee.School);
                row.CreateCell(5, CellType.STRING).SetCellValue(employee.BaseSalary);
                row.CreateCell(6, CellType.STRING).SetCellValue(DepartmentDAL.GetById(employee.DepartmentId).Name);
                row.CreateCell(7, CellType.STRING).SetCellValue(employee.Position);
                ICell contractBegindateCell = row.CreateCell(8, CellType.NUMERIC);
                contractBegindateCell.CellStyle = cellStyle;
                contractBegindateCell.SetCellValue(employee.ContractStartDay);

                ICell contractEnddateCell = row.CreateCell(9, CellType.NUMERIC);
                contractEnddateCell.CellStyle = cellStyle;
                contractEnddateCell.SetCellValue(employee.ContractEndDay);
                
            }

            using (Stream stream = File.OpenWrite(fileName))
            {
                workbook.Write(stream);
            }

        }

      
    }
}
