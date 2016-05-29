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
        private ushort[] _scanResult = new ushort[43];


        public ScanMetaCommand()
        {
            
        }

        public BasicMetaCommandResult ProcessResponce(BasicResponce rsp)
        {
            if (_step == 0)
            {
                _step = 1;
                var cmd = new StatusCommand(_degree, 1200);
                return new BasicMetaCommandResult(MetaCommandAction.Command, cmd);
            }

            if (rsp == null)
            {
                return new BasicMetaCommandResult(MetaCommandAction.Idle);
            }

            var scanResponce = rsp as StatusResponce;
            if (scanResponce != null)
            {
                _scanResult[scanResponce.Degree] = scanResponce.DistanceToObstacle;
            }
            
            if (_step == 1 && _degree < MaxDegree)
            {
                _degree += 1;
                var cmd = new StatusCommand(_degree, 50);
                return new BasicMetaCommandResult(MetaCommandAction.Command, cmd);
            }
           
            return new BasicMetaCommandResult(MetaCommandAction.Done); 
        }

        public ushort[] GetScanResult()
        {
            return _scanResult;
        }
    }
}
