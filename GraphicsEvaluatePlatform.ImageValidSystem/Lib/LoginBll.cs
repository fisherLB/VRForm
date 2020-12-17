/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.ImageValidSystem.Mdel;
using GraphicsEvaluatePlatform.ImageValidSystem.Model;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Encrypt;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Transactions;
using System.Windows.Forms;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.ImageValidSystem.Lib
 * 项目描述: 
 * 类 名 称: Login
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.ImageValidSystem.Lib
 * 文件名称: Login
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/8 11:34:47
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.ImageValidSystem.Lib
{
     public class LoginBll
    {

        private static Dictionary<string, LoginUser> loginusers = new Dictionary<string, LoginUser>();
        public static OperationResult LoginValidate(string LoginName, string Password)
        {
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
                        return new OperationResult(OperationResultType.QueryNull, "无此帐号记录");
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
                        return new OperationResult(OperationResultType.Error, "密码错误！");
                    }

                    //存放用户信息
                    BaseClass.USERID = retData.Tables[0].Rows[0]["Us_id"].ToString();  //登录用户ID
                    BaseClass.USERACCOUNTNAME = LoginName;  //存起登录用户名
                    BaseClass.USERNAME = retData.Tables[0].Rows[0]["Us_name"].ToString();
                    BaseClass.USERINFO = retData;  //存起登录用户信息
                    BaseClass.IPADDRESS = BaseClass.GetIPAddress(); //获取本机ip
                    BaseClass.UNITID = retData.Tables[0].Rows[0]["UnitID"].ToString();
                    BaseClass.UNITFULLNAME= retData.Tables[0].Rows[0]["Unit_name"].ToString();
                    ret.Message = "登录成功";
                    UserlogBll.InsertLog(dr["Us_account"].ToString().Trim(), dr["Us_name"].ToString().Trim(), BaseClass.IPADDRESS, DateTime.Now, "系统登录", "登录成功 ", "");
                    OperationLogBll.InsertLog(dr["Us_id"].ToString().Trim(), BaseClass.IPADDRESS, DateTime.Now, "登录成功", "");
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

        /// <summary>
        /// 加载登录用户
        /// </summary>
        /// <param name="loginnameCmb"></param>
        public static void LoadLoginUser(ComboBox loginnameCmb)
        {
            //读取文件流对象
            FileStream fs = new FileStream("LoginUser.bin", FileMode.OpenOrCreate);
            if (fs.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                //读出存在Data.bin 里的用户信息
                loginusers = bf.Deserialize(fs) as Dictionary<string, LoginUser>;
                //循环添加到Combox1
                foreach (LoginUser user in loginusers.Values)
                {
                    loginnameCmb.Items.Add(user.UserName);
                }

                //combox1 用户名默认选中第一个
                if (loginnameCmb.Items.Count > 0)
                    loginnameCmb.SelectedIndex = loginnameCmb.Items.Count - 1;
            }
            fs.Close();
        }

        public static void RememberLgoinUser(string userName, string passWord,CheckBox ckbPwd)
        {
            LoginUser user = new LoginUser();
            //登录成功时 如果没有LoginUser.bin文件就创建、有就打开
            FileStream fs = new FileStream("LoginUser.bin",FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            user.UserName = userName;

            if (ckbPwd.Checked)
                user.PassWord = passWord;
            else
                user.PassWord = "";
            //在集合中是否存在用户
            //if (loginusers.ContainsKey(user.UserName))
            //{
            //    //如果有清除
            //    loginusers.Remove(user.UserName);
            //}
            loginusers = new Dictionary<string, LoginUser>();
            //loginusers.RemoveAll();
            //添加用户信息到集合
            loginusers.Add(user.UserName, user);
            //写入文件
            bf.Serialize(fs, loginusers);
            //关闭
            fs.Close();
        }


        //显示用户所对应匹配的信息
        public static void DisplayUserInfo(ComboBox loginnameCmb,TextBox txtPwd,CheckBox ckbPwd)
        {

            string key = loginnameCmb.Text.Trim();
            //查找用户Id
            if (loginusers.ContainsKey(key) == false)
            {
                txtPwd.Text = "";
                return;
            }
            //查找到赋值
            LoginUser user = loginusers[key];
            txtPwd.Text = user.PassWord;
            // 如有有密码 选中复选框
            ckbPwd.Checked = txtPwd.Text.Trim().Length > 0 ? true : false;
        }
    }
}
