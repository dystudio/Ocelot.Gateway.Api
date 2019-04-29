using CheapestMovies.Api.Managers;
using CheapestMovies.Api.Models;
using CheapestMovies.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CheapestMovies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieManager _movieManager;


        public MoviesController(IMovieManager movieManager)
        {
            _movieManager = movieManager ?? throw new ArgumentNullException(nameof(movieManager));
        }

        [HttpGet]
        public ActionResult GetCheapestMovies()
        {
            try
            {
                var response = _movieManager.GetCheapestMovies();

                if (response.Result != null) return Ok(response.Result);
            }
            catch (Exception ex)
            {

            }
            return NotFound();
        }

        //[HttpGet("{id}")]
        //public ActionResult GetCheapestMovieDetailById(string id)
        //{
        //    //Always good to validate the input parameter in public methods
        //    if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

        //    try
        //    {
        //        var response = _moviesService.GetCheapestMovieDetailById(_settings.MovieDetailsEndPoint, id);
        //        if (response.Result != null) return Ok(response.Result);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Yell    Log    Catch  Throw
        //    }
        //    return NotFound();
        //}
    }

}
