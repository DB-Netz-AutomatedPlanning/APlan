using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class XLSreader
    {
        /// <summary>
        /// read an .xls (1997-2003) file without opening the file using NOPI library.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetNumber"></param>
        public  ArrayList ReadXLSContent(string filePath, int sheetNumber)
        {
            ArrayList data = new ArrayList();
            ArrayList columnNames = new ArrayList();

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var workbook = new HSSFWorkbook(stream);
                var sheet = workbook.GetSheetAt(sheetNumber);
                var rows = sheet.GetRowEnumerator();
                while (rows.MoveNext())
                {
                    var row = (IRow)rows.Current;
                    if (row.RowNum == 0)
                    {
                        columnNames = ReadColumnNames(row); // get column names from the first row.
                    }
                    else
                    {
                        Dictionary<string, string> dictionary = new Dictionary<string, string>();
                        var cells = row.GetEnumerator();
                        int index = 0;
                        while (cells !=null && cells.MoveNext())
                        {
                            ICell? cell = cells.Current;
                            string? value = cell?.ToString();
                            if (cell!=null & value != null && !value.Equals("") && cells.Current?.ColumnIndex < columnNames.Count)
                            {
                                dictionary.Add(columnNames[cells.Current.ColumnIndex].ToString(), value.ToString());
                            }
                            index++;
                        }
                        if(dictionary.Count>0)
                        data.Add(dictionary);
                    }
                }
            }
            return data;
        }
        /// <summary>
        /// read the columns heading (first row in the sheet)
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public  ArrayList ReadColumnNames(IRow row)
        {
            ArrayList columnNames = new ArrayList();
            var cells = row.GetEnumerator();

            while (cells.MoveNext())
            {
                var cell = cells.Current;
                var value = cell.ToString();
                columnNames.Add(value);
            }
            return columnNames;
        }
    }

