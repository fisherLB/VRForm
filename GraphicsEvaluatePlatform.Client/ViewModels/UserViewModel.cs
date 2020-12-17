/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Client.Models;
using GraphicsEvaluatePlatform.Client.Repository;
using GraphicsEvaluatePlatform.Client.Services;
using GraphicsEvaluatePlatform.Client.Views.UserWin;
using GraphicsEvaluatePlatform.Infrastructure;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Client.ViewModels
 * 项目描述: 
 * 类 名 称: UserViewModel
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Client.ViewModels
 * 文件名称: UserViewModel
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/19 9:38:32
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Client.ViewModels
{


    public class UserViewModel : NotificationObject
    {
        /// <summary>
        /// 查询命令
        /// </summary>
        public DelegateCommand searchUserCommand { get; set; }
        /// 保存命令
        /// </summary>
        public DelegateCommand saveUserCommand { get; set; }
        /// <summary>
        /// 打开新增弹窗命令
        /// </summary>
        public DelegateCommand OpenAddWincCommand { get; set; }
        /// <summary>
        /// 打开编辑
        /// </summary>
        public DelegateCommand OpenEditWinCommand { get; set; }

        /// <summary>
        /// 修改命令
        /// </summary>
        public DelegateCommand updateUserOnCommand { get; set; }
        
        public DelegateCommand NextPageCommand { get; set; }

        public DelegateCommand PreviousPageCommand { get; set; }

        public DelegateCommand HomePageCommand { get; set; }

        public DelegateCommand TailPageCommand { get; set; }

        public DelegateCommand GoComman { get; set; }

       

        /// <summary>
        /// 删除命令
        /// </summary>
        public DelegateCommand delUserCommand { get; set; }
        /// <summary>
        /// 选中命令
        /// </summary>
        public DelegateCommand SelectMenuItemCommand { get; set; }
        public DelegateCommand PlaceOrderCommand { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                this.RaisePropertyChanged("IsSelected");
            }
        }

        private int count;

        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                this.RaisePropertyChanged("Count");
            }
        }

        private IUserService UserService;
        private Page window { get; set; }

        private UserModel _usermodel;

        public UserModel UserModel
        {
            get { return _usermodel; }
            set
            {
                _usermodel = value;
                this.RaisePropertyChanged("UserModel");
            }
        }

        private string _searchkey;
        public string SearchKey
        {
            get { return _searchkey; }
            set
            {
                _searchkey = value;
                this.RaisePropertyChanged("SearchKey");
            }
        }

        private List<UserModel> _userList;
        public List<UserModel> UserList
        {
            get { return _userList; }
            set
            {
                _userList = value;
                this.RaisePropertyChanged("UserList");
            }
        }
        private int CurrentIndex = 1;
        private int _pageindex = 1;
        public int PageIndex
        {
            get { return _pageindex; }
            set
            {
                _pageindex = value;
                if (value != CurrentIndex)
                {
                    WhenMyValueChange();
                }
                
            }
        }

        /// <summary>
        /// 分页
        /// </summary>
        private PagerViewModel pagerViewModel { get; set; }

        /// <summary>
        ///构造函数
        /// </summary>
        public UserViewModel(object window, PagerViewModel pager)
        {

            OnMyValueChanged += new MyValueChanged(afterMyValueChanged);

            UserService = new UserService();

            pagerViewModel = pager;
            pagerViewModel.PagingHandler += new EventPagingHandler(e => PageIndex = e.PageIndex);
            this.LoadUserList();
            this.searchUserCommand = new DelegateCommand(new Action(this.LoadUserList));//查询
            //this.PlaceOrderCommand = new DelegateCommand(new Action(this.PlaceOrderCommandExecute));
            this.PlaceOrderCommand = new DelegateCommand(new Action(this.PlaceOrderCommandExecute));
            //this.SelectMenuItemCommand = new DelegateCommand(new Action(this.SelectMenuItemExecute));
            this.OpenAddWincCommand = new DelegateCommand(new Action(this.OpenAddWin)); //打开新建弹窗

            this.OpenEditWinCommand = new DelegateCommand(new Action(this.OpenEditWin));//打开编辑弹窗
            this.window = (Page)window;

        }

        //public AddUserViewModel()
        //{
        //    UserService = new UserService();
        //    this.saveUserCommand = new DelegateCommand(new Action(this.AddUser));//新增
        //}
        private List<UserItemViewModel> _userMenuList;

        public List<UserItemViewModel> UserMenuList
        {
            get { return _userMenuList; }
            set { _userMenuList = value; }
        }

        private void PlaceOrderCommandExecute()
        {
          
           
            MessageBox.Show("");
        }
        private void SelectMenuItemExecute()
        {
            this.Count = this.UserMenuList.Count(i => i.IsSelected == true);

        }

        //定义的委托
        public delegate void MyValueChanged(object sender, EventArgs e);
        //与委托相关联的事件
        public event MyValueChanged OnMyValueChanged;



        //事件处理函数，在这里添加变量改变之后的操作
        private void afterMyValueChanged(object sender, EventArgs e)
        {
            //do something
            this.LoadUserList();
        }

        //事件触发函数
        private void WhenMyValueChange()
        {
            if (OnMyValueChanged != null)
            {
                OnMyValueChanged(this, null);
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        private void LoadUserList()
        {

            int recordCount = 0;
            int pageIndex = 1;
            if (pagerViewModel.PageIndex != 0 && pagerViewModel.PageIndex != 1)
            {
                pageIndex = pagerViewModel.PageIndex;
            }

            CurrentIndex = pagerViewModel.PageIndex;
            var ret = UserService.GetList(3, pageIndex, null);
            var pageData = (PageData)ret.AppendData;
            this.UserMenuList = UserService.GetAllUserList(pageData.rows.Tables[0]); //tsService.GetEmailAccounts(PageIndex, 20, out recordCount);
            

            //pagerViewModel.PageIndex = pageIndex;
            pagerViewModel.PageSize = 1;
            pagerViewModel.RecordCount = pageData.total;
            pagerViewModel.PageCount = pagerViewModel.RecordCount % 1 > 0 ? pagerViewModel.RecordCount / 1 + 1 : pagerViewModel.RecordCount / 1;

            List<int> indexList = new List<int>();
            for (int i = 0; i < pagerViewModel.PageCount; i++)
            {
                indexList.Add(i + 1);
            }
            pagerViewModel.IndexList = indexList;
        }
        /// <summary>
        /// 打开新增弹窗
        /// </summary>
        public void OpenAddWin()
        {
            addUserWin win = new addUserWin();
            win.Show();
        }
        /// <summary>
        /// 打开编辑弹窗
        /// </summary>
        public void OpenEditWin()
        {
            this.Count = this.UserMenuList.Count(i => i.IsSelected == true);
            if (this.Count == 0)
            {
                MessageBox.Show("请选择一条记录！");
                return;
            }
            if (this.Count > 1)
            {
                MessageBox.Show("只能选择一条记录！");
                return;
            }
            var selectedDishes = this.UserMenuList.Where(i => i.IsSelected == true).ToList().FirstOrDefault();
            updataUserWin win = new updataUserWin(selectedDishes);
            win.Show();
        }
    }
}
