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
        Task<Dictionary<string, MovieDetails>> GetMovieDetailById(string url, string id);
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
            foreach (var movieProvider in allmovies)
            {
                //write logic to get cheapest movies of all
                //Open Threads
                //change return type to w/o dictionary
            }
            return allmovies;
        }

        public async Task<Dictionary<string, MovieDetails>> GetMovieDetailById(string url, string id)
        {
            //Always good to validate the input parameter in public methods
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            return await _httpResponse.GetResponse<Dictionary<string, MovieDetails>>($"{url}/{id}");
        }
    }
}
