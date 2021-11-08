using System;
using System.Net.Sockets;
using System.Net;

namespace dummy_tcp_client
{
    class DummyTCPClient
    {
        private (IPAddress ip, int port) connection_info;
        private TcpClient client = null;
        private NetworkStream Connection = null;
        public bool Connected = false;

        public DummyTCPClient((IPAddress ip, int port) conn_info)
        {
            this.connection_info.ip = conn_info.ip;
            this.connection_info.port = conn_info.port;
        }

        public void CreateDummyConnection()
        {
            client = new TcpClient(connection_info.ip.ToString(), connection_info.port);
            if (client.Connected)
            {
                this.Connected = true;
                Connection = client.GetStream();
                Console.WriteLine("Connected to host {0} at port {1}", ((IPEndPoint)(client.Client.RemoteEndPoint)).Address, ((IPEndPoint)(client.Client.RemoteEndPoint)).Port);
                Console.WriteLine("Local connection info: ip: {0} port: {1}", ((IPEndPoint)(client.Client.LocalEndPoint)).Address, ((IPEndPoint)(client.Client.LocalEndPoint)).Port);
            }
            else
            {
                Console.WriteLine("Could not connect to host at {0}:{1}", connection_info.ip.ToString(), connection_info.port);
                
            }
        }

        public string send_and_recv(string msg)
        {
            Byte[] msg_ascii = System.Text.Encoding.ASCII.GetBytes(msg);
            Byte[] response_ascii = new byte[256];
            string response_string;

            if (client.Connected)
            {
                Connection.Write(msg_ascii,0,msg_ascii.Length);
                int bytes = Connection.Read(response_ascii, 0, response_ascii.Length);
                response_string = System.Text.Encoding.ASCII.GetString(response_ascii, 0, bytes);
                return response_string;
            } else
            {
                return "client has disconnected";
            }
        }

        public void manual_mode()
        {
            string input = String.Empty;
            while (true)
            {
                Console.WriteLine("What do you want to send");
                input = Console.ReadLine();
                if(input == "")
                {
                    break;
                }

                Console.WriteLine(send_and_recv(input));
            }
            Console.WriteLine("Closed Connection");
            Connection.Close();
            client.Close();
        }

        ~DummyTCPClient()
        {
            Connection.Close();
            client.Close();
        }

    }
}
