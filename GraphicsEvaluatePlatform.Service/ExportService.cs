using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace GraphicsEvaluatePlatform.Service
{
    public static class ExportService
    {
        #region 全局静态变量

        public static bool THREADFLAG = false;
        public static string UNITFLAG = "";
        public static string USERFLAG = "";
        public static string CATEFLAG = "";
        static int COUNT = 0;
        static int TOTAL = 0;
        static int ERRORCOUNT = 0;//记录错误信息条数
        static int INSERTCOUNT = 0;//记录插入成功条数
        static int UPDATECOUNT = 0;//记录更新信息条数
        static int REPEATCOUNT = 0;//记录重复信息条数
        public static int STATUS = 0;// -2:结束; -1：出错; 0：运行中。1:暂停/重复；2：结束。
        //static string MESSAGE = "";
        static string FILENAME = string.Empty;
        static bool ISREPLACE = false;
        static bool ISALWAY = false;
        static bool ISCANCEL = false;
        public static Thread THREAD = null;
        #endregion

        #region 导出数据

        #region 导出各式文件   
        /// <summary>
        /// 导出条目记录到Text文本文件
        /// </summary>
        /// <param name="filePath">导出生成的文件路径</param>
        /// <param name="unitId">单位id</param>
        /// <param name="cateId">门类id</param>
        /// <param name="ids">勾选的条目数据</param>
        /// <param name="names">选中导出的字段名称</param>
        /// <param name="codes">选中导出的字段编码</param>
        /// <param name="searchType">查询的方式</param>
        /// <param name="searchCondition">查询的条件</param>
        /// <param name="sheetName">sheet名称</param>
        /// <returns></returns>
        public static OperationResult WriteEntryTextFile(string filePath, string unitId, string cateId, string ids, string names, string codes, string searchType, string searchCondition, string sheetName)
        {
            OperationResult ret = new OperationResult(OperationResultType.Error);
            int sum = 0;
            var dtTableName = new DataTable();/// CategoryService.GetTableName(unitId, cateId).Tables[0];//表名
            if (dtTableName.Rows.Count != 1)
                return new OperationResult(OperationResultType.ParamError, "参数错误,指定分类无对应数据表");
            var tablename = dtTableName.Rows[0]["TableName"].ToString();
            var catefullname = dtTableName.Rows[0]["Cate_full_name"].ToString();
            var SubTable = tablename.Substring(0, 4);
            using (FileStream fw = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fw, Encoding.Default))
                {
                    int page = 1;

                    int rowsNum = 65535;
                    string str = System.Configuration.ConfigurationManager.AppSettings["QueryRows"].ToString();
                    bool ishand = true;
                    do
                    {
                        ret = GetData(unitId, cateId, ids, searchType, searchCondition, rowsNum, page++);
                        DataTable dt = new DataTable();
                        if (ret.ResultType == 0)
                            dt = (DataTable)ret.AppendData;

                        if (ishand)
                        {
                            sw.WriteLine(names);
                            ishand = false;
                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            sum += dt.Rows.Count;
                            string[] arrCodes = codes.Split(',');
                            foreach (DataRow row in dt.Rows)//遍历DataTable的每一行。
                            {
                                string s = string.Empty;
                                for (int i = 0; i < arrCodes.Length; i++)
                                {
                                    if (dt.Columns.Contains(arrCodes[i]))
                                        s += row[arrCodes[i]].ToString() + ",";
                                }
                                if (s.Length > 0)
                                    s = s.Substring(0, s.Length - 1);
                                //Infrastructure.Common.Office.ExcelUtil
                                sw.WriteLine(s);
                            }
                        }
                        else
                            break;
                        sw.Flush();
                        fw.Flush();
                    } while (true);
                }
            }
            if (ret.ResultType == 0)
            {
                string userId = ServiceBase.GetInfo(ServiceBase.USERID).ToString();
                string userName = ServiceBase.GetInfo(ServiceBase.USERNAME).ToString();
                string userUnitCode = "";// UnitManageService.getUnitCode(unitId).AppendData.ToString();
                string userUnitName = "";//ServiceBase.GetInfo(ServiceBase.UNITNAME).ToString();
                string dataUnitCode = "";// UnitManageService.getUnitCode(unitId).AppendData.ToString();//暂时与用户一样
                string dataUnitName = ServiceBase.GetInfo(ServiceBase.UNITNAME).ToString();//暂时与用户一样
                string dir = filePath;
                string year = DateTime.Now.Year.ToString();
                ret = ImportLog(unitId, dataUnitCode, dataUnitName, userUnitName, userName, DateTime.Now, sum, "", year, "导出", "");

#if UNITTEST
                    UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "首页-提交入库", "入库多条记录成功", "");
                    OperationLogService.InsertLog("177A50AE-B82C-4E6C-B2C5-A03F5EB8FBD6", "192.168.1.62", DateTime.Now, "首页-提交入库: 入库多条记录成功", "");
#else

                string logdesc = "将" + catefullname + "的" + sum + "条记录导出到" + filePath + "Excel表格";
                string logtype = "导出条目";

                string logfuntion = "归档管理-导出";
                if (SubTable == "JNGL")
                    logfuntion = "卷内管理-导出";
                else if (SubTable == "AJGL")
                    logfuntion = "案卷管理-导出";
                else if (SubTable == "WJGL")
                    logfuntion = "文件管理-导出";
                else if (SubTable == "ZLGL")
                    logfuntion = "资料管理-导出";
                UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, logfuntion, logdesc, logtype);
                OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, logdesc, logtype);
