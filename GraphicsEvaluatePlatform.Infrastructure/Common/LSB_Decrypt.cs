using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Infrastructure.Common
{
    //提取文字
    public class LSB_Decrypt
    {
        //伪装图片的路径 
        private string cam_PicPath = null;
        //还原出的隐藏信息的保存路径 
        private string txt_SavePath = null;
        //伪装图片的文件流 
        private FileStream cam_Stream = null;
        //还原出的隐藏信息的文件流 
        private FileStream txt_SaveStream = null;

        /// <summary> 
        /// LSBDecrypt的构造函数 
        /// </summary> 
        /// <param name="path1">伪装图片的路径</param> 
        /// <param name="path2">还原出的隐藏信息的保存路径</param> 
        public LSB_Decrypt(string path1, string path2)
        {
            cam_PicPath = path1;
            txt_SavePath = path2;
        }

        /// <summary> 
        /// 从伪装图片的第55至第66字节中提取出隐藏信息的长度 
        /// </summary> 
        /// <returns>隐藏信息长度</returns> 
        private int GetInfoLength()
        {
            cam_Stream = new FileStream(cam_PicPath, FileMode.Open, FileAccess.Read);
            //将文件流定位到第55个字节处 
            cam_Stream.Position = 54;
            //读取包含隐藏信息长度的12个字节块 
            byte[] lengthBlock = new byte[12];
            cam_Stream.Read(lengthBlock, 0, lengthBlock.Length);
            //提取LSB 
            byte[] lengthBitArray = ExtractHidingBits(lengthBlock);
            //还原出整型长度值 
            int len = lengthBitArray[0] * 65536 + lengthBitArray[1] * 256 + lengthBitArray[2];
            //如果长度不正确表明该图片不含有隐藏信息，此检测LSB方法不能适用于所有情况，具体的检测算法未知 
            if (len <= 0 || len > (cam_Stream.Length - 54) / 4 - 3)
            {
                cam_Stream.Close();
                return -1;
            }
            return len;
        }

        /// <summary> 
        /// 利用位操作提取伪装文件流中每12字节的LSB位 
        /// </summary> 
        /// <param name="arr">长度为12的字节数组，含有隐藏信息</param> 
        /// <returns>从12字节块中提取出的3字节隐藏信息</returns> 
        private byte[] ExtractHidingBits(byte[] arr)
        {
            //用于存放从12个字节块中提取出来的3个字节的隐藏信息 
            byte[] buf = new byte[3];
            //24个bit位的处理 
            buf[0] = (byte)((arr[0] & 2) == 0 ? (buf[0] & 127) : (buf[0] | 128));           //a7 
            buf[0] = (byte)((arr[0] & 1) == 0 ? (buf[0] & 191) : (buf[0] | 64));            //a6 
            buf[0] = (byte)((arr[1] & 1) == 0 ? (buf[0] & 223) : (buf[0] | 32));            //a5 
            buf[0] = (byte)((arr[2] & 4) == 0 ? (buf[0] & 239) : (buf[0] | 16));            //a4 
            buf[0] = (byte)((arr[2] & 2) == 0 ? (buf[0] & 247) : (buf[0] | 8));             //a3 
            buf[0] = (byte)((arr[2] & 1) == 0 ? (buf[0] & 251) : (buf[0] | 4));             //a2 
            buf[0] = (byte)((arr[3] & 2) == 0 ? (buf[0] & 253) : (buf[0] | 2));             //a1 
            buf[0] = (byte)((arr[3] & 1) == 0 ? (buf[0] & 254) : (buf[0] | 1));             //a0 

            buf[1] = (byte)((arr[4] & 1) == 0 ? (buf[1] & 127) : (buf[1] | 128));           //b7 
            buf[1] = (byte)((arr[5] & 4) == 0 ? (buf[1] & 191) : (buf[1] | 64));            //b6 
            buf[1] = (byte)((arr[5] & 2) == 0 ? (buf[1] & 223) : (buf[1] | 32));            //b5 
            buf[1] = (byte)((arr[5] & 1) == 0 ? (buf[1] & 239) : (buf[1] | 16));            //b4 
            buf[1] = (byte)((arr[6] & 2) == 0 ? (buf[1] & 247) : (buf[1] | 8));             //b3 
            buf[1] = (byte)((arr[6] & 1) == 0 ? (buf[1] & 251) : (buf[1] | 4));             //b2 
            buf[1] = (byte)((arr[7] & 1) == 0 ? (buf[1] & 253) : (buf[1] | 2));             //b1 
            buf[1] = (byte)((arr[8] & 4) == 0 ? (buf[1] & 254) : (buf[1] | 1));             //b0 

            buf[2] = (byte)((arr[8] & 2) == 0 ? (buf[2] & 127) : (buf[2] | 128));           //c7 
            buf[2] = (byte)((arr[8] & 1) == 0 ? (buf[2] & 191) : (buf[2] | 64));            //c6 
            buf[2] = (byte)((arr[9] & 2) == 0 ? (buf[2] & 223) : (buf[2] | 32));            //c5 
            buf[2] = (byte)((arr[9] & 1) == 0 ? (buf[2] & 239) : (buf[2] | 16));            //c4 
            buf[2] = (byte)((arr[10] & 1) == 0 ? (buf[2] & 247) : (buf[2] | 8));            //c3 
            buf[2] = (byte)((arr[11] & 4) == 0 ? (buf[2] & 251) : (buf[2] | 4));            //c2 
            buf[2] = (byte)((arr[11] & 2) == 0 ? (buf[2] & 253) : (buf[2] | 2));            //c1 
            buf[2] = (byte)((arr[11] & 1) == 0 ? (buf[2] & 254) : (buf[2] | 1));            //c0 

            return buf;
        }

        /// <summary> 
        /// 执行信息提取操作 
        /// </summary> 
        /// <returns>执行成功返回true，失败返回false</returns> 
        public bool ExecuteDecrypt()
        {
            int infoLen = GetInfoLength();
            if (infoLen == -1)
            {
                return false;
            }
            txt_SaveStream = new FileStream(txt_SavePath, FileMode.Create, FileAccess.Write);
            //置文件流位置 
            cam_Stream.Position = 66;
            txt_SaveStream.Position = 0;
            //计算循环次数 
            int cycle = (infoLen % 3 == 0) ? (infoLen / 3) : (infoLen / 3 + 1);
            //按12字节一组每次提取3个字节的隐藏信息并写入文件 
            for (int i = 0; i < cycle; i++)
            {
                byte[] contentBlock = new byte[12];
                cam_Stream.Read(contentBlock, 0, contentBlock.Length);
                byte[] contentBitArray = ExtractHidingBits(contentBlock);
                txt_SaveStream.Write(contentBitArray, 0, contentBitArray.Length);
            }
            txt_SaveStream.Flush();
            cam_Stream.Close();
            txt_SaveStream.Close();
            return true;
        }
    }
}
