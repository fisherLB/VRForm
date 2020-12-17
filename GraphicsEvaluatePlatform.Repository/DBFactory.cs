//#define UNITTEST
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Configuration;

namespace  GraphicsEvaluatePlatform.Repository
{
    /// <summary>
    /// 数据工厂
    /// </summary>
    public class DBFactory
    {

        /// <summary>
        /// 获取数据库引擎
        /// </summary>
        /// <returns></returns>
        public static  GraphicsEvaluatePlatform.Repository.IDataBase GetDB()
        {
             GraphicsEvaluatePlatform.Repository.IDataBase db = null;
#if UNITTEST
            var DBWay="2";
#else
            var DBWay = ConfigurationManager.AppSettings["DBWay"];
#endif

            if (DBWay=="1")
                //SqlServer数据库
                db = new GraphicsEvaluatePlatform.Repository.SqlDataBase();
            else if (DBWay == "2")
                //Mysql数据库
                db = new GraphicsEvaluatePlatform.Repository.MySqlDataBase();
            //else if (DBWay == "3")
            //    db = new GraphicsEvaluatePlatform.Repository.OracleDataBase();
            return db;
        }

        /// <summary>
        /// 获取SQL数据库引擎
        /// </summary>
        /// <returns></returns>
        public static  GraphicsEvaluatePlatform.Repository.IDataBase GetSqlDB()
        {
             GraphicsEvaluatePlatform.Repository.IDataBase db = null;

            db = new  GraphicsEvaluatePlatform.Repository.SqlDataBase();


            return db;
        }

        /// <summary>
        /// 获取SQL数据库引擎
        /// </summary>
        /// <returns></returns>
        public static  GraphicsEvaluatePlatform.Repository.IDataBase GetSqlDB(string constr)
        {
             GraphicsEvaluatePlatform.Repository.IDataBase db = null;

            //db = new  GraphicsEvaluatePlatform.Repository.SqlDataBase(constr);

            //if (!File.Exists(Common.Config.AppConfigPath))
            //    return new  GraphicsEvaluatePlatform.Repository.AccessDataBase(constr);

            //if (Common.Config.DBWay == Common.Config.DataBaseType.Access.GetHashCode().ToString())
            //    //access数据库
            //    db = new  GraphicsEvaluatePlatform.Repository.AccessDataBase(constr);
            //else if (Common.Config.DBWay == Common.Config.DataBaseType.Sql.GetHashCode().ToString())
            //    //sql数据库
            //    db = new  GraphicsEvaluatePlatform.Repository.SqlDataBase(constr);
            //else
            //    db = new  GraphicsEvaluatePlatform.Repository.AccessDataBase(constr);

            return db;
        }

        /// <summary>
        /// 获取Access数据库引擎
        /// </summary>
        /// <returns></returns>
        public static  GraphicsEvaluatePlatform.Repository.IDataBase GetAccessDB()
        {
             GraphicsEvaluatePlatform.Repository.IDataBase db = null;

            //db = new  GraphicsEvaluatePlatform.Repository.AccessDataBase();

            return db;
        }

        /// <summary>
        /// 获取Access数据库引擎
        /// </summary>
        /// <returns></returns>
        public static  GraphicsEvaluatePlatform.Repository.IDataBase GetAccessDB(string path)
        {
             GraphicsEvaluatePlatform.Repository.IDataBase db = null;

            //db = new  GraphicsEvaluatePlatform.Repository.AccessDataBase(path);

            return db;
        }

    }
}
