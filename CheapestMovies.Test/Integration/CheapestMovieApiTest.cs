//using CheapestMovies.Api.Controllers;
//using CheapestMovies.Api.Managers;
//using CheapestMovies.Api.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace CheapestMovies.Test.Integration
//{
//    public class CheapestMovieApiTest : IDisposable
//    {
//        IMovieManager _movieManager = null;
//        IConfigService _configService = null;
//        IAggregatedMovieService _movieService = null;
//        IHttpService _httpService = null;
//        IHttpClientFactory _httpClientFactory = null;
//        public CheapestMovieApiTest()
//        {            
//            _configService = new ConfigService(new ConfigurationBuilder()
//                         .SetBasePath(Directory.GetCurrentDirectory())
//                         .AddJsonFile("appsettings.json")
//                         .Build());

          

//            _httpService = new HttpService(_httpClientFactory);
//            _movieService = new AggregatedMovieService(_httpService);
//            _movieManager = new MovieManager(_configService, _movieService);
//        }
//        public void Dispose()
//        {
//            _movieManager = null;
//            _configService = null;
//            _movieService = null;
//            _httpService = null;
//            _httpClientFactory = null;
//        }

//        [Fact(DisplayName = "Integration Test")]
//        public async Task Api_starts_and_produces_result()
//        {
//            //given
//            var sut = new MoviesController(_movieManager);

//            //When
//            var actual = await sut.GetAggregatedMoviesFromAllWorlds();

//            //Then
//            var result = Assert.IsType<NotFoundResult>(actual);
//            Assert.Equal(404, result.StatusCode);
//        }
//    }
//}
