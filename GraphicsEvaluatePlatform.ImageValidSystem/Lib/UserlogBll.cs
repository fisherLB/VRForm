/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.ImageValidSystem.Mdel;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.ImageValidSystem.Lib
 * 项目描述: 
 * 类 名 称: UserlogBll
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.ImageValidSystem.Lib
 * 文件名称: UserlogBll
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/14 11:50:12
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.ImageValidSystem.Lib
{
    public  static class UserlogBll
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

            dicValue.Add("Ul_UnitID", BaseClass.UNITID);
            dicValue.Add("Ul_UnitName", BaseClass.UNITFULLNAME);
            DataBll.Add(dicValue, "t_UserLog");
        }
    }
}
