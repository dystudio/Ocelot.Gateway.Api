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
        Task<Dictionary<string, MoviesCollection>> GetAggregatedMovies();
        Task<Dictionary<string, MovieDetail>> GetAggregatedMovieDetail(string universalId);
        Task<MoviesCollection> GetCheapestMovies();
        Task<Movie> GetCheapestMovie(string universalId);
    }
    public class MovieManager : IMovieManager
    {
        private readonly IConfigService _configService;
        private readonly IAggregatedMovieService _movieService;
        private OcelotGateway _ocelotSettings => _configService.GetSection<OcelotGateway>(nameof(OcelotGateway));

        public MovieManager(IConfigService configService, IAggregatedMovieService movieService)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
            _configService = configService ?? throw new ArgumentNullException(nameof(configService));
        }

        public async Task<Dictionary<string, MoviesCollection>> GetAggregatedMovies()
        {
            return await _movieService.GetAggregatedMoviesFromAllWorlds(_ocelotSettings.MoviesEndpoint);
        }
        public async Task<Dictionary<string, MovieDetail>> GetAggregatedMovieDetail(string universalId)
        {
            return await _movieService.GetAggregatedMovieDetailFromAllWorlds(_ocelotSettings.MovieDetailsEndPoint, universalId);
        }
        public async Task<MoviesCollection> GetCheapestMovies()
        {
            var allMovies = await GetAggregatedMovies();

            //Create union of all IDs and groupby its UniversalID accross different movie databases (worlds)
            var groupedMovie = allMovies.Values.ToList()
                                    .SelectMany(x => x.Movies).GroupBy(o => o.UniversalID);


            ConcurrentBag<Movie> currentBag = new ConcurrentBag<Movie>();

            Parallel.ForEach(groupedMovie, movie =>
            {
                Dictionary<string, MovieDetail> movieDetailFromAll;
                movieDetailFromAll = GetAggregatedMovieDetail(movie.Key).Result;

                var cheapestMovie = CompareAndFindCheapest(movieDetailFromAll);
                if (cheapestMovie != null) currentBag.Add(cheapestMovie.Result);
            });

            //var currentBag = new List<Movie>();
            //foreach (var movie in groupedMovie)
            //{                
            //    var cheapestMovie = await GetCheapestMovie(movie.Key);
            //    if (cheapestMovie != null) currentBag.Add(cheapestMovie);
            //}


            var cheapestMovieList = new MoviesCollection() { Movies = currentBag.ToList() };
            return cheapestMovieList;
        }
        public async Task<Movie> GetCheapestMovie(string universalId)
        {
            var movieDetailFromAll = await GetAggregatedMovieDetail(universalId);

            return await CompareAndFindCheapest(movieDetailFromAll);
        }
        private async Task<Movie> CompareAndFindCheapest(Dictionary<string, MovieDetail> movieDetailFromAll)
        {
            if (movieDetailFromAll == null) { return null; }

            MovieDetail movie = new MovieDetail { Price = decimal.MaxValue };
            foreach (var item in movieDetailFromAll)
            {
                if (item.Value != null) movie = movie.Price < item.Value.Price ? movie : item.Value;
            }

            return movie;
        }
    }
}
