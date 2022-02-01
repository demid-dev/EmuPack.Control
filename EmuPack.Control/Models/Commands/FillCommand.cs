using EmuPack.Control.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPack.Control.Models.Commands
{
    public class FillCommand : Command
    {
        public string PrescriptionStartNotification { get; private set; }
        public string PrescriptionId { get; private set; }
        public string PodPosition { get; private set; }
        public string DispensingStartNotification { get; private set; }
        public string CassetteStartNotification { get; private set; }
        public string TotalNumberOfCassettes { get; private set; }
        public List<CassetteUsedInFilling> CassettesUsedInFilling { get; private set; }

        public FillCommand(FillCommandDTO fillCommandDTO)
        {
            CommandID = FillCommandValues.CommandId;
            PrescriptionStartNotification = FillCommandValues.PrescriptionStartNotification;
            PrescriptionId = NormalizeCommandField(fillCommandDTO.PrescriptionId,
                FillCommandValues.PrescriptionIdLength, CommandPadding.Zeroing);
            PodPosition = fillCommandDTO.PodPosition;
            DispensingStartNotification = FillCommandValues.DispensingStartNotification;
            CassetteStartNotification = FillCommandValues.CassetteStartNotification;
            CassettesUsedInFilling = new List<CassetteUsedInFilling>();
            fillCommandDTO.CassetteUsedInFilling.ForEach(cassette =>
            {
                string cassetteId = NormalizeCommandField(cassette.CassetteId,
                    FillCommandValues.CassetteIDLength, CommandPadding.Zeroing);
                string quantityOfDrug = NormalizeCommandField(cassette.QuantityOfDrug,
                    FillCommandValues.QuantityOfDrugLength, CommandPadding.Zeroing);
                CassettesUsedInFilling.Add(new CassetteUsedInFilling(cassetteId, quantityOfDrug));
            });
            TotalNumberOfCassettes = NormalizeCommandField(CassettesUsedInFilling.Count,
                FillCommandValues.TotalNumberOfCassettesLength, CommandPadding.Zeroing);
            CommandString = FormCommand();
        }

        protected override string FormCommand()
        {
            List<string> commandFieldsList = new List<string> { PrescriptionStartNotification, PrescriptionId,
                PodPosition, DispensingStartNotification, CassetteStartNotification, TotalNumberOfCassettes };

            CassettesUsedInFilling.ForEach(cassette =>
            {
                commandFieldsList.Add(cassette.CassetteID);
                commandFieldsList.Add(cassette.QuantityOfDrug);
            });

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendJoin("", commandFieldsList);
            DataLength = NormalizeCommandField(stringBuilder.Length.ToString(),
                CommandValues.DataLengthLength, CommandPadding.Zeroing);

            return base.FormCommand() + stringBuilder.ToString();
        }
    }

    public class CassetteUsedInFilling
    {
        public string CassetteID { get; private set; }
        public string QuantityOfDrug { get; private set; }

        public CassetteUsedInFilling(string cassetteId, string quantityOfDrug)
        {
            CassetteID = cassetteId;
            QuantityOfDrug = quantityOfDrug;
        }
    }

    static class FillCommandValues
    {
        static public string CommandId { get; private set; }
        static public string PrescriptionStartNotification { get; private set; }
        static public int PrescriptionIdMinValue { get; private set; }
        static public int PrescriptionIdMaxValue { get; private set; }
        static public int PrescriptionIdLength { get; private set; }
        static public int PodPositionLength { get; private set; }
        static public string DispensingStartNotification { get; private set; }
        static public string CassetteStartNotification { get; private set; }
        static public int CassetteStartNotificationLength { get; private set; }
        static public int TotalNumberOfCassettesMinValue { get; private set; }
        static public int TotalNumberOfCassettesMaxValue { get; private set; }
        static public int TotalNumberOfCassettesLength { get; private set; }
        static public int CassetteIDMinValue { get; private set; }
        static public int CassetteIDMaxValue { get; private set; }
        static public int CassetteIDLength { get; private set; }
        static public int QuantityOfDrugMinValue { get; private set; }
        static public int QuantityOfDrugMaxValue { get; private set; }
        static public int QuantityOfDrugLength { get; private set; }

        static FillCommandValues()
        {
            CommandId = "FL";
            PrescriptionStartNotification = "P";
            PrescriptionIdMinValue = 0;
            PrescriptionIdMaxValue = 9999;
            PrescriptionIdLength = 4;
            PodPositionLength = 2;
            DispensingStartNotification = "M";
            CassetteStartNotification = "C";
            CassetteStartNotificationLength = 1;
            TotalNumberOfCassettesMinValue = 1;
            TotalNumberOfCassettesMaxValue = 40;
            TotalNumberOfCassettesLength = 2;
            CassetteIDMinValue = 1;
            CassetteIDMaxValue = 40;
            CassetteIDLength = 2;
            QuantityOfDrugMinValue = 1;
            QuantityOfDrugMaxValue = 99;
            QuantityOfDrugLength = 2;
        }
    }
}
