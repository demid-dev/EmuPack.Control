export interface IDispensingOperation {
  prescriptionId: number;
  dispensingDrugs: IDispensingDrug[];
}

export interface IDispensingDrug {
  cassetteId: number;
  drugName: number;
  usedPods: IUsedPod[];
}

export interface IUsedPod {
  podPosition: string;
  quantityOfDrug: number;
}
