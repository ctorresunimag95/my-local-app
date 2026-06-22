import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Movie } from '../reservation/types/movie.types';
import { ReservationService } from '../reservation/services/reservation.service';
import { NzAlign, NzFlexModule, NzJustify } from 'ng-zorro-antd/flex';
import { NzSegmentedModule } from 'ng-zorro-antd/segmented';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalModule } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-home',
  imports: [CommonModule, NzFlexModule, NzSegmentedModule, NzCardModule, NzSpinModule, NzEmptyModule, NzGridModule, NzIconModule, NzModalModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  standalone: true
})
export class HomeComponent {
  movies: Movie[] = [];
  selectedJustification: NzJustify = 'space-evenly';
  selectedLAlignment: NzAlign = 'center';
  isLoading: boolean = false;
  selectedMovie: Movie | null = null;
  isModalVisible: boolean = false;

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

  openMovieModal(movie: Movie): void {
    this.selectedMovie = movie;
    this.isModalVisible = true;
  }

  closeModal(): void {
    this.isModalVisible = false;
    this.selectedMovie = null;
  }
}
