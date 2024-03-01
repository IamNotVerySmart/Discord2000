using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Serverek.IO;

namespace WpfApp1.Serverek
{
     public class Server
     {
        TcpClient tcpClient;
        public PR PR;

        public event Action connectedEvent;
        public event Action MsgEvent;
        public event Action DisEvent;
        public Server()
        {
            tcpClient = new TcpClient();
        }

        public void CTS(string username)
        {
            if (!tcpClient.Connected) 
            {
                tcpClient.Connect("127.0.0.1", 8070);
                PR = new PR(tcpClient.GetStream());
                if (!string.IsNullOrEmpty(username))
                {
                    var ConnectPackage = new PB();
                    ConnectPackage.WriteOpCode(0);
                    ConnectPackage.WriteString(username);
                    tcpClient.Client.Send(ConnectPackage.GetPacketBytes());
                }
                ReadPackets();
            }
        }
        public void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = PR.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        case 5:
                            MsgEvent?.Invoke();
                            break;
                        case 10:
                            DisEvent?.Invoke();
                            break;
                        default: break;
                    }
                }
            });
        }
        public void SendMessageToServer(string message) 
        {
            var messagePacket = new PB();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteString(message);
            tcpClient.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
