using System.IO;
using System;
using Newtonsoft.Json;
using SpeedTestCollector.util;

namespace SpeedTestCollector.configuration
{
    public class Configuration
    {
        private Config _config;
        public Config Config { get { return _config; } }

        private string root = Environment.CurrentDirectory.ToString();

        public Configuration()
        {
            Logger.Print("Lade Configuration...");
            try
            {
                string json = File.ReadAllText(root + "/configuration/config.json");
                _config = JsonConvert.DeserializeObject<Config>(json);

                if (_config.python_path == null)
                {
                    throw new Exception("config.json ist fehlerhaft!");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
    }

    public class Config
    {
        public string python_path { get; set; }
        public string test_script_path { get; set; }
        public Sql mysql { get; set; }
    }

    public class Sql
    {
        public string host { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string database { get; set; }
        public string format { get; set; }
    }
}