/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.ImageValidSystem.Model
 * 项目描述: 
 * 类 名 称: LoginUser
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.ImageValidSystem.Model
 * 文件名称: LoginUser
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/12 10:28:29
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.ImageValidSystem.Model
{
    [Serializable]
    public class LoginUser
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}
