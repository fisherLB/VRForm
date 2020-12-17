namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    partial class frmNoDetectionList
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Text0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Text1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Text2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Text3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Text4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Text5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Text6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Text7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_right = new System.Windows.Forms.Panel();
            this.groupBox_right = new System.Windows.Forms.GroupBox();
            this.btn_select = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.panel_left = new System.Windows.Forms.Panel();
            this.groupBox_left = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel_right.SuspendLayout();
            this.groupBox_right.SuspendLayout();
            this.panel_left.SuspendLayout();
            this.groupBox_left.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Text0,
            this.Text1,
            this.Text2,
            this.Text3,
            this.Text4,
            this.Text5,
            this.Text6,
            this.Text7});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(865, 590);
            this.dataGridView1.TabIndex = 0;
            // 
            // Text0
            // 
            this.Text0.DataPropertyName = "Text0";
            this.Text0.HeaderText = "编号";
            this.Text0.Name = "Text0";
            this.Text0.ReadOnly = true;
            // 
            // Text1
            // 
            this.Text1.DataPropertyName = "Text1";
            this.Text1.HeaderText = "机构名称";
            this.Text1.Name = "Text1";
            this.Text1.ReadOnly = true;
            // 
            // Text2
            // 
            this.Text2.DataPropertyName = "Text2";
            this.Text2.HeaderText = "项目名称";
            this.Text2.Name = "Text2";
            this.Text2.ReadOnly = true;
            // 
            // Text3
            // 
            this.Text3.DataPropertyName = "Text3";
            this.Text3.HeaderText = "检测状态";
            this.Text3.Name = "Text3";
            this.Text3.ReadOnly = true;
            // 
            // Text4
            // 
            this.Text4.DataPropertyName = "Text4";
            this.Text4.HeaderText = "文件名";
            this.Text4.Name = "Text4";
            this.Text4.ReadOnly = true;
            // 
            // Text5
            // 
            this.Text5.DataPropertyName = "Text5";
            this.Text5.HeaderText = "对比度";
            this.Text5.Name = "Text5";
            this.Text5.ReadOnly = true;
            // 
            // Text6
            // 
            this.Text6.DataPropertyName = "Text6";
            this.Text6.HeaderText = "图像大小";
            this.Text6.Name = "Text6";
            this.Text6.ReadOnly = true;
            // 
            // Text7
            // 
            this.Text7.DataPropertyName = "Text7";
            this.Text7.HeaderText = "检测时间";
            this.Text7.Name = "Text7";
            this.Text7.ReadOnly = true;
            // 
            // panel_right
            // 
            this.panel_right.Controls.Add(this.groupBox_right);
            this.panel_right.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_right.Location = new System.Drawing.Point(871, 0);
            this.panel_right.Name = "panel_right";
            this.panel_right.Size = new System.Drawing.Size(135, 610);
            this.panel_right.TabIndex = 2;
            // 
            // groupBox_right
            // 
            this.groupBox_right.Controls.Add(this.btn_select);
            this.groupBox_right.Controls.Add(this.btn_close);
            this.groupBox_right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_right.Location = new System.Drawing.Point(0, 0);
            this.groupBox_right.Name = "groupBox_right";
            this.groupBox_right.Size = new System.Drawing.Size(135, 610);
            this.groupBox_right.TabIndex = 0;
            this.groupBox_right.TabStop = false;
            this.groupBox_right.Text = "功能列表";
            // 
            // btn_select
            // 
            this.btn_select.Location = new System.Drawing.Point(22, 68);
            this.btn_select.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(100, 33);
            this.btn_select.TabIndex = 3;
            this.btn_select.Text = "选择处理";
            this.btn_select.UseVisualStyleBackColor = true;
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(22, 135);
            this.btn_close.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(100, 33);
            this.btn_close.TabIndex = 2;
            this.btn_close.Text = "关闭";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // panel_left
            // 
            this.panel_left.Controls.Add(this.groupBox_left);
            this.panel_left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_left.Location = new System.Drawing.Point(0, 0);
            this.panel_left.Name = "panel_left";
            this.panel_left.Size = new System.Drawing.Size(871, 610);
            this.panel_left.TabIndex = 3;
            // 
            // groupBox_left
            // 
            this.groupBox_left.Controls.Add(this.dataGridView1);
            this.groupBox_left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_left.Location = new System.Drawing.Point(0, 0);
            this.groupBox_left.Name = "groupBox_left";
            this.groupBox_left.Size = new System.Drawing.Size(871, 610);
            this.groupBox_left.TabIndex = 0;
            this.groupBox_left.TabStop = false;
            this.groupBox_left.Text = "不通过图像列表";
            // 
            // frmNoDetectionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 610);
            this.Controls.Add(this.panel_left);
            this.Controls.Add(this.panel_right);
            this.Name = "frmNoDetectionList";
            this.Text = "不合格图像列表";
            this.Click += new System.EventHandler(this.F_NoDetectionList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel_right.ResumeLayout(false);
            this.groupBox_right.ResumeLayout(false);
            this.panel_left.ResumeLayout(false);
            this.groupBox_left.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Text0;
        private System.Windows.Forms.DataGridViewTextBoxColumn Text1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Text2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Text3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Text4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Text5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Text6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Text7;
        private System.Windows.Forms.Panel panel_right;
        private System.Windows.Forms.GroupBox groupBox_right;
        private System.Windows.Forms.Button btn_select;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Panel panel_left;
        private System.Windows.Forms.GroupBox groupBox_left;
    }
}