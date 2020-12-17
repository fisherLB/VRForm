using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;

namespace GraphicsEvaluatePlatform.Repository
{
    /// <summary>
    /// sql语句生产类
    /// </summary>
    public class SQLBuilder
    {

        /// sql参数化
        /// </summary>
        /// <param name="paramList">参键值对(字段的列名和值)</param>
        /// <param name="TableName">数据表名称</param>
        /// <param name="type">curd类型(0:插入,1:更新,2:删除,3:查询)</param>
        /// <param name="whereList">条件键值对(字段的列名和值)</param>
        /// <returns></returns>
        public static Dictionary<string, SqlParameter[]> GetSqlParameter(Dictionary<string, object> paramList, string tableName, int? type, Dictionary<string, object> whereList)
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
            switch (type)
            {
                //插入
                case 0:
                    sbSqlRet.Append(" Insert into " + tableName + " (");
                    foreach (var item in paramList)
                    {
                        if (item.Key.ToString().ToLower() != "id")
                        {
                            sbSqlKey.Append(item.Key + ",");
                            sbSqlValue.Append("@" + item.Key + ",");
                            ListSqlPa.Add(new SqlParameter("@" + item.Key, item.Value.ToString()));
                        }

                    }
                    sbSqlRet.Append(sbSqlKey.ToString().TrimEnd(',') + ")values(" + sbSqlValue.ToString().TrimEnd(',') + ") ; select @@IDENTITY;");
                    sbSqlPaRe = ListSqlPa.ToArray();
                    break;
                //更新
                case 1:
                    sbSqlRet.Append(" Update " + tableName + " Set ");
                    foreach (var item in paramList)
                    {
                        if (!(item.Key.ToString().ToLower() == "id"))
                        {
                            sbSqlKey.Append(item.Key + "=@" + item.Key + ",");
                            listKeys.Add(item.Key.ToString());
                            ListSqlPa.Add(new SqlParameter("@" + item.Key, item.Value.ToString()));
                        }
                    }
                    sbSqlRet.Append(sbSqlKey.ToString().TrimEnd(','));

                    if (whereList != null && whereList.Count > 0)
                    {
                        sbSqlRet.Append(" where ");
                        foreach (var item in whereList)
                        {
                            sbSqlValue.Append(item.Key + "=@" + item.Key + ",");
                            if (!listKeys.Contains(item.Key.ToString()))
                            {
                                ListSqlPa.Add(new SqlParameter("@" + item.Key, item.Value.ToString()));
                            }

                        }
                    }
                    sbSqlRet.Append(sbSqlValue.ToString().TrimEnd(','));
                    sbSqlPaRe = ListSqlPa.ToArray();
                    break;
                //删除
                case 2:
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
                    break;
                //查询
                case 3:
                    sbSqlKey.Append(" Select ");
                    if (paramList == null || paramList.Count == 0)
                    {
                        sbSqlKey.Append(" * from " + tableName + " ");
                    }
                    else
                    {
                        foreach (var item in paramList)
                        {
                            sbSqlKey.Append(item.Key + ",");
                        }
                        sbSqlValue.Append(sbSqlKey.ToString().TrimEnd(',') + " from " + tableName + " ");
                        if (whereList.Count > 0)
                        {
                            sbSqlValue.Append(" where ");
                            foreach (var item in whereList)
                            {
                                sbSqlValue.Append(item.Key + "=@" + item.Key + ",");
                                ListSqlPa.Add(new SqlParameter("@" + item.Key, item.Value.ToString()));
                            }
                        }
                    }
                    sbSqlRet.Append(sbSqlKey.ToString().TrimEnd(',') + sbSqlValue.ToString().TrimEnd(','));
                    sbSqlPaRe = ListSqlPa.ToArray();
                    break;
                default:
                    break;
            }
            dic.Add(sbSqlRet.ToString(), sbSqlPaRe);
            return dic;
        }


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="tableName"></param>
        /// <param name="colums_id"></param>
        /// <returns></returns>
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






