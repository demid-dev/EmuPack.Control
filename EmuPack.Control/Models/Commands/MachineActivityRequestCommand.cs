using EmuPack.Control.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPack.Control.Models.Commands
{
    public class MachineActivityRequestCommand: Command
    {
        public string DataStartNotification { get; private set; }
        public string DrawerStatus { get; private set; }
        public string ClearingWarningsInitiated { get; private set; }

        public MachineActivityRequestCommand(MachineActivityRequestCommandDTO dto)
        {
            CommandID = MachineActivityRequestValues.CommandId;
            DataStartNotification = MachineActivityRequestValues.DataStartNotification;
            if (dto.DrawerShouldBeLocked)
                DrawerStatus = MachineActivityRequestValues.DrawerStatusLocked;
            else
                DrawerStatus = MachineActivityRequestValues.DrawerStatusUnlocked;
            if (dto.ClearingShouldBeInitiated)
                ClearingWarningsInitiated = MachineActivityRequestValues.ClearingInitiated;
            else
                ClearingWarningsInitiated = MachineActivityRequestValues.ClearingNotInitiated;

            CommandString = FormCommand();
        }

        protected override string FormCommand()
        {
            List<string> commandFieldsList = new List<string> { DataStartNotification,
                DrawerStatus, ClearingWarningsInitiated };

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendJoin("", commandFieldsList);
            DataLength = NormalizeCommandField(stringBuilder.Length.ToString(),
                CommandValues.DataLengthLength, CommandPadding.Zeroing);

            return base.FormCommand() + stringBuilder.ToString();
        }
    }

    static class MachineActivityRequestValues
    {
        static public string CommandId { get; private set; }
        static public string DataStartNotification { get; private set; }
        static public string DrawerStatusLocked { get; private set; }
        static public string DrawerStatusUnlocked { get; private set; }
        static public string ClearingNotInitiated { get; private set; }
        static public string ClearingInitiated { get; private set; }

        static MachineActivityRequestValues()
        {
            CommandId = "MR";
            DataStartNotification = "D";
            DrawerStatusLocked = "00";
            DrawerStatusUnlocked = "01";
            ClearingNotInitiated = "00";
            ClearingInitiated = "01";
        }
    }
}
