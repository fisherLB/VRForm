using GraphicsEvaluatePlatform.Client.Models;
using GraphicsEvaluatePlatform.Client.Repository;
using GraphicsEvaluatePlatform.Client.Services;
using GraphicsEvaluatePlatform.Infrastructure;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GraphicsEvaluatePlatform.Client.ViewModels
{
    public class AddOrModifyUserViewModel : NotificationObject
    {
        /// <summary>
        /// 新增命令
        /// </summary>
        public DelegateCommand saveUserCommand { get; set; }
        /// <summary>
        /// 修改命令
        /// </summary>
        public DelegateCommand updataUserCommand { get; set; }

        private IUserService UserService;

        private Window window { get; set; }
        private UserItemViewModel userItemModel { get; set; }

        private string _usermodel;

        public string UserModel
        {
            get { return _usermodel; }
            set
            {
                _usermodel = value;
                this.RaisePropertyChanged("UserModel");
            }
        }
        /// <summary>
        /// 用户条目ID
        /// </summary>
        private string _us_id;

        public string Us_id
        {
            get { return _us_id; }
            set { _us_id = value; }
        }

        /// <summary>
        /// 姓名
        /// </summary>
        private string _us_name;

        public string Us_name
        {
            get { return _us_name; }
            set { _us_name = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        private string _us_account;

        public string Us_account
        {
            get { return _us_account; }
            set { _us_account = value; }
        }
        /// <summary>
        /// 用户密码
        /// </summary>
        private string _us_Password;

        public string Us_Password
        {
            get { return _us_Password; }
            set { _us_Password = value; }
        }
        /// <summary>
        /// 用户备注
        /// </summary>
        private string _us_remark;

        public string Us_remark
        {
            get { return _us_remark; }
            set { _us_remark = value; }
        }
        private string _us_status;
        /// <summary>
        /// 账号情况
        /// </summary>
        public string Us_status
        {
            get { return _us_status; }
            set { _us_status = value; }
        }
        private string _us_type;
        /// <summary>
        /// 账号类型
        /// </summary>
        public string Us_type
        {
            get { return _us_type; }
            set { _us_type = value; }
        }
        private string _us_create_time;
        /// <summary>
        /// 创建时间
        /// </summary>
        public string Us_create_time
        {
            get { return _us_create_time; }
            set { _us_create_time = value; }
        }
        private string _us_create_name;
        /// <summary>
        /// 创建人
        /// </summary>
        public string Us_create_name
        {
            get { return _us_create_name; }
            set { _us_create_name = value; }
        }

        private string _ok_us_Password;
        /// <summary>
        /// 确认密码
        /// </summary>
        public string Ok_Us_Password
        {
            get { return _ok_us_Password; }
            set { _ok_us_Password = value; }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="win"></param>
        public AddOrModifyUserViewModel(Window win)
        {
            UserService = new UserService();
            //新增
            this.saveUserCommand = new DelegateCommand(new Action(this.AddUser));
            
            //修改
            this.updataUserCommand = new DelegateCommand(new Action(this.UpdataUser));
            window = win;
        }


        public AddOrModifyUserViewModel(Window win, UserItemViewModel userItem)
        {
            this.Us_account = userItem.userModel.Us_account;
            this.Us_name = userItem.userModel.Us_name;
            this.Us_Password = userItem.userModel.Us_Password;
            

            userItemModel = userItem;

        }

        //public AddUserViewModel()
        //{
        //    UserService = new UserService();
        //    this.saveUserCommand = new DelegateCommand(new Action(this.AddUser));
        //}

        /// <summary>
        /// 新增用户
        /// </summary>
        private void AddUser()
        {
            if (this.Us_Password != this._ok_us_Password)
            {
                MessageBoxResult mdr = MessageBox.Show("两次输入的密码不一样,请重新输入！", "提示");
                return;
            }
            else
            {
                var re = UserService.AddUser(new UserModel { Us_account = this.Us_account, Us_name = this.Us_name, Us_Password = this.Us_Password, Us_remark = this.Us_remark });
                if (re.ResultType != OperationResultType.Success)
                {
                    MessageBox.Show(re.Message);
                    return;
                }
                else
                {
                    MessageBoxResult mdr = MessageBox.Show("新增成功！", "提示");
                    if (mdr == MessageBoxResult.OK)
                    {
                        this.window.Close();
                    }
                }
            }
            
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        private void UpdataUser()
        {
            var re = UserService.EditUser(new UserModel { Us_id = this.Us_id , Us_account = this.Us_account, Us_name = this.Us_name, Us_Password = this.Us_Password, Us_remark = this.Us_remark });
            if (re.ResultType != OperationResultType.Success)
            {
                MessageBox.Show(re.Message);
                return;
            }
            else
            {
                MessageBoxResult mdr = MessageBox.Show("修改成功！", "提示");
                if (mdr == MessageBoxResult.OK)
                {
                    this.window.Close();
                }
            }
        }
    }
}
