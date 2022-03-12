using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    [SerializableAttribute]
    public class CMessage
    {
        public DateTime Time { get; set; }
        public string SourceAddr { get; set; }
        public string DestinationAddr { get; set; }
        public string Message { get; set; }
        public int sourcePort { get; set; }
        public int destinationPort { get; set; }
        public bool syn { get; set; }
        public bool ack { get; set; }

        public CMessage()
        { 
        }
        public CMessage(string msg)
        {
            Time = DateTime.Now;
            Message = msg;
        }

    }
}
