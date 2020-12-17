using System;
using System.Transactions;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Repository;

namespace GraphicsEvaluatePlatform.ImageValidSystem.Lib
{
    public static class BllProjectsManage
    {
        public static OperationResult GetList(string unitId)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(unitId, "unitId");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("BllProject").Error("GetList 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    string sqlwhere = "";
                    if (!string.IsNullOrEmpty(unitId)||unitId=="-1")
                    {
                        sqlwhere = "pro.p_unitId='" + unitId + "'";
                    }
                    var sql = "select p_Id,p_name,p_UnitId  from t_projects pro "+ sqlwhere + " order by p_name";
                    var retft = DataBll.Query(sql);;
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
                Logger.GetLogger("ProjectManagementService").Error("GetList 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetList 发生异常", ex.Message);
            }
            return ret;
        }
    }
}
