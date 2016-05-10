using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

//Fuc... Fantastic robot framework )
namespace FRF
{
    class Program
    {
        private const byte StartByte = 0xFE;

        static void Main(string[] args)
        {
            ComPort port = null;
            try
            {
                port = new ComPort();
                while (true)
                {
                    string str = Console.ReadLine();
                    if (str == "exit")
                    {
                        break;
                    }

                    port.SendCommand(1, GetBytes(str));
                }
               
            }
            finally
            {
                if (port != null)
                {
                    port.Close();
                }
                
            }
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

    }
}
