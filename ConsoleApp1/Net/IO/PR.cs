using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;

namespace ConsoleApp1.Net.IO
{
    class PR : BinaryReader
    {
        private NetworkStream _stream;
        public PR(NetworkStream ns) : base(ns)
        {
            _stream = ns;
        }

        public string ReadMessage()
        {
            byte[] buffer;
            var length = ReadInt32();
            buffer = new byte[length];
            _stream.Read(buffer, 0, length);

            var msg = Encoding.ASCII.GetString(buffer);
            return msg;
        }
    }
}
