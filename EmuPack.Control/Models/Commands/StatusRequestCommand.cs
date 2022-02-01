using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Models.Commands
{
    public class StatusRequestCommand : Command
    {
    }

    static class StatusRequestCommandValues
    {
        static public string CommandId { get; private set; }

        static StatusRequestCommandValues()
        {
            CommandId = "SR";
        }
    }
}
