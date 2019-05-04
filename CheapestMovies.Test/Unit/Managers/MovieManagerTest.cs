using System;
using CheapestMovies.Api.Managers;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using CheapestMovies.Api.Services;
using System.Threading.Tasks;
using CheapestMovies.Api.Models;
using CheapestMovies.Test.InputData;
using System.Linq;

namespace CheapestMovies.Test.Unit.Managers
{

    public class MovieManagerTest : IDisposable
    {
        Mock<IConfigService> mockConfigService;
        Mock<IAggregatedMovieService> mockMovieService;

        public MovieManagerTest()
        {
            mockConfigService = new Mock<IConfigService>();
            mockMovieService = new Mock<IAggregatedMovieService>();
        }
        public void Dispose()
        {
            mockConfigService = null;
            mockMovieService = null;
        }
        public static IEnumerable<object[]> GetInputParams()
        {
            // THE TEST DATA STRUCTURE
            // Dictionary<string, MoviesCollection> allMovies, int uniqueMovieCount
            // GetMockMoviesWithDuplicates(int worldCount, int movieCount, int duplicateCount)

            //Correct unique movies counts
            yield return new object[] { Helper.GetMockMoviesWithDuplicates(2, 5, 2), 8 };
            yield return new object[] { Helper.GetMockMoviesWithDuplicates(1, 2, 0), 2 };
            yield return new object[] { Helper.GetMockMoviesWithDuplicates(1, 0, 0), 0 };
            yield return new object[] { Helper.GetMockMoviesWithDuplicates(0, 0, 0), 0 };
            yield return new object[] { Helper.GetMockMoviesWithDuplicates(3, 2, 1), 4 };
        }
        public static IEnumerable<object[]> GetInputParamsWithPrice()
        {
            // THE TEST DATA STRUCTURE
            // Dictionary<string, MoviesCollection> allMovies, decimal cheapestPrice
            // GetMockMoviesWithPrice(decimal cheapestPrice, decimal higherPrice)

            //Correct cheapest price
            yield return new object[] { Helper.GetMockMoviesWithPrice(2, 5), 2 };
            yield return new object[] { Helper.GetMockMoviesWithPrice(5, 3), 3 };
            yield return new object[] { Helper.GetMockMoviesWithPrice(0, 5), 0 };
            yield return new object[] { Helper.GetMockMoviesWithPrice(0, 0), 0 };
        }

        [Theory]
        [MemberData(nameof(GetInputParams))]
        public void GetAggregatedMovies_returns_unique_movies(Dictionary<string, MoviesCollection> allMovies, int uniqueMovieCount)
        {
            //Given
            mockConfigService.Setup(m => m.GetSection<OcelotGatewayApi>(It.IsAny<string>())).Returns(new OcelotGatewayApi { MoviesUrl = "api/movies" });
            mockMovieService.Setup(m => m.GetAggregatedMoviesFromAllWorlds(It.IsAny<string>())).Returns(Task.FromResult(allMovies));
            var sut = new MovieManager(mockConfigService.Object, mockMovieService.Object);

            //When
            var actual = sut.GetAggregatedMovies();

            //Then
            Assert.IsAssignableFrom<IEnumerable<Movie>>(actual.Result);
            Assert.Equal(uniqueMovieCount, actual.Result.Count());
        }

        [Theory]
        [MemberData(nameof(GetInputParamsWithPrice))]
        public void GetCheapestMovie_returns_cheapest_movie(Dictionary<string, MovieDetail> allMovies, decimal cheapestPrice)
        {
            //Given
            mockConfigService.Setup(m => m.GetSection<OcelotGatewayApi>(It.IsAny<string>())).Returns(new OcelotGatewayApi { MoviesUrl = "api/movies" });
            mockMovieService.Setup(m => m.GetAggregatedMovieDetailFromAllWorlds(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(allMovies));
            var sut = new MovieManager(mockConfigService.Object, mockMovieService.Object);

            //When
            var actual = sut.GetCheapestMovie(It.IsAny<string>());
            Assert.IsType<MovieDetail>(actual.Result);
            Assert.Equal(cheapestPrice, actual.Result.Price);
        }
    }
}
