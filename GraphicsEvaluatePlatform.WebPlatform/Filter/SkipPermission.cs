using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraphicsEvaluatePlatform.WebPlatform.Filter
{
    /// <summary>
    /// 跳过权限验证，但要登录验证
    /// </summary>
    public class SkipPermissionAttribute : Attribute
    {
    }
}