import { Component, OnInit } from '@angular/core';
import{ Movie } from '../models/movie';
import { MovieApiService } from '../services/movie-api.service';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.css']
})
export class MoviesComponent implements OnInit {
  movies: Observable<Movie[]>;  
  constructor(private movieService: MovieApiService) { }

  ngOnInit() {
    this.getMovies();
  }
  getMovies(): void {
    // this.movieService.getMovies()
    //   .subscribe(movies => this.movies = movies);
    this.movies = this.movieService.getMovies();
  }
}
