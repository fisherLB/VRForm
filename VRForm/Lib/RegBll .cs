/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.ImageValidSystem.Mdel;
using GraphicsEvaluatePlatform.ImageValidSystem.Model;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Infrastructure.Encrypt;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Data;
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
namespace VRForm.Lib
{
     public class RegBll
    {

        private static Dictionary<string, LoginUser> loginusers = new Dictionary<string, LoginUser>();
        public static OperationResult RegUser(string UserName, string Password)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(UserName, "UserName");
                PublicHelper.CheckArgument(Password, "Password");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("RegBll").Error("UserLogin 发生异常", ex);
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
                    dicCondition.Add("Name", UserName);
                    var retData = DataBll.GetModel("", "VRUsers", dicCondition);
                    if (retData.Tables[0].Rows.Count>0)
                    {
                        return new OperationResult(OperationResultType.QueryNull, $"抱歉注册账号”"+UserName+"“已存在，请使用别的用户名");
                    }

                   
                            //判断用户名是否重复(同一机构下)
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("Name", UserName);
                            //dic.Add("Password", Password);
                            DataTable ds = DataTrim.DataTableTrim(DataBll.GetModel("", "VRUsers", dic).Tables[0]);
                            if (ds.Rows.Count >= 1)
                            {
                                return new OperationResult(OperationResultType.Error, "用户名已存在", new { Success = false, Message = "用户名已存在" });
                            }



                    dic.Add("Password",EncryptPasswordFactory.GetEncipher(EncryptType.MD5).EncryptFor(Password.ToString().Trim()));
                    dic.Add("CreateTime", DateTime.Now);
                    dic.Add("VRTime", 5);

                            var dsret = DataBll.Add(dic, "VRUsers");


                            ret.Message = "新增用户成功,请到用户组管理为用户分配用户组以获取基础权限或直接到权限管理为用户单独分配权限";
                            ret.AppendData = new { Success = true, Message = ret.Message };
                            tran.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.GetLogger("UserService").Error("AddUser 发生异常", ex);
                        if (ex.Message.IndexOf("不能在具有唯一索引") != -1)
                            return new OperationResult(OperationResultType.ParamError, "用户名已存在", new { Success = false, Message = "用户名已存在" });
                        else
                            return new OperationResult(OperationResultType.ParamError, "新增用户失败", new { Success = false, Message = "新增用户失败" });
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
