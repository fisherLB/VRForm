using GraphicsEvaluatePlatform.Client.ViewModels;
using GraphicsEvaluatePlatform.Client.Views.ProjectWindow;
using GraphicsEvaluatePlatform.Client.Views.UserWin;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicsEvaluatePlatform.Client.Views
{
    /// <summary>
    /// userManage.xaml 的交互逻辑
    /// </summary>
    public partial class userManage : Page
    {
        public userManage()
        {
            InitializeComponent();
            PagerViewModel pagerViewModel = this.userPager.pagerViewModel;
            this.DataContext = new UserViewModel(this, pagerViewModel);
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            UserControl1 demo = new UserControl1();
            this.grid.Children.Add(demo);
        }
        ///// <summary>
        ///// 新增
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void add_Click_1(object sender, RoutedEventArgs e)
        //{
        //    addUserWin win = new addUserWin();
        //    win.Show();
        //    //UserControl1 demo = new UserControl1();
        //    //this.grid.Children.Add(demo);
        //}
        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void edit_Click_1(object sender, RoutedEventArgs e)
        //{
        //    updataUserWin win = new updataUserWin();
        //    win.Show();
        //}
    }
}
