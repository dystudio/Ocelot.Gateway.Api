using CheapestMovies.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace CheapestMovies.Api.Extensions
{
    public static class MovieExtensions
    {
        public static IEnumerable<Movie> GetUniqueMovies(this Dictionary<string, MoviesCollection> moviesFromAllWorlds)
        {
            if (moviesFromAllWorlds == null || moviesFromAllWorlds.Count == 0) return Enumerable.Empty<Movie>();

            var allMoviesList = moviesFromAllWorlds.Values.ToList();
            allMoviesList.RemoveAll(world => world == null);
            var uniqueMovies = allMoviesList.SelectMany(x => x.Movies)
                                .GroupBy(o => o.UniversalID)
                                .Select(y => y.First());
            return uniqueMovies;
        }

        public static MovieDetail CompareAndFindCheapest(this Dictionary<string, MovieDetail> movieDetailFromAll)
        {
            if (movieDetailFromAll == null || movieDetailFromAll.Count == 0) { return null; }

            MovieDetail movie = new MovieDetail { Price = decimal.MaxValue };
            foreach (var item in movieDetailFromAll)
            {
                if (item.Value != null) { movie = movie.Price < item.Value.Price ? movie : item.Value; }
            }

            return movie;
        }
        public static string FormatJson(this string json)
        {
            //Precautionary measures as Ocelot messes up with JSON sometimes. 
            return json.Replace(":}", ":\"\"}").Replace(":,", ":\"\",");
        }
    }
}
