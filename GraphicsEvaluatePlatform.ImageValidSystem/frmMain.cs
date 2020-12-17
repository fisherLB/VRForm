using GraphicsEvaluatePlatform.ImageValidSystem.Lib;
using GraphicsEvaluatePlatform.ImageValidSystem.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;
namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            //pageControl1.OnPageChanged += new EventHandler(pageControl1_OnPageChanged); 
        }
        const int close_size =15;//tab关闭大小
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            //初始化tab
            this.tabControl_menu.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.tabControl_menu.Padding = new System.Drawing.Point(close_size, 5);
            LoadIndex();//加载主页
        }
        //加载主页
        private TabPage tab_nameindex= null;
        public void LoadIndex()
        {
            if (ErgodicModiForm("frm_Index", tabControl_menu))
            {
                tab_nameindex = new TabPage("主页");
                tab_nameindex.Name = "frm_Index";
                tabControl_menu.Controls.Add(tab_nameindex);
                frm_Index form = new frm_Index();
                form.TopLevel = false;
                //form.BackColor = Color.White;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = System.Windows.Forms.DockStyle.Fill;
                form.Show();
                tab_nameindex.Controls.Add(form);
            }
            tabControl_menu.SelectedTab = tab_nameindex;
        }
        //数据包生成--自定义生成
        private void tsmiDatapackage_Click(object sender, EventArgs e)
        {
            frmDatapackage form = new frmDatapackage();
            form.ShowDialog();
        }
        //参数配置
        private void tsmiSysCofing_Click(object sender, EventArgs e)
        {
            frmSysCofing sysf = new frmSysCofing();
            sysf.ShowDialog();
        }
        //数据库配置
        private TabPage tab_name = null;
        private void tsmiDBconfig_Click(object sender, EventArgs e)
        {
            if (ErgodicModiForm("tsmiDBconfig", tabControl_menu))
            {
                tab_name = new TabPage("数据库配置");

                tab_name.Name = "tsmiDBconfig";
                tabControl_menu.Controls.Add(tab_name);
                frmDBCofnig form = new frmDBCofnig();
                form.TopLevel = false;
                form.BackColor = Color.White;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = System.Windows.Forms.DockStyle.Fill;
                form.Show();
                tab_name.Controls.Add(form);
            }
            tabControl_menu.SelectedTab = tab_name;
        }
        //项目管理
        private TabPage tab_tsmiProject = null;
        private void tsmiProject_Click(object sender, EventArgs e)
        {
            if (ErgodicModiForm("tsmiProject", tabControl_menu))
            {
                tab_tsmiProject = new TabPage("项目管理");

                tab_tsmiProject.Name = "tsmiProject";
                tabControl_menu.Controls.Add(tab_tsmiProject);
                FrmProjectList form = new FrmProjectList();
                form.TopLevel = false;
                form.BackColor = Color.White;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = System.Windows.Forms.DockStyle.Fill;
                form.Show();
                tab_tsmiProject.Controls.Add(form);
            }
            tabControl_menu.SelectedTab = tab_tsmiProject;
        }
        //项目导入
        private void 项目导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        //同步项目
        private void 同步项目ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //批量检测
        private TabPage tab_tsmiDetectionMulti = null;
        private void tsmiDetectionMulti_Click(object sender, EventArgs e)
        {
            if (ErgodicModiForm("tsmiDetectionMulti", tabControl_menu))
            {
                tab_tsmiDetectionMulti = new TabPage("批量检测");

                tab_tsmiDetectionMulti.Name = "tsmiDetectionMulti";
                tabControl_menu.Controls.Add(tab_tsmiDetectionMulti);
                FrmDetectionList form = new FrmDetectionList();
                form.TopLevel = false;
                form.BackColor = Color.White;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = System.Windows.Forms.DockStyle.Fill;
                form.Show();
                tab_tsmiDetectionMulti.Controls.Add(form);
            }
            tabControl_menu.SelectedTab = tab_tsmiDetectionMulti;
        }
        //单张检测
        private TabPage tab_tsmiDetectionSingle = null;
        private void tsmiDetectionSingle_Click(object sender, EventArgs e)
        {
            if (ErgodicModiForm("tsmiDetectionSingle", tabControl_menu))
            {
                tab_tsmiDetectionSingle = new TabPage("单张检测");
                tab_tsmiDetectionSingle.Name = "tsmiDetectionSingle";
                    tabControl_menu.Controls.Add(tab_tsmiDetectionSingle);
                    frmRepNobyMingxi form = new frmRepNobyMingxi();
                    form.TopLevel = false;
                    form.BackColor = Color.White;
                    form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = System.Windows.Forms.DockStyle.Fill;
                form.Show();
                tab_tsmiDetectionSingle.Controls.Add(form);
                }
                tabControl_menu.SelectedTab = tab_tsmiDetectionSingle;
            }

        //批量图像处理
        private TabPage tab_tsmiProcessList = null;
        private void tsmiProcessMutil_Click(object sender, EventArgs e)
        {
            if (ErgodicModiForm("tsmiProcessList", tabControl_menu))
            {
                tab_tsmiProcessList = new TabPage("合格检测报表");

                tab_tsmiProcessList.Name = "tsmiProcessList";
                tabControl_menu.Controls.Add(tab_tsmiProcessList);
                FrmProcessList form = new FrmProcessList();
                form.TopLevel = false;
                form.BackColor = Color.White;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = System.Windows.Forms.DockStyle.Fill;
                form.Show();
                tab_tsmiProcessList.Controls.Add(form);
            }
            tabControl_menu.SelectedTab = tab_tsmiProcessList;
        }
        //单张图像处理
        private TabPage tab_tsmiProcessSingle = null;
        private void tsmiProcessSingle_Click(object sender, EventArgs e)
        {

            if (ErgodicModiForm("tsmiProcessSingle", tabControl_menu))
            {
                tab_tsmiProcessSingle = new TabPage("合格检测报表");

                tab_tsmiProcessSingle.Name = "tsmiProcessSingle";
                tabControl_menu.Controls.Add(tab_tsmiProcessSingle);
                FrmProcessSingle form = new FrmProcessSingle();
                form.TopLevel = false;
                form.BackColor = Color.White;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = System.Windows.Forms.DockStyle.Fill;
                form.Show();
                tab_tsmiProcessSingle.Controls.Add(form);
            }
            tabControl_menu.SelectedTab = tab_tsmiProcessSingle;
        }
        //随机抽检
        private void 随机抽检ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        //手工抽检
        private void 手工抽检ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        //合格检测报表

        private TabPage tab_tsmiUnqualifiedList = null;
        private void tsmiUnqualifiedList_Click(object sender, EventArgs e)
        {
           
            if (ErgodicModiForm("tsmiUnqualifiedList", tabControl_menu))
            {
                tab_tsmiUnqualifiedList = new TabPage("合格检测报表");

                tab_tsmiUnqualifiedList.Name = "tsmiUnqualifiedList";
                tabControl_menu.Controls.Add(tab_tsmiUnqualifiedList);
                FrmUnqualifiedList form = new FrmUnqualifiedList();
                form.TopLevel = false;
                form.BackColor = Color.White;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = System.Windows.Forms.DockStyle.Fill;
                form.Show();
                tab_tsmiUnqualifiedList.Controls.Add(form);
            }
            tabControl_menu.SelectedTab = tab_tsmiUnqualifiedList;

        }
        //不合格检测报表
        private TabPage tab_tsmiQualifiedList= null;
        private void tsmiQualifiedList_Click(object sender, EventArgs e)
        {
           
            if (ErgodicModiForm("tsmiQualifiedList", tabControl_menu))
            {
                tab_tsmiQualifiedList = new TabPage("不合格检测报表");

                tab_tsmiQualifiedList.Name = "tsmiQualifiedList";
                tabControl_menu.Controls.Add(tab_tsmiQualifiedList);
                FrmQualifiedList form = new FrmQualifiedList();
                form.TopLevel = false;
                form.BackColor = Color.White;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = System.Windows.Forms.DockStyle.Fill;
                form.Show();
                tab_tsmiQualifiedList.Controls.Add(form);
            }
            tabControl_menu.SelectedTab = tab_tsmiQualifiedList;
        }
        //当前时间
        private void timer_datetimes_Tick(object sender, EventArgs e)
        {
            toolbottom_showtime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        //遍历选项卡判断是否存在该子窗体
        private Boolean ErgodicModiForm(string MainTabControlKey, System.Windows.Forms.TabControl objTabControl)
        {
            //遍历选项卡判断是否存在该子窗体  
            foreach (System.Windows.Forms.Control con in objTabControl.Controls)
            {
                TabPage tab = (TabPage)con;
                if (tab.Name == MainTabControlKey)
                {
                    return false;//存在  
                }
            }
            return true;//不存在  
        }
        //重新绘画
        private void tabControl_menu_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                Rectangle my_TabRect = this.tabControl_menu.GetTabRect(e.Index);
                e.Graphics.DrawString(this.tabControl_menu.TabPages[e.Index].Text
                , this.Font, SystemBrushes.ControlText, my_TabRect.X + 2, my_TabRect.Y + 2);
                //矩形框  
                using (Pen p = new Pen(Color.White))
                {
                    my_TabRect.Offset(my_TabRect.Width - (close_size + 3), 2);
                    my_TabRect.Width = close_size;
                    my_TabRect.Height = close_size;
                    e.Graphics.DrawRectangle(p, my_TabRect);

                }
                //填充矩形框  
                Color recColor = e.State == DrawItemState.Selected ? Color.YellowGreen: Color.White;
                using (Brush b = new SolidBrush(recColor))
                {
                    e.Graphics.FillRectangle(b, my_TabRect);
                }
                //画关闭符号X
                using (Pen objpen = new Pen(Color.Black))
                {
                    Point p1 = new Point(my_TabRect.X + 3, my_TabRect.Y + 3);
                    Point p2 = new Point(my_TabRect.X + my_TabRect.Width - 3, my_TabRect.Y + my_TabRect.Height - 3);
                    e.Graphics.DrawLine(objpen, p1, p2);
                    Point p3 = new Point(my_TabRect.X + 3, my_TabRect.Y + my_TabRect.Height - 3);
                    Point p4 = new Point(my_TabRect.X + my_TabRect.Width - 3, my_TabRect.Y + 3);
                    e.Graphics.DrawLine(objpen, p3, p4);
                }

                e.Graphics.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
   //按下X关闭
        private void tabControl_menu_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = e.X, y = e.Y;

                //关闭区域     
                Rectangle my_TabRect = this.tabControl_menu.GetTabRect(this.tabControl_menu.SelectedIndex);
                my_TabRect.Offset(my_TabRect.Width - (close_size + 3), 2);
                my_TabRect.Width = close_size;
                my_TabRect.Height = close_size;
                //关闭选项卡     
                bool isClose = x > my_TabRect.X && x < my_TabRect.Right
                 && y > my_TabRect.Y && y < my_TabRect.Bottom;

                if (isClose == true)
                {
                    this.tabControl_menu.TabPages.Remove(this.tabControl_menu.SelectedTab);
                }
            }
        }


    }
}
