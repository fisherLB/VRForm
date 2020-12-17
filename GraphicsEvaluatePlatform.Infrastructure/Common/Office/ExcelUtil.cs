using ICSharpCode.SharpZipLib.Zip;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace GraphicsEvaluatePlatform.Infrastructure.Common.Office
{
    public class ExcelUtil
    {
        private const int MAXSHEETROW = 65535;
        public static OperationResult WriteToExcel(DataTable dt, string filepath)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            MemoryStream ms = new MemoryStream();    //创建内存流用于写入文件       
            IWorkbook ibook = new HSSFWorkbook();   //创建Excel工作部   
            ISheet sheet = ibook.CreateSheet();//创建工作表
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow irow = sheet.CreateRow(sheet.LastRowNum);//在工作表中添加一行
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string value = dt.Rows[i][j].ToString().Trim();
                    ICell cell = irow.CreateCell(0);//创建单元格
                    cell.SetCellValue(value);//赋值
                }
            }
            ibook.Write(ms);//将Excel写入流
            ms.Flush();
            ms.Position = 0;
            FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            ms.WriteTo(fs);//将流写入文件
            return ret;
        }
        public static OperationResult writetoexcel(DataTable dt, string filepath)
        {

            OperationResult ret = new OperationResult(OperationResultType.Success);
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Close();
            }
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);//读取流

            POIFSFileSystem ps = new POIFSFileSystem(fs);//需using NPOI.POIFS.FileSystem;
            IWorkbook ibook = new HSSFWorkbook(ps);
            ISheet sheet = ibook.GetSheetAt(0);//获取工作表
            IRow row = sheet.GetRow(0); //得到表头
            FileStream fout = new FileStream(filepath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);//写入流
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    row = sheet.CreateRow((sheet.LastRowNum + 1));//在工作表中添加一行
                    ICell cell1 = row.CreateCell(0);
                    cell1.SetCellValue(dt.Rows[i][j].ToString().Trim());//赋值
                }
            }
            fout.Flush();
            ibook.Write(fout);//写入文件
            ibook = null;
            fout.Close();
            return ret;
        }
        public static OperationResult WirteToExcel(DataTable dt, string filepath)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            DataTable data = new DataTable();
            MemoryStream ms = new MemoryStream();
            FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            IWorkbook ibook = null;
            int startRow = 0;
            bool ishand = false;

            ISheet sheet = null;
            if (fs.Length > 0)
            {
                ibook = WorkbookFactory.Create(fs);
                sheet = ibook.GetSheetAt(ibook.NumberOfSheets - 1);
            }
            else
            {
                ibook = new HSSFWorkbook();
                sheet = ibook.CreateSheet();
            }
            IRow irow = sheet.GetRow(0); //得到表头
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (sheet.LastRowNum < 65535)
                {
                    irow = sheet.CreateRow(sheet.LastRowNum + 1);
                }
                else
                {
                    sheet = ibook.CreateSheet(ibook.NumberOfSheets.ToString());
                }
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = irow.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString().Trim());
                }
            }
            ibook.Write(ms);
            ms.WriteTo(fs);
            ms.Flush();
            ms.Position = 0;
            return ret;
        }
        #region Excel复制行
        /// <summary>
        /// Excel复制行
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="sheet"></param>
        /// <param name="starRow"></param>
        /// <param name="rows"></param>
        private static void InsertRow(IWorkbook wb, ISheet sheet, int starRow, int rowsNum)
        {
            /*
             * ShiftRows(int startRow, int endRow, int n, bool copyRowHeight, bool resetOriginalRowHeight); 
             * 
             * startRow 开始行
             * endRow 结束行
             * n 移动行数
             * copyRowHeight 复制的行是否高度在移
             * resetOriginalRowHeight 是否设置为默认的原始行的高度
             * 
             */
            sheet.ShiftRows(starRow + 1, sheet.LastRowNum, rowsNum, true, true);

            starRow = starRow - 1;
            for (int i = 0; i < rowsNum; i++)
            {
                HSSFRow sourceRow = null;
                HSSFRow targetRow = null;
                HSSFCell sourceCell = null;
                HSSFCell targetCell = null;
                short m;
                starRow = starRow + 1;
                sourceRow = (HSSFRow)sheet.GetRow(starRow);
                targetRow = (HSSFRow)sheet.CreateRow(starRow + 1);
                targetRow.HeightInPoints = sourceRow.HeightInPoints;
                for (m = (short)sourceRow.FirstCellNum; m < sourceRow.LastCellNum; m++)
                {
                    sourceCell = (HSSFCell)sourceRow.GetCell(m);
                    targetCell = (HSSFCell)targetRow.CreateCell(m);
                    //                    targetCell.Encoding = sourceCell.Encoding;
                    targetCell.CellStyle = sourceCell.CellStyle;
                    targetCell.SetCellType(sourceCell.CellType);
                }
            }
        }
        #endregion
        /// <summary>
        /// DataSet 写入文件
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static byte[] DataSetToFileStream(DataSet ds, string tableName)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            DataTable dt = ds.Tables[0];
            try
            {
                MemoryStream ms = new MemoryStream();
                byte[] buffer = null;
                using (ZipFile zfile = ZipFile.Create(ms))
                {
                    zfile.BeginUpdate();
                    int count = dt.Rows.Count / MAXSHEETROW + 1;
                    int rowsnum = 0;
                    int sheettotal = dt.Rows.Count / MAXSHEETROW + 1;
                    int filetotal = sheettotal / 3 + 1;
                    string FilePathName = string.Empty;
                    for (int i = 0; i < filetotal && rowsnum < dt.Rows.Count; i++)//循环文件
                    {
                        string FileName = i + ".xls";
                        string path = "/Temps/" + FileName;
                        FilePathName = System.Web.HttpContext.Current.Server.MapPath(path);
                        File.Delete(FilePathName);
                        FileStream file = new FileStream(FilePathName, FileMode.OpenOrCreate);
                        HSSFWorkbook book = new HSSFWorkbook();
                        for (int j = 0; j < sheettotal && rowsnum < dt.Rows.Count; j++)//循环sheet
                        {
                            if (j % 3 == 0 && j > 0)
                                break;
                            ISheet isheet = book.CreateSheet("Sheet" + j);
                            IRow handrow = isheet.CreateRow(0);
                            ret = CreateHand(dt, handrow);
                            for (int k = 1; rowsnum < dt.Rows.Count; rowsnum++, k++)//循环DataTabel每行。
                            {
                                if (k % MAXSHEETROW == 0)
                                    break;
                                IRow iRow = isheet.CreateRow(k);
                                DataRow dRow = dt.Rows[rowsnum];
                                ret = FillCell(iRow, dRow, dt.Columns.Count);
                            }
                        }
                        book.Write(file);
                        file.Flush();
                        file.Close();
                        file.Dispose();
                        GC.Collect();
                        zfile.NameTransform = new ZipNameTransform();
                        zfile.Add(FilePathName, FileName);
                    }
                    zfile.CommitUpdate();
                    buffer = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(buffer, 0, buffer.Length);
                }
                return buffer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 设置表头
        /// </summary>
        /// <param name="dt">原table</param>
        /// <param name="handrow">目标表头</param>
        /// <returns></returns>
        private static OperationResult CreateHand(DataTable dt, IRow handrow)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            int colnum = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                string name = dc.ColumnName;
                handrow.CreateCell(colnum++).SetCellValue(dc.ColumnName);
            }
            return ret;
        }
        private static OperationResult FillCell(IRow irow, DataRow drow, int colnum)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            for (int j = 0; j < colnum; j++)
            {
                irow.CreateCell(j).SetCellValue(drow[j].ToString());
            }
            return ret;
        }
        public static string GetFileStreamForDataSet(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            //创建Excel文件的对象
            HSSFWorkbook book = new HSSFWorkbook();
            int sheetnum = 1;
            int rownum = 0;
            int colnum = 0;
            ISheet sheet1 = book.CreateSheet("Sheet" + sheetnum);
            IRow handrow = sheet1.CreateRow(rownum++);
            //添加第一行的头部标题   
            foreach (DataColumn dc in dt.Columns)
            {

                string name = dc.ColumnName;
                handrow.CreateCell(colnum++).SetCellValue(dc.ColumnName);
            }
            IRow row1 = sheet1.CreateRow(1);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i % MAXSHEETROW == 0 && i > 0)
                {
                    rownum = 0;
                    colnum = 0;
                    sheet1 = book.CreateSheet("Sheet" + (++sheetnum));
                    handrow = sheet1.CreateRow(rownum++);
                    //添加第一行的头部标题  
                    foreach (DataColumn dc in dt.Columns)
                    {
                        string name = dc.ColumnName;
                        handrow.CreateCell(colnum++).SetCellValue(dc.ColumnName);
                    }
                }
                IRow rowtemp = sheet1.CreateRow(rownum++);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    rowtemp.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                }
            }
            string FileName = "UserLog" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
            string path = "/Documents/" + FileName;
            string FilePathName = System.Web.HttpContext.Current.Server.MapPath(path);

            File.Delete(FilePathName);
            FileStream file = new FileStream(FilePathName, FileMode.OpenOrCreate);
            book.Write(file);
            book.Write(file);
            file.Close();
            file.Dispose();
            return FilePathName;
        }
        public static byte[] GetByteForDataSet(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            //创建Excel文件的对象
            HSSFWorkbook book = new HSSFWorkbook();
            int sheetnum = 1;
            int rownum = 0;
            int colnum = 0;
            ISheet sheet1 = book.CreateSheet("Sheet" + sheetnum);
            IRow handrow = sheet1.CreateRow(rownum++);
            //添加第一行的头部标题   
            foreach (DataColumn dc in dt.Columns)
            {

                string name = dc.ColumnName;
                handrow.CreateCell(colnum++).SetCellValue(dc.ColumnName);
            }
            IRow row1 = sheet1.CreateRow(1);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i % MAXSHEETROW == 0 && i > 0)
                {
                    rownum = 0;
                    colnum = 0;
                    sheet1 = book.CreateSheet("Sheet" + (++sheetnum));
                    handrow = sheet1.CreateRow(rownum++);
                    //添加第一行的头部标题  
                    foreach (DataColumn dc in dt.Columns)
                    {
                        string name = dc.ColumnName;
                        handrow.CreateCell(colnum++).SetCellValue(dc.ColumnName);
                    }
                }
                IRow rowtemp = sheet1.CreateRow(rownum++);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    rowtemp.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                }
            }
            return ToByte(book);
        }

        public static byte[] GetByteForDataTable(DataTable dt)
        {

            //创建Excel文件的对象
            HSSFWorkbook book = new HSSFWorkbook();
            int sheetnum = 1;
            int rownum = 0;
            int colnum = 0;
            ISheet sheet1 = book.CreateSheet("Sheet" + sheetnum);
            IRow handrow = sheet1.CreateRow(rownum++);
            //添加第一行的头部标题   
            foreach (DataColumn dc in dt.Columns)
            {

                string name = dc.ColumnName;
                handrow.CreateCell(colnum++).SetCellValue(dc.ColumnName);
            }
            IRow row1 = sheet1.CreateRow(1);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i % MAXSHEETROW == 0 && i > 0)
                {
                    rownum = 0;
                    colnum = 0;
                    sheet1 = book.CreateSheet("Sheet" + (++sheetnum));
                    handrow = sheet1.CreateRow(rownum++);
                    //添加第一行的头部标题  
                    foreach (DataColumn dc in dt.Columns)
                    {
                        string name = dc.ColumnName;
                        handrow.CreateCell(colnum++).SetCellValue(dc.ColumnName);
                    }
                }
                IRow rowtemp = sheet1.CreateRow(rownum++);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    rowtemp.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                }
            }
            return ToByte(book);
        }
        private static byte[] ToByte(HSSFWorkbook book)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //XSSFWorkbook即读取.xlsx文件返回的MemoryStream是关闭
                //但是可以ToArray(),这是NPOI的bug
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// DataTable生成Sheet
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static ISheet DataTableToSheet(DataTable dt, ISheet sheet)
        {
            int rowIndex = 0;//行数
            int colsIndex = 0;//列数       
            IRow handrow = sheet.CreateRow(rowIndex++);//新增行.
            //添加第一行的头部标题   生成表头.
            foreach (DataColumn dc in dt.Columns)
            {
                string name = dc.ColumnName;
                handrow.CreateCell(colsIndex++).SetCellValue(dc.ColumnName);
            }
            IRow row1 = sheet.CreateRow(1);//新增一行.
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                rowIndex = 0;
                colsIndex = 0;
                handrow = sheet.CreateRow(rowIndex++);
                IRow rowtemp = sheet.CreateRow(rowIndex++);
                for (int j = 0; j < dt.Columns.Count; j++)
                    rowtemp.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
            }
            return sheet;
        }
        /// <summary>
        /// 根据DataSet,fileType 获取Excel 的 byte[]
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static byte[] GetByteForExcel(DataSet ds)
        {
            //创建Excel文件的对象
            HSSFWorkbook book = new HSSFWorkbook();
            DataTable dt = ds.Tables[0];
            int sheetnum = 1;
            int rownum = 0;
            int colnum = 0;
            ISheet sheet1 = book.CreateSheet("Sheet" + sheetnum);
            IRow handrow = sheet1.CreateRow(rownum++);
            //添加第一行的头部标题   
            foreach (DataColumn dc in dt.Columns)
            {

                string name = dc.ColumnName;
                handrow.CreateCell(colnum++).SetCellValue(dc.ColumnName);
            }

            IRow row1 = sheet1.CreateRow(1);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i % MAXSHEETROW == 0 && i > 0)
                {
                    rownum = 0;
                    colnum = 0;
                    sheet1 = book.CreateSheet("Sheet" + (++sheetnum));
                    handrow = sheet1.CreateRow(rownum++);

                    //添加第一行的头部标题  
                    foreach (DataColumn dc in dt.Columns)
                    {

                        string name = dc.ColumnName;
                        handrow.CreateCell(colnum++).SetCellValue(dc.ColumnName);
                    }
                }
                IRow rowtemp = sheet1.CreateRow(rownum++);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    rowtemp.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());

                }


            }
            return ToByte(book);

            // 写入到客户端 
            //MemoryStream ms = new MemoryStream();
            //book.Write(ms);
            //return ms.ToArray();
            //ms.Seek(0, SeekOrigin.Begin);
            //return File(ms, "application/vnd.ms-excel", fileName+".xls");
        }

        /// <summary>
        /// 根据DataSet,fileType 获取Excel 的 byte[]
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static byte[] GetByteForExcel(DataTable dt)
        {
            //创建Excel文件的对象
            HSSFWorkbook book = new HSSFWorkbook();
            int sheetnum = 1;
            int rownum = 0;
            int colnum = 0;
            ISheet sheet1 = book.CreateSheet("Sheet" + sheetnum);
            IRow handrow = sheet1.CreateRow(rownum++);
            //添加第一行的头部标题   
            foreach (DataColumn dc in dt.Columns)
            {
                string name = dc.ColumnName;
                handrow.CreateCell(colnum++).SetCellValue(dc.ColumnName);
            }

            IRow row1 = sheet1.CreateRow(1);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i % MAXSHEETROW == 0 && i > 0)
                {
                    rownum = 0;
                    colnum = 0;
                    sheet1 = book.CreateSheet("Sheet" + (++sheetnum));
                    handrow = sheet1.CreateRow(rownum++);

                    //添加第一行的头部标题  
                    foreach (DataColumn dc in dt.Columns)
                    {

                        string name = dc.ColumnName;
                        handrow.CreateCell(colnum++).SetCellValue(dc.ColumnName);
                    }
                }
                IRow rowtemp = sheet1.CreateRow(rownum++);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    rowtemp.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                }
            }
            return ToByte(book);
        }

        #region 导入模块

        /// <summary>
        /// 清除数据源中空白行
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable RemoveNullRow(DataTable dt)
        {
            bool isNullRow = true;
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][j] != null && dt.Rows[i][j].ToString() != "")
                    {
                        isNullRow = false;
                        break;
                    }
                }
                if (isNullRow == true)
                    dt.Rows.Remove(dt.Rows[i]);
                isNullRow = true;
            }
            return dt;
        }
        /// <summary>
        /// Excel导入DataTable
        /// </summary>
        /// <param name="excelPath">Excel路径</param>
        /// <param name="sheetName">工作</param>
        /// <param name="firstRowAsHeader">第一行是否为表头</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string excelPath, string sheetName, bool firstRowAsHeader)
        {
            using (FileStream fileStream = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook ibook = new HSSFWorkbook(fileStream);
                HSSFFormulaEvaluator evaluator = new HSSFFormulaEvaluator(ibook);
                ISheet sheet;

                if (string.IsNullOrEmpty(sheetName))
                {
                    sheet = ibook.GetSheetAt(0);

                }
                else
                {
                    sheet = ibook.GetSheet(sheetName);
                }
                if (sheet == null)
                    throw new NullReferenceException("没有找到<" + sheetName + ">工作表");
                if (firstRowAsHeader)
                {
                    return SheetToDataTableFirstRowAsHeader(sheet, evaluator);
                }
                else
                {
                    return SheetToDataTable(sheet, evaluator);
                }
            }
        }
        public static DataTable GetDataTable(string filePath, string sheetName, Dictionary<int, object> dic, int pages, int rows, bool firstRowAsHeader)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                #region 读取iwook
                HSSFWorkbook ibook = new HSSFWorkbook(fileStream);
                HSSFFormulaEvaluator evaluator = new HSSFFormulaEvaluator(ibook);
                ISheet sheet;
                if (string.IsNullOrEmpty(sheetName))
                {
                    sheet = ibook.GetSheetAt(0);
                }
                else
                {
                    sheet = ibook.GetSheet(sheetName);
                }
                if (sheet == null)
                    throw new NullReferenceException("没有找到<" + sheetName + ">工作表");
                #endregion

                if (firstRowAsHeader)
                {
                    return SheetToDataTableFirstRowAsHeader(sheet, dic, pages, rows, evaluator);
                }
                else
                {
                    return SheetToDataTable(sheet, dic, pages, rows, evaluator);
                }
            }
        }


        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelFileToDataTable(string fileName, string sheetName, bool isFirstRowColumn)
        {
            IWorkbook ibook = null;
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    ibook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    ibook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = ibook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = ibook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = ibook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue.Trim();
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            ICell cell = row.GetCell(j);
                            if (cell != null)
                            {       //Cell为非NUMERIC时，调用IsCellDateFormatted方法会报错，所以先要进行类型判断
                                if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                                    dataRow[j] = cell.DateCellValue.ToString("yyyyMMdd");
                                else
                                {
                                    dataRow[j] = row.GetCell(j).ToString();
                                }
                            }
                            else
                            {
                                dataRow[j] = "";
                            }
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        //第一行作为标题
        private static DataTable SheetToDataTableFirstRowAsHeader(ISheet sheet, HSSFFormulaEvaluator evaluator)
        {
            using (DataTable dt = new DataTable())
            {
                // IRow firstRow = sheet.GetRow(0) ;//第一行作为标题
                IRow firstRow;
                //IRow secondRow;
                try
                {
                    firstRow = sheet.GetRow(0); //第1行做表头
                    //secondRow = sheet.GetRow(2); //或者第三行
                }
                catch (Exception)
                {


                    throw new NullReferenceException("表头设置错误");
                }

                int cellCount = GetCellCount(sheet);


                for (int i = 0; i < cellCount; i++) //从第一列开始取数据
                {
                    //if (firstRow.GetCell(i) != null && secondRow.GetCell(i) != null)
                    if (firstRow.GetCell(i) != null)
                    {
                        //先取第二行，数据为空则取第三行
                        //if (!string.IsNullOrEmpty(firstRow.GetCell(i).StringCellValue.Trim()) || !string.IsNullOrEmpty(secondRow.GetCell(i).StringCellValue.Trim()))
                        //dt.Columns.Add((!string.IsNullOrEmpty(firstRow.GetCell(i).StringCellValue.Trim()) ? firstRow.GetCell(i).StringCellValue.Trim() : (secondRow.GetCell(i).StringCellValue.Trim() ?? string.Format("F{0}", i + 1))), typeof(string));
                        //double d = firstRow.GetCell(i).NumericCellValue;
                        string s = firstRow.GetCell(i).ToString(); //firstRow.GetCell(i).StringCellValue.Trim();
                        dt.Columns.Add(firstRow.GetCell(i).ToString() ?? string.Format("F{0}", i + 1),
                            typeof(string));
                    }
                    else
                    {
                        dt.Columns.Add(string.Format("F{0}", i + 1), typeof(string));
                    }
                }

                // for (int i = 1; i <= sheet.LastRowNum; i++)//从第二行开始取数据
                for (int i = 1; i <= sheet.LastRowNum; i++) //从第4行开始
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dr = dt.NewRow();
                    FillDataRowByHSSFRow(row, evaluator, ref dr);
                    dt.Rows.Add(dr);
                }

                dt.TableName = sheet.SheetName;
                return dt;
            }
        }
        //导入DataTable 
        private static DataTable SheetToDataTable(ISheet sheet, HSSFFormulaEvaluator evaluator)
        {
            using (DataTable dt = new DataTable())
            {
                if (sheet.LastRowNum != 0)
                {
                    int cellCount = GetCellCount(sheet);

                    for (int i = 0; i < cellCount; i++)
                    {
                        dt.Columns.Add(string.Format("F{0}", i), typeof(string));
                    }

                    for (int i = 0; i < sheet.FirstRowNum; ++i)
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.Add(dr);
                    }

                    for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        DataRow dr = dt.NewRow();
                        FillDataRowByHSSFRow(row, evaluator, ref dr);
                        dt.Rows.Add(dr);
                    }
                }

                dt.TableName = sheet.SheetName;
                return dt;
            }
        }
        private static DataTable SheetToDataTable(ISheet sheet, Dictionary<int, object> dic, int pages, int rows, HSSFFormulaEvaluator evaluator)
        {
            using (DataTable dt = new DataTable())
            {
                if (sheet.LastRowNum != 0)//sheet 不为空。行数 不为零
                {
                    int cellCount = GetCellCount(sheet);//取列数
                    for (int i = 0; i < cellCount; i++)//遍历每一列
                    {
                        dt.Columns.Add(string.Format("F{0}", i), typeof(string));//填值 
                    }
                    for (int i = 0; i < sheet.FirstRowNum; ++i)
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.Add(dr);
                    }

                    for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        DataRow dr = dt.NewRow();
                        FillDataRowByHSSFRow(row, evaluator, ref dr);
                        dt.Rows.Add(dr);
                    }
                }

                dt.TableName = sheet.SheetName;
                return dt;
            }
        }

        private static DataTable SheetToDataTableFirstRowAsHeader(ISheet sheet, Dictionary<int, object> dic, int pages, int rows, HSSFFormulaEvaluator evaluator)
        {
            using (DataTable dt = new DataTable())
            {
                if (sheet.LastRowNum != 0)
                {
                    int cellCount = GetCellCount(sheet);
                    for (int i = 0; i < cellCount && i < dic.Count; i++)
                    {
                        if (dic.ContainsKey(i) && dic[i].ToString() != "-1")
                        {
                            dt.Columns.Add(string.Format("{0}", dic[i]), typeof(string));
                        }
                    }

                    for (int i = 0; i < sheet.FirstRowNum; ++i)
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.Add(dr);
                    }

                    for (int i = rows * (pages - 1); i < ((rows * pages)) && i <= sheet.LastRowNum; i++)
                    {
                        if (i == 0)
                        {
                            continue;
                        }
                        if (i <= sheet.LastRowNum)
                        {
                            IRow row = sheet.GetRow(i);
                            DataRow dr = dt.NewRow();
                            FillDataRowByHSSFRow(row, evaluator, ref dr);
                            dt.Rows.Add(dr);
                        }
                    }
                }

                dt.TableName = sheet.SheetName;
                return dt;
            }
        }











        /// <summary>
        /// 通过IRow填充DataRow
        /// </summary>
        /// <param name="row"></param>
        /// <param name="evaluator"></param>
        /// <param name="dr"></param>
        private static void FillDataRowByHSSFRow(IRow row, HSSFFormulaEvaluator evaluator, ref DataRow dr)
        {
            if (row != null)
            {
                for (int j = 0; j < dr.Table.Columns.Count; j++)
                {
                    HSSFCell cell = row.GetCell(j) as HSSFCell;

                    if (cell != null)
                    {
                        switch (cell.CellType)
                        {
                            case CellType.Blank:
                                dr[j] = DBNull.Value;
                                break;
                            case CellType.Boolean:
                                dr[j] = cell.BooleanCellValue;
                                break;
                            case CellType.Numeric:
                                if (DateUtil.IsCellDateFormatted(cell))
                                {
                                    dr[j] = cell.DateCellValue;
                                }
                                else
                                {
                                    dr[j] = cell.NumericCellValue;
                                }
                                break;
                            case CellType.String:
                                dr[j] = cell.StringCellValue.Trim();
                                break;
                            case CellType.Error:
                                dr[j] = cell.ErrorCellValue;
                                break;
                            case CellType.Formula:
                                cell = evaluator.EvaluateInCell(cell) as HSSFCell;
                                dr[j] = cell.ToString().Trim();
                                break;
                            default:
                                throw new NotSupportedException(string.Format("Catched unhandle CellType[{0}]",
                                    cell.CellType));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 得到cell总数
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private static int GetCellCount(ISheet sheet)
        {
            int firstRowNum = sheet.FirstRowNum;

            int cellCount = 0;

            for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i)
            {
                IRow row = sheet.GetRow(i);

                if (row != null && row.LastCellNum > cellCount)
                {
                    cellCount = row.LastCellNum;
                }
            }

            return cellCount;
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlForFacility"></param>
        /// <param name="sqlForDisease"></param>
        /// <param name="strColumn"></param>
        /// <param name="OID"></param>
        /// <param name="num"></param>
        /// <param name="iworkbook"></param>
        /// <returns></returns>
        public static IWorkbook DownInspectReport(DataTable dt, DataTable disSource, IList<string> strColumn, string OID, int num, IWorkbook iworkbook)
        {
            //dt = DataBll.Query(@"select my11 as '密级' ,count(0)as '总数'
            //from GDGL1307532 f left
            //join GDGLMgr1307532 m on f.Cate_id = m.CateID
            //where m.IsDelete = 0
            //and m.IsDestory = 0
            //and m.IsTransfer = 0
            //and(my11 != '' and my11 != '无密')
            //group by my11
            //order by my11 ");
            dt.Columns.Add("10年", Type.GetType("string"));
            dt.Columns.Add("30年", Type.GetType("string"));
            dt.Columns.Add("短期", Type.GetType("string"));
            dt.Columns.Add("长期", Type.GetType("string"));
            dt.Columns.Add("永久", Type.GetType("string"));
            //BLPageControl pgControl = new BLPageControl();
            //string path = HttpContext.Current.Server.MapPath("~/Template/Template1.xls");
            ////IWorkbook iworkbook;
            //////////////
            ////读取模板//
            //////////////
            //using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            //{
            //    iworkbook = new HSSFWorkbook(stream);
            //}

            ISheet sheet = iworkbook.GetSheetAt(0);

            //取数据。
            //   DataTable dt = new DataTable();// pgControl.GetDataForSql(sqlForFacility);
            ///////////////////////////
            //从第五行开始，填充数据///
            ///////////////////////////
            int rowIndex = 3 + num * 23;
            foreach (DataRow row in dt.Rows)
            {
                IRow irow = sheet.GetRow(rowIndex);
                int columnCount = strColumn.Count * 2;
                string[] list = { "总面积" };
                for (int i = 0, j = 0, k = 0; i < columnCount; i++)
                {
                    ICell newCell = irow.GetCell(i);
                    if (i % 2 == 0)
                    {
                        string drValue = strColumn[j] + "：";
                        newCell.SetCellValue(drValue);
                        j++;
                    }
                    else
                    {

                        string colName = strColumn[k];
                        if (colName == "设备等级")
                        {
                            colName = "本年评定等级";
                        }
                        string drValue = row[colName].ToString();
                        if (list[0].ToString() != colName)
                            newCell.SetCellValue(drValue);
                        else
                            newCell.SetCellValue(Convert.ToDouble(drValue));
                        k++;
                    }
                }
                ////int sourceIndex = 0;
                //// int targetIndex = 0;
                ////statict_sheet.CopyRow(sourceIndex, targetIndex);


            }
            ///////////////////////////////
            //从第8行开始，再次填充数据////
            ///////////////////////////////
            rowIndex += 3;
            //  BLPageControl dispgControl = new BLPageControl();
            //  DataTable disSource = new DataTable();// pgControl.GetDataForSql(sqlForDisease);
            string[] str1 = { "所属病害", "检查说明" };
            foreach (DataRow dirow in disSource.Rows)
            {
                for (int i = 0; i < str1.Length; i++)
                {
                    string col = str1[i];
                    string drV = dirow[col].ToString();

                    if (drV != "")
                    {
                        IRow disRow = sheet.CreateRow(rowIndex++);
                        disRow.HeightInPoints = 20;

                        ICell newCell = disRow.CreateCell(0);
                        newCell.SetCellValue(drV);
                    }
                }
            }
            return iworkbook;
        }
        #region 导出
        /// <summary>
        /// 从filepath得workbook
        /// 从workbook得sheet
        /// 将datatble写入sheet
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static OperationResult DataTableToFile(DataTable dt, string filepath, string sheetname)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                if (!File.Exists(filepath))
                {
                    MemoryStream ms = new MemoryStream();    //创建内存流用于写入文件       
                    IWorkbook ibook = new HSSFWorkbook();   //创建Excel工作部   
                    ISheet sheet = ibook.CreateSheet(sheetname);//创建工作表
                    sheet.ForceFormulaRecalculation = true; //强制要求Excel在打开时重新计算的属性       
                    IRow header = sheet.CreateRow(sheet.LastRowNum);//在工作表中添加一行
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell cell = header.CreateCell(i);
                        string val = dt.Columns[i].Caption ?? dt.Columns[i].ColumnName;
                        cell.SetCellValue(val);
                    }
                    int rowIndex = 1;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dtRow = dt.Rows[i];
                        IRow excelRow = sheet.CreateRow(rowIndex++);
                        for (int j = 0; j < dtRow.ItemArray.Length; j++)
                        {
                            excelRow.CreateCell(j).SetCellValue(dtRow[j].ToString());
                        }
                    }

                    ibook.Write(ms);//将Excel写入流
                    ms.Flush();
                    ms.Position = 0;

                    FileStream dumpFile = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    ms.WriteTo(dumpFile);//将流写入文件
                    ibook = null;
                    dumpFile.Close();
                    ms.Dispose();
                }
                else
                {
                    FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);//读取流

                    POIFSFileSystem ps = new POIFSFileSystem(fs);//需using NPOI.POIFS.FileSystem;
                    IWorkbook workbook = new HSSFWorkbook(ps);
                    ISheet sheet = workbook.GetSheetAt(0);//获取工作表
                    IRow row = sheet.GetRow(0); //得到表头
                    FileStream fout = new FileStream(filepath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);//写入流
                    if (sheet.SheetName != sheetname)
                    {
                        sheet = workbook.CreateSheet(sheetname);
                        IRow header = sheet.CreateRow(sheet.LastRowNum);//在工作表中添加一行
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            ICell cell = header.CreateCell(i);
                            string val = dt.Columns[i].Caption ?? dt.Columns[i].ColumnName;
                            cell.SetCellValue(val);
                        }
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dtRow = dt.Rows[i];
                        row = sheet.CreateRow((sheet.LastRowNum + 1));//在工作表中添加一行
                                                                      // IRow excelRow = sheet.CreateRow(rowIndex++);
                        for (int j = 0; j < dtRow.ItemArray.Length; j++)
                        {
                            row.CreateCell(j).SetCellValue(dtRow[j].ToString());
                        }
                    }
                    fout.Flush();
                    workbook.Write(fout);//写入文件
                    workbook = null;
                    fout.Close();
                }

                //     FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);//读取流
                //  //   POIFSFileSystem ps = new POIFSFileSystem(fs);//需using NPOI.POIFS.FileSystem;
                // IWorkbook    ibook = new HSSFWorkbook(fs);
                // FileStream fout = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);//写入流

                // #region datatable写入sheet    
                //// int sheetnum = dt.Rows.Count % MAXSHEETROW;
                // //for (int sheetindex = 0; sheetindex <= sheetnum; sheetindex++)
                // //{
                //     ISheet sheet = ibook.CreateSheet(sheetname);
                //     sheet.ForceFormulaRecalculation = true; //强制要求Excel在打开时重新计算的属性       
                //     IRow header = sheet.CreateRow(0);
                //     for (int i = 0; i < dt.Columns.Count; i++)
                //     {
                //         ICell cell = header.CreateCell(i);
                //         string val = dt.Columns[i].Caption ?? dt.Columns[i].ColumnName;
                //         cell.SetCellValue(val);
                //     }
                //     int rowIndex = 1;
                //     for (int i = 0; i < dt.Rows.Count; i++)
                //     {
                //         DataRow dtRow = dt.Rows[i];
                //         IRow excelRow = sheet.CreateRow(rowIndex++);
                //         for (int j = 0; j < dtRow.ItemArray.Length; j++)
                //         {
                //             excelRow.CreateCell(j).SetCellValue(dtRow[j].ToString());
                //         }
                //     }

                // fout.Flush();
                // ibook.Write(fout);//写入文件
                // ibook = null;
                // fout.Close();

            }
            catch (Exception ex)
            {
                Logging.Logger.GetLogger("ExcelUtil").Error("写入workbook出错了。", ex);
                return new OperationResult(OperationResultType.Error, ex.Message);
            }

            #endregion
            return ret;
        }

        /// <summary>   
        /// 创建 Excel 
        /// </summary>   
        /// <param name="dt">源DataTable</param>
        /// <param name="dirPath">路径</param>
        /// <param name="fileName">文件名</param>
        public static void CreateExcel(DataTable dt, string dirPath, string fileName)
        {
            string fullFilePath = Path.Combine(dirPath, fileName);
            using (FileStream sw = File.Create(fullFilePath))
            {
                var excel = GetWorkbook(dt, fullFilePath);
                excel.Write(sw);
                sw.Close(); //在服务端生成文件
            }
        }
        public static void CreateExcel(DataTable dt, string filepath)
        {
            using (FileStream sw = File.Create(filepath))
            {
                var excel = DataTableToWork(dt, filepath);
                excel.Write(sw);
                sw.Close(); //在服务端生成文件
            }
        }
        /// <summary>   
        /// DataTable导出到Excel的MemoryStream   
        /// </summary>   
        /// <param name="dt">源DataTable</param>   
        /// <param name="fileName">文件名，会作为表头</param>   
        public static IWorkbook DataTableToWork(DataTable dt, string fileName)
        {
            IWorkbook ibook = new HSSFWorkbook();
            string ext = Path.GetExtension(fileName);
            if (ext != null)
            {
                string extension = ext.ToLower();
                if (extension == ".xls")
                    ibook = new HSSFWorkbook();
                else if (extension == ".xlsx")
                    ibook = new XSSFWorkbook();
            }
            ISheet sheet = ibook.CreateSheet();
            sheet.ForceFormulaRecalculation = true; //强制要求Excel在打开时重新计算的属性
            int rowIndex = 0;
            foreach (DataRow drow in dt.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = ibook.CreateSheet();
                    }
                    IRow headerRow = sheet.CreateRow(1);
                    foreach (DataColumn column in dt.Columns)
                    {
                        headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                        //设置列宽   
                    }

                    rowIndex = 2;
                }
                #endregion
                #region 填充内容

                IRow irow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dt.Columns)
                {
                    ICell newCell = irow.CreateCell(column.Ordinal);
                    string drValue = drow[column].ToString();
                    newCell.SetCellValue(drValue);
                }

                #endregion
                rowIndex++;
            }
            return ibook;

        }

        public static void DataTableToSheet(DataSet ds, string filePath, string fileName)
        {
            int allcount = 100000;
            #region
            fileName = "xxxx.xls";
            filePath = "/exceldata/" + fileName;
            string FilePathName = HttpContext.Current.Server.MapPath(filePath);
            File.Delete(FilePathName);
            FileStream Fs = new FileStream(FilePathName, FileMode.Create);
            IWorkbook ibook = new HSSFWorkbook();
            int sheetindex = 1;
            ISheet sheet = ibook.CreateSheet("￈xx" + sheetindex.ToString() + "");
            int pagesize = 1000;
            int currentIndex = 1;
            int sheetcount = 0;
            #endregion
            int fornum = (allcount - allcount % pagesize) / pagesize + 1;
            while (currentIndex <= fornum)
            {
                //DataSet ds = qStationLogsys.GetAllLogPart(pagesize, currentIndex);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (sheetcount % 65535 == 0)
                        {
                            if (sheetindex != 1)
                            {
                                ibook.Write(Fs);
                                //ibook.Clear();
                                Fs.Flush();
                                Fs.Close();
                                Fs.Dispose();
                                Fs = new FileStream(FilePathName, FileMode.Open);
                                sheetcount = 0;
                                sheet = ibook.CreateSheet("xxx" + sheetindex.ToString() + "");
                            }
                            var ret = CreateSheetTitle(ds.Tables[0], sheet);//写入头的内容
                            sheetcount++;
                            sheetindex++;
                            CreateSheetContent(sheet, sheetcount, ds.Tables[0].Rows[i]);
                            //写入内容
                        }
                        else
                        {
                            CreateSheetContent(sheet, sheetcount, ds.Tables[0].Rows[i]);
                            sheetcount++;
                        }
                    }
                }
                currentIndex++;
                ds.Clear();
            }

            ibook.Write(Fs);
            //ibook.Clear();
            Fs.Flush();
            Fs.Close();
            Fs.Dispose();
        }
        public static void DataTableToFile(DataTable dt, ISheet sheet)
        {
            foreach (DataRow dr in dt.Rows)
            {
                IRow row = sheet.CreateRow(0);
                object[] arr = dr.ItemArray;
                for (int i = 0; i < arr.Length; i++)
                {
                    row.CreateCell(i).SetCellValue(arr[i].ToString());
                }
            }


            //创建Excel2007工作簿  
            // IWorkbook book = new XSSFWorkbook();

            //创建Excel2007工作表  
            //  ISheet sheet = book.CreateSheet(sheetName);

            //创建Excel行  
            //

            //给单元格赋值  
            //row.CreateCell(0).SetCellValue("序号");
            //row.CreateCell(1).SetCellValue("大区(区域)");
            //row.CreateCell(2).SetCellValue("省(简)");
            //row.CreateCell(3).SetCellValue("说明");

            /* 
             * 将Excel文件写入相应的Excel文件中 
             */
            //FileStream fs = File.Create(filePath);
            //book.Write(fs);
            //fs.Close();
        }
        private static void CreateSheetContent(ISheet sheet, int sheetcount, DataRow dataRow)
        {
            throw new NotImplementedException();
        }

        private static OperationResult CreateSheetTitle(DataTable dt, ISheet sheet)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            int colnum = 0;
            IRow handrow = sheet.CreateRow(0);
            foreach (DataColumn dc in dt.Columns)
            {
                string name = dc.ColumnName;
                handrow.CreateCell(colnum++).SetCellValue(dc.ColumnName);
            }
            return ret;
        }

        /// <summary>   
        /// DataTable导出到Excel的MemoryStream   
        /// </summary>   
        /// <param name="dt">源DataTable</param>   
        /// <param name="fileName">文件名，会作为表头</param>   
        public static IWorkbook GetWorkbook(DataTable dt, string fileName)
        {
            IWorkbook ibook = new HSSFWorkbook();
            // IWorkbook ibook = new XSSFWorkbook()

            string ext = Path.GetExtension(fileName);
            if (ext != null)
            {
                string extension = ext.ToLower();
                if (extension == ".xls")
                    ibook = new HSSFWorkbook();
                else if (extension == ".xlsx")
                    ibook = new XSSFWorkbook();
            }


            ISheet sheet = ibook.CreateSheet();
            sheet.ForceFormulaRecalculation = true; //强制要求Excel在打开时重新计算的属性
            /*
         * CreateFreezePane()
         * 第一个参数表示要冻结的列数；
         * 第二个参数表示要冻结的行数；
         * 第三个参数表示右边区域可见的首列序号，从1开始计算；
         * 第四个参数表示下边区域可见的首行序号，也是从1开始计算
         */
            sheet.CreateFreezePane(0, 4, 0, 5); //冻结表头与列头
            ICellStyle dateStyle = ibook.CreateCellStyle();
            IDataFormat format = ibook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
            int[] arrColWidth = new int[dt.Columns.Count];//取得列宽  
            foreach (DataColumn item in dt.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dt.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dt.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = ibook.CreateSheet();
                    }
                    #region 表头及样式

                    {
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(Path.GetFileNameWithoutExtension(fileName));

                        ICellStyle headStyle = ibook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;

                        IFont font = ibook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;
                        CellRangeAddress vra = new CellRangeAddress(0, 0, 0, dt.Columns.Count - 1);
                        sheet.AddMergedRegion(vra);
                    }

                    #endregion
                    #region 列头及样式
                    {
                        IRow headerRow = sheet.CreateRow(1);
                        ICellStyle headStyle = ibook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center; // CellHorizontalAlignment.CENTER;
                        IFont font = ibook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dt.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            //设置列宽   
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                    }

                    #endregion
                    rowIndex = 2;
                }
                #endregion
                #region 填充内容

                IRow irow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dt.Columns)
                {
                    ICell newCell = irow.CreateCell(column.Ordinal);

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())//对不同的数据类型，处以不同的方式 。
                    {
                        case "System.String": //字符串类型   
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime": //日期类型  
                            if (drValue == "")
                            {
                                newCell.SetCellValue(drValue);
                            }
                            else
                            {
                                DateTime dateV;
                                DateTime.TryParse(drValue, out dateV);
                                newCell.SetCellValue(dateV);
                                newCell.CellStyle = dateStyle; //格式化显示   
                            }
                            break;
                        case "System.Boolean": //布尔型   
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //整型   
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //浮点型   
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": //空值处理   
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }

                #endregion
                rowIndex++;
            }
            return ibook;

        }


        public byte[] SetSecretData(Dictionary<string, string> dic, string secretStr, string termStr)
        {

            HSSFWorkbook ibook = new HSSFWorkbook();
            //设置样式
            ICellStyle headStyle = ibook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.Center;
            ISheet sheet = ibook.CreateSheet();  //生成一个表格


            var st = termStr.Split(',');
            var tm = secretStr.Split(',');
            int RowsNum = tm.Length + 3;
            int ColsNum = st.Length + 2;  //多少列

            int columIndex = 0;
            int rowIndex = 0;
            for (int i = 0; i < RowsNum; i++)
            {
                IRow row = sheet.CreateRow(i);//创建一行

                for (int j = 0; j < ColsNum; j++)//创建每一个单元格
                {
                    ICell cell = row.CreateCell(j);//创建一个
                    if (j == 0 && i > 1)// 第一列 从第二行开始 依次写密级
                    {
                        if (rowIndex < tm.Length)

                            cell.SetCellValue(tm[rowIndex++]);
                    }
                    if (i <= 1 && j == 0)//设置第一个单元格为“密级”
                    {
                        cell.SetCellValue("密级");
                    }
                    if (i == 0)//第一行 从第二列开始 都是 “保管年限”
                    {
                        if (j > 0 && j < ColsNum - 1)
                            cell.SetCellValue("保管年限");
                        if (j == ColsNum - 1)
                            cell.SetCellValue("合计");
                    }
                    if (i == 1 && j > 0)//第二行 从第二列开始 依次填写 保管年限
                    {
                        if (columIndex < st.Length)
                            cell.SetCellValue(st[columIndex++]);
                        if (j == ColsNum - 1)
                            cell.SetCellValue("合计");

                    }
                    if (i > 1 && i < RowsNum - 1)   //从第二行和第三行之间，填统计数值
                    {

                        if (j > 0 && j < ColsNum - 1)
                        {

                            string cellName = "" + tm[i - 2].ToString() + "-" + st[j - 1];

                            cell.SetCellValue(dic["" + cellName].ToString());


                        }
                        if (j == ColsNum - 1)  //最后一列，合计那一列
                        {
                            if (i > 1 && i < RowsNum - 1) //从第三行开始,除去最后一行
                            {
                                cell.SetCellValue(dic[tm[i - 2]]);  //设置最后一列合计的值
                            }

                        }

                    }
                    if (i == RowsNum - 1 && j > 0)
                    {
                        if (j < ColsNum - 1)
                            cell.SetCellValue(dic[st[j - 1]]);  //设置最后一列合计的值
                        else
                            cell.SetCellValue(dic["totalCount"]);  //设置最后一列合计的值
                    }
                    if (i == RowsNum - 1 && j == 0)  //最后一行，第一列，合计
                    {
                        cell.SetCellValue("合计");
                    }
                }
            }

            //合并单元格
            SetCellRangeAddress(sheet, 0, 1, 0, 0);
            SetCellRangeAddress(sheet, 0, 0, 1, ColsNum - 2);
            SetCellRangeAddress(sheet, 0, 1, ColsNum - 1, ColsNum - 1);
            byte[] bt = ToByte(ibook);
            return bt;
        }
        /// <summary>
        /// 取sheet 总行数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static int GetTableRows(string filePath, string sheetName)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int result = 0;
                #region 读取iwook
                HSSFWorkbook ibook = new HSSFWorkbook(fileStream);
                HSSFFormulaEvaluator evaluator = new HSSFFormulaEvaluator(ibook);
                ISheet sheet;
                if (string.IsNullOrEmpty(sheetName))
                {
                    sheet = ibook.GetSheetAt(0);
                }
                else
                {
                    sheet = ibook.GetSheet(sheetName);
                }
                if (sheet == null)
                    throw new NullReferenceException("没有找到<" + sheetName + ">工作表");
                else
                {
                    result = sheet.LastRowNum;
                }
                #endregion
                return result;
            }

        }



        /// 合并单元格
        /// </summary>
        /// <param name="sheet">要合并单元格所在的sheet</param>
        /// <param name="rowstart">开始行的索引</param>
        /// <param name="rowend">结束行的索引</param>
        /// <param name="colstart">开始列的索引</param>
        /// <param name="colend">结束列的索引</param>
        public static void SetCellRangeAddress(ISheet sheet, int rowstart, int rowend, int colstart, int colend)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(rowstart, rowend, colstart, colend);
            sheet.AddMergedRegion(cellRangeAddress);
        }





        /// <summary>
        /// 使用内存数据用于Web导出表格。
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="strHeaderText"></param>
        /// <param name="strFileName"></param>
        public static void ExportByWeb(MemoryStream ms, HttpContext curContext, string strFileName)
        {
            curContext.Response.Clear();
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(strFileName, System.Text.Encoding.UTF8));
            curContext.Response.BinaryWrite(ms.GetBuffer());
            curContext.Response.Flush();
            curContext.Response.End();// 停止页面的执行  
        }
        public static OperationResult DataTableToExcel(DataTable dt, string filepath)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            MemoryStream ms = new MemoryStream();    //创建内存流用于写入文件       
            IWorkbook workbook = new HSSFWorkbook();   //创建Excel工作部   
            ISheet sheet = workbook.CreateSheet();//创建工作表
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    IRow irow = sheet.CreateRow(i);//在工作表中添加一行
                    ICell cell = irow.CreateCell(j);//创建单元格
                    cell.SetCellValue(dt.Rows[i][j].ToString().Trim());//赋值
                }
            }
            workbook.Write(ms);//将Excel写入流
            ms.Flush();
            ms.Position = 0;
            FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            ms.WriteTo(fs);//将流写入文件k
            return ret;

        }



        #region 导出2
        /// <summary>
        /// 将DataTable转换为excel2003格式。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static byte[] DataTableToExcelSheet(DataTable dt, string sheetName)
        {
            IWorkbook book = new HSSFWorkbook();//新建工作薄book
            if (dt.Rows.Count < MAXSHEETROW)//如果行数小于sheet的最大行数
                DataWriteToSheet(dt, 0, dt.Rows.Count - 1, book, sheetName);//写入sheet
            else//如果行数大于sheet的最大行数
            {
                int page = dt.Rows.Count / MAXSHEETROW;//分sheet 
                for (int i = 0; i < page; i++)
                {
                    int start = i * MAXSHEETROW;
                    int end = (i * MAXSHEETROW) + MAXSHEETROW - 1;
                    DataWriteToSheet(dt, start, end, book, sheetName + i.ToString());
                }
                int lastPageItemCount = dt.Rows.Count % MAXSHEETROW;
                DataWriteToSheet(dt, dt.Rows.Count - lastPageItemCount, lastPageItemCount, book, sheetName + page.ToString());
            }
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            return ms.ToArray();
        }
        private static void DataWriteToSheet(DataTable dt, int startRow, int endRow, IWorkbook book, string sheetName)
        {
            ISheet sheet = book.CreateSheet(sheetName);
            IRow header = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = header.CreateCell(i);
                string val = dt.Columns[i].Caption ?? dt.Columns[i].ColumnName;
                cell.SetCellValue(val);
            }
            int rowIndex = 1;
            for (int i = startRow; i <= endRow; i++)
            {
                DataRow dtRow = dt.Rows[i];
                IRow excelRow = sheet.CreateRow(rowIndex++);
                for (int j = 0; j < dtRow.ItemArray.Length; j++)
                {
                    excelRow.CreateCell(j).SetCellValue(dtRow[j].ToString());
                }
            }

        }

        #endregion
    }
}
