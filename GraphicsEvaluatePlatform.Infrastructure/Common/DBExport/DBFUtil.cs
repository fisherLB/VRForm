using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;

namespace GraphicsEvaluatePlatform.Infrastructure.Common.DBExport
{
    /// <summary>
    /// DBF数据库操作
    /// </summary>
    public class DBFUtil
    {
        public DBFUtil(string dbpath)
        {
            string connStr = @"Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=" + dbpath + ";Exclusive=No;NULL=NO;Collate=Machine;BACKGROUNDFETCH=NO;DELETED=NO";

            //string connStr = @"Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=" + dbpath + ";Exclusive=No;NULL=NO;Collate=Machine;BACKGROUNDFETCH=NO;DELETED=NO";
            conn = new OdbcConnection(connStr);
            tablename = dbpath;
        }

        public DBFUtil()
        {

        }

        string tablename = "";

        OdbcConnection conn = null;

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <returns></returns>
        public DataSet GetData()
        {
            try
            {
                conn.ConnectionTimeout = 500;
                conn.Open();

                string sql = "select *  from " + tablename + "  ";
                OdbcDataAdapter da = new OdbcDataAdapter(sql, conn);

                DataSet ds = new DataSet();
                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch
            {
                conn.Close();
                return null;
            }

        }

        public DataSet GetData(string cols)
        {
            try
            {
                conn.ConnectionTimeout = 500;
                conn.Open();

                string sql = "select  " + cols + "  from " + System.IO.Path.GetFileName(tablename) + "  ";
                OdbcDataAdapter da = new OdbcDataAdapter(sql, conn);

                DataSet ds = new DataSet();
                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch
            {

                conn.Close();
                return null;
            }

        }

        public DataSet Test()
        {
            try
            {
                conn.ConnectionTimeout = 500;
                conn.Open();

                string sql = "select SYS(2015) tt ";
                OdbcDataAdapter da = new OdbcDataAdapter(sql, conn);

                DataSet ds = new DataSet();
                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch
            {

                conn.Close();
                return null;
            }

        }

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="ds"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string ExportData(string path, string filename, DataSet ds, params string[] filter)
        {
            string fullname = CreateTable(path, filename, ds.Tables[0].Columns, filter);
            try
            {
                if (ds.Tables[0].Rows.Count == 0)
                    return "";

                if (fullname == "")
                    return "";
                string tablename = System.IO.Path.GetFileName(fullname);

                string sql = "";

                //string ole_connstring = @"Provider=microsoft.jet.oledb.4.0;Data Source=" + path + ";Extended Properties=dBASE IV;";
                string ole_connstring = "Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=" + path + "; ";
                if (myconn == null)
                {
                    //myconn = new System.Data.OleDb.OleDbConnection(ole_connstring);
                }
                OdbcConnection myconn1 = new OdbcConnection(ole_connstring);
                myconn1.Open();
                //System.Data.OleDb.OleDbCommand cmd = null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string col = "";
                    string val = "";
                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {

                        if (Isfilter(filter, dc.ColumnName))
                            //字段过滤
                            continue;

                        if (dc.ColumnName.Length > 8)
                            continue;

                        string v = dr[dc].ToString();

                        //剔除内容中的换行(dbf内容中换行符号会导致错误)
                        v = v.Replace("\r\n", " ").Replace("\n", " ");

                        v = Strings.StringUtil.GetSubString(v, 220);

                        if (val == "")
                            val = "'" + v + "'";
                        else
                            val += "," + "'" + v + "'";

                        if (col == "")
                            col = dc.ColumnName;
                        else
                            col += "," + dc.ColumnName;
                    }



                    sql = "Insert Into " + tablename + " values(" + val + ")";
                    OdbcCommand cmd = new OdbcCommand(sql, myconn1);
                    //cmd = new System.Data.OleDb.OleDbCommand(sql, myconn);
                    cmd.ExecuteNonQuery();

                }



                myconn1.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fullname;

        }


        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="ds"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string ExportDataIn(string path, string filename, DataSet ds, params string[] infilter)
        {
            if (ds.Tables[0].Rows.Count == 0)
                return "";
            string fullname = CreateTableIn(path, filename, ds.Tables[0].Columns, infilter);
            if (fullname == "")
                return "";
            string tablename = System.IO.Path.GetFileName(fullname);

            string sql = "";

            //string ole_connstring = @"Provider=microsoft.jet.oledb.4.0;Data Source=" + path + ";Extended Properties=dBASE IV;";
            string ole_connstring = "Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=" + path + "; ";
            if (myconn == null)
            {
                //myconn = new System.Data.OleDb.OleDbConnection(ole_connstring);
            }
            OdbcConnection myconn1 = new OdbcConnection(ole_connstring);
            myconn1.Open();
            //System.Data.OleDb.OleDbCommand cmd = null;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string col = "";
                string val = "";
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {

                    if (!Isfilter(infilter, dc.ColumnName))
                        //字段过滤
                        continue;

                    if (dc.ColumnName.Length > 8)
                    {

                    }

                    string v = dr[dc].ToString();

                    //剔除内容中的换行(dbf内容中换行符号会导致错误)
                    v = v.Replace("\r\n", " ").Replace("\n", " ");

                    if (val == "")
                        val = "'" + v + "'";
                    else
                        val += "," + "'" + v + "'";

                    if (col == "")
                        col = dc.ColumnName;
                    else
                        col += "," + dc.ColumnName;
                }



                sql = "Insert Into " + tablename + " values(" + val + ",sys(2015)" + ")";
                OdbcCommand cmd = new OdbcCommand(sql, myconn1);
                //cmd = new System.Data.OleDb.OleDbCommand(sql, myconn);
                cmd.ExecuteNonQuery();

            }



            myconn1.Close();


            return fullname;

        }


        OleDbConnection myconn = null;
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="dcs"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string CreateTable(string path, string filename, DataColumnCollection dcs, params string[] filter)
        {

            if (dcs == null)
                return "";
            if (dcs.Count == 0)
                return "";

            string fullname = Strings.StringUtil.GetFileName(path + "\\" + filename);
            string fullnamenew = System.IO.Path.GetFileName(fullname);

            //string connect = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+path+";Extended Properties=dBASE IV;";
            string connect = "Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=" + path + "; ";
            //string connStr = @"Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=" + path + ";Exclusive=No;NULL=NO;Collate=Machine;BACKGROUNDFETCH=NO;DELETED=NO";


            //myconn = new OleDbConnection(connect);
            OdbcConnection myconn = new OdbcConnection(connect);
            string sqlt = "";

            foreach (DataColumn dc in dcs)
            {
                string col = dc.ColumnName.Replace("/", "");

                if (Isfilter(filter, col))
                    //字段过滤
                    continue;

                if (col.Length > 6)
                    continue;

                if (sqlt == "")
                    sqlt = col + " Character(254)";
                else
                    sqlt += "," + col + " Character(254)";
            }

            sqlt = "CREATE TABLE " + fullname + " (" + sqlt + ")";

            myconn.Open();

            //OleDbCommand olec = new OleDbCommand(sqlt, myconn);
            OdbcCommand olec = new OdbcCommand(sqlt, myconn);
            try
            {

                int i = olec.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                olec.Dispose();
                myconn.Close();
            }

            return fullname;


        }

        public string CreateTableIn(string path, string filename, DataColumnCollection dcs, params string[] filter)
        {

            if (dcs == null)
                return "";
            if (dcs.Count == 0)
                return "";

            string fullname = Common.Strings.StringUtil.GetFileName(path + "\\" + filename);
            string fullnamenew = System.IO.Path.GetFileName(fullname);

            //string connect = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+path+";Extended Properties=dBASE IV;";
            string connect = "Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=" + path + "; ";
            //string connStr = @"Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=" + path + ";Exclusive=No;NULL=NO;Collate=Machine;BACKGROUNDFETCH=NO;DELETED=NO";


            //myconn = new OleDbConnection(connect);
            OdbcConnection myconn = new OdbcConnection(connect);
            string sqlt = "";

            foreach (DataColumn dc in dcs)
            {
                string col = dc.ColumnName.Replace("/", "");

                if (!Isfilter(filter, col))
                    //字段过滤
                    continue;

                if (col.Length > 8)
                {
                    col = col.Substring(0, 5);
                }

                if (sqlt == "")
                    sqlt = col + " Character(254)";
                else
                    sqlt += "," + col + " Character(254)";
            }

            sqlt = "CREATE TABLE " + fullname + " (" + sqlt + ",sskey Character(254) " + ")";

            myconn.Open();

            //OleDbCommand olec = new OleDbCommand(sqlt, myconn);
            OdbcCommand olec = new OdbcCommand(sqlt, myconn);
            try
            {

                int i = olec.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                olec.Dispose();
                myconn.Close();
            }

            return fullname;


        }


        //是否过滤
        private bool Isfilter(string[] filter, string col)
        {
            if (filter != null)
            {//过滤字段
                foreach (string s in filter)
                {
                    if (col.ToLower() == s.ToLower())
                        return true;
                }
            }

            return false;
        }

        public bool CreateDBF(string filename)
        {
            bool r = false;
            string outconnstring = string.Format("Provider = Microsoft.Jet.OLEDB.4.0 ;Data Source ={0};Extended Properties=dBASE 5.0;", filename);
            OleDbConnection outConn = new OleDbConnection(outconnstring);
            OleDbCommand dc = outConn.CreateCommand();

            try
            {
                outConn.Open();
                dc.CommandType = CommandType.Text;
                dc.CommandText = "CREATE TABLE aa.DBF(tt varchar(50))";
                dc.ExecuteNonQuery();
                r = true;
            }
            catch
            {
                return false;
            }
            finally
            {
                dc.Dispose();
                if (outConn.State == System.Data.ConnectionState.Open)
                    outConn.Close();
                outConn.Dispose();
            }
            return r;
        }


        public void DBFExport(string path)
        {

            string connect = "Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=" + path + "; ";
            OdbcConnection myconn = new OdbcConnection(connect);
            string sqlt = "CREATE TABLE aa.DBF (cc int(10))";

            myconn.Open();
            OdbcCommand olec = new OdbcCommand(sqlt, myconn);
            try
            {
                int i = olec.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                olec.Dispose();
                myconn.Close();
            }
        }
    }
}
