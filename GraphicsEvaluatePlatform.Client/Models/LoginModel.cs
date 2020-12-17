/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Client.Models
 * 项目描述: 
 * 类 名 称: LoginModel
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Client.Models
 * 文件名称: LoginModel
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/18 9:54:44
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Client.Models
{
    [Serializable]
    class LoginModel
    {
        public string UserName { get; set; }
        public string Password  { get; set; }

        public bool  IsRemember{ get; set; }
    }
}
