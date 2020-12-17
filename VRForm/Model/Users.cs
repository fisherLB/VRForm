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
namespace VRForm.Mdel
{
    [Serializable]
    public class Users
    {
        /// <summary>
        /// 用户记录ID
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string Password
        {
            get;
            set;
        }
        
        public DateTime CreateTime
        {
            get;set;
        }

    }
}

