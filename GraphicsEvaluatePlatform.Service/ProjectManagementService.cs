//#define UNITTEST
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GraphicsEvaluatePlatform.Service
{
    public static class ProjectManagementService
    {
        /// <summary>
        /// 获取项目列表
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
                Logger.GetLogger("ProjectManagementService").Error("GetList 发生异常", ex);
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
                    var userType = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString().Trim();
                    string fullPath = ServiceBase.GetInfo(ServiceBase.UNITFULLPATH).ToString();
#endif
                    string unitWhere = "";
                    //搜索关键字
                    if (pager.filter != "")
                    {
                        unitWhere = "Us_account like '%" + pager.filter + "%' or Us_name like '%" + pager.filter + "%'";
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
                            unitWhere += ((unitWhere == "" ? "" : " and ") + " p_UnitId in (" + idstr + ")");//本级和下级的机构id
                        }
                        else
                        {//一般操作员（获取本单位数据）
                            var UnitID = ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim();
                            unitWhere += ((unitWhere == "" ? "" : " and ") + " p_UnitId = '" + UnitID + "'");
                        }
                    }
                    else
                    {//选择某一单位
                        unitWhere += ((unitWhere == "" ? "" : " and ") + " p_UnitId = '" + unitid + "'");
                    }

                    var datas = DataBll.GetDataSetList("t_Projects", pager.PageSize, pager.PageIndex, "", unitWhere, "p_DataTime desc", "p_Id");//分页
                    //string sql = "select * from t_Users where" + unitWhere;//不分页
                    //DataSet datas = DataBll.Query(sql);
                    var count = DataBll.GetCount("t_Projects", unitWhere);

                    ret.Message = "查询成功";
                    ret.AppendData = new { total = count, rows = DataTrim.DataTableTrim(datas.Tables[0]) };
                    //string where = "";
                    //if (userType.ToString() != "1")
                    //{
                    //    //不是超级管理员，获取本机构和下级机构的机构Id
                    //    DataSet uds = DataBll.Query("select u_id from t_units where u_FullPath='" + fullPath + "'");
                    //    if (uds.Tables[0].Rows.Count > 0)
                    //    {
                    //        where = "and p_UnitId in (";
                    //        foreach (DataRow dr in uds.Tables[0].Rows)
                    //        {
                    //            where += Convert.ToInt32(dr["u_Id"].ToString()) + ",";
                    //        }
                    //        where = where.TrimEnd(',');
                    //        where += ")";
                    //    }

                    //}
                    ////超级管理员查询所有人员，不是超级管理员只查询本机构和下级机构的人员

                    ////  var where = "and p_untid in ('"+unitid+"')";
                    //var startIndex = 1;
                    //var endIndex = 1;
                    //if (pager.PageSize < 1)
                    //{
                    //    pager.PageSize = 10;
                    //}
                    //if (pager.PageIndex < 1)
                    //{
                    //    pager.PageIndex = 1;
                    //}
                    //startIndex = (pager.PageIndex - 1) * pager.PageSize;
                    //endIndex = pager.PageIndex * pager.PageSize;

                    //string sqlWhere = "(p_Name like '%" + pager.filter + "%'" + (unitid == "-1" ? ")" + where : ("and p_UnitId ='" + unitid + "')"));
                    //var sql = "select * from ( select row_number() over (order by p_DataTime desc) rownumber , pro.*,ut.u_Name from t_projects pro left join t_Units ut on pro.p_unitId = ut.u_id where " + sqlWhere + " ) ret where ret.rownumber > " + startIndex + " and ret.rownumber<=" + endIndex + " order by ret.rownumber asc";
                    //var retft = DataBll.Query(sql).Tables[0];
                    //retft = DataTrim.DataTableTrim(retft);//去空格
                    //var count = DataBll.GetCount("t_projects", "(p_name like '%" + pager.filter + "%') " + (unitid == "-1" ? "" + where : ("and p_unitId = '" + unitid + "'")));  

                    //ret.Message = "查询成功";
                    //ret.AppendData = new
                    //{
                    //    total = count,
                    //    rows = retft
                    //};

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

        /// <summary>
        /// 获取单位下的项目列表
        /// </summary>
        /// <param name="unitId">单位id</param>
        /// <returns></returns>
        public static OperationResult getProgectOfUnit(string unitId)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(unitId, "unitId");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectManageService").Error("getProgectOfUnit 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                string where = "";
                if (unitId == "-1")//选择全部
                {
                    var ustype = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString();//当前登录用户类型
                    if (ustype == "1")//超级管理员
                    {
                        where = "";
                    }
                    else if (ustype == "2")//一般管理员
                    {
                        var UnitName = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString();
                        var unitids = DataBll.Query("select u_id from t_units where u_FullPath='" + UnitName + "'"); ;
                        var idstr = "";
                        foreach (DataRow iditem in unitids.Tables[0].Rows)
                            idstr += "'" + iditem[0].ToString() + "',";
                        idstr = idstr.Substring(0, idstr.Length - 1);
                        where = " where p_UnitId in (" + idstr + ")";
                       
                    }
                    else//一般操作员
                    {
                        var UnitID = ServiceBase.GetInfo(ServiceBase.UNITID).ToString();
                        where = " where  p_UnitId  = '" + UnitID + "'";
                    }
                }
                else {//选择某个单位
                    where = " where p_UnitId = '" + unitId + "'";
                }
                string sql = "select p_Id,p_Name from t_Projects " + where;
                DataTable dt = DataBll.Query(sql).Tables[0];
                dt = DataTrim.DataTableTrim(dt);
                ret.Message = "查询成功";
                ret.AppendData = new
                {
                    rows = dt
                };
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectManageService").Error("getProgectOfUnit 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "getProgectOfUnit 发生异常", ex.Message);
            }
            return ret;
        }

        /// <summary>
        /// 获取详细的条目信息
        /// </summary>
        /// <param name="id">条目id</param>
        /// <returns></returns>
        public static OperationResult GetDetail(string id)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(id, "id");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectManageService").Error("GetDetail 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    var sql = "select * from t_Projects where p_id=" + id;
                    var retft = DataBll.Query(sql).Tables[0];
                    retft = DataTrim.DataTableTrim(retft);

                    ret.Message = "查询成功";
                    ret.AppendData = new
                    {
                        total = 1,
                        rows = retft
                    };

