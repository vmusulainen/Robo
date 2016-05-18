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
                var thinker = new Thinker();

                thinker.MainLoop();

                /*while (true)
                {
                    string str = Console.ReadLine();
                    if (str == "exit")
                    {
                        break;
                    }

                    port.SendCommand(Commands.Status, new byte[] { });
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
