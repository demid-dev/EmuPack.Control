import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {IDispensingOperationDTO} from "../api-models/dispensing-operation-dto";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class CommandsService {

  constructor(private http: HttpClient) {
  }

  public dispense(dispensingOperation: IDispensingOperationDTO) {
    return this.http.post(`${environment.api}/Commands/dispense`, dispensingOperation);
  }

  public reinitialize() {
    return this.http.post(`${environment.api}/Commands/reinitialize`, null);
  }
}
