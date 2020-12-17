/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Client.Models
 * 项目描述: 
 * 类 名 称: ClientDataModel
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Client.Models
 * 文件名称: ClientDataModel
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/21 9:37:23
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Client.Models
{
    public class ClientDataModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DataTable Data { get; set; }
    }

}
