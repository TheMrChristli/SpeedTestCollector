using System;
using Newtonsoft.Json;

namespace SpeedTestCollector.util
{
    public class Result
    {
        private Output _result;
        public Output Results { get { return _result; } }
        public Result(string json)
        {
            _result = JsonConvert.DeserializeObject<Output>(json);
        }
    }

    public class Output
    {
        public double download { get; set; }
        public double upload { get; set; }
        public double ping { get; set; }
    }
}