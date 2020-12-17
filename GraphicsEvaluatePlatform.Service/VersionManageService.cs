using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GraphicsEvaluatePlatform.Service
{
   public class VersionManageService
    {
       /// <summary>
       /// 获取版本列表
       /// </summary>
       /// <param name="pager"></param>
       /// <returns></returns>
        public static OperationResult getList(BootstrapPager pager) {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(pager, "pager");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("VersionManageService").Error("GetList 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    var unitid = "";
                    var filters = pager.filter.Split(',');
                    unitid = filters[1].Split(':')[1];
                    pager.filter = filters[0];
#if UNITTEST
                    //var userType = 1;
                    //string fullPath = "泰坦研发部";
#else
                    var userType = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString().Trim();//用户类型
                    string fullPath = ServiceBase.GetInfo(ServiceBase.UNITFULLPATH).ToString();//单位全路径
#endif
                    string unitWhere = "";

                    //搜索关键字
                    if (pager.filter != "")
                    {
                        unitWhere = "v_VersionName like '%" + pager.filter + "%";
                    }

                    //用户权限
                    if (unitid == "-1")//选取全部
                    {
                        if (userType == "1")//超级管理员（获取所有单位数据）
                        {
                            unitWhere += "";
                        }
                        else if (userType == "2")//一般管理员（获取本级和下级机构数据）
                        {
                            var UnitName = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString().Trim();
                            var unitids = DataBll.Query("select u_Id from t_Units where u_FullPath = '" + UnitName + "' or u_FullPath like '%" + UnitName + "-%' or u_FullPath like '%-" + UnitName + "'");
                            var idstr = "";
                            foreach (DataRow iditem in unitids.Tables[0].Rows)
                                idstr += "'" + iditem[0].ToString() + "',";
                            idstr = idstr.Substring(0, idstr.Length - 1);
                            unitWhere += ((unitWhere == "" ? "" : " and ") + " v_UnitId in (" + idstr + ")");//本级和下级的机构id
                        }
                        else
                        {//一般操作员（获取本单位数据）
                            var UnitID = ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim();
                            unitWhere += ((unitWhere == "" ? "" : " and ") + " v_UnitId = '" + UnitID + "'");
                        }
                    }
                    else
                    {//选择某一单位
                        unitWhere += ((unitWhere == "" ? "" : " and ") + " v_UnitId = '" + unitid + "'");
                    }

                    var datas = DataBll.GetDataSetList("t_Version", pager.PageSize, pager.PageIndex, "", unitWhere, "v_CreationTime desc", "v_id");//分页
                    var count = DataBll.GetCount("t_Version", unitWhere);

                    ret.Message = "查询成功";
                    ret.AppendData = new { total = count, rows = DataTrim.DataTableTrim(datas.Tables[0]) };
#if UNITTEST
                    //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "查询用户列表成功", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-查询用户列表", "查询成功", "");
#else
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "查询版本列表成功", "");
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "版本管理-查询版本列表", "查询成功", "");
#endif
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("VersionManageService").Error("getList 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "getList 发生异常", ex.Message);
            }
            return ret;
        }

        /// <summary>
        /// 新增版本
        /// </summary>
        /// <param name="Pro">版本键值对</param>
        /// <returns></returns>
        public static OperationResult addVersion(Dictionary<string, object> Pro) {
            try
            {
                PublicHelper.CheckArgument(Pro, "Pro");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("VersionManagementService").Error("addVersion 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                //判断是否重复
                int count = DataBll.GetCount("t_Version", " v_VersionName='" + Pro["v_VersionName"] + "'");
                if (count > 0)
                {
                    ret.Message = "该版本已存在！";
                    ret.ResultType = OperationResultType.Error;
                    return ret;
                }

                using (var tran = new TransactionScope())
                {
                    if (Pro["v_UnitId"].ToString() == "-1")
                    {
                        Pro.Add("v_Id", Guid.NewGuid());//条目id
                        Pro.Add("v_CreationTime", DateTime.Now);//创建时间
                        Pro.Add("v_Creator", ServiceBase.GetInfo(ServiceBase.USERNAME).ToString().Trim());//创建者
                        var dsret = DataBll.Add(Pro, "t_Version");
                        if (dsret != null)
                        {
                            ret.Message = "新增版本成功！";
                            ret.AppendData = new { Success = true, Message = ret.Message };
                        }
                        else
                        {
                            ret.Message = "新增版本失败！";
                            ret.AppendData = new { Success = false, Message = ret.Message };
                        }

#if UNITTEST
                    //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "新增用户成功,内容: " + JsonUtil.ToJson(Pro) + "", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-新增用户", "新增名为:" + Pro["p_Name"] + "的版本成功", "");
#else
                        OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "新增版本成功", "");
                        UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-版本管理-新增版本", "新增版本成功", "");
#endif
                        ret.Message = "新增版本成功！";
                        ret.AppendData = new { Success = true, Message = ret.Message };
                        tran.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("VersionManageService").Error("addVersion 发生异常", ex);
                if (ex.Message.IndexOf("不能在具有唯一索引") != -1)
                    return new OperationResult(OperationResultType.ParamError, "版本已存在", new { Success = false, Message = "版本已存在" });
                else
                    return new OperationResult(OperationResultType.ParamError, "新增失败", new { Success = false, Message = "新增版本失败" });
            }
            return ret;
        }

        /// <summary>
        /// 更改版本
        /// </summary>
        /// <param name="Pro"></param>
        /// <returns></returns>
        public static OperationResult Update(Dictionary<string, object> Pro)
        {
            //检查传入的参数是否异常 
            try
            {
                PublicHelper.CheckArgument(Pro, "Pro");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("Pro").Error("Update 传入参数异常", ex);
            }
            var ret = new OperationResult(OperationResultType.Success);
            object re = new object();
            try
            {
                using (var tran = new TransactionScope())
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("v_Id", Pro["v_Id"]);
                    bool issuc = DataBll.Update(Pro, "t_Version", dic, "v_Id");

#if UNITTEST
                  //  OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "更新ID为: " + Pro["p_Id"] + " 的版本成功", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-更新用户信息", "更新ID为: " + Pro["p_Id"] + " 的版本成功", "");
#else
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "更新ID为: " + Pro["v_Id"] + " 的版本成功", "");
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-版本管理-更新版本信息", "更新ID为: " + Pro["v_Id"] + " 的版本成功", "");
#endif
                    if (issuc)
                    {
                        ret.Message = "编辑版本成功";
                    }
                    else
                    {
                        ret.ResultType = OperationResultType.Error;
                        ret.Message = "编辑版本失败";
                    }
                    tran.Complete();
                }
                return ret;
            }
            catch (Exception ex)
            {
                ret.ResultType = OperationResultType.Error;
                ret.Message = "编辑版本异常";
                Logger.GetLogger("VersionManageService").Error("Message:" + ex.Message + "Inner:" + ex.InnerException);
            }
            return ret;
        }

        /// <summary>
        /// 删除版本
        /// </summary>
        /// <param name="ids">删除的版本id</param>
        /// <returns></returns>
        public static OperationResult Delete(string ids)
        {
            try
            {
                PublicHelper.CheckArgument(ids, "ids");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("VersionManagementService").Error("Delete 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            if (ids == "")
            {
                ret = new OperationResult(OperationResultType.ParamError);
                ret.Message = "版本ID错误";
                return ret;
            }
            string[] arrids = ids.Split(',');
            if (arrids.Length == 0)
            {
                ret = new OperationResult(OperationResultType.ParamError);
                ret.Message = "版本ID错误";
                return ret;
            }
            try
            {
                using (var tran = new TransactionScope())
                {
                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                    foreach (var item in arrids)
                    {
                        dicCondition.Clear();
                        dicCondition.Add("v_Id", item);
                        //删除版本记录
                        DataBll.Delete(dicCondition, "t_Version");

#if UNITTEST
                        //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "删除ID为: " + item + "的用户成功", "");
                        //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-删除用户", "删除ID为: " + item + "的用户成功", "");
#else
                        OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "删除ID为: " + item + "的版本成功", "");
                        UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "版本管理-删除版本", "删除ID为: " + item + "的版本成功", "");
#endif
                    }
                    tran.Complete();
                }
                ret = new OperationResult(OperationResultType.Success);
                ret.Message = "删除版本信息成功";
            }
            catch (Exception ex)
            {
                Logger.GetLogger("VersionManagementService").Error("Delete 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "Delete 发生异常", ex);
            }
            return ret;
        }

        /// <summary>
        /// 启用，禁用版本
        /// </summary>
        /// <param name="ids">条目id</param>
        /// <param name="values">1:启用，0：禁用</param>
        /// <returns></returns>
        public static OperationResult ActiveVersion(string ids, string values)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                PublicHelper.CheckArgument("ids", ids);
                PublicHelper.CheckArgument("values", values);
            }
            catch (Exception ex)
            {
                Logger.GetLogger("VersionManageService").Error("ActiveVersion发生异常", ex);
                return new OperationResult(OperationResultType.Error, "VersionManageService的方法 ActiveVersion参数有误!");
            }
            try
            {
                StringBuilder sbSql = new StringBuilder();
                string[] arrIds = ids.Split(',');
                string[] arrValues = values.Split(',');
                for (int i = 0; i < arrIds.Length; i++)
                {
                    sbSql.Append(";Update t_Version set v_Status='" + arrValues[i] + "' where v_Id='" + arrIds[i] + "';");
                }
                using (var tran = new TransactionScope())
                {
                    DataBll.Query(sbSql.ToString());
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("VersionManageService").Error(" ActiveVersion发生异常", ex);
                return new OperationResult(OperationResultType.Error, "VersionManageService的方法 ActiveVersion参数有误!");
            }
            return ret;
        }

    }
}
