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
        Task<Dictionary<string, MoviesList>> GetCheapestMovies(string url);
        Task<Movie> GetCheapestMovieDetailById(string url, string id);
    }
    public class MoviesService : IMoviesService
    {
        private IHttpResponse _httpResponse;
        public MoviesService(IHttpResponse httpResponse)
        {
            _httpResponse = httpResponse ?? throw new ArgumentNullException(nameof(httpResponse));
        }

        public async Task<Dictionary<string, MoviesList>> GetCheapestMovies(string url)
        {
            var allmovies = await _httpResponse.GetResponse<Dictionary<string, MoviesList>>($"{url}");
            MoviesList cheapestmovieList = new MoviesList() { Movies = new List<Movie>() };

            ////Create union of all IDs and call GetMovieDetailById which will return the lowest price
            ////call parallel
            ////NOT GOOD. change the logic as the first collection could be null
            //foreach (var movies in allmovies.Values)
            //{
            //    foreach (var movie in movies.Movies)
            //    {
            //        //Fix configSettings via DI
            //        var cheapestMovie = await GetCheapestMovieDetailById("http://localhost:2000/api/movie", movie.UniversalID);
            //        cheapestmovieList.Movies.Add(cheapestMovie);
            //    }
            //}


            return allmovies;
        }

        public async Task<Movie> GetCheapestMovieDetailById(string url, string id)
        {
            //Always good to validate the input parameter in public methods
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));


            var movieDetailFromAll = await _httpResponse.GetResponse<Dictionary<string, MovieDetails>>($"{url}/{id}");

            //Init first movie
            MovieDetails movie = movieDetailFromAll.First().Value;

            //Skip first as it's already there in "movie" variable
            foreach (var item in movieDetailFromAll.Skip(1))
            {
                movie = movie.Price < item.Value.Price ? movie : item.Value;
            }
            return movie;
        }
    }
}
