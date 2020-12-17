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
    public partial class frmBatchImage : Form
    {
        public frmBatchImage()
        {
            InitializeComponent();
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //选择文件
        private void btn_select_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            this.txt_filetxt.Text = path.SelectedPath;

            if (txt_filetxt.Text != "")
            {
                try
                {
                    string str_path = txt_filetxt.Text.Trim();
                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(str_path);
                    int fileNum = dir.GetFiles().Length;
                    lbl_sumfile.Text = fileNum.ToString();
                }
                catch
                {
                    lbl_sumfile.Text = "0";
                }
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
                MessageBox.Show("请选择要检测的图像文件夹！");
                return;
            }
            else
            {
                Form1 form1 = new Form1();
                form1.Show();

            }

        }
    }
}
