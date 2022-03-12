using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    internal class Server
    {
        private List<int> synCon = new List<int>();
        private int maxQueue = 5;
        public void Listen()
        {
            var endPoint = new IPEndPoint(IPAddress.Parse(Config.serverAddress), Config.serverPort);
            var currentSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                currentSocket.Bind(endPoint);
                currentSocket.Listen(5);
                
                while (true)
                {
                    var handler = currentSocket.Accept();
                    var buffer = new byte[1024];
                    var size = 0;
                    var data = new CMessage();

                    do
                    {
                        size = handler.Receive(buffer);
                        var obj = CustomConverter.Deserialize(buffer);
                        if (obj is CMessage)
                            data = (CMessage)obj;
                    }
                    while (handler.Available > 0);

                    var msg = new CMessage()
                    {
                        SourceAddr = data.DestinationAddr,
                        sourcePort = data.destinationPort,
                        Message = "",
                        DestinationAddr = data.SourceAddr,
                        destinationPort = data.sourcePort,
                        syn = false,
                        ack = false
                    };

                    if (data.syn && !synCon.Contains(data.sourcePort))
                    {
                        msg.SourceAddr = data.DestinationAddr;
                        msg.sourcePort = data.destinationPort;
                        msg.Message = data.Message;
                        msg.Time = data.Time;
                        msg.DestinationAddr = data.SourceAddr;
                        msg.destinationPort = data.sourcePort;
                        msg.syn = false;
                        msg.ack = true;

                        if (synCon.Count < maxQueue)
                        {
                            synCon.Add(data.sourcePort);
                            Console.WriteLine($"{data.sourcePort} syn");
                        }
                        else
                        {
                            Console.WriteLine($"{data.SourceAddr}:{data.sourcePort} failed to enqueue syn(max queue)");
                        }
                    }
                    else if (data.ack && synCon.Contains(data.sourcePort))
                    {
                        synCon.Remove(data.sourcePort);
                        Console.WriteLine($"{data.SourceAddr}:{data.sourcePort} connected successfully.");

                    }
                    handler.Send(CustomConverter.Serialize(msg));

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
            finally
            {
                currentSocket.Shutdown(SocketShutdown.Both);
                currentSocket.Close();
            }
        }
    }


}
