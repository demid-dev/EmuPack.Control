using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.DTOs
{
    public class PrescriptionRegistrationCommandDTO
    {
        [Range(0, 9999)]
        public int PrescriptionId { get; set; }
        [Required]
        public List<PrescriptionRegistrationDrugDTO> RxRegistrationCommandDrugsDTOs { get; set; }
    }

    public class PrescriptionRegistrationDrugDTO
    {
        [Range(1, 40)]
        public int CassetteId { get; set; }
        [Required]
        [StringLength(30)]
        public string DrugName { get; set; }
        [Range(1, 99999)]
        public int QuantityPerCassette { get; set; }
    }
}
