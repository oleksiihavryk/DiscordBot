import { TestBed } from '@angular/core/testing';

import { ApplicationNavigatorService } from './application-navigator.service';

describe('ApplicationNavigatorService', () => {
  let service: ApplicationNavigatorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApplicationNavigatorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
