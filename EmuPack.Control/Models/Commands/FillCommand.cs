using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Models.Commands
{
    public class FillCommand: Command
    {
        public string PrescriptionStartNotification { get; private set; }
        public string PrescriptionId { get; private set; }
        public string PodPosition { get; private set; }
        public string DispensingStartNotification { get; private set; }
        public string CassetteStartNotification { get; private set; }
        public string TotalNumberOfCassettes { get; private set; }
        public List<CassetteUsedInFilling> CassettesUsedInFilling { get; private set; }


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

}
