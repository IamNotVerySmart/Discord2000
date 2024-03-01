using System.Net.Sockets;
using System.Net;
using ConsoleApp1.Net.IO;
namespace ConsoleApp1
{
    internal class Program
    {
        static List<Client> clients;
        static TcpListener _listener;
        static void Main(string[] args)
        {
            clients = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8070);
            _listener.Start();
             while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                clients.Add(client);
                BroadcastConnetion();
            }
        }

        static void BroadcastConnetion()
        {
            foreach (var user in clients)
            {
                foreach (var usr in clients)
                {
                    var brodcastPacket = new Net.IO.PB();
                    brodcastPacket.WriteOpCode(1);
                    brodcastPacket.WriteString(usr.name);
                    brodcastPacket.WriteString(usr.guid.ToString());
                    user.ClientSocket.Client.Send(brodcastPacket.GetPacketBytes());
                    
                }
            }
        }
        public static void BroadcastMessage(string message)
        {
            foreach (var user in clients)
            {
                var msgPacket = new PB();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteString(message);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
        }
        public static void BroadcastDisconect(string uid)
        {
            var disconnectedUser = clients.Where(x => x.guid.ToString() == uid).FirstOrDefault();
            clients.Remove(disconnectedUser);
            foreach (var user in clients)
            {
                var disPacket = new PB();
                disPacket.WriteOpCode(10);
                disPacket.WriteString(uid);
                user.ClientSocket.Client.Send(disPacket.GetPacketBytes());
            }
            BroadcastMessage($"[{disconnectedUser.name}] Disconnected");
        }
    }
}
