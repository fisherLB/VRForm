//#define UNITTEST
/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Repository
 * 项目描述: 
 * 类 名 称: SqlServerHelper
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Repository
 * 文件名称: SqlServerHelper
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/2 8:51:28
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Repository
{
     public static class SqlServerHelper
    {
#if UNITTEST
        public static string SqlConnection =@"server=192.168.1.20\NEWSERVERSQL2008;uid=sa;pwd=taitan;database=GraphicsEvaluatePlatform"; //Common.Config.SQLConnString;
#else
        public static string SqlConnection = ConfigurationManager.AppSettings["SQLSERVERCONSTR"];
#endif
        /// <summary>
        /// 返回新增语句和新增信息参数集
        /// </summary>
        /// <param name="paramList">参键值对(字段的列名和值)</param>
        /// <param name="tableName">表名</param>
        /// <param name="returnmodel">是否返回更新对象（false不返回，TRUE返回）</param>
        /// <returns></returns>
        public static Dictionary<string, SqlParameter[]> GetInsertSqlParameter(Dictionary<string, object> paramList, string tableName)
        {
            Dictionary<string, SqlParameter[]> dic = new Dictionary<string, SqlParameter[]>();
            SqlParameter[] sbSqlPaRe = new SqlParameter[0];
            if (paramList != null)
                sbSqlPaRe = new SqlParameter[paramList.Count];
            List<SqlParameter> ListSqlPa = new List<SqlParameter>();
            StringBuilder sbSqlKey = new StringBuilder();
            StringBuilder sbSqlValue = new StringBuilder();
            StringBuilder sbSqlRet = new StringBuilder();
            sbSqlRet.Append(" Insert into " + tableName + " (");
            foreach (var item in paramList)
            {
                //    if (item.Key.ToString().ToLower() != "id")
                //    {
                sbSqlKey.Append(item.Key + ",");
                sbSqlValue.Append("@" + item.Key + ",");
                ListSqlPa.Add(new SqlParameter("@" + item.Key, item.Value.ToString()));
                // }

            }
            //新增成功后返回，主键
            string boolre = " output inserted.* ";


            sbSqlRet.Append(sbSqlKey.ToString().TrimEnd(',') + ") " + boolre + " values(" + sbSqlValue.ToString().TrimEnd(',') + ") ;");
            sbSqlPaRe = ListSqlPa.ToArray();


            dic.Add(sbSqlRet.ToString(), sbSqlPaRe);
            return dic;
        }

        public static Dictionary<string, SqlParameter[]> GetInsertSqlParameter(Dictionary<string, object> paramList, string tableName, string column_id)
        {
            Dictionary<string, SqlParameter[]> dic = new Dictionary<string, SqlParameter[]>();
            SqlParameter[] sbSqlPaRe = new SqlParameter[0];
            if (paramList != null)
                sbSqlPaRe = new SqlParameter[paramList.Count];
            List<SqlParameter> ListSqlPa = new List<SqlParameter>();
            StringBuilder sbSqlKey = new StringBuilder();
            StringBuilder sbSqlValue = new StringBuilder();
            StringBuilder sbSqlRet = new StringBuilder();
            sbSqlRet.Append(" Insert into " + tableName + " (");
            foreach (var item in paramList)
            {
                if (item.Key.ToString() != column_id)
                {
                    sbSqlKey.Append(item.Key + ",");
                    sbSqlValue.Append("@" + item.Key + ",");
                    ListSqlPa.Add(new SqlParameter("@" + item.Key, item.Value.ToString()));
                }

            }
            //新增成功后返回，主键
            string boolre = " output inserted." + column_id + " ";


            sbSqlRet.Append(sbSqlKey.ToString().TrimEnd(',') + ") " + boolre + " values(" + sbSqlValue.ToString().TrimEnd(',') + ") ;");
            sbSqlPaRe = ListSqlPa.ToArray();


            dic.Add(sbSqlRet.ToString(), sbSqlPaRe);
            return dic;
        }


        /// 更新数据sql参数化
        /// </summary>
        /// <param name="paramList">参键值对(字段的列名和值)集合</param>
        /// <param name="TableName">数据表名称</param>
        /// <param name="type">curd类型(0:插入,1:更新,2:删除,3:查询)</param>
        /// <param name="whereList">条件键值对(字段的列名和值)集合</param>
        /// <param name="id">数据表主键字段名</param>
        /// <returns></returns>
        public static Dictionary<string, SqlParameter[]> GetUpdateSqlParameter(Dictionary<string, object> paramList, string tableName, Dictionary<string, object> whereList, string id)
        {
            Dictionary<string, SqlParameter[]> dic = new Dictionary<string, SqlParameter[]>();
            SqlParameter[] sbSqlPaRe = new SqlParameter[0];
            if (paramList != null)
                sbSqlPaRe = new SqlParameter[paramList.Count];
            List<SqlParameter> ListSqlPa = new List<SqlParameter>();
            StringBuilder sbSqlKey = new StringBuilder();
            StringBuilder sbSqlValue = new StringBuilder();
            StringBuilder sbSqlRet = new StringBuilder();
            List<string> listKeys = new List<string>();

            sbSqlRet.Append(" Update " + tableName + " Set ");
            foreach (var item in paramList)
            {
                //更新时不更新主键，排除ID
                if (!(item.Key.ToString().ToLower() == id))
                {
                    sbSqlKey.Append(item.Key + "=@" + item.Key + ",");
                    listKeys.Add(item.Key.ToString());
                    ListSqlPa.Add(new SqlParameter("@" + item.Key, item.Value.ToString()));
                }
            }
            sbSqlRet.Append(sbSqlKey.ToString().TrimEnd(','));

            if (whereList != null && whereList.Count > 0)
            {
                sbSqlRet.Append(" output inserted.* where ");
                foreach (var item in whereList)
                {
                    sbSqlValue.Append(item.Key + "=@" + item.Key + " and ");
                    if (!listKeys.Contains(item.Key.ToString()))
                    {
                        ListSqlPa.Add(new SqlParameter("@" + item.Key, item.Value.ToString()));
                    }
                }
            }

            sbSqlRet.Append(StringUtil.RemoveLastChar(sbSqlValue.ToString(), " and "));
            sbSqlPaRe = ListSqlPa.ToArray();
            dic.Add(sbSqlRet.ToString(), sbSqlPaRe);
            return dic;
        }

        /// <summary>
        /// 返回删除语句和删除参数集
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="whereList">删除条件键值对</param>
        /// <returns></returns>
        public static Dictionary<string, SqlParameter[]> GetDeleteSqlParameter(string tableName, Dictionary<string, object> whereList)
        {
            Dictionary<string, SqlParameter[]> dic = new Dictionary<string, SqlParameter[]>();
            SqlParameter[] sbSqlPaRe = new SqlParameter[0];

            List<SqlParameter> ListSqlPa = new List<SqlParameter>();
            StringBuilder sbSqlKey = new StringBuilder();

            StringBuilder sbSqlRet = new StringBuilder();
            sbSqlKey.Append(" Delete from " + tableName + " ");
            if (whereList.Count > 0)
            {
                sbSqlKey.Append(" where ");
            }
            foreach (var item in whereList)
            {
                sbSqlKey.Append(item.Key + "=@" + item.Key + ",");
                ListSqlPa.Add(new SqlParameter("@" + item.Key, item.Value.ToString()));
            }
            sbSqlRet.Append(sbSqlKey.ToString().TrimEnd(','));
            sbSqlPaRe = ListSqlPa.ToArray();


            dic.Add(sbSqlRet.ToString(), sbSqlPaRe);
            return dic;
        }



        /// <summary>
        /// 返回查询语句和查询参数集
        /// </summary>
        /// <param name="cols">要查询的列（默认为"",返回所有列；指定返回列，如：age,id,username）</param>
        /// <param name="tableName">查询的表名</param>
        /// <param name="whereList">查询条件键值对</param>
        /// <returns></returns>
        public static Dictionary<string, SqlParameter[]> GetSelectSqlParameter(string cols, string tableName, Dictionary<string, object> whereList)
        {
            Dictionary<string, SqlParameter[]> dic = new Dictionary<string, SqlParameter[]>();
            SqlParameter[] sbSqlPaRe = new SqlParameter[0];

            List<SqlParameter> ListSqlPa = new List<SqlParameter>();
            StringBuilder sbSqlKey = new StringBuilder();
            StringBuilder sbSqlValue = new StringBuilder();
            StringBuilder sbSqlRet = new StringBuilder();


            sbSqlKey.Append(" Select ");
            if (cols == "")
            {
                sbSqlKey.Append(" * from " + tableName + " with(nolock)");
            }
            else
            {
                sbSqlKey.Append(cols + " from " + tableName + " with(nolock)");
            }

            if (whereList.Count > 0)
            {
                sbSqlValue.Append(" where ");
                foreach (var item in whereList)
                {
                    sbSqlValue.Append(item.Key + "=@" + item.Key + ",");
                    ListSqlPa.Add(new SqlParameter("@" + item.Key, item.Value.ToString()));
                }
            }

            sbSqlRet.Append(sbSqlKey.ToString().TrimEnd(',') + sbSqlValue.ToString().TrimEnd(',').Replace(",", " and "));

            sbSqlPaRe = ListSqlPa.ToArray();

            dic.Add(sbSqlRet.ToString(), sbSqlPaRe);
            return dic;
        }


        /// <summary>
        /// 返回查询语句和查询参数集
        /// </summary>
        /// <param name="cols">要查询的列（默认为"",返回所有列；指定返回列，如：age,id,username）</param>
        /// <param name="tableName">查询的表名</param>
        /// <param name="whereList">查询条件键值对</param>
        /// <param name="orderby">排序条件,如：orderby id desc</param>
        /// <returns></returns>
        public static Dictionary<string, SqlParameter[]> GetSelectSqlParameter(string cols, string tableName, Dictionary<string, object> whereList, string orderby)
        {
            Dictionary<string, SqlParameter[]> dic = new Dictionary<string, SqlParameter[]>();
            SqlParameter[] sbSqlPaRe = new SqlParameter[0];

            List<SqlParameter> ListSqlPa = new List<SqlParameter>();
            StringBuilder sbSqlKey = new StringBuilder();
            StringBuilder sbSqlValue = new StringBuilder();
            StringBuilder sbSqlRet = new StringBuilder();


            sbSqlKey.Append(" Select ");
            if (cols == "")
            {
                sbSqlKey.Append(" * from " + tableName + " ");
            }
            else
            {
                sbSqlValue.Append(cols + " from " + tableName + " ");
            }

            if (whereList.Count > 0)
            {
                sbSqlValue.Append(" where ");
                int i = 1;
                foreach (var item in whereList)
                {
                    if (i < whereList.Count)
                    {
                        sbSqlValue.Append(item.Key + "=@" + item.Key + " and ");
                        ListSqlPa.Add(new SqlParameter("@" + item.Key, item.Value.ToString()));
                    }
                    else
                    {
                        sbSqlValue.Append(item.Key + "=@" + item.Key + "");
                        ListSqlPa.Add(new SqlParameter("@" + item.Key, item.Value.ToString()));
                    }
                    i++;
                }
            }

            sbSqlRet.Append(sbSqlKey.ToString().TrimEnd(',') + sbSqlValue.ToString().TrimEnd(','));

            if (orderby != "")
                sbSqlRet.Append("  " + orderby);
            sbSqlPaRe = ListSqlPa.ToArray();

            dic.Add(sbSqlRet.ToString(), sbSqlPaRe);
            return dic;
        }

        public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnection))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return ds;
                }
            }
        }

        public static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {


                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }


        /// <summary>
        /// 为执行命令预备一个 SqlCommand 对象
        /// </summary>
        /// <param name="cmd">SqlCommand 对象</param>
        /// <param name="conn">SqlConnection 对象</param>
        /// <param name="trans">SqlTransaction 对象</param>
        /// <param name="cmdType">命令类型 (stored procedure, text, etc.)</param>
        /// <param name="cmdText">存储过程名或 T-SQL 命令</param>
        /// <param name="cmdParms">执行命令所需的 SqlParamters 数组</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        /// <summary>
        /// 一次插入大批量数据
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool SqlBulkCopyByDatatable(string TableName, DataTable dt)
        {
            string connString = SqlConnection;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(SqlConnection, SqlBulkCopyOptions.UseInternalTransaction | SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.KeepNulls))
                {
                    sqlbulkcopy.BatchSize = dt.Rows.Count;
                    try
                    {
                        sqlbulkcopy.DestinationTableName = TableName;
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        }
                        sqlbulkcopy.WriteToServer(dt);
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnection))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }


        /// <summary>
        /// 2012-2-21新增重载，执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="connection">SqlConnection对象</param>
        /// <param name="trans">SqlTransaction事件</param>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(SqlConnection connection, SqlTransaction trans, string SQLString)
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, connection))
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.Transaction = trans;
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    //trans.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// 通过数据库连接串执行一个SQL命令 (返回一个结果集) 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteSqlGetDataSet( CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">命令类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名或 T-SQL 命令</param>
        /// <param name="commandParameters">执行命令所需的 SqlParamters 数组</param>
        /// <returns>返回一个 DataSet 对象包含执行的结果集</returns>
        public static DataSet ExecuteSqlGetDataSet(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            string connString = SqlConnection;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    SqlCommand cmd = new SqlCommand();
                    DataSet ds = new DataSet();
                    try
                    {
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                        sda.SelectCommand = cmd;

                        sda.Fill(ds);
                        cmd.Parameters.Clear();

                        return ds;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static bool TabExists(string TableName)
        {
            string strsql = "select count(*) from sysobjects where id = object_id(N'[" + TableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            //string strsql = "SELECT count(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + TableName + "]') AND type in (N'U')";
            object obj = DbHelperSQL.GetSingle(strsql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查字段是否存在
        /// </summary>
        /// <param name="sTblName"></param>
        /// <param name="sFldName"></param>
        /// <returns></returns>
        public static bool CheckField(String sTblName, String sFldName)
        {
            bool isExist = false;
            try
            {
                SqlConnection aConnection = new SqlConnection(SqlConnection);
                aConnection.Open();

                DataSet ds = new DataSet();
                SqlDataAdapter ape = new SqlDataAdapter("select name from syscolumns where id in(select id from sysobjects where name='" + sTblName + "') and name='" + sFldName + "'", aConnection);
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

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns></returns>
        public static object GetMax(string tablename, string col)
        {
            string sql = "select max(" + col + ") as val  from " + tablename;
            DataSet ds = Query(sql);

            if (ds == null)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return 0;

            return ds.Tables[0].Rows[0]["val"].ToString();
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns></returns>
        public static object GetMax(string tablename, string col, string where)
        {
            string sql = "select max(" + col + ") as val  from " + tablename + " where " + where;
            DataSet ds = Query(sql);

            if (ds == null)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return 0;

            return ds.Tables[0].Rows[0]["val"].ToString();
        }

        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="col"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static object GetMin(string tablename, string col, string where)
        {
            string sql = "select min(" + col + ") as val  from " + tablename + " where " + where;
            DataSet ds = Query(sql);

            if (ds == null)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return 0;

            return ds.Tables[0].Rows[0]["val"].ToString();
        }
    }
}
