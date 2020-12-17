//#define UNITTEST
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using System.Linq;

namespace  GraphicsEvaluatePlatform.Repository
{
    /// <summary>
    /// SQL数据库操作类
    /// </summary>
    public class SqlDataBase : IDataBase
    {
        //数据库链接串
#if UNITTEST
        public static string SqlConnection =@"server=192.168.1.20\NEWSERVERSQL2008;uid=sa;pwd=taitan;database=GraphicsEvaluatePlatform"; //Common.Config.SQLConnString;
#else
        public static string SqlConnection = ConfigurationManager.AppSettings["SQLSERVERCONSTR"];
#endif
        //分页存储过程
        public static string SP_NAME = ConfigurationManager.AppSettings["SPName"] == null ? "sp_page" : ConfigurationManager.AppSettings["SPName"];

        public SqlDataBase()
        {
            
        }

        public SqlDataBase(string constr)
        {
            SqlConnection = constr;
        }


       

        /// <summary>
        /// 检查字段是否存在
        /// </summary>
        /// <param name="sTblName"></param>
        /// <param name="sFldName"></param>
        /// <returns></returns>
        public bool CheckField(String sTblName, String sFldName)
        {
            return SqlServerHelper.CheckField(sTblName, sFldName);
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns></returns>
        public virtual object GetMax(string tablename, string col)
        {
            return SqlServerHelper.GetMax(tablename, col);
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns></returns>
        public virtual object GetMax(string tablename, string col, string where)
        {
            return SqlServerHelper.GetMax(tablename,where, col);

        }

        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <returns></returns>
        public virtual object GetMin(string tablename, string col, string where)
        {
            return SqlServerHelper.GetMin(tablename, col, where);

        }

        public  bool ConnectionTest(out string msg, string connectStr)
        {
            bool IsCanConnectioned = false;
             msg = "链接成功";
            //创建连接对象
             SqlConnection mySqlConnection = new SqlConnection(connectStr);
            //ConnectionTimeout 在.net 1.x 可以设置 在.net 2.0后是只读属性，则需要在连接字符串设置
            //如：server=.;uid=sa;pwd=;database=PMIS;Integrated Security=SSPI; Connection Timeout=30
            //mySqlConnection.ConnectionTimeout = 1;//设置连接超时的时间
            try
            {
                //Open DataBase
                //打开数据库
              
                mySqlConnection.Open();
                IsCanConnectioned = true;
            }
            catch(Exception ex)
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
            //mySqlConnection   is   a   SqlConnection   object 
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

        public bool ConnectionTest(string constr,out string msg)
        {
            bool IsCanConnectioned = false;
            msg = "链接成功";
            //创建连接对象
            SqlConnection mySqlConnection = new SqlConnection(constr);
            //ConnectionTimeout 在.net 1.x 可以设置 在.net 2.0后是只读属性，则需要在连接字符串设置
            //如：server=.;uid=sa;pwd=;database=PMIS;Integrated Security=SSPI; Connection Timeout=30
            //mySqlConnection.ConnectionTimeout = 1;//设置连接超时的时间
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
                msg = "数据库链接失败：" + constr + ex.Message + "请检查数据库是否已经安装";
            }
            finally
            {
                //Close DataBase
                //关闭数据库连接
                mySqlConnection.Close();
            }
            //mySqlConnection   is   a   SqlConnection   object 
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
        public DataSet Add(Dictionary<string, object> dic, string tableName)
        {
            Dictionary<string, SqlParameter[]> di = SqlServerHelper.GetInsertSqlParameter(dic, tableName);
            List<SqlParameter[]> listParams = new List<SqlParameter[]>();
            DataSet ds = new DataSet();

            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                ds = SqlServerHelper.ExecuteSqlGetDataSet(CommandType.Text, sql, listParams[0]);
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
            Dictionary<string, SqlParameter[]> di = SqlServerHelper.GetInsertSqlParameter(dic, tableName, column_id);
            List<SqlParameter[]> listParams = new List<SqlParameter[]>();
            object obj = new object();
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {

                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                obj = SqlServerHelper.GetSingle(sql, listParams[0]);
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
            Dictionary<string, SqlParameter[]> di = SqlServerHelper.GetUpdateSqlParameter(dic, tableName, whereList, id);

            List<SqlParameter[]> listParams = new List<SqlParameter[]>();
            bool result = false;
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                int rows = SqlServerHelper.ExecuteSql(sql.ToString(), listParams[0]);
                if (rows > 0)
                {
                    result = true;
                }

            }
            return result;
        }

        public bool Delete(Dictionary<string, object> whereList, string tableName)
        {
            Dictionary<string, SqlParameter[]> di = SqlServerHelper.GetDeleteSqlParameter(tableName, whereList);

            List<SqlParameter[]> listParams = new List<SqlParameter[]>();
            bool result = false;
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                int rows = SqlServerHelper.ExecuteSql(sql.ToString(), listParams[0]);
                if (rows > 0)
                {
                    result = true;
                }

            }
            return result;
        }

        public DataSet GetList(string cols, string tableName, Dictionary<string, object> whereList)
        {
            Dictionary<string, SqlParameter[]> di = SqlServerHelper.GetSelectSqlParameter(cols, tableName, whereList);
            List<SqlParameter[]> listParams = new List<SqlParameter[]>();
            DataSet ds = new DataSet();
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                ds = SqlServerHelper.Query(sql.ToString(), listParams[0]);
            }
            return ds;
        }

        public DataSet GetList(string cols, string tableName, Dictionary<string, object> whereList, string orderby)
        {
            Dictionary<string, SqlParameter[]> di = SqlServerHelper.GetSelectSqlParameter(cols, tableName, whereList, orderby);
            List<SqlParameter[]> listParams = new List<SqlParameter[]>();
            DataSet ds = new DataSet();
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                ds = SqlServerHelper.Query(sql.ToString(), listParams[0]);
            }
            return ds;
        }

        public DataSet Query(string sql)
        {
            DataSet ds = new DataSet();
            ds = SqlServerHelper.Query(sql);
            return ds;
        }


        public DataSet GetModel(string cols, string tableName, Dictionary<string, object> whereList)
        {
            Dictionary<string, SqlParameter[]> di = SqlServerHelper.GetSelectSqlParameter(cols, tableName, whereList);
            List<SqlParameter[]> listParams = new List<SqlParameter[]>();
            DataSet ds = new DataSet();
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                ds = SqlServerHelper.Query(sql.ToString(), listParams[0]);
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
            return SqlServerHelper.Query(sql);
        }

        public int GetCount(string tableName, string where)
        {
            string sql = "select Count(*) from " + tableName + " where " + (where == "" ? "1=1" : where);
            return Convert.ToInt32(SqlServerHelper.GetSingle(sql));
        }

        public int GetCount(string sql)
        {
            string str = sql;
            return Convert.ToInt32(SqlServerHelper.GetSingle(str));
        }

        public int CreatTable(string sql)
        {
            return SqlServerHelper.ExecuteSql(sql);
        }

        public bool TableExists(string tableName)
        {
            return SqlServerHelper.TabExists(tableName);
        }

        public bool SqlBulkCopyByDatatable(string TableName, DataTable dt)
        {
            return SqlServerHelper.SqlBulkCopyByDatatable(TableName, dt);
        }
    }
}
