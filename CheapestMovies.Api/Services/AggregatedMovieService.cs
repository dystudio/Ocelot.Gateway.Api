using CheapestMovies.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheapestMovies.Api.Services
{
    public interface IAggregatedMovieService
    {
        Task<Dictionary<string, MoviesCollection>> GetAggregatedMoviesFromAllWorlds(string url);
        Task<Dictionary<string, MovieDetail>> GetAggregatedMovieDetailFromAllWorlds(string url, string universalId);
    }

    
    /// <summary>
    /// The class sits above the plain http response and future data filtering 
    /// should be implemented in this class if needed.
    /// </summary>
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

            Dictionary<string, MoviesCollection> moviesFromAll = null;
            try
            {
                moviesFromAll = await _httpService.GetHttpResponse<Dictionary<string, MoviesCollection>>($"{url}");
            }
            catch (Exception)
            {
                // Yell    Log    Catch  Throw     
            }

            return moviesFromAll;
        }

        public async Task<Dictionary<string, MovieDetail>> GetAggregatedMovieDetailFromAllWorlds(string url, string universalId)
        {
            //Always good to validate the input parameter in public methods
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(universalId)) throw new ArgumentNullException(nameof(universalId));

            Dictionary<string, MovieDetail> movieDetailFromAll = null;
            try
            {
                movieDetailFromAll = await _httpService.GetHttpResponse<Dictionary<string, MovieDetail>>($"{url}/{universalId}");
            }
            catch (Exception)
            {
                // Yell    Log    Catch  Throw     
            }

            return movieDetailFromAll;
        }
    }
}
