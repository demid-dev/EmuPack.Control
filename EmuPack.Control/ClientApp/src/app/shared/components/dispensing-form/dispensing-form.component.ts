import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {Form, FormArray, FormControl, FormGroup, Validators} from "@angular/forms";
import {IDispensingOperation, IUsedPod} from "../../models/dispensing-operation";
import {noPodGreaterThanZero} from "../../validators/used-pods.validator";
import {IDispensingDrugDTO, IDispensingOperationDTO, IUsedPodDTO} from "../../api-models/dispensing-operation-dto";
import {CommandsService} from "../../services/commands.service";

@Component({
  selector: 'app-dispensing-form',
  templateUrl: './dispensing-form.component.html',
  styleUrls: ['./dispensing-form.component.scss']
})
export class DispensingFormComponent implements OnInit {
  form: FormGroup;
  // @ts-ignore
  @ViewChild('submitButton') submitButton: ElementRef<HTMLElement>;

  public constructor(private commandsService: CommandsService) {
    this.form = this.createForm();
  }

  ngOnInit(): void {
  }

  createForm(): FormGroup {
    return new FormGroup({
      prescriptionId: new FormControl(1, [Validators.required, Validators.min(1), Validators.max(9999)]),
      dispensingDrugs: new FormArray([this.createDispensingDrug()])
    });
  }

  addDispensingDrug() {
    (this.form.get(`dispensingDrugs`) as FormArray).push(this.createDispensingDrug());
  }

  createDispensingDrug(): FormGroup {
    const dispensingDrug = new FormGroup({
      cassetteId: new FormControl(1, [Validators.required, Validators.min(1), Validators.max(40)]),
      drugName: new FormControl(``, [Validators.required, Validators.maxLength(30)]),
      usedPods: new FormArray(this.createUsedPodsArray().controls, noPodGreaterThanZero())
    });

    return dispensingDrug;
  }

  createUsedPodsArray(): FormArray {
    const usedPods = new FormArray([]);
    for (let i = 0; i < 7; i++) {
      usedPods.push(new FormGroup({
        podPosition: new FormControl(`A` + i),
        quantityOfDrug: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(99)])
      }));
      usedPods.push(new FormGroup({
        podPosition: new FormControl(`B` + i),
        quantityOfDrug: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(99)])
      }));
      usedPods.push(new FormGroup({
        podPosition: new FormControl(`C` + i),
        quantityOfDrug: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(99)])
      }));
      usedPods.push(new FormGroup({
        podPosition: new FormControl(`D` + i),
        quantityOfDrug: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(99)])
      }));
    }

    return usedPods;
  }

  getDispensingDrugsArrayControls() {
    return (this.form.get('dispensingDrugs') as FormArray).controls;
  }

  getDispensingDrugsControls(idx: number): FormGroup {
    return ((this.form.get('dispensingDrugs') as FormArray).controls[idx] as FormGroup);
  }

  getUsedPodControls(formGroup: FormGroup, idx: number): FormGroup {
    return ((formGroup.get('usedPods') as FormArray).controls[idx] as FormGroup);
  }

  submit() {
    this.lockSubmitButton(this.submitButton);

    let dispensingOperation: IDispensingOperation = {
      prescriptionId: this.form.get('prescriptionId')?.value,
      dispensingDrugs: this.form.get('dispensingDrugs')?.value
    };

    dispensingOperation.dispensingDrugs.forEach((drug => {
      let array: IUsedPod[] = [];
      drug.usedPods.forEach((pod) => {
        if (pod.quantityOfDrug > 0) {
          array.push(pod);
        }
      })
      drug.usedPods = array;
    }));


    this.dispense(dispensingOperation);
  }

  dispense(dispensingOperation: IDispensingOperation) {
    let dispensingDrugDTOs: IDispensingDrugDTO[] = [];

    dispensingOperation.dispensingDrugs.forEach(drug => {
      let usedPodsDTOs: IUsedPodDTO[] = [];
      drug.usedPods.forEach(usedPod => {
        let usedPodDTO: IUsedPodDTO = {
          PodPosition: usedPod.podPosition,
          QuantityOfDrug: usedPod.quantityOfDrug
        };
        usedPodsDTOs.push(usedPodDTO);
      });

      let drugDTO: IDispensingDrugDTO = {
        CassetteId: drug.cassetteId,
        DrugName: drug.drugName,
        UsedPodDTOs: usedPodsDTOs
      };

      dispensingDrugDTOs.push(drugDTO);
    });

    this.commandsService.dispense({
      PrescriptionId: dispensingOperation.prescriptionId,
      DispensingDrugDTOs: dispensingDrugDTOs
    }).subscribe(() => {
    });
  }

  clearForm() {
    this.form = this.createForm();
  }

  lockSubmitButton(elem: ElementRef) {
    elem.nativeElement.disabled = true;
    setTimeout(()=>{
      elem.nativeElement.disabled = false;
    }, 3000);
  }
}
