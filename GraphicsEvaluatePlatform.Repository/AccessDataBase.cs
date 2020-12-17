using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace  GraphicsEvaluatePlatform.Repository
{
    /// <summary>
    /// ACCESS数据库操作类
    /// </summary>
    public class AccessDataBase : IDataBase
    {
        //数据库链接串
        public static string CONN_STRING ="";//Common.Config.OleDbConnectionString;
        //分页存储过程
        public static readonly string SP_NAME ="";// Common.Config.Sp_pageName;



        public AccessDataBase()
        { }

        public AccessDataBase(string filename)
        {
            CONN_STRING = "";// Common.Config.GetAccessStr(filename);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        public bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
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
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string sqlString)
        {
            using (OleDbConnection connection = new OleDbConnection(CONN_STRING))
            {

                using (OleDbCommand cmd = new OleDbCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.OleDb.OleDbException e)
                    {
                        connection.Close();
                        throw e;
                    }
                    finally
                    {
                        
                        connection.Close();
                    }
                }
            }
        }


        /// <summary>
        /// 检查字段是否存在
        /// </summary>
        /// <param name="sTblName"></param>
        /// <param name="sFldName"></param>
        /// <returns></returns>
        public bool CheckField(String sTblName, String sFldName)
        {
            bool isExist = false;
            try
            {
                OleDbConnection aConnection = new OleDbConnection(CONN_STRING);
                aConnection.Open();

                object[] oa ={ null, null, sTblName, sFldName };

                DataTable schemaTable = aConnection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Columns, oa);

                if (schemaTable.Rows.Count == 0)
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

        public string Test()
        {
            OleDbConnection aConnection = new OleDbConnection(CONN_STRING);
            aConnection.Open();
            OleDbCommand cmd = new OleDbCommand("select * from Doc_Data_149", aConnection);
            OleDbDataReader o = cmd.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            while (o.Read())
            {
                sb.Append(o.GetValue(1).ToString() + "=");
            }
            aConnection.Close();
            return sb.ToString();
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string SQLString)
        {
            OleDbConnection connection1 = new OleDbConnection(CONN_STRING);

            using (OleDbConnection connection = new OleDbConnection(CONN_STRING))
            {
                using (OleDbCommand cmd = new OleDbCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.OleDb.OleDbException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

     
        public DataSet Query(string SQLString)
        {
            try
            {
      
                using (OleDbConnection connection = new OleDbConnection(CONN_STRING))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        connection.Open();
                        OleDbDataAdapter command = new OleDbDataAdapter(SQLString, connection);
                        command.Fill(ds);
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
            catch (Exception ex)
            {
                //System.IO.File.AppendAllText(Common.Config.LogPathStr + "Log.txt", DateTime.Now.ToString() + "\r\n" + ex.Message + "\r\n", System.Text.Encoding.UTF8);
                throw ex;
            }
           
        }


        /// <summary>
        /// 获取分页好的结果集
        /// </summary>
        /// <param name="sqlSelect"></param>
        /// <param name="sqlTable"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public DataSet GetDataSetListBySp(string sqlSelect, string sqlTable, int startIndex, int endIndex)
        {
            return null;
        }


        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns></returns>
        public virtual object GetMax(string tablename, string col)
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
        public virtual object GetMax(string tablename, string col, string where)
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
        /// <returns></returns>
        public virtual object GetMin(string tablename, string col, string where)
        {

           // string sql = "select min(" + col + ") as val  from " + tablename + " where " + where;
            string sql = "select top 1  * from (select *, len(" + col + ") as clen  from " + tablename + " where " + where + " ) a order by clen," + col;
            DataSet ds = Query(sql);

            if (ds == null)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return 0;

            return ds.Tables[0].Rows[0][col].ToString();
        }

        ///// <summary>
        ///// 判断是否存在某表的某个字段
        ///// </summary>
        ///// <param name="tableName">表名称</param>
        ///// <param name="columnName">列名称</param>
        ///// <returns>是否存在</returns>
        //public bool ColumnExists(string tableName, string columnName)
        //{
        //    string sql = "select count(1) from syscolumns where [id]=object_id('" + tableName + "') and [name]='" + columnName + "'";
        //    object res = GetSingle(sql);
        //    if (res == null)
        //    {
        //        return false;
        //    }
        //    return Convert.ToInt32(res) > 0;
        //}



        public bool ConnectionTest(out string msg,string connectStr)
        {
            bool IsCanConnectioned = false;
            try
            {
        
                msg = "链接成功";
                //创建连接对象
                OleDbConnection connection = new OleDbConnection(connectStr);
                //ConnectionTimeout 在.net 1.x 可以设置 在.net 2.0后是只读属性，则需要在连接字符串设置
                //如：server=.;uid=sa;pwd=;database=PMIS;Integrated Security=SSPI; Connection Timeout=30
                //mySqlConnection.ConnectionTimeout = 1;//设置连接超时的时间
                try
                {
                    //Open DataBase
                    //打开数据库
                    connection.Open();
                    IsCanConnectioned = true;
                }
                catch (Exception ex)
                {
                    //Can not Open DataBase
                    //打开不成功 则连接不成功
                    IsCanConnectioned = false;
                    msg = "数据库链接失败：" + ex.Message + "请检查数据库文件是否已丢失（）" + connectStr;
                }
                finally
                {
                    //Close DataBase
                    //关闭数据库连接
                    connection.Close();
                }
                //mySqlConnection   is   a   SqlConnection   object 
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
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
            catch (Exception ex)
            {
                //System.IO.File.AppendAllText(Common.Config.LogPathStr + "Log.txt", DateTime.Now.ToString() + "\r\n" + ex.Message + "\r\n", System.Text.Encoding.UTF8);
                //Can not Open DataBase
                //打开不成功 则连接不成功
                IsCanConnectioned = false;
                msg = "数据库链接失败：" + ex.Message + "请检查数据库文件是否已丢失" + CONN_STRING;
                return IsCanConnectioned;

            }
        }


        /// <summary>
        /// 获取所有表名
        /// </summary>
        /// <returns></returns>
        public List<string> GetTableNames()
        {
            List<string> list = new List<string>();
            DataSet ds = Query("SELECT MSysObjects.Name FROM MsysObjects WHERE (Left([Name],1)<>\"~\") AND (Left$([Name],4) <> \"Msys\") AND (MSysObjects.Type)=1 ");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr["name"].ToString());
            }

            return list;
        }

        /// <summary>
        /// 获取所有视图名
        /// </summary>
        /// <returns></returns>
        public List<string> GetViewNames()
        {
            List<string> list = new List<string>();
            DataSet ds = Query("SELECT MSysObjects.Name FROM MsysObjects WHERE (Left([Name],1)<>\"~\") AND (MSysObjects.Type)=5 ORDER BY MSysObjects.Name");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr["name"].ToString());
            }

            return list;
        }


        public string GetFldType(string sTblName, string sFldName)
        {
            OleDbConnection aConnection = new OleDbConnection(CONN_STRING);
            aConnection.Open();

            object[] oa ={ null, null, sTblName, sFldName };

            DataTable schemaTable = aConnection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Columns, oa);


            string type = "";
            //foreach (DataColumn dc in schemaTable.Columns)
            //{
            //    type +=dc.ColumnName+ ",";
            //}

            foreach (DataRow dr in schemaTable.Rows)
            {
                int lx = int.Parse(dr["DATA_TYPE"].ToString());
                switch (lx)
                {
                    case 2: type = "int"; break;
                    case 3: type = "int"; break;
                    case 4: type = "Single"; break;
                    case 5: type = "double"; break;
                    case 6: type = "decimal"; break;
                    case 7: type = "DateTime"; break;
                    case 11: type = "bool"; break;
                    case 17: type = "byte"; break;
                    case 72: type = "varchar(255)"; break;
                    case 130: type = "varchar(255)"; break;
                    case 131: type = "decimal"; break;
                    case 128: type = "varchar(255)"; break;
                    default: type = "varchar(255)"; break;
                }

            }
            aConnection.Close();
            return type;
        }







        #region IDataBase 成员GetReader


        public System.Data.Common.DbDataReader GetReader(string sqlString)
        {
            OleDbConnection aConnection = null;
            try
            {
                aConnection = new OleDbConnection(CONN_STRING);
                aConnection.Open();
                OleDbCommand cmd = new OleDbCommand(sqlString, aConnection);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                aConnection.Close();
                return null;

                throw;
            }

        }

        public DataSet Add(Dictionary<string, object> dic, string tableName)
        {
            throw new NotImplementedException();
        }

        public object Add(Dictionary<string, object> dic, string tableName, string column_id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Dictionary<string, object> dic, string tableName, Dictionary<string, object> whereList, string id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Dictionary<string, object> whereList, string tableName)
        {
            throw new NotImplementedException();
        }

        public DataSet GetList(string cols, string tableName, Dictionary<string, object> whereList)
        {
            throw new NotImplementedException();
        }

        public DataSet GetList(string cols, string tableName, Dictionary<string, object> whereList, string orderby)
        {
            throw new NotImplementedException();
        }

        public DataSet GetModel(string cols, string tableName, Dictionary<string, object> whereList)
        {
            throw new NotImplementedException();
        }

        public DataSet GetDataSetList(string tableName, int pageNum, int currentlyPage, string cols, string where, string sort, string col)
        {
            throw new NotImplementedException();
        }

        public int GetCount(string tableName, string where)
        {
            throw new NotImplementedException();
        }

        public int GetCount(string sql)
        {
            throw new NotImplementedException();
        }

        public int CreatTable(string sql)
        {
            throw new NotImplementedException();
        }

        public bool TableExists(string tableName)
        {
            throw new NotImplementedException();
        }

        public bool SqlBulkCopyByDatatable(string TableName, DataTable dt)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
