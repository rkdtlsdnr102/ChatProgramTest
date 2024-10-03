using System;
using System.Net;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Chat Server Activated");

            byte[] addr = { 127, 0, 0, 1 };
            int port = 5050;
            IPEndPoint serverEndPoint = new IPEndPoint(new IPAddress(addr), port);

            CServer server = new CServer(serverEndPoint);

            server.Run();
        }
    }
}
