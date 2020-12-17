/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Model
 * 项目描述: 
 * 类 名 称: ClientDataModel
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Model
 * 文件名称: ClientDataModel
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/8 18:27:20
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Model
{
    public class ClientDataModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DataTable Data { get; set; }
    }

}
