using System.Collections.Generic;

namespace CheapestMovies.Api.Models
{
    public class Movie
    {
        public string UniversalID
        {
            get
            {
                if (!string.IsNullOrEmpty(ID)) return ID.Substring(2);
                return "0";
            }
        }
        public string Title { get; set; }
        public string Year { get; set; }
        public string ID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
        public decimal Price { get; set; }
    }
    public class MoviesCollection
    {
        public List<Movie> Movies { get; set; }
    }
    public class MovieDetail : Movie
    {
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Metascore { get; set; }
        public string Rating { get; set; }
        public string Votes { get; set; }
    }
}
