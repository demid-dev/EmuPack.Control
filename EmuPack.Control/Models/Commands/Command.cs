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
        public string CommandString { get; protected set; }

        public Command()
        {
            SendFrom = CommandValues.SendFrom;
            SendTo = CommandValues.SendTo;
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

        public virtual string NormalizeCommandField(string fieldToNormalize, int fieldLength, CommandPadding padding)
        {
            string normalizedField = fieldToNormalize;
            if (padding == CommandPadding.Zeroing)
            {
                for (int i = 0; i < fieldLength - fieldToNormalize.Length; i++)
                {
                    normalizedField = CommandValues.ZeroPadSymbol + normalizedField;
                }
            }
            else
            {
                for (int i = 0; i < fieldLength - fieldToNormalize.Length; i++)
                {
                    normalizedField += CommandValues.SpacePadSymbol;
                }
            }

            return normalizedField;
        }

        public virtual string NormalizeCommandField(int fieldToNormalize, int fieldLength, CommandPadding padding)
        {
            string stringFieldToNormalize = fieldToNormalize.ToString();
            string normalizedField = stringFieldToNormalize;
            if (padding == CommandPadding.Zeroing)
            {
                for (int i = 0; i < fieldLength - stringFieldToNormalize.Length; i++)
                {
                    normalizedField = CommandValues.ZeroPadSymbol + normalizedField;
                }
            }
            else
            {
                for (int i = 0; i < fieldLength - stringFieldToNormalize.Length; i++)
                {
                    normalizedField += CommandValues.SpacePadSymbol;
                }
            }

            return normalizedField;
        }
    }

    static class CommandValues
    {
        static public string SendFrom { get; private set; }
        static public string SendTo { get; private set; }
        static public int DataLengthLength { get; private set; }

        static public char ZeroPadSymbol { get; private set; }
        static public char SpacePadSymbol { get; private set; }

        static CommandValues()
        {
            SendFrom = "C1";
            SendTo = "M1";
            DataLengthLength = 5;

            ZeroPadSymbol = '0';
            SpacePadSymbol = ' ';
        }
    }

    public enum CommandPadding
    {
        Zeroing,
        Spacing
    }

}
