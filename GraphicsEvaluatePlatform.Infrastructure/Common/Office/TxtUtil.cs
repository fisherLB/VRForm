using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
namespace GraphicsEvaluatePlatform.Infrastructure.Common.Office
{
    public class TxtUtil
    {
        public TxtUtil(string filename)
        {
            _fileFullName = filename;
        }

        public TxtUtil()
        {

        }

        //文件路径全名
        private string _fileFullName = "";

        /// <summary>
        /// 读取文本数据
        /// </summary>
        /// <returns></returns>
        public string ReadFile()
        {
            StringBuilder str = new StringBuilder();
            if (_fileFullName == "")
                return "";
            try
            {
                str.Append(File.ReadAllText(_fileFullName, System.Text.Encoding.Default));

            }
            catch
            { return ""; }

            return str.ToString();

        }
        public string ReadFileForWeb()
        {
            if (_fileFullName == "")
                return "";

            string line = String.Empty;
            try
            {

                using (StreamReader reader = new StreamReader(_fileFullName, System.Text.Encoding.Default))
                {
                    string l = String.Empty;
                    while ((l = reader.ReadLine()) != null)
                    {
                        line += l + "<BR>";
                    }
                }

            }
            catch
            { return ""; }
            return line;
        }


        /// <summary>
        /// 获取文本中数据集
        /// </summary>
        /// <param name="splitChar">分隔符</param>
        /// <returns></returns>
        public DataSet GetData(string splitChar)
        {
            StringBuilder str = new StringBuilder();
            str.Append(ReadFile());
            if (splitChar == "")
                return null;

            List<string> listStr = Common.Strings.StringUtil.ArrStrToList(str.ToString(), "\r\n");

            if (listStr.Count == 0)
                return null;

            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            List<string> colStr = Common.Strings.StringUtil.ArrStrToList(listStr[0], splitChar);
            GetDataColumns(ds, colStr);

            for (int i = 1; i < listStr.Count; i++)
            {
                string row = listStr[i].Trim();
                if (row == "")
                    continue;
                List<string> cellStr = Common.Strings.StringUtil.ArrStrToList(row, splitChar);
                DataRow dr = ds.Tables[0].NewRow();
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if (j >= cellStr.Count)
                        break;
                    DataColumn dc = ds.Tables[0].Columns[j];
                    dr[dc] = cellStr[j];
                }
                ds.Tables[0].Rows.Add(dr);

            }

            return ds;
        }

        public DataSet GetDataFromDARMS2000(string splitChar)
        {
            StringBuilder str = new StringBuilder();
            str.Append(ReadFile());
            if (splitChar == "")
                return null;

            List<string> listStr = Common.Strings.StringUtil.ArrStrToList(str.ToString(), "\r\n");

            if (listStr.Count == 0)
                return null;

            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            List<string> colStr = Common.Strings.StringUtil.ArrStrToList(listStr[0], splitChar);
            GetDataColumns1(ds, colStr);

            for (int i = 1; i < listStr.Count; i++)
            {
                string row = listStr[i];
                if (row == "")
                    continue;
                List<string> cellStr = Common.Strings.StringUtil.ArrStrToList(row, splitChar);
                DataRow dr = ds.Tables[0].NewRow();
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if (j >= cellStr.Count)
                        break;
                    DataColumn dc = ds.Tables[0].Columns[j];
                    dr[dc] = cellStr[j];
                }
                ds.Tables[0].Rows.Add(dr);

            }