#endif
            }
            return ret;
        }
        /// <summary>
        /// 导出条目记录到Excel
        /// </summary>
        /// <param name="filePath">导出生成的文件路径</param>
        /// <param name="unitId">单位id</param>
        /// <param name="cateId">门类id</param>
        /// <param name="ids">勾选的条目数据</param>
        /// <param name="names">选中导出的字段名称</param>
        /// <param name="codes">选中导出的字段编码</param>
        /// <param name="searchType">查询的方式</param>
        /// <param name="searchCondition">查询的条件</param>
        /// <param name="sheetName">sheet名称</param>
        /// <returns></returns>
        public static OperationResult WriteEntryExcelFile(string filePath, string unitId, string cateId, string ids, string names, string codes, string searchType, string searchCondition, string sheetName)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            var dtTableName = new DataTable();// CategoryService.GetTableName(unitId, cateId).Tables[0];//表名
            if (dtTableName.Rows.Count != 1)
                return new OperationResult(OperationResultType.ParamError, "参数错误,指定分类无对应数据表");
            var tablename = dtTableName.Rows[0]["TableName"].ToString();
            var catefullname = dtTableName.Rows[0]["Cate_full_name"].ToString();
            var SubTable = tablename.Substring(0, 4);
            if (!DataBll.TableExists(tablename))
                return new OperationResult(OperationResultType.ParamError, "参数错误,指定分类无对应数据表");
            try
            {
                int page = 0;

                int rowsNum = 65535;
                string str = System.Configuration.ConfigurationManager.AppSettings["QueryRows"].ToString();
                int outint = 0;
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out outint))
                {
                    rowsNum = outint;
                }
                string colsnames = "";
                string[] name = names.Split(',');
                string[] code = codes.Split(',');
                for (int i = 0; i < name.Length && i < code.Length; i++)
                {
                    if (string.IsNullOrEmpty(colsnames))
                    {
                        colsnames += code[i] + " as " + name[i];
                    }
                    else
                    {
                        colsnames += "," + code[i] + " as " + name[i];
                    }
                }
                bool first = true;
                do
                {

                    ret = GetData(unitId, cateId, ids, searchType, searchCondition, rowsNum, ++page, colsnames);
                    DataTable dt = new DataTable();
                    if (ret.ResultType == 0)
                        dt = (DataTable)ret.AppendData;
                    else
                        break;

                    ///参数:
                    ///     文件路径filepath
                    ///     数据集合datatable
                    ///功能：
                    ///     从filepath中取出工作薄:workbook
                    ///     由workbook creat sheet
                    ///     将datatable 写入sheet
                    if ((dt.Rows.Count == 0 || dt == null) && !first)
                        break;
                    ret = Infrastructure.Common.Office.ExcelUtil.DataTableToFile(dt, filePath, sheetName + page.ToString());
                    if (ret.ResultType != OperationResultType.Success)
                    {
                        break;
                    }
                    //byte[] data = Infrastructure.Common.Office.ExcelUtil.DataTableToExcelSheet(dt, sheetname);
                    //if (!File.Exists(filePath))
                    //{
                    //    FileStream fs = new FileStream(filePath, FileMode.CreateNew);
                    //    fs.Write(data, 0, data.Length);
                    //    fs.Close();
                    //}
                    //else
                    //{
                    //    FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
                    //    fs.Write(data, 0, data.Length);
                    //    fs.Close();
                    //}
                    first = false;
                } while (true);
                if (ret.ResultType == 0)
                {
                    string userId = ServiceBase.GetInfo(ServiceBase.USERID).ToString();
                    string userName = ServiceBase.GetInfo(ServiceBase.USERNAME).ToString();
                    string userUnitCode = "";// UnitManageService.getUnitCode(unitId).AppendData.ToString();
                    string userUnitName = ServiceBase.GetInfo(ServiceBase.UNITNAME).ToString();
                    string dataUnitCode = "";// UnitManageService.getUnitCode(unitId).AppendData.ToString();//暂时与用户一样
                    string dataUnitName = ServiceBase.GetInfo(ServiceBase.UNITNAME).ToString();//暂时与用户一样
                    string dir = filePath;
                    string year = DateTime.Now.Year.ToString();
                    ret = ImportLog(unitId, dataUnitCode, dataUnitName, userUnitName, userName, DateTime.Now, rowsNum, "", year, "导出", "");

#if UNITTEST
                    UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "首页-提交入库", "入库多条记录成功", "");
                    OperationLogService.InsertLog("177A50AE-B82C-4E6C-B2C5-A03F5EB8FBD6", "192.168.1.62", DateTime.Now, "首页-提交入库: 入库多条记录成功", "");
