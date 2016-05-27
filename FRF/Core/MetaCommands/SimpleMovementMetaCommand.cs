using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MetaCommands
{
    class SimpleMovementMetaCommand : IMetaCommand
    {
        private const int MinDistance = 50;
        private const byte Degree = 22;
        private MoveDirection _direction = MoveDirection.Stopped;
        private const byte Speed = 125;

        public BasicMetaCommandResult ProcessResponce(BasicResponce rsp)
        { 
            if (rsp == null)
            {
                Console.WriteLine("Sending status");
                var cmd = new StatusCommand(Degree, 200);
                return new BasicMetaCommandResult(MetaCommandAction.Command, cmd);
            }

            Console.WriteLine("Received command");
            var status = rsp as StatusResponce;
            if (status == null)
            {
                Console.WriteLine("Idle");
                return new BasicMetaCommandResult(MetaCommandAction.Idle);
            }

            if (status.DistanceToObstacle < MinDistance && _direction == MoveDirection.Forward)
            {
                return MoveBackward();
            }

            if (status.DistanceToObstacle >= MinDistance && _direction == MoveDirection.Backwards)
            {
                return MoveForward();
            }

            if (_direction == MoveDirection.Stopped)
            {
                Console.WriteLine("Stopped.Moving forward");
                return MoveForward();
            }

            return new BasicMetaCommandResult(MetaCommandAction.Idle);
        }

        private BasicMetaCommandResult MoveForward()
        {
            Console.WriteLine("Moving forward");
            _direction = MoveDirection.Forward;
            return Move(MoveDirection.Forward);
        }

        private BasicMetaCommandResult MoveBackward()
        {
            Console.WriteLine("Moving backward");
            _direction = MoveDirection.Backwards;
            return Move(MoveDirection.Backwards);
        }

        private BasicMetaCommandResult Move(MoveDirection direction)
        {
            var cmd = new MoveCommand(direction, Speed, 200);
            return new BasicMetaCommandResult(MetaCommandAction.Command, cmd);
        }
    }
}
