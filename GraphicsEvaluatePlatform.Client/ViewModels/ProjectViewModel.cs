using GraphicsEvaluatePlatform.Client.Repository;
using GraphicsEvaluatePlatform.Client.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Windows.Controls;
using GraphicsEvaluatePlatform.Client.Models;

namespace GraphicsEvaluatePlatform.Client.ViewModels
{
    public class ProjectViewModel : NotificationObject
    {
        private Project _model;

        public Project Model
        {
            get { return _model; }
            set
            {
                _model = value;
                this.RaisePropertyChanged("Model");
            }
        }

        private readonly IProjectService ProjectService;
        private Page Page { get; set; }

        /// <summary>
        ///构造函数
        /// </summary>
        public ProjectViewModel(object page)
        {
            ProjectService = new ProjectService();
            this.SearchCommand = new DelegateCommand(new Action(this.List));//查询
            this.SaveCommand = new DelegateCommand(new Action(this.Add));//新增
            this.UpdateCommand = new DelegateCommand(new Action(this.Edit));//修改
            this.DelCommand = new DelegateCommand(new Action(this.Del));//删除
            this.Page = (Page)page;

        }

        /// <summary>
        ///  取数据集合
        /// </summary>
        public DelegateCommand GetListCommand { get; set; }

        /// <summary>
        ///  取数据集合
        /// </summary>
        public DelegateCommand DetailCommand { get; set; }

        /// <summary>
        /// 保存命令
        /// </summary>
        public DelegateCommand SaveCommand { get; set; }

        /// <summary>
        /// 修改命令
        /// </summary>
        public DelegateCommand UpdateCommand { get; set; }

        /// <summary>
        /// 删除命令
        /// </summary>
        public DelegateCommand DelCommand { get; set; }

        /// <summary>
        /// 查询命令
        /// </summary>
        public DelegateCommand SearchCommand { get; set; }

        /// <summary>
        ///  取数据明细
        /// </summary>
        public DelegateCommand GetDetailCommand { get; set; }

        /// <summary>
        /// 集合
        /// </summary>
        private void List()
        {
        }

        /// <summary>
        /// 明细
        /// </summary>
        private void Detail()
        {
            return;
        }

        /// <summary>
        /// 新增
        /// </summary>
        private void Add()
        {
            return;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        private void Edit()
        {
            return;
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void Del()
        {
            return;
        }

    }
}
