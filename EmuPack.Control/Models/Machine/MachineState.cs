using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Models.Machine
{
    public class MachineState
    {
        public List<Notification> Notifications { get; set; }
    }

    public class Notification
    {
        public string Description { get; set; }
        public string Timestamp { get; set; }

        public Notification()
        {

        }
    }
}
