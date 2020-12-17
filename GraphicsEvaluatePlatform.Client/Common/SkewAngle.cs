using System;
using System.Drawing;
using System.Drawing.Imaging;
namespace GraphicsEvaluatePlatform.Client.Common
{
    public static class SkewAngle
    {
        // Bitmap c_bmp;
        static double c_start = -20;
        static double c_step = 0.2;
        static int c_steps = 40 * 5;
        static double[] c_sinA;
        static double[] c_cosA;
        static double c_DMin;
        static double c_DStep = 1;
        static int cDi_Count;
        //在直线上的点的个数
        static int[] c_HMatrix;
        public struct InsLine
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
        public static double GetSkewAngle(this Bitmap bmp)
        {
            InsLine[] hl = null;
            int i = 0;
            double sum = 0;
            int i_Count = 0;
            //变换
            TransFormImage(bmp);
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
        private static InsLine[] GetTop(int i_Count)
        {
            InsLine[] hl = null;
            int i = 0;
            int j = 0;
            InsLine tmp;
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
        //public static SkewAngle(Bitmap bmp)
        //{
        //    c_bmp = bmp;
        //}
        //变换
        private static void TransFormImage(Bitmap c_bmp)
        {
            int x = 0;
            int y = 0;
            int hMin = c_bmp.Height / 4;
            int hMax = c_bmp.Height * 3 / 4;
            Init(c_bmp);
            for (y = hMin; y <= hMax; y++)
            {
                for (x = 1; x <= c_bmp.Width - 2; x++)
                {
                    //边缘
                    if (IsBlack(c_bmp,x, y))
                    {
                        if (!IsBlack(c_bmp,x, y + 1))
                        {
                            TransFormImage(x, y);
                        }
                    }
                }
            }
        }
        private static void TransFormImage(int x, int y)
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
        private static double TransFormImageDi_Index(double d)
        {
            return Convert.ToInt32(d - c_DMin);
        }
        //颜色--是否是黑色
        private static bool IsBlack(Bitmap c_bmp,int x, int y)
        {
            Color c = default(Color);
            double luminance = 0;
            c = c_bmp.GetPixel(x, y);
            luminance = (c.R * 0.299) + (c.G * 0.587) + (c.B * 0.114);
            return luminance < 140;
        }
        private static void Init(Bitmap c_bmp)
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
        public static double Getd_Alpha(int i_Index)
        {
            return c_start + i_Index * c_step;
        }
        public static Bitmap RotateToImage(Bitmap bmp, double angle)
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
    }
}
