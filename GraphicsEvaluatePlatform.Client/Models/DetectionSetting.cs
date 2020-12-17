using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Client.Models
{
    [Serializable]
    public class DetectionSetting
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid id
        {
            get;
            set;
        }

        /// <summary>
        /// 数据标识
        /// </summary>
        public bool datasign
        {
            get;
            set;
        }
        
        /// <summary>
        /// 数据时间
        /// </summary>
        public DateTime  datatime
        {
            get;
            set;
        }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// 项目id
        /// </summary>
        public Guid projectid
        {
            get;
            set;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectname
        {
            get;
            set;
        }

        /// <summary>
        /// 命名规则
        /// </summary>
        public string NameRule
        {
            get;
            set;
        }

        /// <summary>
        /// 图片大小最小值
        /// </summary>
        public decimal SizeMin
        {
            get;
            set;
        }

        /// <summary>
        /// 图片大小最大值
        /// </summary>
        public decimal SizeMax
        {
            get;
            set;
        }

        /// <summary>
        /// 分辨率最小值
        /// </summary>
        /// 
        public decimal ResolutionMin
        {
            get;
            set;
        }

        /// <summary>
        /// 分辨率最大值
        /// </summary>
        public decimal ResolutionMax
        {
            get;
            set;
        }

        /// <summary>
        /// 纠偏最小值
        /// </summary>
        public decimal RectifyMin
        {
            get;
            set;
        }

        /// <summary>
        /// 纠偏最大值
        /// </summary>
        public decimal RectifyMax
        {
            get;
            set;
        }

        /// <summary>
        /// 隐式水印
        /// </summary>
        public string ImplicitWatermarks
        {
            get;
            set;
        }

        /// <summary>
        /// 显示水印
        /// </summary>
        public string DisplayWatermarks
        {
            get;
            set;
        }

        /// <summary>
        /// 图片格式
        /// </summary>
        public string PictureType
        {
            get;
            set;
        }

        /// <summary>
        /// 检测通过本地保存路径
        /// </summary>
        public string FitPath
        {
            get;
            set;
        }

        /// <summary>
        /// 检测不通过本地保存路径
        /// </summary>
        public string UnFitPath
        {
            get;
            set;
        }


        /// <summary>
        /// 是否设置文件命名规则
        /// </summary>
        public bool Is_FileNamingRules
        {
            get;
            set;
        }

        /// <summary>
        /// 是否设置图像格式
        /// </summary>
        public bool Is_PictureFormat
        {
            get;
            set;
        }

        /// <summary>
        /// 是否设置图像大小范围
        /// </summary>
        public bool Is_ImageSizeRange
        {
            get;
            set;
        }

        /// <summary>
        /// 是否设置分辨率范围
        /// </summary>
        public bool Is_ResolutionRange
        {
            get;
            set;
        }

        /// <summary>
        /// 是否添加水印
        /// </summary>
        public bool Is_AddingWatermark
        {
            get;
            set;
        }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks
        {
            get;
            set;
        }

    }
}
