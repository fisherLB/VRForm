using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Infrastructure.Common.Systems
{ /// <summary>
  /// 注册表操作类
  /// </summary>
    public class RegeditOperation
    {

        public RegeditOperation()
        {

        }

        #region 公共方法

        /// <summary>
        /// 写入注册表,如果指定项已经存在,则修改指定项的值
        /// </summary>
        /// <param name="keytype">注册表基项枚举</param>
        /// <param name="key">注册表项,不包括基项</param>
        /// <param name="name">值名称</param>
        /// <param name="values">值</param>
        /// <returns>返回布尔值,指定操作是否成功</returns>
        public bool SetValue(RegeditOperation.keyType keytype, string key, string name, string values)
        {
            try
            {
                RegistryKey rk = (RegistryKey)getRegistryKey(keytype);

                RegistryKey rkt = rk.CreateSubKey(key);

                if (rkt != null)
                    rkt.SetValue(name, values);
                else
                {
                    throw (new Exception("要写入的项不存在"));
                }

                return true;
            }
            catch
            {
                return false;
            }

        }


        /// <summary>
        /// 读取注册表
        /// </summary>
        /// <param name="keytype">注册表基项枚举</param>
        /// <param name="key">注册表项,不包括基项</param>
        /// <param name="name">值名称</param>
        /// <returns>返回字符串</returns>
        public string GetValue(RegeditOperation.keyType keytype, string key, string name)
        {
            try
            {
                RegistryKey rk = (RegistryKey)getRegistryKey(keytype);

                RegistryKey rkt = rk.OpenSubKey(key);

                if (rkt != null)
                    return rkt.GetValue(name).ToString();
                else
                    throw (new Exception("无法找到指定项"));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string GetValueByPath(RegeditOperation.keyType keytype, string path, string name)
        {

            List<string> list = Common.Strings.StringUtil.ArrStrToList(path, "/");
            RegistryKey hkml = (RegistryKey)getRegistryKey(keytype);
            for (int i = 0; i < list.Count; i++)
            {
                string k = list[i];
                hkml = hkml.OpenSubKey(k, true);
            }


            string val = "";
            try
            {
                val = hkml.GetValue(name).ToString();
            }
            catch
            { }

            return val;

        }



        /// <summary>
        /// 删除注册表中的值
        /// </summary>
        /// <param name="keytype">注册表基项枚举</param>
        /// <param name="key">注册表项名称,不包括基项</param>
        /// <param name="name">值名称</param>
        /// <returns>返回布尔值,指定操作是否成功</returns>
        public bool DeleteValue(RegeditOperation.keyType keytype, string key, string name)
        {
            try
            {
                RegistryKey rk = (RegistryKey)getRegistryKey(keytype);

                RegistryKey rkt = rk.OpenSubKey(key, true);

                if (rkt != null)
                    rkt.DeleteValue(name, true);
                else
                    throw (new Exception("无法找到指定项"));

                return true;

            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 删除注册表中的指定项
        /// </summary>
        /// <param name="keytype">注册表基项枚举</param>
        /// <param name="key">注册表中的项,不包括基项</param>
        /// <returns>返回布尔值,指定操作是否成功</returns>
        public bool DeleteSubKey(RegeditOperation.keyType keytype, string key)
        {
            try
            {
                RegistryKey rk = (RegistryKey)getRegistryKey(keytype);

                rk.DeleteSubKeyTree(key);

                return true;

            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 判断指定项是否存在
        /// </summary>
        /// <param name="keytype">基项枚举</param>
        /// <param name="key">指定项字符串</param>
        /// <returns>返回布尔值,说明指定项是否存在</returns>
        public bool IsExist(RegeditOperation.keyType keytype, string key)
        {
            RegistryKey rk = (RegistryKey)getRegistryKey(keytype);

            if (rk.OpenSubKey(key) == null)
                return false;
            else
                return true;
        }


        /// <summary>
        /// 检索指定项关联的所有值
        /// </summary>
        /// <param name="keytype">基项枚举</param>
        /// <param name="key">指定项字符串</param>
        /// <returns>返回指定项关联的所有值的字符串数组</returns>
        public string[] GetValues(RegeditOperation.keyType keytype, string key)
        {
            RegistryKey rk = (RegistryKey)getRegistryKey(keytype);

            RegistryKey rkt = rk.OpenSubKey(key);

            if (rkt != null)
            {

                string[] names = rkt.GetValueNames();

                if (names.Length == 0)
                    return names;
                else
                {
                    string[] values = new string[names.Length];

                    int i = 0;

                    foreach (string name in names)
                    {
                        values[i] = rkt.GetValue(name).ToString();

                        i++;
                    }

                    return values;

                }
            }
            else
            {
                throw (new Exception("指定项不存在"));
            }

        }
        /// <summary>
        /// 写入注册表,如果指定项已经存在,则修改指定项的值
        /// </summary>
        /// <param name="keytype">注册表基项枚举</param>
        /// <param name="key">注册表项,不包括基项</param>
        /// <param name="name">值名称</param>
        /// <param name="values">值</param>
        /// <returns>返回布尔值,指定操作是否成功</returns>
        public bool SetValueReg(RegeditOperation.keyType keytype, string key, string name, string values)
        {
            try
            {
                List<string> list = Common.Strings.StringUtil.ArrStrToList(key, "/");
                RegistryKey hkml = (RegistryKey)getRegistryKey(keytype);
                for (int i = 0; i < list.Count; i++)
                {
                    try
                    {
                        string k = list[i];
                        hkml = hkml.OpenSubKey(k, true);
                    }

                    catch { }
                }

                hkml.SetValue(name, values);




                return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// 开机启动
        /// </summary>
        /// <param name="Started"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public static void RunWhenStart(bool Started, string name, string path)
        {
            RegistryKey Run = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (Run == null)
                Run = Run.CreateSubKey(@"SOFTWARE/Microsoft/Windows/CurrentVersion/Run");

            if (Started == true)
            {
                try
                {
                    Run.SetValue(name, path);
                    Run.Close();
                }
                catch//没有权限会异常            
                { }
            }
            else
            {
                try
                {
                    Run.DeleteValue(name);
                    Run.Close();
                }
                catch//没有权限会异常 
                { }
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 返回RegistryKey对象
        /// </summary>
        /// <param name="keytype">注册表基项枚举</param>
        /// <returns></returns>
        private object getRegistryKey(RegeditOperation.keyType keytype)
        {
            RegistryKey rk = null;

            switch (keytype)
            {
                case keyType.HKEY_CLASS_ROOT:
                    rk = Registry.ClassesRoot;
                    break;
                case keyType.HKEY_CURRENT_USER:
                    rk = Registry.CurrentUser;
                    break;
                case keyType.HKEY_LOCAL_MACHINE:
                    rk = Registry.LocalMachine;
                    break;
                case keyType.HKEY_USERS:
                    rk = Registry.Users;
                    break;
                case keyType.HKEY_CURRENT_CONFIG:
                    rk = Registry.CurrentConfig;
                    break;
            }

            return rk;
        }

        #endregion

        #region 枚举
        /// <summary>
        /// 注册表基项枚举
        /// </summary>
        public enum keyType : int
        {
            /// <summary>
            /// 注册表基项 HKEY_CLASSES_ROOT
            /// </summary>
            HKEY_CLASS_ROOT,
            /// <summary>
            /// 注册表基项 HKEY_CURRENT_USER
            /// </summary>
            HKEY_CURRENT_USER,
            /// <summary>
            /// 注册表基项 HKEY_LOCAL_MACHINE
            /// </summary>
            HKEY_LOCAL_MACHINE,
            /// <summary>
            /// 注册表基项 HKEY_USERS
            /// </summary>
            HKEY_USERS,
            /// <summary>
            /// 注册表基项 HKEY_CURRENT_CONFIG
            /// </summary>
            HKEY_CURRENT_CONFIG
        }
        #endregion


    }


}
