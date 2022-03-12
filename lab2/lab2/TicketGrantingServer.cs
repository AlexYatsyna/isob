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
    class TicketGrantingServer
    {
        public void Listen()
        {
            UdpClient reciever = new UdpClient(Config.tgsPort);
            Console.WriteLine($"Tgs started on 127.0.0.1:{Config.tgsPort}");
            IPEndPoint remoteIp = null;
            try
            {
                while (true)
                {
                    byte[] data = reciever.Receive(ref remoteIp);
                    remoteIp.Port = Config.cPort;

                    var message = CustomConverter<Message>.Deserialize(Encoding.UTF8.GetString(data));

                    if (message.Type == MessageType.CToTgs)
                    {

                        var tgtJs = DES.Decrypt(message.Data[0], Config.kAsTgs);
                        var tgt = JsonConvert.DeserializeObject<TicketGranting>(tgtJs);

                        var autJs = DES.Decrypt(message.Data[1], Config.kCTgs);
                        var aut = JsonConvert.DeserializeObject<TimeMark>(autJs);

                        var id = message.Data[2];
                        Console.WriteLine($"Recieved data from cliet to tgs: \n\n{message.Data[0]} \n {message.Data[1]} \n {message.Data[2]}\n");

                        var answer = new Message();

                        if (tgt.Clidentity == aut.Cl)
                        {
                            if (DES.CheckTime(tgt.Time, aut.Time, tgt.Duration))
                            {
                                answer.Type = MessageType.TgsToC;

                                var ticket = new TicketGranting()
                                {
                                    Clidentity = aut.Cl,
                                    SIdentity = id,
                                    Duration = Config.tgsTDur.Ticks,
                                    Time = DateTime.Now,
                                    Key = Config.kCSs
                                };

                                var ticketEncr =DES.Encrypt( DES.Encrypt(JsonConvert.SerializeObject(ticket), Config.kTgsSs),Config.kCTgs);
                                var kCSsEncr = DES.Encrypt(Config.kCSs, Config.kCTgs);
                                Console.WriteLine($"Data from TGS to Client : \n\n {ticketEncr} \n {kCSsEncr} \n");

                                answer.Data.Add(ticketEncr);
                                answer.Data.Add(kCSsEncr);
                            }
                            else
                            {
                                answer.Type = MessageType.TicketNotValid;
                                Console.WriteLine($"Ticket not validin tgs");
                            }
                        }
                        else
                        {
                            answer.Type = MessageType.AccessDenied;
                            Console.WriteLine("Access denied in tgs.");
                        }
                        answer.Send(remoteIp);
                        Console.WriteLine($"Message sended from tgs server to {remoteIp.Address}:{remoteIp.Port} ");

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
