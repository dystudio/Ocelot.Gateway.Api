using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using CheapestMovies.Api.Managers;
using System.Threading.Tasks;
using CheapestMovies.Test.InputData;
using CheapestMovies.Api.Models;
using CheapestMovies.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CheapestMovies.Test.Unit.Controllers
{
    public class MovieControllerTest : IDisposable
    {
        Mock<IMovieManager> mockMovieManager;
        public MovieControllerTest()
        {
            mockMovieManager = new Mock<IMovieManager>();
        }
        public void Dispose()
        {
            mockMovieManager = null;
        }

        public static IEnumerable<object[]> GetInputParams()
        {
            // THE TEST DATA STRUCTURE
            // IEnumerable<Movie> allMovies, int movieCount

            //Correct movie count
            yield return new object[] { Helper.GetMockMoviesAggregated(2), 2 };
            yield return new object[] { Helper.GetMockMoviesAggregated(1), 1 };
            yield return new object[] { Helper.GetMockMoviesAggregated(0), 0 };
        }
        public static IEnumerable<object[]> GetInputParamsForMovieDetail()
        {
            //THE TEST DATA STRUCTURE
            //Dictionary<string, MovieDetail> movieDetailFromAll, int worldCount

            //Correct counts
            yield return new object[] { Helper.GetMockMovieDetail(1), 1 };
            yield return new object[] { Helper.GetMockMovieDetail(2), 2 };
            yield return new object[] { Helper.GetMockMovieDetail(0), 0 };
        }

        public static IEnumerable<object[]> GetCheapestMovie()
        {
            //THE TEST DATA STRUCTURE
            //MovieDetail movieDetail

            //Correct state
            yield return new object[] { new MovieDetail() };
        }

        [Theory(DisplayName = "CTRL: GetAggregatedMoviesFromAllWorlds Returns Correct Object State")]
        [MemberData(nameof(GetInputParams))]
        public async Task GetAggregatedMoviesFromAllWorlds_returns_correct_object_state(List<Movie> allMovies, int movieCount)
        {
            //Given
            mockMovieManager.Setup(m => m.GetAggregatedMovies()).ReturnsAsync(allMovies);
            var sut = new MoviesController(mockMovieManager.Object);

            //When
            var actual = await sut.GetAggregatedMoviesFromAllWorlds();

            //Then            
            var result = Assert.IsType<OkObjectResult>(actual);
            Assert.Equal(200, result.StatusCode);
            var items = Assert.IsType<List<Movie>>(result.Value);
            Assert.Equal(movieCount, items.Count);
        }

        [Theory(DisplayName = "CTRL: GetAggregatedMovieDetailFromAllWorlds Returns Correct Object State")]
        [MemberData(nameof(GetInputParamsForMovieDetail))]
        public async Task GetAggregatedMovieDetailFromAllWorlds_returns_correct_object_state(Dictionary<string, MovieDetail> movieDetail, int worldCount)
        {
            //Given
            string universalId = "123";
            mockMovieManager.Setup(m => m.GetAggregatedMovieDetail(It.IsAny<string>())).ReturnsAsync(movieDetail);
            var sut = new MoviesController(mockMovieManager.Object);

            //When
            var actual = await sut.GetAggregatedMovieDetailFromAllWorlds(universalId);

            //Then
            var result = Assert.IsType<OkObjectResult>(actual);
            Assert.Equal(200, result.StatusCode);
            var items = Assert.IsType<Dictionary<string, MovieDetail>>(result.Value);
            Assert.Equal(worldCount, items.Count);
        }

        [Theory(DisplayName = "CTRL: GetCheapestMovie Returns Correct Object State")]
        [MemberData(nameof(GetCheapestMovie))]
        public async Task GetCheapestMovie_returns_correct_object_state(MovieDetail movieDetail)
        {
            //Given
            string universalId = "123";
            mockMovieManager.Setup(m => m.GetCheapestMovie(It.IsAny<string>())).ReturnsAsync(movieDetail);
            var sut = new MoviesController(mockMovieManager.Object);

            //When
            var actual = await sut.GetCheapestMovie(universalId);

            //Then
            var result = Assert.IsType<OkObjectResult>(actual);
            Assert.Equal(200, result.StatusCode);
            var items = Assert.IsType<MovieDetail>(result.Value);
        }

        [Fact(DisplayName = "CTRL: GetAggregatedMoviesFromAllWorlds Returns 404")]
        public async Task GetAggregatedMoviesFromAllWorlds_returns_404()
        {
            //Given
            List<Movie> movies = null;
            mockMovieManager.Setup(m => m.GetAggregatedMovies()).ReturnsAsync(movies);
            var sut = new MoviesController(mockMovieManager.Object);

            //When
            var actual = await sut.GetAggregatedMoviesFromAllWorlds();

            //Then            
            var result = Assert.IsType<NotFoundResult>(actual);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact(DisplayName = "CTRL: GetAggregatedMovieDetailFromAllWorlds Returns 404")]
        public async Task GetAggregatedMovieDetailFromAllWorlds_returns_404()
        {
            //Given
            string universalId = "123";
            Dictionary<string, MovieDetail> movieDetail = null;
            mockMovieManager.Setup(m => m.GetAggregatedMovieDetail(It.IsAny<string>())).ReturnsAsync(movieDetail);
            var sut = new MoviesController(mockMovieManager.Object);

            //When
            var actual = await sut.GetAggregatedMovieDetailFromAllWorlds(universalId);

            //Then
            var result = Assert.IsType<NotFoundResult>(actual);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact(DisplayName = "CTRL: GetCheapestMovie Returns 404")]
        public async Task GetCheapestMovie_returns_404()
        {
            //Given
            string universalId = "123";
            MovieDetail movieDetail = null;
            mockMovieManager.Setup(m => m.GetCheapestMovie(It.IsAny<string>())).ReturnsAsync(movieDetail);
            var sut = new MoviesController(mockMovieManager.Object);

            //When
            var actual = await sut.GetCheapestMovie(universalId);

            //Then
            var result = Assert.IsType<NotFoundResult>(actual);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact(DisplayName = "CTRL: GetAggregatedMoviesFromAllWorlds Returns 500")]
        public async Task GetAggregatedMoviesFromAllWorlds_returns_500()
        {
            //Given            
            mockMovieManager.Setup(m => m.GetAggregatedMovies()).Throws<Exception>();
            var sut = new MoviesController(mockMovieManager.Object);

            //When
            var actual = await sut.GetAggregatedMoviesFromAllWorlds();

            //Then            
            var result = Assert.IsType<StatusCodeResult>(actual);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact(DisplayName = "CTRL: GetAggregatedMovieDetailFromAllWorlds Returns 500")]
        public async Task GetAggregatedMovieDetailFromAllWorlds_returns_500()
        {
            //Given
            string universalId = "123";
            mockMovieManager.Setup(m => m.GetAggregatedMovieDetail(It.IsAny<string>())).Throws<Exception>();
            var sut = new MoviesController(mockMovieManager.Object);

            //When
            var actual = await sut.GetAggregatedMovieDetailFromAllWorlds(universalId);

            //Then
            var result = Assert.IsType<StatusCodeResult>(actual);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact(DisplayName = "CTRL: GetCheapestMovie Returns 500")]
        public async Task GetCheapestMovie_returns_500()
        {
            //Given
            string universalId = "123";
            mockMovieManager.Setup(m => m.GetCheapestMovie(It.IsAny<string>())).Throws<Exception>();
            var sut = new MoviesController(mockMovieManager.Object);

            //When
            var actual = await sut.GetCheapestMovie(universalId);

            //Then
            var result = Assert.IsType<StatusCodeResult>(actual);
            Assert.Equal(500, result.StatusCode);
        }
    }
}
