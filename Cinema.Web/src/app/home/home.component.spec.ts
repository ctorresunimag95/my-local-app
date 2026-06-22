import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { HomeComponent } from './home.component';
import { ReservationService } from '../reservation/services/reservation.service';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let reservationService: jasmine.SpyObj<ReservationService>;

  beforeEach(async () => {
    reservationService = jasmine.createSpyObj<ReservationService>('ReservationService', ['getMovies']);
    reservationService.getMovies.and.returnValue(of([]));

    await TestBed.configureTestingModule({
      imports: [HomeComponent],
      providers: [{ provide: ReservationService, useValue: reservationService }]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render selected movie details in the modal', () => {
    component.selectedMovie = {
      id: '1',
      name: 'Inception',
      description: 'Mind-bending sci-fi',
      genre: 'Sci-Fi',
      posterUrl: 'poster.jpg'
    };
    component.isModalVisible = true;
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Inception');
    expect(compiled.textContent).toContain('Mind-bending sci-fi');
    expect(compiled.textContent).toContain('Sci-Fi');
  });
});
