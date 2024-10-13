using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatServer
{
    class Logger
    {
        public static void PrintLog( string log)
        {
            string logMsg = String.Format("[{0}]{1}", DateTime.UtcNow.ToString("yy/MM/DD-hh:mm:ss"), log);

            Console.WriteLine(logMsg);
        }
    }

    public class CServer
    {
        public static Mutex s_UserSyncMutex = new Mutex();

        private LinkedList<CServerClientUnit> _clientUnitList;
        private IPEndPoint _serverEndPoint;
        private Socket _serverSocket;
        private string _log;

        public IPEndPoint ServerEndPoint
        {
            get { return _serverEndPoint; }
            set { _serverEndPoint = value; } 
        }

        public Socket ServerSocket
        {
            get
            {
                return _serverSocket;
            }

            set
            {
                _serverSocket = value;
            }
        }

        public LinkedList<CServerClientUnit> ClientUnitList
        {
            get
            {
                return _clientUnitList;
            }

            set
            {
                _clientUnitList = value;
            }
        }      

        public CServer( IPEndPoint serverEndPoint )
        {
            ServerEndPoint = serverEndPoint;
            ServerSocket = null;
            ClientUnitList = new LinkedList<CServerClientUnit>();
        }

        public void Run()
        {
            Close();

            try
            {
                ServerSocket = new(ServerEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                ServerSocket.Bind(ServerEndPoint);
                ServerSocket.Listen(100);

                while (true)
                {
                    Socket clientSocket = ServerSocket.Accept();

                    CServerClientUnit newClientUnit = new CServerClientUnit(this, clientSocket);

                    AddUser(newClientUnit);

                    newClientUnit.Run();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error:{0}", e.ToString());
            }

            //https://hihaoun.tistory.com/entry/c%EC%9D%84-%EC%9D%B4%EC%9A%A9%ED%95%9C-TCPIP-%EC%84%9C%EB%B2%84-%ED%81%B4%EB%9D%BC%EC%9D%B4%EC%96%B8%ED%8A%B8-%EC%86%8C%EC%BC%93Socket%ED%86%B5%EC%8B%A0          

        }

        public void Close()
        {
            if( null != ServerSocket)
            {
                ServerSocket.Close();
                ServerSocket.Dispose();
            }

            foreach (CServerClientUnit clientUnit in ClientUnitList)
            {
                // 클라이언트와의 소켓 접속을 모두 닫아야함
            }
        }

        public void AddUser( CServerClientUnit clientUnit)
        {
            s_UserSyncMutex.WaitOne();

            ClientUnitList.AddLast(clientUnit);

            s_UserSyncMutex.ReleaseMutex();
        }

        private void AcceptCallback(IAsyncResult ar )
        {
            
        }

       
    }
}
