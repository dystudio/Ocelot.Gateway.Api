import { Component, OnInit } from '@angular/core';
import{ Movie } from '../models/movie';
import { MovieApiService } from '../services/movie-api.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-movie-detail',
  templateUrl: './movie-detail.component.html',
  styleUrls: ['./movie-detail.component.sass']
})
export class MovieDetailComponent implements OnInit {
  movie: Movie;  

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private movieService: MovieApiService) { 
  }
  
  
  ngOnInit() {
    const universalID: string = this.route.snapshot.paramMap.get('universalID');
    console.log(universalID);
    this.getMovie(universalID);
  }

  getMovie(universalID: string): void {
    this.movieService.getMovie(universalID)
          .subscribe(movie => this.movie = movie);          
  }
  goBack(): void {
    this.location.back();
  }

}
