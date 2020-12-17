/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Service
 * 项目描述: 
 * 类 名 称: StaticVariable
 * 说    明: 获取静态页面
 * 版 本 号: v1.0.0
 * 作    者: 刘桂林
 * 命名空间: GraphicsEvaluatePlatform.Service
 * 文件名称: StaticVariable
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/14 19:33:32
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Service
{
    public class StaticVariable
    {
        /// <summary>
        /// 获取权限弹出框
        /// </summary>
        /// <returns></returns>
        public static string GetPermissionAlert()
        {
            try
            {
                string filePath = "~/StaticResource/PermissionHtml/Permission.html";//相对路径
                string path = HttpContext.Current.Server.MapPath(filePath);//绝对路径
                string html = StringUtil.ReadHtmlFile(path);//获取文件值
                return html;
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}
