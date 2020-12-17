using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEvaluatePlatform.ImageValidSystem.Mdel
{
    [Serializable]
    public class Projects
    {
        /// <summary>
        /// 用户记录ID
        /// </summary>
        public string P_Id
        {
            get;
            set;
        }
        public string P_DataSign
        {
            get;
            set;
        }
        public string P_DataTime
        {
            get;
            set;
        }
        public string P_UserId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string P_UnitId
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
        /// 用户帐号
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
        public string P_DataSize
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string P_Remarks
        {
            get;
            set;
        }

    }
}
