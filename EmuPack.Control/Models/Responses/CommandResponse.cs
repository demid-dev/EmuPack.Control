using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Models.Responses
{
    public class CommandResponse
    {
        public string CommandId { get; private set; }
        public string SendFrom { get; private set; }
        public string SendTo { get; private set; }
        public string DataLength { get; private set; }
        public string ResponseCode { get; private set; }
        public string ResponseString { get; private set; }

        public CommandResponse(string response)
        {
            CommandId = response.Substring(CommandResponseValues.CommandIdStartIndex,
                CommandResponseValues.CommandIdLength);
            SendFrom = response.Substring(CommandResponseValues.SendFromStartIndex,
                CommandResponseValues.SendFromLength);
            SendTo = response.Substring(CommandResponseValues.SendToStartIndex,
                CommandResponseValues.SendToLength);
            DataLength = response.Substring(CommandResponseValues.DataLengthStartIndex,
                CommandResponseValues.DataLengthLength);
            ResponseCode = response.Substring(CommandResponseValues.ResponseCodeStartIndex,
                CommandResponseValues.ResponseCodeLength);
            ResponseString = response;
        }

        protected virtual string GetNumberWithoutPadding(string number)
        {
            while (number.Length > 1 && number[0] == '0')
            {
                number = number.Remove(0, 1);
            }
            return number;
        }
    }

    public enum CommandResponseCodes
    {
        Sucess,
        WrongCommandFormat,
        MachineBlockedCommand
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
        static public Dictionary<CommandResponseCodes, string> ResponseCodes { get; private set; }

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
            ResponseCodes = new Dictionary<CommandResponseCodes, string>
            {
                [CommandResponseCodes.Sucess] = "00",
                [CommandResponseCodes.WrongCommandFormat] = "01",
                [CommandResponseCodes.MachineBlockedCommand] = "02"
            };
        }
    }
}
