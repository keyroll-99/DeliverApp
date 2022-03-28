using DeployApp.Model;
using System.Data.SqlClient;

namespace DeployApp
{
    public static class ExecuteScript
    {
        private const string folderNameField = "folder";
        private const string scriptNameFiled = "name";

        public static ScriptModel GetExecutedScripts(ref SqlConnection conn)
        {
            var query = "Select TOP 1 * from deployScripts order by create_at DESC, folder DESC, name DESC;";
            var result = new ScriptModel();
            try
            {
                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                reader.Read();
                result.Folder = reader[folderNameField].ToString();
                result.ScriptNumber = Convert.ToInt32(reader[scriptNameFiled].ToString().Split(".")[0]);
                reader.Close();

            }
            catch (Exception ex)
            {
                result.Folder = null;
                result.ScriptNumber = null;
            }

            return result;
        }

        public static void RunScripts(ref SqlConnection conn, string basePath, ScriptModel executedScripts)
        {
            executedScripts.ScriptNumber++;
            var scriptPath = $"{basePath}{Path.DirectorySeparatorChar}Script{Path.DirectorySeparatorChar}";
            List<string> executedSql = new List<string>();
            var cmd = new SqlCommand
            {
                Connection = conn
            };
            bool ok = true;

            while (Directory.Exists($"{scriptPath}{executedScripts.Folder}"))
            {
                while (true)
                {
                    var fiels = Directory.EnumerateFiles($"{scriptPath}{executedScripts.Folder}", $"{executedScripts.ScriptNumber.Value.ToString("00")}.*.sql");

                    if (!fiels.Any())
                    {
                        break;
                    }

                    foreach (var file in fiels)
                    {

                        var pathArray = file.Split(Path.DirectorySeparatorChar);
                        var fileName = pathArray.Last();
                        var folder = pathArray[pathArray.Length - 2];
                        try
                        {
                            var content = File.ReadAllText(file);
                            cmd.CommandText = content;
                            cmd.ExecuteNonQuery();
                            executedSql.Add(content);
                            var insertSql =
                                $"INSERT INTO deployScripts (name, folder) VALUES ('{fileName}', '{folder}')";
                            cmd.CommandText = insertSql;
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine(ex.Message);
                            throw new Exception(ex.Message);
                        }
                    }
                    executedScripts.ScriptNumber++;
                }
                executedScripts.Folder = (Convert.ToDecimal(executedScripts.Folder) + 1).ToString("0000");
                executedScripts.ScriptNumber = 0;
            }
        }
    }
}
