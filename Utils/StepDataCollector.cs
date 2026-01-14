using Allure.Net.Commons;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PlaywrightPoc.Utils
{
    public static class StepDataCollector
    {
        private const string Key = "stepData";

        public static void Init(ScenarioContext ctx)
        {
            if (!ctx.ContainsKey(Key))
                ctx.Add(Key, new List<StepData>());
        }

        public static void Add(ScenarioContext ctx, string stepName, string dataKey, string dataValue)
        {
            if (ctx == null) return;

            Init(ctx);
            var list = ctx.Get<List<StepData>>(Key);
            var now = DateTime.UtcNow;
            var last = list.LastOrDefault();

            // reuse last entry if same step within 5s, otherwise create new
            StepData target;
            if (last == null || !string.Equals(last.StepName, stepName, StringComparison.OrdinalIgnoreCase) || (DateTime.Parse(last.Timestamp) - now).Duration() > TimeSpan.FromSeconds(5))
            {
                target = new StepData { StepName = stepName, Timestamp = now.ToString("o") };
                list.Add(target);
            }
            else
            {
                target = last;
            }

            // Apply optional masking using PlaywrightConfig stored in scenario context
            if (ctx.ContainsKey("playwrightConfig"))
            {
                var cfg = ctx.Get<PlaywrightConfig>("playwrightConfig");
                if (cfg != null && cfg.MaskSensitiveData)
                {
                    var sensitive = (cfg.SensitiveKeys ?? Array.Empty<string>()).Any(k => dataKey.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0);
                    if (sensitive || dataKey.IndexOf("password", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        dataValue = "****";
                    }
                }
            }

            target.Data[dataKey] = dataValue;

            // Attach per-step JSON to Allure immediately so it appears under the step (no hard-coded paths)
            try
            {
                var cfg = ctx.ContainsKey("playwrightConfig") ? ctx.Get<PlaywrightConfig>("playwrightConfig") : null;
                var artifactsDir = cfg?.TracesDir ?? "Traces";
                var baseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".";
                Directory.CreateDirectory(Path.Combine(baseDir, artifactsDir));
                var scenarioName = ctx.ScenarioInfo?.Title ?? "scenario";
                var fileName = SanitizeFileName(scenarioName) + "_" + SanitizeFileName(stepName) + "_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmssfff") + ".json";
                var path = Path.Combine(baseDir, artifactsDir, fileName);
                var single = JsonSerializer.Serialize(new { step = stepName, key = dataKey, value = dataValue, timestamp = now.ToString("o") }, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, single);
                AllureApi.AddAttachment($"Step Data: {stepName}", "application/json", path);

                // Also try to add a test-level parameter so the value is immediately visible in the Allure 'Parameters' table.
                try
                {
                    // Use AllureLifecycle if available — wrap in try/catch to avoid hard dependency failures
                    var lifecycleType = typeof(AllureApi).Assembly.GetType("Allure.Net.Commons.AllureLifecycle");
                    var instanceProp = lifecycleType?.GetProperty("Instance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    var instance = instanceProp?.GetValue(null);
                    var addParam = lifecycleType?.GetMethod("AddParameter", new[] { typeof(string), typeof(string) });
                    if (instance != null && addParam != null)
                    {
                        addParam.Invoke(instance, new object[] { stepName + ":" + dataKey, dataValue });
                    }
                }
                catch { /* swallow - reporting enhancement only */ }
            }
            catch { /* non-fatal, do not fail test because reporting failed */ }
        }

        public static string ToJson(ScenarioContext ctx)
        {
            if (ctx == null || !ctx.ContainsKey(Key)) return "[]";
            var list = ctx.Get<List<StepData>>(Key);
            return JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        }

        private static string SanitizeFileName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "artifact";
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name.Replace(' ', '_');
        }
    }
}