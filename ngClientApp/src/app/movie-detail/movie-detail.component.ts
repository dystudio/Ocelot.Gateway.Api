import { Component, OnInit } from '@angular/core';
import { MovieDetail } from '../models/movie';
import { MovieApiService } from '../services/movie-api.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';


@Component({
  selector: 'app-movie-detail',
  templateUrl: './movie-detail.component.html',
  styleUrls: ['./movie-detail.component.css']
})
export class MovieDetailComponent implements OnInit {
  movie: MovieDetail;  

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private movieService: MovieApiService) { 
  }
  
  
  ngOnInit() {
    const universalID: string = this.route.snapshot.paramMap.get('universalID');
    this.getMovie(universalID);
  }

  getMovie(universalID: string): void {
    this.movieService.getMovie(universalID)          
          .subscribe(moviedetail => this.movie = moviedetail);  
  }
  goBack(): void {
    this.location.back();
  }
}
