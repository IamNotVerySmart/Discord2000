using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Serverek.IO
{
    public class PR : BinaryReader
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
