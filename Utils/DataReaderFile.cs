using PlaywrightPoc.TestData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PlaywrightPoc.Utils
{
    public static class DataReaderFile
    {
        public static List<PlaywrightConfig> GetJsonData(string path)
        {
            var data = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<PlaywrightConfig>>(data);
        }
        public static List<ApplicationData> GetAppData(string path)
        {

            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip
            };

            // Deserialize your JSON string using these options
            var data = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<ApplicationData>>(data,options);
        }

        public static List<EnvironmentData> GetEnvironmentData(string path)
        {
            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip
            };

            var data = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<EnvironmentData>>(data, options);

        }
    }
}
