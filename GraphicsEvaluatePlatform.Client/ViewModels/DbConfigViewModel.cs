/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using GraphicsEvaluatePlatform.Repository;
using System.Windows;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Client.ViewModels
 * 项目描述: 
 * 类 名 称: DbConfigViewModel
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Client.ViewModels
 * 文件名称: DbConfigViewModel
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/22 16:56:43
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Client.ViewModels
{
    class DbConfigViewModel: NotificationObject
    {
        public DelegateCommand TestConnectDbCommand { get; set; }
        public DelegateCommand SaveConfigCommand { get; set; }
        private string _serverip;
        private string _dbname;
        private string _dbusername;
        private string _dbpassword;
        private string _port;
        public string ServerIp
        {
            get { return _serverip; }
            set
            {
                _serverip = value;
                this.RaisePropertyChanged("ServerIp");
            }
        }

        public string DbName
        {
            get { return _dbname; }
            set
            {
                _dbname = value;
                this.RaisePropertyChanged("DbName");
            }
        }

        public string  DbUserName
        {
            get { return _dbusername; }
            set
            {
                _dbusername = value;
                this.RaisePropertyChanged("DbUserName");
            }

        }

        public string DbPassword
        {
            get { return _dbpassword; }
            set
            {
                _dbpassword = value;
                this.RaisePropertyChanged("DbPassword");
            }
        }


        public string Port
        {
            get { return _port; }
            set
            {
                _port = value;
                this.RaisePropertyChanged("Port");
            }
        }


        public DbConfigViewModel()
        {
            this.loadDbconfig();
            this.SaveConfigCommand = new DelegateCommand(new Action(this.SaveConfig));
            this.TestConnectDbCommand = new DelegateCommand(new Action(this.TestConnectDb));
        }
        /// <summary>
        /// 测试链接数据库端口
        /// </summary>
        private void TestConnectDb()
        {
            try
            {
                string msg = "";
                string connectStr = "Host=" + this.ServerIp + ";UserName=" + this.DbUserName + ";Password=" + this.DbPassword + ";Database=" + this.DbName + ";Port=" + this.Port + ";CharSet=utf8;Allow Zero Datetime=true;sslmode=none;Old Guids=true;";
                DataBll.ConnectionTest(out msg, connectStr);
                MessageBox.Show(msg);
            }
            catch (Exception ex)
            {
                throw ex;
            }        }

        private void loadDbconfig()
        {
            var connestr = ConfigurationManager.AppSettings["MYSQLCONSTR"].ToString();
            var ConnectArr = connestr.Split(';');
            this.ServerIp = ConnectArr[0].Split('=')[1];
            this.DbUserName= ConnectArr[1].Split('=')[1];
            this.DbPassword= ConnectArr[2].Split('=')[1];
            this.DbName= ConnectArr[3].Split('=')[1];
            this.Port= ConnectArr[4].Split('=')[1] ;
        }
        /// <summary>
        /// 保存数据库端口
        /// </summary>
        private void SaveConfig()
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            try
            {
                string value = "Host=" + this.ServerIp + ";UserName=" + this.DbUserName + ";Password=" + this.DbPassword + ";Database=" + this.DbName + ";Port=" + this.Port + ";CharSet=utf8;Allow Zero Datetime=true;sslmode=none;Old Guids=true;";
                //ConfigurationManager.AppSettings.Set("MYSQLCONSTR", value);
                cfa.AppSettings.Settings["MYSQLCONSTR"].Value = value;
                cfa.Save();
                MessageBox.Show("保存成功");
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}
