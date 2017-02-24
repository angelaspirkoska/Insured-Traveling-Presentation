using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using System.IO;
using System.Reflection;
using InsuredTraveling.FormBuilder;

namespace InsuredTraveling.ExcelReader
{
    public static class ExcelReader
    {
        public static IHtmlString ReadExcel(string path)
        {
         
            ExcelPackage pck = new ExcelPackage(new FileInfo(path));
            ExcelWorksheet worksheet = pck.Workbook.Worksheets["Item_Details"];
            var builder = HeaderTag.Header("bla", null);
            // int col = 1;

            for (int col = 1; worksheet.Cells[col, 1].Value != null ; col++)
            {
                var value = worksheet.Cells[col, 5].Value;
                if (value != null && value.ToString().Equals("textarea"))
                {
                    builder += FormBuilder.TextAreaTag.TextArea(worksheet.Cells[col, 1].Value.ToString(), worksheet.Cells[col, 1].Value.ToString(), worksheet.Cells[col, 1].Value.ToString());
                }
                //for (int row = 1; row < 5; row++)
                //{
                   
                //}
            }
            var result = new HtmlString(builder);
            return result;

        }
    }
}