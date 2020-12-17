//#define UNITTEST
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;
namespace GraphicsEvaluatePlatform.Service
{
    public class DetectionSettingService
    {
        /// <summary>
        /// 新增检测设置
        /// </summary>
        /// <param name="Ds"></param>
        /// <returns></returns>
        public static OperationResult Add(Dictionary<string, object> dic)
        {
            try
            {
                PublicHelper.CheckArgument(dic, "dic");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("DetectionSettingService").Error("Add 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                //判断是否重复
                int count = DataBll.GetCount("t_DetectionSettings", "UnitId='" + dic["UnitId"] + "' and Name='" + dic["Name"] + "'");
                if (count > 0)
                {
                    ret.Message = "该检测设置已存在";
                    ret.ResultType = OperationResultType.Error;
                    return ret;
                }

                using (var tran = new TransactionScope())
                {
                    // Ds.Remove("c_Id");                    
                    if (dic["UnitId"].ToString() == "-1")
                    {
#if UNITTEST
                        dic["UnitId"] = 1;//ServiceBase.GetInfo(ServiceBase.UNITID);
                        dic["UnitName"] = "泰坦软件研发部";//ServiceBase.GetInfo(ServiceBase.UNITFULLNAME);
#else
                        dic["UnitId"] = ServiceBase.GetInfo(ServiceBase.UNITID);
                        dic["UnitName"] = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME);
#endif
                    }

                    if (!dic.ContainsKey("Id"))
                    {
                        dic.Add("Id", Guid.NewGuid());
                    }
                    else
                    {
                        dic["Id"] = Guid.NewGuid();
                    }
                    if (!dic.ContainsKey("DataTime"))
                    {
                        dic.Add("DataTime", DateTime.Now);
                    }
                    else
                    {
                        dic["DataTime"] = DateTime.Now;
                    }
                    if (!dic.ContainsKey("DataSign"))//是否已删除标识 0：未删除 1：已删除
                    {
                        dic.Add("DataSign", "0");
                    }
                    else
                    {
                        dic["DataSign"] = "0";
                    }

                    var dsret = DataBll.Add(dic, "t_DetectionSettings");
                    if (dsret != null)
                    {
                        ret.Message = "新增检测设置成功！";
                        ret.AppendData = new { Success = true, Message = ret.Message };
                    }
                    else
                    {
                        ret.Message = "新增检测设置失败！";
                        ret.AppendData = new { Success = false, Message = ret.Message };
                    }

#if UNITTEST
                    //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "新增用户成功,内容: " + JsonUtil.ToJson(Ds) + "", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-新增用户", "新增名为:" + Ds["c_Name"] + "的检测设置成功", "");
#else
                    //OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "查询用户列表成功", "");
                    //UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户管理-查询用户列表", "查询成功", "");
#endif
                    ret.Message = "新增检测设置成功！";
                    ret.AppendData = new { Success = true, Message = ret.Message };
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("DetectionSettingService").Error("Add 发生异常", ex);
                if (ex.Message.IndexOf("不能在具有唯一索引") != -1)
                    return new OperationResult(OperationResultType.ParamError, "检测设置已存在", new { Success = false, Message = "检测设置已存在" });
                else
                    return new OperationResult(OperationResultType.ParamError, "新增失败", new { Success = false, Message = "新增检测设置失败" });
            }
            return ret;
        }
        /// <summary>
        /// 更改检测设置
        /// </summary>
        /// <param name="Ds"></param>
        /// <returns></returns>
        public static OperationResult Update(Dictionary<string, object> dic)
        {
            //检查传入的参数是否异常 
            try
            {
                PublicHelper.CheckArgument(dic, "dic");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("DetectionSetting").Error("Update 传入参数异常", ex);
            }
            var ret = new OperationResult(OperationResultType.Success);
            object re = new object();
            try
            {
                using (var tran = new TransactionScope())
                {
                    Dictionary<string, object> dickey = new Dictionary<string, object>();
                    dickey.Add("Id", dic["Id"]);
                    bool issuc = DataBll.Update(dic, "t_DetectionSettings", dickey, "Id");

#if UNITTEST
                    //  OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "更新ID为: " + Ds["Id"] + " 的检测设置成功", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-更新用户信息", "更新ID为: " + Ds["Id"] + " 的检测设置成功", "");
#else
                    //OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "更新ID为: " + Ds["Id"] + " 的检测设置成功", "");
                    //UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户管理-更新用户信息", "更新ID为: " + Ds["c_Id"] + " 的检测设置成功", "");
#endif
                    if (issuc)
                    {
                        ret.Message = "编辑检测设置成功";
                    }
                    else
                    {
                        ret.ResultType = OperationResultType.Error;
                        ret.Message = "编辑检测设置失败";
                    }
                    tran.Complete();
                }
                return ret;
            }
            catch (Exception ex)
            {
                ret.ResultType = OperationResultType.Error;
                ret.Message = "编辑检测设置异常";
                Logger.GetLogger("DetectionSettingService").Error("Message:" + ex.Message + "Inner:" + ex.InnerException);
            }
            return ret;
        }

        /// <summary>
        /// 获取检测设置列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public static OperationResult GetList(BootstrapPager pager)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(pager, "pager");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("DetectionSettingService").Error("GetList 发生异常", ex);
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
                    var userType = ServiceBase.GetInfo(ServiceBase.USERTYPE);
                    string fullPath = ServiceBase.GetInfo(ServiceBase.UNITFULLPATH).ToString();
#endif
                    //var userType = 1;
                    //string fullPath = "泰坦研发部";
                    string where = "";
                    if (userType.ToString() != "1")
                    {
                        //不是超级管理员，获取本机构和下级机构的机构Id
                        DataSet uds = DataBll.Query("select u_id from t_units where u_FullPath='" + fullPath + "'");
                        if (uds.Tables[0].Rows.Count > 0)
                        {
                            where = "and UnitId in (";
                            foreach (DataRow dr in uds.Tables[0].Rows)
                            {
                                where += Convert.ToInt32(dr["Id"].ToString()) + ",";
                            }
                            where = where.TrimEnd(',');
                            where += ")";
                        }

                    }
                    //超级管理员查询所有人员，不是超级管理员只查询本机构和下级机构的人员

                    //  var where = "and c_untid in ('"+unitid+"')";
                    var startIndex = 1;
                    var endIndex = 1;
                    if (pager.PageSize < 1)
                    {
                        pager.PageSize = 10;
                    }
                    if (pager.PageIndex < 1)
                    {
                        pager.PageIndex = 1;
                    }
                    startIndex = (pager.PageIndex - 1) * pager.PageSize;
                    endIndex = pager.PageIndex * pager.PageSize;
                    string sqlWhere = "(Name like '%" + pager.filter + "%'" + (unitid == "-1" ? ")" + where : ("and UnitId = '" + unitid + "')"));
                    var sql = "select * from ( select row_number() over (order by DataTime desc) rownumber , c.*,ut.u_Name from t_DetectionSettings c left join t_Units ut on c.unitId = ut.u_id where " + sqlWhere + " and DataSign = '0' ) ret where ret.rownumber > " + startIndex + " and ret.rownumber<=" + endIndex + " order by ret.rownumber asc";
                    var retft = DataBll.Query(sql).Tables[0];
                    var count = DataBll.GetCount("t_DetectionSettings", "(name like '%" + pager.filter + "%' and DataSign = '0' ) " + (unitid == "-1" ? "" + where : ("and unitId = '" + unitid + "'")));
                    ret.Message = "查询成功";
                    retft = DataTrim.DataTableTrim(retft);
                    ret.AppendData = new
                    {
                        total = count,
                        rows = retft
                    };

#if UNITTEST
                    //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "查询用户列表成功", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-查询用户列表", "查询成功", "");
#else
                    //OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "查询检测设置列表成功", "");
                    //UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "检测设置管理-查询检测设置列表", "查询成功", "");
#endif
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("DetectionSettingService").Error("GetList 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetList 发生异常", ex.Message);
            }
            return ret;
        }

