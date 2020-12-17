using GraphicsEvaluatePlatform . Infrastructure . Logging;
using System;
using System . Collections;
using System . Collections . Generic;
using System . Data;
using System . Diagnostics;
using System . IO;
using System . IO . MemoryMappedFiles;
using System . Linq;
using System . Text;
using System . Threading;
using System . Threading . Tasks;

namespace GraphicsEvaluatePlatform.Infrastructure.Common.Files
{
    public class CSVUtil
    {
        private ArrayList al;
        public CSVUtil()
        {
            al = new ArrayList();
        }
        /// <summary>
        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName">CSV文件路径</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static OperationResult OpenCSV(string filePath,bool isOnlyHead=false)
        {
            OperationResult ret = new OperationResult(OperationResultType . Success);
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(filePath , FileMode . Open , FileAccess . Read))
            {
                using (StreamReader sr = new StreamReader(fs , Encoding . Default))
                {
                    try
                    {
                        //StreamReader sr = new StreamReader(fs, encoding);
                        //string fileContent = sr.ReadToEnd();
                        //记录每次读取的一行记录
                        string strLine = "";
                        //记录每行记录中的各字段内容
                        string [ ] aryLine = null;
                        string [ ] tableHead = null;
                        //标示列数
                        int columnCount = 0;
                        //标示是否是读取的第一行
                        bool IsFirst = true;
                        //逐行读取CSV中的数据
                        while ((strLine = sr . ReadLine()) != null)
                        {                          
                            if (IsFirst == true)
                            {
                                if ( strLine . Substring ( 0 , 1 ) == "#" )
                                    continue;
                                tableHead = strLine . Split(',');
                                 IsFirst = false;
                                columnCount = tableHead . Length;
                                //创建列
                                for (int i = 0 ; i < columnCount ; i++)
                                {
                                    tableHead [ i ] = tableHead [ i ] . Replace("\"" , "");
                                    DataColumn dc = new DataColumn(tableHead [ i ]);
                                    if (dt.Columns.Contains(dc.ColumnName))
                                    {
                                        ret.ResultType = OperationResultType.Error;
                                        ret.Message += "列 " + dc.ColumnName + " 名称重复无法导入！";                                      
                                        continue;
                                    }
                                    dt . Columns . Add(dc);
                                }
                                if (ret.ResultType == OperationResultType.Error)
                                {
                                    return ret;
                                }                                     
                            }
                            else
                            {
                                if ( strLine . Substring ( 0 , 1 ) == "#" )
                                    break;
                                aryLine = strLine . Split ( ',' );
                                DataRow dr = dt . NewRow();
                                for (int j = 0 ; j < columnCount ; j++)
                                {
                                    dr [ j ] = aryLine [ j ] . Replace("\"" , "");
                                }
                                dt . Rows . Add(dr);                
                            }
                            if (isOnlyHead)
                            {
                                break;  
                            }
                        }
                        if (aryLine != null && aryLine . Length > 0)
                        {
                            dt . DefaultView . Sort = tableHead [ 2 ] + " " + "DESC";
                        }

                        sr . Close();
                        fs . Close();
                        ret . AppendData = dt;
                        return ret;
                    }
                    catch (Exception ex  )
                    {
                        Logger . GetLogger("CSVUtil") . Error("OpenCSV 发生异常" , ex);
                        return new OperationResult(OperationResultType .Error , ex.Message);
                    }
                }
                
            }
        }
        public static OperationResult OpenCSV ( string filePath , string type )
        {
            OperationResult ret = new OperationResult ( OperationResultType . Success );
            DataTable dt = new DataTable ( );
            using ( FileStream fs = new FileStream ( filePath , FileMode . Open , FileAccess . Read ) )
            {
                using ( StreamReader sr = new StreamReader ( fs , Encoding . Default ) )
                {
                    try
                    {
                        string strLine = "";
                        string [ ] aryLine = null;
                        string [ ] tableHead = null;
                        int columnCount = 0;
                        bool IsFirst = true;
                        bool begin = false;
                        while ( ( strLine = sr . ReadLine ( ) ) != null )
                        {
                            if ( IsFirst == true )// 需要表头
                            {
                                if ( !begin )// 未开始
                                {
                                    if ( strLine.Split(',')[0] == type )//如果遇到标签                                   
                                        begin = true;//设置开始读表头；
                                    continue;//但还是要从下一行开始
                                }
                                else
                                {
                                    tableHead = strLine . Split ( ',' );
                                    IsFirst = false;
                                    columnCount = tableHead . Length;
                                    for ( int i = 0 ; i < columnCount ; i++ )
                                    {
                                        tableHead [ i ] = tableHead [ i ] . Replace ( "\"" , "" );
                                        DataColumn dc = new DataColumn ( tableHead [ i ] );
                                        dt . Columns . Add ( dc );
                                    }
                                }
                            }
                            else//已经得到表头静养
                            {
                                if ( strLine . Substring ( 0 , 1 ) == "#" )
                                    break;
                                aryLine = strLine . Split ( ',' );
                                DataRow dr = dt . NewRow ( );
                                for ( int j = 0 ; j < columnCount ; j++ )
                                {
                                    dr [ j ] = aryLine [ j ] . Replace ( "\"" , "" );
                                }
                                dt . Rows . Add ( dr );
                            }
                        }//end while
                        if ( aryLine != null && aryLine . Length > 0 )
                        {
                            dt . DefaultView . Sort = tableHead [ 2 ] + " " + "DESC";
                        }
                        sr . Close ( );
                        fs . Close ( );
                        ret . AppendData = dt;
                        return ret;
                    }
                    catch ( Exception ex )
                    {
                        Logger . GetLogger ( "CSVUtil" ) . Error ( "OpenCSV 发生异常" , ex );
                        return new OperationResult ( OperationResultType . Error , ex . Message );
                    }
                }
            }
        }
        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fileName">CSV的文件路径</param>
        public static bool SaveCSV(DataTable dt, string fullPath)
        {
            try
            {
                FileInfo fi = new FileInfo(fullPath);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                string data = "";
                //写出列名称
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    data += "\"" + dt.Columns[i].ColumnName.ToString() + "\"";
                    if (i < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
                //写出各行数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    data = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string str = dt.Rows[i][j].ToString();
                        str = string.Format("\"{0}\"", str);
                        data += str;
                        if (j < dt.Columns.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                }
                sw.Close();
                fs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改文件名称 
        /// </summary>
        /// <param name="OldPath">旧的路径 完整的物理路径</param>
        /// <param name="NewPath">新的路径</param>
        /// <returns></returns>
        public static bool ChangeFileName(string OldPath, string NewPath)
        {
            bool re = false;
            //OldPath = HttpContext.Current.Server.MapPath(OldPath);虚拟的
            //NewPath = HttpContext.Current.Server.MapPath(NewPath);
            try
            {
                if (File.Exists(OldPath))
                {
                    File.Move(OldPath, NewPath);
                    re = true;
                }
            }
            catch
            {
                re = false;
            }
            return re;
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static bool SaveCSV(string fullPath, string Data)
        {
            FileStream fs = new FileStream(fullPath, FileMode.Append);

            bool re = true;
            try
            {
                FileStream FileStream = new FileStream(fullPath, FileMode.Append);
                StreamWriter sw = new StreamWriter(FileStream, System.Text.Encoding.UTF8);
                sw.WriteLine(Data);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                FileStream.Close();
            }
            catch
            {
                re = false;
            }
            return re;
        }
        //private void btnSaveCSV_Click(object sender, EventArgs e)
        //{
        //    saveFileDialog1.Filter = "CSV文件|*.CSV";
        //    saveFileDialog1.InitialDirectory = "C:\\";
        //    if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        string fileName = saveFileDialog1.FileName;
        //        SaveCSV(ds.Tables[0], fileName);
        //    }
        //}
        /////
        ///// 将DataTable中数据写入到CSV文件中
        /////
        ///// 提供保存数据的DataTable
        ///// CSV的文件路径
        //public void SaveCSV(DataTable dt, string fileName)
        //{
        //    FileStream fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
        //    StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
        //    string data = "";
        //    //写出列名称
        //    for (int i = 0; i < dt.Columns.Count; i++)
        //    {
        //        data += dt.Columns[i].ColumnName.ToString();
        //        if (i < dt.Columns.Count - 1)
        //        {
        //            data += ",";
        //        }
        //    }
        //    sw.WriteLine(data);
        //    //写出各行数据
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        data = "";
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            data += dt.Rows[i][j].ToString();
        //            if (j < dt.Columns.Count - 1)
        //            {
        //                data += ",";
        //            }
        //        }
        //        sw.WriteLine(data);
        //    }
        //    sw.Close();
        //    fs.Close();
        //   // MessageBox.Show("CSV文件保存成功！");
        //}


        /// <summary>
        /// 读取csv文件到DataTable
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        static private DataTable ReadCsv(string filepath)
        {
            DataTable dt = new DataTable("NewTable");
            DataRow row;

            string[] lines = File.ReadAllLines(filepath, Encoding.UTF8);
            string[] head = lines[0].Split(',');
            int cnt = head.Length;
            for (int i = 0; i < cnt; i++)
            {
                dt.Columns.Add(head[i]);
            }
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].Trim();
                if ((string.IsNullOrWhiteSpace(lines[i])))
                {
                    continue;
                }
                try
                {
                    row = dt.NewRow();
                    row.ItemArray = GetRow(lines[i], cnt);
                    dt.Rows.Add(row);
                }
                catch { }
            }
            return dt;
        }
        /// <summary>
        /// 解析字符串 获取 该行的数据 已经处理,及"号
        /// </summary>
        /// <param name="line">该行的内容</param>
        /// <param name="cnt">总的条目数</param>
        /// <returns></returns>
        static private string[] GetRow(string line, int cnt)
        {
            //line = line.Replace("\"\"", "\""); //若空数据加引号替换不正确
            string[] strs = line.Split(',');
            if (strs.Length == cnt)
            {
                return RemoveQuotes(strs);
            }
            List<string> list = new List<string>();
            int n = 0, begin = 0;
            bool flag = false;

            for (int i = 0; i < strs.Length; i++)
            {

                //没有引号 或者 中间有引号 直接添加
                if (strs[i].IndexOf("\"") == -1
                    || (flag == false && strs[i][0] != '\"'))
                {
                    list.Add(strs[i]);
                    continue;
                }
                //其实有引号，但该段没有,号，直接添加
                n = 0;
                foreach (char ch in strs[i])
                {
                    if (ch == '\"')
                    {
                        n++;
                    }
                }
                if (n % 2 == 0)
                {
                    list.Add(strs[i]);
                    continue;
                }
                //该段有引号 有 ,号，下一段增加后添加
                flag = true;
                begin = i;
                i++;
                for (i = begin + 1; i < strs.Length; i++)
                {
                    foreach (char ch in strs[i])
                    {
                        if (ch == '\"')
                        {
                            n++;
                        }
                    }
                    if (strs[i][strs[i].Length - 1] == '\"' && n % 2 == 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (; begin <= i; begin++)
                        {
                            sb.Append(strs[begin]);
                            if (begin != i)
                            {
                                sb.Append(",");
                            }
                        }
                        list.Add(sb.ToString());
                        break;
                    }
                }
            }
            return RemoveQuotes(list.ToArray());
        }
        /// <summary>
        /// 将解析的数据 去除多余的引号
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        static string[] RemoveQuotes(string[] strs)
        {
            for (int i = 0; i < strs.Length; i++)
            {
                //若该项数据为空 但csv文件中加上双引号
                if (strs[i] == "\"\"")
                {
                    strs[i] = "";
                    continue;
                }
                //若该项数据头和尾加上引号
                if (strs[i].Length > 2 && strs[i][0] == '\"' && strs[i][strs[i].Length - 1] == '\"')
                {
                    strs[i] = strs[i].Substring(1, strs[i].Length - 2);
                }
                //若该项数据中间有引号
                strs[i] = strs[i].Replace("\"\"", "\"");
            }
            return strs;
        }


        //      1.如果csv文件字段中有特殊字符，整个字段应该用双引号包起来

        //   特殊字符有三种， 逗号[,] 回车换行[\r\n]    和处于字段开头的双引号["]


        //例如：字段   a, b, c（b, c 文本中包含逗号）       , d


        //         就应该变成    a, "b,c", d


        //         有回车换行的也是一样


        //2.如果 csv字段中有特殊字符，并且字段中含有双引号，则字段中的双引号应该写两次


        // 例如：字段  a, b, c"aa      ,d


        //          就应该变成   a, "b,c""aa", d
        public static string[][] read_csv(string text)
        {
            if (text == null)
                return null;
            var text_array = new List<string[]>();
            var line = new List<string>();
            var field = new StringBuilder();
            //是否在双引号内
            bool in_quata = false;
            //字段是否开始
            bool field_start = true;
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (in_quata)
                {
                    //如果已经处于双引号范围内
                    if (ch == '\"')
                    {
                        //如果是两个引号，则当成一个普通的引号处理
                        if (i < text.Length - 1 && text[i + 1] == '\"')
                        {
                            field.Append('\"');
                            i++;
                        }
                        else
                            //否则退出引号范围
                            in_quata = false;
                    }
                    else //双引号范围内的任何字符（除了双引号）都当成普通字符
                    {
                        field.Append(ch);
                    }
                }
                else
                {
                    switch (ch)
                    {
                        case ',': //新的字段开始
                            line.Add(field.ToString());
                            field.Remove(0, field.Length);
                            field_start = true;
                            break;
                        case '\"'://引号的处理
                            if (field_start)
                                in_quata = true;
                            else
                                field.Append(ch);
                            break;
                        case '\r': //新的记录行开始
                            if (field.Length > 0 || field_start)
                            {
                                line.Add(field.ToString());
                                field.Remove(0, field.Length);
                            }
                            text_array.Add(line.ToArray());
                            line.Clear();
                            field_start = true;
                            //在 window 环境下，\r\n通常是成对出现，所以要跳过
                            if (i < text.Length - 1 && text[i + 1] == '\n')
                                i++;
                            break;
                        default:
                            field_start = false;
                            field.Append(ch);
                            break;
                    }
                }
            }
            //文件结束
            if (field.Length > 0 || field_start)
                line.Add(field.ToString());
            if (line.Count > 0)
                text_array.Add(line.ToArray());
            return text_array.ToArray();
        }


        public static void WriteCSV(string filePathName, List<String[]> ls)
        {
            WriteCSV(filePathName, false, ls);
        }
        //write a file, existed file will be overwritten if append = false  
        public static void WriteCSV(string filePathName, bool append, List<String[]> ls)
        {
            StreamWriter fileWriter = new StreamWriter(filePathName, append, Encoding.Default);
            foreach (String[] strArr in ls)
            {
                fileWriter.WriteLine(String.Join(", ", strArr));
            }
            fileWriter.Flush();
            fileWriter.Close();

        }
        public static List<String[]> ReadCSV(string filePathName)
        {
            FileInfo fi = new FileInfo(filePathName);
            List<String[]> ls = new List<String[]>();
            StreamReader fileReader = new StreamReader(filePathName);
            string strLine = "";
            while (strLine != null)
            {
                strLine = fileReader.ReadLine();
                if (strLine != null && strLine.Length > 0)
                {
                    ls.Add(strLine.Split(','));
                    //Debug.WriteLine(strLine);  
                }
            }
            fileReader.Close();
            return ls;
        }
        //MemoryMappedFile 内存映射+ Parallel 并行分块 读写大文件
        private static void SpiltFile(string srcFile, int portionSize)
        {
            string savedPath = @"\\stcsrv-c81\MMFeedHealthyDatacache\2016_07_10\Feedkeys\No_Process_test.txt";
            FileInfo fi = new FileInfo(srcFile);
            // total size in bytes
            Int64 size = fi.Length;
            object locker = new object();
            object writeLock = new object();
            List<MappedFile> mappedFiles = new List<MappedFile>();
            Int64 fileToRead = size;//文件总的大小

            portionSize = portionSize * 1024 * 1024; //每块大小

            Int64 portion = (Int64)Math.Ceiling(size * 1.0 / portionSize); //分成多少块


            MemoryMappedViewAccessor mmf_reader = null;
            Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();
            Int64 fileSize = 0;
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(srcFile, FileMode.Open, "xyz", size))
            {
                //using (var writeMap = MemoryMappedFile.CreateFromFile(savedPath, FileMode.Create, "test", size, MemoryMappedFileAccess.ReadWrite))
                //{
                //bool mutexCreated;
                //Mutex mutex = new Mutex(true, "testmapmutex", out mutexCreated);//进程间同步
                Parallel.For(0, portion, (i, ParallelLoopState) =>
                {

                    //for (int i = 26; i < portion; i++)
                    //{
                    lock (locker)
                    {
                        fileSize = Math.Min(portionSize, fileToRead - portionSize * i);
                        if (fileSize > 0)
                        {
                            byte[] buffer;
                            using (mmf_reader = mmf.CreateViewAccessor(i * portionSize, fileSize, MemoryMappedFileAccess.Read))
                            {
                                buffer = new byte[fileSize];
                                mmf_reader.ReadArray(0, buffer, 0, (int)fileSize);
                                mappedFiles.Add(new MappedFile
                                {
                                    Offset = i * portionSize, //fileOffset,
                                    Buffer = buffer,
                                    FileSize = fileSize
                                });
                            }

                            //fileToRead -= fileSize;
                            //lock (writeLock)
                            //{
                            //using (var writeMmf = MemoryMappedFile.OpenExisting("xyz"))
                            //{
                            //    using (var writeAccessor = writeMmf.CreateViewStream(i * portionSize, fileSize))
                            //    {
                            //        var w = new BinaryWriter(new FileStream(savedPath, FileMode.Create, FileAccess.Write));
                            //        //writeAccessor.WriteArray(i * portionSize, buffer, 0, buffer.Length);
                            //        //writeAccessor.Write(buffer, 0, buffer.Length);
                            //        w.Write(buffer);
                            //    }
                            //}

                            //using (MemoryMappedViewAccessor writeView = writeMap.CreateViewAccessor())
                            //{
                            //    writeView.WriteArray(i * portionSize, buffer, 0, (int)fileSize);
                            //}

                        }
                        //}
                    }

                });
            }


            using (var writeMap = MemoryMappedFile.CreateFromFile(savedPath, FileMode.Create, "test", size, MemoryMappedFileAccess.ReadWrite))
            {
                using (MemoryMappedViewAccessor writeView = writeMap.CreateViewAccessor())
                {
                    Parallel.For(0, mappedFiles.Count, i =>
                    {
                        try
                        {
                            Monitor.Enter(locker);
                            writeView.WriteArray(mappedFiles[i].Offset, mappedFiles[i].Buffer, 0, (int)mappedFiles[i].FileSize);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            Monitor.Exit(locker);
                        }

                    });
                }
            }
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);

        }

        public class MappedFile
        {
            public long Offset { get; set; }
            public byte[] Buffer { get; set; }
            public long FileSize { get; set; }
        }


        ////读取超大文件 
        //private static void ReadBigFile( string sTmpFile,string tTmpFile,int )
        //{
        //    //string sTmpFile = @"c:\tmpTest.txt";
        //    if (File.Exists(sTmpFile))
        //    {
        //        File.Delete(sTmpFile);
        //    }

        //    if (!File.Exists(sTmpFile))
        //    {
        //        FileStream fs;
        //        fs = File.Create(sTmpFile);
        //        fs.Close();
        //    }

        //    //if (!File.Exists(txtFileName.Text.Trim()))
        //    //{
        //    //    lblResult.Text = "File not exist!";
        //    //    txtFileName.Focus();
        //    //    return false;
        //    //}

        //    FileStream streamInput = File.OpenRead(tTmpFile);
        //    FileStream streamOutput = File.OpenWrite(sTmpFile);

        //    int iRowCount = 10;
        //    int.TryParse(rowNum, out iRowCount);

        //    try
        //    {
        //        for (int i = 1; i <= iRowCount;)
        //        {
        //            int result = streamInput.ReadByte();
        //            if (result == 13)
        //            {
        //                i++;
        //            }
        //            if (result == -1)
        //            {
        //                break;
        //            }
        //            streamOutput.WriteByte((byte)result);
        //        }
        //    }
        //    finally
        //    {
        //        streamInput.Dispose();
        //        streamOutput.Dispose();
        //    }

        //    string sContent = ReaderFile(sTmpFile);
        //    CopyToClipboard(sContent);

        //    return true;
        //}

        //public static string ReaderFile(string path)
        //{
        //    string fileData = string.Empty;
        //    try
        //    {   ///读取文件的内容      
        //        StreamReader reader = new StreamReader(path, Encoding.Default);
        //        fileData = reader.ReadToEnd();
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        // throw new Exception(ex.Message,ex);    
        //    }  ///抛出异常      
        //    return fileData;
        //}

        //private void CopyToClipboard(string sSource)
        //{
        //    Clipboard.Clear();
        //    if (!string.IsNullOrEmpty(sSource))
        //    {
        //        Clipboard.SetText(sSource);
        //    }
        //}


        byte[] byData = new byte[100];
        char[] charData = new char[1000];
        public void Read()
        {
            try
            {
                StreamReader reader = new StreamReader("E:\\test.txt", Encoding.Default);
                reader.ReadLine();
                StreamWriter writer = new StreamWriter("E:\\test.txt",true ,Encoding.Default);
                
                FileStream file = new FileStream("E:\\test.txt", FileMode.Open);
                file.Seek(0, SeekOrigin.Begin);
                file.Read(byData, 0, 100); //byData传进来的字节数组,用以接受FileStream对象中的数据,第2个参数是字节数组中开始写入数据的位置,它通常是0,表示从数组的开端文件中向数组写数据,最后一个参数规定从文件读多少字符.
                Decoder d = Encoding.Default.GetDecoder();
                d.GetChars(byData, 0, byData.Length, charData, 0);
                Console.WriteLine(charData);
                file.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void Read(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line.ToString());
            }
        }
        public void Write()
        {
            FileStream fs = new FileStream("E:\\ak.txt", FileMode.Create);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes("Hello World!");
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }
        public void Write(string path)
        {
            FileInfo fi = new FileInfo(path);
                 fi.AppendText();
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write("Hello World!!!!");
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
    }
}

