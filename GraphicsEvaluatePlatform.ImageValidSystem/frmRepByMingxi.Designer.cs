namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    partial class frmRepByMingxi
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(800, 450);
            this.dataGridView1.TabIndex = 2;
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
            // frmRepByMingxi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmRepByMingxi";
            this.Text = "已合格图像明细";
            this.Load += new System.EventHandler(this.frmRepByMingxi_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
    }
}