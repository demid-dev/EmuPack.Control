using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPack.Control.Models.Commands
{
    public abstract class Command
    {
        public string CommandID { get; protected set; }
        public string SendFrom { get; protected set; }
        public string SendTo { get; protected set; }
        public string DataLength { get; protected set; }

        public Command()
        {
            SendFrom = CommandResponseValues.SendFrom;
            SendTo = CommandResponseValues.SendTo;
        }

        protected virtual string FormCommand()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(CommandID);
            stringBuilder.Append(SendFrom);
            stringBuilder.Append(SendTo);
            stringBuilder.Append(DataLength);
            return stringBuilder.ToString();
        }
    }

    static class CommandResponseValues
    {
        static public string SendFrom { get; private set; }
        static public string SendTo { get; private set; }

        static CommandResponseValues()
        {
            SendFrom = "C1";
            SendTo = "M1";
        }
    }
}
