using Microsoft.Extensions.Configuration;
using System;

namespace CheapestMovies.Api.Services
{
    public interface IConfigService
    {
        TResult GetSection<TResult>(string sectionName) where TResult : class;
    }
    class ConfigService : IConfigService
    {
        private readonly IConfiguration _configuration;

        public ConfigService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        public TResult GetSection<TResult>(string sectionName) where TResult : class
        {
            //Always good to validate the input parameter in public methods
            if (string.IsNullOrEmpty(sectionName)) return null;

            TResult section = null;

            try
            {
                section = Activator.CreateInstance(typeof(TResult)) as TResult;
                _configuration.GetSection(sectionName).Bind(section);
            }
            catch (Exception)
            {
                // Yell    Log    Catch  Throw     
            }

            return section;
        }
    }
}
