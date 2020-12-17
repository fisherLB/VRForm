//#define UNITTEST

/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2017. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using System.Data;
using System.IO;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using System.Transactions;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Service
 * 项目描述: 
 * 类 名 称: UserLogService
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: 
 * 命名空间: GraphicsEvaluatePlatform.Service
 * 文件名称: UserLogService
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/4/20 18:50:32
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Service
{
    public static class UserLogService
    {

        public static OperationResult GetUserLogList(BootstrapPager pager)
        {
            try
            {
                PublicHelper.CheckArgument(pager, "pager");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserLogService").Error("GetUserLogList 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }

            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                var unitid = "";
                var filters = pager.filter.Split(',');
                unitid = filters[1].Split(':')[1];
                pager.filter = filters[0];

                var ustype = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString().Trim();
                var wherestr = "";

                if (pager.filter != "")
                {
                    wherestr = "(Us_name " + (pager.filter.Contains('%') ? "like '" + pager.filter + "'" : "= '" + pager.filter + "'") + "or Us_account " + (pager.filter.Contains('%') ? "like '" + pager.filter + "'" : "= '" + pager.filter + "'") + ")";
                }

                if (unitid != "-1")
                {
                    wherestr += ((wherestr == "" ? "" : " and ") + " Ul_UnitID = '" + unitid + "'");
                }
                else
                {
                    if (ustype == "1")
                    {
                        wherestr += "";

                    }
                    else if (ustype == "2")
                    {
                        var UnitName = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString().Trim();
                        var unitids = DataBll.Query("select Unit_ID from t_Units where FullPath = '" + UnitName + "' or FullPath like '%" + UnitName + "-%' or FullPath like '%-" + UnitName + "'");
                        var idstr = "";
                        foreach (DataRow iditem in DataTrim.DataTableTrim(unitids.Tables[0]).Rows)
                            idstr += "'" + iditem[0].ToString() + "',";

                        idstr = idstr.Substring(0, idstr.Length - 1);

                        wherestr += ((wherestr == "" ? "" : " and ") + " Ul_UnitID in (" + idstr + ")");
                    }
                    else
                    {
                        var UnitID = ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim();
                        wherestr += " and UnitID = '" + UnitID + "'";
                    }
                }

                var datas = DataBll.GetDataSetList("t_UserLog", pager.PageSize, pager.PageIndex, "", wherestr, "Ul_time desc", "Ul_id");
                var count = DataBll.GetCount("t_UserLog", wherestr);
                ret.Message = "查询成功";
                ret.AppendData = new
                {
                    total = count,
                    rows = DataTrim.DataTableTrim(datas.Tables[0])
                };
                OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim(), ServiceBase.GetIPAddress(), DateTime.Now, "查询用户日志列表成功", "");
                InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString().Trim(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString().Trim(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-日志管理-查询用户日志列表", "查询成功", "");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserLogService").Error("GetUserLogList 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error);
            }
            return ret;
        }

        public static void InsertLog(string account, string name, string ip, DateTime time, string function, string desc, string type)
        {
            Dictionary<string, object> dicValue = new Dictionary<string, object>();
            dicValue.Add("Us_account", account);
            dicValue.Add("Us_name", name);
            dicValue.Add("Ul_ip", ip);
            dicValue.Add("Ul_time", time.ToString("yyyy-MM-dd HH:mm:ss"));
            dicValue.Add("Ul_function", function);
            dicValue.Add("Ul_descript", desc);
#if UNITTEST
            dicValue.Add("Ul_UnitID", "-1");
            dicValue.Add("Ul_UnitName", "");
#else
            dicValue.Add("Ul_UnitID", ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim() == null ? "-1" : ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim());
            dicValue.Add("Ul_UnitName", ServiceBase.GetInfo(ServiceBase.USERINFO).ToString().Trim() == null ? "" : ((Users)ServiceBase.GetInfo(ServiceBase.USERINFO)).Unit_name);
#endif
            DataBll.Add(dicValue, "t_UserLog");
        }

        /// <summary>
        /// 写文件字节流
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="uid"></param>
        /// <param name="cid"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="rowsNum"></param>
        /// <returns></returns>
        public static OperationResult WriteFileStream(string filePath, string unitId, string value, string ids, int rowsNum)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Ul_id", "日志ID");
                dic.Add("Us_account", "用户名");
                dic.Add("Us_name", "真实姓名");
                dic.Add("Ul_ip", "IP地址");
                dic.Add("Ul_time", "操作时间");
                dic.Add("Ul_descript", "操作描述:");
                dic.Add("Ul_type", "操作类型");
                dic.Add("Ul_function", "操作功能");
                dic.Add("Ul_UnitID", "机构ID");
                dic.Add("Ul_UnitName", "机构名称");
                BootstrapPager pager = new BootstrapPager();
                pager.PageIndex = 0;
                pager.PageSize = rowsNum;
                if (ids != null)
                {
                    pager.filter = ids;
                }
                else {

                    pager.filter = value + "," + unitId;
                }
                   
                using (FileStream fw = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fw, Encoding.Default))
                    {
                        bool ishand = true;
                        do
                        {
                            pager.PageIndex++;
                            if (ids == null)
                            {
                                pager.filter = value + "&" + unitId;
                            }

                            DataTable dt = (DataTable)GetUserLogTable(pager).AppendData;
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (ishand)
                                {
                                    string s = string.Empty;
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        string name = dt.Columns[i].ColumnName;
                                        s += dic[name].ToString() + ",";
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
                Logger.GetLogger("UserLogService").Error("WriteFileStream 发生异常", ex);
                return new OperationResult(OperationResultType.Error, "写文件 错误" + ex);
            }
        }
        /// <summary>
        /// 获取用户日志导出数据
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public static OperationResult GetUserLogTable(BootstrapPager pager)
        {
            try
            {
                PublicHelper.CheckArgument(pager, "pager");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserLogService").Error("GetUserLogTable 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }

            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                string unitid = "";
                var ustype = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString().Trim();
                string ids = "";
                string[] filters = pager.filter.Split('&');
                var wherestr = "";
                pager.filter = filters[0];
                if (filters.Length == 1)
                {
                    string[] s = filters[0].Split('|');
                    for (int i = 0; i < s.Length; i++)
                    {
                        ids += "'" + s[i] + "',";
                    }

                    ids = ids.Substring(0, ids.Length - 1);

                    wherestr = "(Ul_id  in (" + ids + "))";

                }
                else if (filters.Length == 2)
                {


                    unitid = filters[1];
                    if (pager.filter != "")
                    {
                        wherestr = "(Us_name " + (pager.filter.Contains('%') ? "like '" + pager.filter + "'" : "= '" + pager.filter + "'") + "or Us_account " + (pager.filter.Contains('%') ? "like '" + pager.filter + "'" : "= '" + pager.filter + "'") + ")";
                    }

                    if (unitid != "-1")
                    {
                        wherestr += ((wherestr == "" ? "" : " and ") + " Ul_UnitID = '" + unitid + "'");
                    }
                    else
                    {
                        if (ustype == "1")
                        {
                            wherestr += "";

                        }
                        else if (ustype == "2")
                        {
                            var UnitName = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString().Trim();
                            var unitids = DataBll.Query("select u_id from t_Units where u_FullPath = '" + UnitName + "' or u_FullPath like '%" + UnitName + "-%' or u_FullPath like '%-" + UnitName + "'");
                            var idstr = "";
                            foreach (DataRow iditem in DataTrim.DataTableTrim(unitids.Tables[0]).Rows)
                                idstr += "'" + iditem[0].ToString() + "',";

                            idstr = idstr.Substring(0, idstr.Length - 1);

                            wherestr += ((wherestr == "" ? "" : " and ") + " Ul_UnitID in (" + idstr + ")");
                        }
                        else
                        {
                            var UnitID = ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim();
                            wherestr += " and UnitID = '" + UnitID + "'";
                        }
                    }
                }
                var datas = DataBll.GetDataSetList("t_UserLog", pager.PageSize, pager.PageIndex, "", wherestr, "Ul_time desc", "Ul_id");

                ret.Message = "查询成功";
                ret.AppendData = DataTrim.DataTableTrim(datas.Tables[0]);
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserLogService").Error("GetUserLogTable 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error);
            }
            return ret;
        }
    }
}
