import { Component } from '@angular/core';
import { LoggerMessage, LoggerService } from 'src/app/services/logger.service';

@Component({
  selector: 'app-logger-window',
  templateUrl: './logger-window.component.html',
  styleUrls: ['./logger-window.component.css']
})
export class LoggerWindowComponent {
  constructor(public loggerService: LoggerService) {}

  updateOld() {
    this.loggerService.requestUpdateOld();
    return false;
  }
  updateNew() {
    this.loggerService.requestUpdateNew();
    return false;
  }
}
