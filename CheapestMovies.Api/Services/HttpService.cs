﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CheapestMovies.Api.Services
{
    public interface IHttpService
    {
        Task<TResponse> GetHttpResponse<TResponse>(string url) where TResponse : class;
    }
    public class HttpService : IHttpService
    {
        private static HttpClient _client = new HttpClient();

        public async Task<TResponse> GetHttpResponse<TResponse>(string url) where TResponse : class
        {
            //Always good to validate the input parameter in public methods
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));

            try
            {
                var res = await _client.GetAsync(url)
                                              .ContinueWith(async x =>
                                              {
                                                  var result = x.Result;

                                                  result.EnsureSuccessStatusCode();

                                                  var response = await result.Content.ReadAsStringAsync();

                                                  response = response.Replace(":}", ":\"\"}").Replace(":,", ":\"\",");

                                                  return JsonConvert.DeserializeObject<TResponse>(response);
                                              });
                return res.Result;
            }
            catch (Exception ex)
            {
                // Yell    Log    Catch  Throw     
            }
            return null;
        }
    }
}