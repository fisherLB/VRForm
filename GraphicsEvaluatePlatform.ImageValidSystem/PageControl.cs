using System;
using System.ComponentModel;
using System.Windows.Forms;
namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    public partial class PageControl : UserControl
    {
        public PageControl()
        {
            InitializeComponent();
        }

        #region 分页字段和属性

        private int pageIndex = 1;
        /// <summary>
        /// 当前页面
        /// </summary>
        [DefaultValue(1)]
        public virtual int PageIndex
        {
            get
            {
                if (pageIndex == 0)
                    return 1;
                return pageIndex;
            }
            set { pageIndex = value; }
        }

        /// <summary>
        /// 当前记录数
        /// </summary>
        public virtual int CurrentItem { get; set; }

        private int pageSize = 10;
        /// <summary>
        /// 每页记录数
        /// </summary>
        [DefaultValue(50)]
        public virtual int PageSize
        {
            get
            {
                if (pageSize == 0)
                    return 10;
                return pageSize;
            }
            set { pageSize = value; }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public virtual int RecordCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize != 0)
                    return GetPageCount();
                else
                    return 0;
            }
        }

        /// <summary>
        /// 计算总页数
        /// </summary>
        /// <returns></returns>
        private int GetPageCount()
        {
            if (PageSize == 0)
                return 0;
            int pageCount = RecordCount / PageSize;
            if (RecordCount % PageSize == 0)
                pageCount = RecordCount / PageSize;
            else
                pageCount = RecordCount / PageSize + 1;
            return pageCount;
        }
        #endregion

        public event EventHandler OnPageChanged;

        #region 页跳转事件
        /// <summary>
        /// 首页按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkFirst_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PageIndex = 1;
            DrawControl(true);
        }

        /// <summary>
        /// 上一页按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkPrev_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PageIndex = Math.Max(1, PageIndex - 1);
            DrawControl(true);
        }

        /// <summary>
        /// 下一页按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PageIndex = Math.Min(PageCount, PageIndex + 1);
            DrawControl(true);
        }

        /// <summary>
        /// 尾页按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkLast_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PageIndex = PageCount;
            DrawControl(true);
        }
        #endregion

        /// <summary>
        /// 外部调用
        /// </summary>
        /// <param name="count"></param>
        public void DrawControl(int count, int currentItem)
        {
            RecordCount = count;
            CurrentItem = currentItem;
            DrawControl(false);
        }

        #region 事件
        /// <summary>
        /// 页面控件呈现
        /// </summary>
        /// <param name="callEvent"></param>
        private void DrawControl(bool callEvent)
        {
            btnGo.Text = "跳转";
            lblCurrentPage.Text = PageIndex.ToString();
            lblPageCount.Text = PageCount.ToString();
            lblTotalCount.Text = RecordCount.ToString();
            txtPageSize.Text = PageSize.ToString();
            lblCurrentItem.Text = CurrentItem.ToString();

            if (callEvent && OnPageChanged != null)
                OnPageChanged(this, null);  //当前分页数字改变时，触发委托事件

            SetFormCtrEnabled();

            if (PageCount == 1) //有且仅有一页
            {
                lnkFirst.Enabled = false;
                lnkPrev.Enabled = false;
                lnkNext.Enabled = false;
                lnkLast.Enabled = false;
                btnGo.Enabled = false;
            }
            else if (PageIndex == 1)    //第一页
            {
                lnkFirst.Enabled = false;
                lnkPrev.Enabled = false;
            }
            else if (PageIndex == PageCount)    //最后一页
            {
                lnkNext.Enabled = false;
                lnkLast.Enabled = false;
            }
        }

        private void SetFormCtrEnabled()
        {
            lnkFirst.Enabled = true;
            lnkPrev.Enabled = true;
            lnkNext.Enabled = true;
            lnkLast.Enabled = true;
            btnGo.Enabled = true;
        }

        /// <summary>
        /// enter键功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPageNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            btnGo_Click(null, null);
        }

        /// <summary>
        /// 跳转页数限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int num = 0;
            if (int.TryParse(txtPageNum.Text.Trim(), out num) && num > 0)
            {
                if (num > PageCount)
                    txtPageNum.Text = PageCount.ToString();
            }
            else
                txtPageNum.Text = "";
        }

        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            int num = 0;
            if (int.TryParse(txtPageNum.Text.Trim(), out num) && num > 0)
            {
                PageIndex = num;
                DrawControl(true);
            }
        }
        #endregion

        bool isTextChanged = false;

        /// <summary>
        /// 分页属性改变了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPageSize_TextChanged(object sender, EventArgs e)
        {
            int num = 0;
            if (!int.TryParse(txtPageSize.Text.Trim(), out num) || num <= 0)
            {
                num = 50;
                txtPageSize.Text = "50";
            }
            else
            {
                isTextChanged = true;
            }
            PageSize = num;
        }

        /// <summary>
        /// 光标离开分页属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPageSize_Leave(object sender, EventArgs e)
        {
            if (isTextChanged)
            {
                isTextChanged = false;
                lnkFirst_LinkClicked(null, null);
            }
        }

        /// <summary>
        /// 分页属性按下enter建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPageSize_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                isTextChanged = false;
                lnkFirst_LinkClicked(null, null);
            }
        }

    }
}
