using CheapestMovies.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheapestMovies.Api.Services
{
    public interface IAggregatedMovieService
    {      
        Task<Dictionary<string, MoviesCollection>> GetAggregatedMoviesFromAllWorlds(string url);
        Task<Dictionary<string, MovieDetail>> GetAggregatedMovieDetailFromAllWorlds(string url, string universalId);
    }
    public class AggregatedMovieService : IAggregatedMovieService
    {
        private IHttpService _httpService;
        public AggregatedMovieService(IHttpService httpService)
        {
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
        }
     
        public async Task<Dictionary<string, MoviesCollection>> GetAggregatedMoviesFromAllWorlds(string url)
        {
            //Always good to validate the input parameter in public methods
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));

            var moviesFromAll = await _httpService.GetHttpResponse<Dictionary<string, MoviesCollection>>($"{url}");

            return moviesFromAll;
        }

        public async Task<Dictionary<string, MovieDetail>> GetAggregatedMovieDetailFromAllWorlds(string url, string universalId)
        {
            //Always good to validate the input parameter in public methods
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(universalId)) throw new ArgumentNullException(nameof(universalId));

            var movieDetailFromAll = await _httpService.GetHttpResponse<Dictionary<string, MovieDetail>>($"{url}/{universalId}");

            return movieDetailFromAll;
        }
    }
}
