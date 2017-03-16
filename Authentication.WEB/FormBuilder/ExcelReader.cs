using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using System.IO;
using System.Reflection;
using InsuredTraveling.FormBuilder;
using System.Text.RegularExpressions;

namespace InsuredTraveling.FormBuilder
{
    public static class ExcelReader
    {
        public static IHtmlString ReadExcel(string path)
        {

            ExcelPackage pck = new ExcelPackage(new FileInfo(path));

            DetermineFunction(pck);
            var result = CreateForm(pck);
            return result;
        }
        public static void DetermineFunction(ExcelPackage pck)
        {
            ExcelWorksheet worksheet = pck.Workbook.Worksheets["Dget"];

            int row = 1;
            for (int col = 1; worksheet.Cells[col, row].Value != null; col++)
            {
                for (row = 1; worksheet.Cells[col, row].Value != null; row++)
                {
                    var formula = worksheet.Cells[col, row].Formula;
                    if (formula.ToUpper().StartsWith("DGET"))
                    {
                        var result = Dget(formula, pck, worksheet);
                    }
                 }
                row = 1;
            }
               
          }
      

        public static Dget Dget(string formula, ExcelPackage pck, ExcelWorksheet current)
        {
            Regex regex = new Regex(@".+\((.+)\)");
            Match match = regex.Match(formula);
            ExcelWorksheet dgetWorksheet = current;
            Dget result = new Dget();

            if (match.Success)
            {
                var parameters = match.Groups[1].Value.Split(',');
                string[] sheetAndValueLocation;
                string value;

                if (parameters[1].Contains("!"))
                {
                     sheetAndValueLocation = parameters[1].Split('!');
                    dgetWorksheet = pck.Workbook.Worksheets[sheetAndValueLocation[0].TrimEnd().TrimStart()];
                    value = dgetWorksheet.Cells[sheetAndValueLocation[1].TrimStart().TrimEnd()].Value.ToString();

                }
                else
                {
                    if (parameters[1].Contains(":"))
                    {
                        value = dgetWorksheet.Cells[parameters[1]].Value.ToString().TrimStart().TrimEnd();
                    }
                    else value = parameters[1].Replace('"',' ').TrimStart().TrimEnd();
                    
                }
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                else
                {
                    result.PropertyValueName = value;
                }

                dgetWorksheet = current;
                string index = parameters[0];
                Location valueStart, valueEnd;
                if (parameters[0].Contains("!"))
                {                   
                    sheetAndValueLocation = parameters[0].Split('!');
                    index = sheetAndValueLocation[1].TrimEnd().TrimStart();
                    dgetWorksheet = pck.Workbook.Worksheets[sheetAndValueLocation[0].TrimEnd().TrimStart()];
                }
               
                    var indexes = index.Split(':');
                
                    valueStart = GetLocation(indexes[0]);
                    valueEnd = GetLocation(indexes[1]);
                result.Database = new string [valueEnd.Column, valueEnd.Row];
               
                for (int column = valueStart.Column; column <= valueEnd.Column; column++)
                {
                    for(int row = valueStart.Row; row <= valueEnd.Row; row++)
                    {
                       
                        result.Database[column-1, row - 1] = dgetWorksheet.Cells[column, row].Value.ToString();
                    }
                }

                dgetWorksheet = current;
                Location parameterStart, parameterEnd;
                if (parameters[2].Contains("!"))
                {
                    sheetAndValueLocation = parameters[2].Split('!');
                    dgetWorksheet = pck.Workbook.Worksheets[sheetAndValueLocation[0].TrimEnd().TrimStart()];
                    indexes = dgetWorksheet.Cells[sheetAndValueLocation[1].TrimStart().TrimEnd()].Value.ToString().Split(':');
                    parameterStart = GetLocation(indexes[0]);
                    parameterEnd =  GetLocation(indexes[1]);
                }
                else
                {
                    indexes = parameters[2].TrimStart().TrimEnd().Split(':');
                    parameterStart = GetLocation(indexes[0]);
                    parameterEnd = GetLocation(indexes[1]);
                }

                result.ParametersNameAndInputValue = new string[parameterEnd.Column, parameterEnd.Row];

                for (int column = parameterStart.Column; column <= parameterEnd.Column; column++)
                {
                    for (int row = parameterStart.Row; row <= parameterEnd.Row; row++)
                    {

                        result.ParametersNameAndInputValue[column - 1, row - 1] = dgetWorksheet.Cells[column, row].Value.ToString();
                    }
                }

                // nested for

            }
            return result;
        }
        
        private static Location GetLocation(string location)
        {
            //works only with two letters
            Regex regex = new Regex(@"(\D+)(\d+)");
            Match match = regex.Match(location);
            if (match.Success)
            {
                var row = match.Groups[1].Value.ToCharArray();
                int rowInt = 0;
                for(int i = 0; i < row.Length-1; i++)
                {
                    var number = System.Convert.ToInt32(row[i]) - System.Convert.ToInt32('A') + 1;
                   
                    rowInt += (row.Length -1 - i) * 26 * number;
                }
                rowInt += System.Convert.ToInt32(row[row.Length-1]) - System.Convert.ToInt32('A') + 1;
               
                int columnInt = Convert.ToInt32(match.Groups[2].Value);
                return new Location(columnInt, rowInt);
            }
            return null;
        }

