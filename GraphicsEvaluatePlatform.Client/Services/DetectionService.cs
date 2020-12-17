using GraphicsEvaluatePlatform.Client.Models;
using GraphicsEvaluatePlatform.Client.Repository;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common;
using GraphicsEvaluatePlatform.Infrastructure.Common.ImageHelper;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GraphicsEvaluatePlatform.Client.Services
{
    class DetectionService : IDetectionService
    {
        /// <summary>
        /// 检测指定的图像
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="deType"></param>
        /// <returns></returns>
        public OperationResult DetectionImage(Guid projectId, string pictureIds)
        {
            OperationResult ret = new OperationResult(OperationResultType.Error);
            //  元步骤，对单张进行图像检测：
            //  viewmodel 将 项目id 图像id（可由多个guid 以','拼接的字符串）、检测类的model以参数形式传入；
            //  项目id用于取检测配置项；
            //  图像id用于取图像信息；
            //  model 是初始化的检测类；
            //  将图像信息与检测配置项进行对比；
            //  对比结果用于更新model；
            // 将更新结果作为返回值 返回；
            //根据项目id取图像文件夹路径path
            //根据path分批次取图像picturelist
            //根据项目id取图像文件夹路径path
            //根据path分批次取图像datatable
            //for循环检测每一行row
            string[] idArr = pictureIds.Split(',');
            if (idArr.Length == 0)
            {
                Logger.GetLogger("DetectionService").Error("DetectionImage 参数pictureId 不正确发生异常");
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            PictureService picservice = new PictureService();
            int pagerow = 1000;//每页数量
            int pageindex = 0;//当前页码
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("projectid", projectId);
            dic.Add("ids", pictureIds);
            DataSet dsp = (DataSet)picservice.GetList(pagerow, pageindex, dic).AppendData;
            if (dsp != null)
            {
                DetectionSettingService dsservice = new DetectionSettingService();
                DetectionSetting detectionsetting = new DetectionSetting();
                detectionsetting = dsservice.GetDetail(projectId);
                //对每一行的图像数据进行检测，判断是否合格
                foreach (DataRow dr in dsp.Tables[0].Rows)
                {
                    Picture picture = new Picture();
                    Detection detection = new Detection()
                    {
                        Id = Guid.NewGuid(),
                        DataSign = true,
                        DataTime = DateTime.Now,
                        IsAuto = true,
                        IsFitHeight = true,
                        IsFitResolution = true,
                        IsFitName = true,
                        IsFitSize = true,
                        IsFitSkew = true,
                        IsFitType = true,
                        IsFitWater = true,
                        IsFitWidth = true,
                        IsPass = true,
                        PicSoruce = picture,
                        Remarks = "",
                    };
                    ret = DetectionUnit(picture, detection, detectionsetting);
                    if (ret.ResultType == OperationResultType.Success)
                    {
                        detection = (Detection)ret.AppendData;
                        Add(detection);
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// 检测项目中的图像
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="deType"></param>
        /// <returns></returns>
        public OperationResult DetectionProject(Guid projectId)
        {
            //  元步骤，对单张进行图像检测：
            //  viewmodel 将 项目id 图像id（可由多个guid 以','拼接的字符串）、检测类的model以参数形式传入；
            //  项目id用于取检测配置项；
            //  图像id用于取图像信息；
            //  model 是初始化的检测类；
            //  将图像信息与检测配置项进行对比；
            //  对比结果用于更新model；
            // 将更新结果作为返回值 返回；
            //根据项目id取图像文件夹路径path
            //根据path分批次取图像picturelist
            //根据项目id取图像文件夹路径path
            //根据path分批次取图像datatable
            //for循环检测每一行row
            OperationResult ret = new OperationResult(OperationResultType.Success);
            PictureService picservice = new PictureService();
            int pagerow = 1000;//每页数量
            int pageindex = 0;//当前页码
            do//以指定行数，循环取图像列表数据，直到取完与projectid相关的所有图像
            {
                Dictionary<string, object> dic = new Dictionary<string, object>
                {
                    { "projectid", projectId }
                };
                DataSet dsp = (DataSet)picservice.GetList(pagerow, pageindex, dic).AppendData;
                if (dsp != null)
                {
                    //对每一行的图像数据进行检测，判断是否合格
                    foreach (DataRow dr in dsp.Tables[0].Rows)
                    {
                        string result = "1";
                        Picture picture = new Picture();
                        DetectionSetting detectionsetting = new DetectionSetting();
                        Detection detection = new Detection()
                        {
                            Id = Guid.NewGuid(),
                            DataSign = true,
                            DataTime = DateTime.Now,
                            IsAuto = true,
                            IsFitHeight = true,
                            IsFitResolution = true,
                            IsFitName = true,
                            IsFitSize = true,
                            IsFitSkew = true,
                            IsFitType = true,
                            IsFitWater = true,
                            IsFitWidth = true,
                            IsPass = true,
                            PicSoruce = picture,
                            Remarks = "",
                        };
                        picture.Name = dr["name"].ToString();//名称
                        picture.Type = dr["type"].ToString();//格式
                        //picture.Size = int.Parse(dr["size"].ToString());//大小
                        picture.Skew = decimal.Parse(dr["skewness"].ToString());//偏斜角
                        //picture.Resolution = int.Parse(dr["revolution"].ToString());//分辨率
                        picture.Height = decimal.Parse(dr["height"].ToString());//高度
                        picture.Width = decimal.Parse(dr["width"].ToString());//宽度 
                        picture.Water = dr["water"].ToString();//水印 
                        //根据项目id取检测配置DetectSetting
                        DetectionSettingService dsservice = new DetectionSettingService();
                        detectionsetting = dsservice.GetDetail(projectId);
                        ret = DetectionUnit(picture, detection, detectionsetting);
                        if (ret.ResultType == OperationResultType.Success)
                        {
                            detection = (Detection)ret.AppendData;
                            Add(detection);
                        }
                        else
                        {
                            ret.Message = "检测时出错中止！";
                            return ret;
                        }
                    }
                }
                else
                {
                    break;
                }
            } while (true);
            throw new NotImplementedException();
        }
        /// <summary>
        /// 检测单元
        /// </summary>
        /// <param name="picture">图像对象</param>
        /// <param name="deset">检测配置</param>
        /// <returns></returns>
        private OperationResult DetectionUnit(Picture picture, Detection detection, DetectionSetting deset)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
           // if (picture.Size > deset.SizeMax || picture.Size < deset.SizeMin)
            {
                detection.IsFitSize = false;
                detection.IsPass = false;
            }
            //if (picture.Resolution > deset.ResolutionMax || picture.Size < deset.ResolutionMin)
            {
                detection.IsFitResolution = false;
                detection.IsPass = false;
            }
            if (picture.Skew > deset.RectifyMax || picture.Skew < deset.RectifyMin)
            {
                detection.IsFitSkew = false;
                detection.IsPass = false;
            }
            if (picture.Water == "")
            {
                detection.IsFitWater = false;
                detection.IsPass = false;
            }
            if (!NameRule(picture.Name))
            {
                detection.IsFitSkew = false;
                detection.IsPass = false;
            }
            ret.AppendData = detection;
            return ret;
        }

        public OperationResult Add(Detection model)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", model.Id);
            dic.Add("DataSign", model.DataSign);
            dic.Add("DataTime", model.DataTime);
            dic.Add("IsFitType", model.IsFitType);
            dic.Add("IsFitWater", model.IsFitWater);
            dic.Add("IsFitResolution", model.IsFitResolution);
            dic.Add("IsFitSize", model.IsFitSize);
            dic.Add("IsFitHeight", model.IsFitHeight);
            dic.Add("IsFitWidth", model.IsFitWidth);
            dic.Add("IsFitName", model.IsFitName);
            dic.Add("Remarks", model.Remarks);
            dic.Add("IsPass", model.IsPass);
            dic.Add("IsFitSkew", model.IsFitSkew);
            var obj = DataBll.Add(dic, "t_detection");
            if (obj != null)
            {
                ret.Message = "添加检测成功。";
            }
            else
            {
                ret.ResultType = OperationResultType.Error;
                ret.Message = "添加检测失败！";
            }
            return ret;
        }

        public OperationResult Del(string id)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> EntityToDictionary<T>(T obj)
        {
            throw new NotImplementedException();
        }

        public OperationResult GetDetail(string id)
        {
            throw new NotImplementedException();
        }

        public List<Detection> GetList()
        {
            throw new NotImplementedException();
        }

        public OperationResult GetList(int pageNum, int currentlyPage, Dictionary<string, object> dic)
        {
            throw new NotImplementedException();
        }

        public OperationResult Search(Detection model)
        {
            throw new NotImplementedException();
        }

        public OperationResult Update(Detection model)
        {
            throw new NotImplementedException();
        }

        public string ValueOf(object obj)
        {
            return (obj == null) ? "null" : obj.ToString(); ;
        }

        public IDictionary<string, object> GetDictionary(object source)
        {
            if (source == null)
            {
                return new Dictionary<string, object>();
            }
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(source);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            for (int i = 0; i < properties.Count; i++)
            {
                dictionary.Add(properties[i].Name, properties[i].GetValue(source).ToString());
            }
            return dictionary;
        }
        public bool NameRule(string name)
        {
            return true;
        }
        public OperationResult GetCount(Guid projectId)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            int count = DataBll.GetCount("t_picture", "projectId='" + projectId + "'");
            if (count >= 0)
            {
                ret.AppendData = count;
            }
            else
            {
                ret.ResultType = OperationResultType.Error;
                ret.Message = "取总数失败！";
            }
            return ret;
        }
        public OperationResult GetPictureInfo(string path)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            Bitmap bitmap = new Bitmap(path);
            image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(path));
            PropertyItem[] propertyItem = bitmap.PropertyItems;
            Image img = Bitmap.FromFile(path);
            ImageProcessing sk = new ImageProcessing(bitmap);
            string name = "";
            decimal skew = (decimal)sk.GetSkewAngle();
            float resoH = bitmap.HorizontalResolution;
            float resoV = bitmap.VerticalResolution;
            Size size = bitmap.Size;
            decimal height = bitmap.Height;
            decimal width = bitmap.Width;
            decimal bright = 0;
            decimal constract = 0;
            string water = "";
            string str_pathtxt = Application.StartupPath + "\\W" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            if (!File.Exists(str_pathtxt))
            {
                FileStream fs1 = new FileStream(str_pathtxt, FileMode.Create, FileAccess.Write);//创建写入文件 
                fs1.Close();
            }
            LSB_Decrypt decrypt = new LSB_Decrypt(path, str_pathtxt);
            if (decrypt.ExecuteDecrypt())
            {
                StreamReader sr = new StreamReader(str_pathtxt, Encoding.UTF8);
                water = sr.ReadLine().ToString();
                sr.Close();
                File.Delete(str_pathtxt);//删除txt
            }
            Picture picture = new Picture()
            {
                Id = Guid.Empty,
                DataTime = DateTime.Now,
                DataSign = true,
                Name = name,
                Height = height,
                Width = width,
                Size = size,
                Vertical = resoV,
                Horizontal = resoH,
            };
            ret.AppendData = picture;
            return ret;
        }
    }
}
