namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    partial class frmParticularity
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
            this.lbl_noby = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbl_yesby = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_start = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_sumfile = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_select = new System.Windows.Forms.Button();
            this.txt_filetxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_noby
            // 
            this.lbl_noby.AutoSize = true;
            this.lbl_noby.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.lbl_noby.ForeColor = System.Drawing.Color.Red;
            this.lbl_noby.Location = new System.Drawing.Point(445, 244);
            this.lbl_noby.Name = "lbl_noby";
            this.lbl_noby.Size = new System.Drawing.Size(23, 25);
            this.lbl_noby.TabIndex = 30;
            this.lbl_noby.Text = "0";
            this.lbl_noby.Click += new System.EventHandler(this.lbl_noby_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(345, 245);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 29;
            this.label7.Text = "未通过检测:";
            // 
            // lbl_yesby
            // 
            this.lbl_yesby.AutoSize = true;
            this.lbl_yesby.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_yesby.ForeColor = System.Drawing.Color.Green;
            this.lbl_yesby.Location = new System.Drawing.Point(264, 243);
            this.lbl_yesby.Name = "lbl_yesby";
            this.lbl_yesby.Size = new System.Drawing.Size(23, 25);
            this.lbl_yesby.TabIndex = 28;
            this.lbl_yesby.Text = "0";
            this.lbl_yesby.Click += new System.EventHandler(this.lbl_yesby_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(173, 245);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 27;
            this.label3.Text = "已通过检测:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(88, 245);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 26;
            this.label5.Text = "检测结果：";
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(173, 291);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(100, 33);
            this.btn_start.TabIndex = 25;
            this.btn_start.Text = "开始检测";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(407, 268);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 12);
            this.label4.TabIndex = 24;
            // 
            // lbl_sumfile
            // 
            this.lbl_sumfile.AutoSize = true;
            this.lbl_sumfile.Location = new System.Drawing.Point(173, 189);
            this.lbl_sumfile.Name = "lbl_sumfile";
            this.lbl_sumfile.Size = new System.Drawing.Size(11, 12);
            this.lbl_sumfile.TabIndex = 23;
            this.lbl_sumfile.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(88, 189);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 22;
            this.label1.Text = "当前文件：";
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(612, 127);
            this.btn_close.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(100, 33);
            this.btn_close.TabIndex = 21;
            this.btn_close.Text = "关闭";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_select
            // 
            this.btn_select.Location = new System.Drawing.Point(481, 126);
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(100, 33);
            this.btn_select.TabIndex = 20;
            this.btn_select.Text = "选择";
            this.btn_select.UseVisualStyleBackColor = true;
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // txt_filetxt
            // 
            this.txt_filetxt.Location = new System.Drawing.Point(173, 129);
            this.txt_filetxt.Name = "txt_filetxt";
            this.txt_filetxt.Size = new System.Drawing.Size(293, 21);
            this.txt_filetxt.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "选择文件：";
            // 
            // frmParticularity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lbl_noby);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbl_yesby);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbl_sumfile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_select);
            this.Controls.Add(this.txt_filetxt);
            this.Controls.Add(this.label2);
            this.Name = "frmParticularity";
            this.Text = "图像检测";
            this.Click += new System.EventHandler(this.F_Particularity_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_noby;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_yesby;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_sumfile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_select;
        private System.Windows.Forms.TextBox txt_filetxt;
        private System.Windows.Forms.Label label2;
    }
}