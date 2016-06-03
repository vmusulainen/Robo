using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Core
{
    public enum Commands : byte
    {
        Status = 0,
        Answer = 1,
        Error = 2,
        Move = 3,
        RangeScan = 4
    };

    public enum MoveDirection : byte
    {
        Stopped = 0,
        Forward = 1,
        Backwards = 2
    };

    public enum TurnDirection : byte
    {
        Left = 3,
        Right = 4
    };

    class BaseCommand
    {
        protected int Timeout = 0;

        public BaseCommand(int timeout = 0)
        {
            Timeout = timeout;
        }

        public virtual void Execute(ComPort port)
        {
            throw new NotImplementedException();
        }

        protected void Execute(ComPort port, Commands cmd, byte[] data)
        {
            port.SendCommand(cmd, data);
            Thread.Sleep(Timeout);
        }
    }

    class StatusCommand : BaseCommand
    {
        private byte _degree;

        public StatusCommand(byte degree, int timeout = 0) : base(timeout)
        {
            _degree = degree;
        }

        public override void Execute(ComPort port)
        {
            base.Execute(port, Commands.Status, new byte[] { _degree });
        }
    }

    class RangeScanCommand : BaseCommand
    {
        public const byte MinDegree = 0;
        public const byte MaxDegree = 43;
        public const byte CenterDegree = 22;
        
        private byte _startDegree;
        private byte _endDegree;

        public RangeScanCommand(byte startDegree, byte endDegree, int timeout = 0) : base(timeout)
        {
            _startDegree = startDegree;
            if (_startDegree > MaxDegree)
            {
                _startDegree = MaxDegree;
            }

            _endDegree = endDegree;
            if (_endDegree > MaxDegree)
            {
                _endDegree = MaxDegree;
            }

            if (_startDegree > _endDegree)
            {
                throw new Exception("Start degree should be <= end degree");
            }
        }

        public override void Execute(ComPort port)
        {
            base.Execute(port, Commands.RangeScan, new byte[] { _startDegree, _endDegree });
        }
    }

    class MoveCommand : BaseCommand
    {
        private MoveDirection _direction;
        private byte _speed;

        public MoveCommand(MoveDirection directrion, byte speed, int timeout = 0)
            : base(timeout)
        {
            _direction = directrion;
            _speed = speed;
        }

        public override void Execute(ComPort port)
        {
            base.Execute(port, Commands.Move, new byte[] { (byte)_direction, _speed });
        }
    }

    class TurnCommand : BaseCommand
    {
        private TurnDirection _direction;
        private byte _speed;
        private byte _degree;
        private byte _stopAfter;

        public TurnCommand(TurnDirection directrion, byte degree, byte speed=255, bool stopAfter=true, int timeout = 0)
            : base(timeout)
        {
            _direction = directrion;
            _speed = speed;
            _degree = degree;
            _stopAfter = Convert.ToByte(stopAfter);
        }

        public override void Execute(ComPort port)
        {
            base.Execute(port, Commands.Move, new byte[] { (byte)_direction, _speed, _degree, _stopAfter });
        }
    }

    class StopCommand : BaseCommand
    {
        public StopCommand(int timeout = 0)
            : base(timeout)
        {

        }

        public override void Execute(ComPort port)
        {
            base.Execute(port, Commands.Move, new byte[] { (byte)MoveDirection.Backwards, 0 });
        }
    }
}
