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

namespace HSMS.UI.softWHelpWindow
{
    /// <summary>
    /// aboutUsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class aboutUsWindow : Window
    {
        public aboutUsWindow()
        {
            InitializeComponent();

            ImageBrush b = new ImageBrush();

            b.ImageSource = new BitmapImage(new Uri("pack://application:,,,/backgroundImage/manage1.jpg"));

            b.Stretch = Stretch.Fill;

            this.Background = b;
        }
    }
}
