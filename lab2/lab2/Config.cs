using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    static class Config
    {
        public static readonly string kC = "K_CCCC";
        public static readonly string kCTgs = "K_C_TGS";
        public static readonly string kCSs = "K_C_SS";
        public static readonly string kAsTgs = "K_AS_TGS";
        public static readonly string kTgsSs = "K_TGS_SS";

        public static readonly int cPort = 8000;
        public static readonly int asPort = 8001;
        public static readonly int ssPort = 8002;
        public static readonly int tgsPort = 8003;

        public static readonly TimeSpan asTDur = new TimeSpan(24, 0, 0);
        public static readonly TimeSpan tgsTDur = new TimeSpan(12, 0, 0);

        public static readonly string filepath = @"d:\out.txt";

    }
}
