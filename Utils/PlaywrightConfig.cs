using System;
using System.Collections.Generic;
using System.Text;

namespace PlaywrightPoc.Utils
{
    public class PlaywrightConfig
    {
        public string Channel {  get; set; }
        public bool Headless { get; set; }
        public string Args {  get; set; }
    }
}
