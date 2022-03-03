
using lab2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace lab2_Client
{
    class Client
    {
        public string Login { get; set; }
        private string Tgt { get; set; }
        private string Tgs { get; set; }
        private string KCTgs { get; set; }
        private string KCSs { get; set; }
        private DateTime Time { get; set; }
        private readonly IPEndPoint AsEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Config.asPort);
        private readonly IPEndPoint SsEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Config.ssPort);
        private readonly IPEndPoint TgsEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Config.tgsPort);

        public void Reg(string login)
        {
            Login = login;
            var message = new Message(MessageType.CToAs);
            message.Data.Add(Login);
            message.Send(AsEP);
        }

        public void GetPerm()
        {
            if(KCTgs != null && Tgt!=null)
            {
                var message = new Message(MessageType.CToTgs);
                message.Data.Add(Tgt);

                var mark = new TimeMark()
                { 
                    Cl = Login,
                    Time = DateTime.Now
                };
                var aut = JsonConvert.SerializeObject(mark).ToString();
                message.Data.Add(DES.Encrypt(aut, KCTgs));
                message.Data.Add("SS_ID");

                message.Send(TgsEP);

            }
        }

        public void Listen()
        {
            var reciever = new UdpClient(Config.cPort);
            IPEndPoint remoteIp = null;

            try
            {
                while(true)
                {
                    byte[] data = reciever.Receive(ref remoteIp);
                    var message = CustomConverter<Message>.Deserialize(Encoding.UTF8.GetString(data));
                    switch (message.Type)
                    {
                        case MessageType.AsToC:
                            Tgt = DES.Decrypt(message.Data[0], Config.kC);
                            KCTgs = DES.Decrypt(message.Data[1], Config.kC);
                            for(int i = KCTgs.Length-1;i>0;i--)
                            {
                                if(!KCTgs[i].Equals(" "))
                                {
                                    KCTgs = KCTgs.Remove(i);
                                    break;
                                }
                            }
                            
                            Console.WriteLine("AS to C successfull");
                            
                            break;
                        case MessageType.TgsToC:
                            Tgs = DES.Decrypt(message.Data[0], KCTgs);
                            KCSs = DES.Decrypt(message.Data[1], KCTgs);
                            for (int i = KCSs.Length - 1; i > 0; i--)
                            {
                                if (!KCSs[i].Equals(" "))
                                {
                                    KCSs = KCSs.Remove(i);
                                    break;
                                }
                            }
                            var locMessage = new Message(MessageType.CToSs);
                            locMessage.Data.Add(Tgs);
                            var mark = new TimeMark()
                            {
                                Cl = Login,
                                Time = DateTime.Now
                            };
                            Time = mark.Time;
                            var aut = JsonConvert.SerializeObject(mark);
                            locMessage.Data.Add(DES.Encrypt(aut, KCSs));
                            locMessage.Send(SsEP);
                            Console.WriteLine("TGS to C successfull");
                            break;
                        case MessageType.SsToC:
                            var time = DES.Decrypt(message.Data[0], KCSs);
 
                            var forCheck = JsonConvert.DeserializeObject<long>(time);
                            if (Time.Ticks + 1 == forCheck)
                            {
                                Console.WriteLine("successfull");
                            }
                            break;
                        case MessageType.TicketNotValid:
                            Console.WriteLine("Ticket is not valid");
                            break;
                        case MessageType.AccessDenied:
                            Console.WriteLine("Access denied");
                            break;
                        default:
                            Console.WriteLine("Something wrong in type message");
                            break;

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
