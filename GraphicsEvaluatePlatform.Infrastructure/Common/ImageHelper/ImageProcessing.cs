using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Infrastructure.Common.ImageHelper
{

    /// <summary>
    ///  图像处理类
    /// </summary>
    public class ImageProcessing
    {
        public class InsLine
        {
            //直线上的点数
            public int i_Count;
            //指针在矩阵
            public int i_Index;
            //α
            public double d_Alpha;
            //值
            public double d_values;
        }
        Bitmap c_bmp;
        double c_start = -20;
        double c_step = 0.2;
        int c_steps = 40 * 5;
        double[] c_sinA;
        double[] c_cosA;
        double c_DMin;
        double c_DStep = 1;
        int cDi_Count;
        //在直线上的点的个数
        int[] c_HMatrix;

        public double GetSkewAngle()
        {
            ImageProcessing.InsLine[] hl = null;
            int i = 0;
            double sum = 0;
            int i_Count = 0;
            //变换
            TransFormImage();
            // 图像中检测到的前20条线
            hl = GetTop(20);
            //线的平均角
            for (i = 0; i <= 19; i++)
            {
                sum += hl[i].d_Alpha;
                i_Count += 1;
            }
            return sum / i_Count;
        }
        //检测图像20条线
        private InsLine[] GetTop(int i_Count)
        {
            InsLine[] hl = null;
            int i = 0;
            int j = 0;
            InsLine tmp = null;
            int d_Alphai_Index = 0;
            int di_Index = 0;
            hl = new InsLine[i_Count + 1];
            for (i = 0; i <= i_Count - 1; i++)
            {
                hl[i] = new InsLine();
            }
            for (i = 0; i <= c_HMatrix.Length - 1; i++)
            {
                if (c_HMatrix[i] > hl[i_Count - 1].i_Count)
                {
                    hl[i_Count - 1].i_Count = c_HMatrix[i];
                    hl[i_Count - 1].i_Index = i;
                    j = i_Count - 1;
                    while (j > 0 && hl[j].i_Count > hl[j - 1].i_Count)
                    {
                        tmp = hl[j];
                        hl[j] = hl[j - 1];
                        hl[j - 1] = tmp; j -= 1;
                    }
                }
            }
            for (i = 0; i <= i_Count - 1; i++)
            {
                di_Index = hl[i].i_Index / c_steps;
                d_Alphai_Index = hl[i].i_Index - di_Index * c_steps;
                hl[i].d_Alpha = Getd_Alpha(d_Alphai_Index);
                hl[i].d_values = di_Index + c_DMin;
            }
            return hl;
        }
        //图像来源
        public ImageProcessing(Bitmap bmp)
        {
            c_bmp = bmp;
        }
        //变换
        private void TransFormImage()
        {
            int x = 0;
            int y = 0;
            int hMin = c_bmp.Height / 4;
            int hMax = c_bmp.Height * 3 / 4;
            Init();
            for (y = hMin; y <= hMax; y++)
            {
                for (x = 1; x <= c_bmp.Width - 2; x++)
                {
                    //边缘
                    if (IsBlack(x, y))
                    {
                        if (!IsBlack(x, y + 1))
                        {
                            TransFormImage(x, y);
                        }
                    }
                }
            }
        }
        private void TransFormImage(int x, int y)
        {
            int d_Alpha = 0;
            double d = 0;
            int di_Index = 0;
            int i_Index = 0;
            for (d_Alpha = 0; d_Alpha <= c_steps - 1; d_Alpha++)
            {
                d = y * c_cosA[d_Alpha] - x * c_sinA[d_Alpha];
                di_Index = (int)TransFormImageDi_Index(d);
                i_Index = di_Index * c_steps + d_Alpha;
                try
                {
                    c_HMatrix[i_Index] += 1;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                }
            }
        }
        private double TransFormImageDi_Index(double d)
        {
            return Convert.ToInt32(d - c_DMin);
        }
        //颜色--是否是黑色
        private bool IsBlack(int x, int y)
        {
            Color c = default(Color);
            double luminance = 0;
            c = c_bmp.GetPixel(x, y);
            luminance = (c.R * 0.299) + (c.G * 0.587) + (c.B * 0.114);
            return luminance < 140;
        }
        private void Init()
        {
            int i = 0;
            double angle = 0;
            // 预先计算sin和cos
            c_sinA = new double[c_steps];
            c_cosA = new double[c_steps];
            for (i = 0; i <= c_steps - 1; i++)
            {
                angle = Getd_Alpha(i) * Math.PI / 180.0;
                c_sinA[i] = Math.Sin(angle);
                c_cosA[i] = Math.Cos(angle);
            }  // d的范围
            c_DMin = -c_bmp.Width;
            cDi_Count = (int)(2 * (c_bmp.Width + c_bmp.Height) / c_DStep);
            c_HMatrix = new int[cDi_Count * c_steps + 1];
        }
        public double Getd_Alpha(int i_Index)
        {
            return c_start + i_Index * c_step;
        }
        public Bitmap RotateToImage(Bitmap bmp, double angle)
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
        public  Bitmap Brightness_Pic(Bitmap a, int v)
        {
            try
            {
                int w = a.Width;
                int h = a.Height;
                BitmapData bmpData = a.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
              //  int bytes = a.Width * a.Height * 3;
                IntPtr ptr = bmpData.Scan0;//像素起始地址
                int stride = bmpData.Stride;//跨距
       
                unsafe
                {
                    byte* p = (byte*)ptr;
                    int temp;
                    for (int j = 0; j < a.Height; j++)//每一行
                    {
                        for (int i = 0; i < a.Width * 3; i++, p++)//
                        {
                            temp = (int)(p[0] + v);
                            temp = (temp > 255) ? 255 : temp < 0 ? 0 : temp;
                            p[0] = (byte)temp;
                        }
                        p += stride - a.Width * 3;
                    }
                }
                a.UnlockBits(bmpData);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {

            }

            return a;
        }

        //对比度调整
        public  Bitmap Contrast_Pic(Bitmap a, double v)
        {
            BitmapData bmpData = a.LockBits(new Rectangle(0, 0, a.Width, a.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int bytes = a.Width * a.Height * 3;
            IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;
            unsafe
            {
                byte* p = (byte*)ptr;
                int temp;
                for (int j = 0; j < a.Height; j++)
                {
                    for (int i = 0; i < a.Width * 3; i++)
                    {
                        var c = p[0] - 127;
                        temp = (int)((p[0] - 127) * v + 127);
                        temp = (temp > 255) ? 255 : temp < 0 ? 0 : temp;
                        p[0] = (byte)temp;
                        p++;
                    }
                    p += stride - a.Width * 3;
                }
            }
            a.UnlockBits(bmpData);
            return a;
        }

    }
}
