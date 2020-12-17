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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicsEvaluatePlatform.Client.Views.ProjectWindow
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            //this.DataContext = new AddUserViewModel();
        }

        private void cancel02_Click(object sender, RoutedEventArgs e)
        {
            this.grid.Children.Clear();
            //UserControl.Clear();
            //new userManage().grid.Children.Clear();
            //this.Visibility = System.Windows.Visibility.Hidden;
            //new userManage().grid.Children.Clear();
        }

        private void close02_Click(object sender, RoutedEventArgs e)
        {
            this.grid.Children.Clear();
            //this.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
