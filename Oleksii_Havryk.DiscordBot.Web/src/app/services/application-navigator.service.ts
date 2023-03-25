import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApplicationNavigatorService {
  public allowToViewForLoggerWindow: boolean = true;

  constructor() { }

  public viewLoggerWindow(): void {
    this.allowToViewForLoggerWindow = true;
  }
}
