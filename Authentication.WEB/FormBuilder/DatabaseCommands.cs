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
            commandText.Append("INSERT INTO Lists" + excelID + " (List_ID ,List_Name,Parameter_Value) VALUES ");
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

                command.Append("CREATE DEFINER=`root`@`localhost` PROCEDURE `" + function.Name + "_" + excelID + "` (");
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
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "server=mysql5018.smarterasp.net;user id = 9eb138_config;database=db_9eb138_config;Pwd=Tunderwriter1; Allow User Variables=True;persistsecurityinfo=True;Convert Zero Datetime=True";
            var command = DatabaseCommands.GeneratePolicyCommand(excelID, tagInfoExcel);
            try
            {
                conn.Open();
                MySqlCommand mysqlCommand = new MySqlCommand();
                mysqlCommand.Connection = conn;
                try
                {
                    mysqlCommand.CommandText = "get_users_by_state";
                    mysqlCommand.CommandType = CommandType.StoredProcedure;

                    mysqlCommand.Parameters.AddWithValue("@item_type", "Taxi");
                    mysqlCommand.Parameters["@item_type"].Direction = ParameterDirection.Input;

                    mysqlCommand.Parameters.AddWithValue("@ccRange", "greater then 3000");
                    mysqlCommand.Parameters["@ccRange"].Direction = ParameterDirection.Input;

                    mysqlCommand.Parameters.AddWithValue("@Rates", MySqlDbType.VarChar);
                    mysqlCommand.Parameters["@Rates"].Direction = ParameterDirection.Output;

                    mysqlCommand.ExecuteNonQuery();

                    var m = mysqlCommand.Parameters["@Rates"].Value;
                }
                catch (Exception ex)
                {

                }
                mysqlCommand = new MySqlCommand();
                mysqlCommand.CommandText = command;
                mysqlCommand.Connection = conn;
                mysqlCommand.ExecuteNonQuery();
                try
                {
                    mysqlCommand.CommandText = DatabaseCommands.GenerateListCommand(excelID);
                    mysqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                try
                {
                    mysqlCommand.CommandText = DatabaseCommands.PopulateListsCommand(excelID, tagInfoExcel);
                    mysqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                var commandsDget = DatabaseCommands.GenerateDGETCommands(excelID, dgetFunctions);
                for (int i = 0; i < commandsDget.Count; i++)
                {
                    try
                    {
                        mysqlCommand.CommandText = commandsDget[i];
                        mysqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            finally
            {
                conn.CloseAsync();
            }
            var test = GenerateMasterProcedure(excelID, procedures, functions, tagInfoExcel);
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
                    masterProcedure.Append("IN "+ "`" + variable .Name + "`" +" VARCHAR(50),");
                }
                masterProcedure.Append("OUT `result` VARCHAR(50)");
                masterProcedure.Append(")");
                masterProcedure.Append("BEGIN");
                var masterHelpFunct = GenerateMasterHelpingFunc(excelID, functions, masterProcedure);
                masterProcedure.Append(masterHelpFunct);
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
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;;
            }
            return false;
        }

        public static StringBuilder GenerateMasterHelpingFunc(int excelID, List<Function> functions, StringBuilder masterProcedure)
        {
            try
            {
                foreach (var function in functions)
                {
                    var funcValue = function.ToString().ToLower();
                    if (funcValue.StartsWith("dget"))
                    {
                        Dget castFunction = (Dget)function;

                        StringBuilder parameters = new StringBuilder();
                        for (int i = 0; i < castFunction.DatabaseRows; i++)
                        {
                            parameters.Append(castFunction.Database[i, 0].Replace(' ', '_') + ", ");
                        }
                        parameters.Length = parameters.Length - 2;

                        masterProcedure.Append(" SET @Ouput" + function.Name + "='';");
                        masterProcedure.Append(" CALL " + function.Name + "_" + excelID + "(" + parameters + ", @Ouput" + function.Name + "); ");

                    }
                    else if (funcValue.StartsWith("(if"))
                    {
                        IfCondition castFunction = (IfCondition)function;
                        masterProcedure.Append(" SET @Ouput" + function.Name + "='';");
                        switch (castFunction.Condition.Operation)
                        {
                            case "=":
                                {
                                    masterProcedure.Append(" CALL IfEquals(@Ouput" + function.Name + ", " + castFunction.Condition.OperandLeft + ", " + castFunction.Condition.OperandRight + ", " + castFunction.IfTrue + ", " + castFunction.IfFalse + "); ");
                                    break;
                                }
                            case ">":
                                {
                                    masterProcedure.Append(" CALL IfBigger(@Ouput" + function.Name + ", " + castFunction.Condition.OperandLeft + ", " + castFunction.Condition.OperandRight + ", " + castFunction.IfTrue + ", " + castFunction.IfFalse + "); ");
                                    break;
                                }
                            case "<":
                                {
                                    masterProcedure.Append(" CALL IfSmaller(@Ouput" + function.Name + ", " + castFunction.Condition.OperandLeft + ", " + castFunction.Condition.OperandRight + ", " + castFunction.IfTrue + ", " + castFunction.IfFalse + "); ");
                                    break;
                                }
                            case ">=":
                                {
                                    masterProcedure.Append(" CALL IfBiggerAndEqual@Ouput" + function.Name + ", " + castFunction.Condition.OperandLeft + ", " + castFunction.Condition.OperandRight + ", " + castFunction.IfTrue + ", " + castFunction.IfFalse + "); ");
                                    break;
                                }
                            case "<=":
                                {
                                    masterProcedure.Append(" CALL IfSmallerAndEqual(@Ouput" + function.Name + ", " + castFunction.Condition.OperandLeft + ", " + castFunction.Condition.OperandRight + ", " + castFunction.IfTrue + ", " + castFunction.IfFalse + "); ");
                                    break;
                                }
                        }
                    }
                    else if (funcValue.StartsWith("round"))
                    {
                        Round castFunction = (Round)function;
                        masterProcedure.Append(" SET @Ouput" + function.Name + "='';");
                        masterProcedure.Append(" CALL Round(" + castFunction.Number + ", @OuputRound, " + castFunction.RoundTo + "); ");
                    }
                    else if (funcValue.StartsWith("exact"))
                    {
                        Exact castFunction = (Exact)function;
                        masterProcedure.Append(" SET @Ouput" + function.Name + "='';");
                        masterProcedure.Append(" CALL Exact(" + castFunction.LeftOperand + ", @OuputExact, " + castFunction.RightOperand + "); ");
                    }
                    else
                    {
                        MathOperation castFunction = (MathOperation)function;
                        masterProcedure.Append(" SET @Ouput" + function.Name + "='';");
                        switch (castFunction.Operation)
                        {
                            case "*":
                                {
                                    masterProcedure.Append(" CALL Multiplication(" + castFunction.OperandRight + ", " + castFunction.OperandLeft + "," + "@Ouput" + function.Name + "); ");
                                    break;
                                }
                            case "/":
                                {
                                    masterProcedure.Append(" CALL Division(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Ouput" + function.Name + "); ");
                                    break;
                                }
                            case "+":
                                {
                                    masterProcedure.Append(" CALL Addition(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Ouput" + function.Name + "); ");
                                    break;
                                }
                            case "-":
                                {
                                    masterProcedure.Append(" CALL Substraction(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Ouput" + function.Name + "); ");
                                    break;
                                }
                            case ">":
                                {
                                    masterProcedure.Append(" CALL CompareBigger(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Ouput" + function.Name + "); ");
                                    break;
                                }
                            case ">=":
                                {
                                    masterProcedure.Append(" CALL CompareBiggerAndEqual(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Ouput" + function.Name + "); ");
                                    break;
                                }
                            case "=":
                                {
                                    masterProcedure.Append(" CALL CompareEqual(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Ouput" + function.Name + "); ");
                                    break;
                                }
                            case "<":
                                {
                                    masterProcedure.Append(" CALL CompareSmaller(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Ouput" + function.Name + "); ");
                                    break;
                                }
                            case "<=":
                                {
                                    masterProcedure.Append(" CALL CompareSmallerAndEqual(" + castFunction.OperandLeft + ", " + castFunction.OperandRight + "," + "@Ouput" + function.Name + "); ");
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
            var masterProcedure = new StringBuilder();
            try
            {
                foreach (var procedure in procedures)
                {
                    var procValue = procedure.ToString().ToLower();
                    if (procValue.StartsWith("(if"))
                    {
                        IfCondition castProcedure = (IfCondition)procedure;
                        masterProcedure.Append(" SET @OuputProcedure" + procedure.Name + "='';");
                        castProcedure.Condition.OperandLeft = ChangeParameterIfProcedure(castProcedure.Condition.OperandLeft);
                        castProcedure.Condition.OperandRight = ChangeParameterIfProcedure(castProcedure.Condition.OperandRight);
                        castProcedure.IfFalse = ChangeParameterIfProcedure(castProcedure.IfFalse);
                        castProcedure.IfTrue = ChangeParameterIfProcedure(castProcedure.IfTrue);

                        switch (castProcedure.Condition.Operation)
                        {
                            case "=":
                                {
                                    masterProcedure.Append(" CALL IfEquals(@OuputProcedure" + procedure.Name + ", " + castProcedure.Condition.OperandLeft + ", " + castProcedure.Condition.OperandRight + ", " + castProcedure.IfTrue + ", " + castProcedure.IfFalse + "); ");
                                    break;
                                }
                            case ">":
                                {
                                    masterProcedure.Append(" CALL IfBigger(@OuputProcedure" + procedure.Name + ", " + castProcedure.Condition.OperandLeft + ", " + castProcedure.Condition.OperandRight + ", " + castProcedure.IfTrue + ", " + castProcedure.IfFalse + "); ");
                                    break;
                                }
                            case "<":
                                {
                                    masterProcedure.Append(" CALL IfSmaller(@OuputProcedure" + procedure.Name + ", " + castProcedure.Condition.OperandLeft + ", " + castProcedure.Condition.OperandRight + ", " + castProcedure.IfTrue + ", " + castProcedure.IfFalse + "); ");
                                    break;
                                }
                            case ">=":
                                {
                                    masterProcedure.Append(" CALL IfBiggerAndEqual(@OuputProcedure" + procedure.Name + ", " + castProcedure.Condition.OperandLeft + ", " + castProcedure.Condition.OperandRight + ", " + castProcedure.IfTrue + ", " + castProcedure.IfFalse + "); ");
                                    break;
                                }
                            case "<=":
                                {
                                    masterProcedure.Append(" CALL IfSmallerAndEqual(@OuputProcedure" + procedure.Name + ", " + castProcedure.Condition.OperandLeft + ", " + castProcedure.Condition.OperandRight + ", " + castProcedure.IfTrue + ", " + castProcedure.IfFalse + "); ");
                                    break;
                                }
                        }
                    }
                    else if (procValue.StartsWith("round"))
                    {
                        Round castProcedure = (Round)procedure;
                        castProcedure.Number = ChangeParameterIfProcedure(castProcedure.Number);
                        castProcedure.RoundTo = ChangeParameterIfProcedure(castProcedure.RoundTo);
                        masterProcedure.Append(" SET @OuputProcedure" + procedure.Name + "='';");
                        masterProcedure.Append(" CALL Round(" + castProcedure.Number + ", " + "@OuputProcedure" + procedure.Name + ", " + castProcedure.RoundTo + "); ");
                    }
                    else if (procValue.StartsWith("exact"))
                    {
                        Exact castProcedure = (Exact)procedure;
                        castProcedure.LeftOperand = ChangeParameterIfProcedure(castProcedure.LeftOperand);
                        castProcedure.RightOperand = ChangeParameterIfProcedure(castProcedure.RightOperand);
                        masterProcedure.Append(" SET @OuputProcedure" + procedure.Name + "='';");
                        masterProcedure.Append(" CALL Exact(" + castProcedure.LeftOperand + ", " + "@OuputProcedure" + procedure.Name + ", " + castProcedure.RightOperand + "); ");
                    }
                    else
                    {
                        MathOperation castProcedure = (MathOperation)procedure;
                        masterProcedure.Append(" SET @OuputProcedure" + procedure.Name + "='';");
                        castProcedure.OperandLeft = ChangeParameterIfProcedure(castProcedure.OperandLeft);
                        castProcedure.OperandRight = ChangeParameterIfProcedure(castProcedure.OperandRight);
                        switch (castProcedure.Operation)
                        {
                            case "*":
                                {
                                    masterProcedure.Append(" CALL Multiplication(" + castProcedure.OperandRight + ", " + castProcedure.OperandLeft + "," + "@OuputProcedure" + castProcedure.Name + "); ");
                                    break;
                                }
                            case "/":
                                {
                                    masterProcedure.Append(" CALL Division(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + "@OuputProcedure" + castProcedure.Name + "); ");
                                    break;
                                }
                            case "+":
                                {
                                    masterProcedure.Append(" CALL Addition(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + "@OuputProcedure" + castProcedure.Name + "); ");
                                    break;
                                }
                            case "-":
                                {
                                    masterProcedure.Append(" CALL Substraction(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + "@OuputProcedure" + castProcedure.Name + "); ");
                                    break;
                                }
                            case ">":
                                {
                                    masterProcedure.Append(" CALL CompareBigger(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + "@OuputProcedure" + castProcedure.Name + "); ");
                                    break;
                                }
                            case ">=":
                                {
                                    masterProcedure.Append(" CALL CompareBiggerAndEqual(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + "@OuputProcedure" + castProcedure.Name + "); ");
                                    break;
                                }
                            case "=":
                                {
                                    masterProcedure.Append(" CALL CompareEqual(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + "@OuputProcedure" + castProcedure.Name + "); ");
                                    break;
                                }
                            case "<":
                                {
                                    masterProcedure.Append(" CALL CompareSmaller(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + "@OuputProcedure" + castProcedure.Name + "); ");
                                    break;
                                }
                            case "<=":
                                {
                                    masterProcedure.Append(" CALL CompareSmallerAndEqual(" + castProcedure.OperandLeft + ", " + castProcedure.OperandRight + "," + "@OuputProcedure" + castProcedure.Name + "); ");
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

        public static string ChangeParameterIfProcedure(string parameter)
        {
            string ouput = null;
            if (parameter.ToLower().StartsWith("procedure"))
            {
                var procedurNum = parameter.Last();
                ouput = "@OutputProcedure" + procedurNum;
                return ouput;
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
            catch
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