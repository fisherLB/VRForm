using GraphicsEvaluatePlatform.Infrastructure.Common;
using GraphicsEvaluatePlatform.Infrastructure.Common.ImageHelper;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;
namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    public partial class FrmDetectionSingle : Form
    {
        public string PicId = "";
        public string PicPath = "";
        public string PicName = "";
        public string PicType = "";
        public static Bitmap bitmap;
        ImageProcessing sk = null;
        private static Bitmap CloneMap;
        private string CloneId = "";
        private string ClonePath = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";//指定备份图像路径;
        private string CloneName = "";

        private delegate void AddTxt(string msg);
        public FrmDetectionSingle()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmDetectionSingle_Load(object sender, EventArgs e)
        {

            DataTable dtClass = new DataTable("dtClass");
            dtClass.Columns.Add("name");
            dtClass.Columns.Add("id");
            dtClass.Rows.Add(new object[] { "JPG", "0" });
            dtClass.Rows.Add(new object[] { "GIF", "1" });
            dtClass.Rows.Add(new object[] { "BMP", "2" });

            cmbType.DisplayMember = "name";
            cmbType.ValueMember = "id";
            cmbType.DataSource = dtClass;


            txtContent.Focus();
            lblPath.Text = PicPath;
            lblName.Text = PicName;
            txtName.Text = PicName;
            lblType.Text = PicType;
            //加载图片

            if (!string.IsNullOrEmpty(PicPath))
            {
                //取图像大小
                FileInfo file = new FileInfo(PicPath);
                decimal dec_file = file.Length / 1024;
                lbl_size.Text = dec_file.ToString() + " KB";
                txtSize.Text = dec_file.ToString() + " KB";
                Image img = Bitmap.FromFile(PicPath);
                //图片的绝对路径
                pictureBox1.Image = img;
                pictureBox2.Image = img;
                nudHeight.Value = img.Height;
                nudWidth.Value = img.Width;
                lblResolution.Text = img.Width.ToString() + " * " + img.Height.ToString() + " 像素";
                try
                {
                    bitmap = new Bitmap(img);
                    ImageProcessing sk = new ImageProcessing(bitmap);
                    // using ( bitmap = new Bitmap(img))
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormat.Jpeg);//将图像以指定的格式保存到指定的流中。
                        pictureBox1.Image = Image.FromStream(stream);
                    }
                    //取倾斜角

                    double skewangle = sk.GetSkewAngle();
                    lblSkewness.Text = skewangle.ToString();
                    nudSkewness.Text = skewangle.ToString();


                    //取水印内容
                    string str_pathtxt = Application.StartupPath + "\\W" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                    if (!File.Exists(str_pathtxt))
                    {
                        FileStream fs1 = new FileStream(str_pathtxt, FileMode.Create, FileAccess.Write);//创建写入文件 
                        fs1.Close();
                    }
                    LSB_Decrypt decrypt = new LSB_Decrypt(lblPath.Text, str_pathtxt);
                    if (decrypt.ExecuteDecrypt())
                    {
                        StreamReader sr = new StreamReader(str_pathtxt, Encoding.UTF8);
                        txtContent.Text = sr.ReadLine().ToString();
                        lblContent.Text = sr.ReadLine().ToString();
                        sr.Close();
                        File.Delete(str_pathtxt);//删除txt
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
                //var Contrast=ImageHelper.LDPic(img,width,height,val)

            }
        }
        /// <summary>
        /// 旋转图像
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="angle"></param>
        /// <returns></returns>


        /// <summary>
        /// 纠偏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Adjust(double dou)
        {
            Bitmap bm = new Bitmap(PicPath);
            ImageProcessing sk = new ImageProcessing(bm);
            CloneMap = sk.RotateToImage(bm, -dou);
            //bmpSave.Save(path, ImageFormat.Jpeg);//保存
            // bitmap = (Bitmap)Image.FromFile(path);
            pictureBox2.Image = CloneMap.Clone() as Image;
        }
        private void ContrastSet(double dou)
        {

            Bitmap bm = new Bitmap(PicPath);
            ImageProcessing sk = new ImageProcessing(bm);
            CloneMap = sk.Contrast_Pic(bm, dou);
            if (pictureBox2.InvokeRequired)
                pictureBox2.Invoke(new MethodInvoker(() =>
                {
                    pictureBox2.Image = CloneMap.Clone() as Image;
                }));
            else
            {
                pictureBox2.Image = CloneMap.Clone() as Image;
            }

        }
        private void LightSet(int val)
        {
            bitmap = new Bitmap(PicPath);
            ImageProcessing sk = new ImageProcessing(bitmap);
            CloneMap = sk.Brightness_Pic(bitmap, val);
            if (pictureBox2.InvokeRequired)
                pictureBox2.Invoke(new MethodInvoker(() =>
                {
                    pictureBox2.Image = CloneMap.Clone() as Image;
                }));
            else
            {
                pictureBox2.Image = CloneMap.Clone() as Image;
            }
        }
        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 偏斜角度滑块滑动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSkewness_Scroll(object sender, EventArgs e)
        {
            if (!nudSkewness.Focused)
            {
                nudSkewness.Value = tbSkewness.Value;
            }
            Adjust(tbSkewness.Value);
        }
        /// <summary>
        /// 偏斜角度滑块值变动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudSkewness_ValueChanged(object sender, EventArgs e)
        {
            if (!tbSkewness.Focused)
                tbSkewness.Value = (int)(nudSkewness.Value);
            Adjust((double)nudSkewness.Value);
        }
        /// <summary>
        /// 对比度滑动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbContrast_Scroll(object sender, EventArgs e)
        {

            var tvalue = tbContrast.Value;
            var nvalue = nudContrast.Value;
            if (!nudContrast.Focused)
            {
                nudContrast.Value = (decimal)tvalue;
                //  double v = (double)tvalue - (double)nvalue;
                ContrastSet(tvalue);
            }
        }
        /// <summary>
        /// 对比度值变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudContrast_ValueChanged(object sender, EventArgs e)
        {
            var tvalue = tbContrast.Value;
            var nvalue = nudContrast.Value;
            if (!tbContrast.Focused)
            {
                tbContrast.Value = (int)nvalue;        
                ContrastSet((double)nvalue);
            }
        }
        /// <summary>
        /// 亮度滑动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbLight_Scroll(object sender, EventArgs e)
        {
            var tvalue = tbLight.Value;
            var nvalue = nudLight.Value;
            if (!nudLight.Focused)
            {
                nudLight.Value = (decimal)tvalue;
                LightSet(tvalue);
            }

        }

        /// <summary>
        /// 亮度变值事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudLight_ValueChanged(object sender, EventArgs e)
        {
            var tvalue = tbLight.Value;
            var nvalue = nudLight.Value;
            if (!tbLight.Focused)
            {
                tbLight.Value = (int)nvalue;
                var v = nvalue - tvalue;
                LightSet((int)nvalue);
            }

        }
        /// <summary>
        /// 确认按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PicPath))
                return;
            Bitmap bm = new Bitmap(PicPath);//生成图片对象
            double du = tbSkewness.Value;//取偏斜度设定值
            ImageProcessing sk = new ImageProcessing(bm);
            Bitmap bmpSave = sk.RotateToImage(bm, -du);//偏斜度
            if (ClonePath == "")
            {
                ClonePath = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";//指定备份图像路径

            }
            bmpSave.Save(ClonePath, ImageFormat.Jpeg);//保存生成新的图像文件
            CloneMap = (Bitmap)Image.FromFile(ClonePath);//打开新的图像为clonemap
            pictureBox2.Image = CloneMap.Clone() as Image;//在控件pictureBox2中呈现
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {

            FrmDetectionSingle_Load(sender, e);
        }
        /// <summary>
        /// 加水印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddWater_Click(object sender, EventArgs e)
        {
            if (PicPath != "0")
            {
                if (txtContent.Text != "")
                {
                    //备份原始文件 
                    string backupFile = string.Format(@"{0}\{1}_bak.jpg", Path.GetDirectoryName(lblPath.Text.Trim()), Path.GetFileNameWithoutExtension(lblPath.Text));
                    File.Copy(lblPath.Text, backupFile, true);

                    // 判断文件是否存在，不存在则创建，否则读取值显示到窗体
                    if (ClonePath == "")
                    {
                        ClonePath = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                    }
                    if (!File.Exists(ClonePath))
                    {
                        FileStream fs1 = new FileStream(ClonePath, FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(this.txtContent.Text.Trim());//开始写入值
                        sw.Close();
                        fs1.Close();
                    }
                    else
                    {
                        FileStream fs = new FileStream(ClonePath, FileMode.Open, FileAccess.Write);
                        StreamWriter sr = new StreamWriter(fs);
                        sr.WriteLine(txtContent.Text.Trim());//开始写入值
                        sr.Close();
                        fs.Close();
                    }
                    //生成图像 
                    LSB_Encrypt lsb = new LSB_Encrypt(lblPath.Text, ClonePath);
                    lsb.Exe_Encrypt();
                }
                else
                {
                    MessageBox.Show("请写入水印内容", "提示");
                    txtContent.Focus();
                }
            }
            else
            {
                MessageBox.Show("请打开图像", "提示");
                lblPath.Text = "0";
            }
        }
    }
}
