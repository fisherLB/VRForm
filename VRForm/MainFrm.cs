using VRForm.Mdel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VRForm
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
            showtime.Text = BaseClass.VRTIME;
            UsernameLabel.Text = BaseClass.USERACCOUNTNAME;
        }

        private void btn_Logout(object sender, EventArgs e)
        {
            this.Hide();
            LoginFrm loginFrm = new LoginFrm();
            loginFrm.Show();
        }

        private void btn_StartVR(object sender, EventArgs e)
        {
            showtime.Text = "1小时20分钟";
           // StartVR();
           // RunAppasAdmin(@"C:\Program Files (x86)\360\360Safe\SoftMgr\SoftMgr.exe");
        }

        private void btn_StopVR(object sender, EventArgs e)
        {

            //关闭窗体
            Process[] pro = Process.GetProcesses();//获取已开启的所有进程
            //遍历所有查找到的进程
            for (int i = 0; i < pro.Length; i++)
            {
                //判断此进程是否是要查找的进程
                if (pro[i].ProcessName.ToString().ToLower() == "vr")
                {
                    pro[i].Kill();//结束进程
                }
            }
            
        }


        private void StartVR()
        {
            //保存资源文件
            FileStream str = new FileStream(@"C:\TEMP\vr.exe", FileMode.OpenOrCreate);
            /*注：     
             * 1.先添加组件--->常规--->资源文件
             * 2.在Resource1.resx中添加资源--->添加现有文件--->把文件名改成test
             */
            str.Write(test.vr, 0, test.vr.Length);
            str.Close();
            //打开应用程序
            if (File.Exists(@"C:\TEMP\vr.exe"))
            {
                Process.Start(@"C:\TEMP\vr.exe");
                //隐藏当前窗体             
                this.Hide();
            }
            else
            {
                MessageBox.Show("找不到要运行的程序！");
            }
        }

        public void RunAppasAdmin(string strUri)
        {
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName = strUri;
                psi.UseShellExecute = true;
                psi.CreateNoWindow = true;
                psi.Arguments = "";//带参bai数
                try
                {
                    System.Diagnostics.Process.Start(psi);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName = strUri;
                psi.UseShellExecute = true;
                psi.WorkingDirectory = Environment.CurrentDirectory;
                psi.Verb = "runas";
                try
                {
                    System.Diagnostics.Process.Start(psi);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void btn_Recharge(object sender, EventArgs e)
        {
            RechargeFrm rechargeFrm = new RechargeFrm();
            this.Hide();
            rechargeFrm.Show();
        }
    }
}
