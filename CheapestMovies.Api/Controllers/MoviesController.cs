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
        private readonly IMoviesService _moviesService;
        private readonly IConfigService _configService;
        private UrlSettings _settings => _configService.GetSection<UrlSettings>(nameof(UrlSettings));

        public MoviesController(IMoviesService movieService, IConfigService configSerivce)
        {
            _moviesService = movieService ?? throw new ArgumentNullException(nameof(movieService));
            _configService = configSerivce ?? throw new ArgumentNullException(nameof(configSerivce));
        }

        [HttpGet]
        public ActionResult GetCheapestMovies()
        {
            try
            {
                var response = _moviesService.GetCheapestMovies(_settings.MoviesListUrl);
                return Ok(response.Result);
            }
            catch (Exception)
            {
                // Yell    Log    Catch  Throw     
            }
            return null;
        }
        [HttpGet("{id}")]
        public ActionResult GetCheapestMovieDetailById(string id)
        {
            //Always good to validate the input parameter in public methods
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            try
            {
                var response = _moviesService.GetCheapestMovieDetailById(_settings.MovieDetailsUrl, id);
                return Ok(response.Result);
            }
            catch (Exception)
            {
                // Yell    Log    Catch  Throw
            }
            return null;
        }
    }

}
