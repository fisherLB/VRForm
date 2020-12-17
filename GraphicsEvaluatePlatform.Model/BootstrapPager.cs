/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Model
 * 项目描述: 
 * 类 名 称: BootstrapPager
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: 覃明健
 * 命名空间: GraphicsEvaluatePlatform.Model
 * 文件名称: BootstrapPager
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/3 10:41:57
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Model
{

    /// <summary>
    /// bootstrap分页参数
    /// </summary>
    public class BootstrapPager
    {
        /// <summary>
        /// 每页行数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 过滤条件
        /// </summary>
        public string filter { get; set; }
    }
}
