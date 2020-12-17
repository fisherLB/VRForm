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
 * 类 名 称: MenuTree
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Model
 * 文件名称: MenuTree
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/3 11:40:50
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Model
{
    [Serializable]
    public class MenuTree
    {
            string _Func_id;
            string _Func_name;
            string _Func_full_name;
            string _Func_urlPath;
            string _Func_sequence;
            string _Func_parent_id;
            string _Depth;

            /// <summary>
            /// 数据
            /// </summary>
        public object data { get; set; }
            /// <summary>
            /// id
            /// </summary>
            public string Func_id
            {
                get
                {
                    return _Func_id;
                }
                set
                {
                    _Func_id = value;
                }
            }

            public string Func_type { get; set; }

            public string Func_code { get; set; }

            public string ParentId
            {
                get
                {
                    return _Func_parent_id;
                }
                set
                {
                    _Func_parent_id = value;
                }
            }

            public string Func_name
            {
                get
                {
                    return _Func_name;
                }
                set
                {
                    _Func_name = value;
                }
            }


            public string Func_full_name
            {
                get
                {
                    return _Func_full_name;
                }
                set
                {
                    _Func_full_name = value;
                }
            }


            public string Func_urlPath
            {
                get
                {
                    return _Func_urlPath;
                }
                set
                {
                    _Func_urlPath = value;
                }
            }

            public string Func_sequence
            {
                get
                {
                    return _Func_sequence;
                }
                set
                {
                    _Func_sequence = value;
                }
            }

            public string Func_icon { get; set; }
            public string Depth { get; set; }
        /// <summary>
        /// 孩子节点
        /// </summary>
        public List<MenuTree> children { get; set; }
    }
}
