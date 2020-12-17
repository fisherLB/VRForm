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
 * 类 名 称: DataUserGroupPermissionTree
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: Administrator
 * 命名空间: GraphicsEvaluatePlatform.Model
 * 文件名称: DataUserGroupPermissionTree
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/4 9:37:03
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Model
{
    public class DataUserGroupPermissionTree
    {


        public string id { get; set; }
        public string text { get; set; }

        public string Ugc_isSonCate { get; set; }

        public string Cate_full_name { get; set; }
        public string Ugc_isChangeFiles { get; set; }

        public string Ugc_isViewFiles { get; set; }

        public string parentid { get; set; }
        public string codetype { get; set; }
        public string state { get; set; }
        public List<DataUserGroupPermissionTree> children { get; set; }

    }
}
