//#define UNITTEST
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using System.Linq;

namespace  GraphicsEvaluatePlatform.Repository
{
    public class MySqlDataBase : IDataBase
    {
#if UNITTEST
        public static string MySqlConnetion = @"Host=localhost;UserName=root;Password=123456;Database=graphicsevaluateplatform;Port=3306;CharSet=utf8;Allow Zero Datetime=true;sslmode=none;"; 
#else
        static string MySqlConnetion = ConfigurationManager.AppSettings["MYSQLCONSTR"];
#endif


        public MySqlDataBase()
        {

        }

        public MySqlDataBase(string constr)
        {
            MySqlConnetion = constr;
        }

        #region 验证数据库连接
        /// <summary>
        /// 验证数据库连接
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ConnectionTest(out string msg,string  connectStr)
        {
            bool IsCanConnectioned = false;
            msg = "链接成功";
            //创建连接对象
            MySqlConnection mySqlConnection = new MySqlConnection(connectStr);
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
        #endregion
        #region 验证字段是否存在
        /// <summary>
        /// 验证字段是否存在
        /// </summary>
        /// <param name="sTblName"></param>
        /// <param name="sFldName"></param>
        /// <returns></returns>
        public bool CheckField(string sTblName, string sFldName)
        {
            return MySqlDBHelper.CheckField(sTblName, sFldName);
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
            return MySqlDBHelper.GetMax(tablename, col);
        }
        #endregion
        #region 获取最大值（带where）
        public object GetMax(string tablename, string col, string where)
        {
            return MySqlDBHelper.GetMax(tablename, col,where);
        }
        #endregion
        #region 获取最小值
        public object GetMin(string tablename, string col, string where)
        {
            return MySqlDBHelper.GetMin(tablename, col, where);
        }
        #endregion

        public DataSet Add(Dictionary<string, object> dic, string tableName)
        {
            Dictionary<string, MySqlParameter[]> di = MySqlDBHelper.GetInsertMySqlParameter(dic, tableName);
            List<MySqlParameter[]> listParams = new List<MySqlParameter[]>();
            DataSet ds = new DataSet();

            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                ds = MySqlDBHelper.ExecuteSqlGetDataSet(CommandType.Text, sql, listParams[0]);
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
            Dictionary<string, MySqlParameter[]> di = MySqlDBHelper.GetInsertMySqlParameter(dic, tableName, column_id);
            List<MySqlParameter[]> listParams = new List<MySqlParameter[]>();
            object obj = new object();
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {

                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                obj = MySqlDBHelper.GetSingle(sql, listParams[0]);
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
            Dictionary<string, MySqlParameter[]> di = MySqlDBHelper.GetUpdateMySqlParameter(dic, tableName, whereList, id);

            List<MySqlParameter[]> listParams = new List<MySqlParameter[]>();
            bool result = false;
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                int rows = MySqlDBHelper.ExecuteSql(sql.ToString(), listParams[0]);
                if (rows > 0)
                {
                    result = true;
                }

            }
            return result;
        }

        public bool Delete(Dictionary<string, object> whereList, string tableName)
        {
            Dictionary<string, MySqlParameter[]> di = MySqlDBHelper.GetDeleteMySqlParameter(tableName, whereList);

            List<MySqlParameter[]> listParams = new List<MySqlParameter[]>();
            bool result = false;
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                int rows = MySqlDBHelper.ExecuteSql(sql.ToString(), listParams[0]);
                if (rows > 0)
                {
                    result = true;
                }

            }
            return result;
        }

        public DataSet GetList(string cols, string tableName, Dictionary<string, object> whereList)
        {
            Dictionary<string, MySqlParameter[]> di = MySqlDBHelper.GetSelectMySqlParameter(cols, tableName, whereList);
            List<MySqlParameter[]> listParams = new List<MySqlParameter[]>();
            DataSet ds = new DataSet();
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                ds = MySqlDBHelper.Query(sql.ToString(), listParams[0]);
            }
            return ds;
        }

        public DataSet GetList(string cols, string tableName, Dictionary<string, object> whereList, string orderby)
        {
            Dictionary<string, MySqlParameter[]> di = MySqlDBHelper.GetSelectMySqlParameter(cols, tableName, whereList, orderby);
            List<MySqlParameter[]> listParams = new List<MySqlParameter[]>();
            DataSet ds = new DataSet();
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                ds = MySqlDBHelper.Query(sql.ToString(), listParams[0]);
            }
            return ds;
        }

        public DataSet Query(string sql)
        {
            DataSet ds = new DataSet();
            ds = MySqlDBHelper.Query(sql);
            return ds;
        }


        public DataSet GetModel(string cols, string tableName, Dictionary<string, object> whereList)
        {
            Dictionary<string, MySqlParameter[]> di = MySqlDBHelper.GetSelectMySqlParameter(cols, tableName, whereList);
            List<MySqlParameter[]> listParams = new List<MySqlParameter[]>();
            DataSet ds = new DataSet();
            if (di.Count > 0)
            {
                string sql = "";
                foreach (var item in di)
                {
                    sql = item.Key;
                    listParams.Add(item.Value);
                }
                ds = MySqlDBHelper.Query(sql.ToString(), listParams[0]);
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
            string sql = "SELECT " + sbsql + " FROM " + tableName + " w1   "+wsbql+sortsql+ "  LIMIT   "+startIndex+","+ pageNum;
            return MySqlDBHelper.Query(sql);
        }

        public int GetCount(string tableName, string where)
        {
            string sql = "select Count(*) from " + tableName + " where " + (where == "" ? "1=1" : where);
            return Convert.ToInt32(MySqlDBHelper.GetSingle(sql));
        }

        public int GetCount(string sql)
        {
            string str = sql;
            return Convert.ToInt32(MySqlDBHelper.GetSingle(str));
        }

        public int CreatTable(string sql)
        {
            return MySqlDBHelper.ExecuteSql(sql);
        }

        public bool TableExists(string tableName)
        {
            return MySqlDBHelper.TabExists(tableName);
        }

        public bool SqlBulkCopyByDatatable(string TableName, DataTable dt)
        {
            return MySqlDBHelper.SqlBulkCopyByDatatable(TableName, dt);
        }
    }
}
