import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { ApplicationWindowsModule } from './application-windows/application-windows.module';
import { HeaderModule } from './header/header.module';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    HeaderModule,
    ApplicationWindowsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