#if UNITTEST
                    //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "查询用户列表成功", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-查询用户列表", "查询成功", "");
#else
                    //OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "查询客户端列表成功", "");
                    //UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "客户端管理-查询客户端列表", "查询成功", "");
#endif
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectManageService").Error("GetDetail 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetDetail 发生异常", ex.Message);
            }
            return ret;
        }

        /// <summary>
        /// 新增项目
        /// </summary>
        /// <param name="Pro"></param>
        /// <returns></returns>
        public static OperationResult Add(Dictionary<string, object> Pro)
        {
            try
            {
                PublicHelper.CheckArgument(Pro, "Pro");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectManagementService").Error("Add 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                //判断是否重复
                int count = DataBll.GetCount("t_Projects", "p_UnitId='" + Pro["p_UnitId"] + "' and p_Name='" + Pro["p_Name"] + "'");
                if (count > 0)
                {
                    ret.Message = "该项目已存在";
                    ret.ResultType = OperationResultType.Error;
                    return ret;
                }

                using (var tran = new TransactionScope())
                {
                    // Pro.Remove("p_Id");                    
                    if (Pro["p_UnitId"].ToString() == "-1")
                    {
#if UNITTEST
                        Pro["p_UnitId"] = 1;//ServiceBase.GetInfo(ServiceBase.UNITID);
                        Pro["p_UnitName"] = "泰坦软件研发部";//ServiceBase.GetInfo(ServiceBase.UNITFULLNAME);
#else
                        //Pro["p_UnitId"] = ServiceBase.GetInfo(ServiceBase.UNITID);
                        //Pro["p_UnitName"] = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME);
#endif
                    }
                    if (!Pro.ContainsKey("p_Id"))
                    {
                        Pro.Add("p_Id", Guid.NewGuid());
                    }
                    else
                    {
                        Pro["p_Id"] = Guid.NewGuid();
                    }
                    if (!Pro.ContainsKey("p_DataTime"))
                    {
                        Pro.Add("p_DataTime", DateTime.Now);
                    }
                    else
                    {
                        Pro["p_DataTime"] =DateTime.Now;
                    }
                    //Pro["p_UnitId"] = 1;//ServiceBase.GetInfo(ServiceBase.UNITID);
                    //Pro["p_UnitName"] = "泰坦软件研发部";//ServiceBase.GetInfo(ServiceBase.UNITFULLNAME);
             
                    var dsret = DataBll.Add(Pro, "t_Projects");
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
                Logger.GetLogger("ProjectManagementService").Error("Add 发生异常", ex);
                if (ex.Message.IndexOf("不能在具有唯一索引") != -1)
                    return new OperationResult(OperationResultType.ParamError, "项目已存在", new { Success = false, Message = "项目已存在" });
                else
                    return new OperationResult(OperationResultType.ParamError, "新增失败", new { Success = false, Message = "新增项目失败" });
            }
            return ret;
        }
  
        /// <summary>
        /// 更改项目
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
                Logger.GetLogger("Projects").Error("Update 传入参数异常", ex);
            }
            var ret = new OperationResult(OperationResultType.Success);
            object re = new object();
            try
            {
                using (var tran = new TransactionScope())
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("p_Id", Pro["p_Id"]);
                    bool issuc = DataBll.Update(Pro, "t_projects", dic, "p_Id");

#if UNITTEST
                  //  OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "更新ID为: " + Pro["p_Id"] + " 的项目成功", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-更新用户信息", "更新ID为: " + Pro["p_Id"] + " 的项目成功", "");
#else
                    //OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "更新ID为: " + Pro["p_Id"] + " 的项目成功", "");
                    //UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户管理-更新用户信息", "更新ID为: " + Pro["p_Id"] + " 的项目成功", "");
#endif
                    if (issuc)
                    {
                        ret.Message = "编辑项目成功";
                    }
                    else
                    {
                        ret.ResultType = OperationResultType.Error;
                        ret.Message = "编辑项目失败";
                    }
                    tran.Complete();
                }
                return ret;
            }
            catch (Exception ex)
            {
                ret.ResultType = OperationResultType.Error;
                ret.Message = "编辑项目异常";
                Logger.GetLogger("ProjectManageService").Error("Message:" + ex.Message + "Inner:" + ex.InnerException);
            }
            return ret;
        }
            
        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="ids">删除的项目id</param>
        /// <returns></returns>
        public static OperationResult Delete(string ids)
        {
            try
            {
                PublicHelper.CheckArgument(ids, "ids");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectManagementService").Error("Delete 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            if (ids == "")
            {
                ret = new OperationResult(OperationResultType.ParamError);
                ret.Message = "项目ID错误";
                return ret;
            }
            string[] arrids = ids.Split(',');
            if (arrids.Length == 0)
            {
                ret = new OperationResult(OperationResultType.ParamError);
                ret.Message = "项目ID错误";
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
                        dicCondition.Add("p_id", item);
                        //删除用户记录
                        DataBll.Delete(dicCondition, "t_Projects");

#if UNITTEST
                        //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "删除ID为: " + item + "的用户成功", "");
                        //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-删除用户", "删除ID为: " + item + "的用户成功", "");
#else
                        //OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "删除ID为: " + item + "的项目成功", "");
                        //UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "项目管理-删除项目", "删除ID为: " + item + "的项目成功", "");
#endif
                    }
                    tran.Complete();
                }
                ret = new OperationResult(OperationResultType.Success);
                ret.Message = "删除项目信息成功";
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectManagementService").Error("Delete 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "Delete 发生异常", ex);
            }
            return ret;
        }

        /// <summary>
        /// 启用，禁用项目
        /// </summary>
        /// <param name="ids">条目id</param>
        /// <param name="values">1:启用，0：禁用</param>
        /// <returns></returns>
        public static OperationResult ActiveProjects(string ids, string values)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                PublicHelper.CheckArgument("ids", ids);
                PublicHelper.CheckArgument("values", values);
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectManageService").Error("ActiveProjects发生异常", ex);
                return new OperationResult(OperationResultType.Error, "ClientsManageService的方法 ActiveProjects参数有误!");
            }
            try
            {
                StringBuilder sbSql = new StringBuilder();
                string[] arrIds = ids.Split(',');
                string[] arrValues = values.Split(',');
                for (int i = 0; i < arrIds.Length; i++)
                {
                    sbSql.Append(";Update t_Projects set p_IsEnabled='" + arrValues[i] + "' where p_Id='" + arrIds[i] + "';");
                }
                using (var tran = new TransactionScope())
                {
                    DataBll.Query(sbSql.ToString());
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectManageService").Error(" ActiveProjects发生异常", ex);
                return new OperationResult(OperationResultType.Error, "ProjectsManageService的方法 ActiveProjects参数有误!");
            }
            return ret;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="unitId"></param>
        /// <param name="value"></param>
        /// <param name="ids"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static OperationResult Export(string filePath, string unitId, string value, string ids, int rows)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                dic.Add("p_Id", "项目Id");
                dic.Add("p_UnitId", "机构Id");
                dic.Add("p_UnitName", "机构名称");
                dic.Add("p_Name", "项目名称");
                dic.Add("p_DataTime", "创建时间");
                dic.Add("p_Region", "地区");
                dic.Add("p_Contactor", "负责人");
                dic.Add("p_DataSize", "数据大小");
                dic.Add("p_Remarks", "备注·");
                BootstrapPager pager = new BootstrapPager();
                pager.PageIndex = 0;
                pager.PageSize = rows;
                if (ids != "" && ids != null)
                {
                    pager.filter = ids;
                }
                else
                    pager.filter = value + "," + unitId;
                using (FileStream fw = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fw, Encoding.Default))
                    {
                        bool ishand = true;
                        do
                        {
                            pager.PageIndex++;
                            if (ids == "" || ids == null)
                            {
                                pager.filter = value + "," + unitId;
                            }

                            DataTable dt = ((DataSet)GetListForExport(pager).AppendData).Tables[0];
                            dt = DataTrim.DataTableTrim(dt);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (ishand)
                                {
                                    string s = string.Empty;
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        string name = dt.Columns[i].ColumnName;
                                        if (dic.ContainsKey(name))
                                        {

                                        s += dic[name].ToString() + ",";
                                        }
                                    }
                                    s = s.Substring(0, s.Length - 1);
                                    sw.WriteLine(s);
                                    ishand = false;
                                }
                                foreach (DataRow row in dt.Rows)
                                {
                                    string s = string.Empty;
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        s += row[dt.Columns[i].ColumnName].ToString() + ",";
                                    }
                                    s = s.Substring(0, s.Length - 1);
                                    sw.WriteLine(s);
                                }
                            }
                            else
                                break;
                            sw.Flush();
                            fw.Flush();

                        } while (true);
                        return ret;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectManageService").Error("Export 发生异常", ex);
                return new OperationResult(OperationResultType.Error, "写文件 错误" + ex);
            }
        }
        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public static OperationResult GetListForExport(BootstrapPager pager)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(pager, "pager");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ProjectManageService").Error("GetListForExport 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    //                    var unitid = "";
                    //                    var filters = pager.filter.Split(',');
                    //                    unitid = filters[1].Split(':')[1];
                    //                    pager.filter = filters[0];
                    //#if UNITTEST
                    //                    var userType = 1;
                    //                    string fullPath = "泰坦研发部";
                    //#else
                    //                    //var userType = ServiceBase.GetInfo(ServiceBase.USERTYPE);
                    //                    //string fullPath = ServiceBase.GetInfo(ServiceBase.UNITFULLPATH).ToString();
                    //#endif
                    //                    var userType = 1;
                    //                    string fullPath = "泰坦研发部";
                    //                    string where = "";
                    //                    if (userType.ToString() != "1")
                    //                    {
                    //                        //不是超级管理员，获取本机构和下级机构的机构Id
                    //                        DataSet uds = DataBll.Query("select u_id from t_units where u_FullPath='" + fullPath + "'");
                    //                        if (uds.Tables[0].Rows.Count > 0)
                    //                        {
                    //                            where = "and p_UnitId in (";
                    //                            foreach (DataRow dr in uds.Tables[0].Rows)
                    //                            {
                    //                                where += Convert.ToInt32(dr["u_Id"].ToString()) + ",";
                    //                            }
                    //                            where = where.TrimEnd(',');
                    //                            where += ")";
                    //                        }

                    //                    }

                    string unitid = "";
                    var ustype = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString();
                    string ids = "";
                    string[] filters = pager.filter.Split(',');
                    var wherestr = "";
                    pager.filter = filters[0];
                    if (filters.Length == 1)
                    {
                        string[] s = filters[0].Split('|');
                        for (int i = 0; i < s.Length; i++)
                        {
                            ids += "'" + s[i].Trim() + "',";
                        }

                        ids = ids.Substring(0, ids.Length - 1);

                        wherestr = "(p_id  in (" + ids + "))";

                    }
                    else if (filters.Length == 2)
                    {


                        unitid = filters[1];
                        if (pager.filter != "")
                        {
                            wherestr = "(p_name " + (pager.filter.Contains('%') ? "like '" + pager.filter + "'" : "= '" + pager.filter + "'") + ")";
                        }

                        if (unitid != "-1")
                        {
                            wherestr += ((wherestr == "" ? "" : " and ") + " p_UnitID = '" + unitid + "'");
                        }
                        else
                        {
                            if (ustype == "1")
                            {
                                wherestr += "";

                            }
                            else if (ustype == "2")
                            {
                             var UnitName = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString();
                                var unitids = DataBll.Query("select u_id from t_units where u_FullPath='" + UnitName + "'"); ;//= DataBll.Query("select Unit_ID from UnitTable where FullPath = '" + UnitName + "' or FullPath like '%" + UnitName + "-%' or FullPath like '%-" + UnitName + "'");
                                var idstr = "";
                                foreach (DataRow iditem in unitids.Tables[0].Rows)
                                    idstr += "'" + iditem[0].ToString() + "',";

                                idstr = idstr.Substring(0, idstr.Length - 1);

                                wherestr += ((wherestr == "" ? "" : " and ") + " p_UnitID in (" + idstr + ")");

                                //  DataSet uds = DataBll.Query("select u_id from t_units where u_FullPath='" + fullPath + "'");
                    //                        if (uds.Tables[0].Rows.Count > 0)
                    //                        {
                    //                            where = "and p_UnitId in (";
                    //                            foreach (DataRow dr in uds.Tables[0].Rows)
                    //                            {
                    //                                where += Convert.ToInt32(dr["u_Id"].ToString()) + ",";
                    //                            }
                    //                            where = where.TrimEnd(',');
                    //                            where += ")";

                            }
                            else
                            {
                                var UnitID = ServiceBase.GetInfo(ServiceBase.UNITID).ToString();
                                wherestr += " and UnitID = '" + UnitID + "'";
                            }
                        }
                    }


                    //超级管理员查询所有人员，不是超级管理员只查询本机构和下级机构的人员

                    //  var where = "and p_untid in ('"+unitid+"')";
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
                    var datas = DataBll.GetDataSetList("t_projects", pager.PageSize, pager.PageIndex, "p_UnitName,p_Name,p_Region,p_Contactor,p_DataSize,p_Remarks", wherestr, "p_name desc", "p_id");

                    //string sqlWhere = "(p_Name like '%" + pager.filter + "%'" + (unitid == "-1" ? ")" + where : ("and p_UnitId ='" + unitid + "')"));
                    //var sql = "select * from ( select row_number() over (order by p_DataTime desc) rownumber , pro.*,ut.u_Name from t_projects pro left join t_Units ut on pro.p_unitId = ut.u_id where " + sqlWhere + " ) ret where ret.rownumber > " + startIndex + " and ret.rownumber<=" + endIndex + " order by ret.rownumber asc";
                    //var retft = DataBll.Query(sql);
                    //var count = DataBll.GetCount("t_projects", "(p_name like '%" + pager.filter + "%') " + (unitid == "-1" ? "" + where : ("and p_unitId = '" + unitid + "'")));
                    ret.Message = "查询成功";
                    ret.AppendData = datas;
                    //ret.AppendData = new
                    //{
                    //    total = count,
                    //    rows = retft.Tables[0]
                    //};

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
                Logger.GetLogger("ProjectManageService").Error("GetListForExport 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetListForExport 发生异常", ex.Message);
            }
            return ret;
        }
      
    }
}
