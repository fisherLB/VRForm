/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2017. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using GraphicsEvaluatePlatform.Model;

/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Infrastructure 
 * 项目描述: 
 * 类 名 称: DataBll
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Infrastructure
 * 文件名称: DataBll
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/4/6 10:10:38
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Repository
{
    public class DataBll
    {
        /// <summary>
        /// 新增数据，返回插入对象
        /// </summary>
        /// <param name="dic">参数键值对</param>
        /// <param name="tableName">表名</param>
        /// <param name="returnmodel">是否返回更新对象（false不返回，TRUE返回）</param>
        /// <returns></returns>
        public static DataSet Add(Dictionary<string, object> dic, string tableName)
        {
            var dbProvider = DBFactory.GetDB();
            return dbProvider.Add(dic, tableName);
        }

        /// <summary>
        /// 新增数据，返回主键值（若主键为int自增类型，将object类型转换为int类型，判断是否新增成功；若主键为GUID类型将object转换为stringl类型）
        /// </summary>
        /// <param name="dic">参数键值对</param>
        /// <param name="tableName">表名</param>
        /// <param name="column_id">主键列名</param>
        /// <returns></returns>
        public static object Add(Dictionary<string, object> dic, string tableName, string column_id)
        {
            var dbProvider = DBFactory.GetDB();
            return dbProvider.Add(dic, tableName,column_id);
        }

      

      
     

        /// <summary>
        /// 修改数据,返回是否修改成功
        /// </summary>
        /// <param name="dic">参数键值对</param>
        /// <param name="tableName">表名</param>
        /// <param name="whereList">条件键值对</param>
        /// <param name="id">数据表的主键名</param>
        /// <returns></returns>
        public static bool Update(Dictionary<string, object> dic, string tableName, Dictionary<string, object> whereList, string id)
        {
            var dbProvider = DBFactory.GetDB();
            return dbProvider.Update(dic, tableName, whereList,id);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="whereList">条件键值对</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static bool Delete(Dictionary<string, object> whereList, string tableName)
        {
            var dbProvider = DBFactory.GetDB();
            return dbProvider.Delete(whereList, tableName);
        }


        /// <summary>
        /// 查询返回列表（不含分页）
        /// </summary>
        /// <param name="cols">要查询的列（默认为"",返回所有列；指定返回列，如：age,id,username）</param>
        /// <param name="tableName">查询的表名</param>
        /// <param name="whereList">查询条件键值对</param>
        /// <returns></returns>
        public static DataSet GetList(string cols, string tableName, Dictionary<string, object> whereList)
        {
            var dbProvider = DBFactory.GetDB();
            return dbProvider.GetList(cols, tableName,whereList);
        }



        /// <summary>
        /// 查询返回列表（不含分页）
        /// </summary>
        /// <param name="cols">要查询的列（默认为"",返回所有列；指定返回列，如：age,id,username）</param>
        /// <param name="tableName">查询的表名</param>
        /// <param name="whereList">查询条件键值对</param>
        /// <param name="orderby">排序条件，如：orderby Id desc</param>
        /// <returns></returns>
        public static DataSet GetList(string cols, string tableName, Dictionary<string, object> whereList, string orderby)
        {
            var dbProvider = DBFactory.GetDB();
            return dbProvider.GetList(cols, tableName, whereList,orderby);
        }



       

     


        /// <summary>
        /// 直接执行sql返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet Query(string sql)
        {
            var dbProvider = DBFactory.GetDB();
            return dbProvider.Query(sql);
        }

      

        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereList"></param>
        /// <returns></returns>
        public static DataSet GetModel(string cols, string tableName, Dictionary<string, object> whereList)
        {
            var dbProvider = DBFactory.GetDB();
            return dbProvider.GetModel(cols,tableName,whereList);
        }

        /// <summary>
        /// 获取分页数据集
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pageNum">每页的数据数</param>
        /// <param name="currentlyPage">当前页</param>
        /// <param name="cols">展现的字段，如：age,id,name</param>
        /// <param name="where">查询条件（如：id>1）</param>
        /// <param name="sort">排序方式（如age desc,id asc）</param>
        /// <param name="col">查询一个列名,必填(如：age)</param>
        /// <returns></returns>
        public static DataSet GetDataSetList(string tableName, int pageNum, int currentlyPage, string cols, string where, string sort, string col)
        {
            var dbProvider = DBFactory.GetDB();
            return dbProvider.GetDataSetList(tableName, pageNum, currentlyPage,cols,where,sort,col);
        }


        /// <summary>
        /// 获取记录数量
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public static int GetCount(string tableName, string where)
        {

            var dbProvider = DBFactory.GetDB();
            return dbProvider.GetCount(tableName,where);
        }
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="sql"><查询语句/param>
        /// <returns></returns>
        public static int GetCount(string sql)
        {
            var dbProvider = DBFactory.GetDB();
            return dbProvider.GetCount(sql);
        }
        

        /// <summary>
        /// 生成数据表
        /// </summary>
        /// <param name="sql">生成语句</param>
        /// <returns></returns>
        public static int CreatTable(string sql)
        {
            var dbProvider = DBFactory.GetDB();
            return dbProvider.CreatTable(sql);
        }

        public static bool ConnectionTest(out string msg,string connectStr)
        {
            var dbProvider = DBFactory.GetDB();
            return dbProvider.ConnectionTest(out msg, connectStr);
        }


        /// <summary>
        /// 是否存在表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>

        public static bool TableExists(string tableName)
        {
            var dbProvider = DBFactory.GetDB();
            return dbProvider.TableExists(tableName);
        }


        /// <summary>
        /// 一次插入大批量数据
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool SqlBulkCopyByDatatable(string TableName, DataTable dt)
        {
            var dbProvider = DBFactory.GetDB();
            return dbProvider.SqlBulkCopyByDatatable(TableName,dt);

        }
        /// <summary>
        /// 修改数据,返回是修改之后的记录
        /// </summary>
        /// <param name="dic">参数键值对</param>
        /// <param name="tableName">表名</param>
        /// <param name="whereList">条件键值对</param>
        /// <param name="id">数据表的主键名</param>
        /// <returns></returns>
        public static object UpdateReturnObj(Dictionary<string, object> dic, string tableName, Dictionary<string, object> whereList, string id)
        {
            Dictionary<string, SqlParameter[]> di = SQLBuilder.GetUpdateSqlParameter(dic, tableName, whereList, id);

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

                obj = DbHelperSQL.ExecuteSqlGetDataSet(CommandType.Text, sql, listParams[0]);
                //返回插入数据的ID
                return obj;


            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 返回事务提交类
        /// </summary>
        /// <param name="paramList">键值对</param>
        /// <param name="tableName">表名</param>
        /// <param name="column_id">主键名</param>
        /// <returns></returns>
        public static TranParam GetInsertTranParam(Dictionary<string, object> paramList, string tableName, string column_id)
        {
            TranParam tp = new TranParam();

            Dictionary<string, SqlParameter[]> di = SQLBuilder.GetInsertSqlParameter(paramList, tableName, column_id);
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

                tp.sql = sql;
                tp.sqlParam = listParams[0];
            }
            return tp;
        }
    }
}
