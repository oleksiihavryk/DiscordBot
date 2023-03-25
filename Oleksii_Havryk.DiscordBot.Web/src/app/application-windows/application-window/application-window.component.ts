import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-application-window',
  templateUrl: './application-window.component.html',
  styleUrls: ['./application-window.component.css']
})
export class ApplicationWindowComponent {
  @Input() public title: string = '';
}
