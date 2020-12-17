/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Model
 * 项目描述: 图像测评系统
 * 类 名 称: ChangeUserGroupViewModel
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: 刘桂林
 * 命名空间: GraphicsEvaluatePlatform.Model
 * 文件名称: ChangeUserGroupViewModel
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/3 8:36:45
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Model
{
    public class ChangeUserGroupViewModel
    {
        public string[] added { get; set; }
        public string[] deleted { get; set; }
        public string groupID { get; set; }
    }
}
