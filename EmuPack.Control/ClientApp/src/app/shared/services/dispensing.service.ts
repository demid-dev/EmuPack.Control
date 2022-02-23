import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {IDispensingOperationDTO} from "../api-models/dispensing-operation-dto";

@Injectable({
  providedIn: 'root'
})
export class DispensingService {


  constructor(private http: HttpClient) {
  }

  public dispense(dispensingOperation: IDispensingOperationDTO) {
    return this.http.post(`${environment.api}/Commands/dispense`, dispensingOperation);
  }
}
