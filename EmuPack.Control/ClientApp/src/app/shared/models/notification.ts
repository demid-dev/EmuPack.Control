export interface Notification {
  notificationType: number;
  warningFields: WarningField[];
  timestamp: string;
}

export interface WarningField {
  fieldName: string;
  value: string;
}
