using System;
using System.Collections.Generic;
using System.Web;
using OfficeOpenXml;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;
using InsuredTraveling.ViewModels;


namespace InsuredTraveling.FormBuilder
{
    public static class ExcelReader
    {
        public static int helperFunctions { get; set; }
        public static int procedures { get; set; }
        public static List<Dget> dgetFunctions { get; set; }     
        public static IHtmlString ReadExcel(ExcelFileViewModel e)
        {
            ExcelPackage pck = new ExcelPackage(new FileInfo(e.Path));
            dgetFunctions = new List<Dget>();
            var tagInfoExcel = ParseFormElements(pck);         
            var result = CreateForm(pck, tagInfoExcel);
            var functions = DetermineFunction(pck);
            var procedures = DetermineProcedure(pck);
            DatabaseCommands.CreateDatabaseTables(e.Id, tagInfoExcel, dgetFunctions, procedures);
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
                        Dget newDget = new Dget();
                        newDget.Resolver(formula, functionName, pck, worksheet);
                        dgetFunctions.Add(newDget);
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
            int procedureSequence = 0;
            string functionName = worksheet.Cells[row, procedures - 1].Value.ToString();
            for (row = 2; worksheet.Cells[row, procedures].Value != null; row++)
            {
                Function result = null;
                if (!worksheet.Cells[row, procedures].Value.ToString().Equals("empty"))
                {
                    var formula = worksheet.Cells[row, procedures].Formula;
                    procedureSequence++;
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
        public static HtmlString CreateForm(ExcelPackage pck, List<TagInfo> tagInfoExcel)
        {          
            var formBuilder = new FormBuilder()
               .SetName("my-form")
               .SetAction("/index")
               .SetMethod("post");
            foreach (var excelRow in tagInfoExcel)
            {
                var wrapper = TagFactory.GenerateWrappedTagFor(excelRow);
                formBuilder.AddElement(wrapper);
            }
            var result = formBuilder.ToHtmlString();

            return new HtmlString(result);
        }
        public static List<TagInfo> ParseFormElements(ExcelPackage pck)
        {
            ExcelWorksheet worksheet = pck.Workbook.Worksheets["ConfigurationSetup"];
            ExcelWorksheet worksheetListDetails = pck.Workbook.Worksheets["Lists"];
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
                if (!worksheet.Cells[col, 1].Value.ToString().Equals("empty"))
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
            return tagInfoExcel;
        }
      

    }
   
}
    