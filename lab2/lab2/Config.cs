using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    static class Config
    {
        public static readonly string kC = "dghfghfs";
        public static readonly string kCTgs = "dfgdfg";
        public static readonly string kCSs = "sdfghfg";
        public static readonly string kAsTgs = "sdffgb";
        public static readonly string kTgsSs = "aswerwr";

        public static readonly int cPort = 8000;
        public static readonly int asPort = 8001;
        public static readonly int ssPort = 8002;
        public static readonly int tgsPort = 8003;

        public static readonly TimeSpan asTDur = new TimeSpan(24, 0, 0);
        public static readonly TimeSpan tgsTDur = new TimeSpan(12, 0, 0);

    }
}
