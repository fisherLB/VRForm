using GraphicsEvaluatePlatform.Client.Common;
using GraphicsEvaluatePlatform.Client.ViewModels;
using System;
using System.IO;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFBinarizationLib;

namespace GraphicsEvaluatePlatform.Client.Views.wpfProcess
{
    /// <summary>
    /// SinglePictureProcess.xaml 的交互逻辑
    /// </summary>
    public partial class SinglePictureProcess : Page
    {
        public SinglePictureProcess()
        {
            InitializeComponent();
            this.DataContext = new ProcessViewModel(this);
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBox_Copy3.Text = ((Slider)sender).Value.ToString();
        }

        ////获取测试图像目录
        //private void buttonFileOpen_Click(object sender, RoutedEventArgs e)
        //{
        //    // 文件夹选择对话框  
        //    FolderBrowserDialog dialog = new FolderBrowserDialog();

        //    // 不显示创建新文件夹按钮  
        //    dialog.ShowNewFolderButton = false;

        //    // 设置初始目录  
        //    //dialog.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;

        //    // 获取用户选择文件夹  
        //    if (dialog.ShowDialog() == DialogResult.OK)
        //    {   // 显示用户选择文件夹  
        //        textBlock1.Text = dialog.SelectedPath;
        //        textBlock1.ToolTip = textBlock1.Text;

        //        // 获取所有图像文件列表  
        //        String[] images = Folder.GetImages(textBlock1.Text, SearchOption.TopDirectoryOnly);
        //        if (images != null)
        //        {   // 更新图像显示列表区  
        //            // Folder.DisplayImages(stackPanel_Showcase, images, ImageMouseDown);
        //            //foreach (var FileName in images)
        //            {
        //                image.Source = new BitmapImage(new Uri(images[0]));

        //                // 获取位图图像  
        //                BitmapImage bitmap = image.Source as BitmapImage;
        //                if (bitmap == null)
        //                    return;
        //                image1.Source = bitmap.ToGrayBitmap();
        //            }
        //        }
        //    }
        //}

        //// 图像框鼠标点击事件处理  
        //private void ImageMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
        //    {   // 获取图像控件  
        //        if (!(sender is Image image))
        //            return;

        //        Border SelectedBorder = image.Parent as Border;
        //        StackPanel panel = SelectedBorder.Parent as StackPanel;
        //        foreach (Border item in panel.Children)
        //        {
        //            if (item == SelectedBorder)
        //            {   // 红色边框  
        //                item.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
        //            }
        //            else
        //            {   // 墨绿边框  
        //                item.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkGreen);
        //            }
        //        }

        //        // 获取位图图像  
        //        BitmapImage bitmap = image.Source as BitmapImage;
        //        if (bitmap == null)
        //            return;

        //        // 显示灰度图像  
        //        image1.Source = bitmap;

        //        // 显示大津法二值化图像  
        //        Int32 Threshold;
        //        image2.Source = bitmap.ToGrayBitmap();
        //        //                image2.Source = bitmap.ToBinaryBitmap(BinarizationMethods.Otsu, out Threshold);
        //        //                OstuTextBlock.Text = Threshold.ToString();

        //        // 显示迭代法二值化图像  
        //        //image3.Source = bitmap.ToBinaryBitmap(BinarizationMethods.Iterative, out Threshold);
        //        //IterativeTextBlock.Text = Threshold.ToString();
        //    }
        //}


    }
}
