import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApplicationWindowComponent } from './application-window/application-window.component';
import { LoggerWindowComponent } from './logger-window/logger-window.component';



@NgModule({
  declarations: [
    ApplicationWindowComponent,
    LoggerWindowComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    LoggerWindowComponent
  ]
})
export class ApplicationWindowsModule { }
