using GraphicsEvaluatePlatform.ImageValidSystem.Lib;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Transactions;
using System.Windows.Forms;

namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            //加载登录用户
            LoginBll.LoadLoginUser(this.loginnameCmb);
            //显示匹配信息
            LoginBll.DisplayUserInfo(this.loginnameCmb, this.txtPwd, this.ckbPwd);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (loginnameCmb.Text.Trim() == "")
            {
                MessageBox.Show("请输入账户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                loginnameCmb.Focus();
                return;
            }
            if (txtPwd.Text.Trim() == "")
            {
                MessageBox.Show("请输入密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPwd.Focus();
                return;
            }

            OperationResult re = LoginBll.LoginValidate(loginnameCmb.Text.Trim(), txtPwd.Text.Trim());
            //登录成功
            if (re.ResultType == OperationResultType.Success)
            {
                LoginBll.RememberLgoinUser(loginnameCmb.Text.Trim(), txtPwd.Text.Trim(), this.ckbPwd);
                frmMain main = new frmMain();
                main.Show();
                this.Hide();
            }
            else
            {
                //登录失败，显示失败信息
                MessageBox.Show(re.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sybtn_Click(object sender, EventArgs e)
        {
            var getUserDataMethod = ConfigurationManager.AppSettings["GetUsersData"];
            var UserData = SynchronousData.SendClient(getUserDataMethod);
            var ConvertData = JsonUtil.ConvertToObject<ClientDataModel>(UserData);
            SynchronousData.SynchronousUsersData(ConvertData.Data, "t_users");
            var getUnitDataMethod = ConfigurationManager.AppSettings["GetUnitData"];
            var UnitData = SynchronousData.SendClient(getUnitDataMethod);
            var ConvertUnitData = JsonUtil.ConvertToObject<ClientDataModel>(UnitData);
            SynchronousData.SynchronousUsersData(ConvertUnitData.Data, "t_Units");

            var getProjectDataMethod = ConfigurationManager.AppSettings["GetObjectData"];
            var ProjectData = SynchronousData.SendClient(getProjectDataMethod);
            var ConvertProjectData = JsonUtil.ConvertToObject<ClientDataModel>(ProjectData);
            SynchronousData.SynchronousUsersData(ConvertProjectData.Data, "t_Projects");

            var GetDetectionSettingsDataMethod= ConfigurationManager.AppSettings["GetDetectionSettingsData"];
            var DetectionData= SynchronousData.SendClient(GetDetectionSettingsDataMethod);
            var ConvertDeteData= JsonUtil.ConvertToObject<ClientDataModel>(DetectionData);
            SynchronousData.SynchronousUsersData(ConvertDeteData.Data, "t_DetectionSettings");

        }
        /// <summary>
        /// 登录按钮按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

            txtPwd.Focus();
        }

        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                btnLogin_Click(null, null);
        }
        /// <summary>
        ///数据库配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_setconfig_Click(object sender, EventArgs e)
        {
            frmDBCofnig frm_cofnig = new frmDBCofnig();
            frm_cofnig.ShowDialog();
        }
    }
}