#else

                    string logdesc = "将" + catefullname + "的" + rowsNum + "条记录导出到" + filePath + "Excel表格";
                    string logtype = "导出条目";

                    string logfuntion = "归档管理-导出";
                    if (SubTable == "JNGL")
                        logfuntion = "卷内管理-导出";
                    else if (SubTable == "AJGL")
                        logfuntion = "案卷管理-导出";
                    else if (SubTable == "WJGL")
                        logfuntion = "文件管理-导出";
                    else if (SubTable == "ZLGL")
                        logfuntion = "资料管理-导出";
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, logfuntion, logdesc, logtype);
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, logdesc, logtype);
#endif
                }
                return ret;
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ImAndExService").Error("WriteStream发生异常", ex);
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }
        #endregion
        /// <summary>
        /// 取源数据
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="cateId"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static OperationResult GetData(string unitId, string cateId, string ids, string searchType, string searchCondition, int rows, int page, string colsname = " * ")
        {
            try
            {
                PublicHelper.CheckArgument(unitId, "unitId");
                PublicHelper.CheckArgument(cateId, "cateId");
                PublicHelper.CheckArgument(ids, "ids");
                PublicHelper.CheckArgument(rows, "rows");
                PublicHelper.CheckArgument(page, "page");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ImAndExService").Error("GetData 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                var dtTableName = new DataTable();// CategoryService.GetTableName(unitId, cateId).Tables[0];
                if (dtTableName.Rows.Count != 1)
                    return new OperationResult(OperationResultType.ParamError, "参数错误,指定分类无对应数据表");
                var tablename = dtTableName.Rows[0]["TableName"].ToString();
                if (!DataBll.TableExists(tablename))
                    return new OperationResult(OperationResultType.ParamError, "参数错误,指定分类无对应数据表");
                var mgrtablename = "";
                mgrtablename = tablename.Insert(4, "Mgr");
                if (!DataBll.TableExists(mgrtablename))
                    return new OperationResult(OperationResultType.ParamError, "参数错误,指定分类无对应管理型数据表");
#if UNITTEST
                var usid = "177A50AE-B82C-4E6C-B2C5-A03F5EB8FBD6";
                var ustype = "1";
#else
                var usid = ServiceBase.GetInfo(ServiceBase.USERID).ToString();
                var ustype = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString();
#endif

                string wherestr = "";
                var length = tablename.Length;
                var tempname = "Template" + tablename.Substring(4);
                var FullPath = dtTableName.Rows[0]["FullPath"].ToString();

                string uid = "";
                string cid = "";
                Dictionary<string, object> dirList = new Dictionary<string, object>();
                if (ids != "")
                {
                    StringBuilder sbIds = new StringBuilder();
                    string[] arrIds = ids.Split(',');
                    for (int i = 0; i < arrIds.Length; i++)
                    {
                        sbIds.Append("'" + arrIds[i] + "',");

                    }
                    sbIds.Remove(sbIds.Length - 1, 1);
                    wherestr = "my00 in (" + sbIds + ")";
                }
                else
                {
                    try
                    {
                        switch (searchType)
                        {
                            case "senior":
                                var filters = searchCondition.Split('&')[0].Split(';');
                                uid = filters[1].Split(':')[1];
                                cid = filters[2].Split(':')[1];
                                filters = searchCondition.Split(';');
                                var value = filters[0];
                                StringBuilder sbWhere = new StringBuilder();
                                if (searchCondition != null && searchCondition != "")
                                {
                                    if (value == "[]")
                                    {
                                        wherestr = "1=1";
                                    }
                                    else
                                    {
                                      
                                        // List<ColInfoTypeModel> clist = Infrastructure.Common.Strings.JsonUtil.ConvertToObject<List<ColInfoTypeModel>>(value);
                                        //if (clist.Count > 0)
                                        //{
                                        //    sbWhere.Append("(");
                                        //    int i = 1;
                                        //    foreach (var item in clist)
                                        //    {
                                        //        sbWhere.Append("(");
                                        //        //全部
                                        //        if (item.colname == "-1")
                                        //        {
                                        //            string templateList = "";// TemplateService.LoadTemplateByCateId2(cid, "2").AppendData.ToString();
                                        //            switch (item.operchar)
                                        //            {
                                        //                case "等于（=）":
                                        //                    sbWhere.Append(templateList.Replace(",", "='" + item.colvalue + "' or ").Replace("*", "='" + item.colvalue + "'"));
                                        //                    break;
                                        //                case "不等于（！=）":
                                        //                    sbWhere.Append(templateList.Replace(",", ">'" + item.colvalue + "' or ").Replace("*", ">'" + item.colvalue + "'"));
                                        //                    break;
                                        //                case "小于（<）":
                                        //                    sbWhere.Append(templateList.Replace(",", "<'" + item.colvalue + "' or ").Replace("*", "<'" + item.colvalue + "'"));
                                        //                    break;
                                        //                case "小于等于（<=）":
                                        //                    sbWhere.Append(templateList.Replace(",", "!='" + item.colvalue + "' or ").Replace("*", "<='" + item.colvalue + "'"));
                                        //                    break;
                                        //                case "大于（>）":
                                        //                    sbWhere.Append(templateList.Replace(",", ">='" + item.colvalue + "' or ").Replace("*", ">'" + item.colvalue + "'"));
                                        //                    break;
                                        //                case "大于等于（>=）":
                                        //                    sbWhere.Append(templateList.Replace(",", "<='" + item.colvalue + "' or ").Replace("*", ">='" + item.colvalue + "'"));
                                        //                    break;
                                        //                case "类似于（like）":
                                        //                    sbWhere.Append(templateList.Replace(",", " like '%" + item.colvalue + "%' or ").Replace("*", " like  '%" + item.colvalue + "%'"));
                                        //                    break;
                                        //                default:
                                        //                    break;
                                        //            }
                                        //        }
                                        //        else
                                        //        {
                                        //            switch (item.operchar)
                                        //            {
                                        //                case "等于（=）":
                                        //                    sbWhere.Append(item.colname + "='" + item.colvalue + "'");
                                        //                    break;
                                        //                case "不等于（！=）":
                                        //                    sbWhere.Append(item.colname + "!='" + item.colvalue + "'");
                                        //                    break;
                                        //                case "小于（<）":
                                        //                    sbWhere.Append(item.colname + "<'" + item.colvalue + "'");
                                        //                    break;
                                        //                case "小于等于（<=）":
                                        //                    sbWhere.Append(item.colname + "<='" + item.colvalue + "'");
                                        //                    break;
                                        //                case "大于（>）":
                                        //                    sbWhere.Append(item.colname + ">'" + item.colvalue + "'");
                                        //                    break;
                                        //                case "大于等于（>=）":
                                        //                    sbWhere.Append(item.colname + ">='" + item.colvalue + "'");
                                        //                    break;
                                        //                case "类似于（like）":
                                        //                    sbWhere.Append(item.colname + " like '%" + item.colvalue + "%'");
                                        //                    break;
                                        //                default:
                                        //                    break;
                                        //            }
                                        //        }
                                        //        sbWhere.Append(")");
                                        //        //不是最后一个
                                        //        if (i < clist.Count)
                                        //        {
                                        //            switch (clist[i].caozuo)
                                        //            {
                                        //                case "并且":
                                        //                    sbWhere.Append(" and ");
                                        //                    break;
                                        //                case "或者":
                                        //                    sbWhere.Append(" or ");
                                        //                    break;
                                        //                default:
                                        //                    break;
                                        //            }
                                        //        }
                                        //        i++;
                                        //    }
                                        //    sbWhere.Append(")");
                                        //    wherestr = sbWhere.ToString();
                                        //}
                                    }
                                }
                                break;
                            case "column":
                                searchCondition = searchCondition.Replace(",", "&");
                                string[] arrParams = searchCondition.Split('&');
                                for (int i = 0; i < arrParams.Length; i++)
                                {
                                    string[] arrParams2 = arrParams[i].Split('=');
                                    if (!string.IsNullOrEmpty(arrParams2[1]))
                                    {
                                        dirList.Add(arrParams2[0], System.Web.HttpUtility.UrlDecode(arrParams2[1]));
                                    }
                                }
                                uid = dirList["unitID"].ToString();
                                cid = dirList["cateID"].ToString();
                                sbWhere = new StringBuilder();
                                foreach (var item in dirList)
                                {
                                    if (item.Key != "unitID" && item.Key != "cateID" && item.Key != "page" && item.Key != "rows")
                                    {
                                        sbWhere.Append(item.Key + " " + " like '%" + item.Value + "%',");
                                    }
                                }
                                wherestr = sbWhere.ToString().TrimEnd(',').Replace(",", " and ");
                                break;
                            case "quick":
                            case "ordinary":
                                var arr = searchCondition.Split(',');
                                uid = arr[1].Split(':')[1];
                                cid = arr[2].Split(':')[1].Split('&')[0];
                                var key = arr[0].Split(':')[0];
                                value = arr[0].Split(':')[1].Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
                                if (key == "-1")
                                {
                                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                                    dicCondition.Add("Temp_columnVisible", 1);
                                    dicCondition.Add("Temp_searchVisible", 1);
                                    dicCondition.Add("Temp_isEnabled", 1);
                                    var keylist = DataBll.GetList("Temp_columncode as code", tempname, dicCondition);
                                    List<string> lstKey = new List<string>();
                                    StringBuilder sb = new StringBuilder();
                                    if (value != "")
                                    {
                                        foreach (DataRow item in keylist.Tables[0].Rows)
                                            sb.Append(item["code"].ToString() + " like '%" + value + "%' or ");
                                        wherestr = sb.ToString().Substring(0, sb.ToString().Length - " or ".Length);
                                    }
                                }
                                else
                                {
                                    wherestr = "" + key + " like '%" + value + "%' ";
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.GetLogger("ImAndExService").Error("GetData 方法中的switch 分支 发生异常", ex);
                        ret = new OperationResult(OperationResultType.Error, "GetData的switch 发生异常", ex.Message);
                    }
                }
                //string seque = "len(my01),my01";//如果是档案 案卷 卷内 则用档号排序。
                //if (tablename.Substring(0, 4) == "WJGL" || tablename.Substring(0, 4) == "ZLGL")//如果是文件 或资料 则以流水号my04排序。
                //{
                //    seque = "len(my04),my04 ";
                //}
                //var sortcondiction = "";
                //获取排序条件
                //if (ServiceBase.dicUserCatelogSortSet.ContainsKey(cid + "," + usid))
                //{
                //    var sortds = new DataSet();// ServiceBase.dicUserCatelogSortSet[cid + "," + usid];
                //    if (sortds.Tables[0].Rows.Count > 0)
                //    {
                //        Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                //        dicCondition.Add("Temp_columnVisible", 1);
                //        dicCondition.Add("Temp_searchVisible", 1);
                //        dicCondition.Add("Temp_isEnabled", 1);
                //        var keylist = DataBll.GetList("Temp_columncode as code, Temp_columnType as type", tempname, dicCondition);
                //        for (int i = 0; i < sortds.Tables[0].Rows.Count; i++)
                //        {
                //            //var y = sortds.Tables[0].Rows[i]["SeqFieldCode"].ToString();
                //            //var x = keylist.Tables[0].Select("code = '" + sortds.Tables[0].Rows[i]["SeqFieldCode"].ToString() + "'")[0]["type"].ToString();
                //            if (keylist.Tables[0].Select("code = '" + sortds.Tables[0].Rows[i]["SeqFieldCode"].ToString() + "'")[0]["type"].ToString() == "int")
                //                sortcondiction += "convert(int, F." + sortds.Tables[0].Rows[i]["SeqFieldCode"].ToString() + ") " + sortds.Tables[0].Rows[i]["SeqOrderBy"].ToString();
                //            else if (keylist.Tables[0].Select("code = '" + sortds.Tables[0].Rows[i]["SeqFieldCode"].ToString() + "'")[0]["type"].ToString() == "double")
                //                sortcondiction += "convert(decimal, F." + sortds.Tables[0].Rows[i]["SeqFieldCode"].ToString() + ") " + sortds.Tables[0].Rows[i]["SeqOrderBy"].ToString();
                //            else
                //                sortcondiction += "len(F." + sortds.Tables[0].Rows[i]["SeqFieldCode"].ToString() + ") " + sortds.Tables[0].Rows[i]["SeqOrderBy"].ToString() + ", F." + sortds.Tables[0].Rows[i]["SeqFieldCode"].ToString() + " " + sortds.Tables[0].Rows[i]["SeqOrderBy"].ToString();

                //            if (i == (sortds.Tables[0].Rows.Count - 1))
                //            { }
                //            else
                //            {
                //                sortcondiction += ",";
                //            }
                //        }
                //    }
                //    else
                //    {
                //        if (tablename.Substring(0, 4) == "WJGL" || tablename.Substring(0, 4) == "ZLGL")//如果是文件 或资料 则以流水号my04排序。
                //            sortcondiction += "len( F.my04),F.my04 asc";
                //        else
                //            sortcondiction += " len(F.my01),F.my01 asc ";
                //   }

                //}
                //else
                //{
                //    ////sortcondiction = "FM.ArchivesTime desc";
                //    //if (tablename.Substring(0, 4) == "WJGL" || tablename.Substring(0, 4) == "ZLGL")//如果是文件 或资料 则以流水号my04排序。
                //    //    sortcondiction += "len( F.my04),F.my04 asc";
                //    //else
                //    //    sortcondiction = " len(F.my01),F.my01 asc ";
                //}
                var sortcondiction = "";
                using (var tran = new TransactionScope())
                {
                    var sql = @"SELECT  " + colsname + @"
                                FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY " + sortcondiction + @" ) AS rowunm ,
                                F.*
                                FROM      " + tablename + @" F
                                LEFT JOIN " + mgrtablename + @" FM ON F.Cate_id = FM.CateID
                                where (FM.isDelete = 0 and FM.isDestory = 0 and FM.IsTransfer = 0" + (ustype == "3" ? " and FM.HandlerID = '" + usid + "')" : ")")
                                + (wherestr != "" ? " and ( " + wherestr + " )" : "") + @"                              
                                ) AS t
                                WHERE t.rowunm > " + ((page - 1) * rows) + " AND t.rowunm <= " + page * rows + " ";
                    var dsret = DataBll.Query(sql);
#if UNITTEST
                    UserLogService.InsertLog("admin", "兄台", "192.168.1.62", DateTime.Now, FullPath, "查询条目列表成功", "");
                    OperationLogService.InsertLog("177A50AE-B82C-4E6C-B2C5-A03F5EB8FBD6", "192.168.1.62", DateTime.Now, FullPath + ": 查询条目列表成功", "");

#else
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, FullPath, "查询条目列表成功", "");
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, FullPath + ": 查询条目列表成功", "");
#endif
                    ret.Message = "查询成功";
                    ret.AppendData = dsret.Tables[0];
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ImAndExService").Error("GetData 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetData 发生异常", ex.Message);
            }
            return ret;
        }



        /// <summary>
        /// 取分类模板
        /// </summary>
        /// <param name="cateId">分类id</param>
        /// <param name="unitId">机构id</param>
        /// <param name="type">查询文件：1：全部清单；2：可见可用清单；</param>
        /// <returns></returns>
        public static OperationResult GetTemplate(string cateId, string unitId)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                string sqlCate = "select TableName from Category where Cate_id='" + cateId + "'";
                DataSet dsCate = DataBll.Query(sqlCate);
                if (dsCate != null && dsCate.Tables.Count > 0)
                {
                    DataTable dt = dsCate.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string tableName = dt.Rows[0]["TableName"].ToString();
                        if (!string.IsNullOrEmpty(tableName))
                        {
                            using (var trans = new TransactionScope())
                            {
                                string tempaleTableName = "Template" + tableName.Substring(4);
                                // string sql = "if  exists (select * from sysobjects where id = object_id(N'" + tempaleTableName + "')) select * from " + tempaleTableName + " where Cate_id='" + cateId + "' and Temp_columnVisible = '1' and Temp_isEnabled = '1' " + (type == "2" ? "and Temp_searchVisible = '1'" : "") + " order by Temp_sequence asc";
                                string sql = "if  exists (select * from sysobjects where id = object_id(N'" + tempaleTableName + "')) select * from " + tempaleTableName + " where Cate_id='" + cateId + "' and  Temp_ColumnName is not null and  Temp_ColumnName !='' order by Temp_sequence asc";
                                DataSet ds = DataBll.Query(sql);
                                ret.AppendData = ds;
                                trans.Complete();
                                return ret;
                            }
                        }
                        else
                        {
                            ret.ResultType = OperationResultType.Error;
                            ret.Message = "表不存在！";
                        }
                    }
                    else
                    {
                        ret.Message = "查无此表！";
                        ret.ResultType = OperationResultType.Error;
                    }
                }
                else
                {
                    ret.Message = "查无此表！";
                    ret.ResultType = OperationResultType.Error;
                }
                return ret;
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ImAndExService").Error("写入模板文件 发生异常", ex);
                return new OperationResult(OperationResultType.Error, "执行数据查询时出错了，错误原因:" + ex.Message.ToString());
            }
        }
        /// <summary>
        /// 查询获取在此分类,单位下的用到的所有提示项.
        /// </summary>
        /// <param name="cateId">分类id</param>
        /// <param name="unitId">单位id</param>
        /// <returns></returns>
        public static OperationResult GetTipsItem(string cateId, string unitId)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                string sqlCate = "select TableName from Category where Cate_id='" + cateId + "'";//取数据表名
                DataSet dsCate = DataBll.Query(sqlCate);
                if (dsCate != null && dsCate.Tables.Count > 0)
                {
                    DataTable dt = dsCate.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string tableName = dt.Rows[0]["TableName"].ToString();
                        if (!string.IsNullOrEmpty(tableName))
                        {
                            using (var trans = new TransactionScope())
                            {
                                string tempaleTableName = "Template" + tableName.Substring(4);
                                string sql = "if  exists (select * from sysobjects where id = object_id(N'" + tempaleTableName
                                                + @"')) select
                                                    t.Temp_columncode,
                                                    t . Temp_ColumnName,
                                                    (case when t.Temp_selected = '1' then ( select DicName from DictionaryClass where Dic_ID = t . Temp_selected_context) else '' end) as dicName,
                                                    di . ItemContent from " + tempaleTableName +
                                                    @" t   left join DictionaryItem di on di.Dic_ID=t.Temp_selected_context
                                                    where t.Temp_selected=1 and ( t.Temp_ColumnName is not null or t.Temp_ColumnName!='' )  and len (t.Temp_selected_context)>35
                                                    order by Temp_columncode";
                                DataSet ds = DataBll.Query(sql);
                                ret.AppendData = ds;
                                trans.Complete();
                                return ret;
                            }
                        }
                        else
                        {
                            ret.ResultType = OperationResultType.Error;
                            ret.Message = "表不存在！";
                        }
                    }
                    else
                    {
                        ret.Message = "查无此表！";
                        ret.ResultType = OperationResultType.Error;
                    }
                }
                else
                {
                    ret.Message = "查无此表！";
                    ret.ResultType = OperationResultType.Error;
                }
                return ret;
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ImAndExService").Error("写入模板文件 发生异常", ex);
                return new OperationResult(OperationResultType.Error, "执行数据查询时出错了，错误原因:" + ex.Message.ToString());
            }
        }
        #endregion


        #region 导入文件

        public static OperationResult ToXml(string unitId, string cateId, string keys, string values, string ids, string names, string codes, string filepath)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            DataTable dt = GetAllData(unitId, cateId, keys, values, ids, names, codes);
            ret = Infrastructure.Common.Office.XmlUtil.GetByteForDataSet(dt, filepath);
            return ret;
        }

        public static OperationResult ToMDB(string unitId, string cateId, string keys, string values, string ids, string names, string codes, string filepath)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);

            DataTable dt = GetAllData(unitId, cateId, keys, values, ids, names, codes);
            Infrastructure.Common.DBExport.MDBUtil mdb = new Infrastructure.Common.DBExport.MDBUtil();
            string reMsg = "";
            mdb.DataTableExportToAccess(dt, filepath, "DataTransfer", ref reMsg);
            return ret;
        }

        public static OperationResult ToDBF(string unitId, string cateId, string keys, string values, string ids, string names, string codes, string filepath, string catename)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            DataTable dt = GetAllData(unitId, cateId, keys, values, ids, names, codes);
            Infrastructure.Common.DBExport.DBFUtil dbf = new Infrastructure.Common.DBExport.DBFUtil();
            string reMsg = "";
            dbf.ExportData(filepath, catename, dt.DataSet, reMsg);
            return ret;
        }

        public static OperationResult ToExcel(string unitId, string cateId, string keys, string values, string ids, string names, string codes, string filepath)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            DataTable dt = GetAllData(unitId, cateId, keys, values, ids, names, codes);
            Infrastructure.Common.Office.ExcelUtil.CreateExcel(dt, filepath);
            return ret;
        }

        public static DataTable GetAllData(string unitId, string cateId, string keys, string values, string ids, string names, string codes)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);

            DataTable dt = new DataTable();
            var dtTableName = new DataTable();// CategoryService.GetTableName(unitId, cateId).Tables[0];

            var tablename = dtTableName.Rows[0]["TableName"].ToString();

            var mgrtablename = tablename.Insert(4, "Mgr");


