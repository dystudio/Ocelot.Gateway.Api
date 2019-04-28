using CheapestMovies.Api.Helpers;
using CheapestMovies.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheapestMovies.Api.Services
{
    public interface IMoviesService
    {
        Task<MoviesList> GetCheapestMovies(string url);
        Task<Movie> GetCheapestMovieDetailById(string url, string id);
    }
    public class MoviesService : IMoviesService
    {
        private IHttpResponse _httpResponse;
        public MoviesService(IHttpResponse httpResponse)
        {
            _httpResponse = httpResponse ?? throw new ArgumentNullException(nameof(httpResponse));
        }

        public async Task<MoviesList> GetCheapestMovies(string url)
        {
            //Always good to validate the input parameter in public methods
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));

            var allmovies = await _httpResponse.GetResponse<Dictionary<string, MoviesList>>($"{url}");

            //return allmovies;

            MoviesList cheapestmovieList = new MoviesList() { Movies = new List<Movie>() };

            //Create union of all IDs and call GetMovieDetailById which will return the lowest price
            //call parallel
            var aggregatedMovie = allmovies.Values.ToList()
                                    .SelectMany(x => x.Movies);

            var groupedMovie = aggregatedMovie.GroupBy(o => o.UniversalID);

            foreach (var movie in groupedMovie)
            {
                //Compare prices only when other movie databases have it
                if (movie.Count() > 1)
                {
                    //Fix configSettings via DI
                    var cheapestMovie = GetCheapestMovieDetailById("http://localhost:2000/api/movie", movie.Key);
                    cheapestmovieList.Movies.Add(cheapestMovie.Result);
                }
            }

            return cheapestmovieList;
        }

        public async Task<Movie> GetCheapestMovieDetailById(string url, string id)
        {
            //Always good to validate the input parameter in public methods
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));


            var movieDetailFromAll = await _httpResponse.GetResponse<Dictionary<string, MovieDetails>>($"{url}/{id}");

            //Init first movie
            MovieDetails movie = new MovieDetails { Price = decimal.MaxValue };
            if (movieDetailFromAll != null && movieDetailFromAll.Count > 0)
            {
                foreach (var item in movieDetailFromAll)
                {
                    movie = movie.Price < item.Value.Price ? movie : item.Value;
                }
            }
            return movie;
        }
    }
}
