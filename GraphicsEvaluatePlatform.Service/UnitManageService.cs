//#define UNITTEST
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
    public class UnitManageService
    {
        private static List<ComboTree> reTreeList = new List<ComboTree>();
        /// <summary>
        /// 获取单位列表
        /// </summary>
        /// <returns></returns>
        public static OperationResult GetUnitList()
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    var wherestr = "";
#if UNITTEST
                    var usType = "1";
                    var unitname = DataBll.Query("select u_Name from t_Units where u_Id= 'd079a6f3-5547-4313-a144-f952d524416d'").Tables[0].Rows[0][0].ToString();
#else
                    var usType = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString().Trim();
                    var unitname = DataBll.Query("select u_Name from t_Units where u_Id='" + ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim() + "'").Tables[0].Rows[0][0].ToString().Trim();
#endif
                    if (usType == "1") //如果是超级管理员
                    {
                        wherestr = "";
                    }
                    else if (usType == "2") //如果是一般管理员
                    {
                        wherestr = "( ut.u_FullPath='" + unitname + "' or ut.u_FullPath like '%-" + unitname + "%' or ut.u_FullPath like '%" + unitname + "-%')";
                    }
                    else //如果是一般操作员
                    {
                        wherestr = "( ut.u_FullPath='" + unitname + "' or ut.u_FullPath like '%-" + unitname + "')";
                    }
                    string sql = "select ut.* from t_Units ut " + (wherestr == "" ? "" : (" where " + wherestr)) + " order by u_DataTime asc";
                    var dtUnit = DataBll.Query(sql).Tables[0];                    
                    dtUnit = DataTrim.DataTableTrim(dtUnit);//去掉字符串后面的空格
                    var count = DataBll.GetCount("t_Units ut", wherestr == "" ? "1=1" : wherestr);

                    //修改节点parentid和depth
                    if (usType != "1") {
                        for (int i = 0; i < dtUnit.Rows.Count; i++)//如果是根节点
                        {
                            if (dtUnit.Select("u_Id = '" + dtUnit.Rows[i]["u_ParentId"].ToString() + "'").Length == 0)
                            {
                                dtUnit.Rows[i]["u_ParentId"] = null;
                                dtUnit.Rows[i]["u_Depth"] = 0;
                            }
                            else//如果不是根节点
                            {
                                string depth = dtUnit.Select("u_Id ='" + dtUnit.Rows[i]["u_ParentId"].ToString().Trim() + "'")[0]["u_Depth"].ToString().Trim();
                                dtUnit.Rows[i]["u_Depth"] = int.Parse(depth) + 1;
                            }

                        }
                    }                 
                    //返回值
                    ret.AppendData = new
                    {
                        total = count,
                        rows = dtUnit
                    };

                    //添加操作日志和用户日志
                    var UserID = ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim();//当前登录用户id
                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                    dicCondition.Add("Us_id", UserID);
                    var retdata = DataBll.GetModel("", "t_Users", dicCondition).Tables[0];
                    var User = retdata.Rows[0];
                    UserLogService.InsertLog(User["Us_account"].ToString(), User["Us_name"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "单位管理-加载单位列表", "成功加载单位列表！", "");
                    OperationLogService.InsertLog(User["Us_id"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "成功加载单位列表！", "");

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UnitManageService").Error("GetUnitList 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetUnitList 发生异常", ex.Message);
            }
            return ret;
        }

        /// <summary>
        /// 获取单位下拉框树形列表
        /// </summary>
        /// <returns>操作对象</returns>
        public static OperationResult GetUnitComboTree()
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                Dictionary<string, object> dicCondiction = new Dictionary<string, object>();
                dicCondiction.Add("IsEnable", 1);

                var user_Type = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString().Trim();
                var UintID = "";
                if (user_Type.ToString() != "1")//若登录用户不是开发管理员，获取当前单位id
                {
                    UintID = ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim();
                }

                var dicUnitList = ServiceBase.dicUnitList;
               
                List<ComboTree> treeList = new List<ComboTree>();
                if (dicUnitList.Count == 0)
                {
                    treeList.Add(new ComboTree { id = "", text = "无记录" });
                }
                else
                {
                    foreach (var item in dicUnitList)
                    {
                        DataRow dr = (DataRow)item.Value;
                        if (dr["u_IsEnable"].ToString().Trim() == "True")
                        {
                            if (user_Type.ToString().Trim() == "1")
                            {
                                treeList.Add(new ComboTree { id = dr["u_Id"].ToString().Trim(), text = dr["u_Name"].ToString().Trim(), ParentId = dr["u_ParentId"].ToString().Trim() });
                            }
                            else if (user_Type.ToString().Trim() == "2")
                            {
                                if (dr["u_FullPath"].ToString().Trim().Equals(ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString().Trim()) || dr["u_FullPath"].ToString().Trim().Contains(ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString().Trim()) || dr["u_ParentId"].ToString().Trim() == ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim())
                                {
                                    treeList.Add(new ComboTree { id =dr["u_Id"].ToString().Trim(), text = dr["u_Name"].ToString().Trim(), ParentId = dr["u_ParentId"].ToString().Trim() });
                                }
                            }
                            else
                            {
                                if (dr["u_FullPath"].ToString().Trim().Equals(ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString().Trim()) || dr["u_FullPath"].ToString().Trim().EndsWith(ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString().Trim()))
                                {
                                    treeList.Add(new ComboTree { id = dr["u_Id"].ToString().Trim(), text = dr["u_Name"].ToString().Trim(), ParentId = dr["u_ParentId"].ToString().Trim() });
                                }
                            }
                        }
                    }
                }
                reTreeList = new List<ComboTree>();
                GetComboTree(treeList, UintID);
                string json = JsonUtil.ToJson(reTreeList);
                return new OperationResult(OperationResultType.Success, "查询结果成功", reTreeList);
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UnitManageService").Error("GetUnitComboTree 发生异常", ex);
            }
            return new OperationResult(OperationResultType.Success, "查询结果成功", new List<ComboTree>() { new ComboTree { id = "", text = "无记录" } });
        }

        /// <summary>
        /// 获取左侧机构树数据
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private static void GetComboTree(List<ComboTree> list, string UintID)
        {
            var user_Type = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString();
            var children = list.Where(x => x.ParentId == UintID).ToList();
            if (user_Type.ToString() != "1")
                children = list.Where(x => x.id == UintID).ToList();

            if (children.Count > 0)
            {
                foreach (var item in children)
                {
                    BindChildren(list, item);
                    reTreeList.Add(item);
                }
            }
        }

        /// <summary>
        /// 获取树子节点
        /// </summary>
        /// <param name="list"></param>
        /// <param name="model"></param>
        private static void BindChildren(List<ComboTree> list, ComboTree model)
        {
            var children = list.Where(x => x.ParentId == model.id).ToList();
            if (children.Count > 0)
            {
                model.nodes = children;
                foreach (var item in children)
                {
                    BindChildren(list, item);
                }
            }
        }

        /// <summary>
        /// 新建单位
        /// </summary>
        /// <param name="unit">unit键值对</param>
        /// <returns></returns>
        public static OperationResult AddUnit(Dictionary<string,object> unit)
        {

            //声明返回值
            OperationResult ret = new OperationResult(OperationResultType.Success);
           
            try
            {
                //检查参数
                PublicHelper.CheckArgument(unit, "unit");
            }
            catch (Exception ex) {
                Logger.GetLogger("unit").Error("新建单位AddUnit传入参数错误", ex);
                return new OperationResult(OperationResultType.ParamError, ex.Message);
            }

            try
            {
                using (var tran = new TransactionScope()) {

                    //获取当前用户信息
                    var UserID = ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim();
                    Dictionary<string, object> dicConditions = new Dictionary<string, object>();
                    dicConditions.Add("Us_id", UserID);
                    var retdata = DataBll.GetModel("", "t_Users", dicConditions).Tables[0];
                    var User = retdata.Rows[0];

                    //判断单位名称是否重复
                    int count = DataBll.GetCount("t_Units", "u_Name='" + unit["u_Name"] + "'");
                    if (count > 0)
                    {
                        ret.Message = "该单位已存在";
                        ret.ResultType = OperationResultType.Error;
                        return ret;
                    }

                    //计算单位的全路径和深度
                    if (unit["u_ParentId"].ToString() != null && unit["u_ParentId"].ToString() != "")
                    {
                        var dicCondition = new Dictionary<string, object>();
                        dicCondition.Add("u_Id", unit["u_ParentId"]);
                        var rettarunit = DataBll.GetModel("u_FullPath,u_Depth", "t_Units", dicCondition);
                        unit["u_FullPath"] = rettarunit.Tables[0].Rows[0]["u_FullPath"].ToString().Trim() + "-" + unit["u_Name"].ToString().Trim();
                        unit["u_Depth"] = int.Parse(rettarunit.Tables[0].Rows[0]["u_Depth"].ToString().Trim()) + 1;
                    }
                    else
                    {
                        unit["u_FullPath"] = unit["u_Name"].ToString().Trim();
                        unit["u_Depth"] = 0;
                    }

                    //插入单位信息
                    var guid = Guid.NewGuid();
                    unit.Add("u_Id", guid);
                    unit.Add("u_DataTime", DateTime.Now.Date.ToString().Trim());
                    object unitRe = DataBll.Add(unit, "t_Units");                   
                    if (null != unitRe)
                    {
                        //添加操作日志和用户日志                        
                        UserLogService.InsertLog(User["Us_account"].ToString(), User["Us_name"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "单位管理-新增单位", "成功新增单位，单位为：" + unit["u_Name"] + "！", "");
                        OperationLogService.InsertLog(User["Us_id"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "成功新增单位，单位为：" + unit["u_Name"] + "！", "");
                        ret.ResultType = OperationResultType.Success;
                        ret.Message = "新增单位成功!";
                    }
                    else
                    {
                        //添加操作日志和用户日志                        
                        UserLogService.InsertLog(User["Us_account"].ToString(), User["Us_name"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "单位管理-新增单位", "新增单位失败！", "");
                        OperationLogService.InsertLog(User["Us_id"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "新增单位失败！", "");
                        ret.ResultType = OperationResultType.Error;
                        ret.Message = "新增单位失败!";
                    }

                    //将新增的单位信息存入内存中的单位信息
                    if (unitRe != null)
                    {                        
                        ServiceBase.dicUnitList.Add(((DataSet)unitRe).Tables[0].Rows[0]["u_Id"].ToString().Trim(), ((DataSet)unitRe).Tables[0].Rows[0]);
                        //新增的单位按创建时间升序排列
                        ServiceBase.dicUnitList = ServiceBase.dicUnitList.OrderBy(x => DateTime.Parse(((DataRow)x.Value)["u_DataTime"].ToString())).ToDictionary(x => x.Key, o => o.Value);

                    }
                    tran.Complete();
                }
                  
            }
            catch (Exception ex) {
                Logger.GetLogger("unit").Error("新建单位AddUnit执行过程发生错误", ex);
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
            return ret;
        }


        /// <summary>
        /// 修改单位
        /// </summary>
        /// <param name="unit">unit键值对</param>
        /// <returns></returns>
        public static OperationResult EditUnit(Dictionary<string, object> unit)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
           
            try
            {
                //检查参数
                PublicHelper.CheckArgument(unit, "unit");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("unit").Error("修改单位EditUnit传入参数错误", ex);
                return new OperationResult(OperationResultType.ParamError, ex.Message);
            }

            try
            {
                using (var tran = new TransactionScope())
                {
                    //判断单位名称是否重复
                    int count = DataBll.GetCount("t_Units", "u_Name='" + unit["u_Name"] + "' and u_Id != '" + unit["u_Id"] + "'");
                    if (count > 0)
                    {
                        ret.Message = "该单位已存在";
                        ret.ResultType = OperationResultType.Error;
                        return ret;
                    }

                    //修改单位和下级单位的全路径
                    DataSet dsOld = DataBll.Query("select u_Name from t_Units where u_Id = '" + unit["u_Id"] + "'");
                    string nameOld = dsOld.Tables[0].Rows[0]["u_Name"].ToString().Trim();//修改前的单位全称
                    string ids = "";
                    if (unit["childIds"].ToString() == "")
                    {
                        ids = "'" + unit["u_Id"] + "'";
                    }
                    else
                    {
                        ids = "'" + unit["u_Id"].ToString() + "'," + unit["childIds"].ToString();  //本单位和下级单位的id
                    }
                    string[] arrs = ids.Split(',');
                    var sql = "select u_FullPath,u_Id,u_ParentId,u_DataTime from t_Units where u_Id in (" + ids + ") order by u_DataTime asc";
                    DataTable dt = DataBll.Query(sql).Tables[0];//修改前本级和下级单位的全路径
                    var fullpath = "";
                    Dictionary<string, string> FullNameDic = new Dictionary<string, string>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        fullpath = dt.Rows[i]["u_FullPath"].ToString();
                        string[] arrF = fullpath.Split('-');
                        if (i == 0)//修改的本机构
                        {
                            string RemoveStr = arrF[arrF.Length - 1];
                            fullpath = fullpath.TrimEnd(RemoveStr.ToCharArray()) + unit["u_Name"].ToString();
                        }
                        else  //非本机构
                        {
                            fullpath = "";
                            for (int j = 0; j < arrF.Length; j++) {
                                if (arrF[j] == nameOld) {
                                    arrF[j] = unit["u_Name"].ToString().Trim();                                  
                                }
                                fullpath += arrF[j].ToString().Trim() + "-";
                            }
                            fullpath = fullpath.TrimEnd('-');                      
                        }
                        DataBll.Query("update t_Units set u_FullPath = '" + fullpath + "' where u_Id = " + arrs[i]);
                    }
                    unit.Remove("childIds");

                    //修改用户表(t_Users)，客户端表(t_Clients)，项目表(t_Projects)中的单位名称
                    updateUnitName("Unit_name",unit["u_Name"].ToString().Trim(), "t_Users", "UnitID",unit["u_Id"].ToString().Trim(), "Us_id");
                    updateUnitName("c_UnitName", unit["u_Name"].ToString().Trim(), "t_Clients", "c_UnitId", unit["u_Id"].ToString().Trim(), "c_Id");
                    updateUnitName("p_UnitName", unit["u_Name"].ToString().Trim(), "t_Projects", "p_UnitId", unit["u_Id"].ToString().Trim(), "p_Id");

                    //修改单位其他信息
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("u_Id", unit["u_Id"]);
                    Boolean result = DataBll.Update(unit, "t_Units", dic, "u_Id");

                    if (result)
                    {
                        ret.ResultType = OperationResultType.Success;
                        ret.Message = "修改单位成功!";
                    }
                    else
                    {
                        ret.ResultType = OperationResultType.Error;
                        ret.Message = "修改单位失败!";
                    }

                    //修改成功，更新内存中的单位信息
                    if (result )
                    {
                        string str = "select * from t_Units where u_Id = '" + unit["u_Id"] + "'";
                        DataSet ds = DataBll.Query(str);
                        Dictionary<string, object> dicUnitList = ServiceBase.dicUnitList;
                        dicUnitList[(ds).Tables[0].Rows[0]["u_Id"].ToString().Trim()] = ds.Tables[0].Rows[0];
                    }

                    tran.Complete();
                }               
            }                 
            catch (Exception ex)
            {
                Logger.GetLogger("unit").Error("修改单位EditUnit执行过程发生错误", ex);
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
            return ret;
        }

        /// <summary>
        /// 修改单位名称时，修改单位关联表中的单位名
        /// </summary>
        /// <param name="unitName">单位名称在关联表中的字段名称</param>
        /// <param name="unitNameValue">修改后的单位名称</param>
        /// <param name="tableName">关联表的表名</param>
        /// <param name="unitId">单位id在关联表中的字段名称</param>
        /// <param name="unitIdValue">单位id</param>
        /// <param name="key">关联表的主键</param>
        /// <returns></returns>
        public static object updateUnitName(string unitName,string unitNameValue,string tableName,string unitId,string unitIdValue,string key)
        {            
            var obj = new object();
            Dictionary<string, object> dicUnitName = new Dictionary<string, object>();
            dicUnitName.Add(unitName, unitNameValue);
            Dictionary<string, object> dicUs = new Dictionary<string, object>();
            dicUs.Add(unitId, unitIdValue);
            DataBll.Update(dicUnitName, tableName, dicUs, key);
            return obj;
        }
            

        /// <summary>
        /// 删除单位
        /// </summary>
        /// <param name="ids">删除的单位id(可删除多个单位)</param>
        /// <returns></returns>
        public static OperationResult DeleteUnit(string ids) {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                //检查参数
                PublicHelper.CheckArgument(ids, "ids");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UnitManageService").Error("DeleteUnit 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }

            try
            {
                using (var tran = new TransactionScope())
                {
                    string[] arrids = ids.Split(',');                    
                    for (int i = 0; i < arrids.Length; i++)
                    {
                        //判断此单位是否被使用过，若使用过，则不能删除
                        string where = "UnitID = '" + arrids[i].ToString().Trim() + "'";
                        var count = DataBll.GetCount("t_users",where);
                        if (count > 0) {
                            return new OperationResult(OperationResultType.Error, "机构中已有用户，无法删除");
                        }
                        var count2 = DataBll.GetCount("t_UserGroup", where);
                        if (count2 > 0)
                        {
                            return new OperationResult(OperationResultType.Error, "机构中已有用户组，无法删除");
                        }
                        where = "p_UnitId = '" + arrids[i].ToString().Trim() + "'";
                        var count3 = DataBll.GetCount("t_Projects", where);
                        if (count3 > 0)
                        {
                            return new OperationResult(OperationResultType.Error, "机构中已有项目，无法删除");
                        }

                        where = "c_UnitId = '" + arrids[i].ToString().Trim() + "'";
                        var count4 = DataBll.GetCount("t_Clients", where);
                        if (count4 > 0)
                        {
                            return new OperationResult(OperationResultType.Error, "机构中已有客户端，无法删除");
                        }

                        
                        where = "UnitId = '" + arrids[i].ToString().Trim() + "' and DataSign != '1'";
                        var count5 = DataBll.GetCount("t_DetectionSettings", where);
                        if (count5 > 0)
                        {
                            return new OperationResult(OperationResultType.Error, "机构中已有检测设置数据，无法删除");
                        }

                        //修改子机构的全路径

                        //删除单位表(t_Units)信息
                        Dictionary<string, object> whereDic = new Dictionary<string, object>();
                        whereDic.Add("u_Id", arrids[i]);
                        DataBll.Delete(whereDic, "t_Units");
                    }

                    ret.ResultType = OperationResultType.Success;
                    ret.Message = "删除成功！";

                    //删除单位成功，更新内存中单位信息
                    if (ret.ResultType == OperationResultType.Success)
                    {
                        foreach (var item in arrids)
                        {                            
                            ServiceBase.dicUnitList.Remove(item);
                        }
                    }
                    tran.Complete();
                   
                }               
                }catch (Exception ex) {
                Logger.GetLogger("UnitManageService").Error("删除单位DeleteUnit执行过程发生异常", ex);
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
            return ret;
        }

        /// <summary>
        /// 启用，禁用机构
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static OperationResult ActiveUnits(string ids, string values)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                PublicHelper.CheckArgument("ids", ids);
                PublicHelper.CheckArgument("values", values);
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UnitManageService").Error("ActiveUnits发生异常", ex);
                return new OperationResult(OperationResultType.Error, "UnitManageService的方法ActiveUnits参数有误!");
            }
            try
            {
                string value = "";
                if (values == "1")
                {
                    value = "True";
                }
                else {
                    value = "False";
                }
                //StringBuilder sbSql = new StringBuilder();
                //string[] arrIds = ids.Split(',');
                //for (int i = 0; i < arrIds.Length; i++)
                //{
                //    sbSql.Append(";Update t_Units set u_IsEnable='" + value + "' where u_Id='" + arrIds[i] + "';");
                //}                
                using (var tran = new TransactionScope())
                {
                    string[] arrIds = ids.Split(',');
                    List<object> obList = new List<object>();
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    Dictionary<string, object> dicvalue = new Dictionary<string, object>();
                    dicvalue.Add("u_IsEnable", value);
                    for (int i = 0; i < arrIds.Length; i++)
                    {
                        dic.Clear();
                        dic.Add("u_Id", arrIds[i].ToString().Trim());
                        //object obj = DataBll.UpdateReturnObj(dicvalue, "t_Units", dic, "u_Id");
                        Boolean result = DataBll.Update(dicvalue, "t_Units", dic, "u_Id");
                        string sql = "select * from t_Units where u_Id = '" + arrIds[i] + "'";
                        DataSet ds = DataBll.Query(sql);                     
                        if (result)
                        {
                            obList.Add(ds);
                        }
                    }
                    //更新状态成功后，更新内存中单位信息
                    if (obList.Count > 0)
                    {
                        Dictionary<string, object> dicUnitList = ServiceBase.dicUnitList;
                        foreach (var item in obList)
                        {
                            dicUnitList[((DataSet)item).Tables[0].Rows[0]["u_Id"].ToString().Trim()] = ((DataSet)item).Tables[0].Rows[0];
                        }
                    }
                    tran.Complete();
                }             
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UnitManageService").Error("ActiveUnits发生异常", ex);
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
            return ret;
        }

      
    }
}
