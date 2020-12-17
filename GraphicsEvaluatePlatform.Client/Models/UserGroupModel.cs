using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEvaluatePlatform.Client.Models
{
    public class UserGroupModel: NotificationObject
    {
        /// <summary>
        /// 用户组ID
        /// </summary>
        public string Ug_id { get; set; }
        /// <summary>
        /// 用户组名称
        /// </summary>
        public string Ug_Name { get; set; }
        /// <summary>
        /// 用户组类型
        /// </summary>
        public string Ug_type { get; set; }
        /// <summary>
        /// 用户组创建时间
        /// </summary>
        public string Ug_create_time { get; set; }
        /// <summary>
        /// 用户组创建人
        /// </summary>
        public string Ug_create_name { get; set; }
        /// <summary>
        /// 用户组备注
        /// </summary>
        public string Ug_Remark { get; set; }
        /// <summary>
        /// 用户组是否启用
        /// </summary>
        public string Ug_IsEnable { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public string UnitID { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string Unit_Name { get; set; }

    }
}
