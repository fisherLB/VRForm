using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;
using GraphicsEvaluatePlatform.ImageValidSystem.Model;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Repository;

namespace GraphicsEvaluatePlatform.ImageValidSystem.Lib
{
    public static class BllPicture
    {
        public static OperationResult GetList(string unitId,string projectId,int pageNum, int currentlyPage)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(unitId, "unitId");
                PublicHelper.CheckArgument(unitId, "projectId");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("BllPicture").Error("GetList 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    string where = "";
                    if (!string.IsNullOrEmpty(unitId)&& unitId != "-1"&&unitId!=Guid.Empty.ToString())
                    {
                        dic.Add("untId", unitId);
                    }
                    if (!string.IsNullOrEmpty(projectId) && projectId != "-1")
                    {
                        dic.Add("projectId", projectId);
                        where = "projectId='" + projectId + "' ";
                    }
                    var ds = DataBll.GetDataSetList("t_picture",pageNum,currentlyPage, "id,projectid,projectname,name,path,size,type,status,remarks",where, " name asc", "name");
                    var total = DataBll.GetCount("t_picture",where); 
                    ret = new OperationResult(OperationResultType.Success);
                    ret.AppendData = new PageDataModel
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
                Logger.GetLogger("BllPicture").Error("GetList 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetList 发生异常", ex.Message);
            }
            return ret;
        }

        internal static OperationResult AddFile(FileInfo[] files,string projectId,string projectname)
        {
            foreach (var file in files)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("id", Guid.NewGuid().ToString());
                dic.Add("name", file.Name);
                dic.Add("path", file.FullName);
                dic.Add("datasign",1);
                dic.Add("datatime",DateTime.Now);
                dic.Add("userid",Guid.Empty.ToString());
                dic.Add("projectid",projectId);
                dic.Add("projectname", projectname);
                dic.Add("size",file.Length);
                dic.Add("type",file.Extension);
                dic.Add("status",0);
                dic.Add("remarks","");
                var ret=DataBll.Add(dic,"t_picture");
            }
            throw new NotImplementedException();
        }
    }
}
