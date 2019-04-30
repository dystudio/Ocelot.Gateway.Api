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
        Task<Dictionary<string, MoviesCollection>> GetAggregatedMoviesFromAllWorlds();
        Task<Dictionary<string, MovieDetail>> GetAggregatedMovieDetailFromAllWorlds(string universalId);
        Task<MoviesCollection> GetCheapestMoviesFromAllWorlds();
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

        public async Task<Dictionary<string, MoviesCollection>> GetAggregatedMoviesFromAllWorlds()
        {
            return await _movieService.GetAggregatedMoviesFromAllWorlds(_ocelotSettings.MoviesEndpoint);
        }

        public async Task<Dictionary<string, MovieDetail>> GetAggregatedMovieDetailFromAllWorlds(string universalId)
        {
            return await _movieService.GetAggregatedMovieDetailFromAllWorlds(_ocelotSettings.MovieDetailsEndPoint, universalId);
        }
        public async Task<MoviesCollection> GetCheapestMoviesFromAllWorlds()
        {
            var allMovies = await GetAggregatedMoviesFromAllWorlds();

            //Create union of all IDs and groupby its UniversalID accross different movie databases (worlds)
            var groupedMovie = allMovies.Values.ToList()
                                    .SelectMany(x => x.Movies).GroupBy(o => o.UniversalID);

            ConcurrentBag<Movie> currentBag = new ConcurrentBag<Movie>();


            //Parallel.ForEach(groupedMovie, movie =>
            //{
            //    //Compare prices only when other movie databases have it
            //    if (movie.Count() > 1)
            //    {
            //        var cheapestMovie = CompareAndFindCheapest(movie.Key).Result;
            //        if (cheapestMovie != null) currentBag.Add(cheapestMovie);
            //    }
            //});

            foreach (var movie in groupedMovie)
            {
                //Compare prices only when other movie databases have it
                if (movie.Count() > 1)
                {
                    var cheapestMovie = await CompareAndFindCheapest(movie.Key);
                    if (cheapestMovie != null) currentBag.Add(cheapestMovie);
                }
            }


            var cheapestMovieList = new MoviesCollection() { Movies = currentBag.ToList() };
            return cheapestMovieList;
        }
        private async Task<Movie> CompareAndFindCheapest(string universalId)
        {
            Dictionary<string, MovieDetail> movieDetailFromAll;
            try
            {
                movieDetailFromAll = await GetAggregatedMovieDetailFromAllWorlds(universalId);
                if (movieDetailFromAll == null || movieDetailFromAll.Count == 0) { return null; }
            }
            catch (Exception ex)
            {

                throw;
            }

            //Init first movie
            MovieDetail movie = new MovieDetail { Price = decimal.MaxValue };
            foreach (var item in movieDetailFromAll)
            {
                movie = movie.Price < item.Value.Price ? movie : item.Value;
            }

            return movie;
        }
    }
}
