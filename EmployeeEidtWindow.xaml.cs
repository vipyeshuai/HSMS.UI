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
using System.Windows.Media.Effects;
using Microsoft.Win32;
using System.IO;

namespace HSMS.UI
{
    /// <summary>
    /// EmployeeEidtWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EmployeeEidtWindow : Window
    {
        public EmployeeEidtWindow()
        {
            InitializeComponent();
        }

        public Guid EditingId { set; get; }
        public bool IsAddNew { set; get; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbDepartment.ItemsSource = DepartmentDAL.ListALL();
            cbEducation.ItemsSource = IdNameDAL.GetByCatagory("学历");
            cbGender.ItemsSource = IdNameDAL.GetByCatagory("性别");
            cbMarriage.ItemsSource = IdNameDAL.GetByCatagory("婚姻状况");
            cbPartyStatus.ItemsSource = IdNameDAL.GetByCatagory("政治面貌");
            if (IsAddNew)
            {
                Employee employee = new Employee();
                employee.InDate = DateTime.Today;
                employee.ContractStartDay = DateTime.Today;
                employee.ContractEndDay = DateTime.Today.AddYears(1);
                employee.Nationality = "汉族";
                employee.Email = "836246914@qq.com";
                //employee.Number = "Ideas";
                employee.Number = SettingDAL.GetValue("员工工号前缀");
                gridEmployee.DataContext = employee;

            }
            else
            {
                Employee employee = EmployeeDAL.GetById(EditingId);
                gridEmployee.DataContext = employee;

                if (employee.Photo!= null)
                {
                    ShowImage(employee.Photo);
                }
            }
        }
        private void ShowImage(byte[] imageByte)
        {
            MemoryStream stream = new MemoryStream(imageByte);
            BitmapImage bitImage = new BitmapImage();
            bitImage.BeginInit();
            bitImage.StreamSource = stream;
            bitImage.EndInit();
            imgPhoto.Source = bitImage;
        }

        private void CheckTextBoxNotNull(ref bool IsOk,params TextBox[] textBoxs)
        {
            foreach (TextBox textBox in textBoxs)
            {
                if (textBox.Text.Length <= 0)
                {
                    IsOk = false;
                    textBox.Background = Brushes.Red;
                }
                else
                {
                    textBox.Background = null;
                }
            }
        }
        private void CheckComboBoxNotNull(ref bool IsOk, params ComboBox[] cbxs)
        {
            foreach (ComboBox cbx in cbxs)
            {
                if (cbx.SelectedIndex<0)
                {
                    IsOk = false;
                    //cbx.Effect=new DropShadow
                    cbx.Effect = new DropShadowEffect { Color=Colors.Red};
                }
                else
                {
                    cbx.Background = null;
                }
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool IsOK = true;
            CheckTextBoxNotNull(ref IsOK, txtName, txtNational, txtNativeAddr, txtAddr,
             txtBaseSalary, txtTelNum, txtIdNum, txtPosition, txtNumber);
            CheckComboBoxNotNull(ref IsOK, cbGender, cbMarriage,
                cbPartyStatus, cbEducation, cbDepartment);

            if (IsOK == false)
                return;

            Guid operatorId = CommonHelper.GetOperatorId();
           
            if (IsAddNew)
            {
                Employee employee = (Employee)gridEmployee.DataContext;
                if (employee.Photo == null)
                    EmployeeDAL.InsertImageNull(employee);
                else
                    EmployeeDAL.Insert(employee);
                T_OperationLogDAL.Insert(operatorId, "新增职员" + employee.Name);
            }
            else
            {
                Employee employee = (Employee)gridEmployee.DataContext;
                if (employee.Photo == null)
                    EmployeeDAL.UpdateImageNull(employee);
                else
                    EmployeeDAL.Update(employee);
                T_OperationLogDAL.Insert(operatorId, "修改职员" + employee.Name);

            }
            DialogResult = true;
        }

        private void btnChoosePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "png图片|*.png|jpg图片|*.jpg";
            if (ofd.ShowDialog() == true)
            {
                string fileName = ofd.FileName;
                Employee employee = (Employee)gridEmployee.DataContext;
                employee.Photo = File.ReadAllBytes(fileName);
                imgPhoto.Source = new BitmapImage(new Uri(fileName));
                    
            }

        }

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            CaptureWindow win = new CaptureWindow();
            if (win.ShowDialog() == true)
            {
                ShowImage(win.CaptureData);
                Employee employee = (Employee)gridEmployee.DataContext;
                employee.Photo = win.CaptureData;
            }
        }

       
    }
}
