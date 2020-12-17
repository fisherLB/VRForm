using GraphicsEvaluatePlatform.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEvaluatePlatform.Client.Repository
{
    interface IProjectService
    {
        OperationResult Save(Models.Project model);
        OperationResult Update(Models.Project model);
        OperationResult Del(string id);
        OperationResult Search(Models.Project model);
        OperationResult GetDetail(string id);
        string ValueOf(object obj);
        Dictionary<string, object> EntityToDictionary<T>(T obj);
         List<Models.Project> GetList();
        OperationResult GetList(int pageNum, int currentlyPage,Dictionary<string,object>dic);
    }
}
