using Ocelot.Middleware;
using Ocelot.Middleware.Multiplexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TicketGateway.Api
{
    public class MovieAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<DownstreamResponse> movieLists)
        {
            //foreach (var movieList in movieLists)
            //{
            //    var movies = Task.FromResult<DownstreamResponse>(movieList);
            //}
            var cinema = Task.FromResult<DownstreamResponse>(movieLists[0]).Result;
            var film = Task.FromResult<DownstreamResponse>(movieLists[1]).Result;
            //var str = await cinema.Content.ReadAsStringAsync();
            //var movies = JsonConvert.DeserializeObject<AllMovies>(str);

            return await Task.FromResult<DownstreamResponse>(cinema);
        }
    }
    public class AllMovies
    {
        public List<Movie> Movies { get; set; }
    }
    public class Movie
    {
        public string Title { get; set; }
    }
}