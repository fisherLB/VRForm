using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEvaluatePlatform.Client.Models
{
    [Serializable]
    public class Process
    {
        /// <summary>
        /// 处理表主键
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
        /// 对比度变动数值
        /// </summary>
        /// 
        public decimal Contrast
        {
            get;
            set;
        }

        /// <summary>
        /// 亮度变动数值
        /// </summary>
        /// 
        public decimal Bright
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

        public Picture PicSource { get; set; }
        public Picture PicTarget { get; set; }


    }
}
