using CheapestMovies.Api.Models;
using CheapestMovies.Api.Services;
using CheapestMovies.Test.InputData;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CheapestMovies.Test.Unit.Services
{
    public class AggregatedMovieServiceTest : IDisposable
    {
        Mock<IHttpService> mockHttpService;
        public AggregatedMovieServiceTest()
        {
            mockHttpService = new Mock<IHttpService>();
        }
        public void Dispose()
        {
            mockHttpService = null;
        }
        public static IEnumerable<object[]> GetInputParams()
        {
            //THE TEST DATA STRUCTURE
            //Dictionary<string, MoviesCollection> allMovies, int worldCount, int movieCount

            //Correct counts
            yield return new object[] { Helper.GetMockMovies(2, 5), 2, 10 };
            yield return new object[] { Helper.GetMockMovies(1, 2), 1, 2 };
            yield return new object[] { Helper.GetMockMovies(1, 0), 1, 0 };
            yield return new object[] { Helper.GetMockMovies(0, 0), 0, 0 };
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


        [Theory(DisplayName = "GetAggregatedMoviesFromAllWorlds Returns Correct Count")]
        [MemberData(nameof(GetInputParams))]
        public async Task GetAggregatedMoviesFromAllWorlds_returns_correct_count(Dictionary<string, MoviesCollection> allMovies, int worldCount, int movieCount)
        {
            //Given
            string url = "url";
            mockHttpService.Setup(m => m.GetHttpResponse<Dictionary<string, MoviesCollection>>(It.IsAny<string>())).ReturnsAsync(allMovies);
            var sut = new AggregatedMovieService(mockHttpService.Object);

            //When
            var actual = await sut.GetAggregatedMoviesFromAllWorlds(url);
            var moviesToList = actual.Values.ToList();
            var movies = moviesToList.SelectMany(x => x.Movies);

            //Then
            Assert.IsType<Dictionary<string, MoviesCollection>>(actual);
            Assert.Equal(worldCount, actual.Count);
            Assert.Equal(movieCount, movies.Count());
        }

        [Theory(DisplayName = "GetAggregatedMovieDetailFromAllWorlds Returns Correct Count")]
        [MemberData(nameof(GetInputParamsForMovieDetail))]
        public async Task GetAggregatedMovieDetailFromAllWorlds_returns_correct_count(Dictionary<string, MovieDetail> movieDetailFromAll, int worldCount)
        {
            //Given
            string url = "url"; string universalId = "123";
            mockHttpService.Setup(m => m.GetHttpResponse<Dictionary<string, MovieDetail>>(It.IsAny<string>())).ReturnsAsync(movieDetailFromAll);
            var sut = new AggregatedMovieService(mockHttpService.Object);

            //When
            var actual = await sut.GetAggregatedMovieDetailFromAllWorlds(url, universalId);            

            //Then
            Assert.IsType<Dictionary<string, MovieDetail>>(actual);
            Assert.Equal(worldCount, actual.Count);
        }

        [Fact(DisplayName = "GetAggregatedMoviesFromAllWorlds Handles Exception")]
        public async Task GetAggregatedMoviesFromAllWorlds_handles_exception()
        {
            //Given
            string url = "url";
            mockHttpService.Setup(m => m.GetHttpResponse<Dictionary<string, MoviesCollection>>(It.IsAny<string>())).Throws<Exception>();
            var sut = new AggregatedMovieService(mockHttpService.Object);

            //When
            var actual = await sut.GetAggregatedMoviesFromAllWorlds(url);           

            //Then
            Assert.Null(actual);
        }

        [Fact(DisplayName = "GetAggregatedMovieDetailFromAllWorlds Handles Exception")]
        public async Task GetAggregatedMovieDetailFromAllWorlds_handles_exception()
        {
            //Given
            string url = "url"; string universalId = "123";
            mockHttpService.Setup(m => m.GetHttpResponse<Dictionary<string, MovieDetail>>(It.IsAny<string>())).Throws<Exception>();
            var sut = new AggregatedMovieService(mockHttpService.Object);

            //When
            var actual = await sut.GetAggregatedMovieDetailFromAllWorlds(url, universalId);

            //Then
            Assert.Null(actual);
        }
    }
}
