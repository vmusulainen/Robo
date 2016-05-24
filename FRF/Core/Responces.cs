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
            _responcesTypes = new Type[3];
            _responcesTypes[(byte) Commands.Status] = typeof (StatusResponce);
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
        public int DistanceToObstacle { get; set; }
        public ushort SensorPos { get; set; }
        public byte Temperature { get; set; }
        public byte Degree { get; set; }

        public StatusResponce(Commands code, byte[] data) : base(code)
        {
            //RobotStatus = (RobotStatus)data[0];
            Temperature = data[0];
            int dst = data[1];
            dst = dst << 8;
            dst += data[2];

            if (dst == 65535)
            {
                DistanceToObstacle = -1;
            }
            else
            {
                DistanceToObstacle = dst;
            }

            Degree = data[4];
        }

        public override string ToString()
        {
            return String.Format("Status. Temperature: {0}, Distance: {1}, Degree: {2}", Temperature, DistanceToObstacle, Degree);
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
