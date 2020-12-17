using GraphicsEvaluatePlatform.Client.ViewModels;
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

namespace GraphicsEvaluatePlatform.Client.Views.UserWin
{
    /// <summary>
    /// addUserWin.xaml 的交互逻辑
    /// </summary>
    public partial class addUserWin : Window
    {
        /// <summary>
        /// 确定
        /// </summary>
        public addUserWin()
        {
            InitializeComponent();
            this.DataContext = new AddOrModifyUserViewModel(this);
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancel02_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
