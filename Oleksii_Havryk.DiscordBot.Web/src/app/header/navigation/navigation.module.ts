import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationBarComponent } from './navigation-bar/navigation-bar.component';
import { NavigationButtonComponent } from './navigation-button/navigation-button.component';
import { BotSwitchButtonComponent } from './bot-switch-button/bot-switch-button.component';
import { LoggerButtonComponent } from './logger-button/logger-button.component';



@NgModule({
  declarations: [
    NavigationBarComponent,
    NavigationButtonComponent,
    BotSwitchButtonComponent,
    LoggerButtonComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    NavigationBarComponent
  ]
})
export class NavigationModule { }
