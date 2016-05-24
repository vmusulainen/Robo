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
using Core;

namespace Core
{
    class ReadedCommandData
    {
        public byte Code { get; set; }
        public byte Len { get; set; }
        public byte PosInCommand { get; set; }
        public byte[] Data;
        public byte Crc { get; set; }

        public ReadedCommandData()
        {
            Len = 0;
            Code = 0;
            PosInCommand = 0;
            Data = new byte[ComPort.MaxCommandDataLength];
            Crc = 0;
        }
    }

    public class ComPort
    {
        private SerialPort _port = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        public const byte StartByte = 0xFE;
        public const byte MaxCommandDataLength = 255 - 4;
        private bool _readingCommand = false;
        private ReadedCommandData _commandData;

        public delegate void ReceivedCommandHandler(BasicResponce cmd);
        public event ReceivedCommandHandler ReceivedResponce;

        public ComPort()
        {
            _port.Open();
            _port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int bytes = _port.BytesToRead;
            byte[] buffer = new byte[bytes];
            _port.Read(buffer, 0, bytes);

            //Console.Write("Data received: ");
            foreach (byte bt in buffer)
            {
                //Console.Write(bt.ToString("X2"));
                //Console.Write(" ");
                if (_readingCommand)
                {
                    ProcessReadedByte(bt);
                }
                else
                {
                    if (bt == StartByte)
                    {
                        _readingCommand = true;
                        _commandData = new ReadedCommandData();
                    }
                }
            }

            //Console.WriteLine(" ");
        }

        public void ProcessReadedByte(byte bt)
        {
            switch (_commandData.PosInCommand)
            {
                case 0:
                    _commandData.Code = bt;
                    break;
                case 1:
                    _commandData.Len = bt;
                    break;
                default:
                    //data
                    if (_commandData.PosInCommand < _commandData.Len + 2)
                    {
                        _commandData.Data[_commandData.PosInCommand - 2] = bt;
                    }
                    break;
            }

            if (_commandData.PosInCommand == _commandData.Len + 2)
            {
                _readingCommand = false;
                if (bt == CalcCRC(_commandData))
                {
                    ProcessCommand(_commandData);
                }
                else
                {
                    Console.WriteLine("FRF: Wrong CRC");
                }
            }

            _commandData.PosInCommand++;
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

        public void SendCommand(Commands code, byte[] data)
        {
            if (data.Length > MaxCommandDataLength)
            {
                throw new Exception("Command's data should be " + MaxCommandDataLength + " bytes maximum");
            }

            byte len = (byte)(data.Length + 4); //start byte, length, crc
            var command = new byte[len];
            command[0] = StartByte;
            command[1] = (byte)code;
            command[2] = (byte)(len - 4);

            Buffer.BlockCopy(data, 0, command, 3, data.Length);
            command[len - 1] = CalcCRC(command, len - 1);

            Write(command);
        }

        private byte CalcCRC(ReadedCommandData rcd)
        {
            byte crc = CalcCRC(rcd.Data, rcd.Len);
            crc += StartByte;
            crc += rcd.Code;
            crc += rcd.Len;

            return crc;
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

        private void ProcessCommand(ReadedCommandData rcd)
        {
            var responce = BasicResponce.Create((Commands)rcd.Code, rcd.Data);
            if (ReceivedResponce != null)
            {
                Console.WriteLine(responce.ToString());
                ReceivedResponce(responce);
            }

            //Console.WriteLine("FRF: Received command:" + rcd.Code.ToString());  
        }
    }

    
}
