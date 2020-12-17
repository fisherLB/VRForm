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

namespace GraphicsEvaluatePlatform.Client.Views
{
    /// <summary>
    /// SetdbconfigFrom.xaml 的交互逻辑
    /// </summary>
    public partial class SetdbconfigFrom : Window
    {
        public SetdbconfigFrom()
        {
            InitializeComponent();
            this.DataContext = new DbConfigViewModel();
        }
    }
}
