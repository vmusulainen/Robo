﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.MetaCommands;

namespace Core
{
    public class Thinker
    {
        private Queue<BasicResponce> _responces = new Queue<BasicResponce>(10);
        private ComPort _port;
        private IMetaCommand _currentMetaCommand = null;

        public Thinker(ComPort port)
        {
            _port = port;
            _port.ReceivedResponce += PortOnReceivedResponce;
            
            //_currentMetaCommand = new ScanMetaCommand();
            //_currentMetaCommand = new SimpleMovementMetaCommand();
            _currentMetaCommand = new ExplorerMetaCommand();
        }

        private void PortOnReceivedResponce(BasicResponce cmd)
        {
            _responces.Enqueue(cmd);
        }

        public void MainLoop()
        {
            while (true)
            {
                BasicResponce responce = null;
                if (_responces.Count > 0)
                {
                    responce = _responces.Dequeue();
                }

                if (_currentMetaCommand != null)
                {
                    var result = _currentMetaCommand.ProcessResponce(responce);
                    switch(result.Action)
                    {
                        case MetaCommandAction.Command:
                            result.Command.Execute(_port);
                            break;

                        case MetaCommandAction.NewMetaCommand:
                            _currentMetaCommand = result.MetaCommand;
                            break;

                        case MetaCommandAction.Done:
                            _currentMetaCommand = null;
                            break;

                        default:
                            break;
                    }
                }

                Thread.Sleep(1);
               
            }
            
            /*var cmd = new TurnCommand(TurnDirection.Left, 180, 255, true, 500);
            cmd.Execute(_port);*/

            /*while (true)
            {
                //var cmd = new RangeScanCommand(11, 11, 3500);
                var cmd = new StatusCommand(11, 500);
                cmd.Execute(_port);
            }*/

        }
    }
}
