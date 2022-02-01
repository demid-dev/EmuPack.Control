using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.DTOs
{
    public class MachineActivityRequestCommandDTO
    {
        public bool DrawerShouldBeLocked { get; set; }
        public bool ClearingShouldBeInitiated { get; set; }
    }
}
