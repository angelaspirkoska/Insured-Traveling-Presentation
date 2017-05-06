using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


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
                for(int i = 0; i < function.ParametersNameAndInputValueRows; i++)
                {
                    if(function.ParametersNameAndInputValue[i, 0].Equals(function.PropertyValueName)){
                        command.Append(" OUT `");
                        command.Append(function.ParametersNameAndInputValue[i, 0].Replace(' ', '_') + "` VARCHAR(50), ");
                    }
                    else
                    {
                        command.Append(" IN `");
                        command.Append(function.ParametersNameAndInputValue[i, 0].Replace(' ', '_') + "` VARCHAR(50), ");
                    }
                    
                    //Da ne e null
                    if(function.ParametersNameAndInputValue[i, 1] != null)
                    {
                        tempCommand.Append(function.ParametersNameAndInputValue[i, 0].Replace(' ', '_') + "='\","+ function.ParametersNameAndInputValue[i, 0].Replace(' ', '_') + ",\"' AND ");
                    }
                   
                }
                tempCommand.Length = tempCommand.Length - 4;

                command.Length--;
                command.Length--;
                command.Append(") NOT DETERMINISTIC CONTAINS SQL SQL SECURITY DEFINER BEGIN SET @c2 = ''; SET @sql = CONCAT(\"SELECT " +function.PropertyValueName.Replace(' ', '_'));
                command.Append(" INTO @c2 FROM "+ function.Name + "_" + excelID +" WHERE " + tempCommand.ToString());

                command.Append("\"); PREPARE stmt FROM @sql; EXECUTE stmt; SET "+ function.PropertyValueName.Replace(' ', '_') + " = @c2; END");

                commands.Add(command.ToString());

            }      
            return commands;
        }

        public static string GenerateMasterProcedure(int excelID, List<Function> procedures)
        {
            StringBuilder masterProcedure = new StringBuilder();
            StringBuilder masterParameters = new StringBuilder();
            masterProcedure.Append("");
            foreach (Function procedure in procedures)
            {
                var ifNesto = procedure.ToString().ToLower().StartsWith("(if");
                if (procedure.ToString().ToLower().StartsWith("(if") || procedure.ToString().ToLower().StartsWith("(exact") || procedure.ToString().ToLower().StartsWith("(round") || procedure.ToString().ToLower().StartsWith("(dget"))
                {

                }
                else
                {

                    MathOperation newOperation = (MathOperation)procedure;
                    var operandLeft = " IN `Procedure" + procedure.Name + "_OperandLeft" + "` FLOAT, ";
                    var operandRight = " IN `Procedure" + procedure.Name + "_OperandRight" + "` FLOAT, ";
                    masterParameters.Append(operandLeft);
                    masterParameters.Append(operandRight);

                    masterProcedure.Append("SET @Procedure" + procedure.Name +"_Output = ''; ");
                    masterProcedure.Append("( call ");
                    if (newOperation.Operation == "*")
                    {
                        masterProcedure.Append("Multiplication(");
                    }

                    masterProcedure.Append("Procedure" + procedure.Name + "_OperandLeft, " + "Procedure" + procedure.Name + "_OperandRight, " + "@Procedure" + procedure.Name +"_Output); ");

                    if (newOperation.OperandLeft.StartsWith("Procedure"))
                    {
                        var procedureName = newOperation.OperandLeft.Replace("Procedure", "");
                        var inputFromPreviousOutput = "@Procedure" + procedureName + "_Output";
                    }
                }
            }
            return null;
        }
        public static void CreateDatabaseTables(int excelID, List<TagInfo> tagInfoExcel, List<Dget> dgetFunctions, List<Function> procedures)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "server=localhost;user id = root;database=db_9eb138_travel;Allow User Variables=True;persistsecurityinfo=True;Convert Zero Datetime=True";
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
                for (int i =0; i < commandsDget.Count; i++)
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
            var test = GenerateMasterProcedure(excelID, procedures);
        }
    }
}