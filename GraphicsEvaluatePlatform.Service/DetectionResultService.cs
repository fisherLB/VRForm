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
  public  class DetectionResultService
    {
        /// <summary>
        /// 获取检测设置列表（and查询功能）
        /// </summary>
        /// <param name="pager">分页参数</param>
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

            //定义返回值
            OperationResult ret = new OperationResult(OperationResultType.Success);

            try
            {
                using (var tran = new TransactionScope())
                {

                    var unitid = "";
                    var progectid = "";
                    string[] arrFilters = pager.filter.Split(',');
                    unitid = arrFilters[1].Split(':')[1];//单位id
                    progectid = arrFilters[2].Split(':')[1];//项目id
                    pager.filter = arrFilters[0];//keyword，搜索输入的关键字
#if UNITTEST
                    var userType = 1;
                    string fullPath = "泰坦研发部";
#else
                    var userType = ServiceBase.GetInfo(ServiceBase.USERTYPE);
                    string fullPath = ServiceBase.GetInfo(ServiceBase.UNITFULLPATH).ToString();
#endif                                                         
                    //页数和行数
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

                    //拼接查询条件
                    string where = "";
                    //string sqlWhere = (unitid == "-1" ? ")" + where : ("and UnitId ='" + unitid + "')"));
                    //var sql = "select * from ( select row_number() over (order by p_DataTime desc) rownumber , pro.*,ut.u_Name from t_projects pro left join t_Units ut on pro.p_unitId = ut.u_id where " + sqlWhere + " ) ret where ret.rownumber > " + startIndex + " and ret.rownumber<=" + endIndex + " order by ret.rownumber asc";
                    var sql = "select * from (select row_number() over (order by FileName desc) rownumber , dr.* from t_DetectionResult dr where  ProjectId = '"+progectid+"') ret where ret.rownumber > " + startIndex + " and ret.rownumber<=" + endIndex + " order by ret.rownumber asc";
                    var retft = DataBll.Query(sql).Tables[0];
                    retft = DataTrim.DataTableTrim(retft);//去空格
                    var count = DataBll.GetCount("t_DetectionResult", (unitid == "-1" ? "" + where : ("UnitId = '" + unitid + "' and ProjectId = '" + progectid +"'")));

                    ret.Message = "查询成功";
                    ret.AppendData = new
                    {
                        total = count,
                        rows = retft
                    };

#if UNITTEST
                    //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "查询用户列表成功", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-查询用户列表", "查询成功", "");
#else
                    //用户日志和操作日志
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "查询检测结果列表成功", "");
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "检测结果管理-查询检测结果列表", "查询成功", "");
#endif
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("DetectionResultService").Error("GetList 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetList 发生异常", ex.Message);
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
        public static OperationResult Export(string filePath, string unitId,string projectId, string value, string ids, int rows)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                dic.Add("FileName", "文件名称");
                dic.Add("ArchivalCode", "档号");
                dic.Add("SuffixName", "后缀名");
                dic.Add("Category", "分类名称");
                dic.Add("UnitName", "单位名称");
                dic.Add("ProjectName", "项目名称");
                dic.Add("DetectionState", "检测状态");
                dic.Add("PageNumber", "不通过页数");
                dic.Add("NamingRules", "命名规则是否通过");
                dic.Add("Watermark", "水印是否通过");
                dic.Add("Resolution", "分辨率是否通过");
                dic.Add("FileSize", "文件大小是否通过");
                dic.Add("Correction", "纠偏是否通过");
                BootstrapPager pager = new BootstrapPager();
                //pager.PageIndex = 0;
                //pager.PageSize = rows;
                if (ids != "" && ids != null)
                {
                    pager.filter = ids;
                }
                else
                    pager.filter = value + "," + unitId + "," + projectId;
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
                                pager.filter = value + "," + unitId + "," + projectId;
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
                Logger.GetLogger("DetectionResultService").Error("Export 发生异常", ex);
                return new OperationResult(OperationResultType.Error, "写文件 错误" + ex);
            }
        }
        /// <summary>
        /// 获取检测结果列表
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
                Logger.GetLogger("DetectionResultService").Error("GetListForExport 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {                 
                    string unitid = "";
                    string projectid = "";                 
                    string ids = "";
                    string[] filters = pager.filter.Split(',');
                    var wherestr = "";
                    var ustype = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString();
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
                    else if (filters.Length > 1)
                    {
                        unitid = filters[1];
                        projectid = filters[2];
                        if (pager.filter != "")//查询关键字
                        {
                            wherestr = "(FileName " + (pager.filter.Contains('%') ? "like '" + pager.filter + "'" : "= '" + pager.filter + "'") + ")";
                        }

                        //权限
                        if (unitid != "-1")
                        {
                            wherestr += ((wherestr == "" ? "" : " and ") + " UnitId = '" + unitid + "'");
                            wherestr += ((projectid == "" ? "":" and projectId ='" + projectid + "'"));
                        }
                        else
                        {
                            if (ustype == "1")//超级管理员
                            {
                                wherestr += "";

                            }
                            else if (ustype == "2")//一般管理员
                            {
                                var UnitName = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString();
                                var unitids = DataBll.Query("select u_id from t_units where u_FullPath='" + UnitName + "'"); ;
                                var idstr = "";
                                foreach (DataRow iditem in unitids.Tables[0].Rows)
                                    idstr += "'" + iditem[0].ToString() + "',";
                                idstr = idstr.Substring(0, idstr.Length - 1);
                                wherestr += ((wherestr == "" ? "" : " and ") + " UnitId in (" + idstr + ")");
                                wherestr += ((projectid == "" ? "" : " and projectId ='" + projectid + "'"));
                            }
                            else//一般操作员
                            {
                                var UnitID = ServiceBase.GetInfo(ServiceBase.UNITID).ToString();
                                wherestr += ((wherestr == "" ? "" : " and ") + " UnitId  = '" + UnitID + "'");

                            }
                        }
                    }

                    //超级管理员查询所有人员，不是超级管理员只查询本机构和下级机构的人员                   
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
                    var datas = DataBll.GetDataSetList("t_DetectionResult", pager.PageSize, pager.PageIndex, "FileName,ArchivalCode,SuffixName,Category,UnitName,ProjectName,DetectionState,PageNumber,NamingRules,Watermark,Resolution,FileSize,Correction", wherestr, "FileName desc", "Dr_id");
                    ret.Message = "查询成功";
                    ret.AppendData = datas;
#if UNITTEST
                    //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "查询用户列表成功", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-查询用户列表", "查询成功", "");
#else
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "查询检测结果列表成功", "");
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "检测结果管理-查询检测结果列表", "查询成功", "");
#endif
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("DetectionResultService").Error("GetListForExport 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetListForExport 发生异常", ex.Message);
            }
            return ret;
        }
    }
}
