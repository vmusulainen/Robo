using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Forward = 0,
        Backwards = 1
    };

    public enum TurnDirection : byte
    {
        Left = 2,
        Right = 3
    };


    interface ICommand
    {
         void Execute(ComPort port);
    }

    class StatusCommand : ICommand
    {
        private byte _degree;

        public StatusCommand(byte degree)
        {
            _degree = degree;
        }

        public void Execute(ComPort port)
        {
            port.SendCommand(Commands.Status, new byte[] { _degree });
        }
    }

    class MoveCommand : ICommand
    {
        private MoveDirection _direction;
        private byte _speed;

        public MoveCommand(MoveDirection directrion, byte speed)
        {
            _direction = directrion;
            _speed = speed;
        }

        public void Execute(ComPort port)
        {
            port.SendCommand(Commands.Move, new byte[] { (byte)_direction, _speed });
        }
    }

    class TurnCommand : ICommand
    {
        private TurnDirection _direction;
        private byte _speed;

        public TurnCommand(TurnDirection directrion, byte speed)
        {
            _direction = directrion;
            _speed = speed;
        }

        public void Execute(ComPort port)
        {
            port.SendCommand(Commands.Move, new byte[] { (byte)_direction, _speed });
        }
    }

    class StopCommand : ICommand
    {
        public StopCommand()
        {

        }

        public void Execute(ComPort port)
        {
            port.SendCommand(Commands.Move, new byte[] { (byte)MoveDirection.Backwards, 0 });
        }
    }
}
