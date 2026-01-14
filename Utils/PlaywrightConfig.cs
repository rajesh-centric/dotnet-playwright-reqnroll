using System;
using System.Collections.Generic;
using System.Text;

namespace PlaywrightPoc.Utils
{
    public class PlaywrightConfig
    {
        // Browser options
        public string? Channel { get; set; }
        public bool Headless { get; set; }
        public string? Args { get; set; }

        // Failure artifacts (configurable — no hard-coded values in code)
        public bool TraceOnFailure { get; set; } = false;
        public string? TracesDir { get; set; }

        public bool ScreenshotsOnFailure { get; set; } = true;
        public string? ScreenshotDir { get; set; }

        // Optional trace filename format (use {scenario} and {timestamp})
        public string? TraceFileNameFormat { get; set; }
        public string? ScreenshotFileNameFormat { get; set; }

        // Test data masking for reporting (no hard-coded sensitive values)
        public bool MaskSensitiveData { get; set; } = true;
        public string[]? SensitiveKeys { get; set; }
        public string? BaseURL { get; set; }
    }
}
