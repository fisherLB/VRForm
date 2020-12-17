using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Transactions;
using GraphicsEvaluatePlatform.ImageValidSystem.Mdel;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Repository;

namespace GraphicsEvaluatePlatform.ImageValidSystem.Lib
{
    public static class BllProject
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
                    string sqlwhere = "where 1=1 ";
                    if (!string.IsNullOrEmpty(unitId) && unitId != "-1" && unitId != Guid.Empty.ToString())
                    {
                        sqlwhere += " and  pro.p_unitId='" + unitId + "'";
                    }

                    var sql = "select p_Id,p_name,p_UnitId  from t_projects pro " + sqlwhere + " order by p_name";
                    var retft = DataBll.Query(sql); ;
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
                Logger.GetLogger("BllProject").Error("GetList 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetList 发生异常", ex.Message);
            }
            return ret;
        }

        internal static OperationResult GetDetail(string id)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(id, "id");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("BllProject").Error("GetDetail 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(id) && id != "-1" && id != Guid.Empty.ToString())
                    {
                        dic.Add("p_Id", id);
                    } 
                    var retft = DataBll.GetModel("", "t_projects", dic);
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
                Logger.GetLogger("BllProject").Error("GetDetail 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetDetail 发生异常", ex.Message);
            }
            return ret;
        }

        internal static OperationResult Save(Projects model, string type)
        {
            try
            {
                PublicHelper.CheckArgument(model, "model");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("BllProject").Error("Add 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                //判断是否重复
                int count = DataBll.GetCount("t_Projects", "p_UnitId='" + model.P_UnitId + "' and p_Name='" + model.P_Name + "'");
                if (count > 0)
                {
                    ret.Message = "该项目已存在";
                    ret.ResultType = OperationResultType.Error;
                    return ret;
                }

                using (var tran = new TransactionScope())
                {
                    // Pro.Remove("p_Id");                    
                    if (model.P_UnitId == "-1")
                    {
#if UNITTEST
                        Pro["p_UnitId"] = 1;//ServiceBase.GetInfo(ServiceBase.UNITID);
                        Pro["p_UnitName"] = "泰坦软件研发部";//ServiceBase.GetInfo(ServiceBase.UNITFULLNAME);
#else
                        //Pro["p_UnitId"] = ServiceBase.GetInfo(ServiceBase.UNITID);
                        //Pro["p_UnitName"] = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME);
#endif
                    }
                    if (string.IsNullOrEmpty(model.P_Id))
                    {
                        model.P_Id = Guid.NewGuid().ToString();
                    }

                    if (string.IsNullOrEmpty(model.P_DataTime))
                    {
                        model.P_DataTime = DateTime.Now.ToString();
                    }
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    Dictionary<string, object> dicwhere = new Dictionary<string, object>();
                    dicwhere.Add("p_Id",model.P_Id);
                    dic = EntityToDictionary<Projects>(model);
                    DataSet dsret  = DataBll.Add(dic, "t_projects");

                    if (dsret != null)
                    {
                        ret.Message = "新增项目成功！";
                        ret.AppendData = new { Success = true, Message = ret.Message };
                    }
                    else
                    {
                        ret.Message = "新增项目失败！";
                        ret.AppendData = new { Success = false, Message = ret.Message };
                    }

#if UNITTEST
                    //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "新增用户成功,内容: " + JsonUtil.ToJson(Pro) + "", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-新增用户", "新增名为:" + Pro["p_Name"] + "的项目成功", "");
#else
                    //OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "查询用户列表成功", "");
                    //UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户管理-查询用户列表", "查询成功", "");
#endif
                    ret.Message = "新增项目成功！";
                    ret.AppendData = new { Success = true, Message = ret.Message };
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("BllProject").Error("Add 发生异常", ex);
                if (ex.Message.IndexOf("不能在具有唯一索引") != -1)
                    return new OperationResult(OperationResultType.ParamError, "项目已存在", new { Success = false, Message = "项目已存在" });
                else
                    return new OperationResult(OperationResultType.ParamError, "新增失败", new { Success = false, Message = "新增项目失败" });
            }
            return ret;
        }
        internal static OperationResult Update(Projects model, string type)
        {
            try
            {
                PublicHelper.CheckArgument(model, "model");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("BllProject").Error("Update传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                //判断是否重复
                int count = DataBll.GetCount("t_Projects", "p_UnitId='" + model.P_UnitId + "' and p_Name='" + model.P_Name + "'");
                if (count > 0)
                {
                    ret.Message = "该项目已存在";
                    ret.ResultType = OperationResultType.Error;
                    return ret;
                }
                using (var tran = new TransactionScope())
                {               
                    if (model.P_UnitId == "-1")
                    {
#if UNITTEST
                        Pro["p_UnitId"] = 1;//ServiceBase.GetInfo(ServiceBase.UNITID);
                        Pro["p_UnitName"] = "泰坦软件研发部";//ServiceBase.GetInfo(ServiceBase.UNITFULLNAME);
#else
                        //Pro["p_UnitId"] = ServiceBase.GetInfo(ServiceBase.UNITID);
                        //Pro["p_UnitName"] = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME);
#endif
                    }
                    if (string.IsNullOrEmpty(model.P_DataTime))
                    {
                        model.P_DataTime = DateTime.Now.ToString();
                    }
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    Dictionary<string, object> dicwhere = new Dictionary<string, object>();
                    dicwhere.Add("p_Id", model.P_Id);
                    dic = EntityToDictionary<Projects>(model);
                    dic.Remove("P_Id");    
                    bool brest = DataBll.Update(dic, "t_projects", dicwhere, model.P_Id);
         
                    if (brest)
                    {
                        ret.Message = "修改项目成功！";
                        ret.AppendData = new { Success = true, Message = ret.Message };
                    }
                    else
                    {
                        ret.Message = "修改项目失败！";
                        ret.AppendData = new { Success = false, Message = ret.Message };
                    }

#if UNITTEST
                    //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "新增用户成功,内容: " + JsonUtil.ToJson(Pro) + "", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-新增用户", "新增名为:" + Pro["p_Name"] + "的项目成功", "");
#else
                    //OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "查询用户列表成功", "");
                    //UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户管理-查询用户列表", "查询成功", "");
#endif
                    ret.Message = "修改项目成功！";
                    ret.AppendData = new { Success = true, Message = ret.Message };
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("BllProject").Error("Edit 发生异常", ex);
                if (ex.Message.IndexOf("不能在具有唯一索引") != -1)
                    return new OperationResult(OperationResultType.ParamError, "项目已存在", new { Success = false, Message = "项目已存在" });
                else
                    return new OperationResult(OperationResultType.ParamError, "新增失败", new { Success = false, Message = "新增项目失败" });
            }
            return ret;
        }
        /// <summary>
        /// 实体转键值对
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> EntityToDictionary<T>(T obj) where T : class
        {
            //初始化定义一个键值对，注意最后的括号
            Dictionary<string, object> dic = new Dictionary<string, object>();
            //返回当前 Type 的所有公共属性Property集合
            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo p in props)
            {
                var property = obj.GetType().GetProperty(p.Name);//获取property对象
                var value = p.GetValue(obj, null);//获取属性值
                dic.Add(p.Name, valueOf(value));
            }
            return dic;
        }
        public static String valueOf(Object obj)
        {
            return (obj == null) ? "null" : obj.ToString();
        }
    }
}
