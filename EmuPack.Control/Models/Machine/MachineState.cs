using EmuPack.Control.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Models.Machine
{
    public class MachineState
    {
        public bool DrawerOpened { get; private set; }
        public bool AdaptorInDrawer { get; private set; }
        public List<string> RegistredPrescriptionsIds { get; private set; }
        public List<string> WarningCassettesIds { get; private set; }

        public MachineState()
        {
            RegistredPrescriptionsIds = new List<string>();
            WarningCassettesIds = new List<string>();
        }

        public void UpdateMachineState(StatusCommandResponse statusCommandResponse)
        {
            if (statusCommandResponse.DrawerStatus == StatusCommandResponseValues.DrawerOpenedValue)
                DrawerOpened = true;
            else
                DrawerOpened = false;
            if (statusCommandResponse.AdaptorStatus == StatusCommandResponseValues.AdaptorInDrawerValue)
                AdaptorInDrawer = true;
            else
                AdaptorInDrawer = false;

            RegistredPrescriptionsIds = statusCommandResponse.RegistredPrescriptionsIds;
            WarningCassettesIds = statusCommandResponse.WarningCassettesIds;
        }
    }
}
