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
    public class DetectionSettingService : IDetectionSettingService
    {
        public  DetectionSetting GetDetail(Guid id)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(id, "id");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("DetectionSettingService").Error("GetDetail 发生异常", ex);
                return null;
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    if ( id != Guid.Empty)
                    {
                        dic.Add("p_Id", id);
                    }
                    var retft = DataBll.GetModel("", "t_detectionsetting", dic);
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
                Logger.GetLogger("DetectionSettingService").Error("GetDetail 发生异常", ex);
                ret = null;
            }
            return null;
        }

        internal bool NameQualified(string name)
        {
            throw new NotImplementedException();
        }
    }
}
