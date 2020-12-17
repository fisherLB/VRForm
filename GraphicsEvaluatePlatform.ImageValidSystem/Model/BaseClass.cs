/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.ImageValidSystem.Mdel
 * 项目描述: 
 * 类 名 称: StaticBase
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.ImageValidSystem.Mdel
 * 文件名称: StaticBase
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/10 17:56:42
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.ImageValidSystem.Mdel
{
    /// <summary>
    /// 
    /// </summary>
     public static class BaseClass
    {
        public static Dictionary<string, object> USERINFODIC = new Dictionary<string, object>();
        public static  string USERID = "";  //存放用户ID
        public static  string USERNAME = ""; //存放用户名
        public static string USERACCOUNTNAME = ""; //存放登录账号名
        public static object USERINFO = new object(); //存放用户登录信息
        public static string IPADDRESS = "";  //存放本机IP
        public static string UNITID = "";  //机构ID 
        public static string UNITFULLNAME = "";
  


        public static string GetIPAddress()
        {
            System.Net.IPHostEntry myEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            string ipAddress = myEntry.AddressList[0].ToString();
            return ipAddress;
        }

    }
}
