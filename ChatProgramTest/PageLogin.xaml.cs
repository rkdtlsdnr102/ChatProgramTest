using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_EnterChat_Click(object sender, RoutedEventArgs e)
        {
            // ID 입력 대기
            InsertIdWindow dlg = new InsertIdWindow();

            dlg.Owner = this;

            dlg.ShowDialog();

            if (false == dlg.DialogResult)
            {
                return;
            }

            using Socket clientSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress serverAddr = new IPAddress(new byte[] { 127, 0, 0, 1 });
            IPEndPoint clientEp = new IPEndPoint(serverAddr, 5050);

            clientSocket.Connect(clientEp);

            CClientUnit clientUnit = CClientUnit.Instance;

            // 서버와 통신 시작
            clientUnit.Run(clientSocket, 500);

        }
    }
}