        /// <summary>
        /// 删除检测设置
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static OperationResult Delete(string ids)
        {
            try
            {
                PublicHelper.CheckArgument(ids, "ids");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("DetectionSettingService").Error("Delete 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            if (ids == "")
            {
                ret = new OperationResult(OperationResultType.ParamError);
                ret.Message = "检测设置ID错误";
                return ret;
            }
            string[] arrids = ids.Split(',');
            if (arrids.Length == 0)
            {
                ret = new OperationResult(OperationResultType.ParamError);
                ret.Message = "检测设置ID错误";
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
                        dicCondition.Add("id", item);
                        //删除检测设置
                        string sql = "update t_DetectionSettings set DataSign = '1' where Id = '" + item + "'";
                        DataBll.Query(sql);
                        //DataBll.Delete(dicCondition, "t_DetectionSettings");

#if UNITTEST
                        //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "删除ID为: " + item + "的用户成功", "");
                        //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-删除用户", "删除ID为: " + item + "的用户成功", "");
#else
                        //OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "删除ID为: " + item + "的检测设置成功", "");
                        //UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "检测设置管理-删除检测设置", "删除ID为: " + item + "的检测设置成功", "");
#endif
                    }
                    tran.Complete();
                }
                ret = new OperationResult(OperationResultType.Success);
                ret.Message = "删除检测设置信息成功";
            }
            catch (Exception ex)
            {
                Logger.GetLogger("DetectionSettingService").Error("Delete 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "Delete 发生异常", ex);
            }
            return ret;
        }
        public static OperationResult IsEnable(Dictionary<string, object> dic)
        {
            //检查传入的参数是否异常 
            try
            {
                PublicHelper.CheckArgument(dic, "dic");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("DetectionSetting").Error("Update 传入参数异常", ex);
            }
            var ret = new OperationResult(OperationResultType.Success);
            object re = new object();
            try
            {
                using (var tran = new TransactionScope())
                {
                    Dictionary<string, object> dickey = new Dictionary<string, object>();
                    dic.Add("Id", dic["Id"]);
                    bool issuc = DataBll.Update(dic, "t_DetectionSettings", dickey, "Id");

#if UNITTEST
                    //  OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "更新ID为: " + Ds["Id"] + " 的检测设置成功", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-更新用户信息", "更新ID为: " + Ds["Id"] + " 的检测设置成功", "");
#else
                    //OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "更新ID为: " + Ds["Id"] + " 的检测设置成功", "");
                    //UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户管理-更新用户信息", "更新ID为: " + Ds["c_Id"] + " 的检测设置成功", "");
#endif
                    if (issuc)
                    {
                        ret.Message = "编辑检测设置成功";
                    }
                    else
                    {
                        ret.ResultType = OperationResultType.Error;
                        ret.Message = "编辑检测设置失败";
                    }
                    tran.Complete();
                }
                return ret;
            }
            catch (Exception ex)
            {
                ret.ResultType = OperationResultType.Error;
                ret.Message = "编辑检测设置异常";
                Logger.GetLogger("DetectionSettingService").Error("Message:" + ex.Message + "Inner:" + ex.InnerException);
            }
            return ret;
        }
        public static OperationResult ActiveDetectionSetting(string ids, string values)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                PublicHelper.CheckArgument("ids", ids);
                PublicHelper.CheckArgument("values", values);
            }
            catch (Exception ex)
            {
                Logger.GetLogger("DetectionSettingService").Error("ActiveDetectionSetting发生异常", ex);
                return new OperationResult(OperationResultType.Error, "DetectionSettingService的方法ActiveDetectionSetting参数有误!");
            }
            try
            {
                StringBuilder sbSql = new StringBuilder();
                string[] arrIds = ids.Split(',');
                string[] arrValues = values.Split(',');
                for (int i = 0; i < arrIds.Length; i++)
                {
                    sbSql.Append(";Update t_DetectionSettings set c_IsEnable='" + arrValues[i] + "' where c_id='" + arrIds[i] + "';");
                }
                using (var tran = new TransactionScope())
                {
                    DataBll.Query(sbSql.ToString());
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("DetectionSettingService").Error("ActiveDetectionSetting发生异常", ex);
                return new OperationResult(OperationResultType.Error, "DetectionSettingService的方法ActiveDetectionSetting参数有误!");
            }
            return ret;
        }

        public static object GetSettings()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
          
            var data = DataBll.Query("select * from DetectionSettings").Tables[0];
            return data;

        }
    }
}
