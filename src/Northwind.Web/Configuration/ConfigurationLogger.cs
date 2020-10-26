using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Northwind.Web.Configuration
{
    public class ConfigurationLogger : IConfiguration
    {
        private readonly ILogger _logger;

        private readonly IConfiguration _configuration;

        public ConfigurationLogger(IConfiguration configuration, ILogger<ConfigurationLogger> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IConfigurationSection GetSection(string key)
        {
            var section = _configuration.GetSection(key);

            _logger.LogInformation($"Read section '{section.Path}' -> '{section.Value}'");

            return section;
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return _configuration.GetChildren();
        }

        public IChangeToken GetReloadToken()
        {
            return _configuration.GetReloadToken();
        }

        public string this[string key]
        {
            get
            {
                var value = _configuration[key];

                _logger.LogInformation($"Read config key '{key}' -> {value}");

                return value;
            }
            set => _configuration[key] = value;
        }
    }
}
