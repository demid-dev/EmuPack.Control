using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Models.Commands
{
    public class MachineActivityRequestCommand: Command
    {
        public string DataStartNotification { get; private set; }
        public string DrawerStatus { get; private set; }
        public string ClearingWarningsInitiated { get; private set; }
    }
}
