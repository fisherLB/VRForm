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
    public partial class frmDBCofnig : Form
    {
        public frmDBCofnig()
        {
            InitializeComponent();
        }

        private void frmDBCofnig_Load(object sender, EventArgs e)
        {

        }
        private void F_DBCofnig_Load(object sender, EventArgs e)
        {

        }
        //测试连接
        private void btn_testcofing_Click(object sender, EventArgs e)
        {
            MessageBox.Show("数据库连接成功");
        }
        //保存
        private void btn_save_Click(object sender, EventArgs e)
        {
            MessageBox.Show("保存成功");
        }

    }
}
