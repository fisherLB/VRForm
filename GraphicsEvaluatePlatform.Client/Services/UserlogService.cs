/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Client.Basics;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Client.Services
 * 项目描述: 
 * 类 名 称: UserlogService
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Client.Services
 * 文件名称: UserlogService
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/18 16:00:55
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Client.Services
{
    public static class UserlogService
    {
        public static void InsertLog(string account, string name, string ip, DateTime time, string function, string desc, string type)
        {
            Dictionary<string, object> dicValue = new Dictionary<string, object>();
            dicValue.Add("Ul_id", Guid.NewGuid().ToString());
            dicValue.Add("Us_account", account);
            dicValue.Add("Us_name", name);
            dicValue.Add("Ul_ip", ip);
            dicValue.Add("Ul_time", time.ToString("yyyy-MM-dd HH:mm:ss"));
            dicValue.Add("Ul_function", function);
            dicValue.Add("Ul_descript", desc);
            dicValue.Add("Ul_UnitID", BaseService.UNITID);
            dicValue.Add("Ul_UnitName", BaseService.UNITFULLNAME);
            DataBll.Add(dicValue, "t_UserLog");
        }
    }
}
