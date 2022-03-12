using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    public class attack
    {
        public void Listen(int fakePort)
        {
            try
            {
                for (int i = 0; i < 30;)
                {
                    var endPoint = new IPEndPoint(IPAddress.Parse(Config.serverAddress), Config.serverPort);
                    var socketTCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socketTCP.Connect(endPoint);

                    var message = new CMessage()
                    {
                        SourceAddr = Config.fakeAddress,
                        sourcePort = fakePort,
                        Message = "fake",
                        Time = DateTime.Now,
                        DestinationAddr = Config.serverAddress,
                        destinationPort = Config.serverPort,
                        syn = true,
                        ack = false,
                    };

                    socketTCP.Send(CustomConverter.Serialize(message));

                    fakePort++;
                    i++;

                    socketTCP.Shutdown(SocketShutdown.Both);
                    socketTCP.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
