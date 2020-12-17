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
    public partial class frmDatapackage : Form
    {
        public frmDatapackage()
        {
            InitializeComponent();
        }

        private void btn_testcofing_Click(object sender, EventArgs e)
        {
            if (txt_filetxt.Text == "")
            {
                MessageBox.Show("请选择生成数据包存放路径！");
            }
        }
        //关闭
        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //选择
        private void btn_select_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            this.txt_filetxt.Text = path.SelectedPath;
        }
    }
}
