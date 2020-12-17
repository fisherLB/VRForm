
using GraphicsEvaluatePlatform.Client.Views;
using GraphicsEvaluatePlatform.Client.Views.DetectionReport;
using GraphicsEvaluatePlatform.Client.Views.ImportData;
using GraphicsEvaluatePlatform.Client.Views.PictrueSampling;
using GraphicsEvaluatePlatform.Client.Views.StartUp;
using GraphicsEvaluatePlatform.Client.Views.wpfDetection;
using GraphicsEvaluatePlatform.Client.Views.wpfPicture;
using GraphicsEvaluatePlatform.Client.Views.wpfProcess;
using GraphicsEvaluatePlatform.Client.Views.wpfProject;
using System.Windows;
using System.Windows.Controls;


namespace GraphicsEvaluatePlatform.Client.ViewModels
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
            this.Loaded += ShowTab;
            this.DataContext = new MainWindowViewModel();
        }
        //private int i = 1;
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            UCTabItemWithClose ti = new UCTabItemWithClose();
            var item = (System.Windows.Controls.MenuItem)sender;
            string h = item.Header.ToString();
            ti.Header = ((System.Windows.Controls.MenuItem)sender).Header;//新选项卡的名字
            Frame tabFrame = new Frame();
            foreach (TabItem tempItem in TabControl1.Items)  //这里的rightpanel即为右边的TabControl
            {

                if (tempItem.Header.ToString() == h)
                {
                    TabControl1.SelectedItem = tempItem;
                    return;
                }
            }
            dynamic uspage=null;
            if (h == "用户管理")
            {
                uspage = new userManage();
            }else if (h == "项目管理")
            {
                uspage = new ProjectManage();
            }
            else if (h == "批量检测")
            {
                uspage = new BatchDetectionPic();
            }
            else if (h == "批量图像处理")
            {
                uspage = new BatchProcess ();
            }
            else if (h == "单张图像处理")
            {
                uspage = new SinglePictureProcess();
            }
            else if (h == "图像抽检")
            {
                uspage = new SamplingPage();
            }
            else if (h == "合格报表")
            {
                uspage =new QualifiedReport();
            }
            else if (h == "不合格报表")
            {
                uspage = new UnqualifiedReport();
            }

            tabFrame.Content = uspage;
            ti.Content = tabFrame;

            ti.Margin = new Thickness(0, 0, 1, 0);
            ti.Height = 28;
            TabControl1.Items.Add(ti);
            TabControl1.SelectedItem = ti;
            //TabControl1.SelectedIndex = i;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            ImportDataWindow win = new ImportDataWindow();
            win.ShowDialog();
        }
        //打开默认选项卡
        private void ShowTab(object sender, RoutedEventArgs e)
        {
            UCTabItemWithClose ti = new UCTabItemWithClose();
            ti.Header = "XXX-项目管理";
            Frame tabFrame = new Frame();
            StartPage uspage = new StartPage();
            tabFrame.Content = uspage;
            ti.Content = tabFrame;
            ti.Margin = new Thickness(0, 0, 1, 0);
            ti.Height = 28;
            TabControl1.Items.Add(ti);
            TabControl1.SelectedIndex = 0;
        }
    }
}
