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
 * 类 名 称: DataUserPermission
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Model
 * 文件名称: DataUserPermission
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/6/17 17:32:19
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Model
{
    public class DataUserPermissionModel
    {
        public string Us_id { get; set; }
        public string Cate_id { get; set; }
        
        public string Uc_isSonCate { get; set; }
        public string Uc_isChangeFiles { get; set; }
        public string Uc_isViewFiles { get; set; }

    }
}
