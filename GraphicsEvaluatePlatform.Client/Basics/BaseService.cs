/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Client.Basics
 * 项目描述: 
 * 类 名 称: BaseService
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Client.Basics
 * 文件名称: BaseService
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/18 15:58:15
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Client.Basics
{
     public class BaseService
    {
        public static Dictionary<string, object> USERINFODIC = new Dictionary<string, object>();
        public static string USERID = "";  //存放用户ID
        public static string USERNAME = ""; //存放用户名
        public static string USERACCOUNTNAME = ""; //存放登录账号名
        public static object USERINFO = new object(); //存放用户登录信息
        public static string IPADDRESS = "";  //存放本机IP
        public static string UNITID = "";  //机构ID 
        public static string UNITFULLNAME = "";
        public static string USEVERSION = "";

        public static string CLIENTTYPE = "";//网络版还是单机版(1--网络版，stand-alone 0 --单机版)
        public static string GetIPAddress()
        {
            System.Net.IPHostEntry myEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            string ipAddress = myEntry.AddressList[0].ToString();
            return ipAddress;
        }
    }
}
