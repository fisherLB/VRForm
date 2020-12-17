namespace GraphicsEvaluatePlatform.Infrastructure.Common.TelMessage
{
    public class ResponseModel
    {
        string result = "";
        string _msg = "";
        string _errorcode = "";
        string _body = "";
        string _exceptionmsg = "";

        public string Result
        {
            get { return result; }
            set { result = value; }
        }
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }
        /// <summary>
        /// 错误编码
        /// </summary>
        public string ErrorCode
        {
            get { return _errorcode; }
            set { _errorcode = value; }
        }



        /// <summary>
        /// 内容主体
        /// </summary>
        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionMsg
        {
            get { return _exceptionmsg; }
            set { _exceptionmsg = value; }
        }
    }
}