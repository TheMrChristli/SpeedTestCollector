using System.Dynamic;
using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Globalization;
using SpeedTestCollector.configuration;
using SpeedTestCollector.util;

namespace SpeedTestCollector
{
    class Program
    {
        public static Configuration Config { get { return _configuration; } }
        public static MySQLClass MySQL { get { return _mysql; } }
        static void Main(string[] args)
        {
            Logger.Print("Starting...");
            new Program().MainTask().GetAwaiter().GetResult();
        }

        private static Configuration _configuration;
        private static MySQLClass _mysql;

        public Program()
        {
            _configuration = new Configuration();

            Sql sqlCon = _configuration.Config.mysql;
            _mysql = new MySQLClass(sqlCon.host, sqlCon.username, sqlCon.password, sqlCon.database);
        }

        public Task MainTask()
        {
            _mysql.Connect();

            createTable();

            Result result = getResult();
            Output op = Output.ConvertOutput(result.Results);

            string sql = $"INSERT INTO {MySQLClass.TableName} (download, upload, ping) VALUES " +
                        $"('{op.download.ToString("0.00", CultureInfo.InvariantCulture)}', '{op.upload.ToString("0.00", CultureInfo.InvariantCulture)}', {op.ping})";
            Logger.Print(sql);
            _mysql.Update(sql);


            /*
            Logger.Print($"Download: {op.download.ToString()}");
            Logger.Print($"Upload: {op.upload.ToString()}");
            Logger.Print($"Ping: {op.ping.ToString()}");
             */
            return Task.CompletedTask;
        }

        void createTable()
        {
            string sql = $"CREATE TABLE IF NOT EXISTS {MySQLClass.TableName} (" +
                                "id INT AUTO_INCREMENT NOT NULL PRIMARY KEY," +
                                "datetime DATETIME(0) NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
                                "download VARCHAR(11), " +
                                "upload VARCHAR(11), " +
                                "ping INT)";

            _mysql.Update(sql);
        }

        Result getResult()
        {
            Logger.Print("Running Script...");

            Result result;
            string pythonPath = _configuration.Config.python_path;
            string script = _configuration.Config.test_script_path;

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = pythonPath;
            startInfo.Arguments = script + " --json";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;

            using (Process process = Process.Start(startInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string jsonResult = reader.ReadToEnd();
                    result = new Result(jsonResult);
                }
            }

            return result;
        }
    }
}
