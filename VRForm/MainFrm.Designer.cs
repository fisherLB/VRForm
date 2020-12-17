namespace VRForm
{
    partial class MainFrm
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
            this.label1 = new System.Windows.Forms.Label();
            this.labelaccount = new System.Windows.Forms.Label();
            this.startvr = new System.Windows.Forms.Button();
            this.stopvr = new System.Windows.Forms.Button();
            this.existvr = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.showtime = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(41, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前登录账号：";
            // 
            // labelaccount
            // 
            this.labelaccount.AutoSize = true;
            this.labelaccount.ForeColor = System.Drawing.Color.Blue;
            this.labelaccount.Location = new System.Drawing.Point(159, 29);
            this.labelaccount.Name = "labelaccount";
            this.labelaccount.Size = new System.Drawing.Size(0, 15);
            this.labelaccount.TabIndex = 1;
            // 
            // startvr
            // 
            this.startvr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.startvr.ForeColor = System.Drawing.Color.White;
            this.startvr.Location = new System.Drawing.Point(44, 112);
            this.startvr.Name = "startvr";
            this.startvr.Size = new System.Drawing.Size(282, 60);
            this.startvr.TabIndex = 2;
            this.startvr.Text = "开始使用VR设备";
            this.startvr.UseVisualStyleBackColor = false;
            this.startvr.Click += new System.EventHandler(this.btn_StartVR);
            // 
            // stopvr
            // 
            this.stopvr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.stopvr.ForeColor = System.Drawing.Color.White;
            this.stopvr.Location = new System.Drawing.Point(44, 273);
            this.stopvr.Name = "stopvr";
            this.stopvr.Size = new System.Drawing.Size(282, 60);
            this.stopvr.TabIndex = 3;
            this.stopvr.Text = "停止VR设备";
            this.stopvr.UseVisualStyleBackColor = false;
            this.stopvr.Click += new System.EventHandler(this.btn_StopVR);
            // 
            // existvr
            // 
            this.existvr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.existvr.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.existvr.ForeColor = System.Drawing.Color.OrangeRed;
            this.existvr.Location = new System.Drawing.Point(578, 29);
            this.existvr.Name = "existvr";
            this.existvr.Size = new System.Drawing.Size(187, 57);
            this.existvr.TabIndex = 4;
            this.existvr.Text = "退出登录";
            this.existvr.UseVisualStyleBackColor = false;
            this.existvr.Click += new System.EventHandler(this.btn_Logout);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(468, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(187, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "剩余使用时间：";
            // 
            // showtime
            // 
            this.showtime.AutoSize = true;
            this.showtime.Location = new System.Drawing.Point(497, 201);
            this.showtime.Name = "showtime";
            this.showtime.Size = new System.Drawing.Size(0, 15);
            this.showtime.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(473, 273);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(250, 60);
            this.button1.TabIndex = 7;
            this.button1.Text = "续费";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btn_Recharge);
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.ForeColor = System.Drawing.Color.Blue;
            this.UsernameLabel.Location = new System.Drawing.Point(145, 29);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(112, 15);
            this.UsernameLabel.TabIndex = 8;
            this.UsernameLabel.Text = "当前登录账号：";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.showtime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.existvr);
            this.Controls.Add(this.stopvr);
            this.Controls.Add(this.startvr);
            this.Controls.Add(this.labelaccount);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Red;
            this.Name = "MainFrm";
            this.Text = "VR登录系统";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelaccount;
        private System.Windows.Forms.Button startvr;
        private System.Windows.Forms.Button stopvr;
        private System.Windows.Forms.Button existvr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label showtime;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label UsernameLabel;
    }
}