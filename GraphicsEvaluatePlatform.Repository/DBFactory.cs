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
    /// ���ݹ���
    /// </summary>
    public class DBFactory
    {

        /// <summary>
        /// ��ȡ���ݿ�����
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
                //SqlServer���ݿ�
                db = new GraphicsEvaluatePlatform.Repository.SqlDataBase();
            else if (DBWay == "2")
                //Mysql���ݿ�
                db = new GraphicsEvaluatePlatform.Repository.MySqlDataBase();
            //else if (DBWay == "3")
            //    db = new GraphicsEvaluatePlatform.Repository.OracleDataBase();
            return db;
        }

        /// <summary>
        /// ��ȡSQL���ݿ�����
        /// </summary>
        /// <returns></returns>
        public static  GraphicsEvaluatePlatform.Repository.IDataBase GetSqlDB()
        {
             GraphicsEvaluatePlatform.Repository.IDataBase db = null;

            db = new  GraphicsEvaluatePlatform.Repository.SqlDataBase();


            return db;
        }

        /// <summary>
        /// ��ȡSQL���ݿ�����
        /// </summary>
        /// <returns></returns>
        public static  GraphicsEvaluatePlatform.Repository.IDataBase GetSqlDB(string constr)
        {
             GraphicsEvaluatePlatform.Repository.IDataBase db = null;

            //db = new  GraphicsEvaluatePlatform.Repository.SqlDataBase(constr);

            //if (!File.Exists(Common.Config.AppConfigPath))
            //    return new  GraphicsEvaluatePlatform.Repository.AccessDataBase(constr);

            //if (Common.Config.DBWay == Common.Config.DataBaseType.Access.GetHashCode().ToString())
            //    //access���ݿ�
            //    db = new  GraphicsEvaluatePlatform.Repository.AccessDataBase(constr);
            //else if (Common.Config.DBWay == Common.Config.DataBaseType.Sql.GetHashCode().ToString())
            //    //sql���ݿ�
            //    db = new  GraphicsEvaluatePlatform.Repository.SqlDataBase(constr);
            //else
            //    db = new  GraphicsEvaluatePlatform.Repository.AccessDataBase(constr);

            return db;
        }

        /// <summary>
        /// ��ȡAccess���ݿ�����
        /// </summary>
        /// <returns></returns>
        public static  GraphicsEvaluatePlatform.Repository.IDataBase GetAccessDB()
        {
             GraphicsEvaluatePlatform.Repository.IDataBase db = null;

            //db = new  GraphicsEvaluatePlatform.Repository.AccessDataBase();

            return db;
        }

        /// <summary>
        /// ��ȡAccess���ݿ�����
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
