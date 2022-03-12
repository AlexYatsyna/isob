using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new Server();
            var client = new Client();
            var attack = new attack();

            try
            {
                Task.Run(() => server.Listen());
                Thread.Sleep(5000);
                Task.Run(() => attack.Listen(1000));
                Task.Run(() => client.Listen());
                Task.Run(() => attack.Listen(2000));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadKey();
        }
    }
}
