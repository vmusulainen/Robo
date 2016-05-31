using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MetaCommands
{
    class ExplorerMetaCommand : IMetaCommand
    {
        /*
         1. Двигаемся вперед пока не наткнемся на препятствие
         2. Сканируем, поворачиваем на направление с максимальной дистанцией
         2.1 Если повсюду препятствия, поворачиваемся на случайный угол(25-180). Переход к пункту 2.
         3. Двигаемся по новому направлению. Переход к п.1.
             */
        public BasicMetaCommandResult ProcessResponce(BasicResponce rsp)
        {
            if (rsp == null)
            {
                Console.WriteLine("Sending status");
                var cmd = new StatusCommand(10, 200);
                return new BasicMetaCommandResult(MetaCommandAction.Command, cmd);
            }

            Console.WriteLine("Received command");
            var status = rsp as StatusResponce;
            if (status == null)
            {
                Console.WriteLine("Idle");
                return new BasicMetaCommandResult(MetaCommandAction.Idle);
            }



            return new BasicMetaCommandResult(MetaCommandAction.Idle);
        }
    }
}
