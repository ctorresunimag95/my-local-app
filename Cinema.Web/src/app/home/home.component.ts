import { Component } from '@angular/core';
import { Movie } from '../reservation/types/movie.types';
import { ReservationService } from '../reservation/services/reservation.service';
import { NzAlign, NzFlexModule, NzJustify } from 'ng-zorro-antd/flex';
import { NzSegmentedModule } from 'ng-zorro-antd/segmented';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzGridModule } from 'ng-zorro-antd/grid';

@Component({
  selector: 'app-home',
  imports: [NzFlexModule, NzSegmentedModule, NzCardModule, NzSpinModule, NzEmptyModule, NzGridModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  standalone: true
})
export class HomeComponent {
  movies: Movie[] = [];
  selectedJustification: NzJustify = 'space-evenly';
  selectedLAlignment: NzAlign = 'center';
  isLoading: boolean = false;

  constructor(private reservationService: ReservationService) {
  }

  ngOnInit() {
    this.loadMovies();
  }

  loadMovies(): void {
    this.isLoading = true;
    this.reservationService.getMovies().subscribe({
      next: (movies) => {
        this.movies = movies;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load movies', err);
        this.isLoading = false;
      }
    });
  }
}
