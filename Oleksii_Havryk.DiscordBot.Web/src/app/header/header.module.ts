import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationModule } from './navigation/navigation.module';

import { HeaderWindowComponent } from './header-window/header-window.component';

@NgModule({
  declarations: [
    HeaderWindowComponent
  ],
  imports: [
    CommonModule,
    NavigationModule
  ],
  exports: [
    HeaderWindowComponent
  ]
})
export class HeaderModule { }
