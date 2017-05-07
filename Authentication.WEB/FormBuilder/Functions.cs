using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace InsuredTraveling.FormBuilder
{
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
        public void PopulateDatabase()
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
                        if (dgetWorksheet.Cells[column, row].Value != null)
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
            this.Resolver(formula, sequence.ToString(), pck, current);
        }
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("DGET(");
            int i = 0;
            for (i = 0; i < DatabaseRows - 1; i++)
            {
                result.Append(Database[i, 0] + ";");
            }
            result.Append(Database[DatabaseRows - 1, 0] + "," + PropertyValueName + ",");
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
                    OperandLeft = worksheet.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
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
                OperandLeft = worksheet.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
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
                OperandRight = worksheet.Cells[location.Column - 1, location.Row].Value.ToString().Replace(' ', '_');
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
                    OperandLeft = worksheet.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
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
                if (location.Row == procedureRow)
                {
                    OperandLeft = "Procedure" + location.Column;
                }
                else
                {
                    OperandLeft = current.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
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
                    OperandRight = current.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
                }
            }
            else
            {
                OperandRight = operands[1].TrimStart().TrimEnd();
            }
        }
        public override string ToString()
        {
            return "(" + OperandLeft + " " + Operation + " " + OperandRight + ")";
        }
    }
    public class IfCondition : Function
    {
        public MathOperation Condition { get; set; }
        public string IfTrue { get; set; }
        public string IfFalse { get; set; }
        public override void Resolver(string formula, string formulaName, ExcelPackage pck, ExcelWorksheet current)
        {
            formula = formula.Replace("IF", "").Replace("(", "").Replace(")", "");
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
                IfTrue = worksheet.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
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
                IfFalse = worksheet.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
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
                    IfTrue = current.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
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
                    IfFalse = current.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
                }
            }
            else
            {
                IfFalse = formulaSplitted[2].TrimEnd().TrimStart();
            }

        }
        public override string ToString()
        {
            return "(IF " + Condition.ToString() + " ," + IfTrue + "," + IfFalse + ")";
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
                Number = worksheet.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
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
                    var worksheet = pck.Workbook.Worksheets[location.WorksheetName];
                    RoundTo = worksheet.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
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
                    Number = current.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
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
                        RoundTo = current.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
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
            if (RoundTo.Contains("UP") || RoundTo.Contains("DOWN"))
            {
                return "ROUND" + RoundTo + "(" + Number + ")";
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
                LeftOperand = worksheet.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
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
                RightOperand = worksheet.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
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
                    LeftOperand = current.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
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
                    RightOperand = current.Cells[location.Column, location.Row - 1].Value.ToString().Replace(' ', '_');
                }
            }
            else
            {
                RightOperand = formulaSplitted[1].TrimEnd().TrimStart();
            }
        }
        public override string ToString()
        {
            return "EXACT(" + LeftOperand + "," + RightOperand + ")";
        }
    }
}