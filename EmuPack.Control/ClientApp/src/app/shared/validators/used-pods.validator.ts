import {AbstractControl, FormArray, ValidationErrors, ValidatorFn} from "@angular/forms";

export function noPodGreaterThanZero(): ValidatorFn | null {
  return (controls: AbstractControl): ValidationErrors | null => {
    let formArray = controls as FormArray;
    if (!formArray.controls.some(control => control.get('quantityOfDrug')?.value > 0)) {
      return {noPodGreaterThanZero: true};
    }
    return null;
  }
}
