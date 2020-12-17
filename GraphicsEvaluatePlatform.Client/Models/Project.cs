using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Client.Models
{
    [Serializable]
    public class Project
    {
        public Guid P_Id
        {
            get;
            set;
        }
        public bool P_DataSign
        {
            get;
            set;
        }
        public DateTime P_DataTime
        {
            get;
            set;
        }
        public Guid P_UserId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid P_UnitId
        {
            get;
            set;
        }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string P_UnitName
        {
            get;
            set;
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string P_Name
        {
            get;
            set;
        }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string P_Region
        {
            get;
            set;
        }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string P_Contactor
        {
            get;
            set;
        }
        /// <summary>
        /// 联系手机
        /// </summary>
        /// 
        public int P_DataSize
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        /// 
        public string P_Remarks
        {
            get;
            set;
        }
    }
}
