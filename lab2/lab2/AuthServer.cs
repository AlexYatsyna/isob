using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    class AuthServer
    {
        private List<string> users = new List<string>();

        public AuthServer()
        {
            users.Add("alex");
        }

        public void Listen()
        {
            UdpClient reciever = new UdpClient(Config.asPort);
            Console.WriteLine($"Auth server started on 127.0.0.1:{Config.asPort}");
            IPEndPoint remoteIp = null;
            try
            {
                while (true)
                {
                    byte[] data = reciever.Receive(ref remoteIp);
                    remoteIp.Port = Config.cPort;
                    var answer = new Message();

                    var message = CustomConverter<Message>.Deserialize(Encoding.UTF8.GetString(data));

                    if (message.Type == MessageType.CToAs)
                    {
                        var id = message.Data[0];
                        Console.WriteLine($"Message from {remoteIp.Address}:{remoteIp.Port} AurhServer");
                        Console.WriteLine($"Client : {id}");
                        if (users.Contains(id))
                        {
                            answer.Type = MessageType.AsToC;


                            var ticket = new TicketGranting
                            {
                                Clidentity = id,
                                SIdentity = "tgs",
                                Duration = Config.asTDur.Ticks,
                                Time = DateTime.Now,
                                Key = Config.kCTgs.ToString()
                            };
                            var temp = CustomConverter<TicketGranting>.Serialize(ticket);
                            var ticketEncr = DES.Encrypt(DES.Encrypt( temp , Config.kAsTgs),Config.kC);

                            var kCTgsEncr = DES.Encrypt(Config.kCTgs, Config.kC);
                            Console.WriteLine($"Data from AuthServer to Client: \n\n{ticketEncr} \n {kCTgsEncr}\n");

                            answer.Data.Add(ticketEncr);
         
                            answer.Data.Add(kCTgsEncr);


                        }
                        else
                        {
                            answer.Type = MessageType.AccessDenied;
                            Console.WriteLine("Access denied.");
                        }
                        answer.Send(remoteIp);
                        Console.WriteLine($"Message sended from auth server to {remoteIp.Address}:{remoteIp.Port} ");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}
