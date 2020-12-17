using GraphicsEvaluatePlatform.ImageValidSystem.Lib;
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
    public partial class FrmProject : Form
    {
        public FrmProject()
        {
            InitializeComponent();
            pageControl1.OnPageChanged += new EventHandler(PageControl1_OnPageChanged);
        }
        private void FrmProject_Load(object sender, EventArgs e)
        {
            BindData();
            BindDataDetail();
        }
        private void BindData()
        {
            dataGridView2.EnableHeadersVisualStyles = false;//这一句很重要，否则下面的列头设置不起作用
            dataGridView2.Columns[2].HeaderCell.Style.ForeColor = System.Drawing.Color.Red;
            dataGridView2.Columns[3].HeaderCell.Style.ForeColor = System.Drawing.Color.Green;
            dataGridView2.DefaultCellStyle.BackColor = Color.Yellow;
            Hashtable htParams = new Hashtable();
            DataTable dt = new DataTable();
            string unitId = "";
            var ret = BllProject.GetList(unitId);
            dt = ((DataSet)ret.AppendData).Tables[0];
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["p_Id"].Visible = false;
            dataGridView1.Columns["p_UnitId"].Visible = false;
             pageControl1.DrawControl(dt.Rows.Count, 43);

        }
        private void BindDataDetail()
        {
            Hashtable htParams = new Hashtable();
            DataTable dt = new DataTable();
            string unitId = "";
            string projectId = "";
            var ret = BllPicture.GetList(unitId,projectId, pageControl1.PageSize, pageControl1.PageIndex);
            dt = ((DataSet)ret.AppendData).Tables[0];
           dataGridView2.DataSource = dt;
            //pageControl1.DrawControl(dt.Rows.Count, dt.Rows.Count/10);
        }
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridView1.SelectedRows.Count > 0)
            //{
            // int index = dataGridView1.SelectedRows[0].Index;
            //var id = dataGridView1.Rows[index].Cells["p_Id"];
            //txtShorter.Text = dataGridView1.GetRowCellValue(dataGridView1.GetSelectedRows()[0], "shorter").ToString();
            //txtName.Text = dataGridView1.GetRowCellValue(dataGridView1.GetSelectedRows()[0], "name").ToString();
            //nudIndexs.Value = Convert.ToDecimal(dataGridView1.GetRowCellValue(gridView1.GetSelectedRows()[0], "indexs"));
            //if (gridView1.GetRowCellValue(dataGridView1.GetSelectedRows()[0], "worktype") != null)
            //    cbbWorkType.SelectedValue = dataGridView1.GetRowCellValue(dataGridView1.GetSelectedRows()[0], "worktype");
            //else
            //    cbbWorkType.SelectedIndex = 0;
            //txtRemarks.Text = (dataGridView1.GetRowCellValue(dataGridView1.GetSelectedRows()[0], "remarks") != null ? dataGridView1.GetRowCellValue(dataGridView1.GetSelectedRows()[0], "remarks").ToString() : "");
            //}
            int index = e.RowIndex;

            // var id = dataGridView1.Rows[index].Cells["p_Id"].EditedFormattedValue.ToString().Trim();
            //MessageBox.Show("项目ID："+id);
            Hashtable htParams = new Hashtable();
            DataTable dt = new DataTable();
            string unitId = "";
            var ret = BllProject.GetList(unitId);
            dt = ((DataSet)ret.AppendData).Tables[0];
            //dataGridView2.DataSource = dt;
        }
        private void PageControl1_OnPageChanged(object sender, EventArgs e)
        {
            BindData();
        }


    }
}
