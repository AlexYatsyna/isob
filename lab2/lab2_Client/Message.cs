using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public enum MessageType
    {
        CToAs,
        AsToC,
        CToTgs,
        TgsToC,
        CToSs,
        SsToC,
        TicketNotValid,
        AccessDenied

    }
    public class Message
    {
        public MessageType Type { get; set; }
        public List<string> Data { get; set; }

        public Message()
        {
            Data = new List<string>();
        }

        public Message(MessageType messageType = 0)
        {
            Data = new List<string>();
            Type = messageType;
        }

        public void Send(IPEndPoint remoteIp)
        {
            UdpClient sender = new UdpClient();
            try
            {
                var js = JsonConvert.SerializeObject(this);
                byte[] dgram = Encoding.UTF8.GetBytes(js);
                sender.Send(dgram, dgram.Length, remoteIp);
            }
            finally
            {
                sender.Close();
            }
        }
    }
}
