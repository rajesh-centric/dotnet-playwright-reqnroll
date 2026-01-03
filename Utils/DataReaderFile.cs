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
            var data = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<ApplicationData>>(data);
        }
    }
}
