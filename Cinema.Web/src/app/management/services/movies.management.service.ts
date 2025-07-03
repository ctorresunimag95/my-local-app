import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { MoviePublishRequest, MoviePublishResponse, MovieSearchResponse } from '../types/management.movies.types';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MoviesManagementService {
  private apiUrl = `${environment.apiUrls.cinema}/management`;

  constructor(private http: HttpClient) { }

  searchMovie(title: string): Observable<MovieSearchResponse> {
    return this.http.get<MovieSearchResponse>(`${this.apiUrl}/movies/search`, {
      params: { title }
    });
  }

  publishMovie(movie: MoviePublishRequest) : Observable<MoviePublishResponse>{
    return this.http.post<MoviePublishResponse>(`${this.apiUrl}/movies/publish`, movie);
  }
}
