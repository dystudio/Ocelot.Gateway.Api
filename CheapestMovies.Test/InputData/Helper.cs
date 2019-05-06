using CheapestMovies.Api.Models;
using System.Collections.Generic;

namespace CheapestMovies.Test.InputData
{
    public class Helper
    {
        public static Dictionary<string, MoviesCollection> GetMockMovies(int worldCount, int movieCount)
        {


            var allMovies = new Dictionary<string, MoviesCollection>();
            for (int i = 0; i < worldCount; i++)
            {
                var movieCollection = new MoviesCollection { Movies = new List<Movie>() };
                for (int j = 0; j < movieCount; j++)
                {
                    movieCollection.Movies.Add(new Movie() { ID = $"cw{i.ToString()}{j.ToString()}123" });
                }
                allMovies.Add(i.ToString(), movieCollection);
            }
            return allMovies;
        }
        public static Dictionary<string, MoviesCollection> GetMockMoviesWithDuplicates(int worldCount, int movieCount, int duplicateCount)
        {
            var allMovies = GetMockMovies(worldCount, movieCount);
            foreach (var item in allMovies)
            {
                for (int i = 0; i < duplicateCount; i++)
                {
                    item.Value.Movies[i].ID = "cw" + i.ToString();
                }
            }
            return allMovies;
        }
        public static Dictionary<string, MovieDetail> GetMockMoviesWithPrice(decimal cheapestPrice, decimal higherPrice)
        {
            var movieDetail = new Dictionary<string, MovieDetail>();
            movieDetail.Add("cw", new MovieDetail() { Price = cheapestPrice });
            movieDetail.Add("fw", new MovieDetail() { Price = higherPrice });
            movieDetail.Add("mw", new MovieDetail() { Price = higherPrice });

            return movieDetail;
        }
        public static Dictionary<string, MovieDetail> GetMockMovieDetail(int worldCount)
        {
            var movieDetail = new Dictionary<string, MovieDetail>();
            for (int i = 0; i < worldCount; i++)
            {
                movieDetail.Add($"{i}cw", new MovieDetail());
            }
            return movieDetail;
        }
        public static List<Movie> GetMockMoviesAggregated(int count)
        {
            var movies = new List<Movie>();
            for (int i = 0; i < count; i++)
            {
                movies.Add(new Movie());
            }
            return movies;
        }
    }
}
