using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MetaCommands
{
    class ExplorerMetaCommand : IMetaCommand
    {
        private enum Status : byte
        {
            Start = 0,
            StartScanning = 10,
            WaitingForScanResults = 1,
            StartMoving = 2,
            Moving = 22,
            StartTurning = 3,
            None = 4
        };

        private Status _status = Status.Start;
        private TurnDirection _turnDirection;
        private byte _turnDegree;

        private const ushort MinDistance = 60;
        private const byte Speed = 200;
        private const byte MinScanDegree = RangeScanCommand.MinDegree + 10;
        private const byte MaxScanDegree = RangeScanCommand.MaxDegree - 10;

        /*
         1. Двигаемся вперед пока не наткнемся на препятствие
         2. Сканируем, поворачиваем на направление с максимальной дистанцией
         2.1 Если повсюду препятствия, поворачиваемся на случайный угол(25-180). Переход к пункту 2.
         3. Двигаемся по новому направлению. Переход к п.1.
             */
        public BasicMetaCommandResult ProcessResponce(BasicResponce rsp)
        {
            switch (_status)
            {
                case Status.Start:
                    Console.WriteLine(_status.ToString());
                    var startCmd = new RangeScanCommand(MinScanDegree, MaxScanDegree, 200);
                    _status = Status.WaitingForScanResults;
                    return new BasicMetaCommandResult(MetaCommandAction.Command, startCmd);

                case Status.StartScanning:
                    var answer = rsp as AnswerResponce;
                    if (answer != null && answer.Code == Commands.Move)
                    {
                        Console.WriteLine(_status.ToString());
                        var cmd = new RangeScanCommand(MinScanDegree, MaxScanDegree, 200);
                        _status = Status.WaitingForScanResults;
                        return new BasicMetaCommandResult(MetaCommandAction.Command, cmd);
                    }
                    break;

                case Status.WaitingForScanResults:
                    var responce = rsp as RangeScanResponce;
                    if (responce != null)
                    {
                        Console.WriteLine(_status.ToString());
                        var degree = FindNewDirection(responce); 
                        Console.WriteLine(" Selected degree: " + degree);
                        var rnd = new Random();
                        if (degree == null)
                        {
                            _turnDirection = (TurnDirection)rnd.Next((int)TurnDirection.Left, (int)TurnDirection.Right + 1);
                            _turnDegree = (byte)rnd.Next(90, 180);
                            _status = Status.StartTurning;
                            break;
                        }

                        if (degree == 0xFF)
                        {
                            _status = Status.StartMoving;
                            break;
                        }

                        if (degree > RangeScanCommand.CenterDegree)
                        {
                            _turnDirection = TurnDirection.Right;
                            _turnDegree = (byte)(degree - RangeScanCommand.CenterDegree);
                        }
                        else
                        {
                             _turnDirection = TurnDirection.Left;
                             _turnDegree = (byte)(RangeScanCommand.CenterDegree - degree);
                        }
                        
                        _status = Status.StartTurning;
                    }

                    break;

                case Status.StartMoving:
                    Console.WriteLine(_status.ToString());
                    _status = Status.Moving;
                    var moveCmd = new MoveCommand(MoveDirection.Forward, Speed, 500);
                    return new BasicMetaCommandResult(MetaCommandAction.Command, moveCmd);

                case Status.Moving:
                    if (rsp == null)
                    {
                        var statusCmd = new StatusCommand(RangeScanCommand.CenterDegree, 500);
                        return new BasicMetaCommandResult(MetaCommandAction.Command, statusCmd);
                    }

                    Console.WriteLine(_status.ToString());
                    var status = rsp as StatusResponce;
                    if (status == null)
                    {
                        break;
                    }

                    if (status.DistanceToObstacle < MinDistance)
                    {
                        _status = Status.StartScanning;
                        var stopCmd = new MoveCommand(MoveDirection.Stopped, Speed, 500);
                        return new BasicMetaCommandResult(MetaCommandAction.Command, stopCmd);
                    }

                    break;
                case Status.StartTurning:
                    Console.WriteLine(_status.ToString());
                    _status = Status.StartScanning;
                    var turnCmd = new TurnCommand(_turnDirection, _turnDegree, 255, true, 500);
                    return new BasicMetaCommandResult(MetaCommandAction.Command, turnCmd);

                case Status.None:
                    break;
                default:
                    Console.WriteLine("ERROR! Unknown status!");
                    break;
            }

            return new BasicMetaCommandResult(MetaCommandAction.Idle);
        }

        byte? FindNewDirection(RangeScanResponce responce)
        {
            const int scanRange = 2;
            for (int i = 0; i < scanRange; i++)
            {
                if (responce.Distances[RangeScanCommand.CenterDegree - i] > MinDistance)
                {
                    return 0xFF;
                }
            }

            for (int i = 0; i < scanRange; i++)
            {
                if (responce.Distances[RangeScanCommand.CenterDegree + i] > MinDistance)
                {
                    return 0xFF;
                }
            }

           
            ushort max = MinDistance;
            for (int i = responce.StartDegree; i < responce.EndDegree + 1; i++)
            {
                if (responce.Distances[i - responce.StartDegree] > max)
                {
                    max = responce.Distances[i - responce.StartDegree];
                }
            }
 
            var maximums = new List<int>();
            for (int i = responce.StartDegree; i < responce.EndDegree + 1; i++)
            {
                if (responce.Distances[i - responce.StartDegree] == max)
                {
                    maximums.Add(i);
                }
            }

            if (maximums.Count == 0)
            {
                return null;
            }

            var rnd = new Random();
            return (byte)maximums[(byte)rnd.Next(0, maximums.Count)];
        }
    }
}
