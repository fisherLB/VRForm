/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2017. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Service
 * 项目描述: 
 * 类 名 称: OperationLogService
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: 孙宁远
 * 命名空间: GraphicsEvaluatePlatform.Service
 * 文件名称: OperationLogService
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/4/20 18:50:48
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Service
{
    public static class OperationLogService
    {
        public static void InsertLog(string UserID, string IP, DateTime time, string desc, string type)
        {
            Dictionary<string, object> dicValue = new Dictionary<string, object>();
            dicValue.Add("User_ID", UserID);
            dicValue.Add("Operating_IP", IP);
            dicValue.Add("Operating_DateTime", time.ToString("yyyy-MM-dd HH:mm:ss"));
            dicValue.Add("Operating_Desc", desc);
            dicValue.Add("Operating_Type", type);
            DataBll.Add(dicValue, "t_OperationLog");
        }
    }
}