#if UNITTEST
                var usid = "177A50AE-B82C-4E6C-B2C5-A03F5EB8FBD6";
                var ustype = "1";
#else
            var usid = ServiceBase.GetInfo(ServiceBase.USERID).ToString();
            var ustype = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString();
#endif



            string wherestr = "";
            string idstr = "";
            var length = tablename.Length;
            var tempname = "Template" + tablename.Substring(4);
            var FullPath = dtTableName.Rows[0]["FullPath"].ToString();
            if (ids != "")
            {
                StringBuilder sbIds = new StringBuilder();
                string[] arrIds = ids.Split(',');
                for (int i = 0; i < arrIds.Length; i++)
                {
                    sbIds.Append("'" + arrIds[i] + "',");

                }
                sbIds.Remove(sbIds.Length - 1, 1);
                idstr = " and my00 in (" + sbIds + ")";
            }
            else
            {
                if (keys == "-1")
                {


                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                    dicCondition.Add("Temp_columnVisible", 1);
                    dicCondition.Add("Temp_searchVisible", 1);
                    dicCondition.Add("Temp_isEnabled", 1);
                    var keylist = DataBll.GetList("Temp_columncode as code", tempname, dicCondition);
                    List<string> lstKey = new List<string>();
                    StringBuilder sb = new StringBuilder();

                    if (values != "")
                    {
                        foreach (DataRow item in keylist.Tables[0].Rows)
                            sb.Append(item["code"].ToString() + " = '" + values + "' or ");
                        wherestr = sb.ToString().Substring(0, sb.ToString().Length - " or ".Length);
                    }
                }
                else
                {
                    wherestr = " and " + keys + " = '" + values + "' ";
                }
            }
            string seque = "my01";//如果是档案 案卷 卷内 则用档号排序。
            if (tablename.Substring(4) == "WJGL" || tablename.Substring(4) == "ZLGL")//如果是文件 或资料 则以流水号my04排序。
            {
                seque = "my04";
            }
            string[] arrnames = names.Split(',');
            string[] arrcodes = codes.Split(',');
            string sel = "";
            for (int i = 0; i < arrnames.Length; i++)
            {
                sel += " " + arrcodes[i].Trim() + " as '" + arrnames[i].Trim() + "',";
            }
            sel = sel.Substring(0, sel.Length - 1);
            using (var tran = new TransactionScope())
            {
                var sql = @"SELECT  " + sel + @"
                                FROM      " + tablename + @" F
                                LEFT JOIN " + mgrtablename + @" FM ON F.Cate_id = FM.CateID
                                where (FM.isDelete = 0 and FM.isDestory = 0 and FM.IsTransfer = 0" + (ustype == "3" ? " and FM.HandlerID = '" + usid + "')" : ")") + (values != "" ? wherestr : "") + (ids != "" ? idstr : "");
                var dsret = DataBll.Query(sql);
                dt = dsret.Tables[0];
                tran.Complete();
            }
            return dt;
        }        


        #region 插入数据


        /// <summary>
        /// 处理每行，将每行 记录处理为dictionary
        /// </summary>
        /// <param name="cateId"></param>
        /// <param name="unitId"></param>
        /// <param name="userId"></param>
        /// <param name="Row"></param>
        /// <param name="Columns"></param>
        /// <param name="list"></param>
        /// <param name="isForce"></param>
        /// <returns></returns>
        public static OperationResult CreateRow(string cateId, string unitId, string userId, DataRow Row, DataColumnCollection Columns, bool isForce = false)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            Dictionary<string, object> listParams = new Dictionary<string, object>();
            try
            {
                for (int i = 0; i < Columns.Count; i++)//遍历每一列;;
                {
                    string[] s = Columns[i].ColumnName.Split('|');
                    //   string colsname = Columns[i].ColumnName.Split('|')[0];
                    string cellvalue = Row[Columns[i].ColumnName].ToString();
                    string colscode = "";
                    if (s.Length > 1)
                    {
                        colscode = Columns[i].ColumnName.Split('|')[1];
                    }

                    //if (colsname == "" && string.IsNullOrEmpty(cellvalue))
                    //  if ((colscode=="-1")||colscode=="")
                    if (!colscode.Contains("my"))
                    {
                        continue;
                    }
                    string key = colscode;
                    string value = string.IsNullOrEmpty(Row[i].ToString()) ? "" : Row[i].ToString();
                    //  TemplateViewModel model = new TemplateViewModel();
                    string model = null;// list.FirstOrDefault(m => m.Temp_columncode == colscode);//定位模板中列名与源数据一致的列
                    if (model != null)//存在
                    {
                        listParams.Add(key, value);
                    }
                    else//此行记录中有一个列，无法找到对应的模板列。则跳过，并设置此记录无效。
                    {
                        ret.ResultType = OperationResultType.Error;
                        ret.Message += "无法找到对应的列";
                        return ret;
                    }
                }
                ret.ResultType = 0;
                ret.AppendData = listParams;
                return ret;
            }
            catch (Exception e)
            {
                ret.ResultType = OperationResultType.Error;
                ret.Message = e.ToString();
                Logger.GetLogger("ImAndExService").Error("CheckCols 发生异常", e);
                return ret;
            }
        }

        #endregion
        #endregion

        #region 打印
        /// <summary>
        /// 取勾选 的打印数据
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cateId"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public static OperationResult GetSelectedList(string ids, string cateId, string unitId)
        {
            try
            {
                PublicHelper.CheckArgument(ids, "ids");
                PublicHelper.CheckArgument(cateId, "cateId");
                PublicHelper.CheckArgument(unitId, "unitId");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ImAndExService").Error("GetSelectedList 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                StringBuilder sbIds = new StringBuilder();
                string[] arrIds = ids.Split(',');
                for (int i = 0; i < arrIds.Length; i++)
                {
                    sbIds.Append("'" + arrIds[i] + "',");

                }
                sbIds.Remove(sbIds.Length - 1, 1);
                var dtTableName = new DataTable();// CategoryService.GetTableName(unitId, cateId).Tables[0];
                if (dtTableName.Rows.Count != 1)
                    return new OperationResult(OperationResultType.ParamError, "参数错误,指定分类无对应数据表");

                var tablename = dtTableName.Rows[0]["TableName"].ToString();

                if (!DataBll.TableExists(tablename))
                    return new OperationResult(OperationResultType.ParamError, "参数错误,指定分类无对应数据表");

                var mgrtablename = "";
                mgrtablename = tablename.Insert(4, "Mgr");

                if (!DataBll.TableExists(mgrtablename))
                    return new OperationResult(OperationResultType.ParamError, "参数错误,指定分类无对应管理型数据表");

#if UNITTEST
                var usid = "177A50AE-B82C-4E6C-B2C5-A03F5EB8FBD6";
                var ustype = "1";
#else
                var usid = ServiceBase.GetInfo(ServiceBase.USERID).ToString();
                var ustype = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString();
#endif

                string wherestr = "my00 in (" + sbIds + ")";
                var l = tablename.Length;
                var tempname = "Template" + tablename.Substring(4);
                var FullPath = dtTableName.Rows[0]["FullPath"].ToString();
                using (var tran = new TransactionScope())
                {
                    var sql = @"SELECT  *
                                FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY FM.ArchivesTime desc) AS rowunm ,
                                F.*
                                FROM      " + tablename + @" F
                                LEFT JOIN " + mgrtablename + @" FM ON F.Cate_id = FM.CateID
                                where (FM.isDelete = 0 and FM.isDestory = 0 and FM.IsTransfer = 0" + (ustype == "3" ? " and FM.HandlerID = '" + usid + "')" : ")") + (wherestr != "" ? " and ( " + wherestr + " )" : "") + @"
                                ) AS t";

                    var dsret = DataBll.Query(sql);
                    var count = DataBll.Query("select count(F.my00) as count from " + tablename + " F left join " + mgrtablename + " FM on f.Cate_id = fm.CateID where (FM.isDelete = 0 and FM.isDestory = 0 and FM.IsTransfer = 0" + (ustype == "3" ? " and FM.HandlerID = '" + usid + "')" : ")") + " and ( " + wherestr + " )");

