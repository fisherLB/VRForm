/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEvaluatePlatform.Client.Models;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Infrastructure;
using System.Transactions;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Repository;
using GraphicsEvaluatePlatform.Infrastructure.Encrypt;
using GraphicsEvaluatePlatform.Client.Repository;
using GraphicsEvaluatePlatform.Client.Basics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;
using GraphicsEvaluatePlatform.Client.Common;
using System.Data;
/***********************************************************************
* 项目名称: GraphicsEvaluatePlatform.Client.Services
* 项目描述: 
* 类 名 称: LoginService
* 说    明: 
* 版 本 号: v1.0.0
* 作    者: admin
* 命名空间: GraphicsEvaluatePlatform.Client.Services
* 文件名称: LoginService
* CLR 版本: 4.0.30319.42000
* 创建时间: 2018/5/18 10:49:48
***********************************************************************/
namespace GraphicsEvaluatePlatform.Client.Services
{
    class LoginService : ILoginService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model">LoginModel</param>
        /// <returns></returns>
        public OperationResult Login(LoginModel model)
        {
            string LoginName = model.UserName;
            string Password = model.Password;
            bool isChecked = model.IsRemember;
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(LoginName, "LoginName");
                PublicHelper.CheckArgument(Password, "Password");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("AdminService").Error("UserLogin 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }

            //声明返回值
            var ret = new OperationResult(OperationResultType.Success);

            try
            {
                using (var tran = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    //声明查询条件
                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                    //声明更新目标
                    Dictionary<string, object> dicUpdateParam = new Dictionary<string, object>();

                    //根据传入的帐号判断系统是否存在该帐号
                    dicCondition.Add("Us_account", LoginName);
                    var retData = DataBll.GetModel("", "t_Users", dicCondition);
                    if (retData.Tables[0].Rows.Count != 1)
                    {
                        return new OperationResult(OperationResultType.QueryNull, "登录失败，无此帐号记录");
                    }

                    //将该帐号存入临时变量等待使用
                    var dr = retData.Tables[0].Rows[0];

                    //为临时变量设置密码
                    var pwd = EncryptPasswordFactory.GetEncipher(EncryptType.MD5).EncryptFor(Password);
                    //为更新登录情况信息设置查询条件
                    dicCondition.Add("Us_Password", dr["Us_Password"]);
                    //检查密码是否正确
                    if (dr["Us_Password"].ToString().Trim() != pwd)
                    {
                        return new OperationResult(OperationResultType.Error, "登录失败，密码错误！");
                    }

                    //存放用户信息
                    BaseService.USERID = retData.Tables[0].Rows[0]["Us_id"].ToString();  //登录用户ID
                    BaseService.USERACCOUNTNAME = LoginName;  //存起登录用户名
                    BaseService.USERNAME = retData.Tables[0].Rows[0]["Us_name"].ToString();
                    BaseService.USERINFO = retData;  //存起登录用户信息
                    BaseService.IPADDRESS = BaseService.GetIPAddress(); //获取本机ip
                    BaseService.UNITID = retData.Tables[0].Rows[0]["UnitID"].ToString();//存起机构ID
                    BaseService.UNITFULLNAME = retData.Tables[0].Rows[0]["Unit_name"].ToString();//存起机构名称
                    ret.Message = "登录成功";
                    RememberLgoinUser(LoginName, Password, isChecked);
                    UserlogService.InsertLog(dr["Us_account"].ToString().Trim(), dr["Us_name"].ToString().Trim(), BaseService.IPADDRESS, DateTime.Now, "系统登录", "登录成功 ", "");
                    OperationLogService.InsertLog(dr["Us_id"].ToString().Trim(), BaseService.IPADDRESS, DateTime.Now, "登录成功", "");
                    tran.Complete();
                }

                System.Threading.Thread.Sleep(new Random().Next(500, 3000));
            }
            catch (Exception ex)
            {
                ret.ResultType = OperationResultType.Error;
                ret.Message = ex.Message;
                Logger.GetLogger("LoginBll").Error("LoginValidate 发生异常", ex);

            }
            return ret;
        }
     
        private static void RememberLgoinUser(string userName, string passWord, bool isChecked)
        {
            LoginModel user = new LoginModel();
            //登录成功时 如果没有LoginUser.bin文件就创建、有就打开
            FileStream fs = new FileStream("LoginUser.bin", FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            user.UserName = userName;
            if (isChecked)
            {
                user.Password = passWord;
                user.IsRemember = isChecked;
            }
            else
            {
                user.Password = "";
                user.IsRemember = isChecked;
            }
               
            Dictionary<string, LoginModel> loginusers = new Dictionary<string, LoginModel>();
            //添加用户信息到集合
            loginusers.Add(user.UserName, user);
            //写入文件
            bf.Serialize(fs, loginusers);
            //关闭
            fs.Close();
        }

        public LoginModel LoadLoginUser()
        {
            LoginModel model = new LoginModel();
            //读取文件流对象
            FileStream fs = new FileStream("LoginUser.bin", FileMode.OpenOrCreate);
            if (fs.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                //读出存在Data.bin 里的用户信息
                Dictionary<string, LoginModel> loginusers  = bf.Deserialize(fs) as Dictionary<string, LoginModel>;
                //循环添加到Combox1
                foreach (LoginModel user in loginusers.Values)
                {
                    model.UserName=user.UserName;
                    model.Password = user.Password;
                    model.IsRemember = user.IsRemember;
                }
            }
            fs.Close();

            return model;
        }

      /// <summary>
      /// 检查是否更新版本
      /// </summary>
      /// <returns></returns>
        public OperationResult CheckVersion()
        {

            var ret = new OperationResult(OperationResultType.Success);
            var count = DataBll.GetCount("t_Version", "");

            if (count == 0)
            {
                ret.AppendData = -1;//没有数据，要进行初始化
            }
            else
            {
                var getVersionDataMethod = ConfigurationManager.AppSettings["GetVersionData"]; //执行的方法名
                var VersionData = GraphicsEvaluatePlatform.Client.Common.SynchronousData.SendClient(getVersionDataMethod); //返回的数据
                if (VersionData == "操作错误") {
                    return new OperationResult(OperationResultType.Success, "", 0);
                }
                var ConvertData = JsonUtil.ConvertToObject<ClientDataModel>(VersionData);
                DataTable ds = ConvertData.Data;
                string serverTime = "";
                if (ds.Rows.Count > 0)
                {
                    serverTime = ds.Rows[0]["v_CreationTime"].ToString().Trim();
                }
                DataTable dsc = DataBll.Query("select v_CreationTime from t_Version order by v_CreationTime desc").Tables[0];
                string clientTime = "";
                if (dsc.Rows.Count > 0)
                {
                    clientTime = dsc.Rows[0][0].ToString().Trim();
                }
                DateTime dtServer = Convert.ToDateTime(serverTime);
                DateTime dtClient = Convert.ToDateTime(clientTime);

                //dtClient晚于dtServer（即表示已经更新）
                if (DateTime.Compare(dtClient, dtServer) > 0)
                {
                    //不需要更新版本
                    ret.AppendData = 0;
                }
                else
                {
                    //需要更新版本
                    ret.AppendData = 1;
                }
            }
            return ret;
        }

        //同步数据
        public OperationResult SynchronousData()
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);

            var getUserDataMethod = ConfigurationManager.AppSettings["GetUsersData"];
            var UserData = GraphicsEvaluatePlatform.Client.Common.SynchronousData.SendClient(getUserDataMethod);
            var ConvertData = JsonUtil.ConvertToObject<ClientDataModel>(UserData);
            GraphicsEvaluatePlatform.Client.Common.SynchronousData.SynchronousDatas(ConvertData.Data, "t_users", true);
            if (UserData == "操作错误") {
                 ret.AppendData = "同步用户数据失败";
                return ret;
            }
            
            var getUnitDataMethod = ConfigurationManager.AppSettings["GetUnitData"];
            var UnitData = GraphicsEvaluatePlatform.Client.Common.SynchronousData.SendClient(getUnitDataMethod);
            var ConvertUnitData = JsonUtil.ConvertToObject<ClientDataModel>(UnitData);
            GraphicsEvaluatePlatform.Client.Common.SynchronousData.SynchronousDatas(ConvertUnitData.Data, "t_Units", true);
            if (UnitData == "操作错误") {
                ret.AppendData = "同步单位数据失败";
                return ret;
            }

            var getProjectDataMethod = ConfigurationManager.AppSettings["GetObjectData"];
            var ProjectData = GraphicsEvaluatePlatform.Client.Common.SynchronousData.SendClient(getProjectDataMethod);
            var ConvertProjectData = JsonUtil.ConvertToObject<ClientDataModel>(ProjectData);
            GraphicsEvaluatePlatform.Client.Common.SynchronousData.SynchronousDatas(ConvertProjectData.Data, "t_Projects", true);
            if (ProjectData == "操作错误") {
                ret.AppendData = "同步项目数据失败";
                return ret;
            }

            var GetDetectionSettingsDataMethod = ConfigurationManager.AppSettings["GetDetectionSettingsData"];
            var DetectionData = GraphicsEvaluatePlatform.Client.Common.SynchronousData.SendClient(GetDetectionSettingsDataMethod);
            var ConvertDeteData = JsonUtil.ConvertToObject<ClientDataModel>(DetectionData);
            GraphicsEvaluatePlatform.Client.Common.SynchronousData.SynchronousDatas(ConvertDeteData.Data, "t_DetectionSettings", true);
            if (DetectionData == "操作错误") {
                ret.AppendData = "同步检测设置数据失败";
                return ret;
            }
            ret.AppendData = "同步数据成功！";
            return ret;
        }


    }
}
