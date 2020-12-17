using GraphicsEvaluatePlatform.Infrastructure.Common;
using GraphicsEvaluatePlatform.Infrastructure.Common.ImageHelper;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;

namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    public partial class Form1 : Form
    {
        HubConnection hubConnection;
        IHubProxy hubProxy;
        IHubProxy proxy;
        private delegate void AddTxt(string msg);
        public Form1()
        {
            InitializeComponent();
            hubConnection = new HubConnection("http://localhost:15356/signalr/hubs");
            hubProxy = hubConnection.CreateHubProxy("myChatHub");
            //proxy = hubConnection.CreateHubProxy("broadcastMessage");
            hubProxy.On<string>("addMessage", (message) => this.Invoke(new AddTxt(Show), message));
            //proxy.On<string>("addMessage", (message) => this.Invoke(new AddTxt(Show), message));
            hubConnection.Start();
        }

       

        Bitmap bitmap;
        private void FormImages_Load(object sender, EventArgs e)
        {

        }
        //打开图像
        private void btn_openimgage_Click(object sender, EventArgs e)
        {
            OpenFileDialog filedlg = new OpenFileDialog();
            filedlg.Filter = "所有图片 (*.jpg)|*.jpg";
            if (filedlg.ShowDialog() == DialogResult.OK)
            {
                string path = filedlg.FileName;
                lbl_imgpath.Text = path;
                Image img = Bitmap.FromFile(path);
                try
                {
                    using (Bitmap bitmap = new Bitmap(img))
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            bitmap.Save(stream, ImageFormat.Jpeg);
                            pictureBox1.Image = Image.FromStream(stream);
                        }
                    }
                }
                finally
                {
                    if (img != null)
                    {
                        img.Dispose();
                        img = null;
                    }
                }

            }
        }
        //获取图像边斜角
        private void btn_getSkewAngle_Click(object sender, EventArgs e)
        {
            if (lbl_imgpath.Text != "0")
            {
                string path = lbl_imgpath.Text.Trim();
                using (Bitmap bitmap = new Bitmap(path))
                {
                    ImageProcessing sk = new ImageProcessing(bitmap);
                    double skewangle = sk.GetSkewAngle();
                    lbl_skewangle.Text = skewangle.ToString();
                }
            }
            else
            {
                MessageBox.Show("请打开图像", "提示");
                lbl_imgpath.Text = "0";

            }
        }
        //校正图像
        private void button1_Click(object sender, EventArgs e)
        {
            if (lbl_imgpath.Text != "0")
            {
                string path = lbl_imgpath.Text.Trim();
                Bitmap bm = new Bitmap(path);
                ImageProcessing sk = new ImageProcessing(bm);
                double du = double.Parse(txt_values.Text.Trim());
                Bitmap bmpSave = RotateToImage(bm, -du);
                string str_save = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                bmpSave.Save(str_save, ImageFormat.Jpeg);//保存

                bitmap = (Bitmap)Image.FromFile(str_save);
                pictureBox2.Image = bitmap.Clone() as Image;

            }
            else
            {
                MessageBox.Show("请打开图像", "提示");
                lbl_imgpath.Text = "0";
            }
        }
        //旋转图像
        private Bitmap RotateToImage(Bitmap bmp, double angle)
        {
            Graphics g = null;
            Bitmap tmp = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppRgb);
            tmp.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
            g = Graphics.FromImage(tmp);
            try
            {
                g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
                g.RotateTransform((float)angle);
                g.DrawImage(bmp, 0, 0);
            }
            finally
            {
                g.Dispose();
            }
            return tmp;
        }
        //添加隐式水印
        private void button2_Click(object sender, EventArgs e)
        {
            if (lbl_imgpath.Text != "0")
            {
                if (txt_bak.Text != "")
                {
                    //备份原始文件 
                    string backupFile = string.Format(@"{0}\{1}_bak.jpg", Path.GetDirectoryName(lbl_imgpath.Text.Trim()), Path.GetFileNameWithoutExtension(lbl_imgpath.Text));
                    File.Copy(lbl_imgpath.Text, backupFile, true);

                    // 判断文件是否存在，不存在则创建，否则读取值显示到窗体
                    string str_pathtxt = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                    if (!File.Exists(str_pathtxt))
                    {
                        FileStream fs1 = new FileStream(str_pathtxt, FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(this.txt_bak.Text.Trim());//开始写入值
                        sw.Close();
                        fs1.Close();
                    }
                    else
                    {
                        FileStream fs = new FileStream(str_pathtxt, FileMode.Open, FileAccess.Write);
                        StreamWriter sr = new StreamWriter(fs);
                        sr.WriteLine(txt_bak.Text.Trim());//开始写入值
                        sr.Close();
                        fs.Close();
                    }
                    //生成图像 
                    LSB_Encrypt lsb = new LSB_Encrypt(lbl_imgpath.Text, str_pathtxt);
                    lsb.Exe_Encrypt();

                    MessageBox.Show("写入成功！", "提示");
                }
                else
                {
                    MessageBox.Show("请写入水印内容", "提示");
                    txt_bak.Focus();
                }
            }
            else
            {
                MessageBox.Show("请打开图像", "提示");
                lbl_imgpath.Text = "0";
            }

        }

        //校验水印
        private void button3_Click(object sender, EventArgs e)
        {
            if (lbl_imgpath.Text != "0")
            {
                string str_pathtxt = Application.StartupPath + "\\W" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                if (!File.Exists(str_pathtxt))
                {
                    FileStream fs1 = new FileStream(str_pathtxt, FileMode.Create, FileAccess.Write);//创建写入文件 
                    fs1.Close();
                }
                LSB_Decrypt decrypt = new LSB_Decrypt(lbl_imgpath.Text, str_pathtxt);
                if (decrypt.ExecuteDecrypt())
                {
                    StreamReader sr = new StreamReader(str_pathtxt, Encoding.UTF8);
                    txt_bak.Text = sr.ReadLine().ToString();
                    sr.Close();
                    File.Delete(str_pathtxt);//删除txt
                    MessageBox.Show("已成功提取隐藏信息！", "提示");
                }
                else
                {
                    MessageBox.Show("所选择的图像不包含任何隐藏信息！", "提示");
                }

            }
            else
            {
                MessageBox.Show("请打开图像", "提示");
                lbl_imgpath.Text = "0";
            }
        }
        //图像分辨率
        private void button4_Click(object sender, EventArgs e)
        {
            if (lbl_imgpath.Text != "0")
            {
                Image pic = Image.FromFile(lbl_imgpath.Text.Trim());//图片的绝对路径
                pictureBox1.Image = pic;
                int intWidth = pic.Width;//长度像素值
                int intHeight = pic.Height;//高度像素值 

                lbl_w.Text = intWidth.ToString() + " 像素";
                lbl_h.Text = intHeight.ToString() + " 像素";
            }
            else
            {
                MessageBox.Show("请打开图像", "提示");
            }
        }
        //图像大小
        private void button5_Click(object sender, EventArgs e)
        {
            if (lbl_imgpath.Text != "0")
            {
                System.IO.FileInfo file = new System.IO.FileInfo(lbl_imgpath.Text.Trim());
                decimal dec_file = file.Length / 1024;
                lbl_size.Text = dec_file.ToString() + " KB";
            }
            else
            {
                MessageBox.Show("请打开图像", "提示");
            }
        }
        private void Show(object msg)
        {
            if (txtMsg.InvokeRequired)
            {
                txtMsg.Invoke(new MethodInvoker(() => { txtMsg.Text += msg + "\r\n"; }));
            }
            else
            {
                txtMsg.Text += msg + "\r\n";
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            if (hubConnection.State == ConnectionState.Connected)
            {
                hubProxy.Invoke("send", txtSend.Text);
            }










            string url = "http://localhost:15356";
            string mapUrl = "api/TestAPI/Test";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("id", "123");
                    var requestJson = JsonUtil.ToJson(dic);
                    HttpContent httpContent = new StringContent(requestJson);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var result = client.PostAsync(mapUrl, httpContent).Result.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("接口访问数据异常" + ex.Message.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            //return model;

        }
    }
}
