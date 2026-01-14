using System;
using System.Collections.Generic;

namespace PlaywrightPoc.Utils
{
    public class StepData
    {
        public string StepName { get; set; } = string.Empty;
        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("o");
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}