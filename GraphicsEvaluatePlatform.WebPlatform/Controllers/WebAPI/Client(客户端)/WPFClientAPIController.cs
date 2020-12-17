using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GraphicsEvaluatePlatform.WebPlatform.Controllers.WebAPI
{
    public class WPFClientAPIController : ApiController
    {
        /// <summary>
        /// 获取检测设置
        /// </summary>
        /// <param name="boj"></param>
        /// <returns></returns>
        [HttpPost]
        public string  GetUsersData([FromBody] string Ip)
        {
            ClientDataModel reModel = new ClientDataModel();
            try
            {
                var ret = DataBll.Query("select * from t_Users");
               
                if (ret.Tables[0].Rows.Count > 0)
                {
                    reModel.Data = DataTrim.DataTableTrim(ret.Tables[0]);
                    reModel.Message = "获取用户数据成功";
                    reModel.Success = true;
                }
                else
                {
                    reModel.Success = false;
                    reModel.Data = null;
                    reModel.Message = "获取失败，不存在用户数据";
                }
            }
            catch (Exception e)
            {
                reModel.Success = false;
                reModel.Data = null;
                reModel.Message = "获取失败，查询异常";
            }
           
            return JsonUtil.ToJson(reModel);
        }

        /// <summary>
        /// 请求机构数据
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public string GetUnitData([FromBody] string ip)
        {
            ClientDataModel reModel = new ClientDataModel(); 
            try
            {
                var ret = DataBll.Query("select * from t_units");

                if (ret.Tables[0].Rows.Count > 0)
                {
                    reModel.Success = true;
                    reModel.Data = ret.Tables[0];
                    reModel.Message = "获取机构数据成功";
                }
                else
                {
                    reModel.Success = false;
                    reModel.Data = null;
                    reModel.Message = "获取失败，不存在机构数据";
                }
            }
            catch (Exception e)
            {
                reModel.Success = false;
                reModel.Data = null;
                reModel.Message = "获取失败，查询异常";
            }

            return JsonUtil.ToJson(reModel);
        }

        /// <summary>
        /// 请求项目数据
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        
        public string GetObjectData([FromBody] string ip)
        {
            ClientDataModel reModel = new ClientDataModel();
            try
            {
                var ret = DataBll.Query("select * from t_Projects");

                if (ret.Tables[0].Rows.Count > 0)
                {
                    reModel.Success = true;
                    reModel.Data = ret.Tables[0];
                    reModel.Message = "获取用户数据成功";
                }
                else
                {
                    reModel.Success = false;
                    reModel.Data = null;
                    reModel.Message = "获取失败，不存在用户数据";
                }
            }
            catch (Exception e)
            {
                reModel.Success =false;
                reModel.Data = null;
                reModel.Message = "获取失败，查询异常";
            }

            return JsonUtil.ToJson(reModel);
        }

        /// <summary>
        /// 请求检测设置数据
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetDetectionSettingsData([FromBody] string ip)
        {
            ClientDataModel reModel = new ClientDataModel();
            try
            {
                var ret = DataBll.Query("select * from t_DetectionSettings");

                if (ret.Tables[0].Rows.Count > 0)
                {
                    reModel.Success = true;
                    reModel.Data = ret.Tables[0];
                    reModel.Message = "获取用户数据成功";
                }
                else
                {
                    reModel.Success = false;
                    reModel.Data = null;
                    reModel.Message = "获取失败，不存在用户数据";
                }
            }
            catch (Exception e)
            {
                reModel.Success = false;
                reModel.Data = null;
                reModel.Message = "获取失败，查询异常";
            }

            return JsonUtil.ToJson(reModel);
        }


        /// <summary>
        /// 请求检测设置数据
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetVersionData([FromBody] string ip)
        {
            ClientDataModel reModel = new ClientDataModel();
            try
            {
                var ret = DataBll.Query("select * from t_Version order by v_CreationTime desc");
             
                if (ret.Tables[0].Rows.Count > 0)
                {
                    reModel.Success = true;
                    reModel.Data = ret.Tables[0];
                    reModel.Message = "获取版本数据成功";
                }
                else
                {
                    reModel.Success = false;
                    reModel.Data = null;
                    reModel.Message = "获取失败，不存在版本数据";
                }
            }
            catch (Exception e)
            {
                reModel.Success = false;
                reModel.Data = null;
                reModel.Message = "获取失败，查询异常";
            }
            return JsonUtil.ToJson(reModel);
        }
    }
}