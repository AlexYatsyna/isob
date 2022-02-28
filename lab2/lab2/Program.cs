using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(DES.Decrypt(DES.Encrypt("Do you Know {{\\}|What's Fighting for When It's Not Worth Dying for?", "чр3ъ")));
            //Console.ReadKey();
            var authServer = new AuthServer();
            var tgs = new TicketGrantingServer();
            var server = new Server();

            try
            {
                Task.Run(() => authServer.Listen());
                Task.Run(() => tgs.Listen());
                Task.Run(() => server.Listen());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadKey();
        }
    }
}
