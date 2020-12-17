/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Infrastructure.Common.Strings
 * 项目描述: 
 * 类 名 称: DataTrim
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Infrastructure.Common.Strings
 * 文件名称: DataTrim
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/7 14:40:46
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Infrastructure.Common.Strings
{
    /// <summary>
    /// 去掉DataTable数据末尾自动生成的空格
    /// </summary>
    public static class DataTrim
    {
        public static DataTable DataTableTrim(DataTable dt)
        {
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                for (int b = 0; b < dt.Columns.Count; b++)
                {
                    string temp = dt.Rows[k][b].ToString().TrimEnd();
                    dt.Rows[k][b] = temp;
                }
            }
            return dt;
        }
    }
}
