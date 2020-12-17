/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2017. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Model
 * 项目描述: 
 * 类 名 称: Units
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: WorkServer
 * 命名空间: GraphicsEvaluatePlatform.Model
 * 文件名称: Units
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/5/16 12:58:09
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Model
{
    [Serializable]
    public class Units
    {
        /// <summary>
        /// 全宗记录ID
        /// </summary>
        [DisplayName("机构记录ID")]
        public int u_Id { get; set; }

        /// <summary>
        /// 全宗简称
        /// </summary>
        [DisplayName("机构名称")]
        public string u_Name { get; set; }

        /// <summary>
        /// 系统分配时
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime? u_DataTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string u_Remarks { get; set; }

        /// <summary>
        /// Contactor
        /// </summary>
        [DisplayName("负责人")]
        public string u_Contactor { get; set; }

        /// <summary>
        /// Telephone
        /// </summary>
        [DisplayName("联系电话")]
        public string u_Telephone { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        [DisplayName("Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// EMail
        /// </summary>
        [DisplayName("EMail")]
        public string u_EMail { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        [DisplayName("Address")]
        public string u_Address { get; set; }

        /// <summary>
        /// IsEnable
        /// </summary>
        [DisplayName("IsEnable")]
        public bool? u_IsEnable { get; set; }

        /// <summary>
        /// ParentID
        /// </summary>
        [DisplayName("ParentID")]
        public int? u_ParentID { get; set; }

        /// <summary>
        /// FullPath
        /// </summary>
        [DisplayName("FullPath")]
        public string u_FullPath { get; set; }

        /// <summary>
        /// Depth
        /// </summary>
        [DisplayName("Depth")]
        public int? u_Depth { get; set; }

    }
}
