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
                while (true)
                {
                    string str = Console.ReadLine();
                    if (str == "")
                    {
                        break;
                    }

                    port.Write(str);
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
    }
}
