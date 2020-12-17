/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Client.Services
 * 项目描述: 
 * 类 名 称: OperationLogService
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Client.Services
 * 文件名称: OperationLogService
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/18 16:03:08
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Client.Services
{
    public static class OperationLogService
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