            return ds;
        }

        /// <summary>
        /// 导出到文本
        /// </summary>
        /// <param name="splitChar"></param>
        public string WriteToFileEncoding(string path, DataSet ds, string cols, string splitChar, string rowChar, string filename, bool showtitle, Encoding encd, params string[] filter)
        {
            StringBuilder str = new StringBuilder();
            List<string> listCols = Common.Strings.StringUtil.ArrStrToList(cols, ",");


            if (showtitle)
            {//输出列名
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    string col = dc.ColumnName;

                    if (Isfilter(filter, col))
                        //字段过滤
                        continue;

                    if (IsInit(listCols, col) || cols == "*" || cols == "")
                    {
                        if (str.ToString() == "")
                            str.Append(col);
                        else
                            str.Append(splitChar + col);
                    }
                }
                str.Append(rowChar);
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int row = 0;
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    string col = dc.ColumnName;
                    string val = dr[dc].ToString();

                    if (Isfilter(filter, col))
                        //字段过滤
                        continue;

                    if (IsInit(listCols, col) || cols == "*" || cols == "")
                    {
                        if (row == 0)
                            str.Append(val);
                        else
                            str.Append(splitChar + val);
                    }
                    row++;
                }
                str.Append(rowChar);
            }

            string fullname = Common.Strings.StringUtil.GetFileName(path + "\\" + filename);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(fullname, str.ToString(), encd);
            return fullname;
        }


        public DataSet GetData(string splitChar, string tabSplit)
        {
            StringBuilder str = new StringBuilder();
            str.Append(ReadFile());
            if (splitChar == "")
                return null;

            List<string> listStr = Common.Strings.StringUtil.ArrStrToList(str.ToString(), tabSplit);

            if (listStr.Count == 0)
                return null;

            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            List<string> colStr = Common.Strings.StringUtil.ArrStrToList(listStr[0], splitChar);
            GetDataColumns(ds, colStr);

            for (int i = 1; i < listStr.Count; i++)
            {
                string row = listStr[i].Trim();
                if (row == "")
                    continue;
                List<string> cellStr = Common.Strings.StringUtil.ArrStrToList(row, splitChar);
                DataRow dr = ds.Tables[0].NewRow();
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if (j >= cellStr.Count)
                        break;
                    DataColumn dc = ds.Tables[0].Columns[j];
                    dr[dc] = cellStr[j];
                }
                ds.Tables[0].Rows.Add(dr);

            }

            return ds;
        }

        private void GetDataColumns1(DataSet ds, List<string> colStr)
        {
            for (int i = 0; i < colStr.Count; i++)
            {
                string col = colStr[i];
                DataColumn dc = new DataColumn();
                dc.ColumnName = "col" + Convert.ToSingle((i + 1));
                ds.Tables[0].Columns.Add(dc);
            }
        }

        private void GetDataColumns(DataSet ds, List<string> colStr)
        {
            for (int i = 0; i < colStr.Count; i++)
            {
                string col = colStr[i];
                DataColumn dc = new DataColumn();
                dc.ColumnName = col;
                ds.Tables[0].Columns.Add(dc);
            }
        }

        /// <summary>
        /// 导出到文本
        /// </summary>
        /// <param name="splitChar"></param>
        public string WriteToFile(string path, DataSet ds, string cols, string splitChar, string rowChar, string filename, bool showtitle, params string[] filter)
        {
            StringBuilder str = new StringBuilder();
            List<string> listCols = Common.Strings.StringUtil.ArrStrToList(cols, ",");


            if (showtitle)
            {//输出列名
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    string col = dc.ColumnName;

                    if (Isfilter(filter, col))
                        //字段过滤
                        continue;

                    if (IsInit(listCols, col) || cols == "*" || cols == "")
                    {
                        if (str.ToString() == "")
                            str.Append(col);
                        else
                            str.Append(splitChar + col);
                    }
                }
                str.Append(rowChar);
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int row = 0;
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    string col = dc.ColumnName;
                    string val = dr[dc].ToString();

                    if (Isfilter(filter, col))
                        //字段过滤
                        continue;

                    if (IsInit(listCols, col) || cols == "*" || cols == "")
                    {
                        if (row == 0)
                            str.Append(val);
                        else
                            str.Append(splitChar + val);
                    }
                    row++;
                }
                str.Append(rowChar);
            }

            string fullname = Common.Strings.StringUtil.GetFileName(path + "\\" + filename);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(fullname, str.ToString(), System.Text.UnicodeEncoding.UTF8);
            return fullname;
        }

        public void WriteToFile(string path, StringBuilder sb)
        {
            string filename = "new1";
            string fullname = Common.Strings.StringUtil.GetFileName(path + "\\" + filename);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(fullname, sb.ToString(), System.Text.UnicodeEncoding.UTF8);
        }


        public void WriteToFileName(string fullname, StringBuilder sb)
        {


            File.WriteAllText(fullname, sb.ToString(), System.Text.UnicodeEncoding.UTF8);
        }

        //是否过滤
        private bool Isfilter(string[] filter, string col)
        {
            if (filter != null)
            {//过滤字段
                foreach (string s in filter)
                {
                    if (col.ToLower() == s.ToLower())
                        return true;
                }
            }

            return false;
        }

        private bool IsInit(List<string> list, string col)
        {
            foreach (string s in list)
            {
                if (s.Trim() == col.Trim())
                    return true;
            }

            return false;
        }
        public static byte[] GetByteForDataSet(DataSet ds)
        {

            DataTable dt = ds.Tables[0];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sb.Append(dt.Rows[i][j].ToString() + "\t\t");
                }
                sb.AppendLine();
            }

            byte[] b = Encoding.UTF8.GetBytes(sb.ToString());
            return b;
        }


    }
}
