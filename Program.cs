using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace dummy_tcp_client
{
    class Program
    {
        static void Main(string[] args)
        {
            DummyTCPClient dummy = new DummyTCPClient((IPAddress.Parse("192.168.140.121"), 2000));
            while (!dummy.Connected)
            { 
                try
                {
                    dummy.CreateDummyConnection();
                   
                }
                catch (SocketException e)
                {
                    Console.WriteLine("socketException happend {}", e.ToString());
                    Thread.Sleep(3000);
                    Console.WriteLine("trying connectino again");
                }
            }
            Console.WriteLine(dummy.send_and_recv("hello"));
            dummy.manual_mode();
            Console.Read();
        }
    }
}
