using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace  GraphicsEvaluatePlatform.Repository
{
    public interface IDataBase
    {
        bool CheckField(String sTblName, String sFldName);

        object GetMax(string tablename, string col);

        object GetMax(string tablename, string col, string where);

        object GetMin(string tablename, string col, string where);


        bool ConnectionTest(out string msg,string connectStr);


        DataSet Add(Dictionary<string, object> dic, string tableName);

        object Add(Dictionary<string, object> dic, string tableName, string column_id);

        bool Update(Dictionary<string, object> dic, string tableName, Dictionary<string, object> whereList, string id);

        bool Delete(Dictionary<string, object> whereList, string tableName);

        DataSet GetList(string cols, string tableName, Dictionary<string, object> whereList);
        DataSet GetList(string cols, string tableName, Dictionary<string, object> whereList, string orderby);
        DataSet Query(string sql);

        DataSet GetModel(string cols, string tableName, Dictionary<string, object> whereList);

        DataSet GetDataSetList(string tableName, int pageNum, int currentlyPage, string cols, string where, string sort, string col);

        int GetCount(string tableName, string where);
        int GetCount(string sql);
        int CreatTable(string sql);
        bool TableExists(string tableName);
        bool SqlBulkCopyByDatatable(string TableName, DataTable dt);

      
    }
}
