import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BotSwitchButtonComponent } from './bot-switch-button.component';

describe('BotSwitchButtonComponent', () => {
  let component: BotSwitchButtonComponent;
  let fixture: ComponentFixture<BotSwitchButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BotSwitchButtonComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BotSwitchButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
