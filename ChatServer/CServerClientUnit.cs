using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class CServerClientUnit
    {
        private Socket _clientSocket;
        private readonly int BufferSize;
        private byte[] Buffer;
        private CServer ChatServer
        {
            get;set;
        }

        public Socket ClientSocket
        {
            get { return _clientSocket; }
            set { _clientSocket = value; }
        }

        public CServerClientUnit(CServer chatServer, Socket clientSocket, int bufferSize = 500)
        {
            ClientSocket = clientSocket;
            BufferSize = bufferSize;
            Buffer = new byte[(long)BufferSize];
            ChatServer = chatServer;
        }

        public void Run()
        {
            if (null == ClientSocket || null == ChatServer)
                return;

            // 비동기로 데이터 read 시작
            ClientSocket.BeginReceive(Buffer, 0, BufferSize, SocketFlags.None, OnDataReceived, null);
        }

        /// <summary>
        /// Data Receive Callback
        /// </summary>
        /// <param name="ar"></param>
        void OnDataReceived( IAsyncResult ar)
        {
            CServerClientUnit curClientUnit = (CServerClientUnit)ar.AsyncState;

            // 읽어온 데이터를 통해 무언가 수행해야한다.
            Broadcast(curClientUnit, Buffer);

            ClientSocket.EndReceive(ar);

            ClientSocket.BeginReceive(Buffer, 0, BufferSize, SocketFlags.None, OnDataReceived, null);
        }

        public void SendMessage( byte[] buffer)
        {
            _clientSocket.Send(buffer);
        }

        private void Broadcast(CServerClientUnit clientUnit, byte[] buffer)
        {
            CServer.s_UserSyncMutex.WaitOne();

            LinkedList<CServerClientUnit> clientUserList = ChatServer.ClientUnitList;

            foreach (CServerClientUnit otherClientUnit in clientUserList)
            {
                if (clientUnit == otherClientUnit)
                    continue;

                otherClientUnit.SendMessage(buffer);
            }

            CServer.s_UserSyncMutex.ReleaseMutex();
        }
    }
}
