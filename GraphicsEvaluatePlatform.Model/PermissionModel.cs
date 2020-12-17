/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2017. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Model
 * 项目描述: 
 * 类 名 称: PermissionModel
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: 覃明健
 * 命名空间: GraphicsEvaluatePlatform.Model
 * 文件名称: PermissionModel
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/5/4 14:37:28
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Model
{
    public class PermissionModel
    {
        public string Func_id { get; set; }
        public string Us_id { get; set; }
        public string Full_url { get; set; }
        public int Func_grade { get; set; }

    }
}
