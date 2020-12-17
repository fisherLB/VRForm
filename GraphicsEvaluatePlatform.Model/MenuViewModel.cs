/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2017. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Model
 * 项目描述: 
 * 类 名 称: MenuViewModel
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Model
 * 文件名称: MenuViewModel
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/4/25 10:05:37
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Model
{
    [Serializable]
    public class MenuViewModel
    {
        public string menuid { get; set; }
        public string icon { get; set; }
        public string menuname { get; set; }
        public int? menuorder { get; set; }
        public int? index { get; set; }
        public string Action { get; set; }
        public List<MenusViewModel> children { get; set; }
    }
    /// <summary>
    /// 子菜单
    /// </summary>
    [Serializable]
    public class MenusViewModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public object attributes { get; set; }
        public int? menuorder { get; set; }
        public string Action { get; set; }
        public MenuViewModel Menu { get; set; }
        public List<MenusViewModel> children { get; set; }
    }
}
