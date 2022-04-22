import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {SignalRService} from "../../shared/services/signal-r.service";
import {Notification} from "../../shared/models/notification";
import {CommandsService} from "../../shared/services/commands.service";

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
  // @ts-ignore
  @ViewChild('reinitializeButton') reinitializeButton: ElementRef<HTMLElement>;

  constructor(private signalRService: SignalRService,
              private commandsService: CommandsService) {
  }

  async ngOnInit(): Promise<void> {
    await this.signalRService.startConnection();
    this.addNotificationListener();
  }

  private addNotificationListener() {
    this.signalRService.hubConnection.on('ReceiveNotification',
      (notification: Notification) => {
        this.processNotification(notification);
        this.createNotificationModal();
      })
  }

  private processNotification(notification: Notification) {

    const notificationTypes = {
      0: "Tcp connection error occurred",
      1: "Tcp sending communication error occurred",
      2: "Tcp receiving communication error occurred",
      3: "Machine blocked command",
      4: "Dispensing not possible",
      5: "Dispensing was successful",
      6: "Cassette warning occurred",
      7: "Initialization was successful"
    };

    // @ts-ignore
    this.notificationTitle = notificationTypes[notification.notificationType];
    this.notificationDescription = '';
    notification.warningFields.forEach(warningField => {
      this.notificationDescription += ` ${warningField.fieldName}: ${warningField.value}\n`;
    });

    if (notification.notificationType == 1) {
      this.notificationDescription = 'Error happened while sending tcp message'
    }
    if (notification.notificationType == 2) {
      this.notificationDescription = 'Error happened while receiving tcp message'
    }
    if (notification.notificationType == 5) {
      this.notificationDescription = 'Dispensing operation completed, open the drawer';
    }
    if (notification.notificationType == 7) {
      this.notificationDescription = 'Machine state was reinitialized'
    }
  }

  private createNotificationModal() {
    this.content.nativeElement.click();
  }

  reinitialize() {
    this.lockReinitializeButton(this.reinitializeButton);

    this.commandsService.reinitialize()
      .subscribe(() => {
      });
  }

  private lockReinitializeButton(elem: ElementRef) {
    elem.nativeElement.disabled = true;
    setTimeout(() => {
      elem.nativeElement.disabled = false;
    }, 3000);
  }
}
