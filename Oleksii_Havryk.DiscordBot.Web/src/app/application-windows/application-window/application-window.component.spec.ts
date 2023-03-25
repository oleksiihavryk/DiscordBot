import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationWindowComponent } from './application-window.component';

describe('ApplicationWindowComponent', () => {
  let component: ApplicationWindowComponent;
  let fixture: ComponentFixture<ApplicationWindowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplicationWindowComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApplicationWindowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
