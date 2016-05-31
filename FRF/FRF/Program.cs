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
        static void Main(string[] args)
        {
            ComPort port = null;
            try
            {
                port = new ComPort();
                Console.WriteLine("FRF started");

                var thinker = new Thinker(port);
                thinker.MainLoop();

                Console.ReadLine();
                /*while (true)
                {
                    string str = Console.ReadLine();
                    if (str == "exit")
                    {
                        break;
                    }

                    byte degree = Byte.Parse(str);
                    port.SendCommand(Commands.Status, new byte[] { degree });
                }*/

            }
            finally
            {
                if (port != null)
                {
                    port.Close();
                }
                
            }
        }
    }
}
