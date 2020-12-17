using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using System.Linq;

namespace  GraphicsEvaluatePlatform.Repository
{
    public class OracleDataBase : IDataBase
    {
        static string OracleConnection= ConfigurationManager.AppSettings["ORACLECONSTR"];

        public OracleDataBase()
        {

        }

        public OracleDataBase(string constr)
        {
            OracleConnection = constr;
        }

        #region 验证字段是否存在
        /// <summary>
        /// 验证字段是否存在
        /// </summary>
        /// <param name="sTblName"></param>
        /// <param name="sFldName"></param>
        /// <returns></returns>
        public bool CheckField(string sTblName, string sFldName)
        {
            bool isExist = false;
            try
            {
                OracleConnection aConnection = new OracleConnection(OracleConnection);
                aConnection.Open();
                DataSet ds = new DataSet();
                OracleDataAdapter ape = new OracleDataAdapter("select name from syscolumns where id in(select id from sysobjects where name='" + sTblName + "') and name='" + sFldName + "'", aConnection);
                ape.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    isExist = false;
                }
                else
                {
                    isExist = true;
                }
                aConnection.Close();
            }
            catch (Exception err)
            {
                return false;
            }
            return isExist;
        }
        #endregion

        #region 验证数据库连接
        /// <summary>
        /// 验证数据库连接
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ConnectionTest(out string msg,string connectStr)
        {
            bool IsCanConnectioned = false;
            msg = "链接成功";
            //创建连接对象
            OracleConnection mySqlConnection = new OracleConnection(connectStr);
            try
            {
                //Open DataBase
                //打开数据库
                mySqlConnection.Open();
                IsCanConnectioned = true;
            }
            catch (Exception ex)
            {
                //Can not Open DataBase
                //打开不成功 则连接不成功
                IsCanConnectioned = false;
                msg = "数据库链接失败：" + ex.Message + "" + connectStr;
            }
            finally
            {
                //Close DataBase
                //关闭数据库连接
                mySqlConnection.Close();
            }
         
            if (mySqlConnection.State == ConnectionState.Closed || mySqlConnection.State == ConnectionState.Broken)
            {
                //Connection   is   not   available  
                return IsCanConnectioned;
            }
            else
            {
                //Connection   is   available  
                return IsCanConnectioned;
            }
        }
        #endregion
     

