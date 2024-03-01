using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ConsoleApp1.Net.IO;

namespace ConsoleApp1
{
    internal class Client
    {
        public string name {  get; set; }
        public Guid guid { get; set; }
        public TcpClient ClientSocket { get; set; }

        PR _packetReader;
        public Client( TcpClient client) 
        {
            ClientSocket = client;
            guid = Guid.NewGuid();
            _packetReader = new PR(ClientSocket.GetStream());
            var opcode = _packetReader.ReadByte();
            name = _packetReader.ReadMessage();

            Task.Run(() => Process());
        }

        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode= _packetReader.ReadByte();
                    switch(opcode)
                    {
                        case 5:
                            var msg = _packetReader.ReadMessage();
                            Program.BroadcastMessage($"[{DateTime.Now}]: [{name}]: {msg}");
                            break;
                        default:
                            break;
                    }
                }
                catch 
                { 
                    Program.BroadcastDisconect(guid.ToString());
                    ClientSocket.Close();
                    break;
                }

            }
        }
    }
}
