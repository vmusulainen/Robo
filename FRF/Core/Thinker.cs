using System;
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
            
            _currentMetaCommand = new ScanMetaCommand();
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

            /*byte degree = 0;
            _port.SendCommand(Commands.Status, new byte[] { degree });
            while (true)
            {
                if (_responces.Count > 0)
                {
                    var cmd = _responces.Dequeue();
                    if (cmd.Code == Commands.Status)
                    {
                        _port.SendCommand(Commands.Status, new byte[] { degree });
                        degree++;
                    }
                }

                Thread.Sleep(75);
            }*/
        }
    }
}
