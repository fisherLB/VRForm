using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Client.Models
{
    public class Detection
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
        /// 状态 0  单张 1 批量
        /// </summary>
        /// 
        public bool IsAuto
        {
            get;
            set;
        }

        /// <summary>
        /// 通过与否 0：否；1：是；
        /// </summary>
        /// 
        public bool IsPass
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

        /// <summary>
        /// 图像名称
        /// </summary>
        public bool IsFitName
        {
            get;
            set;
        }

        /// <summary>
        /// 大小是否合格
        /// </summary>
        /// 
        public bool IsFitSize
        {
            get;
            set;
        }

        /// <summary>
        ///文件格式
        /// </summary>
        public bool IsFitType
        {
            get;
            set;
        }

        /// <summary>
        /// 偏斜角是否合格
        /// </summary>
        public bool IsFitSkew
        {
            get;
            set;
        }

        /// <summary>
        /// 分辨率是否合格
        /// </summary>
        /// 
        public bool IsFitResolution
        {
            get;
            set;
        }

        /// <summary>
        /// 高度是否合格
        /// </summary>
        /// 
        public bool IsFitHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 宽度是否合格
        /// </summary>
        /// 
        public bool IsFitWidth
        {
            get;
            set;
        }

        /// <summary>
        /// 水印是否合格
        /// </summary>
        public bool IsFitWater
        {
            get;
            set;
        }

        public Picture PicSoruce { get; set; }
    }
}
