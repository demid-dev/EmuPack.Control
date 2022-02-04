using EmuPack.Control.Models.Commands;
using EmuPack.Control.Models.Machine;
using EmuPack.Control.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Services
{
    public class ResponseProcessingService
    {
        private readonly NotificationService _notificationService;

        public ResponseProcessingService(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void ProcessResponse(string response, 
            MachineState machineState)
        {
            CommandResponse commandResponse = new CommandResponse(response);

            CommandResponseValues.ResponseCodes.TryGetValue(CommandResponseCodes.MachineBlockedCommand,
                out string machineBlockedCommandCode);
            if (commandResponse.ResponseCode == machineBlockedCommandCode)
            {
                _notificationService.SendMachineBlockedCommandNotification(commandResponse);
            }
            else if (commandResponse.CommandId == StatusRequestCommandValues.CommandId)
            {
                StatusCommandResponse statusCommandResponse = new StatusCommandResponse(response);
                ProcessStatusResponse(statusCommandResponse, machineState);
            }
        }

        private void ProcessStatusResponse(StatusCommandResponse response, 
            MachineState machineState)
        {
            if (response.WarningCassettesIds.Any())
            {
                _notificationService.SendCassetteWarningNotification(response);
            }

            machineState.UpdateMachineState(response);
        }
    }
}
