export class Movie {
    universalID: string;
    title: string;
    year: string;
    id: string;
    type: string;
    poster: string;    
}

export class MovieDetail extends Movie{
    rated:string;
    released:string;
    genre:string;
    director:string;
    writer:string;
    actors:string;
    plot:string;
    language:string;
    country:string;
    awards:string;
    metascore:string;
    rating:string;
    votes:string;
    price:number;
}