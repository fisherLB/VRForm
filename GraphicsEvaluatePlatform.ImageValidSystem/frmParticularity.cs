using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    public partial class frmParticularity : Form
    {
        public frmParticularity()
        {
            InitializeComponent();
        }

        private void F_Particularity_Load(object sender, EventArgs e)
        {

        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "图像文件(*.jpg;*.gif;*.bmp)|*.jpg;*.gif;*.bmp";
            file.ShowDialog();
            this.txt_filetxt.Text = file.SafeFileName;
            if (txt_filetxt.Text != "")
            {
                lbl_sumfile.Text = "1";
            }
            else
            {
                lbl_sumfile.Text = "0";
            }
        }
        //开始检测
        private void btn_start_Click(object sender, EventArgs e)
        {
            if (txt_filetxt.Text == "")
            {
                MessageBox.Show("请选择要检测的图像文件！");
                return;
            }
        }
        //通过检测
        private void lbl_yesby_Click(object sender, EventArgs e)
        {

        }
        //未通过检测
        private void lbl_noby_Click(object sender, EventArgs e)
        {
            frmNoDetectionList form = new frmNoDetectionList();
            form.Show();
        }
    }
}
