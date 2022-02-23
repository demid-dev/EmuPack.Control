export interface IDispensingOperationDTO {
  PrescriptionId: number;
  DispensingDrugDTOs: IDispensingDrugDTO[]
}

export interface IDispensingDrugDTO {
  CassetteId: number;
  DrugName: number;
  UsedPodDTOs: IUsedPodDTO[]
}

export interface IUsedPodDTO {
  PodPosition: string;
  QuantityOfDrug: number;
}
