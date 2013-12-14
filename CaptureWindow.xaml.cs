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
using WPFMediaKit.DirectShow.Controls;
using System.IO;

namespace HSMS.UI
{
    /// <summary>
    /// CaptureWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CaptureWindow : Window
    {
        public CaptureWindow()
        {
            InitializeComponent();
        }
        public byte[] CaptureData { get; set; }
        private void cbCameras_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            captureElement.VideoCaptureSource = (string)cbCameras.SelectedItem;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbCameras.ItemsSource = MultimediaUtil.VideoInputNames;
            if (MultimediaUtil.VideoInputNames.Length > 0)
            {
                cbCameras.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("电脑没有安装任何摄像头！");
            }
        }

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            
            
            RenderTargetBitmap bmp = new RenderTargetBitmap(
                (int)captureElement.ActualWidth, (int)captureElement.ActualHeight,
                96, 96, PixelFormats.Default);
            bmp.Render(captureElement);
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                CaptureData = ms.ToArray();
            }
           captureElement.Pause();
            
            
        }

        private void btnCaptureAgain_Click(object sender, RoutedEventArgs e)
        {
            captureElement.Play();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
