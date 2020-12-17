using GraphicsEvaluatePlatform.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEvaluatePlatform.Client.Repository
{
    interface IDetectionService
    {        
        OperationResult Add(Models.Detection model);
        OperationResult Update(Models.Detection model);
        OperationResult Del(string id);
        OperationResult Search(Models.Detection model);
        OperationResult GetDetail(string id);
        string ValueOf(object obj);
        Dictionary<string, object> EntityToDictionary<T>(T obj);
         List<Models.Detection> GetList();
        OperationResult GetList(int pageNum, int currentlyPage,Dictionary<string,object>dic);
        OperationResult DetectionProject(Guid projectId);
        OperationResult DetectionImage(Guid projectId, string pictureIds);
        IDictionary<string, object> GetDictionary(object source);
        OperationResult GetCount(Guid projectId);
    }
}
