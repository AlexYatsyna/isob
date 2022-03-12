using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab3
{
    public class Client
    {
        public void Listen()
        {
            try
            {
                var asourcePort = 8080;
                var answer = new CMessage();

                for (int i = 0; i < 4;)
                {
                    var endPoint = new IPEndPoint(IPAddress.Parse(Config.serverAddress), Config.serverPort);
                    var socketTCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socketTCP.Connect(endPoint);

                    var message = new CMessage()
                    {
                        SourceAddr = Config.fakeAddress,
                        sourcePort = asourcePort,
                        Message = $"Data from client {i}",
                        Time = DateTime.Now,
                        DestinationAddr = Config.serverAddress,
                        destinationPort = Config.serverPort,
                        syn = true,
                        ack = false,
                    };

                    if (answer.ack)
                    {
                        message.sourcePort = answer.destinationPort;
                        message.Message = answer.Message;
                        message.syn = false;
                        message.ack = true;
                        asourcePort--;
                        i--;
                    }

                    socketTCP.Send(CustomConverter.Serialize(message));

                    var buffer = new byte[1024];
                    var size = 0;

                    do
                    {
                        size = socketTCP.Receive(buffer);
                        var obj = CustomConverter.Deserialize(buffer);
                        if (obj is CMessage)
                            answer = (CMessage)obj;
                    }
                    while (socketTCP.Available > 0);

                    i++;
                    asourcePort++;

                    socketTCP.Shutdown(SocketShutdown.Both);
                    socketTCP.Close();
                    if(i > 1)
                        Thread.Sleep(500);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
