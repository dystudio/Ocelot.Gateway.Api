import { Component, OnInit } from '@angular/core';
import{ Movie } from '../models/movie';
import { MovieApiService } from '../services/movie-api.service';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.css']
})
export class MoviesComponent implements OnInit {
  movies: Movie[];  
  constructor(private movieService: MovieApiService) { }

  ngOnInit() {
    this.getMovies();
  }
  getMovies(): void {
    this.movieService.getMovies()
      .subscribe(movies => this.movies = movies);
  }
}
