using EmuPack.Control.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPack.Control.Models.Commands
{
    public class PrescriptionRegistrationCommand : Command
    {
        public string RegistrationStartNotification { get; private set; }
        public string PrescriptionId { get; private set; }
        public string TotalNumberOfRegistredCassettes { get; private set; }
        public List<RxRegistrationCommandDrug> RxRegistrationCommandDrugs { get; private set; }

        public PrescriptionRegistrationCommand(PrescriptionRegistrationCommandDTO dto)
        {
            CommandID = PrescriptionRegistrationCommandValues.CommandId;
            RegistrationStartNotification = PrescriptionRegistrationCommandValues.RegistrationStartNotification;
            PrescriptionId = NormalizeCommandField(dto.PrescriptionId,
                PrescriptionRegistrationCommandValues.PrescriptionIdLength,
                CommandPadding.Zeroing);
            TotalNumberOfRegistredCassettes = NormalizeCommandField(dto.RxRegistrationCommandDrugsDTOs.Count,
                PrescriptionRegistrationCommandValues.TotalNumberOfRegistredCassettesLength,
                CommandPadding.Zeroing);

            RxRegistrationCommandDrugs = new List<RxRegistrationCommandDrug>();
            dto.RxRegistrationCommandDrugsDTOs.ForEach(drug =>
            {
                string cassetteId = NormalizeCommandField(drug.CassetteId,
                    PrescriptionRegistrationCommandValues.CassetteIDLength,
                    CommandPadding.Zeroing);
                string drugName = NormalizeCommandField(drug.DrugName,
                    PrescriptionRegistrationCommandValues.DrugNameLength,
                    CommandPadding.Spacing);
                string quantityPerCassette = NormalizeCommandField(drug.QuantityPerCassette,
                    PrescriptionRegistrationCommandValues.QuantityPerCassetteLength,
                    CommandPadding.Zeroing);
                RxRegistrationCommandDrugs.Add(new RxRegistrationCommandDrug(cassetteId,
                    drugName, quantityPerCassette));
            });

            CommandString = FormCommand();
        }

        protected override string FormCommand()
        {
            List<string> commandFieldsList = new List<string> { RegistrationStartNotification,
                PrescriptionId, TotalNumberOfRegistredCassettes };

            RxRegistrationCommandDrugs.ForEach(drug =>
            {
                commandFieldsList.Add(drug.CassetteId);
                commandFieldsList.Add(drug.DrugName);
                commandFieldsList.Add(drug.QuantityPerCassette);
            });

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendJoin("", commandFieldsList);
            DataLength = NormalizeCommandField(stringBuilder.Length.ToString(),
                CommandValues.DataLengthLength, CommandPadding.Zeroing);

            return base.FormCommand() + stringBuilder.ToString();
        }
    }

    public class RxRegistrationCommandDrug
    {
        public string CassetteId { get; private set; }
        public string DrugName { get; private set; }
        public string QuantityPerCassette { get; private set; }

        public RxRegistrationCommandDrug(string cassetteId,
            string drugName, string quantityPerCassette)
        {
            CassetteId = cassetteId;
            DrugName = drugName;
            QuantityPerCassette = quantityPerCassette;
        }
    }

    static class PrescriptionRegistrationCommandValues
    {
        static public string CommandId { get; private set; }
        static public string RegistrationStartNotification { get; private set; }
        static public int PrescriptionIdMinValue { get; private set; }
        static public int PrescriptionIdMaxValue { get; private set; }
        static public int PrescriptionIdLength { get; private set; }
        static public int TotalNumberOfRegistredCassettesMinValue { get; private set; }
        static public int TotalNumberOfRegistredCassettesMaxValue { get; private set; }
        static public int TotalNumberOfRegistredCassettesLength { get; private set; }
        static public int CassetteIDMinValue { get; private set; }
        static public int CassetteIDMaxValue { get; private set; }
        static public int CassetteIDLength { get; private set; }
        static public int DrugNameLength { get; private set; }
        static public int QuantityPerCassetteMinValue { get; private set; }
        static public int QuantityPerCassetteMaxValue { get; private set; }
        static public int QuantityPerCassetteLength { get; private set; }

        static PrescriptionRegistrationCommandValues()
        {
            CommandId = "PR";
            RegistrationStartNotification = "I";
            PrescriptionIdMinValue = 0;
            PrescriptionIdMaxValue = 9999;
            PrescriptionIdLength = 4;
            TotalNumberOfRegistredCassettesMinValue = 1;
            TotalNumberOfRegistredCassettesMaxValue = 40;
            TotalNumberOfRegistredCassettesLength = 2;
            CassetteIDMinValue = 1;
            CassetteIDMaxValue = 40;
            CassetteIDLength = 2;
            DrugNameLength = 30;
            QuantityPerCassetteMinValue = 1;
            QuantityPerCassetteMaxValue = 99999;
            QuantityPerCassetteLength = 5;
        }
    }
}
