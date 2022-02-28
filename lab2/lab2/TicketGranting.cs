using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    class TicketGranting
    {
        public string Clidentity { get; set; }
        public string SIdentity { get; set; }
        public DateTime Time { get; set; }
        public long Duration { get; set; }
        public string Key  { get; set; }
    }
}
