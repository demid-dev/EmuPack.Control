using EmuPack.Control.DTOs;
using EmuPack.Control.Hubs;
using EmuPack.Control.Models.Responses;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async void SendTcpErrorNotification(NotificationType notificationType,
            string hostname, int port)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification",
                GenerateNotificationDTO(notificationType, GenerateTcpWarningFields(hostname, port)));
        }

        public async void SendMachineBlockedCommandNotification(CommandResponse response)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification",
                GenerateNotificationDTO(NotificationType.MachineBlockedCommandError,
                    GenerateMachineBlockedCommandWarningFields(response)));
        }

        public async void SendCassetteWarningNotification(StatusCommandResponse response)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification",
                GenerateNotificationDTO(NotificationType.CassetteWarning,
                    GenerateWarningCassettesFields(response)));
        }

        private List<WarningFieldDTO> GenerateTcpWarningFields(string hostname, int port)
        {
            return new List<WarningFieldDTO>() {
                new WarningFieldDTO { FieldName = "Hostname", Value = hostname.ToString() },
                new WarningFieldDTO { FieldName = "Port", Value = port.ToString()}
            };
        }

        private List<WarningFieldDTO> GenerateMachineBlockedCommandWarningFields(CommandResponse response)
        {
            return new List<WarningFieldDTO>() {
                new WarningFieldDTO { FieldName = "Response command id", Value = response.CommandId },
                new WarningFieldDTO { FieldName = "Response command string", Value = response.ResponseString}
            };
        }

        private List<WarningFieldDTO> GenerateWarningCassettesFields(StatusCommandResponse response)
        {
            List<WarningFieldDTO> warningFields = new List<WarningFieldDTO>();
            response.WarningCassettesIds.ForEach(cassetteId =>
            {
                warningFields.Add(new WarningFieldDTO
                {
                    FieldName = "Warring cassette id",
                    Value = cassetteId
                });
            });

            return warningFields;
        }

        private NotificationDTO GenerateNotificationDTO(NotificationType notificationType,
            List<WarningFieldDTO> warningFields)
        {
            return new NotificationDTO
            {
                NotificationType = notificationType,
                WarningFields = warningFields,
                Timestamp = DateTime.Now.ToString("HH:mm:ss")
            };
        }
    }
}

