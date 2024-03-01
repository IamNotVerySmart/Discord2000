using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Serverek.IO
{
    class PB
    {
        MemoryStream _ms;
        public PB()
        {
            _ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteString(string str)
        {
            var msgLenght = str.Length;
            _ms.Write(BitConverter.GetBytes(msgLenght));
            _ms.Write(Encoding.ASCII.GetBytes(str));
        }

        public byte[] GetPacketBytes() 
        {
            return _ms.ToArray();
        }
    }
}
