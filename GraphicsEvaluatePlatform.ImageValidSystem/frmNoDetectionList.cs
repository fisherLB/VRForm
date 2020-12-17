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
    public partial class frmNoDetectionList : Form
    {
        public frmNoDetectionList()
        {
            InitializeComponent();
        }


        class Item
        {
            private string _t0;
            public string Text0
            {
                get { return _t0; }
            }
            private string _t1;
            public string Text1
            {
                get { return _t1; }
            }
            private string _t2;
            public string Text2
            {
                get { return _t2; }
            }
            private string _t3;
            public string Text3
            {
                get { return _t3; }
            }
            private string _t4;
            public string Text4
            {
                get { return _t4; }
            }
            private string _t5;
            public string Text5
            {
                get { return _t5; }
            }
            private string _t6;
            public string Text6
            {
                get { return _t6; }
            }
            private string _t7;
            public string Text7
            {
                get { return _t7; }
            }
            public Item(string t0, string t1, string t2, string t3, string t4, string t5, string t6, string t7)
            {
                this._t0 = t0;
                this._t1 = t1;
                this._t2 = t2;
                this._t3 = t3;
                this._t4 = t4;
                this._t5 = t5;
                this._t6 = t6;
                this._t7 = t7;
            }
        }

        private void F_NoDetectionList_Load(object sender, EventArgs e)
        {
            Item[] items = new Item[] {
                new Item("1", "泰坦软件", "人才机构", "不通过", "4501-12-02-001.jpg", "不通过", "通过", "2018-03-12 10:10:11"),
                new Item("2", "泰坦软件", "人才机构", "不通过", "4501-12-02-002.jpg", "不通过", "通过", "2018-03-12 10:11:12"),
                new Item("3", "泰坦软件", "人才机构", "不通过", "4501-12-02-003.jpg", "不通过", "通过", "2018-03-12 10:12:11"),
                new Item("4", "泰坦软件", "人才机构", "不通过", "4501-12-02-004.jpg", "不通过", "通过", "2018-03-12 10:12:11"),
                new Item("5", "泰坦软件", "人才机构", "不通过", "4501-12-02-005.jpg", "不通过", "通过", "2018-03-12 10:12:11"),
                new Item("6", "泰坦软件", "人才机构", "不通过", "4501-12-02-006.jpg", "通过", "不通过", "2018-03-12 10:12:11"),
                new Item("7", "泰坦软件", "人才机构", "不通过", "4501-12-02-007.jpg", "不通过", "通过", "2018-03-12 10:12:11"),
                new Item("8", "泰坦软件", "人才机构", "不通过", "4501-12-02-008.jpg", "不通过", "通过", "2018-03-12 10:12:11"),
                new Item("9", "泰坦软件", "人才机构", "不通过", "4501-12-02-009.jpg", "通过", "不通过", "2018-03-12 10:12:11"),
                new Item("10", "泰坦软件", "人才机构", "不通过", "4501-12-02-010.jpg", "不通过", "通过", "2018-03-12 10:12:11"),
                new Item("11", "泰坦软件", "人才机构", "不通过", "4501-12-02-011.jpg", "不通过", "通过", "2018-03-12 10:12:11"),
                new Item("12", "泰坦软件", "人才机构", "不通过", "4501-12-02-012.jpg", "通过", "不通过", "2018-03-12 10:12:11"),
                new Item("13", "泰坦软件", "人才机构", "不通过", "4501-12-02-013.jpg", "不通过", "通过", "2018-03-12 10:12:11") };

            this.dataGridView1.DataSource = items;
        }
        //关闭
        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //选择处理
        private void btn_select_Click(object sender, EventArgs e)
        {
            frmIamgeProcessing ipf = new frmIamgeProcessing();
            ipf.ShowDialog();
        }
    }
}
