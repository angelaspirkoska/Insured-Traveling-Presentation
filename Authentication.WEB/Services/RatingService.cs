using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Linq;

namespace Authentication.WEB.Services
{
    class RatingService
    {
        public int calculatePremium(string filePath, int value1, int value2)
        {
            var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheets.Worksheet(1);
            worksheet.Cell("B1").SetValue(value1);
            worksheet.Cell("B2").SetValue(value2);
            workbook.Save();

           /// I'm doing this to recalculate the formulas in the file
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook wb = excelApp.Workbooks.Open(System.IO.Path.GetFullPath(filePath));
            wb.Close(true);
            excelApp.Quit();

            int result = 0;
            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(filePath, true))
            {
                WorksheetPart worksheetPart = GetWorksheetPartByName(spreadSheet, "Main");
                Cell cell = GetCell(worksheetPart.Worksheet, "B", 3);
                string r = cell.Descendants<CellValue>().FirstOrDefault().Text;
                result = int.Parse(r);
            }

            return result;
        }

        private static WorksheetPart GetWorksheetPartByName(SpreadsheetDocument document, string sheetName)
        {
            IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>().
               Elements<Sheet>().Where(s => s.Name == sheetName);

            if (sheets.Count() == 0)
            {
               // The specified worksheet does not exist.
                   return null;
            }

            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);
            return worksheetPart;
        }

        private static Cell GetCell(Worksheet worksheet, string columnName, uint rowIndex)
        {
            Row row = GetRow(worksheet, rowIndex);

            if (row == null)
                return null;

            return row.Elements<Cell>().Where(c => string.Compare(c.CellReference.Value, columnName +
                   rowIndex, true) == 0).First();
        }

        private static Row GetRow(Worksheet worksheet, uint rowIndex)
        {
            return worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }

    }
}
