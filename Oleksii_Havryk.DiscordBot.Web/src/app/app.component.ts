import { Component } from '@angular/core';
import { ApplicationNavigatorService } from './services/application-navigator.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(public applicationNavigator: ApplicationNavigatorService) {}
}
