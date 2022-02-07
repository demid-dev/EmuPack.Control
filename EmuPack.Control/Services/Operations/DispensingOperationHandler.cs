using EmuPack.Control.DTOs;
using EmuPack.Control.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Services.Operations
{
    public class DispensingOperationHandler
    {
        public void Dispense(MachineClient client,
            DispensingOperationDTO dto)
        {
            RequestStatus(client);

            RegistratePrescription(client, MapDispensingDtoToRegistrationDto(dto));
            MapDispensingDtoToFillDtos(dto).ForEach(fillDto => ExecuteFilling(client, fillDto));
        }

        private void RequestStatus(MachineClient client)
        {
            StatusRequestCommand command = new StatusRequestCommand();
            client.SendCommand(command);
        }

        public PrescriptionRegistrationCommandDTO MapDispensingDtoToRegistrationDto(DispensingOperationDTO dispensingDTO)
        {
            return new PrescriptionRegistrationCommandDTO
            {
                PrescriptionId = dispensingDTO.PrescriptionId,
                RxRegistrationCommandDrugsDTOs = MapDispensingDrugToPrescriptionDrug(dispensingDTO.DispensingDrugDTOs)
            };
        }

        private List<PrescriptionRegistrationDrugDTO> MapDispensingDrugToPrescriptionDrug
            (List<DispensingDrugDTO> dispensingDrugsDTOs)
        {
            List<PrescriptionRegistrationDrugDTO> prescriptionDrugs = new List<PrescriptionRegistrationDrugDTO>();
            dispensingDrugsDTOs.ForEach(dispensingDrug =>
            {
                PrescriptionRegistrationDrugDTO alreadyExistDrug = prescriptionDrugs
                    .SingleOrDefault(drug => drug.CassetteId == dispensingDrug.CassetteId);

                if (alreadyExistDrug == null)
                {
                    prescriptionDrugs.Add(new PrescriptionRegistrationDrugDTO
                    {
                        CassetteId = dispensingDrug.CassetteId,
                        DrugName = dispensingDrug.DrugName,
                        QuantityPerCassette = dispensingDrug.UsedPodDTOs.Sum(pod => pod.QuantityOfDrug)
                    });
                }
                else
                {
                    alreadyExistDrug.QuantityPerCassette += dispensingDrug.UsedPodDTOs.Sum(pod => pod.QuantityOfDrug);
                }
            });

            return prescriptionDrugs;
        }

        public List<FillCommandDTO> MapDispensingDtoToFillDtos(DispensingOperationDTO dispensingDTO)
        {
            List<FillCommandDTO> fillCommandDTOs = new List<FillCommandDTO>();
            List<string> usedPodsPositions = dispensingDTO.DispensingDrugDTOs
                .SelectMany(drug => drug.UsedPodDTOs.Select(pod => pod.PodPosition))
                    .Distinct()
                    .ToList();

            usedPodsPositions.ForEach(podPosition =>
            {
                FillCommandDTO dto = new FillCommandDTO();
                dto.CassetteUsedInFilling = new List<CassetteUsedInFillingDTO>();
                List<DispensingDrugDTO> dispensingDrugs = dispensingDTO.DispensingDrugDTOs
                    .Where(drug => drug.UsedPodDTOs.Select(d => d.PodPosition).Contains(podPosition))
                        .ToList();

                dispensingDrugs.ForEach(drug =>
                {
                    dto.CassetteUsedInFilling.Add(new CassetteUsedInFillingDTO
                    {
                        QuantityOfDrug = drug.UsedPodDTOs.Sum(pod => pod.QuantityOfDrug),
                        CassetteId = drug.CassetteId
                    });
                    dto.PodPosition = podPosition;
                    dto.PrescriptionId = dispensingDTO.PrescriptionId;
                });
                fillCommandDTOs.Add(dto);
            });

            return fillCommandDTOs;
        }

        private void RegistratePrescription(MachineClient client,
            PrescriptionRegistrationCommandDTO dto)
        {
            PrescriptionRegistrationCommand command = new PrescriptionRegistrationCommand(dto);
            client.SendCommand(command);
        }

        private void ExecuteFilling(MachineClient client,
            FillCommandDTO dto)
        {
            FillCommand command = new FillCommand(dto);
            client.SendCommand(command);
        }
    }
}
