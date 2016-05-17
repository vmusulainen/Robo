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
        private SerialPort _port = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);

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

        public void SendCommand(Command cmd)
        {
            Write(cmd.Create());
        }
    }

    public class Command
    {
        private byte _code;
        private byte[] _data;
        private const byte StartByte = 0xFE;
        private const byte MaxCommandDataLength = 255 - 4;
        
        public Command(byte code, byte[] data)
        {
            if (data.Length > MaxCommandDataLength)
            {
                throw new Exception("Command data should be " + MaxCommandDataLength + " symbols maximum");
            }

            _code = code;
            _data = data;
        }

        public byte[] Create()
        {
            byte len = (byte)(_data.Length + 4); //start byte, length, crc
            var command = new byte[len];
            command[0] = StartByte;
            command[1] = _code;
            command[2] = (byte)(len - 4);

            Buffer.BlockCopy(_data, 0, command, 3, _data.Length);
            command[len - 1] = CalcCRC(command, len-1);

            return command;
        }

        private byte CalcCRC(byte[] data, int len)
        {
            byte crc = 0;
            for (int i = 0; i < len; i++)
            {
                crc += data[i];
            }

            return crc;
        }
    }
}
