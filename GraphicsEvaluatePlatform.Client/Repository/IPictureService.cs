using GraphicsEvaluatePlatform.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEvaluatePlatform.Client.Repository
{
    interface IPictureService
    {
        /// <summary>
        /// 分页获取图像列表数据
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="currentlyPage"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        OperationResult GetList(int pageNum, int currentlyPage, Dictionary<string, object> dic);
    }
}
