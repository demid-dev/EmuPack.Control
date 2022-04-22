using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.DTOs
{
    public class NotificationDTO
    {
        public NotificationType NotificationType { get; set; }
        public List<WarningFieldDTO> WarningFields { get; set; }
        public string Timestamp { get; set; }
    }

    public class WarningFieldDTO
    {
        public string FieldName { get; set; }
        public string Value { get; set; }
    }

    public enum NotificationType
    {
        TcpConnectionError,
        TcpSendMessageError,
        TcpReceivingError,
        MachineBlockedCommandError,
        DispensingNotPossibleError,
        DispensingSucessful,
        CassetteWarning,
        InitializationSucessful
    }
}
