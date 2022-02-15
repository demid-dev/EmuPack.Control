using EmuPack.Control.Models.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmuPack.Control.Services.Operations
{
    public class StatusOperationHandler
    {
        private readonly MachineClient _machineClient;

        public StatusOperationHandler(MachineClient machineClient)
        {
            _machineClient = machineClient;
        }

        public void UpdateMachineState()
        {
            RequestStatus();
            WaitUntilMachineStateUpdated();
        }

        private void RequestStatus()
        {
            StatusRequestCommand command = new StatusRequestCommand();
            _machineClient.SendCommand(command);
        }

        private void WaitUntilMachineStateUpdated()
        {
            Thread.Sleep(200);
        }
    }
}
