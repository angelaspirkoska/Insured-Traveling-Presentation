using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace InsuredTraveling.FormBuilder
{
    public static class ExcelReader
    {
        public static int helperFunctions { get; set; }
        public static int procedures { get; set; }
        public static IHtmlString ReadExcel(string path)
        {
            ExcelPackage pck = new ExcelPackage(new FileInfo(path));
            var result = CreateForm(pck);
            var functions = DetermineFunction(pck);
            var procedures = DetermineProcedure(pck);
            var functionsStrings = GenerateStringFunctions(functions);
            var proceduresStrings = GenerateStringProcedures(procedures);
            return result;
        }
        public static List<Function> DetermineFunction(ExcelPackage pck)
        {
            ExcelWorksheet worksheet = pck.Workbook.Worksheets["ConfigurationSetup"];
            int row = 1;
            List<Function> functions = new List<Function>();
           
            for (row = 2; worksheet.Cells[row, helperFunctions].Value != null; row++)
                {
               
                Function result = null;
                if (!worksheet.Cells[row, helperFunctions].Value.ToString().Equals("empty")){
                    var formula = worksheet.Cells[row, helperFunctions].Formula;
                    string functionName = worksheet.Cells[row, helperFunctions - 1].Value.ToString();
                    if (formula.ToUpper().StartsWith("DGET"))
                    {
                        result = new Dget();
                        result.Resolver(formula, functionName, pck, worksheet);             
                    }
                    else if (formula.ToUpper().StartsWith("IF"))
                    {
                        result = new IfCondition();
                        result.Resolver(formula, functionName, pck, worksheet);
                    }
                    else if (formula.ToUpper().StartsWith("ROUND"))
                    {
                        result = new Round();
                        result.Resolver(formula, functionName, pck, worksheet);
                    }
                    else if (formula.ToUpper().StartsWith("EXACT"))
                    {
                        result = new Exact();
                        result.Resolver(formula, functionName, pck, worksheet);
                    }
                    else
                    {
                        result = new MathOperation();
                        result.Resolver(formula, functionName, pck, worksheet);
                    }
                    functions.Add(result);
                }
               
            }
            return functions;
        }     
        public static List<Function> DetermineProcedure(ExcelPackage pck)
        {
            ExcelWorksheet worksheet = pck.Workbook.Worksheets["ConfigurationSetup"];
            List<Function> proceduresList = new List<Function>();
            int row = 1;
            string functionName = worksheet.Cells[row, procedures - 1].Value.ToString();
            for (row = 2; worksheet.Cells[row, procedures].Value != null; row++)
            {
                Function result = null;
                if (!worksheet.Cells[row, procedures].Value.ToString().Equals("empty"))
                {
                    var formula = worksheet.Cells[row, procedures].Formula;

                    if (formula.ToUpper().StartsWith("DGET"))
                    {
                        result = new Dget();
                        result.ResolverProcedure(formula, row, procedures, helperFunctions, pck, worksheet);
                    }
                    else if (formula.ToUpper().StartsWith("IF"))
                    {
                        result = new IfCondition();
                        result.ResolverProcedure(formula, row, procedures, helperFunctions, pck, worksheet);
                    }
                    else if (formula.ToUpper().StartsWith("EXACT"))
                    {
                        result = new Exact();
                        result.ResolverProcedure(formula, row, procedures, helperFunctions, pck, worksheet);
                    }
                    else if (formula.ToUpper().StartsWith("ROUND"))
                    {
                        result = new Round();
                        result.ResolverProcedure(formula, row, procedures, helperFunctions, pck, worksheet);
                    }
                    else
                    {
                        result = new MathOperation();
                        result.ResolverProcedure(formula, row, procedures, helperFunctions, pck, worksheet);
                    }
                    proceduresList.Add(result);
                }

            }
            return proceduresList;
        }
        public static List<string> GenerateStringFunctions(List<Function> functions)
        {
            List<string> generatedStringFunctions = new List<string>();
            foreach(Function function in functions)
            {
                generatedStringFunctions.Add(function.Name + function.ToString());
                // add to database!
            }
            return generatedStringFunctions;
        }
        public static List<string> GenerateStringProcedures(List<Function> procedures)
        {
            List<string> generatedStringProcedures = new List<string>();
            foreach (Function procedure in procedures)
            {
                generatedStringProcedures.Add(procedure.Name + procedure.ToString());
                // add to database!
            }
            return generatedStringProcedures;
        }
        public static HtmlString CreateForm(ExcelPackage pck)
        {
            ExcelWorksheet worksheet = pck.Workbook.Worksheets["ConfigurationSetup"];
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
            int variableName = -1;
            int helpingFunc = -1;

            for (int i = 1; worksheet.Cells[1, i].Value != null; i++)
            {
                var metaData = worksheet.Cells[1, i].Value.ToString();
                switch (metaData)
                {
                    case "FieldId": itemIdIndex = i; break;
                    case "NameCaption": nameCaptionIndex = i; break;
                    case "FieldType": fieldTypeIndex = i; break;
                    case "ListId": listIdIndex = i; break;
                    case "FieldList": fieldListIndex = i; break;
                    case "Default": defaultValueIndex = i; break;
                    case "Required": requiredIndex = i; break;
                    case "RatingIndicator": ratingIndicatorIndex = i; break;
                    case "FieldSize": fieldSizeIndex = i; break;
                    case "CSSClass": classesIndex = i; break;
                    case "CSSStyle": cssIndex = i; break;
                    case "VariableName": variableName = i; break;
                    case "HelpingFunc": helpingFunc = i; break;
                    case "FuncOutput": helperFunctions = i; break;
                    case "MidResult": procedures = i; break;
                    default: continue;
                }
            }
          
            for (int col = 2; worksheet.Cells[col, 1].Value != null; col++)
            {
                if (!worksheet.Cells[col,1].Value.ToString().Equals("empty"))
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


    public class Location
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public string WorksheetName { get; set; }

        public Location(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public Location()
        {
        }
        public static Location GetLocation(string location, ExcelWorksheet current)
        {
            Location result = new Location();
            string located = location;
            if (location.Contains("!"))
            {
                var sheetAndLocation = location.TrimStart().TrimEnd().Split('!');
                result.WorksheetName = sheetAndLocation[0];
                located = sheetAndLocation[1];
            }
            else
            {
                result.WorksheetName = current.Name;
            }
            //works only with two letters
            Regex regex = new Regex(@"(\D+)(\d+)");
            Match match = regex.Match(located);
            if (match.Success)
            {
                var row = match.Groups[1].Value.ToCharArray();
                int rowInt = 0;
                for (int i = 0; i < row.Length - 1; i++)
                {
                    var number = System.Convert.ToInt32(row[i]) - System.Convert.ToInt32('A') + 1;

                    rowInt += (row.Length - 1 - i) * 26 * number;
                }
                rowInt += System.Convert.ToInt32(row[row.Length - 1]) - System.Convert.ToInt32('A') + 1;

                int columnInt = Convert.ToInt32(match.Groups[2].Value);
                result.Column = columnInt;
                result.Row = rowInt;
                return result;
            }
            return null;
        }
    }
    public abstract class Function
        {
          public string Name { get; set; }
          public abstract void Resolver(string formula, string formulaName, ExcelPackage pck, ExcelWorksheet current);
          public abstract void ResolverProcedure(string formula, int sequence, int procedureRow, int functionRow, ExcelPackage pck, ExcelWorksheet current);
        }
    public class Dget : Function
    {
        public string PropertyValueName { get; set; }
        public string[,] ParametersNameAndInputValue { get; set; }
        public string[,] Database { get; set; }
        public int DatabaseRows { get; set; }
        public int DatabaseColumns { get; set; }
        public int ParametersNameAndInputValueRows { get; set; }
        public int ParametersNameAndInputValueColumns { get; set; }


        public void PopulateDatabase( )
        {
            //TODO 
        }

        public override void Resolver(string formula, string formulaName, ExcelPackage pck, ExcelWorksheet current)
        {
            Regex regex = new Regex(@".+\((.+)\)");
            Match match = regex.Match(formula);
            ExcelWorksheet dgetWorksheet = current;          
            Name = formulaName;

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
                    else value = parameters[1].Replace('"', ' ').TrimStart().TrimEnd();

                }
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                else
                {
                    PropertyValueName = value;
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

                valueStart = Location.GetLocation(indexes[0], dgetWorksheet);
                valueEnd = Location.GetLocation(indexes[1], dgetWorksheet);
                Database = new string[valueEnd.Row - valueStart.Row + 1, valueEnd.Column - valueStart.Column + 1];
                DatabaseRows = valueEnd.Row - valueStart.Row + 1;
                DatabaseColumns = valueEnd.Column - valueStart.Column + 1;
                int i = 0, j = 0;
                for (int row = valueStart.Row; row <= valueEnd.Row; row++, i++)
                {
                    for (int column = valueStart.Column; column <= valueEnd.Column; column++, j++)
                    {
                        Database[i, j] = dgetWorksheet.Cells[column, row].Value.ToString();
                    }
                    j = 0;
                }

                dgetWorksheet = current;
                Location parameterStart, parameterEnd;
                if (parameters[2].Contains("!"))
                {
                    sheetAndValueLocation = parameters[2].Split('!');
                    dgetWorksheet = pck.Workbook.Worksheets[sheetAndValueLocation[0].TrimEnd().TrimStart()];
                    indexes = sheetAndValueLocation[1].TrimEnd().TrimStart().Split(':');                   
                } 
                    indexes = parameters[2].TrimStart().TrimEnd().Split(':');
                    parameterStart = Location.GetLocation(indexes[0], dgetWorksheet);
                    parameterEnd = Location.GetLocation(indexes[1], dgetWorksheet);


                ParametersNameAndInputValue = new string[parameterEnd.Row - parameterStart.Row + 1, parameterEnd.Column - parameterStart.Column + 1];
                ParametersNameAndInputValueRows = parameterEnd.Row - parameterStart.Row + 1;
                ParametersNameAndInputValueColumns = parameterEnd.Column - parameterStart.Column + 1;

                i = j = 0;
                for (int row = parameterStart.Row; row <= parameterEnd.Row; row++, i++)
                {
                    for (int column = parameterStart.Column; column <= parameterEnd.Column; column++, j++)
                    {
                        if(dgetWorksheet.Cells[column, row].Value != null)
                        {
                            ParametersNameAndInputValue[i, j] = dgetWorksheet.Cells[column, row].Value.ToString();
                        }
                       
                    }
                    j = 0;
                }

            }
            
            
        }

        public override void ResolverProcedure(string formula, int sequence, int procedureRow, int functionRow, ExcelPackage pck, ExcelWorksheet current)
        {
            this.Resolver(formula, sequence.ToString(),  pck, current);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("DGET(");
              int i = 0;
              for(i = 0; i < DatabaseRows - 1; i++)
                {
                result.Append(Database[i, 0] + ";");
                }
            result.Append(Database[DatabaseRows - 1, 0] + ","+ PropertyValueName + ",");
            
            for (i = 0; i < ParametersNameAndInputValueRows - 1; i++)
            {
                    result.Append(ParametersNameAndInputValue[i, 0] + ";"); 
            }         
                result.Append(ParametersNameAndInputValue[ParametersNameAndInputValueRows - 1, 0] + ")");

            return result.ToString();
        }
    }
    public class MathOperation : Function
    {
        public string Operation { get; set; }
        public string OperandLeft { get; set; }
        public string OperandRight { get; set; }
        private void OperationResolver(string formula)
        {
            
            var minus = "-";
            var plus = "+";
            var multiplication = "*";
            var division = "/";
            var equal = "=";
            var bigger = ">";
            var smaller = "<";
            var biggerEqual = ">=";
            var smallerEqual = "<=";
            if (formula.Contains(minus))
            {
                Operation = minus;
            }
            else if (formula.Contains(plus))
            {
                Operation = plus;
            }
            else if (formula.Contains(equal))
            {
                Operation = equal;
            }
            else if (formula.Contains(multiplication))
            {
                Operation = multiplication;
            }
            else if (formula.Contains(division))
            {
                Operation = division;
            }
            else if (formula.Contains(bigger))
            {
                Operation = bigger;
            }
            else if (formula.Contains(biggerEqual))
            {
                Operation = biggerEqual;
            }
            else if (formula.Contains(smaller))
            {
                Operation = smaller;
            }
            else if (formula.Contains(smallerEqual))
            {
                Operation = smallerEqual;
            }
            else
            {
                Operation = "";
            }
        }
        public override void Resolver(string formula, string formulaName, ExcelPackage pck, ExcelWorksheet current)
        {
            Name = formulaName;
            OperationResolver(formula);
            if (Operation.Equals(String.Empty)){
                Regex reg = new Regex(@"\D\d");
                Match mat = reg.Match(formula);
                if (mat.Success)
                {
                    Operation = "=";
                    OperandRight = "0";
                    var location = Location.GetLocation(formula, current);
                    var worksheet = pck.Workbook.Worksheets[location.WorksheetName];
                    OperandLeft = worksheet.Cells[location.Column, location.Row - 1].Value.ToString();
                }
                return;
            }
            string[] operands;
            operands = formula.Split(new string[] { Operation }, StringSplitOptions.None);
                Regex regex = new Regex(@"\D\d");
                Match match = regex.Match(operands[0]);
                if (match.Success)
                {                   
                    var location = Location.GetLocation(operands[0], current);
                    var worksheet = pck.Workbook.Worksheets[location.WorksheetName];
                    OperandLeft = worksheet.Cells[location.Column, location.Row-1].Value.ToString();
                }
                else
                {
                    OperandLeft = operands[0];
                }
                match = regex.Match(operands[1]);
                if (match.Success)
                {
                    var location = Location.GetLocation(operands[1], current);
                    var worksheet = pck.Workbook.Worksheets[location.WorksheetName];
                    OperandRight = worksheet.Cells[location.Column - 1, location.Row].Value.ToString();
                }
                else
                {
                    OperandRight = operands[1];
                }
            
        }
        public override void ResolverProcedure(string formula, int sequence, int procedureRow, int functionRow, ExcelPackage pck, ExcelWorksheet current)
        {
            Name = sequence.ToString();
            OperationResolver(formula);
            if (Operation.Equals(String.Empty))
            {
                Regex reg = new Regex(@"\D\d");
                Match mat = reg.Match(formula);
                if (mat.Success)
                {
                    Operation = "=";
                    OperandRight = "0";
                    var location = Location.GetLocation(formula, current);
                    var worksheet = pck.Workbook.Worksheets[location.WorksheetName];
                    OperandLeft = worksheet.Cells[location.Column, location.Row - 1].Value.ToString();
                }
                return;
            }

            string[] operands;
            operands = formula.Split(new string[] { Operation }, StringSplitOptions.None);
            Regex regex = new Regex(@"\D\d");
            Match match = regex.Match(operands[0]);
            if (match.Success && !operands[0].Contains("."))
            {
                var location = Location.GetLocation(operands[0], current);
                if(location.Row == procedureRow)
                {
                    OperandLeft = "Procedure" + location.Column;
                }
                else
                {
                    OperandLeft = current.Cells[location.Column, location.Row - 1].Value.ToString();
                }
            }
            else
            {
                OperandLeft = operands[0].TrimStart().TrimEnd();
            }

            match = regex.Match(operands[1]);
            if (match.Success && !operands[1].Contains("."))
            {
                var location = Location.GetLocation(operands[1], current);
                if (location.Row == procedureRow)
                {
                    OperandRight = "Procedure" + location.Column;
                }
                else
                {
                    OperandRight = current.Cells[location.Column, location.Row - 1].Value.ToString();
                }
            }
            else
            {
                OperandRight = operands[1].TrimStart().TrimEnd();
            }


        }
        public override string ToString()
        {
            return "(" + OperandLeft +" "+ Operation + " " + OperandRight + ")";
        }
    }
    public class IfCondition : Function
    {
        public MathOperation Condition { get; set; }
        public string IfTrue { get; set; }
        public string IfFalse { get; set; }
        public override void Resolver(string formula, string formulaName, ExcelPackage pck, ExcelWorksheet current)
        {
            formula = formula.Replace("IF", "").Replace("(", "").Replace(")","");
            var formulaSplitted = formula.Split(',');
            Condition = new MathOperation();
            Condition.Resolver(formulaSplitted[0].TrimEnd().TrimStart(), formulaName, pck, current);
            Name = Condition.Name;
            Regex regex = new Regex(@"\D\d");
            Match match = regex.Match(formulaSplitted[1].TrimEnd().TrimStart());
            if (match.Success && !formulaSplitted[1].Contains('.'))
            {
                var location = Location.GetLocation(formulaSplitted[1].TrimEnd().TrimStart(), current);
                var worksheet = pck.Workbook.Worksheets[location.WorksheetName];
                IfTrue = worksheet.Cells[location.Column, location.Row - 1].Value.ToString();
            }
            else
            {
                IfTrue = formulaSplitted[1].TrimEnd().TrimStart();
            }

            match = regex.Match(formulaSplitted[2].TrimEnd().TrimStart());
            if (match.Success && !formulaSplitted[2].Contains('.'))
            {
                var location = Location.GetLocation(formulaSplitted[2].TrimEnd().TrimStart(), current);
               
                var worksheet = pck.Workbook.Worksheets[location.WorksheetName];
                IfFalse = worksheet.Cells[location.Column, location.Row - 1].Value.ToString();
            }
            else
            {
                IfFalse = formulaSplitted[2].TrimEnd().TrimStart();
            }
        }
        public override void ResolverProcedure(string formula, int sequence, int procedureRow, int functionRow, ExcelPackage pck, ExcelWorksheet current)
        {
            Name = sequence.ToString();
            formula = formula.Replace("IF", "").Replace("(", "").Replace(")", "");
            var formulaSplitted = formula.Split(',');
            Condition = new MathOperation();
            Condition.ResolverProcedure(formulaSplitted[0].TrimEnd().TrimStart(), sequence, procedureRow, functionRow, pck, current);

            Regex regex = new Regex(@"\D\d");
            Match match = regex.Match(formulaSplitted[1].TrimEnd().TrimStart());
            if (match.Success && !formulaSplitted[1].Contains('.'))
            {
                var location = Location.GetLocation(formulaSplitted[1].TrimEnd().TrimStart(), current);
                //tujj novo
                if (location.Row == procedureRow)
                {
                    IfTrue = "Procedure" + location.Column;
                }
                else
                {
                    IfTrue = current.Cells[location.Column, location.Row - 1].Value.ToString();
                }              
            }
            else
            {
                IfTrue = formulaSplitted[1].TrimEnd().TrimStart();
            }

            match = regex.Match(formulaSplitted[2].TrimEnd().TrimStart());
            if (match.Success)
            {
                var location = Location.GetLocation(formulaSplitted[2].TrimEnd().TrimStart(), current);
                //tujj novo
                if (location.Row == procedureRow)
                {
                    IfFalse = "Procedure" + location.Column;
                }
                else
                {
                    IfFalse = current.Cells[location.Column, location.Row - 1].Value.ToString();
                }
            }
            else
            {
                IfFalse = formulaSplitted[2].TrimEnd().TrimStart();
            }

        }
        public override string ToString()
        {
            return "(IF "+ Condition.ToString() + " ," + IfTrue + "," +IfFalse + ")";
        }
    }
    public class Round : Function
    {
        public string Number { get; set; }
        public string RoundTo { get; set; }
        public override void Resolver(string formula, string formulaName, ExcelPackage pck, ExcelWorksheet current)
        {
            Name = formulaName;
            formula = formula.Replace("ROUND", "").Replace("(", "").Replace(")", "");
            if (formula.Contains("UP"))
            {
                RoundTo = "UP";
                formula.Replace("UP", "");
            }
            else if (formula.Contains("DOWN"))
            {
                RoundTo = "DOWN";
                formula.Replace("DOWN", "");
            }
            var formulaSplitted = formula.Split(',');
            Regex regex = new Regex(@"\D\d");
            Match match = regex.Match(formulaSplitted[0].TrimEnd().TrimStart());
            if (match.Success && !formulaSplitted[0].Contains('.'))
            {
                var location = Location.GetLocation(formulaSplitted[0].TrimEnd().TrimStart(), current);
                var worksheet = pck.Workbook.Worksheets[location.WorksheetName];
                Number = worksheet.Cells[location.Column, location.Row - 1].Value.ToString();
            }
            else
            {
                Number = formulaSplitted[0].TrimEnd().TrimStart();
            }

            if (RoundTo == "")
            {
                match = regex.Match(formulaSplitted[1].TrimEnd().TrimStart());
                if (match.Success && !formulaSplitted[1].Contains('.'))
                {
                    var location = Location.GetLocation(formulaSplitted[1].TrimEnd().TrimStart(), current);
                    var worksheet = pck.Workbook.Worksheets[location.WorksheetName];
                    RoundTo = worksheet.Cells[location.Column, location.Row - 1].Value.ToString();
                }
                else
                {
                    RoundTo = formulaSplitted[1].TrimEnd().TrimStart();
                }
            }
        }
        public override void ResolverProcedure(string formula, int sequence, int procedureRow, int functionRow, ExcelPackage pck, ExcelWorksheet current)
        {
            Name = sequence.ToString();
            formula = formula.Replace("ROUND", "").Replace("(", "").Replace(")", "");
            if (formula.Contains("UP"))
            {
                RoundTo = "UP";
                formula.Replace("UP", "");
            } 
            else if (formula.Contains("DOWN"))
            {
                RoundTo = "DOWN";
                formula.Replace("DOWN", "");
            }
            var formulaSplitted = formula.Split(',');
            Regex regex = new Regex(@"\D\d");
            Match match = regex.Match(formulaSplitted[0].TrimEnd().TrimStart());
            if (match.Success && !formulaSplitted[0].Contains('.'))
            {
                var location = Location.GetLocation(formulaSplitted[0].TrimEnd().TrimStart(), current);
                if (location.Row == procedureRow)
                {
                    Number = "Procedure" + location.Column;
                }
                else
                {
                    Number = current.Cells[location.Column, location.Row - 1].Value.ToString();
                }     
            }
            else
            {
                Number = formulaSplitted[0].TrimEnd().TrimStart();
            }

            if (String.IsNullOrEmpty(RoundTo))
            {      
            match = regex.Match(formulaSplitted[1].TrimEnd().TrimStart());
            if (match.Success && !formulaSplitted[1].Contains('.'))
            {
                var location = Location.GetLocation(formulaSplitted[1].TrimEnd().TrimStart(), current);
                if (location.Row == procedureRow)
                {
                    RoundTo = "Procedure" + location.Column;
                }
                else
                {
                    RoundTo = current.Cells[location.Column, location.Row - 1].Value.ToString();
                }
            }
            else
            {
                RoundTo = formulaSplitted[1].TrimEnd().TrimStart();
            }
            }
        }
        public override string ToString()
        {
            if(RoundTo.Contains("UP") || RoundTo.Contains("DOWN"))
            {
                return "ROUND" + RoundTo + "("+Number+")";
            }
            return "ROUND" + "(" + Number + "," + RoundTo + ")";
        }
    }
    public class Exact : Function
    {
        public string LeftOperand { get; set; }
        public string RightOperand { get; set; }
        public override void Resolver(string formula, string formulaName, ExcelPackage pck, ExcelWorksheet current)
        {
            Name = formulaName;
            formula = formula.Replace("EXACT", "").Replace("(", "").Replace(")", "");      
            var formulaSplitted = formula.Split(',');
            Regex regex = new Regex(@"\D\d");
            Match match = regex.Match(formulaSplitted[0].TrimEnd().TrimStart());
            if (match.Success && !formulaSplitted[0].Contains('.'))
            {
                var location = Location.GetLocation(formulaSplitted[0].TrimEnd().TrimStart(), current);
                var worksheet = pck.Workbook.Worksheets[location.WorksheetName];
                LeftOperand = worksheet.Cells[location.Column, location.Row - 1].Value.ToString();
            }
            else
            {
                LeftOperand = formulaSplitted[0].TrimEnd().TrimStart();
            }

            match = regex.Match(formulaSplitted[1].TrimEnd().TrimStart());
            if (match.Success && !formulaSplitted[1].Contains('.'))
            {
                var location = Location.GetLocation(formulaSplitted[1].TrimEnd().TrimStart(), current);
                var worksheet = pck.Workbook.Worksheets[location.WorksheetName];
                RightOperand = worksheet.Cells[location.Column, location.Row - 1].Value.ToString();
            }
            else
            {
                RightOperand = formulaSplitted[1].TrimEnd().TrimStart();
            }
        }
        public override void ResolverProcedure(string formula, int sequence, int procedureRow, int functionRow, ExcelPackage pck, ExcelWorksheet current)
        {
            Name = sequence.ToString();
            formula = formula.Replace("EXACT", "").Replace("(", "").Replace(")", "");
            var formulaSplitted = formula.Split(',');
            Regex regex = new Regex(@"\D\d");
            Match match = regex.Match(formulaSplitted[0].TrimEnd().TrimStart());
            if (match.Success && !formulaSplitted[0].Contains('.'))
            {
                var location = Location.GetLocation(formulaSplitted[0].TrimEnd().TrimStart(), current);
                if (location.Row == procedureRow)
                {
                    LeftOperand = "Procedure" + location.Column;
                }
                else
                {
                    LeftOperand = current.Cells[location.Column, location.Row - 1].Value.ToString();
                }
            }
            else
            {
                LeftOperand = formulaSplitted[0].TrimEnd().TrimStart();
            }


                match = regex.Match(formulaSplitted[1].TrimEnd().TrimStart());
                if (match.Success && !formulaSplitted[1].Contains('.'))
                {
                    var location = Location.GetLocation(formulaSplitted[1].TrimEnd().TrimStart(), current);
                    if (location.Row == procedureRow)
                    {
                        RightOperand = "Procedure" + location.Column;
                    }
                    else
                    {
                        RightOperand = current.Cells[location.Column, location.Row - 1].Value.ToString();
                    }
                }
                else
                {
                    RightOperand = formulaSplitted[1].TrimEnd().TrimStart();
                }          
        }
        public override string ToString()
        {
            return "(" + LeftOperand + " EXACT " + RightOperand + ")";
        }
    }
}
    