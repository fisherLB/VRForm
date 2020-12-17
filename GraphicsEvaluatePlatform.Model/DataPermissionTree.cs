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
 * 类 名 称: DataPermissionTree
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: 覃明健
 * 命名空间: GraphicsEvaluatePlatform.Model
 * 文件名称: DataPermissionTree
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/6/15 17:52:35
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Model
{
   public class DataPermissionTree
   {

      
            public string id { get; set; }
            public string text { get; set; }
            
            public string Uc_isSonCate { get; set; }
            
            public string Cate_full_name { get; set; }
           public string Uc_isChangeFiles { get; set; }

           public string Uc_isViewFiles { get; set; }

            public string ParentId { get; set; }
            public string codetype { get; set; }
            public string state { get; set; }
            public List<DataPermissionTree> children { get; set; }
       
    }
}
