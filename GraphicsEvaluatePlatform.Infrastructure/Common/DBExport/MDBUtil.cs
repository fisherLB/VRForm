using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Infrastructure.Common.DBExport
{
    public class MDBUtil
    {
        /// <summary>
        /// 将ds数据导出到文件中Access文件
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <param name="filename">文件名</param>
        /// <param name="tabName">表名</param>
        /// <param name="reMsg">返回消息</param>
        /// <returns>bool</returns>
        public bool DataSetExportToAccess(DataSet ds, string filename, String tabName, String reMsg)
        {
            #region 引用Interop.ADOX.dll

            if (ds == null || ds.Tables.Count <= 0)
            {
                reMsg = "目前无数据不需要导出";
                return false;
            }
            if (ds.Tables[0].Rows.Count <= 0)
            {
                reMsg = "目前无数据不需要导出";
                return false;
            }
            // 创建数据库
            if (!File.Exists(filename))
            {
                //ADOX.CatalogClass cat = new ADOX.CatalogClass();
                //cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";");
                //cat = null;
            }

            int rows = ds.Tables[0].Rows.Count;
            int cols = ds.Tables[0].Columns.Count;
            StringBuilder sb = new StringBuilder();
            string connString = String.Empty;
            connString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", filename);

            //创建表       
            sb.Append("CREATE TABLE " + tabName + " (");
            String colName = String.Empty;
            String colNames = String.Empty;
            String colNamePramas = String.Empty;
            String colType = String.Empty;
            for (int i = 0; i < cols; i++)
            {
                colName = ds.Tables[0].Columns[i].ColumnName.ToString();
                colType = ds.Tables[0].Columns[i].DataType.ToString();
                //colType = NetDataTypeToDataBaseType(colType);
                if (i == 0)
                {
                    sb.Append(colName + "  " + colType);
                    colNames += colName;
                    colNamePramas += "@" + colName;
                }
                else
                {
                    sb.Append(", " + colName + "  " + colType);
                    colNames += "," + colName;
                    colNamePramas += ",@" + colName;

                }

            }
            sb.Append(" )");
            if (colNames == String.Empty)
            {
                reMsg = "数据集的列数必须大于0";
                return false;
            }

            using (OleDbConnection objConn = new OleDbConnection(connString))
            {
                OleDbCommand objCmd = new OleDbCommand();
                objCmd.Connection = objConn;
                objCmd.CommandText = sb.ToString();
                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    reMsg = "在Excel中创建表失败，错误信息：" + e.Message;
                    return false;
                }



                //写数据
                sb.Remove(0, sb.Length);
                sb.Append(" insert into " + tabName + " (" + colNames + ") values(" + colNamePramas + " )");
                objCmd.CommandText = sb.ToString();
                OleDbParameterCollection param = objCmd.Parameters;
                for (int i = 0; i < cols; i++)
                {
                    colType = ds.Tables[0].Columns[i].DataType.ToString();
                    colName = ds.Tables[0].Columns[i].ColumnName.ToString();
                    if (colType == "System.String")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.VarChar));
                    }
                    else if (colType == "System.DateTime")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Date));

                    }
                    else if (colType == "System.Boolean")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Boolean));

                    }
                    else if (colType == "System.Decimal")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Decimal));

                    }
                    else if (colType == "System.Double")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Double));

                    }
                    else if (colType == "System.Single")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Single));

                    }
                    else if (colType == "System.Single")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Single));
                    }
                    else
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Integer));
                    }

                }

                //遍历DataTable将数据插入新建的Excel文件中
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    for (int i = 0; i < param.Count; i++)
                    {
                        param[i].Value = row[i];
                    }

                    objCmd.ExecuteNonQuery();
                }
            }
            reMsg = "数据成功导出";
            return true;

            #endregion
        }

        /// <summary>
        /// 将dt数据导出到文件中Access文件
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="filename">文件名</param>
        /// <param name="tabName">表名</param>
        /// <param name="reMsg">返回消息</param>
        /// <returns>bool</returns>
        public bool DataTableExportToAccess(DataTable dt, string filename, String tabName, ref String reMsg)
        {
            #region 引用Interop.ADOX.dll


            if (dt.Rows.Count <= 0)
            {
                reMsg = "目前无数据不需要导出";
                return false;
            }
            // 创建数据库
            if (!File.Exists(filename))
            {
              //  ADOX.CatalogClass cat = new ADOX.CatalogClass();
                //cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";");
                //cat = null;
            }

            int rows = dt.Rows.Count;
            int cols = dt.Columns.Count;
            StringBuilder sb = new StringBuilder();
            string connString = String.Empty;
            connString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", filename);

            //创建表       
            sb.Append("CREATE TABLE " + tabName + " (");
            String colName = String.Empty;
            String colNames = String.Empty;
            String colNamePramas = String.Empty;
            String colType = String.Empty;
            for (int i = 0; i < cols; i++)
            {
                colName = dt.Columns[i].ColumnName.ToString();
                colType = dt.Columns[i].DataType.ToString();
                colType = NetDataTypeToDataBaseType(colType);
                if (i == 0)
                {
                    sb.Append(colName + "  " + colType);
                    colNames += colName;
                    colNamePramas += "@" + colName;
                }
                else
                {
                    sb.Append(", " + colName + "  " + colType);
                    colNames += "," + colName;
                    colNamePramas += ",@" + colName;

                }

            }
            sb.Append(" )");
            if (colNames == String.Empty)
            {
                reMsg = "数据集的列数必须大于0";
                return false;
            }

            using (OleDbConnection objConn = new OleDbConnection(connString))
            {
                OleDbCommand objCmd = new OleDbCommand();
                objCmd.Connection = objConn;
                objCmd.CommandText = sb.ToString();
                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    reMsg = "在Access中创建表失败，错误信息：" + e.Message;
                    return false;
                }



                //写数据
                sb.Remove(0, sb.Length);
                sb.Append(" insert into " + tabName + " (" + colNames + ") values(" + colNamePramas + " )");
                objCmd.CommandText = sb.ToString();
                OleDbParameterCollection param = objCmd.Parameters;
                for (int i = 0; i < cols; i++)
                {
                    colType = dt.Columns[i].DataType.ToString();
                    colName = dt.Columns[i].ColumnName.ToString();
                    if (colType == "System.String")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.VarChar));
                    }
                    else if (colType == "System.DateTime")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Date));

                    }
                    else if (colType == "System.Boolean")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Boolean));

                    }
                    else if (colType == "System.Decimal")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Decimal));

                    }
                    else if (colType == "System.Double")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Double));

                    }
                    else if (colType == "System.Single")
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Single));

                    }
                    else
                    {
                        param.Add(new OleDbParameter("@" + colName, OleDbType.Integer));
                    }

                }

                //遍历DataTable将数据插入新建的文件中
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < param.Count; i++)
                    {
                        param[i].Value = row[i];
                    }

                    objCmd.ExecuteNonQuery();
                }
            }
            reMsg = "数据成功导出";
            return true;

            #endregion
        }

        private String NetDataTypeToDataBaseType(String DataType)
        {
            String reType = String.Empty;
            if (DataType.ToString() == "System.String")
            {
                reType = "varchar";
            }
            else if (DataType.ToString() == "System.Decimal")
            {
                reType = "number";
            }
            else if (DataType.ToString() == "System.DateTime")
            {
                reType = "datetime";

            }
            else
            {
                reType = "int";

            }
            return reType;

        }
    }
}
