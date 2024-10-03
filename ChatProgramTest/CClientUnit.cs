using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    class CClientUnit
    {
        private Socket _clientSocket;
        private int _bufferSize;
        private byte[] _buffer;

        public Socket ClientSocket
        {
            get { return _clientSocket; }
            set { _clientSocket = value; }
        }

        private  CClientUnit()
        {
           
        }

        private static CClientUnit _instance;

        public static CClientUnit Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new CClientUnit();
                }

                return _instance;
            }
        }

        public void Run( Socket clientSocket, int bufferSize )
        {
            ClientSocket = clientSocket;
            _bufferSize = bufferSize;
            _buffer = new byte[_bufferSize];

            // 비동기로 데이터 read 시작
            ClientSocket.BeginReceive(_buffer, 0, _bufferSize, SocketFlags.None, OnDataReceived, this);

        }

        /// <summary>
        /// Data Receive Callback
        /// </summary>
        /// <param name="ar"></param>
        void OnDataReceived(IAsyncResult ar)
        {
            CClientUnit curClientUnit = (CClientUnit)ar.AsyncState;

            // 읽어온 데이터를 통해 무언가 수행해야한다.
        }
    }
}
