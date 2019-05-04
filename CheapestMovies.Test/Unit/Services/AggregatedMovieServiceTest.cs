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


        [Theory]
        [MemberData(nameof(GetInputParams))]
        public void GetAggregatedMoviesFormAllWorlds_returns_correct_count(Dictionary<string, MoviesCollection> allMovies, int worldCount, int movieCount)
        {
            //Given
            string url = "url";
            mockHttpService.Setup(m => m.GetHttpResponse<Dictionary<string, MoviesCollection>>(It.IsAny<string>())).Returns(Task.FromResult(allMovies));
            var sut = new AggregatedMovieService(mockHttpService.Object);

            //When
            var actual = sut.GetAggregatedMoviesFromAllWorlds(url);
            var moviesToList = actual.Result.Values.ToList();
            var movies = moviesToList.SelectMany(x => x.Movies);

            //Then
            Assert.IsType<Dictionary<string, MoviesCollection>>(actual.Result);
            Assert.Equal(worldCount, actual.Result.Count);
            Assert.Equal(movieCount, movies.Count());
        }
    }
}
