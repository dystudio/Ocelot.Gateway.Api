using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using CheapestMovies.Api.Services;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using CheapestMovies.Api.Models;
using CheapestMovies.Test.InputData;
using System.Linq;

namespace CheapestMovies.Test.Unit.Services
{
    public class HttpServiceTest : IDisposable
    {
        Mock<IHttpClientFactory> mockHttClientFactory;
        public HttpServiceTest()
        {
            mockHttClientFactory = new Mock<IHttpClientFactory>();
        }
        public void Dispose()
        {
            mockHttClientFactory = null;
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

        public static IEnumerable<object[]> GetRandomParams()
        {
            //Correct types
            yield return new object[] { new Movie { MovieWorld = "Marvel" } };
            yield return new object[] { new Movie { MovieWorld = "Cinema" } };
            yield return new object[] { new Movie { MovieWorld = "Film" } };
        }

        [Theory(DisplayName = "GetHttpResponse Returns Correct Type and Count")]
        [MemberData(nameof(GetInputParams))]
        public async Task GetHttpResponse_returns_desired_type_and_count(Dictionary<string, MoviesCollection> allMovies, int worldCount, int movieCount)
        {
            //Given                       
            string url = "https://abc.com";

            var clientHandlerStub = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(allMovies), Encoding.UTF8, "application/json")
            });

            var httpClient = new HttpClient(clientHandlerStub);
            mockHttClientFactory.Setup(m => m.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var sut = new HttpService(mockHttClientFactory.Object);

            //When
            var actual = await sut.GetHttpResponse<Dictionary<string, MoviesCollection>>(url);
            var moviesToList = actual.Values.ToList();
            var movies = moviesToList.SelectMany(x => x.Movies);

            //Then
            Assert.IsType<Dictionary<string, MoviesCollection>>(actual);
            Assert.Equal(worldCount, actual.Count);
            Assert.Equal(movieCount, movies.Count());
        }

        [Theory(DisplayName = "GetHttpResponse Works W/ Any Data Type")]
        [MemberData(nameof(GetRandomParams))]
        public async Task GetHttpResponse_works_with_any_data_type(Movie movie)
        {
            //Given                       
            string url = "https://abc.com";

            var clientHandlerStub = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(movie), Encoding.UTF8, "application/json")
            });

            var httpClient = new HttpClient(clientHandlerStub);
            mockHttClientFactory.Setup(m => m.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var sut = new HttpService(mockHttClientFactory.Object);

            //When
            var actual = await sut.GetHttpResponse<Movie>(url);

            //Then            
            Assert.IsType<Movie>(movie);
        }
    }
    public class FakeHttpMessageHandler : DelegatingHandler
    {
        private HttpResponseMessage _fakeResponse;

        public FakeHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            _fakeResponse = responseMessage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_fakeResponse);
        }
    }
}
