using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Dao.DataBase
{
    public interface ICommonOperation<T>
    {


        int Save(T t);

        void Delete(int id);

        bool IsExists(string strWhere);

        DataSet GetListTable(string strWhere);

        DataSet GetListView(string strWhere);

        int GetCount(string strWhere);

        List<T> GetModelList(string strWhere);

    }
}
