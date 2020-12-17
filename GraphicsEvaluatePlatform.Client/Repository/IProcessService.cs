using GraphicsEvaluatePlatform.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GraphicsEvaluatePlatform.Client.Repository
{
    interface IProcessService
    {
        /// <summary>
        /// 图像处理元步骤 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        OperationResult Save(Models.Process model);
        OperationResult Update(Models.Process model);
        OperationResult GetDetail(Guid guid);
        /// <summary>
        /// 针对指定的图像进行处理
        /// </summary>
        /// <param name="pictureId">图像id，可以是多少图像id，每个图像id以 ',' 分割</param>
        /// <param name="isAuto">是否自动处理</param>
        /// <returns></returns>
        OperationResult ProcessPicture(Guid projectId, string pictureId, bool isAuto);
        /// <summary>
        /// 针对指定项目下的图像处理
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="isAuto">是否自动处理</param>
        /// <returns></returns>
        OperationResult ProcessProject(Guid projectId, string isAuto);

        OperationResult SetSize(Bitmap bitmap, int width, int height);
        OperationResult SetRotate(Bitmap bitmap, int val);
        OperationResult SetBright(Bitmap bitmap, int val);
        OperationResult SetConstrast(Bitmap bitmap, int  val);

        OperationResult SetWater(string imagePath,string content);
    }
}
