import { TestBed } from '@angular/core/testing';

import { MoviesManagementService } from './movies.management.service';

describe('MoviesManagementService', () => {
  let service: MoviesManagementService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MoviesManagementService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
