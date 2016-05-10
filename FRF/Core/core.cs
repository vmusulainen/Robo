using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Core
{
    public class Core
    {
    }

    public class ComPort
    {
        private const byte StartByte = 0xFE, MaxCommandDataLength = 254;
        private SerialPort _port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);

        public ComPort()
        {
            _port.Open();
            _port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            // Write a string
            //_port.Write("Hello World");

            // Write a set of bytes
            //_port.Write(new byte[] { 0x0A, 0xE2, 0xFF }, 0, 3);

        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Show all the incoming data in the port's buffer
            Console.WriteLine(_port.ReadExisting());
        }

        public void Close()
        {
             _port.Close();
        }

        public void Write(string str)
        {
            _port.Write(str);
        }

        public void Write(byte[] data)
        {
            _port.Write(data, 0, data.Length);
        }

        public void SendCommand(byte commandCode, byte[] data)
        {
            if (data.Length > MaxCommandDataLength)
            {
                throw new Exception("Command data should be " + MaxCommandDataLength + "symbols maximum");
            }
            
            byte[] buffer = {StartByte, commandCode, (byte)data.Length};
            var cmd = Combine(new byte[][] {buffer, data});
            Write(cmd);
        }

        private byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
    }
}
