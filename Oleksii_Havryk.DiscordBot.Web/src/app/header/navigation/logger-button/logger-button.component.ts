import { Component } from '@angular/core';

import { ApplicationNavigatorService } from 'src/app/services/application-navigator.service';

@Component({
  selector: 'app-logger-button',
  templateUrl: './logger-button.component.html',
  styleUrls: ['./logger-button.component.css']
})
export class LoggerButtonComponent {
  constructor(private applicationNavigator: ApplicationNavigatorService) {}

  public viewWindow() {
    this.applicationNavigator.viewLoggerWindow();
  }
}
