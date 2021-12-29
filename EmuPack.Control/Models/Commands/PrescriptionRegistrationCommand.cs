using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Models.Commands
{
    public class PrescriptionRegistrationCommand : Command
    {
        public string RegistrationStartNotification { get; private set; }
        public string PrescriptionId { get; private set; }
        public string TotalNumberOfRegistredCassettes { get; private set; }
        public List<RxRegistrationCommandDrug> RxRegistrationCommandDrugs { get; private set; }
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
}
