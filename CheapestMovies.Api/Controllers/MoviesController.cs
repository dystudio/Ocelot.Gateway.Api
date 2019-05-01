using CheapestMovies.Api.Managers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        public async Task<ActionResult> GetAggregatedMoviesFromAllWorlds()
        {
            try
            {
                var response = await _movieManager.GetAggregatedMovies();

                if (response != null) return Ok(response);
            }
            catch (Exception ex)
            {
                // Yell    Log    Catch  Throw
            }
            return NotFound();
        }

        [HttpGet("{universalId}")]
        public async Task<ActionResult> GetAggregatedMovieDetailFromAllWorlds(string universalId)
        {
            //Always good to validate the input parameter in public methods
            if (string.IsNullOrEmpty(universalId)) throw new ArgumentNullException(nameof(universalId));

            try
            {
                var response = await _movieManager.GetAggregatedMovieDetail(universalId);
                if (response != null) return Ok(response);
            }
            catch (Exception ex)
            {
                // Yell    Log    Catch  Throw
            }
            return NotFound();
        }

        [HttpGet("Cheapest/{universalId}")]
        public async Task<ActionResult> GetCheapestMovie(string universalId)
        {
            try
            {
                var response = await _movieManager.GetCheapestMovie(universalId);

                if (response != null) return Ok(response);
            }
            catch (Exception ex)
            {
                // Yell    Log    Catch  Throw
            }
            return NotFound();
        }
    }

}
