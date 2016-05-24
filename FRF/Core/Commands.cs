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
        Move = 3
    };

    public enum MoveDirection : byte
    {
        Stopped = 0xFF,
        Forward = 0,
        Backwards = 1
    };

    public enum TurnDirection : byte
    {
        Left = 2,
        Right = 3
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

        public TurnCommand(TurnDirection directrion, byte speed, int timeout = 0)
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
