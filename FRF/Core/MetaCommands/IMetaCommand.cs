using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MetaCommands
{
    public enum MetaCommandAction : byte
    {
        Command = 0,
        NewMetaCommand = 1,
        Done = 2,
        Idle = 3
    };

    class BasicMetaCommandResult
    {
        public MetaCommandAction Action { get; set; }
        public BaseCommand Command { get; set; }
        public IMetaCommand MetaCommand { get; set; }

        public BasicMetaCommandResult(MetaCommandAction action, BaseCommand command = null, IMetaCommand metaCommand = null)
        {
            Action = action;
            Command = command;
            MetaCommand = metaCommand;
        }
    }

    interface IMetaCommand
    {
        BasicMetaCommandResult ProcessResponce(BasicResponce responce);
    }
}
