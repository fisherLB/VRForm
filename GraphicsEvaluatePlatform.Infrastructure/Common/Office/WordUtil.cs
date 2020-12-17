using System;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using System.IO;
using System.Data;

namespace GraphicsEvaluatePlatform.Infrastructure.Common.Office
{
    public class WordUtil
    {
        public static byte[] GetByteForDataSet(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            //创建document对象  
            XWPFDocument doc = new XWPFDocument();
            //创建段落对象  
            XWPFParagraph p1 = doc.CreateParagraph();
            p1.Alignment = ParagraphAlignment.CENTER;
            XWPFRun runTitle = p1.CreateRun();
            //runTitle.SetBold(true);
            runTitle.IsBold = true;
            runTitle.SetText(DateTime.Now.ToString());
            runTitle.FontSize = 16;
            runTitle.FontFamily = "宋体";//设置雅黑字体  

            XWPFParagraph p2 = doc.CreateParagraph();
            XWPFRun run1 = p2.CreateRun();
            run1.SetText(" 编号：");
            run1.FontSize = 12;
            run1.FontFamily = "华文楷体";// FontCharRange.None);//设置雅黑字体  

            XWPFTable tableTop = doc.CreateTable(dt.Rows.Count, dt.Columns.Count);
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                for (int j = 1; j < dt.Columns.Count; j++)
                {
                    string value = dt.Rows[i][j].ToString();

                    tableTop.GetRow(i).GetCell(j).SetParagraph(SetCellText(doc, tableTop, value));
                }

            }
            return ToByte(doc);
        }
        private static byte[] ToByte(XWPFDocument word)
        {
            MemoryStream ms = new MemoryStream();

            //XSSFWorkbook即读取.xlsx文件返回的MemoryStream是关闭
            //但是可以ToArray(),这是NPOI的bug
            word.Write(ms);

            ms.Seek(0, SeekOrigin.Begin);
            return ms.ToArray();

        }
        private static XWPFParagraph SetCellText(XWPFDocument doc, XWPFTable table, string setText)
        {
            //table中的文字格式设置  
            CT_P para = new CT_P();
            XWPFParagraph pCell = new XWPFParagraph(para, table.Body);
            pCell.Alignment = ParagraphAlignment.CENTER;//字体居中  
            pCell.VerticalAlignment = TextAlignment.CENTER;//字体居中  

            XWPFRun r1c1 = pCell.CreateRun();
            r1c1.SetText(setText);
            r1c1.FontSize = 12;
            // r1c1.SetFontFamily("华文楷体", FontCharRange.None);//设置雅黑字体  
            //r1c1.SetTextPosition(20);//设置高度  
            return pCell;
        }
    }
}
