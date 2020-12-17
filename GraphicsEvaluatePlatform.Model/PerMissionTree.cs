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
 * 类 名 称: PerMissionTree
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Model
 * 文件名称: PerMissionTree
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/5/3 9:44:36
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Model
{
    public class PermissionTree
    {

     
            string _id;
            string _text;
            object _children;
            string _iconCls;
            bool _check = true;
            string _permissionsselect;
            string _remark;
           
            /// <summary>
            /// 数据
            /// </summary>
            public object data { get; set; }
            /// <summary>
            /// id
            /// </summary>
            public string id
            {
                get
                {
                    return _id;
                }
                set
                {
                    _id = value;
                }
            }


        public string ParentId { get; set; }

        public string Level { get; set; }

        /// <summary>
        /// 显示文本
        /// </summary>
        public string text
            {
                get
                {
                    return _text;
                }
                set
                {
                    _text = value;
                }
            }

            /// <summary>
            /// 备注
            /// </summary>
            public string Remark
            {
                get
                {
                    return _remark;
                }
                set
                {
                    _remark = value;
                }
            }

            /// <summary>
            /// 孩子节点
            /// </summary>
            public object children
            {
                get
                {
                    return _children;
                }
                set
                {
                    _children = value;
                }
            }

            /// <summary>
            /// 图标样式
            /// </summary>
            public string iconCls
            {
                get
                {
                    return _iconCls;
                }
                set
                {
                    _iconCls = value;
                }
            }

            /// <summary>
            /// 复选框
            /// </summary>
            public bool Checked
            {
                get
                {
                    return _check;
                }
                set
                {
                    _check = value;
                }
            }

            /// <summary>
            /// 权限
            /// </summary>
            public string permissionsSelect
            {
                get
                {
                    return _permissionsselect;
                }
                set
                {
                    _permissionsselect = value;
                }
            }
        
    }
}