#if UNITTEST
                    UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, FullPath, "查询条目列表成功", "");
                    OperationLogService.InsertLog("177A50AE-B82C-4E6C-B2C5-A03F5EB8FBD6", "192.168.1.62", DateTime.Now, FullPath + ": 查询条目列表成功", "");

#else
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, FullPath, "查询条目列表成功", "");
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, FullPath + ": 查询条目列表成功", "");
#endif
                    ret.Message = "查询成功";
                    ret.AppendData = new
                    {
                        total = count.Tables[0].Rows[0]["count"],
                        rows = dsret.Tables[0]
                    };

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("ImAndExService").Error("GetSelectedList 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetSelectedList 发生异常", ex.Message);
            }
            return ret;
        }

        #endregion

        #region 导入导出日志
        /// <summary>
        ///
        /// </summary>
        /// <param name="unitId">单位id</param>
        /// <param name="Unit_code">全宗号</param>
        /// <param name="Ai_unitName">全宗单位简称</param>
        /// <param name="Ai_importMenUnitName">操作人单位</param>
        /// <param name="Ai_importMen">操作人</param>
        /// <param name="Ai_exportTime">操作时间</param>
        /// <param name="Ai_sum">数据总量</param>
        /// <param name="Ai_dir">文件路径</param>
        /// <param name="Ai_year">年度</param>
        /// <param name="Ai_type">类型：导入数据，导出数据，导入模板，导出模板，</param>
        /// <param name="Ai_remark">备注</param>
        /// <returns></returns>
        public static OperationResult ImportLog(string unitId, string Unit_code, string Ai_unitName, string Ai_importMenUnitName, string Ai_importMen, DateTime Ai_exportTime, int Ai_sum, string Ai_dir, string Ai_year, string Ai_type, string Ai_remark)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            Dictionary<string, object> dicValue = new Dictionary<string, object>();
            dicValue.Add("Unit_code", Unit_code);
            dicValue.Add("Ai_unitName", Ai_unitName);
            dicValue.Add("Ai_ExportMenUnitName", Ai_importMenUnitName);
            dicValue.Add("Ai_importMenUnitName", Ai_importMenUnitName);
            dicValue.Add("Ai_ExportMen", Ai_importMen);
            dicValue.Add("Ai_importMen", Ai_importMen);
            dicValue.Add("Ai_exportTime", Ai_exportTime.ToShortDateString());
            dicValue.Add("Ai_sum", Ai_sum);
            dicValue.Add("Ai_dir", Ai_dir);
            dicValue.Add("Ai_year", Ai_year);
            dicValue.Add("Ai_type", Ai_type);
            dicValue.Add("Ai_remark", Ai_remark);
            string tableName = "ArchivesImport" + unitId;
            if (!DataBll.TableExists(tableName))
                return new OperationResult(OperationResultType.ParamError, "参数错误,指定分类无对导入日志表");
            else
            {
                DataSet ds = DataBll.Add(dicValue, tableName);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ret.ResultType = OperationResultType.Success;
                }
            }
            return ret;
        }
        #endregion

        public static byte[] ExportExcel(string unitIds, string cateIds, string cateCode, string rownames, string colsnames)
        {
            unitIds = "32";
            cateIds = "";
            cateCode = "";
            rownames = "密级,机构,保管期限,年度";
            colsnames = "密级,机构,保管期限,年度";

            string[] arrUnitId = unitIds.Split(',');
            string[] arrCateId = cateIds.Split(',');

            string[] arrrow = rownames.Split(',');

            string[] arrcols = colsnames.Split(',');



            #region 查出要统计的表 .
            string catesql = "select * from Category  where 1=1 ";
            if (!string.IsNullOrEmpty(unitIds))
            {
                catesql += "and UnitID  in (" + unitIds + ")";
            }
            if (!string.IsNullOrEmpty(cateIds))
            {
                for (int j = 0; j < arrCateId.Length; j++)
                {
                    cateIds += "'" + arrCateId[j].ToString().Trim() + "',";
                }
                cateIds = cateIds.Substring(0, cateIds.Length - 1);
                catesql += "and  cate_id in(" + cateIds + ")";

            }
            if (cateCode != "")
            {
                catesql += "and TableName like '" + cateCode + "%'";
            }
            catesql += "order by UnitID";
            DataTable catedt = DataBll.Query(catesql).Tables[0];
            #endregion
            DataTable dt = new DataTable();
            OperationResult ret = new OperationResult(OperationResultType.Success);
            if (catedt != null && catedt.Rows.Count > 0)//有表可查
            {
                foreach (DataRow dr in catedt.Rows) //遍历每个cate
                {
                    DataTable singelcatedt = new DataTable();//某一 项 category  的表的数据统计
                    DataTable singlerowdt = new DataTable();
                    #region 遍历每个cate
                    string sqlinner = "";
                    string sqlouter = "";
                    string cateId = dr["Cate_id"].ToString().Trim();//分类ID
                    var tablename = dr["TableName"].ToString().Trim();//表名
                    var tmptablename = "Template" + (tablename.Length >= 4 ? tablename.Substring(4) : "");//模板表名
                    var mgrtablename = tablename.Length >= 4 ? tablename.Insert(4, "Mgr") : "";//管理表名
                    Guid result = new Guid();
                    if (Guid.TryParse(cateId, out result)) //有表id
                    {
                        #region 有表有内容
                     //  List<TemplateViewModel> Tlist = (List<TemplateViewModel>)GetTemplateListByCateId(cateId, true).AppendData;//模板字段列表
                        string innersels = "";
                        string outersels = "";
                        string Tlist = null;
                        if (Tlist != null)
                        {
                            #region 列出每个要统计的列和行.水平铺开
                            foreach (string val in arrrow)//取字段名称tempcolumnname,与之对应 的编码columncode.
                            {
                                //TemplateViewModel tvm = Tlist.FirstOrDefault(x => x.Temp_ColumnName == val);
                                //if (tvm != null && !string.IsNullOrEmpty(tvm.Temp_columncode))
                                //{
                                //    innersels += "ISNULL(" + tvm.Temp_columncode + ",'') " + " as '" + val.Trim() + "' ,";
                                //}
                                //else
                                //{
                                //    innersels += "'' as '" + val.Trim() + "' ,";
                                //}
                            }
                            foreach (string val in arrcols)//取字段名称tempcolumnname,与之对应 的编码columncode.
                            {
                                if (!arrrow.Contains(val))
                                {
                                    //TemplateViewModel tvm = Tlist.FirstOrDefault(x => x.Temp_ColumnName == val);

                                    //if (tvm != null && !string.IsNullOrEmpty(tvm.Temp_columncode))
                                    //{
                                    //    innersels += "ISNULL(" + tvm.Temp_columncode + ",'') " + " as '" + val.Trim() + "' ,";
                                    //}
                                    //else
                                    //{
                                    //    innersels += "'' as '" + val.Trim() + "' ,";
                                    //}
                                }
                            }

                            innersels = innersels.Substring(0, innersels.Length - 1);
                            sqlinner += "  select  " + innersels + " from  " + tablename + " f left join	" + mgrtablename + " m on f.Cate_id=m.CateID where m.IsDelete=0 and m.IsDestory = 0 and m.IsTransfer = 0 ";
                            #endregion

                            foreach (string val in arrrow)
                            {
                                #region 根据指定的项,合并所有的行
                                sqlouter = "select " + val.Trim() + ",";
                                foreach (string cell in arrcols)//遍历每个列
                                {
                                    string colsname = cell.ToString().Trim();//列名 保管期限
                                    string sqlgroup = "select  " + colsname + ",count(0) as '总数'  from ( " + sqlinner + ")mm group by " + colsname + " order by " + colsname;//列中各个元素 10年,30年,短期,长期,永久,
                                    DataTable dtcols = DataBll.Query(sqlgroup).Tables[0];           //列中各个元素 10年,30年,短期,长期,永久,
                                    foreach (DataRow row in dtcols.Rows)//遍历各个 元素10年,30年,短期,长期,永久,    并将期数据平铺
                                    {
                                        string value = row[colsname].ToString().Trim() == "" ? " " : row[colsname].ToString().Trim();
                                        sqlouter += " sum(case " + colsname + " when '" + value.Trim() + "' then 1 else 0 end ) as '" + (value.Trim() == "" ? "空的" + cell.Trim() : value.Trim()) + "',";
                                    }
                                }
                                sqlouter = sqlouter.Substring(0, sqlouter.Length - 1);
                                sqlouter += " from (" + sqlinner + ")mm  group by " + val.ToString().Trim() + " order by " + val.ToString().Trim();
                                #endregion
                                #region DataTable 结构合并

                                singlerowdt = DataBll.Query(sqlouter).Tables[0];
                                if (singlerowdt.Rows.Count > 0)//有记录
                                {
                                    if (singelcatedt.Rows.Count > 0)
                                    {
                                        foreach (DataRow row in singlerowdt.Rows)
                                        {
                                            DataRow ndr = singelcatedt.NewRow();
                                            ndr.ItemArray = row.ItemArray;
                                            singelcatedt.Rows.Add(ndr);
                                            singelcatedt.AcceptChanges();
                                        }
                                    }
                                    else
                                    {
                                        singelcatedt = singlerowdt.Copy();
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region DataTable内容合并.
                    foreach (DataColumn sdc in singelcatedt.Columns)
                    {
                        if (!dt.Columns.Contains(sdc.ColumnName))
                        {
                            dt.Columns.Add(sdc.ColumnName, typeof(string), "");
                        }
                    }
                    dt.AcceptChanges();
                    for (int i = 0; i < singelcatedt.Rows.Count; i++)
                    {
                        string val1 = singelcatedt.Rows[i][0].ToString().Trim();
                        bool isExists = false;
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            string val2 = dt.Rows[j][0].ToString().Trim();
                            if (val1 == val2)
                            {
                                isExists = true;
                                break;
                            }
                        }
                        if (!isExists)
                        {
                            DataRow ndr = dt.NewRow();
                            ndr.ItemArray = singelcatedt.Rows[i].ItemArray;
                            dt.Rows.Add(ndr);
                            singelcatedt.Rows[i].Delete();
                            dt.AcceptChanges();
                        }

                    }
                    singelcatedt.AcceptChanges();
                    for (int i = 1; i < singelcatedt.Rows.Count; i++)
                    {
                        for (int j = 0; j < singelcatedt.Columns.Count; j++)
                        {
                            string colsname = singelcatedt.Columns[j].ColumnName;
                            string rowname = singelcatedt.Rows[i][0].ToString().Trim();
                            string value = singelcatedt.Rows[i][j].ToString().Trim();
                            int val = 0;
                            if (int.TryParse(value, out val) && val > 0)
                            {
                                foreach (DataRow ddr in dt.Rows)
                                {
                                    if (ddr[0].ToString().Trim() == rowname)
                                    {
                                        string val2 = ddr[colsname].ToString().Trim();
                                        if (!string.IsNullOrEmpty(val2))
                                        {
                                            ddr[colsname] = val + Convert.ToInt32(ddr[colsname].ToString().Trim());
                                        }
                                        else
                                        {
                                            ddr[colsname] = val;
                                        }

                                        break;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            else
            {
                ret.ResultType = OperationResultType.Error;
                ret.Message += "没有一个表可查";
            }
            byte[] bytes = Infrastructure.Common.Office.ExcelUtil.GetByteForDataTable(dt);
            return bytes;

        }


        /// <summary>
        /// 分页查取条目数据
        /// </summary>
        /// <param name="unitId">单位ID</param>
        /// <param name="cateId">门类ID</param>
        /// <param name="keys">查询条件</param>
        /// <param name="values">查询数值</param>
        /// <param name="ids">勾选的条目ID</param>
        /// <param name="rows">行数</param>
        /// <param name="pages">页码</param>
        /// <returns></returns>
        public static DataTable GetEntries(string unitId, string cateId, string keys, string values, string ids, int rows, int pages)
        {
            DataTable dt = new DataTable();
            var dtTableName = new DataTable();// CategoryService.GetTableName(unitId, cateId).Tables[0];

            var tablename = dtTableName.Rows[0]["TableName"].ToString();

            var mgrtablename = tablename.Insert(4, "Mgr");


#if UNITTEST
                var usid = "177A50AE-B82C-4E6C-B2C5-A03F5EB8FBD6";
                var ustype = "1";
#else
            var usid = ServiceBase.GetInfo(ServiceBase.USERID).ToString();
            var ustype = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString();
#endif



            string wherestr = "";
            string idstr = "";
            var length = tablename.Length;
            var tempname = "Template" + tablename.Substring(4);
            var FullPath = dtTableName.Rows[0]["FullPath"].ToString();
            if (ids != "")
            {
                StringBuilder sbIds = new StringBuilder();
                string[] arrIds = ids.Split(',');
                for (int i = 0; i < arrIds.Length; i++)
                {
                    sbIds.Append("'" + arrIds[i] + "',");

                }
                sbIds.Remove(sbIds.Length - 1, 1);
                idstr = " and my00 in (" + sbIds + ")";
            }
            else
            {
                if (keys == "-1")
                {


                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                    dicCondition.Add("Temp_columnVisible", 1);
                    dicCondition.Add("Temp_searchVisible", 1);
                    dicCondition.Add("Temp_isEnabled", 1);
                    var keylist = DataBll.GetList("Temp_columncode as code", tempname, dicCondition);
                    List<string> lstKey = new List<string>();
                    StringBuilder sb = new StringBuilder();

                    if (values != "")
                    {
                        foreach (DataRow item in keylist.Tables[0].Rows)
                            sb.Append(item["code"].ToString() + " = '" + values + "' or ");
                        wherestr = sb.ToString().Substring(0, sb.ToString().Length - " or ".Length);
                    }
                }
                else
                {
                    wherestr = " and " + keys + " = '" + values + "' ";
                }
            }
            string seque = "my01";//如果是档案 案卷 卷内 则用档号排序。
            if (tablename.Substring(4) == "WJGL" || tablename.Substring(4) == "ZLGL")//如果是文件 或资料 则以流水号my04排序。
            {
                seque = "my04";
            }
            using (var tran = new TransactionScope())
            {
                var sql = @"SELECT  *
                                FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY " + seque + @" asc) AS rowunm ,
                                F.*
                                FROM      " + tablename + @" F
                                LEFT JOIN " + mgrtablename + @" FM ON F.Cate_id = FM.CateID
                                where (FM.isDelete = 0 and FM.isDestory = 0 and FM.IsTransfer = 0" + (ustype == "3" ? " and FM.HandlerID = '" + usid + "')" : ")") + (values != "" ? wherestr : "") + (ids != "" ? idstr : "") + @"
                                ) AS t
                                WHERE t.rowunm > " + ((pages - 1) * rows) + " AND t.rowunm <= " + pages * rows + " ";

                var dsret = DataBll.Query(sql);


#if UNITTEST
                    UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, FullPath, "查询条目列表成功", "");
                    OperationLogService.InsertLog("177A50AE-B82C-4E6C-B2C5-A03F5EB8FBD6", "192.168.1.62", DateTime.Now, FullPath + ": 查询条目列表成功", "");

#else

                UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, FullPath, "查询条目列表成功", "");
                OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, FullPath + ": 查询条目列表成功", "");

#endif
                dt = dsret.Tables[0];
                tran.Complete();
            }
            return dt;
        }

        public static DataTable GetEFiles(string unitId, string cateId, string keys, string values, string ids)
        {
            DataTable dt = new DataTable();
            //            var dtTableName = CategoryService.GetTableName(unitId, cateId).Tables[0];

            //           var tablename = dtTableName.Rows[0]["TableName"].ToString();

            ////          var mgrtablename = tablename.Insert(4, "Mgr");


            //        var usid = ServiceBase.GetInfo(ServiceBase.USERID).ToString();
            //       var ustype = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString();


            //      string wherestr = "";
            //     string idstr = "";
            //    var length = tablename.Length;
            //   var tempname = "Template" + tablename.Substring(4);
            //  var FullPath = dtTableName.Rows[0]["FullPath"].ToString();
            // if (ids != "")
            ////{
            //  StringBuilder sbIds = new StringBuilder();
            // string[] arrIds = ids.Split(',');
            //for (int i = 0; i < arrIds.Length; i++)
            //{
            //   sbIds.Append("'" + arrIds[i] + "',");

            //}
            //  sbIds.Remove(sbIds.Length - 1, 1);
            //   idstr = " and my00 in (" + sbIds + ")";
            //}
            //else
            //{
            //    if (keys == "-1")
            //    {


            //        Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            //        dicCondition.Add("Temp_columnVisible", 1);
            //        dicCondition.Add("Temp_searchVisible", 1);
            //        dicCondition.Add("Temp_isEnabled", 1);
            //        var keylist = DataBll.GetList("Temp_columncode as code", tempname, dicCondition);
            //        List<string> lstKey = new List<string>();
            //        StringBuilder sb = new StringBuilder();

            //        if (values != "")
            //        {
            //            foreach (DataRow item in keylist.Tables[0].Rows)
            //                sb.Append(item["code"].ToString() + " = '" + values + "' or ");
            //            wherestr = sb.ToString().Substring(0, sb.ToString().Length - " or ".Length);
            //        }
            //    }
            //    else
            //    {
            //        wherestr = " and " + keys + " = '" + values + "' ";
            //    }
            //}
            //string seque = "my01";//如果是档案 案卷 卷内 则用档号排序。
            //if (tablename.Substring(4) == "WJGL" || tablename.Substring(4) == "ZLGL")//如果是文件 或资料 则以流水号my04排序。
            //{
            //    seque = "my04";
            //}
            using (var tran = new TransactionScope())
            {
                var sql = @"select * from EFileDetailList where UnitID='" + unitId + "' and CatelogID='" + cateId + "' ";

                var dsret = DataBll.Query(sql);



#if UNITTEST
                    UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "查询条目列表成功", "","");
                    OperationLogService.InsertLog("177A50AE-B82C-4E6C-B2C5-A03F5EB8FBD6", "192.168.1.62", DateTime.Now, ": 查询条目列表成功", "");

#else

                UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "", "查询条目列表成功", "");
                OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, ": 查询条目列表成功", "：：");
#endif
                dt = dsret.Tables[0];
                tran.Complete();
            }
            return dt;
        }

        /// <summary>
        ///写入Excle表格
        /// </summary>
        /// <param name="unitid">单位id</param>
        /// <param name="cateid">门类 id</param>
        /// <param name="entryid">条目id</param>
        /// <returns></returns>
        public static OperationResult ToCsv(DataTable dt, string filepath, string colsnames, string codes)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            bool ishand = false;
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Close();
                ishand = true;
            }

            using (FileStream fw = new FileStream(filepath, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fw, Encoding.Default))
                {
                    int rows = 0;
                    if (ishand)
                    {
                        sw.WriteLine(colsnames);
                        ishand = false;
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        rows += dt.Rows.Count;
                        string[] arrCodes = codes.Split(',');
                        foreach (DataRow row in dt.Rows)//遍历DataTable的每一行。
                        {
                            string s = string.Empty;
                            for (int i = 0; i < arrCodes.Length; i++)
                            {
                                if (dt.Columns.Contains(arrCodes[i]))
                                    s += row[arrCodes[i]].ToString() + ",";
                            }
                            if (s.Length > 0)
                                s = s.Substring(0, s.Length - 1);
                            //Infrastructure.Common.Office.ExcelUtil
                            sw.WriteLine(s);
                        }
                    }
                    sw.Flush();
                    fw.Flush();
                }
            }
            return ret;
        }

        public static OperationResult ToExcel(DataTable dt, string filepath, string colsnames, string codes)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            bool ishand = false;
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Close();
                ishand = true;

            }
            using (FileStream fw = new FileStream(filepath, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fw, Encoding.Default))
                {
                    //ret =Infrastructure.Common.Office.ExcelUtil.DataTjableToExcel(dt,filepath);

                    sw.Flush();
                    fw.Flush();
                }
            }
            return ret;
        }




    }

}
