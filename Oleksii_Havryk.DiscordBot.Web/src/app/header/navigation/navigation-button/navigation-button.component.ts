import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-navigation-button',
  templateUrl: './navigation-button.component.html',
  styleUrls: ['./navigation-button.component.css']
})
export class NavigationButtonComponent {
  @Input() public action: Function | undefined;

  public get actionHandler() {
    return () => {
      if (this.action !== undefined)
        this.action();
      return false;
    }
  }
}