using GraphicsEvaluatePlatform.ImageValidSystem.Lib;
using GraphicsEvaluatePlatform.ImageValidSystem.Model;
using GraphicsEvaluatePlatform.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    public partial class FrmProcessList : Form
    {
        public FrmProcessList()
        {
            InitializeComponent();
        }
        private void FrmProcessList_Load(object sender, EventArgs e)
        {
            pageControl1.OnPageChanged += new EventHandler(PageControl1_OnPageChanged);
            //tsbtnImortPic.Tag = null;
            BindData();
            BindDataDetail();
        }
        private void BindData()
        {
            dataGridView2.EnableHeadersVisualStyles = false;//这一句很重要，否则下面的列头设置不起作用
            //dataGridView2.Columns[2].HeaderCell.Style.ForeColor = System.Drawing.Color.Red;
            //dataGridView2.Columns[3].HeaderCell.Style.ForeColor = System.Drawing.Color.Green;
            //dataGridView2.DefaultCellStyle.BackColor = Color.Yellow;
            Hashtable htParams = new Hashtable();
            DataTable dt = new DataTable();
            string unitId = "";
            var ret = BllProject.GetList(unitId);

            if (ret.ResultType == OperationResultType.Success)
            {
                dt = ((DataSet)ret.AppendData).Tables[0];
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["p_Id"].Visible = false;
                dataGridView1.Columns["p_UnitId"].Visible = false;
            }
            else
            {
                MessageBox.Show("取数据出错", "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PageControl1_OnPageChanged(object sender, EventArgs e)
        {
            BindDataDetail();
        }
        private void BindDataDetail()
        {
            Hashtable htParams = new Hashtable();
            DataTable dt = new DataTable();
            string unitId = "";
            string projectId = "";
            var ret = BllPicture.GetList(unitId, projectId, pageControl1.PageSize, pageControl1.PageIndex);
            PageDataModel pmodel = (PageDataModel)ret.AppendData;
            if (pmodel != null)
            {
                dataGridView2.DataSource = pmodel.rows.Tables[0];
                int total = pmodel.total;
                int num = pmodel.current;
                pageControl1.DrawControl(pmodel.total, num);
            }

        }


        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            string projectId = dataGridView1.Rows[index].Cells["P_Id"].Value.ToString();
            if (index >= 0)
            {
                Hashtable htParams = new Hashtable();
                string unitId = Guid.Empty.ToString();
                var ret = BllPicture.GetList(unitId, projectId, pageControl1.PageSize, pageControl1.PageIndex);
                if (ret.ResultType == OperationResultType.Success)
                {
                    var ds = ((PageDataModel)ret.AppendData).rows;
                    dataGridView2.DataSource = ds.Tables[0];
                    var total = ((PageDataModel)ret.AppendData).total;
                    pageControl1.DrawControl(total, 10);
                }

            }
        }

        #region 按钮点击事件


        private void tsbtnSearch_Click(object sender, EventArgs e)
        {

        }

        private void tsbtnAdd_Click(object sender, EventArgs e)
        {
            FrmProjectSave frm = new FrmProjectSave();
            var ret = frm.ShowDialog();
            if (ret == System.Windows.Forms.DialogResult.Cancel)
            {
                BindData();
            }
        }

        private void tsbtnEdit_Click(object sender, EventArgs e)
        {
            string projectId = dataGridView1.CurrentRow.Cells["P_Id"].Value.ToString();
            if (!string.IsNullOrEmpty(projectId))
            {

                FrmProjectSave frm = new FrmProjectSave();
                frm.Id = projectId;
                var ret = frm.ShowDialog();
                if (ret == System.Windows.Forms.DialogResult.OK)
                {
                    BindData();
                }
            }
        }

        private void tsbtnbrowse_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void tsbtnDetection_Click(object sender, EventArgs e)
        {
            string id = dataGridView2.CurrentRow.Cells["id"].Value.ToString();
            string name = dataGridView2.CurrentRow.Cells["name"].Value.ToString();
            string path = dataGridView2.CurrentRow.Cells["path"].Value.ToString();
            string type = dataGridView2.CurrentRow.Cells["type"].Value.ToString().TrimStart('.');
            FrmDetectionSingle frm = new FrmDetectionSingle();
            frm.PicId = id;
            frm.PicPath = path;
            frm.PicName = name;
            frm.PicType = type;
            this.Dock = DockStyle.Fill;
            var ret = frm.ShowDialog();
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                BindData();
            }
        }
    }
}
