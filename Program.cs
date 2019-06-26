using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using SpeedTestCollector.configuration;
using SpeedTestCollector.util;

namespace SpeedTestCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Print("Starting...");
            new Program().MainTask().GetAwaiter().GetResult();
        }

        private Configuration _configuration;

        public Program() { }

        public async Task MainTask()
        {
            _configuration = new Configuration();

            Result result = getResult();
            Logger.Print($"Download: {result.Results.download.ToString()}");
            Logger.Print($"Upload: {result.Results.upload.ToString()}");
            Logger.Print($"Ping: {result.Results.ping.ToString()}");
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
