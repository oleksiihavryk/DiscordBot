import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderWindowComponent } from './header-window.component';

describe('HeaderWindowComponent', () => {
  let component: HeaderWindowComponent;
  let fixture: ComponentFixture<HeaderWindowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HeaderWindowComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HeaderWindowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
