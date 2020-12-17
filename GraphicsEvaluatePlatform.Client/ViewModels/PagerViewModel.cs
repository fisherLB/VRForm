/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Client.ViewModels
 * 项目描述: 
 * 类 名 称: PagerViewModel
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Client.ViewModels
 * 文件名称: PagerViewModel
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/23 15:02:13
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Client.ViewModels
{

    // 申明委托
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate void EventPagingHandler(EventPagingArg e);

    /// <summary>
    /// 自定义事件参数
    /// </summary>
    public class EventPagingArg : EventArgs
    {
        public int PageIndex { get; set; }

        public EventPagingArg(int pageIndex)
        {
            PageIndex = pageIndex;
        }
    }
    public class PagerViewModel : NotificationObject
    {
        #region 构造器

        public PagerViewModel()
        {
            NextPageCommand = new DelegateCommand(new Action(NextPageCommandExecute));
            PreviousPageCommand = new DelegateCommand(new Action(PreviousPageCommandExecute));
            HomePageCommand = new DelegateCommand(new Action(HomePageCommandExecute));
            TailPageCommand = new DelegateCommand(new Action(TailPageCommandExecute));
        }


        #endregion

        #region Property
        private int pageIndex;

        public int PageIndex
        {
            get { return pageIndex; }
            set
            {
                pageIndex = value;
                this.RaisePropertyChanged("PageIndex");
                if (PagingHandler != null)
                    PagingHandler.Invoke(new EventPagingArg(PageIndex));
            }
        }

        private int pageSize;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; this.RaisePropertyChanged("PageSize"); }
        }

        private int pageCount;

        public int PageCount
        {
            get { return pageCount; }
            set { pageCount = value; this.RaisePropertyChanged("PageCount"); }
        }

        private int recordCount;

        public int RecordCount
        {
            get { return recordCount; }
            set { recordCount = value; this.RaisePropertyChanged("RecordCount"); }
        }

        private List<int> indexList;

        public List<int> IndexList
        {
            get { return indexList; }
            set { indexList = value; this.RaisePropertyChanged("IndexList"); }
        }

        private int _pagetogo;

        public int PageToGo
        {
            get { return _pagetogo; }
            set
            {
                _pagetogo = value;
                this.RaisePropertyChanged("PageToGo");
                System.Windows.Forms.MessageBox.Show("Test");
            }
        }


        #endregion

        #region 命令

        public DelegateCommand NextPageCommand { get; set; }

        public DelegateCommand PreviousPageCommand { get; set; }

        public DelegateCommand HomePageCommand { get; set; }

        public DelegateCommand TailPageCommand { get; set; }

        private void NextPageCommandExecute()
        {
            if (PageIndex < PageCount)
                PageIndex = PageIndex + 1;
        }
        private void PreviousPageCommandExecute()
        {
            if (PageIndex > 1)
                PageIndex = PageIndex - 1;
        }
        private void HomePageCommandExecute()
        {
            PageIndex = 1;
        }
        private void TailPageCommandExecute()
        {
            PageIndex = PageCount;
        }

        #endregion

        #region 事件

        public EventPagingHandler PagingHandler { get; set; }

        #endregion
    }
}
