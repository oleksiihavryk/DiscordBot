import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoggerButtonComponent } from './logger-button.component';

describe('LoggerButtonComponent', () => {
  let component: LoggerButtonComponent;
  let fixture: ComponentFixture<LoggerButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoggerButtonComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoggerButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
