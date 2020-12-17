/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Client.Models;
using GraphicsEvaluatePlatform.Client.Repository;
using GraphicsEvaluatePlatform.Client.Services;
using GraphicsEvaluatePlatform.Client.Views;
using GraphicsEvaluatePlatform.Infrastructure;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;



/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Client.ViewModels
 * 项目描述: 
 * 类 名 称: LoginViewModel
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Client.ViewModels
 * 文件名称: LoginViewModel
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/18 10:05:04
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Client.ViewModels
{
    class LoginViewModel : NotificationObject
    {
        public DelegateCommand loginOnCommand { get; set; }  //登录命令
        public System.Windows.Input.ICommand OnClosed { get; set; }//取消
        public DelegateCommand SetDbConfig { get; set; }//数据库配置

        public DelegateCommand SynchronousDataCommand { get; set; }//同步数据

        private ILoginService loginService;
        private Window window { get; set; }

        private string _username;
        private string _password;
        private bool _ischecked;
        private string _itemvisible;//是否显示版本更新信息
        private string _versiontext;//版本更新显示的文字

        public string ItemVisible
        {
            get { return _itemvisible; }
            set
            {
                _itemvisible = value;
                this.RaisePropertyChanged("ItemVisible");
            }
        }

        public string VersionText
        {
            get { return _versiontext; }
            set
            {
                _versiontext = value;
                this.RaisePropertyChanged("VersionText");
            }
        }


        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value;
                this.RaisePropertyChanged("UserName");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                this.RaisePropertyChanged("Password");
            }
        }

        public bool IsChecked
        {
            get { return _ischecked; }
            set { _ischecked = value; this.RaisePropertyChanged("IsChecked"); }
        }


        /// <summary>
        ///构造函数
        /// </summary>
        public LoginViewModel(object window)
        {
            loginService = new LoginService();
            this.loadUser();
            this.checkVersion();
            this.loginOnCommand = new DelegateCommand(new Action(this.LoginOn));//登录
            this.SynchronousDataCommand = new DelegateCommand(new Action(this.SynchronousData));//同步数据

            this.window = (Window)window;
            //取消
            this.OnClosed = new DelegateCommand<string>(str =>
            {
                MessageBoxResult mdr = MessageBox.Show("确定要取消吗?", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (mdr == MessageBoxResult.OK)
                {
                    this.window.Close();
                }
            });
            this.SetDbConfig = new DelegateCommand(new Action(Set_DbConfigFrom));
        }

        /// <summary>
        /// 数据库配置
        /// </summary>
        private void Set_DbConfigFrom()
        {
            SetdbconfigFrom f = new SetdbconfigFrom();
            f.ShowDialog();
        }

        //登录
        private void LoginOn()
        {

            var re = loginService.Login(new LoginModel() { UserName = UserName, Password = Password, IsRemember = _ischecked });
            if (re.ResultType != OperationResultType.Success)
            {
                MessageBox.Show(re.Message);
                return;
            }

            //打开加载窗体
            Window loadingwin = new LoadingWindow();
            loadingwin.Show();
            //关闭登录窗体
            this.window.Close();
            System.Threading.Thread.Sleep(1000);
            //打开主界面
            Window mainWindow = new MainWindow();
            mainWindow.Show();
            loadingwin.Close();
        }

        //加载最近一次登录的用户
        private void loadUser()
        {
            var model = loginService.LoadLoginUser();
            if (model != null)
            {
                this.UserName = model.UserName;
                this.Password = model.Password;
                this.IsChecked = model.IsRemember;
            }
        }

        //检测数据版本
        private void checkVersion()
        {
            var ret = loginService.CheckVersion();
            if (ret.ResultType == OperationResultType.Success)
            {
                if ((int)ret.AppendData == -1)//需要初始化
                {
                    this.ItemVisible = "Visible";
                    this.VersionText = "登录系统前需要进行初始化";
                }
                else if ((int)ret.AppendData == 0)//不需要更新版本
                {
                    this.ItemVisible = "Collapsed";                  
                }
                else {//需要更新版本
                    this.ItemVisible = "Visible";
                    this.VersionText = "版本信息有更新，登录系统前请同步系统数据！";
                }
            }                       
        }

        //数据同步
        public void SynchronousData() {
            var ret = loginService.SynchronousData();

                MessageBox.Show(ret.AppendData.ToString());

        }
    }
}
