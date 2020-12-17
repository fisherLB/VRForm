namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.btnLogin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.sybtn = new System.Windows.Forms.Button();
            this.ckbAccess = new System.Windows.Forms.CheckBox();
            this.ckbPwd = new System.Windows.Forms.CheckBox();
            this.loginnameCmb = new System.Windows.Forms.ComboBox();
            this.lbl_setconfig = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(101, 227);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(100, 35);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "登陆";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(27, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "用户名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(27, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "密  码：";
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(107, 122);
            this.txtPwd.MaxLength = 30;
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(250, 23);
            this.txtPwd.TabIndex = 2;
            this.txtPwd.UseSystemPasswordChar = true;
            this.txtPwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPwd_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(69, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(305, 29);
            this.label4.TabIndex = 6;
            this.label4.Text = "图像质量测评系统V1.0";
            // 
            // sybtn
            // 
            this.sybtn.Location = new System.Drawing.Point(257, 227);
            this.sybtn.Name = "sybtn";
            this.sybtn.Size = new System.Drawing.Size(100, 35);
            this.sybtn.TabIndex = 4;
            this.sybtn.Text = "同步数据";
            this.sybtn.UseVisualStyleBackColor = true;
            this.sybtn.Click += new System.EventHandler(this.sybtn_Click);
            // 
            // ckbAccess
            // 
            this.ckbAccess.AutoSize = true;
            this.ckbAccess.BackColor = System.Drawing.Color.Transparent;
            this.ckbAccess.ForeColor = System.Drawing.Color.White;
            this.ckbAccess.Location = new System.Drawing.Point(106, 174);
            this.ckbAccess.Name = "ckbAccess";
            this.ckbAccess.Size = new System.Drawing.Size(82, 18);
            this.ckbAccess.TabIndex = 7;
            this.ckbAccess.Text = "记住账号";
            this.ckbAccess.UseVisualStyleBackColor = false;
            // 
            // ckbPwd
            // 
            this.ckbPwd.AutoSize = true;
            this.ckbPwd.BackColor = System.Drawing.Color.Transparent;
            this.ckbPwd.ForeColor = System.Drawing.Color.White;
            this.ckbPwd.Location = new System.Drawing.Point(193, 174);
            this.ckbPwd.Name = "ckbPwd";
            this.ckbPwd.Size = new System.Drawing.Size(82, 18);
            this.ckbPwd.TabIndex = 7;
            this.ckbPwd.Text = "记住密码";
            this.ckbPwd.UseVisualStyleBackColor = false;
            // 
            // loginnameCmb
            // 
            this.loginnameCmb.FormattingEnabled = true;
            this.loginnameCmb.Location = new System.Drawing.Point(107, 80);
            this.loginnameCmb.Name = "loginnameCmb";
            this.loginnameCmb.Size = new System.Drawing.Size(250, 22);
            this.loginnameCmb.TabIndex = 8;
            // 
            // lbl_setconfig
            // 
            this.lbl_setconfig.AutoSize = true;
            this.lbl_setconfig.BackColor = System.Drawing.Color.Transparent;
            this.lbl_setconfig.ForeColor = System.Drawing.Color.White;
            this.lbl_setconfig.Location = new System.Drawing.Point(283, 175);
            this.lbl_setconfig.Name = "lbl_setconfig";
            this.lbl_setconfig.Size = new System.Drawing.Size(77, 14);
            this.lbl_setconfig.TabIndex = 9;
            this.lbl_setconfig.Text = "数据库配置";
            this.lbl_setconfig.Click += new System.EventHandler(this.lbl_setconfig_Click);
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(404, 282);
            this.Controls.Add(this.lbl_setconfig);
            this.Controls.Add(this.loginnameCmb);
            this.Controls.Add(this.ckbPwd);
            this.Controls.Add(this.ckbAccess);
            this.Controls.Add(this.sybtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogin);
            this.Font = new System.Drawing.Font("宋体", 10.5F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户登陆";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button sybtn;
        private System.Windows.Forms.CheckBox ckbAccess;
        private System.Windows.Forms.CheckBox ckbPwd;
        private System.Windows.Forms.ComboBox loginnameCmb;
        private System.Windows.Forms.Label lbl_setconfig;
    }
}