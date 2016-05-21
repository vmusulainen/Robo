using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.MetaCommands
{
    class ScanMetaCommand : IMetaCommand
    {
        private int _step = 0;
        private byte _degree = 0;
        private const byte MaxDegree = 43;

        public ScanMetaCommand()
        {
            
        }

        public BasicMetaCommandResult ProcessResponce(BasicResponce cmd)
        {
            if (_step == 0)
            {
                _step = 1;
                return new BasicMetaCommandResult(MetaCommandAction.Command, new StatusCommand(_degree));
            }

            if (_step == 1)
            {
                _step = 2;
                Thread.Sleep(1200);
            }

            if (cmd == null)
            {
                return new BasicMetaCommandResult(MetaCommandAction.Idle);
            }

            if (_step == 2 && _degree < MaxDegree)
            {
                _degree++;
                return new BasicMetaCommandResult(MetaCommandAction.Command, new StatusCommand(_degree));
            }
           
            return new BasicMetaCommandResult(MetaCommandAction.Done); 
        }
    }
}
