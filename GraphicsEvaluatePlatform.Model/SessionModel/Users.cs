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
 * 类 名 称: Users
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: WorkServer
 * 命名空间: GraphicsEvaluatePlatform.Model
 * 文件名称: Users
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/5/16 12:57:28
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Model
{
    [Serializable]
    public class Users
    {
        /// <summary>
        /// 用户记录ID
        /// </summary>
        [DisplayName("用户记录ID")]
        public Guid Us_id { get; set; }

        /// <summary>
        /// UnitID
        /// </summary>
        [DisplayName("UnitID")]
        public string  UnitID { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [DisplayName("单位名称")]
        public string Unit_name { get; set; }

        /// <summary>
        /// 用户帐号
        /// </summary>
        [DisplayName("用户帐号")]
        public string Us_account { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [DisplayName("用户姓名")]
        public string Us_name { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [DisplayName("用户密码")]
        public string Us_Password { get; set; }

        /// <summary>
        /// 联系手机
        /// </summary>
        [DisplayName("联系手机")]
        public string Us_telephone { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        [DisplayName("电子邮件")]
        public string Us_email { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string Us_remark { get; set; }

        /// <summary>
        /// 帐号情况
        /// </summary>
        [DisplayName("帐号情况")]
        public string Us_status { get; set; }

        /// <summary>
        /// 帐号类型
        /// </summary>
        [DisplayName("帐号类型")]
        public int? Us_type { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime? Us_create_time { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [DisplayName("创建人")]
        public string Us_create_name { get; set; }

        /// <summary>
        /// 最后使用的功能模块名
        /// </summary>
        [DisplayName("最后使用的功能模块名")]
        public string Func_code { get; set; }

        /// <summary>
        /// LoginFaildCount
        /// </summary>
        [DisplayName("LoginFaildCount")]
        public int? LoginFaildCount { get; set; }

        /// <summary>
        /// BanExpire
        /// </summary>
        [DisplayName("BanExpire")]
        public DateTime? BanExpire { get; set; }

        /// <summary>
        /// TableIdentifier
        /// </summary>
        [DisplayName("统计表ID")]
        public int? TableIdentifier { get; set; }

        /// <summary>
        /// Us_Gender
        /// </summary>
        [DisplayName("性别")]
        public string Us_Gender { get; set; }

        /// <summary>
        /// Us_Position
        /// </summary>
        [DisplayName("职务")]
        public string Us_Position { get; set; }

        /// <summary>
        /// Us_Birthday
        /// </summary>
        [DisplayName("生日")]
        public string Us_Birthday { get; set; }

        public string Us_ApproveFlag { get; set; }
        public string  Us_CityManagerFlag { get; set; }
        public string Us_QZManagerFlag{ get; set; }
}
}
