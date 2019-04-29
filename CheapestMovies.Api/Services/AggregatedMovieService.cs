using CheapestMovies.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheapestMovies.Api.Services
{
    public interface IAggregatedMovieService
    {      
        Task<Dictionary<string, MoviesList>> GetAggregatedMovies(string url);
        Task<Dictionary<string, MovieDetails>> GetAggregatedMovieDetails(string url, string universalId);
    }
    public class AggregatedMovieService : IAggregatedMovieService
    {
        private IHttpService _httpService;
        public AggregatedMovieService(IHttpService httpService)
        {
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
        }
     
        public async Task<Dictionary<string, MoviesList>> GetAggregatedMovies(string url)
        {
            //Always good to validate the input parameter in public methods
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));

            var allmovies = await _httpService.GetResponse<Dictionary<string, MoviesList>>($"{url}");

            return allmovies;
        }

        public async Task<Dictionary<string, MovieDetails>> GetAggregatedMovieDetails(string url, string universalId)
        {
            //Always good to validate the input parameter in public methods
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(universalId)) throw new ArgumentNullException(nameof(universalId));

            var movieDetailFromAll = await _httpService.GetResponse<Dictionary<string, MovieDetails>>($"{url}/{universalId}");

            return movieDetailFromAll;
        }
    }
}
