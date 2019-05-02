using CheapestMovies.Api.Models;
using CheapestMovies.Api.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheapestMovies.Api.Managers
{
    public interface IMovieManager
    {
        Task<IEnumerable<Movie>> GetAggregatedMovies();
        Task<Dictionary<string, MovieDetail>> GetAggregatedMovieDetail(string universalId);
        Task<Movie> GetCheapestMovie(string universalId);
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
            var moviesFromAllWorlds = await _movieService.GetAggregatedMoviesFromAllWorlds(_ocelotSettings.MoviesUrl);

            var allMoviesList = moviesFromAllWorlds.Values.ToList();
            allMoviesList.RemoveAll(world => world == null);
            var uniqueMovies = allMoviesList.SelectMany(x => x.Movies)
                                .GroupBy(o => o.UniversalID)
                                .Select(y => y.First());

            return uniqueMovies;
        }
        public async Task<Dictionary<string, MovieDetail>> GetAggregatedMovieDetail(string universalId)
        {
            return await _movieService.GetAggregatedMovieDetailFromAllWorlds(_ocelotSettings.MoviesUrl, universalId);
        }

        public async Task<Movie> GetCheapestMovie(string universalId)
        {
            var movieDetailFromAll = await GetAggregatedMovieDetail(universalId);

            return CompareAndFindCheapest(movieDetailFromAll);
        }
        private MovieDetail CompareAndFindCheapest(Dictionary<string, MovieDetail> movieDetailFromAll)
        {
            if (movieDetailFromAll == null || movieDetailFromAll.Count == 0) { return null; }

            MovieDetail movie = new MovieDetail { Price = decimal.MaxValue };
            foreach (var item in movieDetailFromAll)
            {
                if (item.Value != null) { movie = movie.Price < item.Value.Price ? movie : item.Value; }
            }

            return movie;
        }
    }
}
