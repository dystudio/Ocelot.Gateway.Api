using CheapestMovies.Api.Extensions;
using CheapestMovies.Api.Models;
using CheapestMovies.Api.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheapestMovies.Api.Managers
{
    public interface IMovieManager
    {
        Task<IEnumerable<Movie>> GetAggregatedMovies();
        Task<Dictionary<string, MovieDetail>> GetAggregatedMovieDetail(string universalId);
        Task<MovieDetail> GetCheapestMovie(string universalId);
    }
    public class MovieManager : IMovieManager
    {
        private readonly IConfigService _configService;
        private readonly IAggregatedMovieService _movieService;
        private OcelotGatewayApi _ocelotSettings => _configService.GetSection<OcelotGatewayApi>(nameof(OcelotGatewayApi));

        public MovieManager(IConfigService configService, IAggregatedMovieService movieService)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
            _configService = configService ?? throw new ArgumentNullException(nameof(configService));
        }

        public async Task<IEnumerable<Movie>> GetAggregatedMovies()
        {
            Dictionary<string, MoviesCollection> moviesFromAllWorlds = null;
            try
            {
                moviesFromAllWorlds = await _movieService.GetAggregatedMoviesFromAllWorlds(_ocelotSettings.MoviesUrl);
            }
            catch (Exception)
            {
                // Yell    Log    Catch  Throw     
            }
            if (moviesFromAllWorlds == null) return null;

            var uniqueMovies = moviesFromAllWorlds.GetUniqueMovies();

            return uniqueMovies;
        }

        public async Task<Dictionary<string, MovieDetail>> GetAggregatedMovieDetail(string universalId)
        {
            try
            {
                return await _movieService.GetAggregatedMovieDetailFromAllWorlds(_ocelotSettings.MoviesUrl, universalId);
            }
            catch (Exception)
            {
                // Yell    Log    Catch  Throw     
            }
            return null;
        }

        public async Task<MovieDetail> GetCheapestMovie(string universalId)
        {
            Dictionary<string, MovieDetail> movieDetailFromAll = null;
            try
            {
                movieDetailFromAll = await GetAggregatedMovieDetail(universalId);
            }
            catch (Exception)
            {
                // Yell    Log    Catch  Throw     
            }
            if (movieDetailFromAll == null) return null;

            return movieDetailFromAll.CompareAndFindCheapest();
        }
    }
}
