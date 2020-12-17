namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    partial class frmDatapackage
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
            this.btn_select = new System.Windows.Forms.Button();
            this.txt_filetxt = new System.Windows.Forms.TextBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_testcofing = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_select
            // 
            this.btn_select.Location = new System.Drawing.Point(488, 175);
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(100, 33);
            this.btn_select.TabIndex = 15;
            this.btn_select.Text = "选择";
            this.btn_select.UseVisualStyleBackColor = true;
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // txt_filetxt
            // 
            this.txt_filetxt.Location = new System.Drawing.Point(180, 177);
            this.txt_filetxt.Name = "txt_filetxt";
            this.txt_filetxt.Size = new System.Drawing.Size(293, 21);
            this.txt_filetxt.TabIndex = 14;
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(606, 175);
            this.btn_close.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(100, 33);
            this.btn_close.TabIndex = 13;
            this.btn_close.Text = "关闭";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "生成路径：";
            // 
            // btn_testcofing
            // 
            this.btn_testcofing.Location = new System.Drawing.Point(180, 243);
            this.btn_testcofing.Name = "btn_testcofing";
            this.btn_testcofing.Size = new System.Drawing.Size(100, 33);
            this.btn_testcofing.TabIndex = 11;
            this.btn_testcofing.Text = "生  成";
            this.btn_testcofing.UseVisualStyleBackColor = true;
            this.btn_testcofing.Click += new System.EventHandler(this.btn_testcofing_Click);
            // 
            // frmDatapackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_select);
            this.Controls.Add(this.txt_filetxt);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_testcofing);
            this.Name = "frmDatapackage";
            this.Text = "数据包生成";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_select;
        private System.Windows.Forms.TextBox txt_filetxt;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_testcofing;
    }
}