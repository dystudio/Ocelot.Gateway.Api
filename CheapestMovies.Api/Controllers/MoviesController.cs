using CheapestMovies.Api.Managers;
using CheapestMovies.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheapestMovies.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieManager _movieManager;

        public MoviesController(IMovieManager movieManager)
        {
            _movieManager = movieManager ?? throw new ArgumentNullException(nameof(movieManager));
        }

        /// <summary>
        /// Retrievs distinct movies from all the movie worlds with "UniversalID" assigned to each movie
        /// </summary>
        /// <returns>List of distinct movies</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Movie>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetAggregatedMoviesFromAllWorlds()
        {
            try
            {
                var response = await _movieManager.GetAggregatedMovies();

                if (response != null) return Ok(response);
            }
            catch (Exception)
            {
                // Yell    Log    Catch  Throw
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NotFound();
        }

        /// <summary>
        /// Retrievs single movie detail from all the movie worlds for comparison
        /// </summary>
        /// <param name="universalId">Univeral Id of the movie e.g. 0076759</param>
        /// <returns>movie detail from each movie world</returns>
        [ProducesResponseType(200, Type = typeof(Dictionary<string, MovieDetail>))]
        [ProducesResponseType(404)]
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
            catch (Exception)
            {
                // Yell    Log    Catch  Throw
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NotFound();
        }

        /// <summary>
        /// Retrievs cheapest price of a movie from all movie worlds
        /// </summary>
        /// <param name="universalId">Univeral Id of the movie e.g. 0076759</param>
        /// <returns>Cheapest movie detail</returns>
        [ProducesResponseType(200, Type = typeof(MovieDetail))]
        [ProducesResponseType(404)]
        [HttpGet("Cheapest/{universalId}")]
        public async Task<ActionResult> GetCheapestMovie(string universalId)
        {
            try
            {
                var response = await _movieManager.GetCheapestMovie(universalId);

                if (response != null) return Ok(response);
            }
            catch (Exception)
            {
                // Yell    Log    Catch  Throw
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NotFound();
        }
    }

}
