using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Repository;
using GraphicsEvaluatePlatform.Service;
using GraphicsEvaluatePlatform.WebPlatform.Filter;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GraphicsEvaluatePlatform.WebPlatform.Controllers.WebAPI
{
    public class ClientAPIController : ApiController
    {      
        /// <summary>
        /// 取终端列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [HttpGet]
        [SkipPermission]
        [Route("api/Client/GetClientsList")]
        public string GetClientsList([FromUri]BootstrapPager pager)
        {
            if (pager.filter == null)
                pager.filter = "";
            OperationResult ret = ClientManageService.GetList(pager);
            string retJson = "";
            retJson = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
            return retJson;
        }

        /// <summary>
        /// 新增终端
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Client/AddClient")]
        public string AddClient(dynamic json)
        {
            //Dictionary<string, object> pro = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);
            Dictionary<string, object> pro = JsonUtil.ConvertToObject<Dictionary<string, object>>(JsonUtil.ToJson(json));
            OperationResult ret = ClientManageService.Add(pro);
            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message
            };
            return JsonUtil.ToJson(data);

        }
        /// <summary>
        /// 编辑终端信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Client/EditClient")]
        public string EditClient(dynamic json)
        {
            // Dictionary<string, object> pro = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);
            Dictionary<string, object> pro = JsonUtil.ConvertToObject<Dictionary<string, object>>(JsonUtil.ToJson(json));
            OperationResult ret = ClientManageService.Update(pro);
            if (ret.ResultType == OperationResultType.Success)
            {
                var data = new
                {
                    Success = true,
                    Message = "修改客户端成功!"
                };
                return JsonUtil.ToJson(data);
            }
            else
            {
                var data = new
                {
                    Success = false,
                    Message = "修改客户端失败!"
                };
                return JsonUtil.ToJson(data);
            }
        }

        /// <summary>
        /// 删除终端
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Client/DelClients")]
        public string DelClients([FromBody]string ids)
        {
            OperationResult ret = ClientManageService.Delete(ids);
            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message,
                Data = ret.AppendData
            };
            return JsonUtil.ToJson(data);
        }

        /// <summary>
        /// 启用终端
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Client/ActiveClients")]
        public string ActiveClients(dynamic ids)
        {
            string[] arrids = ids.Split('|');
            string idss = arrids[0];
            string values = arrids[1];
            OperationResult ret = new OperationResult(OperationResultType.Success);
            ret = ClientManageService.ActiveClients(idss, values);
            if (ret.ResultType == 0)
            {
                return JsonUtil.ToJson(new { Success = true, Message = "操作成功" }, "yyyy-MM-dd");
            }
            else
            {
                return JsonUtil.ToJson(new { Success = false, Message = ret.Message }, "yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 禁用终端
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Client/ForbiddenClients")]
        public string ForbiddenClients(dynamic ids)
        {
            string[] arrids = ids.Split('|');
            string idss = arrids[0];
            string values = arrids[1];
            OperationResult ret = new OperationResult(OperationResultType.Success);
            ret = ClientManageService.ActiveClients(idss, values);
            if (ret.ResultType == 0)
            {
                return JsonUtil.ToJson(new { Success = true, Message = "操作成功" }, "yyyy-MM-dd");
            }
            else
            {
                return JsonUtil.ToJson(new { Success = false, Message = ret.Message }, "yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <param name="boj"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetUsersData([FromBody] string Ip)
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
        [HttpGet]
        public string GetUnitData([FromUri] string ip)
        {
            OperationResult op = new OperationResult(OperationResultType.Success);
            try
            {
                var ret = DataBll.Query("select * from t_units");

                if (ret.Tables[0].Rows.Count > 0)
                {
                    op.AppendData = ret.Tables[0];
                    op.Message = "获取用户数据成功";
                }
                else
                {
                    op.ResultType = OperationResultType.QueryNull;
                    op.AppendData = null;
                    op.Message = "获取失败，不存在用户数据";
                }
            }
            catch (Exception e)
            {
                op.ResultType = OperationResultType.Error;
                op.AppendData = null;
                op.Message = "获取失败，查询异常";
            }

            return JsonUtil.ToJson(op);
        }

        /// <summary>
        /// 请求项目数据
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetObjectData([FromUri] string ip)
        {
            OperationResult op = new OperationResult(OperationResultType.Success);
            try
            {
                var ret = DataBll.Query("select * from t_Projects");

                if (ret.Tables[0].Rows.Count > 0)
                {
                    op.AppendData = ret.Tables[0];
                    op.Message = "获取用户数据成功";
                }
                else
                {
                    op.ResultType = OperationResultType.QueryNull;
                    op.AppendData = null;
                    op.Message = "获取失败，不存在用户数据";
                }
            }
            catch (Exception e)
            {
                op.ResultType = OperationResultType.Error;
                op.AppendData = null;
                op.Message = "获取失败，查询异常";
            }

            return JsonUtil.ToJson(op);
        }
    }
}