/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2017. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Model
 * 项目描述: 
 * 类 名 称: MessageModel
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: WorkServer
 * 命名空间: GraphicsEvaluatePlatform.Model
 * 文件名称: MessageModel
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/4/21 10:32:07
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Model
{
    public class MessageModel
    {
        public string Message { get; set; }

        public bool Success { get; set; }

        public object Data { get; set; }

        public MessageModel(string Message, OperationResult result, object Data)
        {
            this.Message = Message;
            this.Success = result.ResultType == OperationResultType.Success ? true : false;
            this.Data = Data;
        }

        public MessageModel(OperationResult result)
        {
            this.Message = result.Message;
            this.Success = result.ResultType == OperationResultType.Success ? true : false;
            this.Data = result.AppendData;
        }
        public MessageModel(string Message, bool Success, object Data)
        {
            this.Message = Message;
            this.Success = Success;
            this.Data = Data;
        }
    }
}
