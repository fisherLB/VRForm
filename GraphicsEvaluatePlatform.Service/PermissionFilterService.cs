/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2017. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Service
 * 项目描述: 
 * 类 名 称: PermissionFilterService
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: 覃明健
 * 命名空间: GraphicsEvaluatePlatform.Service
 * 文件名称: PermissionFilterService
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/5/11 8:40:54
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Service
{
    public static class PermissionFilterService
    {
        /// <summary>
        /// 获取权限级别
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static int GetCurrentUserPermissionGrade(string url)
        {
            int rid = 0;
            //获取用户的权限
            object obj = ServiceBase.GetInfo(ServiceBase.USER_PERMISSION);
            DataSet ds = (DataSet)obj;
            foreach (DataRow dr in DataTrim.DataTableTrim(ds.Tables[0]).Rows)
            {
                if (url == dr["Full_url"].ToString().ToLower().Trim())
                {
                    rid = Convert.ToInt32(dr["Func_grade"].ToString().Trim());
                }
            }
            return rid;
        }

        /// <summary>
        /// 判断当前操作是否有权限
        /// </summary>
        /// <param name="url">action</param>
        /// <returns></returns>
        public static bool CheckCurrentUserOperPermission(string url)
        {
            try
            {
                return GetCurrentUserPermissionGrade(url) > 0;
            }
            catch (Exception)
            {

                return false;
            }

        }

        /// <summary>
        /// 检查action权限
        /// </summary>
        /// <param name="url"></param>
        public static MessageModel CheckActionPermission(string url)
        {
            url = url.Trim().ToLower();
            MessageModel msg = new MessageModel("您没有权限", false, "0");

            //判断用户类型,用户类型为1，超级管理员，拥有所有权限
            int userType = Convert.ToInt32(ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString().Trim());

            int mid = GetCurrentUserPermissionGrade(url);

            if (mid == 3)
            {
                msg.Message = "该功能需要上级授权，才能操作！";
                msg.Data = "3";
            }
            else if (mid == 2)
            {
                msg.Message = "该功能需要身份验证，才能操作！";
                msg.Data = "2";
            }
            else if (mid == 1)
            {
                msg.Success = true;
                msg.Message = "该功能您有权限操作";
            }

            //超级管理员拥有所有权限
            if (userType == 1)
            {
                msg.Success = true;
                msg.Message = "该功能您有权限操作";
            }
            return msg;
        }
    }
}
