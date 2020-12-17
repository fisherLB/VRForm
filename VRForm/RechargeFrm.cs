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
using VRForm.Lib;
using VRForm.Mdel;

namespace VRForm
{
    public partial class RechargeFrm : Form
    {
        public RechargeFrm()
        {
            InitializeComponent();
            this.FormClosed += ReturnMainFrm;
            UsernameLable.Text = BaseClass.USERACCOUNTNAME;
        }

        private void ReturnMainFrm(object sender, EventArgs e)
        {
            this.Hide();
            MainFrm mainFrm = new MainFrm();
            mainFrm.Show();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn_Recharge(object sender, EventArgs e)
        {
            if (CarNumber.Text.Trim() == "")
            {
                MessageBox.Show("请输入卡密", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CarNumber.Focus();
                return;
            }
            OperationResult re = RechargeBll.Recharge(CarNumber.Text);
            //登录成功
            if (re.ResultType == OperationResultType.Success)
            {
                MessageBox.Show(re.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                //登录失败，显示失败信息
                MessageBox.Show(re.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
