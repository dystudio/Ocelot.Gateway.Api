using CheapestMovies.Api.Models;
using CheapestMovies.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheapestMovies.Api.Managers
{
    public interface IMovieManager
    {
        Task<MoviesList> GetCheapestMovies();
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

        public async Task<MoviesList> GetCheapestMovies()
        {
            var allMovies = await _movieService.GetAggregatedMovies(_ocelotSettings.MoviesEndpoint);


            //Create union of all IDs and groupby its UniversalID accross different movie databases
            var groupedMovie = allMovies.Values.ToList()
                                    .SelectMany(x => x.Movies).GroupBy(o => o.UniversalID);

            //Init the output as empty
            MoviesList cheapestMovieList = new MoviesList() { Movies = new List<Movie>() };

            foreach (var movie in groupedMovie)
            {
                //Compare prices only when other movie databases have it
                if (movie.Count() > 1)
                {
                    var cheapestMovie = await CompareAndFindCheapest(movie.Key);
                    cheapestMovieList.Movies.Add(cheapestMovie);
                }
            }

            return cheapestMovieList;
        }
        private async Task<Movie> CompareAndFindCheapest(string universalId)
        {
            var movieDetailFromAll = await _movieService.GetAggregatedMovieDetails(_ocelotSettings.MovieDetailsEndPoint, universalId);

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
