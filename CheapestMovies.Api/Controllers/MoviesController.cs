using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace CheapestMovies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        static HttpClient client = new HttpClient();
        // GET api/values
        [HttpGet]
        public ActionResult GetCheapestMovies()
        {

            string url = "http://localhost:2000/api/movies";
            //string url = "http://localhost:2000/api/filmworld/movie/fw0076759";
            //string url = "http://localhost:2000/api/filmworld/movies";

            var response = client.GetAsync(url)
                     .ContinueWith(async x =>
                     {
                         var result = x.Result;

                         result.EnsureSuccessStatusCode();

                         var res = await result.Content.ReadAsStringAsync();

                         var movies = JsonConvert.DeserializeObject<Dictionary<string, MoviesCollection>>(res);
                         return movies;
                     }).Result;

            return Ok(response.Result);
        }
        [HttpGet("{id}")]
        public ActionResult GetMovieDetailById(string id)
        {

            string url = $"http://localhost:2000/api/movie/{id}";

            var response = client.GetAsync(url)
                     .ContinueWith(async x =>
                     {
                         var result = x.Result;

                         result.EnsureSuccessStatusCode();

                         var res = await result.Content.ReadAsStringAsync();

                         var movies = JsonConvert.DeserializeObject<Dictionary<string, Movie>>(res);
                         return movies;
                     }).Result;

            return Ok(response.Result);
        }



    }
    public class Movie
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string ID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }

    public class MoviesCollection
    {
        public IEnumerable<Movie> Movies { get; set; }
    }
}
