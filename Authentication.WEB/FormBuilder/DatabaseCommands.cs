using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.Mvc;


namespace InsuredTraveling.FormBuilder
{
    public static class DatabaseCommands
    {
        public static string GeneratePolicyCommand(int excelID, List<TagInfo> tagInfoExcel)
        {
            StringBuilder newPolicyTable = new StringBuilder();
            newPolicyTable.Append("CREATE TABLE Policy_" + excelID + " ( ID INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY");
            foreach (TagInfo tag in tagInfoExcel)
            {
                if (tag != null && !String.IsNullOrEmpty(tag.Name))
                {
                    string type = "";
                    if (tag.Type.Equals("textbox") || tag.Type.Equals("alphanumericspacetextbox") || tag.Type.Equals("alphanumerictextbox") || tag.Type.Equals("password"))
                    {
                        type = " VARCHAR(20) ";
                    }
                    else if (tag.Type.Equals("email"))
                    {
                        type = " VARCHAR(50) ";
                    }
                    else if (tag.Type.Equals("textarea"))
                    {
                        type = " VARCHAR(1000) ";
                    }
                    else if (tag.Type.Equals("file"))
                    {
                        type = " VARCHAR(30) ";
                    }
                    else if (tag.Type.Equals("number"))
                    {
                        type = " INT(10) ";
                    }
                    else if (tag.Type.Equals("radio") || tag.Type.Equals("checkbox"))
                    {
                        type = " BOOLEAN DEFAULT NULL ";
                    }
                    else if (tag.Type.Equals("time"))
                    {
                        type = " TIMESTAMP ";
                    }
                    else if (tag.Type.Equals("date"))
                    {
                        type = " DATE ";
                    }
                    if (type != "")
                    {
                        newPolicyTable.Append(", " + tag.Name.Replace(' ', '_') + " " + type + " ");
                        string requiredResult;
                        var required = tag.Attributes.TryGetValue("required", out requiredResult);
                        if ((required && !requiredResult.Equals("")) && !type.Contains("tiny"))
                        {
                            newPolicyTable.Append("NOT NULL");
                        }
                    }

                }
            }
            newPolicyTable.Append(")");
            return newPolicyTable.ToString();
        }
        public static string GenerateListCommand(int excelID)
        {
            return "CREATE TABLE Lists_" + excelID + " ( ID INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY, List_ID INT NOT NULL, List_Name VARCHAR(20) NOT NULL, Parameter_Value VARCHAR(50) NOT NULL)";
        }
        public static string PopulateListsCommand(int excelID, List<TagInfo> tagInfoExcel)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.Append("INSERT INTO Lists_" + excelID + " (List_ID ,List_Name,Parameter_Value) VALUES ");
            foreach (TagInfo tag in tagInfoExcel)
            {
                if (tag.Type.Equals("dropdown"))
                {
                    if (tag.ListItems.Count != 0)
                    {
                        for (int i = 0; i < tag.ListItems.Count; i++)
                        {
                            commandText.Append("('" + tag.Id + "','" + tag.Name + "', '" + tag.ListItems[i] + "'),");
                        }
                    }
                }
            }
            commandText.Length--;
            return commandText.ToString();
        }
        public static List<string> GenerateDGETCommands(int excelID, List<Dget> dgetFunctions)
        {
            List<string> commands = new List<string>();
            foreach (Dget function in dgetFunctions)
            {
                StringBuilder command = new StringBuilder();
                StringBuilder insertInto = new StringBuilder();
                command.Append("CREATE TABLE " + function.Name + "_" + excelID + " (");

                for (int i = 0; i < function.DatabaseRows; i++)
                {
                    command.Append(" " + function.Database[i, 0].Replace(' ', '_') + " VARCHAR(50),");
                    insertInto.Append(function.Database[i, 0].Replace(' ', '_') + ",");
                }
                command.Length--;
                insertInto.Length--;
                command.Append(") ");
                commands.Add(command.ToString());

                command = new StringBuilder();
                command.Append("INSERT INTO " + function.Name + "_" + excelID + " (" + insertInto.ToString() + ") VALUES ");
                for (int i = 1; i < function.DatabaseColumns; i++)
                {
                    command.Append("(");
                    for (int j = 0; j < function.DatabaseRows; j++)
                    {
                        command.Append("'" + function.Database[j, i] + "',");
                    }
                    command.Length--;
                    command.Append("), ");
                }
                command.Length--;
                command.Length--;
                commands.Add(command.ToString());

                command = new StringBuilder();
                var tempCommand = new StringBuilder();

                command.Append("CREATE DEFINER=`9eb138_config`@`%` PROCEDURE `" + function.Name + "_" + excelID + "` (");
                for (int i = 0; i < function.ParametersNameAndInputValueRows; i++)
                {
                    if (function.ParametersNameAndInputValue[i, 0].Equals(function.PropertyValueName))
                    {
                        command.Append(" OUT `");
                        command.Append(function.ParametersNameAndInputValue[i, 0].Replace(' ', '_') + "` VARCHAR(50), ");
                    }
                    else
                    {
                        command.Append(" IN `");
                        command.Append(function.ParametersNameAndInputValue[i, 0].Replace(' ', '_') + "` VARCHAR(50), ");
                    }

                    //Da ne e null
                    if (function.ParametersNameAndInputValue[i, 1] != null)
                    {
                        tempCommand.Append(function.ParametersNameAndInputValue[i, 0].Replace(' ', '_') + "='\"," + function.ParametersNameAndInputValue[i, 0].Replace(' ', '_') + ",\"' AND ");
                    }

                }
                tempCommand.Length = tempCommand.Length - 4;

                command.Length--;
                command.Length--;
                command.Append(") NOT DETERMINISTIC CONTAINS SQL SQL SECURITY DEFINER BEGIN SET @c2 = ''; SET @sql = CONCAT(\"SELECT " + function.PropertyValueName.Replace(' ', '_'));
                command.Append(" INTO @c2 FROM " + function.Name + "_" + excelID + " WHERE " + tempCommand.ToString());

                command.Append("\"); PREPARE stmt FROM @sql; EXECUTE stmt; SET " + function.PropertyValueName.Replace(' ', '_') + " = @c2; END");

                commands.Add(command.ToString());

            }
            return commands;
        }
        public static void CreateDatabaseTables(int excelID, List<TagInfo> tagInfoExcel, List<Dget> dgetFunctions, List<Function> procedures, List<Function> functions)
        {
            ExecutePolicyCommand(excelID, tagInfoExcel);
            ExecuteListCommand(excelID);
            ExecutePopulateListCommand(excelID, tagInfoExcel);
            ExecuteDGETCommand(excelID, dgetFunctions);
            GenerateMasterProcedure(excelID, procedures, functions, tagInfoExcel);
        }

