using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class CServerClientUnit
    {
        private Socket _clientSocket;
        private readonly int BufferSize;
        private byte[] Buffer;


        public Socket ClientSocket
        {
            get { return _clientSocket; }
            set { _clientSocket = value; }
        }

        public CServerClientUnit(Socket clientSocket, int bufferSize = 500)
        {
            ClientSocket = clientSocket;
            BufferSize = bufferSize;
            Buffer = new byte[(long)BufferSize];
        }

        public void Run()
        {
            if (null == ClientSocket)
                return;

            // 비동기로 데이터 read 시작
            ClientSocket.BeginReceive(Buffer, 0, BufferSize, SocketFlags.None, OnDataReceived, this);

        }

        /// <summary>
        /// Data Receive Callback
        /// </summary>
        /// <param name="ar"></param>
        void OnDataReceived( IAsyncResult ar)
        {
            CServerClientUnit curClientUnit = (CServerClientUnit)ar.AsyncState;

            // 읽어온 데이터를 통해 무언가 수행해야한다.
        }
    }
}
