using GraphicsEvaluatePlatform.Client.Models;
using GraphicsEvaluatePlatform.Client.Repository;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common;
using GraphicsEvaluatePlatform.Infrastructure.Common.ImageHelper;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GraphicsEvaluatePlatform.Client.Services
{
    /// <summary>
    ///  图像处理服务类
    /// </summary>
    public class ProcessService : IProcessService
    {
        public OperationResult GetDetail(Guid guid)
        {

            throw new NotImplementedException();
        }
        public OperationResult Init(Guid picId)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            Process process = new Process()
            {
                Id = Guid.Empty,
                DataSign = true,
                DataTime = DateTime.Now,
                Bright = 0,
                Contrast = 0,
                IsAuto = true,
                IsPass = true,
                PicSource = null,
                PicTarget = null,
                Remarks = "",
            };
            PictureService pictureService = new PictureService();
            Picture picture = (Picture)pictureService.GetDetail(picId).AppendData;
            if (picture != null)
            {
                process.PicSource = picture;
                ret.AppendData = process;
            }
            else
            {
                ret.ResultType = OperationResultType.Error;
                ret.Message = "获取图像文件失败！";
            }
            return ret;
        }

        /// <summary>
        /// 图像处理
        /// </summary>
        /// <param name="proId">项目id</param>
        /// <param name="picId">图像id</param>
        /// <param name="isAuto">是否自动</param>
        /// <returns></returns>
        public OperationResult ProcessPicture(Guid proId, string picIds, bool isAuto)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            //取检测配置；
            DetectionSettingService detectionSettingService = new DetectionSettingService();
            DetectionSetting deset = detectionSettingService.GetDetail(proId);// new DetectionSetting();

            var idArr = picIds.Split(',');
            if (idArr.Length > 0)
            {
                foreach (var id in idArr)
                {
                    Process process = new Process();
                    // ret=Save(process);
                }
            }
            throw new NotImplementedException();
        }

        public OperationResult ProcessProject(Guid projectId, string isAuto)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 图像处理的元步骤：
        /// </summary>
        /// <param name="proId"></param>
        /// <param name="picId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public OperationResult Save(Guid proId, string picId, Process model)
        {
            // viewmodel 将项目id，图像id，处理类model 以参数形式传入
            // 用model 中图像相关的信息生成新图像类对象target 
            // 调用检测类的方法检测target 的数值是否合格
            // 将检测结果返回
            // 保存图像文件
            // 若成功则保存model进数据表
            // 保存model
            OperationResult ret = new OperationResult(OperationResultType.Success);
            //根据pictureid,取图像信息
            Picture picture = model.PicTarget;
            DetectionService detectionService = new DetectionService();
            ret = detectionService.DetectionProject(proId);
            if (ret.ResultType == OperationResultType.Success)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                var result = DataBll.Add(dic, "t_Detection");
                if (result != null)
                {
                    ret.Message = "图像处理完成。";
                }
                else
                {
                    ret.Message = "图像处理失败！";
                }
            }
            //用户修改后得到model：包括新旧图像，亮度，对比度变动数值。
            // 根据projectId 得检测配置setting,以此判断修改后图像是否合格。
            //根据将原图像复制到处理后的图像文件夹
            //将model保存
            ret.Message = "";
            ret.AppendData = model;
            return ret;
        }

        public OperationResult Save(Process model)
        {
            throw new NotImplementedException();
        }

        public OperationResult SetBright(Bitmap bitmap, int val)
        {

            OperationResult ret = new OperationResult(OperationResultType.Success);
            ImageProcessing sk = new ImageProcessing(bitmap);
            bitmap = sk.Brightness_Pic(bitmap, val);
            return ret;
        }
    

        public OperationResult SetConstrast(Bitmap bitmap, int val)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            ImageProcessing sk = new ImageProcessing(bitmap);
            bitmap = sk.Contrast_Pic(bitmap, val);
            return ret;
        }

        public OperationResult SetRotate(Bitmap bitmap, int val)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            ImageProcessing sk = new ImageProcessing(bitmap);
            bitmap = sk.RotateToImage(bitmap, -val);
            return ret;
        }

        public OperationResult SetSize(Bitmap bitmap, int width, int height)
        {
            throw new NotImplementedException();
        }

        public OperationResult SetWater(string imagePath, string content)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            //备份原始文件 
            // string backupFile = string.Format(@"{0}\{1}_bak.jpg", Path.GetDirectoryName(lblPath.Text.Trim()), Path.GetFileNameWithoutExtension(lblPath.Text));
            //File.Copy(lblPath.Text, backupFile, true);
            // 判断文件是否存在，不存在则创建，否则读取值显示到窗体

            string textPath = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            if (!File.Exists(textPath))
            {
                FileStream fs1 = new FileStream(textPath, FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1);
                sw.Close();
                fs1.Close();
            }
            else
            {
                FileStream fs = new FileStream(textPath, FileMode.Open, FileAccess.Write);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine(content);//开始写入值
                sr.Close();
                fs.Close();
            }
            //生成图像 
            LSB_Encrypt lsb = new LSB_Encrypt(imagePath, textPath);
            lsb.Exe_Encrypt();
            return ret;
        }



        public OperationResult Update(Process model)
        {
            throw new NotImplementedException();
        }
    }
}