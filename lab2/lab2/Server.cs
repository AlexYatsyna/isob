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
    class Server
    {
        public void Listen()
        {
            UdpClient reciever = new UdpClient(Config.ssPort);
            Console.WriteLine($"Service server started on 127.0.0.1:{Config.ssPort}");
            IPEndPoint remoteIp = null;
            try
            {
                while (true)
                {
                    byte[] data = reciever.Receive(ref remoteIp);
                    remoteIp.Port = Config.cPort;


                    var message = CustomConverter<Message>.Deserialize(Encoding.UTF8.GetString(data));
                    Console.WriteLine($"Service server recieved from {remoteIp.Address}:{remoteIp.Port}");

                    if (message.Type == MessageType.CToSs)
                    {
                        var tgsJs = DES.Decrypt(message.Data[0], Config.kTgsSs);
                        var tgs = JsonConvert.DeserializeObject<TicketGranting>(tgsJs);

                        var autJs = DES.Decrypt(message.Data[1], Config.kCSs);
                        var aut = JsonConvert.DeserializeObject<TimeMark>(autJs);

                        var answer = new Message();
                        if(DES.CheckTime(tgs.Time, aut.Time, tgs.Duration))
                        {
                            answer.Type = MessageType.SsToC;
                            var timeJs = JsonConvert.SerializeObject(aut.Time.Ticks + 1);
                            var encr = DES.Encrypt(timeJs, Config.kCSs);
                            answer.Data.Add(encr);
                        }
                        else
                        {
                            answer.Type = MessageType.TicketNotValid;
                        }

                        answer.Send(remoteIp);
                        Console.WriteLine($"Message sended from SS to {remoteIp.Address}:{remoteIp.Port}");

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
