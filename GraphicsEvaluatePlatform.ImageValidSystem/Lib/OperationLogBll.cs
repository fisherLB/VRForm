/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.ImageValidSystem.Lib
 * 项目描述: 
 * 类 名 称: OperationLogBll
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.ImageValidSystem.Lib
 * 文件名称: OperationLogBll
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/14 12:00:35
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.ImageValidSystem.Lib
{
    public static class OperationLogBll
    {
        public static void InsertLog(string UserID, string IP, DateTime time, string desc, string type)
        {
            Dictionary<string, object> dicValue = new Dictionary<string, object>();
            dicValue.Add("Log_ID", Guid.NewGuid().ToString());
            dicValue.Add("User_ID", UserID);
            dicValue.Add("Operating_IP", IP);
            dicValue.Add("Operating_DateTime", time.ToString("yyyy-MM-dd HH:mm:ss"));
            dicValue.Add("Operating_Desc", desc);
            dicValue.Add("Operating_Type", type);
            DataBll.Add(dicValue, "t_OperationLog");
        }
    }
}
