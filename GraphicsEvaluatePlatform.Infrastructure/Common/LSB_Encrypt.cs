using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Infrastructure.Common
{
    //隐藏文字
    public  class LSB_Encrypt
    {
        //原始图片路径 
        private string original_PicPath = null;
        //隐藏信息路径 
        private string hiding_txt = null;
        //原始图片的文件流 
        private FileStream pic_Stream = null;
        //隐藏信息的文件流 
        private FileStream txt_Stream = null;

        /// <summary> 
        /// LSBEncrypt类的构造函数 
        /// </summary> 
        /// <param name="path1">原始图片路径</param> 
        /// <param name="path2">隐藏信息</param> 
        public LSB_Encrypt(string path, string txt)
        {
            original_PicPath = path;
            hiding_txt = txt;
        }
        /// <summary> 
        /// 将长整型数转换为24位二进制表示的字节数组 
        /// </summary> 
        /// <param name="x">要转换的长整型数</param> 
        /// <returns>二进制表示的字节数组</returns> 
        private byte[] ConvertToBinaryArray(long x)
        {
            byte[] binaryArray = new byte[24];
            for (int i = 0; i != 23; i++)
            {
                binaryArray[23 - i] = (byte)(x & 1);
                x = x >> 1;
            }
            return binaryArray;
        }
        /// <summary> 
        /// 将图像的第55至第66字节的LSB替换为隐藏信息文件的长度 
        /// </summary> 
        private void HideTxtLength()
        {
            byte[] picBlock = new byte[12];
            //读取原始图像的第55至第66字节的内容块 
            pic_Stream.Position = 54;
            pic_Stream.Read(picBlock, 0, picBlock.Length);
            byte[] lenArray = ConvertToBinaryArray(txt_Stream.Length);
            //置入隐藏文件的长度信息 
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                //置a7，a6位 
                picBlock[i * 3] = (byte)((lenArray[index++] == 0) ? (picBlock[i * 3] & 253) : (picBlock[i * 3] | 2));
                picBlock[i * 3] = (byte)((lenArray[index++] == 0) ? (picBlock[i * 3] & 254) : (picBlock[i * 3] | 1));
                //置a5位 
                picBlock[i * 3 + 1] = (byte)((lenArray[index++] == 0) ? (picBlock[i * 3 + 1] & 254) : (picBlock[i * 3 + 1] | 1));
                //置a4,a3,a2位 
                picBlock[i * 3 + 2] = (byte)((lenArray[index++] == 0) ? (picBlock[i * 3 + 2] & 251) : (picBlock[i * 3 + 2] | 4));
                picBlock[i * 3 + 2] = (byte)((lenArray[index++] == 0) ? (picBlock[i * 3 + 2] & 253) : (picBlock[i * 3 + 2] | 2));
                picBlock[i * 3 + 2] = (byte)((lenArray[index++] == 0) ? (picBlock[i * 3 + 2] & 254) : (picBlock[i * 3 + 2] | 1));
            }
            //将原始文件流重定位到第55字节处并将已嵌入长度信息的12字节块写回 
            pic_Stream.Position = 54;
            pic_Stream.Write(picBlock, 0, picBlock.Length);
            pic_Stream.Flush();
        }
        /// <summary> 
        /// 将3个字节的字节数组转换为24位(8bit * 3)二进制表示的字节数组 
        /// </summary> 
        /// <param name="array">长度为3的字节数组</param> 
        /// <returns>二进制表示的字节数组</returns> 
        private byte[] ConvertToBinaryArray(byte[] array)
        {
            byte[] binaryArray = new byte[24];
            int a = array[0];
            int b = array[1];
            int c = array[2];
            for (int i = 0; i != 8; i++)
            {
                binaryArray[7 - i] = (byte)(a & 1);
                a = a >> 1;
            }
            for (int i = 0; i != 8; i++)
            {
                binaryArray[15 - i] = (byte)(b & 1);
                b = b >> 1;
            }
            for (int i = 0; i != 8; i++)
            {
                binaryArray[23 - i] = (byte)(c & 1);
                c = c >> 1;
            }
            return binaryArray;
        }

        /// <summary> 
        /// 将隐藏信息以每3个字节写入原始图像从第67字节开始的每12字节块的LSB中 
        /// </summary> 
        private void HideTxtContent()
        {
            int readCnt = 0;
            //计算循环处理的次数 
            long infoLen = txt_Stream.Length;
            int cnt = (int)(infoLen % 3 == 0 ? infoLen / 3 : infoLen / 3 + 1);
            pic_Stream.Position = 66;
            for (int i = 0; i < cnt; i++)
            {
                //每次循环读取BMP图像的下一个12字节的内容 
                byte[] picBlock = new byte[12];
                readCnt = pic_Stream.Read(picBlock, 0, picBlock.Length);
                //读取待隐藏信息的下一个3字节内容 
                byte[] readBuffer = new byte[3];
                txt_Stream.Read(readBuffer, 0, readBuffer.Length);
                byte[] infoBlock = ConvertToBinaryArray(readBuffer);
                //置位操作 
                int index = 0;
                for (int ii = 0; ii < 4; ii++)
                {
                    //置a7，a6位 
                    picBlock[ii * 3] = (byte)((infoBlock[index++] == 0) ? (picBlock[ii * 3] & 253) : (picBlock[ii * 3] | 2));
                    picBlock[ii * 3] = (byte)((infoBlock[index++] == 0) ? (picBlock[ii * 3] & 254) : (picBlock[ii * 3] | 1));
                    //置a5位 
                    picBlock[ii * 3 + 1] = (byte)((infoBlock[index++] == 0) ? (picBlock[ii * 3 + 1] & 254) : (picBlock[ii * 3 + 1] | 1));
                    //置a4,a3,a2位 
                    picBlock[ii * 3 + 2] = (byte)((infoBlock[index++] == 0) ? (picBlock[ii * 3 + 2] & 251) : (picBlock[ii * 3 + 2] | 4));
                    picBlock[ii * 3 + 2] = (byte)((infoBlock[index++] == 0) ? (picBlock[ii * 3 + 2] & 253) : (picBlock[ii * 3 + 2] | 2));
                    picBlock[ii * 3 + 2] = (byte)((infoBlock[index++] == 0) ? (picBlock[ii * 3 + 2] & 254) : (picBlock[ii * 3 + 2] | 1));
                }
                pic_Stream.Position -= readCnt;
                pic_Stream.Write(picBlock, 0, picBlock.Length);
            }
            pic_Stream.Flush();
        }

        /// <summary> 
        /// 执行信息隐藏操作 
        /// </summary> 
        public void Exe_Encrypt()
        {
            pic_Stream = new FileStream(original_PicPath, FileMode.Open, FileAccess.ReadWrite);
            txt_Stream = new FileStream(hiding_txt, FileMode.Open, FileAccess.Read);
            HideTxtLength();
            HideTxtContent();
            pic_Stream.Close();
            txt_Stream.Close();
        }

    }

}
