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
        public Output(double download, double upload, double ping)
        {
            this.download = download;
            this.upload = upload;
            this.ping = ping;
        }

        public double download { get; set; }
        public double upload { get; set; }
        public double ping { get; set; }

        public static Output ConvertOutput(Output results)
        {
            double download = Math.Round(results.download / 1000000, 2);
            double upload = Math.Round(results.upload / 1000000, 2);
            double ping = Math.Round(results.ping, 0);

            return new Output(download, upload, ping);
        }
    }
}