        /// <summary>
        /// sql语句拼接函数
        /// </summary>
        /// <param name="paramList">参键值对(字段的列名和值)集合</param>
        /// <param name="TableName">数据表名称</param>
        /// <param name="type">curd类型(0:插入,1:更新,2:删除,3:查询)</param>
        /// <param name="whereList">条件键值对(字段的列名和值)集合</param>
        /// <returns></returns>
        public static string GetSql(Dictionary<string, object> paramList, string tableName, int? type, Dictionary<string, object> whereList)
        {
            if (tableName == null || type == null)
            {
                return "";
            }
            StringBuilder sbSqlKey = new StringBuilder();
            StringBuilder sbSqlValue = new StringBuilder();
            StringBuilder sbSqlRet = new StringBuilder();
            switch (type)
            {
                //插入
                case 0:
                    sbSqlRet.Append(" Insert into " + tableName + " (");
                    foreach (var item in paramList)
                    {
                        sbSqlKey.Append(item.Key + ",");
                        sbSqlValue.Append("'" + item.Value.ToString() + "',");
                    }
                    sbSqlRet.Append(sbSqlKey.ToString().TrimEnd(',') + ")values(" + sbSqlValue.ToString().TrimEnd(',') + ")");
                    break;
                //更新
                case 1:
                    sbSqlKey.Append(" Update " + tableName + " Set ");
                    foreach (var item in paramList)
                    {
                        sbSqlKey.Append(item.Key + "='" + item.Value.ToString() + "',");
                    }
                    sbSqlRet.Append(sbSqlKey.ToString().TrimEnd(','));
                    if (whereList != null && whereList.Count > 0)
                    {
                        sbSqlRet.Append(" where ");
                        foreach (var item in whereList)
                        {
                            sbSqlValue.Append(item.Key + "='" + item.Value.ToString() + "',");
                        }
                    }
                    sbSqlRet.Append(sbSqlValue.ToString().TrimEnd(','));
                    break;
                //删除
                case 2:
                    sbSqlKey.Append(" Delete from " + tableName + " ");
                    if (whereList.Count > 0)
                    {
                        sbSqlKey.Append(" where ");
                    }
                    foreach (var item in whereList)
                    {
                        sbSqlKey.Append(item.Key + "='" + item.Value.ToString() + "',");
                    }
                    sbSqlRet.Append(sbSqlKey.ToString().TrimEnd(','));
                    break;
                //查询
                case 3:
                    sbSqlKey.Append(" Select ");
                    if (paramList == null || paramList.Count == 0)
                    {
                        sbSqlKey.Append(" * from " + tableName + " ");
                    }
                    else
                    {
                        foreach (var item in paramList)
                        {
                            sbSqlKey.Append(item.Key + ",");
                        }
                        sbSqlValue.Append(sbSqlKey.ToString().TrimEnd(',') + " from " + tableName + " ");
                        if (whereList.Count > 0)
                        {
                            sbSqlValue.Append(" where ");
                            foreach (var item in whereList)
                            {
                                sbSqlValue.Append(item.Key + "='" + item.Value.ToString() + "',");
                            }
                        }
                    }
                    sbSqlRet.Append(sbSqlValue.ToString().TrimEnd(','));
                    break;
                default:
                    break;
            }
            return sbSqlRet.ToString();
        }


        /// <summary>
        /// 生成数据表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="hasPrimaryKey">是否有主键（ID为主键）</param>
        /// <param name="isIdentity">主键是否自增长</param>
        /// <param name="paramList">表的字段键值对(字段的列名和生成列的类型)集合</param>
        /// <returns></returns>
        public static string CreateTableSql(string tableName, bool hasPrimaryKey, bool isIdentity, Dictionary<string, string> paramList)
        {
            StringBuilder sbSqlRet = new StringBuilder();
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" Create Table " + tableName + "(");
            if (paramList.Count() > 0)
            {
                foreach (var item in paramList)
                {
                    sbSql.Append(item.Key.ToString() + " " + item.Value.ToString());

                    if (item.Key.ToString().ToLower() == "id")
                    {
                        if (hasPrimaryKey)
                        {
                            sbSql.Append("  PRIMARY KEY");
                        }
                        if (isIdentity)
                        {
                            sbSql.Append(" IDENTITY(1,1)");
                        }
                    }
                    sbSql.Append(",");

                }
                sbSql.Remove(sbSql.Length - 1, 1);
            }

            sbSql.Append(")");
            return sbSql.ToString();
        }
    }
}
