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
                var response = await _movieManager.GetAggregatedMoviesFromAllWorlds();

                if (response != null) return Ok(response);
            }
            catch (Exception ex)
            {
                // Yell    Log    Catch  Throw
            }
            return NotFound();
        }

        [HttpGet("Cheapest")]
        public async Task<ActionResult> GetCheapestMovies()
        {
            try
            {
                var response = await _movieManager.GetCheapestMoviesFromAllWorlds();

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
                var response = await _movieManager.GetAggregatedMovieDetailFromAllWorlds(universalId);
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
