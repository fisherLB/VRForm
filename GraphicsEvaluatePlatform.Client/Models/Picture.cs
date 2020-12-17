using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Client.Models
{
    [Serializable]
    public class Picture 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 数据标识 
        /// </summary>
        public bool DataSign
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime DataTime
        {
            get;
            set;
        }

        /// <summary>
        /// 图像名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProId
        {
            get;
            set;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        /// 
        public string ProName
        {
            get;
            set;
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        /// 
        public string Path
        {
            get;
            set;
        }

        /// <summary>
        /// 大小
        /// </summary>
        /// 
        public System.Drawing.Size Size
        {
            get;
            set;
        }

        /// <summary>
        ///文件格式
        /// </summary>
        /// 
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// 水印
        /// </summary>
        /// 
        public string Water
        {
            get;
            set;
        }

        /// <summary>
        /// 分辨率
        /// </summary>
        /// 

        public float Horizontal { get; set; }
        public float Vertical { get; set; }
    
        /// <summary>
        /// 高度
        /// </summary>
        /// 
        public decimal Height
        {
            get;
            set;
        }

        /// <summary>
        /// 宽度
        /// </summary>
        /// 
        public decimal Width
        {
            get;
            set;
        }      

        /// <summary>
        /// 偏斜角
        /// </summary>
        /// 
        public decimal Skew
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        /// 
        public string Remarks
        {
            get;
            set;
        }

    }

}
