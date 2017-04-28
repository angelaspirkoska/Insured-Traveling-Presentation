using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace InsuredTraveling.FormBuilder
{
    public static class DatabaseCommands
    {
        public static string GeneratePolicyCommand(int excelID, List<TagInfo> tagInfoExcel)
        {
            StringBuilder newPolicyTable = new StringBuilder();
            newPolicyTable.Append("CREATE TABLE Policy" + excelID + " ( ID INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY");
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
            return "CREATE TABLE Lists" + excelID + " ( ID INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY, List_ID INT NOT NULL, List_Name VARCHAR(20) NOT NULL, Parameter_Value VARCHAR(50) NOT NULL)";
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
        public static List<string> CreateDGETCommands(int excelID, List<Dget> dgetFunctions)
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
                command.Append(")");
                commands.Add(command.ToString());

                command = new StringBuilder();
                command.Append("INSERT INTO " + function.Name + excelID + " (" + insertInto.ToString() + ") VALUES ");
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
            }
            return commands;
        }
        public static void CreateDatabaseTables(int excelID, List<TagInfo> tagInfoExcel, List<Dget> dgetFunctions)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "server=localhost;user id = root;database=db_9eb138_travel;persistsecurityinfo=True;Convert Zero Datetime=True";
            var command = DatabaseCommands.GeneratePolicyCommand(excelID, tagInfoExcel);
            try
            {
                conn.Open();
                MySqlCommand mysqlCommand = new MySqlCommand();
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
                var commandsDget = DatabaseCommands.CreateDGETCommands(excelID, dgetFunctions);
                foreach (string commandDget in commandsDget)
                {
                    try
                    {
                        mysqlCommand.CommandText = commandDget;
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
        }
    }
}