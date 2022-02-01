using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Models
{
    public class CommandResponse
    {
        public string CommandId { get; private set; }
        public string SendFrom { get; private set; }
        public string SendTo { get; private set; }
        public string DataLength { get; private set; }
        public string ResponseCode { get; private set; }

        public CommandResponse(string resposne)
        {
            CommandId = resposne.Substring(CommandResponseValues.CommandIdStartIndex,
                CommandResponseValues.CommandIdLength);
            SendFrom = resposne.Substring(CommandResponseValues.SendFromStartIndex,
                CommandResponseValues.SendFromLength);
            SendTo = resposne.Substring(CommandResponseValues.SendToStartIndex,
                CommandResponseValues.SendToLength);
            DataLength = resposne.Substring(CommandResponseValues.DataLengthStartIndex,
                CommandResponseValues.DataLengthLength);
            ResponseCode = resposne.Substring(CommandResponseValues.ResponseCodeStartIndex,
                CommandResponseValues.ResponseCodeLength);
        }
    }

    static class CommandResponseValues
    {
        static public int CommandIdStartIndex { get; private set; }
        static public int CommandIdLength { get; private set; }
        static public int SendFromStartIndex { get; private set; }
        static public int SendFromLength { get; private set; }
        static public int SendToStartIndex { get; private set; }
        static public int SendToLength { get; private set; }
        static public int DataLengthStartIndex { get; private set; }
        static public int DataLengthLength { get; private set; }
        static public int ResponseCodeStartIndex { get; private set; }
        static public int ResponseCodeLength { get; private set; }

        static CommandResponseValues()
        {
            CommandIdStartIndex = 0;
            CommandIdLength = 2;
            SendFromStartIndex = 2;
            SendFromLength = 2;
            SendToStartIndex = 4;
            SendToLength = 2;
            DataLengthStartIndex = 6;
            DataLengthLength = 5;
            ResponseCodeStartIndex = 11;
            ResponseCodeLength = 2;
        }
    }
}
