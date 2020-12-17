namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    partial class frmBatchImage
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
            this.btn_start = new System.Windows.Forms.Button();
            this.lbl_sumfile = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_select = new System.Windows.Forms.Button();
            this.txt_filetxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(184, 288);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(100, 33);
            this.btn_start.TabIndex = 21;
            this.btn_start.Text = "开始检测";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // lbl_sumfile
            // 
            this.lbl_sumfile.AutoSize = true;
            this.lbl_sumfile.Location = new System.Drawing.Point(184, 187);
            this.lbl_sumfile.Name = "lbl_sumfile";
            this.lbl_sumfile.Size = new System.Drawing.Size(11, 12);
            this.lbl_sumfile.TabIndex = 20;
            this.lbl_sumfile.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(99, 186);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "当前文件：";
            // 
            // btn_select
            // 
            this.btn_select.Location = new System.Drawing.Point(492, 130);
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(100, 33);
            this.btn_select.TabIndex = 18;
            this.btn_select.Text = "选择";
            this.btn_select.UseVisualStyleBackColor = true;
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // txt_filetxt
            // 
            this.txt_filetxt.Location = new System.Drawing.Point(184, 132);
            this.txt_filetxt.Name = "txt_filetxt";
            this.txt_filetxt.Size = new System.Drawing.Size(293, 21);
            this.txt_filetxt.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(99, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "选择文件：";
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(610, 131);
            this.btn_close.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(100, 33);
            this.btn_close.TabIndex = 15;
            this.btn_close.Text = "关闭";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(91, 134);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 14;
            // 
            // frmBatchImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.lbl_sumfile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_select);
            this.Controls.Add(this.txt_filetxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.label1);
            this.Name = "frmBatchImage";
            this.Text = "frmBatchImage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Label lbl_sumfile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_select;
        private System.Windows.Forms.TextBox txt_filetxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label label1;
    }
}