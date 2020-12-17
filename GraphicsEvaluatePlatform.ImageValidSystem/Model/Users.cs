/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.ImageValidSystem.Mdel
 * 项目描述: 
 * 类 名 称: Users
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.ImageValidSystem.Mdel
 * 文件名称: Users
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/9 9:30:23
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.ImageValidSystem.Mdel
{
    [Serializable]
    public class Users
    {
        /// <summary>
        /// 用户记录ID
        /// </summary>
        public string Us_id
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string UnitID
        {
            get;
            set;
        }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string Unit_name
        {
            get;
            set;
        }
        /// <summary>
        /// 用户帐号
        /// </summary>
        public string Us_account
        {
            get;
            set;
        }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Us_name
        {
            get;
            set;
        }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string Us_Password
        {
            get;
            set;
        }
        /// <summary>
        /// 联系手机
        /// </summary>
        public string Us_telephone
        {
            get;
            set;
        }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Us_email
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Us_remark
        {
            get;
            set;
        }
        /// <summary>
        /// 帐号情况
        /// </summary>
        public string Us_status
        {
            get;
            set;
        }
        /// <summary>
        /// 帐号类型
        /// </summary>
        public string Us_type
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string Us_create_time
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Us_create_name
        {
            get;
            set;
        }
        /// <summary>
        /// 最后使用的功能模块名
        /// </summary>
        public string Func_code
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoginFaildCount
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string BanExpire
        {
            get;
            set;
        }
        /// <summary>
        /// 用户临时表的标识
        /// </summary>
        public string TableIdentifier
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Us_Gender
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Us_Position
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Us_Birthday
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Us_ApproveFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Us_CityManagerFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Us_QZManagerFlag
        {
            get;
            set;
        }

    }
}

