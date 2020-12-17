using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using VRForm.Lib;

namespace VRForm
{
    public partial class LoginFrm : Form
    {
        public LoginFrm()
        {
            InitializeComponent();
        }

        private void sybtn_Click(object sender, EventArgs e)
        {

        }

       

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (UserName.Text.Trim() == "")
            {
                MessageBox.Show("请输入账户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UserName.Focus();
                return;
            }
            if (PassWord.Text.Trim() == "")
            {
                MessageBox.Show("请输入密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PassWord.Focus();
                return;
            }

            OperationResult re = LoginBll.LoginValidate(UserName.Text.Trim(), PassWord.Text.Trim());
            //登录成功
            if (re.ResultType == OperationResultType.Success)
            {
                
                MainFrm main = new MainFrm();
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

     

      

        private void logon_DrawItem(object sender, DrawItemEventArgs e)
        {
            StringFormat sf = new StringFormat();

            #region 头背景
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            Rectangle rec = tabControl1.ClientRectangle;
            //获取背景图片，我的背景图片在项目资源文件中。
            //Image backImage = Properties.Resources.;
            //e.Graphics.DrawImage(backImage, 0, 2, tabControl1.Width, tabControl1.ItemSize.Height + 2);
            #endregion
            #region  设置选择的标签的背景
            //if (e.Index == tabControl1.SelectedIndex)
            //    e.Graphics.DrawImage(Properties.Resources.pop_tab1_1, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            //else
            //    e.Graphics.DrawImage(Properties.Resources.pop_tab1_2, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            e.Graphics.DrawString(((TabControl)sender).TabPages[e.Index].Text,
            System.Windows.Forms.SystemInformation.MenuFont, new SolidBrush(Color.Black), e.Bounds, sf);
            #endregion
            #region 重写标签名
            ColorConverter colorConverter = new ColorConverter();
            Color cwhite = (Color)colorConverter.ConvertFromString("#2178ba");
            SolidBrush white = new SolidBrush(cwhite);
            Rectangle rect0 = tabControl1.GetTabRect(0);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            e.Graphics.DrawString("申请单号", new Font("微软雅黑", 12), white, rect0, stringFormat);

            #endregion
        }

        

        private void regbtn_Click(object sender, EventArgs e)
        {
            if (RegUserName.Text.Trim() == "")
            {
                MessageBox.Show("请输入账户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RegUserName.Focus();
                return;
            }
            if (RegPassword.Text.Trim() == "")
            {
                MessageBox.Show("请输入密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RegPassword.Focus();
                return;
            }

            OperationResult re = RegBll.RegUser(RegUserName.Text.Trim(), RegPassword.Text.Trim());
            //登录成功
            if (re.ResultType == OperationResultType.Success)
            {

                MessageBox.Show(re.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            
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
