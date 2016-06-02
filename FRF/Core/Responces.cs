using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public enum RobotStatus : byte
    {
        Idle = 1,
        Scanning = 2,
        Moving = 3
    };

    public class BasicResponce
    {
        private static Type[] _responcesTypes;

        static BasicResponce()
        {
            _responcesTypes = new Type[5];
            _responcesTypes[(byte)Commands.Status] = typeof(StatusResponce);
            _responcesTypes[(byte)Commands.Answer] = typeof(AnswerResponce);
            _responcesTypes[(byte)Commands.Error] = typeof(ErrorResponce);
            _responcesTypes[(byte)Commands.Move] = null;
            _responcesTypes[(byte)Commands.RangeScan] = typeof(RangeScanResponce);
        }

        public static BasicResponce Create(Commands code, byte[] data)
        {
            return 
                (BasicResponce)System.ComponentModel.TypeDescriptor.CreateInstance(
                    provider: null, // use standard type description provider, which uses reflection
                    objectType: _responcesTypes[(byte)code],
                    argTypes: new Type[] { typeof(Commands), typeof(byte[]) },
                    args: new object[] { code, data }
                );
        }

        public Commands Code { get; set; }

        public BasicResponce(Commands code)
        {
            Code = code;
        }
    }

    public class StatusResponce : BasicResponce
    {
        public RobotStatus RobotStatus { get; set; }
        public ushort DistanceToObstacle { get; set; }
        public ushort SensorPos { get; set; }
        public byte Temperature { get; set; }
        public byte Degree { get; set; }

        public StatusResponce(Commands code, byte[] data) : base(code)
        {
            //RobotStatus = (RobotStatus)data[0];
            Temperature = data[0];
            DistanceToObstacle = data[1];
            DistanceToObstacle = (ushort)(DistanceToObstacle << 8);
            DistanceToObstacle += data[2];
            Degree = data[4];
        }

        public override string ToString()
        {
            return String.Format("Status. Temperature: {0}, Distance: {1}, Degree: {2}", Temperature, DistanceToObstacle, Degree);
        }
    }

    public class RangeScanResponce : BasicResponce
    {
        public ushort[] Distances { get; set; }
        public byte StartDegree { get; set; }
        public byte EndDegree { get; set; }

        public RangeScanResponce(Commands code, byte[] data) : base(code)
        {
            StartDegree = data[0];
            EndDegree = data[1];

            Distances = new ushort[data.Length / 2 - 1];
            for (int i = 2; i < data.Length; i += 2)
            {
                ushort dist = data[i];
                dist = (ushort)(dist << 8);
                dist += data[i + 1];
                Distances[i / 2 - 1] = dist;
            }
        }

        public override string ToString()
        {
            return String.Format("Range scan. From: {0}, To: {1}, Distances: {2}", StartDegree, EndDegree, UshortArrayToString(Distances));
        }

        private static string UshortArrayToString(ushort[] arr)
        {
            var hex = new StringBuilder(arr.Length * 5);
            foreach (ushort el in arr)
            {
                byte lb = (byte)el;
                byte hb = (byte)(el >> 8); 
                hex.AppendFormat("{0:x2}", hb);
                hex.AppendFormat("{0:x2}", lb);
                hex.Append(' ');
            }
                
            return hex.ToString();
        }
    }

    public class AnswerResponce : BasicResponce
    {
        public byte Command { get; set; }
        public byte ErorCode { get; set; }

        public AnswerResponce(Commands code, byte[] data)
            : base(code)
        {
            Command = data[0];
            ErorCode = data[1];
        }

        public override string ToString()
        {
            return String.Format("Answer. To command: {0}, ErrorCode: {1}", Command, ErorCode);
        }
    }

    public class ErrorResponce : BasicResponce
    {
        public byte ErorCode { get; set; }
        private static readonly string[] _errorCodes =
        {
            "Status comand timeout"
        };


        public ErrorResponce(Commands code, byte[] data)
            : base(code)
        {
            ErorCode = data[0];

            Console.WriteLine(this.ToString());
        }

        public string GetError()
        {
            return _errorCodes[ErorCode];
        }

        public override string ToString()
        {
            return String.Format("ERROR! Error description: {0}", GetError());
        }
    }
}
