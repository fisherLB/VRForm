using GraphicsEvaluatePlatform.Client.Models;
using GraphicsEvaluatePlatform.Client.Repository;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GraphicsEvaluatePlatform.Client.Services
{
    public class ProjectService : IProjectService
    {
        public OperationResult Del(string  id)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            Dictionary<string, object> dic= new Dictionary<string, object>();
            dic.Add("p_Id", id);
           bool bl= DataBll.Delete(dic, "t_projects");
            if (bl)
            {
                ret.Message = "删除成功！";
            }
            else
            {
                ret.Message = "删除失败！";
                ret.ResultType = OperationResultType.Error;
            }
            return ret;
        }

        public Dictionary<string, object> EntityToDictionary<T>(T obj)
        {
            //初始化定义一个键值对，注意最后的括号
            Dictionary<string, object> dic = new Dictionary<string, object>();
            //返回当前 Type 的所有公共属性Property集合
            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo p in props)
            {
                var property = obj.GetType().GetProperty(p.Name);//获取property对象
                var value = p.GetValue(obj, null);//获取属性值
                dic.Add(p.Name, ValueOf(value));
            }
            return dic;
        }

        public OperationResult GetDetail(string id)
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
                Logger.GetLogger("ProjectService").Error("GetDetail 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetDetail 发生异常", ex.Message);
            }
            return ret;
        }

        public List<Project> GetList()
        {
            List<Project> projectList = new List<Project>();
            //   string xmlFileName = System.IO.Path.Combine(Environment.CurrentDirectory, @"Data\Data.xml");
            // XDocument xDoc = XDocument.Load(xmlFileName);
            var projects = new DataTable();// xDoc.Descendants("Project");
            foreach (DataRow d in projects.Rows)
            {
                Project project = new Project();
                project.P_Name = d["p_Name"].ToString();
                project.P_Region = d["p_Region"].ToString();
                project.P_Contactor = d["p_Contactor"].ToString();
                project.P_Remarks = d["p_Remarks"].ToString();
                projectList.Add(project);
            }
            return projectList;
        }
        public OperationResult GetList( int pageNum, int currentlyPage,Dictionary<string,object>dic)
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
                Logger.GetLogger("ProjectService").Error("GetList 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    string where = " ";
                    if (dic.ContainsKey("unitId"))
                    {
                        where ="p_unitId='"  + dic["unitId"].ToString()+ "' ";
                    }
   

                    var ds = DataBll.GetDataSetList("t_projects", pageNum, currentlyPage, "id,projectid,projectname,name,path,size,type,status,remarks", where, " name asc", "name");
                    var total = DataBll.GetCount("t_projects", where);
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
                Logger.GetLogger("ProjectService").Error("GetList 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetList 发生异常", ex.Message);
            }
            return ret;
        }
        public OperationResult Save(Project model)
        {
            try
            {
                PublicHelper.CheckArgument(model, "model");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectService").Error("Add 传入参数异常", ex);
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
                 //   if (model.P_UnitId == "-1")
                    {
#if UNITTEST
                        Pro["p_UnitId"] = 1;//ServiceBase.GetInfo(ServiceBase.UNITID);
                        Pro["p_UnitName"] = "泰坦软件研发部";//ServiceBase.GetInfo(ServiceBase.UNITFULLNAME);
#else
                        //Pro["p_UnitId"] = ServiceBase.GetInfo(ServiceBase.UNITID);
                        //Pro["p_UnitName"] = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME);
#endif
                    }
          
                        model.P_Id = Guid.NewGuid();
        
        
                        model.P_DataTime = DateTime.Now;
                    
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    Dictionary<string, object> dicwhere = new Dictionary<string, object>();
                    dicwhere.Add("p_Id", model.P_Id);
                    dic = EntityToDictionary<Project>(model);
                    DataSet dsret = DataBll.Add(dic, "t_projects");

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
                Logger.GetLogger("ProjectService").Error("Add 发生异常", ex);
                if (ex.Message.IndexOf("不能在具有唯一索引") != -1)
                    return new OperationResult(OperationResultType.ParamError, "项目已存在", new { Success = false, Message = "项目已存在" });
                else
                    return new OperationResult(OperationResultType.ParamError, "新增失败", new { Success = false, Message = "新增项目失败" });
            }
            return ret;
        }

        public OperationResult Search(Project model)
        {
            throw new NotImplementedException();
        }

        public OperationResult Update(Project model)
        {
            try
            {
                PublicHelper.CheckArgument(model, "model");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectService").Error("Update传入参数异常", ex);
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
                    //if (model.P_UnitId == "-1")
                    {
#if UNITTEST
                        Pro["p_UnitId"] = 1;//ServiceBase.GetInfo(ServiceBase.UNITID);
                        Pro["p_UnitName"] = "泰坦软件研发部";//ServiceBase.GetInfo(ServiceBase.UNITFULLNAME);
#else
                        //Pro["p_UnitId"] = ServiceBase.GetInfo(ServiceBase.UNITID);
                        //Pro["p_UnitName"] = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME);
#endif
                    }
                   // if (string.IsNullOrEmpty(model.P_DataTime))
                    {
                        model.P_DataTime = DateTime.Now;
                    }
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    Dictionary<string, object> dicwhere = new Dictionary<string, object>();
                    dicwhere.Add("p_Id", model.P_Id);
                    dic = EntityToDictionary<Project>(model);
                    dic.Remove("P_Id");
                    bool brest = DataBll.Update(dic, "t_projects", dicwhere, model.P_Id.ToString());

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
                Logger.GetLogger("ProjectService").Error("Edit 发生异常", ex);
                if (ex.Message.IndexOf("不能在具有唯一索引") != -1)
                    return new OperationResult(OperationResultType.ParamError, "项目已存在", new { Success = false, Message = "项目已存在" });
                else
                    return new OperationResult(OperationResultType.ParamError, "新增失败", new { Success = false, Message = "新增项目失败" });
            }
            return ret;
        }

        public string ValueOf(object obj)
        {
            return (obj == null) ? "null" : obj.ToString();
        }
    }
}
