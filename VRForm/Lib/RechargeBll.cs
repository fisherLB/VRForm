using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace VRForm.Lib
{
    public class RechargeBll
    {
        public static OperationResult Recharge(string number)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(number, "number");
                
            }
            catch (Exception ex)
            {
                Logger.GetLogger("RegBll").Error("UserLogin 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }

            //声明返回值
            var ret = new OperationResult(OperationResultType.Success);

            try
            {
                using (var tran = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    //声明查询条件
                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                    //声明更新目标
                    Dictionary<string, object> dicUpdateParam = new Dictionary<string, object>();

                    //根据传入的帐号判断系统是否存在该帐号
                    dicCondition.Add("Number", number);
                    var retData = DataBll.GetModel("", "Card", dicCondition);
                    if (retData.Tables[0].Rows.Count ==0)
                    {
                        return new OperationResult(OperationResultType.QueryNull, $"抱歉卡密不存在，请确认卡密是否正确");
                    }



                    //判断用户名是否重复(同一机构下)
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Number", number);
                    //dic.Add("Password", Password);
                    DataTable ds = DataTrim.DataTableTrim(DataBll.GetModel("", "Card", dic).Tables[0]);
                    if (ds.Rows.Count >= 1)
                    {
                        var used = Convert.ToBoolean(ds.Rows[0]["IsUsed"].ToString());
                        if (used)
                        {
                            return new OperationResult(OperationResultType.Error, "抱歉，卡密已经被使用", new { Success = false, Message = "卡密已经被使用" });
                        }
                       
                    }

                    Dictionary<string, object> updic = new Dictionary<string, object>();
                    updic.Add("IsUsed", true);
                    var dsret = DataBll.Update(updic, "Card", null, ds.Rows[0]["Id"].ToString()); 
                    ret.Message = "充值成功";
                    ret.AppendData = new { Success = true, Message = ret.Message };
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("AddUser 发生异常", ex);
                if (ex.Message.IndexOf("不能在具有唯一索引") != -1)
                    return new OperationResult(OperationResultType.ParamError, "用户名已存在", new { Success = false, Message = "用户名已存在" });
                else
                    return new OperationResult(OperationResultType.ParamError, "新增用户失败", new { Success = false, Message = "新增用户失败" });
            }

            return ret;
        }
    }
}
