namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.statusStrip_bottom = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolbottom_showtime = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip_top = new System.Windows.Forms.MenuStrip();
            this.tsmiProjects = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiProject = new System.Windows.Forms.ToolStripMenuItem();
            this.项目导入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.同步项目ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图像检测ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDetectionMulti = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDetectionSingle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPictureProc = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiProcessMutil = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiProcessSingle = new System.Windows.Forms.ToolStripMenuItem();
            this.图像管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图像抽检ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.随机抽检ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.手工抽检ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据包生成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDatapackage = new System.Windows.Forms.ToolStripMenuItem();
            this.检测报表生成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUnqualifiedList = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiQualifiedList = new System.Windows.Forms.ToolStripMenuItem();
            this.系统设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSysCofing = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDBconfig = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_datetimes = new System.Windows.Forms.Timer(this.components);
            this.panel_mian = new System.Windows.Forms.Panel();
            this.tabControl_menu = new System.Windows.Forms.TabControl();
            this.statusStrip_bottom.SuspendLayout();
            this.menuStrip_top.SuspendLayout();
            this.panel_mian.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip_bottom
            // 
            this.statusStrip_bottom.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.statusStrip_bottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolbottom_showtime});
            this.statusStrip_bottom.Location = new System.Drawing.Point(0, 610);
            this.statusStrip_bottom.Name = "statusStrip_bottom";
            this.statusStrip_bottom.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip_bottom.Size = new System.Drawing.Size(1284, 22);
            this.statusStrip_bottom.TabIndex = 4;
            this.statusStrip_bottom.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(70, 17);
            this.toolStripStatusLabel1.Text = "当前时间:";
            // 
            // toolbottom_showtime
            // 
            this.toolbottom_showtime.Name = "toolbottom_showtime";
            this.toolbottom_showtime.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip_top
            // 
            this.menuStrip_top.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip_top.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiProjects,
            this.图像检测ToolStripMenuItem,
            this.tsmiPictureProc,
            this.图像管理ToolStripMenuItem,
            this.图像抽检ToolStripMenuItem,
            this.数据包生成ToolStripMenuItem,
            this.检测报表生成ToolStripMenuItem,
            this.系统设置ToolStripMenuItem});
            this.menuStrip_top.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_top.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.menuStrip_top.Name = "menuStrip_top";
            this.menuStrip_top.Padding = new System.Windows.Forms.Padding(2);
            this.menuStrip_top.Size = new System.Drawing.Size(1284, 24);
            this.menuStrip_top.TabIndex = 7;
            this.menuStrip_top.Text = "menuStrip_top";
            // 
            // tsmiProjects
            // 
            this.tsmiProjects.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiProject,
            this.项目导入ToolStripMenuItem,
            this.同步项目ToolStripMenuItem});
            this.tsmiProjects.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiProjects.Image = ((System.Drawing.Image)(resources.GetObject("tsmiProjects.Image")));
            this.tsmiProjects.Name = "tsmiProjects";
            this.tsmiProjects.Size = new System.Drawing.Size(124, 20);
            this.tsmiProjects.Text = "项目管理(&P)";
            // 
            // tsmiProject
            // 
            this.tsmiProject.Name = "tsmiProject";
            this.tsmiProject.Size = new System.Drawing.Size(140, 22);
            this.tsmiProject.Text = "项目管理";
            this.tsmiProject.Click += new System.EventHandler(this.tsmiProject_Click);
            // 
            // 项目导入ToolStripMenuItem
            // 
            this.项目导入ToolStripMenuItem.Name = "项目导入ToolStripMenuItem";
            this.项目导入ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.项目导入ToolStripMenuItem.Text = "导入项目";
            this.项目导入ToolStripMenuItem.Click += new System.EventHandler(this.项目导入ToolStripMenuItem_Click);
            // 
            // 同步项目ToolStripMenuItem
            // 
            this.同步项目ToolStripMenuItem.Name = "同步项目ToolStripMenuItem";
            this.同步项目ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.同步项目ToolStripMenuItem.Text = "同步项目";
            this.同步项目ToolStripMenuItem.Click += new System.EventHandler(this.同步项目ToolStripMenuItem_Click);
            // 
            // 图像检测ToolStripMenuItem
            // 
            this.图像检测ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDetectionMulti,
            this.tsmiDetectionSingle});
            this.图像检测ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("图像检测ToolStripMenuItem.Image")));
            this.图像检测ToolStripMenuItem.Name = "图像检测ToolStripMenuItem";
            this.图像检测ToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            this.图像检测ToolStripMenuItem.Text = "图像检测(&D)";
            // 
            // tsmiDetectionMulti
            // 
            this.tsmiDetectionMulti.Name = "tsmiDetectionMulti";
            this.tsmiDetectionMulti.Size = new System.Drawing.Size(140, 22);
            this.tsmiDetectionMulti.Text = "批量检测";
            this.tsmiDetectionMulti.Click += new System.EventHandler(this.tsmiDetectionMulti_Click);
            // 
            // tsmiDetectionSingle
            // 
            this.tsmiDetectionSingle.Name = "tsmiDetectionSingle";
            this.tsmiDetectionSingle.Size = new System.Drawing.Size(140, 22);
            this.tsmiDetectionSingle.Text = "单张检测";
            this.tsmiDetectionSingle.Click += new System.EventHandler(this.tsmiDetectionSingle_Click);
            // 
            // tsmiPictureProc
            // 
            this.tsmiPictureProc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiProcessMutil,
            this.tsmiProcessSingle});
            this.tsmiPictureProc.Image = ((System.Drawing.Image)(resources.GetObject("tsmiPictureProc.Image")));
            this.tsmiPictureProc.Name = "tsmiPictureProc";
            this.tsmiPictureProc.Size = new System.Drawing.Size(124, 20);
            this.tsmiPictureProc.Text = "图像处理(&T)";
            // 
            // tsmiProcessMutil
            // 
            this.tsmiProcessMutil.Name = "tsmiProcessMutil";
            this.tsmiProcessMutil.Size = new System.Drawing.Size(172, 22);
            this.tsmiProcessMutil.Text = "批量图像处理";
            this.tsmiProcessMutil.Click += new System.EventHandler(this.tsmiProcessMutil_Click);
            // 
            // tsmiProcessSingle
            // 
            this.tsmiProcessSingle.Name = "tsmiProcessSingle";
            this.tsmiProcessSingle.Size = new System.Drawing.Size(172, 22);
            this.tsmiProcessSingle.Text = "单张图像处理";
            this.tsmiProcessSingle.Click += new System.EventHandler(this.tsmiProcessSingle_Click);
            // 
            // 图像管理ToolStripMenuItem
            // 
            this.图像管理ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("图像管理ToolStripMenuItem.Image")));
            this.图像管理ToolStripMenuItem.Name = "图像管理ToolStripMenuItem";
            this.图像管理ToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            this.图像管理ToolStripMenuItem.Text = "图像管理(&M)";
            // 
            // 图像抽检ToolStripMenuItem
            // 
            this.图像抽检ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.随机抽检ToolStripMenuItem,
            this.手工抽检ToolStripMenuItem});
            this.图像抽检ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("图像抽检ToolStripMenuItem.Image")));
            this.图像抽检ToolStripMenuItem.Name = "图像抽检ToolStripMenuItem";
            this.图像抽检ToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            this.图像抽检ToolStripMenuItem.Text = "图像抽检(&C)";
            // 
            // 随机抽检ToolStripMenuItem
            // 
            this.随机抽检ToolStripMenuItem.Name = "随机抽检ToolStripMenuItem";
            this.随机抽检ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.随机抽检ToolStripMenuItem.Text = "随机抽检";
            this.随机抽检ToolStripMenuItem.Click += new System.EventHandler(this.随机抽检ToolStripMenuItem_Click);
            // 
            // 手工抽检ToolStripMenuItem
            // 
            this.手工抽检ToolStripMenuItem.Name = "手工抽检ToolStripMenuItem";
            this.手工抽检ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.手工抽检ToolStripMenuItem.Text = "手工抽检";
            this.手工抽检ToolStripMenuItem.Click += new System.EventHandler(this.手工抽检ToolStripMenuItem_Click);
            // 
            // 数据包生成ToolStripMenuItem
            // 
            this.数据包生成ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDatapackage});
            this.数据包生成ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("数据包生成ToolStripMenuItem.Image")));
            this.数据包生成ToolStripMenuItem.Name = "数据包生成ToolStripMenuItem";
            this.数据包生成ToolStripMenuItem.Size = new System.Drawing.Size(140, 20);
            this.数据包生成ToolStripMenuItem.Text = "数据包生成(&G)";
            // 
            // tsmiDatapackage
            // 
            this.tsmiDatapackage.Name = "tsmiDatapackage";
            this.tsmiDatapackage.Size = new System.Drawing.Size(156, 22);
            this.tsmiDatapackage.Text = "自定义生成";
            this.tsmiDatapackage.Click += new System.EventHandler(this.tsmiDatapackage_Click);
            // 
            // 检测报表生成ToolStripMenuItem
            // 
            this.检测报表生成ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiUnqualifiedList,
            this.tsmiQualifiedList});
            this.检测报表生成ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("检测报表生成ToolStripMenuItem.Image")));
            this.检测报表生成ToolStripMenuItem.Name = "检测报表生成ToolStripMenuItem";
            this.检测报表生成ToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            this.检测报表生成ToolStripMenuItem.Text = "检测报表(&R)";
            // 
            // tsmiUnqualifiedList
            // 
            this.tsmiUnqualifiedList.Name = "tsmiUnqualifiedList";
            this.tsmiUnqualifiedList.Size = new System.Drawing.Size(188, 22);
            this.tsmiUnqualifiedList.Text = "不合格图像报表";
            this.tsmiUnqualifiedList.Click += new System.EventHandler(this.tsmiUnqualifiedList_Click);
            // 
            // tsmiQualifiedList
            // 
            this.tsmiQualifiedList.Name = "tsmiQualifiedList";
            this.tsmiQualifiedList.Size = new System.Drawing.Size(188, 22);
            this.tsmiQualifiedList.Text = "合格图像报表";
            this.tsmiQualifiedList.Click += new System.EventHandler(this.tsmiQualifiedList_Click);
            // 
            // 系统设置ToolStripMenuItem
            // 
            this.系统设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSysCofing,
            this.tsmiDBconfig});
            this.系统设置ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("系统设置ToolStripMenuItem.Image")));
            this.系统设置ToolStripMenuItem.Name = "系统设置ToolStripMenuItem";
            this.系统设置ToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            this.系统设置ToolStripMenuItem.Text = "系统设置(&S)";
            // 
            // tsmiSysCofing
            // 
            this.tsmiSysCofing.Name = "tsmiSysCofing";
            this.tsmiSysCofing.Size = new System.Drawing.Size(156, 22);
            this.tsmiSysCofing.Text = "参数配置";
            this.tsmiSysCofing.Click += new System.EventHandler(this.tsmiSysCofing_Click);
            // 
            // tsmiDBconfig
            // 
            this.tsmiDBconfig.Name = "tsmiDBconfig";
            this.tsmiDBconfig.Size = new System.Drawing.Size(156, 22);
            this.tsmiDBconfig.Text = "数据库配置";
            this.tsmiDBconfig.Click += new System.EventHandler(this.tsmiDBconfig_Click);
            // 
            // timer_datetimes
            // 
            this.timer_datetimes.Enabled = true;
            this.timer_datetimes.Interval = 1000;
            this.timer_datetimes.Tick += new System.EventHandler(this.timer_datetimes_Tick);
            // 
            // panel_mian
            // 
            this.panel_mian.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel_mian.BackgroundImage")));
            this.panel_mian.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel_mian.Controls.Add(this.tabControl_menu);
            this.panel_mian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_mian.Location = new System.Drawing.Point(0, 24);
            this.panel_mian.Name = "panel_mian";
            this.panel_mian.Size = new System.Drawing.Size(1284, 586);
            this.panel_mian.TabIndex = 8;
            // 
            // tabControl_menu
            // 
            this.tabControl_menu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_menu.Location = new System.Drawing.Point(0, 0);
            this.tabControl_menu.Name = "tabControl_menu";
            this.tabControl_menu.SelectedIndex = 0;
            this.tabControl_menu.Size = new System.Drawing.Size(1284, 586);
            this.tabControl_menu.TabIndex = 0;
            this.tabControl_menu.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl_menu_DrawItem);
            this.tabControl_menu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControl_menu_MouseDown);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 632);
            this.Controls.Add(this.panel_mian);
            this.Controls.Add(this.statusStrip_bottom);
            this.Controls.Add(this.menuStrip_top);
            this.Font = new System.Drawing.Font("宋体", 10.5F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "图像质量检测系统";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.statusStrip_bottom.ResumeLayout(false);
            this.statusStrip_bottom.PerformLayout();
            this.menuStrip_top.ResumeLayout(false);
            this.menuStrip_top.PerformLayout();
            this.panel_mian.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripMenuItem 系统设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据包生成ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图像抽检ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图像管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiPictureProc;
        private System.Windows.Forms.ToolStripMenuItem 图像检测ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiProjects;
        private System.Windows.Forms.StatusStrip statusStrip_bottom;
        private System.Windows.Forms.MenuStrip menuStrip_top;
        private System.Windows.Forms.ToolStripMenuItem tsmiSysCofing;
        private System.Windows.Forms.ToolStripMenuItem tsmiDBconfig;
        private System.Windows.Forms.ToolStripMenuItem 随机抽检ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 手工抽检ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiProcessMutil;
        private System.Windows.Forms.ToolStripMenuItem tsmiProcessSingle;
        private System.Windows.Forms.ToolStripMenuItem tsmiDetectionMulti;
        private System.Windows.Forms.ToolStripMenuItem tsmiDetectionSingle;
        private System.Windows.Forms.ToolStripMenuItem 检测报表生成ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiUnqualifiedList;
        private System.Windows.Forms.ToolStripMenuItem tsmiQualifiedList;
        private System.Windows.Forms.ToolStripMenuItem tsmiDatapackage;
        private System.Windows.Forms.Panel panel_mian;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolbottom_showtime;
        private System.Windows.Forms.Timer timer_datetimes;
        private System.Windows.Forms.ToolStripMenuItem tsmiProject;
        private System.Windows.Forms.ToolStripMenuItem 项目导入ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 同步项目ToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl_menu;
    }
}