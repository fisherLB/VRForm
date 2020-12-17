/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2017. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Service
 * 项目描述: 
 * 类 名 称: ServiceBase
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: 覃明健
 * 命名空间: GraphicsEvaluatePlatform.Service
 * 文件名称: ServiceBase
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/3 17:15:38
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Service
{
    public static class ServiceBase
    {
        public static readonly string USERINFO = "UserInfo";
        public static readonly string USERID = "UserID";
        public static readonly string USERNAME = "UserName";
        public static readonly string USERPASSWORD = "UserPassword";
        public static readonly string USERFULLNAME = "UserFullName";
        public static readonly string USERTYPE = "UserType";

        public static string FILTER_PWD = "filter_pwd";//用户过滤密码
        public static string FILTER_USERNAME = "filter_username";//过滤器中用户名
        public static string CHECK_TYPE = "check_Type";//检查方式
        public static string URLVALIDATED = "UrlValidated";  //点击菜单，url是否身份验证已通过、上级授权通过
        

        public static Dictionary<string, object> dicUnitList = new Dictionary<string, object>();


        public static readonly string UNITINFO = "UnitInfo";
        public static readonly string UNITID = "UnitID";
        public static readonly string UNITNAME = "UnitName";
        public static readonly string UNITFULLNAME = "UnitFullName";
        public static readonly string UNITFULLPATH = "UnitFullPath";
        public static readonly string PARENTUNITNAME = "ParentUnitNameALL";

        public static readonly string MENULIST = "MenuList";  //页面公共头部菜单列表

        public static readonly string USER_PERMISSION = "User_Permission";  //用户功能权限

        public static readonly string UNITCOMBOX_LIST = "UnitCombox_List";  //获取机构下拉列表
      

        public static object GetInfo(string type)

        {
            return HttpContext.Current.Session[type];
        }

        public static void SetInfo(string type, object value)
        {
            HttpContext.Current.Session[type] = value;
        }

        public static string GetIPAddress()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }

      


        /// <summary>
        /// 存取单位下拉列表数据
        /// </summary>
        public static void InitMemoryUnitList()
        {
            DataSet dsug = new DataSet();

            string sql = "select * from t_Units order by u_DataTime asc";
            dsug = DataBll.Query(sql);
            foreach (DataRow dr in DataTrim.DataTableTrim(dsug.Tables[0]).Rows)
            {
                dicUnitList.Add(dr["u_Id"].ToString(), dr);
            }
        }

     

     
    }
}
