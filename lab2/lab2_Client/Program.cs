using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab2_Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();
            try
            {
                Task.Run(() => client.Listen());
                Task.Run(() => client.Reg("alex"));
                Thread.Sleep(5000);
                Task.Run(() => client.GetPerm());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadKey();

        }
    }
}