        public static bool ExecutePolicyCommand(int excelID, List<TagInfo> tagInfoExcel)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "server=mysql5018.smarterasp.net;user id = 9eb138_config;database=db_9eb138_config;Pwd=Tunderwriter1; Allow User Variables=True;persistsecurityinfo=True;Convert Zero Datetime=True";
            try
            {
                conn.Open();
                MySqlCommand policyCommand = new MySqlCommand();
                policyCommand.Connection = conn;
                policyCommand.CommandText = DatabaseCommands.GeneratePolicyCommand(excelID, tagInfoExcel);
                policyCommand.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                conn.Close();
                return false;
            }
        }

        public static bool ExecuteListCommand(int excelID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "server=mysql5018.smarterasp.net;user id = 9eb138_config;database=db_9eb138_config;Pwd=Tunderwriter1; Allow User Variables=True;persistsecurityinfo=True;Convert Zero Datetime=True";
            try
            {
                conn.Open();
                MySqlCommand listCommand = new MySqlCommand();
                listCommand.Connection = conn;
                listCommand.CommandText = DatabaseCommands.GenerateListCommand(excelID);
                listCommand.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                conn.Close();
                return false;
            }
        }

        public static bool ExecutePopulateListCommand(int excelID, List<TagInfo> tagInfoExcel)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "server=mysql5018.smarterasp.net;user id = 9eb138_config;database=db_9eb138_config;Pwd=Tunderwriter1; Allow User Variables=True;persistsecurityinfo=True;Convert Zero Datetime=True";
            try
            {
                conn.Open();
                MySqlCommand populateListCommand = new MySqlCommand();
                populateListCommand.Connection = conn;
                populateListCommand.CommandText = DatabaseCommands.PopulateListsCommand(excelID, tagInfoExcel);
                populateListCommand.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                conn.Close();
                return false;
            }
        }

        public static bool ExecuteDGETCommand(int excelID, List<Dget> dgetFunctions)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "server=mysql5018.smarterasp.net;user id = 9eb138_config;database=db_9eb138_config;Pwd=Tunderwriter1; Allow User Variables=True;persistsecurityinfo=True;Convert Zero Datetime=True";
            try
            {
                conn.Open();
                MySqlCommand dgetCommand = new MySqlCommand();
                dgetCommand.Connection = conn;
                var commandsDget = DatabaseCommands.GenerateDGETCommands(excelID, dgetFunctions);
                for (int i = 0; i < commandsDget.Count; i++)
                {

                    dgetCommand.CommandText = commandsDget[i];
                    dgetCommand.ExecuteNonQuery();
                }
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                conn.Close();
                return false;
            }
        }
        public static bool GenerateMasterProcedure(int excelID, List<Function> procedures, List<Function> functions, List<TagInfo> variables)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "server=mysql5018.smarterasp.net;user id = 9eb138_config;database=db_9eb138_config;Pwd=Tunderwriter1; Allow User Variables=True;persistsecurityinfo=True;Convert Zero Datetime=True";
            try
            {
                StringBuilder masterProcedure = new StringBuilder();
                masterProcedure.Append("CREATE PROCEDURE `Master_" + excelID + "`");
                masterProcedure.Append("(");
                foreach(var variable in variables)
                {
                    if(!variable.Type.Equals("label") && !variable.Type.Equals("header") && !String.IsNullOrEmpty(variable.Type))
                    masterProcedure.Append("IN "+ "`" + variable .Name + "`" +" VARCHAR(50),");
                }
                masterProcedure.Append("OUT `result` VARCHAR(50)");
                masterProcedure.Append(")");
                masterProcedure.Append("BEGIN ");
                masterProcedure.Append(ButtonsDefaultValue(variables));
                masterProcedure = GenerateMasterHelpingFunc(excelID, functions, masterProcedure);
                var masterMidResult = GenerateMidResultFuc(excelID, procedures);
                masterProcedure.Append(masterMidResult);
                masterProcedure.Append("END");
                if (masterProcedure.Length > 0)
                {
                    conn.Open();
                    MySqlCommand mysqlCommand = new MySqlCommand();
                    mysqlCommand.Connection = conn;

                    mysqlCommand.CommandText = masterProcedure.ToString();
                    mysqlCommand.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                return false;;
            }
            return false;
        }

        public static StringBuilder ButtonsDefaultValue(List<TagInfo> inputParameters)
        {
            StringBuilder inputParametesDefaultValue = new StringBuilder();
            foreach( TagInfo inputParameter in inputParameters)
            {
                    if (inputParameter.Type.ToLower().Contains("radio") || inputParameter.Type.ToLower().Contains("checkbox"))
                    {
                        inputParametesDefaultValue.Append(" IF " + inputParameter.Name + "= '' THEN ");
                        inputParametesDefaultValue.Append("SET " + inputParameter.Name + " = 'false'; END IF; ");
                    }          
            }
            return inputParametesDefaultValue;
        }

        public static StringBuilder GenerateMasterHelpingFunc(int excelID, List<Function> functions, StringBuilder masterProcedure)
        {
            try
            {
                var fuctName = new List<string>();
                fuctName = functions.Select(x => x.Name).ToList();
                foreach (var function in functions)
                {
                    var funcValue = function.ToString().ToLower();
                    if (funcValue.StartsWith("dget"))
                    {
                        Dget castFunction = (Dget)function;

                        StringBuilder parameters = new StringBuilder();
                        for (int i = 0; i < castFunction.DatabaseRows; i++)
                        {
                            if(castFunction.Database[i, 0] != castFunction.PropertyValueName)
                            {
                                var parametar = castFunction.Database[i, 0].Replace(' ', '_') + ", ";
                                parameters.Append(ChangeParameterIfFunction(parametar, fuctName));
                            }
                               
                        }
                        parameters.Length = parameters.Length - 2;

                        masterProcedure.Append(" SET @Output" + function.Name + "='';");
                        masterProcedure.Append(" CALL " + function.Name + "_" + excelID + "(" + parameters + ", @Output" + function.Name + "); ");

                    }
                    else if (funcValue.StartsWith("(if"))
                    {
                        IfCondition castFunction = (IfCondition)function;
                        masterProcedure.Append(" SET @Output" + function.Name + "='';");
                        castFunction.Condition.OperandLeft = ChangeParameterIfFunction(castFunction.Condition.OperandLeft, fuctName);
                        castFunction.Condition.OperandRight = ChangeParameterIfFunction(castFunction.Condition.OperandRight, fuctName);
                        castFunction.IfTrue = ChangeParameterIfFunction(castFunction.IfTrue, fuctName);
                        castFunction.IfFalse = ChangeParameterIfFunction(castFunction.IfFalse, fuctName);
                        switch (castFunction.Condition.Operation)
                        {
                            case "=":
                                {
                                    masterProcedure.Append(" CALL IfEquals(@Output" + function.Name + ", " + castFunction.Condition.OperandLeft + ", " + castFunction.Condition.OperandRight + ", " + castFunction.IfFalse + ", " + castFunction.IfTrue + "); ");
                                    break;
                                }
                            case ">":
                                {
                                    masterProcedure.Append(" CALL IfBigger(@Output" + function.Name + ", " + castFunction.Condition.OperandLeft + ", " + castFunction.Condition.OperandRight + ", " + castFunction.IfTrue + ", " + castFunction.IfFalse + "); ");
                                    break;
                                }
                            case "<":
                                {
                                    masterProcedure.Append(" CALL IfSmaller(@Output" + function.Name + ", " + castFunction.Condition.OperandLeft + ", " + castFunction.Condition.OperandRight + ", " + castFunction.IfFalse + ", " + castFunction.IfTrue + "); ");
                                    break;
                                }
                            case ">=":
                                {
                                    masterProcedure.Append(" CALL IfBiggerAndEqual@Output" + function.Name + ", " + castFunction.Condition.OperandLeft + ", " + castFunction.Condition.OperandRight + ", " + castFunction.IfFalse + ", " + castFunction.IfTrue + "); ");
                                    break;
                                }
                            case "<=":
                                {
                                    masterProcedure.Append(" CALL IfSmallerAndEqual(@Output" + function.Name + ", " + castFunction.Condition.OperandLeft + ", " + castFunction.Condition.OperandRight + ", " + castFunction.IfFalse + ", " + castFunction.IfTrue + "); ");
                                    break;
                                }
                        }
                    }
                    else if (funcValue.StartsWith("round"))
                    {
                        Round castFunction = (Round)function;
                        masterProcedure.Append(" SET @Output" + function.Name + "='';");
                        castFunction.Number = ChangeParameterIfFunction(castFunction.Number, fuctName);
                        castFunction.RoundTo = ChangeParameterIfFunction(castFunction.RoundTo, fuctName);
                        masterProcedure.Append(" CALL Round(" + castFunction.Number + ", @OutputRound, " + castFunction.RoundTo + "); ");
                    }
                    else if (funcValue.StartsWith("exact"))
                    {
                        Exact castFunction = (Exact)function;
                        masterProcedure.Append(" SET @Output" + function.Name + "='';");
                        castFunction.LeftOperand = ChangeParameterIfFunction(castFunction.LeftOperand, fuctName);
                        castFunction.RightOperand = ChangeParameterIfFunction(castFunction.RightOperand, fuctName);
                        masterProcedure.Append(" CALL Exact(" + castFunction.LeftOperand + ", @OutputExact, " + castFunction.RightOperand + "); ");
                    }
                    else
                    {
                        MathOperation castFunction = (MathOperation)function;
                        masterProcedure.Append(" SET @Output" + function.Name + "='';");
                        castFunction.OperandRight = ChangeParameterIfFunction(castFunction.OperandRight, fuctName);
                        castFunction.OperandLeft = ChangeParameterIfFunction(castFunction.OperandLeft, fuctName);
                        switch (castFunction.Operation)
                        {
                            case "*":
                                {
                                    masterProcedure.Append(" CALL Multiplication(" + castFunction.OperandRight + ", " + castFunction.OperandLeft + "," + "@Output" + function.Name + "); ");
                                    break;
                                }
                            case "/":
                                {
                                    masterProcedure.Append(" CALL Division(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Output" + function.Name + "); ");
                                    break;
                                }
                            case "+":
                                {
                                    masterProcedure.Append(" CALL Addition(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Output" + function.Name + "); ");
                                    break;
                                }
                            case "-":
                                {
                                    masterProcedure.Append(" CALL Substraction(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Output" + function.Name + "); ");
                                    break;
                                }
                            case ">":
                                {
                                    masterProcedure.Append(" CALL CompareBigger(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Output" + function.Name + "); ");
                                    break;
                                }
                            case ">=":
                                {
                                    masterProcedure.Append(" CALL CompareBiggerAndEqual(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Output" + function.Name + "); ");
                                    break;
                                }
                            case "=":
                                {
                                    masterProcedure.Append(" CALL CompareEqual(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Output" + function.Name + "); ");
                                    break;
                                }
                            case "<":
                                {
                                    masterProcedure.Append(" CALL CompareSmaller(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Output" + function.Name + "); ");
                                    break;
                                }
                            case "<=":
                                {
                                    masterProcedure.Append(" CALL CompareSmallerAndEqual(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Output" + function.Name + "); ");
                                    break;
                                }
                        }
                    }
                }
                return masterProcedure;
            }
            catch (Exception e)
            {
                return new StringBuilder();
            }

        }

        public static StringBuilder GenerateMidResultFuc(int excelID, List<Function> procedures)
        {
            var lastProcedure = procedures.Last();
            var masterProcedure = new StringBuilder();
            try
            {
                foreach (var procedure in procedures)
                {
                    string outputParametar = " @OutputProcedure" + procedure.Name;
                    masterProcedure.Append(" SET @OutputProcedure" + procedure.Name + "='';");
                    
                    var procValue = procedure.ToString().ToLower();
                    if (procValue.StartsWith("(if"))
                    {
                        IfCondition castProcedure = (IfCondition)procedure;
                        castProcedure.Condition.OperandLeft = ChangeParameterIfProcedure(castProcedure.Condition.OperandLeft);
                        castProcedure.Condition.OperandRight = ChangeParameterIfProcedure(castProcedure.Condition.OperandRight);
                        castProcedure.IfFalse = ChangeParameterIfProcedure(castProcedure.IfFalse);
                        castProcedure.IfTrue = ChangeParameterIfProcedure(castProcedure.IfTrue);

                        switch (castProcedure.Condition.Operation)
                        {
                            case "=":
                                {
                                    masterProcedure.Append(" CALL IfEquals(" + outputParametar + ", " + castProcedure.Condition.OperandLeft + ", " + castProcedure.Condition.OperandRight + ", " + castProcedure.IfFalse + ", " + castProcedure.IfTrue + "); ");
                                    break;
                                }
                            case ">":
                                {
                                    masterProcedure.Append(" CALL IfBigger(" + outputParametar + ", " + castProcedure.Condition.OperandLeft + ", " + castProcedure.Condition.OperandRight + ", " + castProcedure.IfTrue + ", " + castProcedure.IfFalse + "); ");
                                    break;
                                }
                            case "<":
                                {
                                    masterProcedure.Append(" CALL IfSmaller(" + outputParametar + ", " + castProcedure.Condition.OperandLeft + ", " + castProcedure.Condition.OperandRight + ", " + castProcedure.IfFalse + ", " + castProcedure.IfTrue + "); ");
                                    break;
                                }
                            case ">=":
                                {
                                    masterProcedure.Append(" CALL IfBiggerAndEqual(" + outputParametar + ", " + castProcedure.Condition.OperandLeft + ", " + castProcedure.Condition.OperandRight + ", " + castProcedure.IfFalse + ", " + castProcedure.IfTrue + "); ");
                                    break;
                                }
                            case "<=":
                                {
                                    masterProcedure.Append(" CALL IfSmallerAndEqual(" + outputParametar + ", " + castProcedure.Condition.OperandLeft + ", " + castProcedure.Condition.OperandRight + ", " + castProcedure.IfFalse + ", " + castProcedure.IfTrue + "); ");
                                    break;
                                }
                        }
                    }
                    else if (procValue.StartsWith("round"))
                    {
                        Round castProcedure = (Round)procedure;
                        castProcedure.Number = ChangeParameterIfProcedure(castProcedure.Number);
                        castProcedure.RoundTo = ChangeParameterIfProcedure(castProcedure.RoundTo);
                        masterProcedure.Append(" CALL Round(" + castProcedure.Number + ", " + outputParametar + ", " + castProcedure.RoundTo + "); ");
                    }
                    else if (procValue.StartsWith("exact"))
                    {
                        Exact castProcedure = (Exact)procedure;
                        castProcedure.LeftOperand = ChangeParameterIfProcedure(castProcedure.LeftOperand);
                        castProcedure.RightOperand = ChangeParameterIfProcedure(castProcedure.RightOperand);
                        masterProcedure.Append(" CALL Exact(" + castProcedure.LeftOperand + ", " + outputParametar + ", " + castProcedure.RightOperand + "); ");
                    }
                    else
                    {
                        MathOperation castProcedure = (MathOperation)procedure;
                        castProcedure.OperandLeft = ChangeParameterIfProcedure(castProcedure.OperandLeft);
                        castProcedure.OperandRight = ChangeParameterIfProcedure(castProcedure.OperandRight);
                        switch (castProcedure.Operation)
                        {
                            case "*":
                                {
                                    masterProcedure.Append(" CALL Multiplication(" + castProcedure.OperandRight + ", " + castProcedure.OperandLeft + "," + outputParametar + "); ");
                                    break;
                                }
                            case "/":
                                {
                                    masterProcedure.Append(" CALL Division(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + outputParametar + "); ");
                                    break;
                                }
                            case "+":
                                {
                                    masterProcedure.Append(" CALL Addition(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + outputParametar + "); ");
                                    break;
                                }
                            case "-":
                                {
                                    masterProcedure.Append(" CALL Substraction(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + outputParametar + "); ");
                                    break;
                                }
                            case ">":
                                {
                                    masterProcedure.Append(" CALL CompareBigger(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + outputParametar + "); ");
                                    break;
                                }
                            case ">=":
                                {
                                    masterProcedure.Append(" CALL CompareBiggerAndEqual(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + outputParametar + "); ");
                                    break;
                                }
                            case "=":
                                {
                                    masterProcedure.Append(" CALL CompareEqual(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + outputParametar + "); ");
                                    break;
                                }
                            case "<":
                                {
                                    masterProcedure.Append(" CALL CompareSmaller(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + outputParametar + "); ");
                                    break;
                                }
                            case "<=":
                                {
                                    masterProcedure.Append(" CALL CompareSmallerAndEqual(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + outputParametar + "); ");
                                    break;
                                }
                        }
                    }

                    if(procedure == lastProcedure)
                    {
                        masterProcedure.Append(" SET result=" + outputParametar + ";");
                    }
                  
                }
                return masterProcedure;
            }
            catch (Exception e)
            {
                return new StringBuilder();
            }
        }

        public static string ChangeParameterIfProcedure(string parameter)
        {
            string output = null;
            if(parameter != null)
            {
                if (parameter.ToLower().StartsWith("procedure"))
                {
                    var procedurNum = parameter.ToLower().Replace("procedure", string.Empty);
                    output = "@OutputProcedure" + procedurNum;
                    return output;
                }
                if (parameter.ToLower().StartsWith("c"))
                {
                    output = "@Output" + parameter;
                    return output;
                }
            }
            
            return parameter;
        }
        public static string ChangeParameterIfFunction(string parameter, List<string> fuctName)
        {
            string output = null;
            if(parameter != null)
            {
                var contains = fuctName.Contains(parameter);
                if (contains)
                {
                    output = "@Output" + parameter;
                    return output;
                }
            }
           
            return parameter;
        }

        public static string CalculatePremium(int excelId, FormCollection formCollection)
        {
            string result = "No value";
            MySqlConnection conn = new MySqlConnection();
            MySqlCommand mysqlCommand = new MySqlCommand();
            mysqlCommand.Connection = conn;
            conn.ConnectionString = "server=mysql5018.smarterasp.net;user id = 9eb138_config;database=db_9eb138_config;Pwd=Tunderwriter1; Allow User Variables=True;persistsecurityinfo=True;Convert Zero Datetime=True";
            conn.Open();
            try
            {
                mysqlCommand.CommandText = "Master_" + excelId;
                mysqlCommand.CommandType = CommandType.StoredProcedure;

                foreach (string key in formCollection.Keys)
                {
                    mysqlCommand.Parameters.AddWithValue("@"+key, formCollection[key]);
                    mysqlCommand.Parameters["@"+key].Direction = ParameterDirection.Input;
                }
                mysqlCommand.Parameters.AddWithValue("@result", MySqlDbType.VarChar);
                mysqlCommand.Parameters["@result"].Direction = ParameterDirection.Output;

                mysqlCommand.ExecuteNonQuery();

                result = mysqlCommand.Parameters["@result"].Value.ToString();
            }
            catch(Exception e)
            {

            }
            finally
            {
                conn.Close();
            }
            return result;
        }
    }
}