/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using GraphicsEvaluatePlatform.Client.Basics;
using Microsoft.Practices.Prism.Commands;
using System.Windows;

/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Client.ViewModels
 * 项目描述: 
 * 类 名 称: MainWindowViewModel
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Client.ViewModels
 * 文件名称: MainWindowViewModel
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/21 16:39:54
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Client.ViewModels
{
    class MainWindowViewModel : NotificationObject
    {

        public DelegateCommand LoginOutCommand { get; set; }
        private string currentLoginUser;
        public string CurrentLoginUser
        {
            get { return currentLoginUser; }
            set
            {
                currentLoginUser = value;
                this.RaisePropertyChanged("CurrentLoginUser");
            }
        }

        #region 用户管理菜单标识
        private string userMangerVisibility;
        public string UserMangerVisibility
        {
            get { return userMangerVisibility; }
            set
            {
                userMangerVisibility = value;
                this.RaisePropertyChanged("UserMangerVisibility");
            }
        }
        #endregion
        #region  检测设置菜单标识

        private string detectionSettingVisibility;
        public string DetectionSettingVisibility
        {
            get { return detectionSettingVisibility; }
            set
            {
                detectionSettingVisibility = value;
                this.RaisePropertyChanged("DetectionSettingVisibility");
            }
        }

        #endregion

        //构造函数
        public MainWindowViewModel()
        {
            this.CurrentLoginUser = "您好！" + BaseService.USERNAME;
            #region  根据是单机版还是网络版控制菜单显示
            if (BaseService.USEVERSION.ToString().ToLower() == "network")
            {   //网络版
                this.UserMangerVisibility = "Collapsed";
                this.DetectionSettingVisibility = "Collapsed";
            }          
            #endregion
            this.LoginOutCommand = new DelegateCommand(new Action(this.LoginOut));
        }

        //登出
        private void LoginOut()
        {
            MessageBoxResult dr = MessageBox.Show("所有操作将被强制终此，你确认要退出客户端吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr == MessageBoxResult.OK)
            {
                Environment.Exit(0);// 强制退出，即使有其他的线程没有结束
            }
            else
            {
                return;
            }
        }


    }
}
