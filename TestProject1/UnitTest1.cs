using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Sockets;
using ChatServer;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //서버 키고, client socket을 만들어 로그인 결과 확인
            byte[] addr = { 127, 0, 0, 1 };
            int port = 5050;
            IPEndPoint serverEndPoint = new IPEndPoint(new IPAddress(addr), port);

            CServer server = new CServer(serverEndPoint);

            server.Run();

            // Client Socket 생성

            using Socket clientSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            clientSocket.Connect(serverEndPoint);


            bool blockingState = clientSocket.Blocking;

            bool bTestSuccess = false;

            try
            {
                byte[] tmp = new byte[1];

                clientSocket.Blocking = false;
                clientSocket.Send(tmp, 0, SocketFlags.None);

                Console.WriteLine("Connected");

                bTestSuccess = true;
            }
            catch (SocketException e)
            {
                if (e.NativeErrorCode.Equals(10035))
                {
                    Console.WriteLine("Still Connected, but the send would block");
                }
                else
                {
                    Console.WriteLine("Disconnected: error code {0}!", e.NativeErrorCode);
                }
            }
            finally
            {
                clientSocket.Blocking = blockingState;
            }

            Console.WriteLine("Connected : {0}", clientSocket.Connected);

            Assert.IsTrue(bTestSuccess);
        }
    }
}
