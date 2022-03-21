import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {SignalRService} from "../../shared/services/signal-r.service";
import {Notification} from "../../shared/models/notification";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  // @ts-ignore
  public notificationTitle: any;
  // @ts-ignore
  public notificationDescription: string;
  public notificationActive: boolean = false;

  // @ts-ignore
  @ViewChild('modalButton') content: ElementRef<HTMLElement>;

  constructor(private signalRService: SignalRService) {
  }

  async ngOnInit(): Promise<void> {
    await this.signalRService.startConnection();
    this.addNotificationListener();
  }

  private addNotificationListener() {
    this.signalRService.hubConnection.on('ReceiveNotification',
      (notification: Notification) => {
        if (!this.notificationActive) {
          this.processNotification(notification);
          this.createNotificationModal();
          console.log(notification);
        }
      })
  }

  private processNotification(notification: Notification) {
    this.notificationActive = true;

    const notificationTypes = {
      0: "Tcp connection error occurred",
      1: "Tcp sending communication error occurred",
      2: "Tcp receiving communication error occurred",
      3: "Machine blocked command",
      4: "Dispensing not possible",
      5: "Dispensing was successful",
      6: "Cassette warning occurred"
    };

    // @ts-ignore
    this.notificationTitle = notificationTypes[notification.notificationType];
    this.notificationDescription = '';
    notification.warningFields.forEach(warningField => {
      this.notificationDescription += ` ${warningField.fieldName}: ${warningField.value}\n`;
    });

    if (notification.notificationType == 5) {
      console.log('hgh')
      this.notificationDescription = 'Dispensing operation completed, open the drawer'
    }

    console.log(this.notificationTitle);
  }

  private createNotificationModal() {
    this.content.nativeElement.click();
  }
}
