import {Injectable} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  // @ts-ignore
  public hubConnection: signalR.HubConnection;

  constructor() {
  }

  public async startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(environment.hubUrl).build()

    await this.hubConnection
      .start()
      .then(() => console.log(`Connection started`))
      .catch((err) => console.log(`Error while starting connection: `, err))
  }
}