        public static HtmlString CreateForm(ExcelPackage pck)
        {
            ExcelWorksheet worksheet = pck.Workbook.Worksheets["Item_Details"];
            ExcelWorksheet worksheetListDetails = pck.Workbook.Worksheets["Lists"];
            var formBuilder = new FormBuilder()
               .SetName("my-form")
               .SetAction("/index")
               .SetMethod("post");

            List<TagInfo> tagInfoExcel = new List<TagInfo>();
            Dictionary<string, string> attributes;
            List<string> listValues;
            int itemIdIndex = -1;
            int nameCaptionIndex = -1;
            int fieldTypeIndex = -1;
            int listIdIndex = -1;
            int fieldListIndex = -1;
            int defaultValueIndex = -1;
            int requiredIndex = -1;
            int ratingIndicatorIndex = -1;
            int fieldSizeIndex = -1;
            int classesIndex = -1;
            int cssIndex = -1;

            for (int i = 1; worksheet.Cells[1, i].Value != null; i++)
            {
                var metaData = worksheet.Cells[1, i].Value.ToString();
                switch (metaData)
                {
                    case "Field_ID": itemIdIndex = i; break;
                    case "Name_Caption": nameCaptionIndex = i; break;
                    case "Field_type": fieldTypeIndex = i; break;
                    case "list_id": listIdIndex = i; break;
                    case "Field_list": fieldListIndex = i; break;
                    case "Default": defaultValueIndex = i; break;
                    case "Required": requiredIndex = i; break;
                    case "Rating_indicator": ratingIndicatorIndex = i; break;
                    case "Field_size": fieldSizeIndex = i; break;
                    case "Classes": classesIndex = i; break;
                    case "Css": cssIndex = i; break;
                    default: return null;
                }
            }

            for (int col = 2; worksheet.Cells[col, 1].Value != null; col++)
            {
                attributes = new Dictionary<string, string>();
                listValues = new List<string>();

                if (worksheet.Cells[col, nameCaptionIndex].Value != null)
                {
                    attributes.Add("name", worksheet.Cells[col, nameCaptionIndex].Value.ToString());
                }

                if (worksheet.Cells[col, requiredIndex].Value.ToString() == "1")
                {
                    attributes.Add("required", "true");
                }

                if (worksheet.Cells[col, fieldSizeIndex].Value != null)
                {
                    attributes.Add("field_size", worksheet.Cells[col, fieldSizeIndex].Value.ToString());
                }

                if (worksheet.Cells[col, classesIndex].Value != null)
                {
                    attributes.Add("class", worksheet.Cells[col, classesIndex].Value.ToString());
                }
                if (worksheet.Cells[col, cssIndex].Value != null)
                {
                    attributes.Add("css", worksheet.Cells[col, cssIndex].Value.ToString());
                }

                if (worksheet.Cells[col, defaultValueIndex].Value.ToString() != "n/a")
                {
                    attributes.Add("default", worksheet.Cells[col, defaultValueIndex].Value.ToString());
                }

                var listId = worksheet.Cells[col, listIdIndex].Value;
                var type = worksheet.Cells[col, fieldTypeIndex].Value.ToString();

                if (listId != null && !listId.ToString().Equals("0") && type.Equals("dropdown"))
                {
                    for (int column = 2; worksheetListDetails.Cells[column, 1].Value != null; column++)
                    {
                        var listIdDetails = worksheetListDetails.Cells[column, 1].Value;
                        if (listIdDetails != null && listIdDetails.Equals(listId))
                        {
                            var listValue = worksheetListDetails.Cells[column, 3].Value.ToString();
                            listValues.Add(listValue);
                        }
                    }
                }

                TagInfo tagInfo =
                    new TagInfo
                    {
                        Id = worksheet.Cells[col, itemIdIndex].Value.ToString(),
                        Name = worksheet.Cells[col, nameCaptionIndex].Value.ToString(),
                        Type = worksheet.Cells[col, fieldTypeIndex].Value.ToString(),
                        Attributes = attributes,
                        ListItems = listValues,
                    };

                tagInfoExcel.Add(tagInfo);
            }

            foreach (var excelRow in tagInfoExcel)
            {
                var wrapper = TagFactory.GenerateWrappedTagFor(excelRow);
                formBuilder.AddElement(wrapper);
            }
            var result = formBuilder.ToHtmlString();

            return new HtmlString(result);
        }
    }

    public class Dget
    {
        public string PropertyValueName { get; set; }
        public string[,] ParametersNameAndInputValue { get; set; }
        public string[,] Database { get; set; }

        public void PopulateDatabase( )
        {

        }
    }

    public class Location
    {
        public int Column { get; set; }
        public int Row { get; set; }

        public Location(int column, int row)
        {
            Column = column;
            Row = row;
        }
    }
}
    