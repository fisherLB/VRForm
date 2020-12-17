using GraphicsEvaluatePlatform.ImageValidSystem.Lib;
using GraphicsEvaluatePlatform.ImageValidSystem.Mdel;
using GraphicsEvaluatePlatform.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    public partial class FrmProjectSave : Form
    {
        public string Id = "";
        private decimal cost = 0;
        public FrmProjectSave()
        {
            InitializeComponent();
        }

        private void FrmProjectSave_Load(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {

                bindData();
            });
            bindDropDown();
            //Task t1 = new Task(bindDropDown);
            //Task t2 = new Task(bindDropDown);
            //t1.Start();
            //t2.Start();

        }
        /// <summary>
        /// 下拉框绑定
        /// </summary>
        private void bindDropDown()
        {
            DataTable dtClass = new DataTable("dtClass");
            dtClass.Columns.Add("name");
            dtClass.Columns.Add("id");
            dtClass.Rows.Add(new object[] { "", "-1" });
            dtClass.Rows.Add(new object[] { "广西区人才市场", "0" });
            dtClass.Rows.Add(new object[] { "南宁市人才市", "1" });
            dtClass.Rows.Add(new object[] { "桂林市人才市", "2" });
            dtClass.Rows.Add(new object[] { "北海市人才市", "3" });
            dtClass.Rows.Add(new object[] { "宾阳县人才市", "4" });
            cmbUnit.DisplayMember = "name";
            cmbUnit.ValueMember = "id";
            cmbUnit.DataSource = dtClass;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void bindData()
        {
            if (Id != "")
            {
                OperationResult ret = BllProject.GetDetail(Id);
                if (ret.ResultType == OperationResultType.Success)
                {
                    DataTable dt = ((DataSet)ret.AppendData).Tables[0];
                    if (dt != null)
                    {
                        if (txtName.InvokeRequired)
                            txtName.Invoke(new MethodInvoker(() =>
                            {
                                txtName.Text= dt.Rows[0]["p_Name"].ToString();
                            }));
                        else
                            txtName.Text = dt.Rows[0]["p_Name"].ToString();


                        if (txtContactor.InvokeRequired)
                            txtContactor.Invoke(new MethodInvoker(() =>
                            {
                                txtContactor.Text = dt.Rows[0]["p_Contactor"].ToString();
                            }));
                        else
                            txtContactor.Text = dt.Rows[0]["p_Contactor"].ToString();


                        if (txtRegion.InvokeRequired)
                            txtRegion.Invoke(new MethodInvoker(() =>
                            {
                                txtRegion.Text = dt.Rows[0]["p_Region"].ToString();
                            }));
                        else
                            txtRegion.Text = dt.Rows[0]["p_Region"].ToString();


                        if (txtRemarks.InvokeRequired)
                            txtRemarks.Invoke(new MethodInvoker(() =>
                            {
                                txtRemarks.Text = dt.Rows[0]["p_Remarks"].ToString();
                            }));
                        else
                            txtRemarks.Text = dt.Rows[0]["p_Remarks"].ToString();
                        //txtRegion.Text = dt.Rows[0]["p_Region"].ToString();
                        //txtRemarks.Text = dt.Rows[0]["p_Remarks"].ToString();
                    }
                }

            }
            else
            {

            }
        }


        /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void tsbtnSave_Click(object sender, EventArgs e)
        {
            string type = "add";
            if (txtName.Text.Trim().Length > 0)
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("项目名称不能为空！");
                    return;
                }
                if (string.IsNullOrEmpty(Id))
                {
                    Id = Guid.NewGuid().ToString();
                }
                else
                {
                    type = "edit";
                }
                Random r = new Random();
                Projects model = new Projects()
                {
                    P_Id = Id.ToString(),
                    P_DataSign = "1",
                    P_DataTime = DateTime.Now.ToString(),
                    P_UnitId = Guid.Empty.ToString(),
                    P_UserId = Guid.Empty.ToString(),
                    P_Name = txtName.Text,
                    P_Region = txtRegion.Text,
                    P_Contactor = txtContactor.Text,
                    P_Remarks = txtRemarks.Text,
                };
                OperationResult result = new OperationResult(OperationResultType.Success);
                if (type=="add")
                {

                 result = BllProject.Save(model, type);
                }
                else
                {
                     result = BllProject.Update(model, type);
                }

                if (result.ResultType == OperationResultType.Success)
                {
                    MessageBox.Show(result.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show(result.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        /// <summary>
        /// 关闭按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtName_Click(object sender, EventArgs e)
        {

        }

        private void cbbType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbbType_SelectedValueChanged(object sender, EventArgs e)
        {
            txtName.Text = "";

        }
    }
}