        #region  获取最大值
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public object GetMax(string tablename, string col)
        {
            return OracleDBHelper.CheckField(tablename, col);
        }
        #endregion
        #region 获取最大值（带where）
        public object GetMax(string tablename, string col, string where)
        {
            return OracleDBHelper.GetMax(tablename, col, where);
        }
        #endregion
        #region 获取最小值
        public object GetMin(string tablename, string col, string where)
        {
            return OracleDBHelper.GetMin(tablename, col, where);
        }
        #endregion
    
     
        public DataSet Add(Dictionary<string, object> dic, string tableName)
        {
            Dictionary<string, OracleParameter[]> di = OracleDBHelper.GetInsertOracleParameter(dic, tableName);
            List<OracleParameter[]> listParams = new List<OracleParameter[]>();
            DataSet ds = new DataSet();

            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                ds = OracleDBHelper.ExecuteSqlGetDataSet(CommandType.Text, sql, listParams[0]);
                //返回插入数据的ID
                return ds;
            }
            else
            {
                return null;
            }
        }

        public object Add(Dictionary<string, object> dic, string tableName, string column_id)
        {
            Dictionary<string, OracleParameter[]> di = OracleDBHelper.GetInsertOracleParameter(dic, tableName, column_id);
            List<OracleParameter[]> listParams = new List<OracleParameter[]>();
            object obj = new object();
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {

                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                obj = OracleDBHelper.GetSingle(sql, listParams[0]);
                //返回插入数据的ID
                return obj;
            }
            else
            {
                return null;
            }
        }

        public bool Update(Dictionary<string, object> dic, string tableName, Dictionary<string, object> whereList, string id)
        {
            Dictionary<string, OracleParameter[]> di = OracleDBHelper.GetUpdateOracleParameter(dic, tableName, whereList, id);

            List<OracleParameter[]> listParams = new List<OracleParameter[]>();
            bool result = false;
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                int rows = OracleDBHelper.ExecuteSql(sql.ToString(), listParams[0]);
                if (rows > 0)
                {
                    result = true;
                }

            }
            return result;
        }

        public bool Delete(Dictionary<string, object> whereList, string tableName)
        {
            Dictionary<string, OracleParameter[]> di = OracleDBHelper.GetDeleteOracleParameter(tableName, whereList);

            List<OracleParameter[]> listParams = new List<OracleParameter[]>();
            bool result = false;
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                int rows = OracleDBHelper.ExecuteSql(sql.ToString(), listParams[0]);
                if (rows > 0)
                {
                    result = true;
                }

            }
            return result;
        }

        public DataSet GetList(string cols, string tableName, Dictionary<string, object> whereList)
        {
            Dictionary<string, OracleParameter[]> di = OracleDBHelper.GetSelectOracleParameter(cols, tableName, whereList);
            List<OracleParameter[]> listParams = new List<OracleParameter[]>();
            DataSet ds = new DataSet();
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                ds = OracleDBHelper.Query(sql.ToString(), listParams[0]);
            }
            return ds;
        }

        public DataSet GetList(string cols, string tableName, Dictionary<string, object> whereList, string orderby)
        {
            Dictionary<string, OracleParameter[]> di = OracleDBHelper.GetSelectOracleParameter(cols, tableName, whereList, orderby);
            List<OracleParameter[]> listParams = new List<OracleParameter[]>();
            DataSet ds = new DataSet();
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                ds = OracleDBHelper.Query(sql.ToString(), listParams[0]);
            }
            return ds;
        }

        public DataSet Query(string sql)
        {
            DataSet ds = new DataSet();
            ds = OracleDBHelper.Query(sql);
            return ds;
        }


        public DataSet GetModel(string cols, string tableName, Dictionary<string, object> whereList)
        {
            Dictionary<string, OracleParameter[]> di = OracleDBHelper.GetSelectOracleParameter(cols, tableName, whereList);
            List<OracleParameter[]> listParams = new List<OracleParameter[]>();
            DataSet ds = new DataSet();
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                ds = OracleDBHelper.Query(sql.ToString(), listParams[0]);
            }
            return ds;
        }

        public DataSet GetDataSetList(string tableName, int pageNum, int currentlyPage, string cols, string where, string sort, string col)
        {
            int startIndex = 1;
            int endIndex = 1;
            if (pageNum < 1)
            {
                pageNum = 10;
            }
            if (currentlyPage < 1)
            {
                currentlyPage = 1;
            }
            startIndex = (currentlyPage - 1) * pageNum;
            endIndex = currentlyPage * pageNum;
            string sqlTable = tableName;
            string sqlSelect = "";
            if (cols == "")
                sqlSelect = "*";
            else
                sqlSelect = cols;
            string wsbql = "";
            if (where != "")
                wsbql += " where " + where;


            string sortsql = "order by " + sort;
            string sbsql = "";
            if (cols == "")
                sbsql = "w1.*";
            if (cols != "")
            {
                if (cols.Contains(','))
                {
                    string[] Arr = cols.Split(',');
                    foreach (var item in Arr)
                    {
                        sbsql += "w1." + item + ",";
                    }
                }
                else
                {
                    sbsql = "w1." + cols;
                }
            }
            sbsql = sbsql.TrimEnd(',');
            string sql = "SELECT " + sbsql + " FROM " + tableName + " w1,";
            sql += "(SELECT row_number() OVER (" + sortsql + " ) n," + col;
            sql += " FROM  " + tableName + wsbql + ") w2 ";
            sql += "WHERE w1." + col + " = w2." + col + " AND w2.n > " + startIndex + " and w2.n<=" + endIndex + " order by w2.n asc";
            return OracleDBHelper.Query(sql);
        }

        public int GetCount(string tableName, string where)
        {
            string sql = "select Count(*) from " + tableName + " where " + (where == "" ? "1=1" : where);
            return Convert.ToInt32(OracleDBHelper.GetSingle(sql));
        }

        public int GetCount(string sql)
        {
            string str = sql;
            return Convert.ToInt32(OracleDBHelper.GetSingle(str));
        }

        public int CreatTable(string sql)
        {
            return OracleDBHelper.ExecuteSql(sql);
        }

        public bool TableExists(string tableName)
        {
            return OracleDBHelper.TabExists(tableName);
        }

        public bool SqlBulkCopyByDatatable(string TableName, DataTable dt)
        {
            return OracleDBHelper.SqlBulkCopyByDatatable(TableName, dt);
        }
    }
}
