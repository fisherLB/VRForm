using GraphicsEvaluatePlatform.Client.Models;
using GraphicsEvaluatePlatform.Client.Repository;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace GraphicsEvaluatePlatform.Client.Services
{
    class PictureService : IPictureService
    {
        public OperationResult GetList(int pageNum, int currentlyPage, Dictionary<string, object> dic)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(pageNum, "pageNum");
                PublicHelper.CheckArgument(currentlyPage, "currentlyPage");
                PublicHelper.CheckArgument(dic, "dic");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("PictureService").Error("GetList 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    string where = " ";
                    if (dic.ContainsKey("ProjectId"))
                    {
                        where = "ProjectId='" + dic["ProjectId"].ToString() + "' ";
                    }
                    if (dic.ContainsKey("Ids"))
                    {
                        var IdArr = dic["Ids"].ToString().Split(',');
                        StringBuilder sb = new StringBuilder();
                        if (IdArr.Length>0)
                        {        
                            foreach (var id in IdArr)
                            {
                                sb.Append( "'" + id + "',");
                            }
                            sb.Remove(sb.Length - 1, sb.Length);
                            where = "Id in (" + sb + ")";
                        }                       
                    }

                    var ds = DataBll.GetDataSetList("t_picture", pageNum, currentlyPage, "id,projectid,projectname,name,path,size,type,status,remarks", where, " name asc", "name");
                    var total = DataBll.GetCount("t_picture", where);
                    ret = new OperationResult(OperationResultType.Success);
                    ret.AppendData = new PageData
                    {
                        total = total,
                        current = ds.Tables[0].Rows.Count,
                        rows = ds
                    };
                    ret.Message = "查询列表成功";
#if UNITTEST
                    //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "查询用户列表成功", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-查询用户列表", "查询成功", "");
#else
                    //OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "查询项目列表成功", "");
                    //UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "项目管理-查询项目列表", "查询成功", "");
#endif
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("PictureService").Error("GetList 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetList 发生异常", ex.Message);
            }
            return ret;
        }

        public OperationResult GetDetail(Guid id)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(id, "id");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectService").Error("GetDetail 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    {
                        dic.Add("Id", id);
                    }
                    var retft = DataBll.GetModel("", "t_picture", dic);
                    ret = new OperationResult(OperationResultType.Success, "查询成功", retft);
#if UNITTEST
                    //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "查询用户列表成功", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-查询用户列表", "查询成功", "");
#else
                    //OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "查询项目列表成功", "");
                    //UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "项目管理-查询项目列表", "查询成功", "");
#endif
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectService").Error("GetDetail 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetDetail 发生异常", ex.Message);
            }
            return ret;
        }
    }
}
