using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

            var response = client.GetAsync(url)
                     .ContinueWith(async x =>
                     {
                         var result = x.Result;

                         result.EnsureSuccessStatusCode();

                         var res = await result.Content.ReadAsStringAsync();

                         return JsonConvert.DeserializeObject<object>(res);
                     }).Result;

            return Ok(response);         
        }
       
    }
}
