using EmuPack.Control.DTOs;
using EmuPack.Control.Models.Commands;
using EmuPack.Control.Models.Machine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmuPack.Control.Services.Operations
{
    public class DispensingOperationHandler
    {
        private readonly MachineClient _machineClient;
        private readonly StatusOperationHandler _statusHandler;
        private readonly InitializationOperationHandler _initializationHandler;
        private readonly NotificationService _notificationService;

        public DispensingOperationHandler(MachineClient machineClient,
            StatusOperationHandler statusHandler,
            InitializationOperationHandler initializationHandler,
            NotificationService notificationService)
        {
            _machineClient = machineClient;
            _statusHandler = statusHandler;
            _initializationHandler = initializationHandler;
            _notificationService = notificationService;
        }

        public void Dispense(DispensingOperationDTO dto)
        {
            if (DispensingIsPossible(dto))
            {
                RegistratePrescription(MapDispensingDtoToRegistrationDto(dto));
                MapDispensingDtoToFillDtos(dto).ForEach(fillDto => ExecuteFilling(fillDto));
            }
            if (_machineClient.ConnectedToMachine)
            {
                ChangeDrawerStatus(drawerLocked: false);
                _statusHandler.UpdateMachineState();
            }
        }

        private bool DispensingIsPossible(DispensingOperationDTO dto)
        {
            bool adaptorInDrawer = true;
            bool prescriptionNotRegistred = true;
            if (!_machineClient.ConnectedToMachine)
            {
                _initializationHandler.InitializeMachine();
            }
            if (!_machineClient.ConnectedToMachine)
            {
                return false;
            }
            _statusHandler.UpdateMachineState();
            if (_machineClient.MachineState.DrawerOpened)
            {
                ChangeDrawerStatus(drawerLocked: true);
            }
            if (!_machineClient.MachineState.AdaptorInDrawer)
            {
                adaptorInDrawer = false;
            }
            if (_machineClient.MachineState.RegistredPrescriptionsIds
                .Contains(dto.PrescriptionId.ToString()))
            {
                prescriptionNotRegistred = false;
            }
            _notificationService.SendDispensingNotification(adaptorInDrawer,
                prescriptionNotRegistred);

            return adaptorInDrawer && prescriptionNotRegistred;
        }

        private void ChangeDrawerStatus(bool drawerLocked)
        {
            MachineActivityRequestCommandDTO commandDto = new MachineActivityRequestCommandDTO
            {
                DrawerShouldBeLocked = drawerLocked
            };
            MachineActivityRequestCommand command = new MachineActivityRequestCommand(commandDto);
            _machineClient.SendCommand(command);
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
                        QuantityOfDrug = drug.UsedPodDTOs
                            .First(pod => pod.PodPosition == podPosition).QuantityOfDrug,
                        CassetteId = drug.CassetteId
                    });
                    dto.PodPosition = podPosition;
                    dto.PrescriptionId = dispensingDTO.PrescriptionId;
                });
                fillCommandDTOs.Add(dto);
            });

            return fillCommandDTOs;
        }

        private void RegistratePrescription(PrescriptionRegistrationCommandDTO dto)
        {
            PrescriptionRegistrationCommand command = new PrescriptionRegistrationCommand(dto);
            _machineClient.SendCommand(command);
        }

        private void ExecuteFilling(FillCommandDTO dto)
        {
            FillCommand command = new FillCommand(dto);
            _machineClient.SendCommand(command);
        }
    }
}
