using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PlaywrightPoc.TestData;

namespace PlaywrightPoc.Utils
{
    public static class EnvironmentManager
    {
        private const string DefaultEnv = "dev";
        public static EnvironmentData Current { get; private set; }

        public static void Load(string filePath)
        {
            var envs = DataReaderFile.GetEnvironmentData(filePath);
            string envName = null;
            // try NUnit test parameter first
            try
            {
                envName = TestContext.Parameters.Get("ENV", null);
            }
            catch { }

            // fallback to environment variable
            if (string.IsNullOrEmpty(envName))
            {
                envName = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT");
            }

            if (string.IsNullOrEmpty(envName))
                envName = DefaultEnv;

            Current = envs?.Find(e => e.Name.Equals(envName, StringComparison.OrdinalIgnoreCase))
                      ?? envs?.Find(e => e.Name.Equals(DefaultEnv, StringComparison.OrdinalIgnoreCase));

            if (Current == null && envs?.Count > 0)
            {
                var e = envs[0];
                Current = new EnvironmentData { Name = e.Name, AppUrl = e.AppUrl, UserName = e.UserName, Password = e.Password };
            }
        }
    }
}