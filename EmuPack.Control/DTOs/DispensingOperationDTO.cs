using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.DTOs
{
    public class DispensingOperationDTO
    {
        [Range(0, 9999)]
        public int PrescriptionId { get; set; }
        [Required]
        public List<DispensingDrugDTO> DispensingDrugDTOs { get; set; }
    }

    public class DispensingDrugDTO
    {
        [Range(1, 40)]
        public int CassetteId { get; set; }
        [Required]
        [StringLength(30)]
        public string DrugName { get; set; }
        [Required]
        public List<UsedPodDTO> UsedPodDTOs { get; set; }
    }

    public class UsedPodDTO
    {
        [Required]
        [RegularExpression("A0|A1|A2|A3|A4|A5|A6|" +
            "B0|B1|B2|B3|B4|B5|B6|" +
            "C0|C1|C2|C3|C4|C5|C6|" +
            "D0|D1|D2|D3|D4|D5|D6|" +
            "E0|E1|E2|E3|E4|E5|E6|")]
        public string PodPosition { get; set; }
        [Range(1, 99)]
        public int QuantityOfDrug { get; set; }
    }
}
