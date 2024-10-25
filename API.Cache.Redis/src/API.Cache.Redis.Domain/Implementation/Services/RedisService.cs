using API.Cache.Redis.Domain.Implementation.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace API.Cache.Redis.Domain.Implementation.Services
{
    public class RedisService : IRedisService
    {
        private readonly ILogger<RedisService> _logger;

        private ConfigurationOptions _configurationOptions;

        private string _connectionStringRedis { set; get; }

        private int _connectionStringRedisPort { set; get; }

        private string _connectionStringRedisPassword { set; get; }

        public RedisService(
                ILogger<RedisService> logger,
                IConfiguration configuration
            )
        {
            _logger = logger;
            _connectionStringRedis = configuration["RedisHost"]!;
            _connectionStringRedisPort = Convert.ToInt32(configuration["RedisPort"]!);
            _connectionStringRedisPassword = configuration["RedisPassword"]!;

            SetProperty();
        }

        private void SetProperty()
        {
            _configurationOptions = new ConfigurationOptions
            {
                EndPoints = {
                    { _connectionStringRedis, _connectionStringRedisPort }
                },
                Password = _connectionStringRedisPassword
            };
        }

        public string? Get(string key)
        {
            var result = String.Empty;

            try
            {
                using (var redis = ConnectionMultiplexer.Connect(_configurationOptions))
                {
                    var db = redis.GetDatabase();

                    result = db.StringGet(key)!;
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError($"Erro workflow api Get (RedisService): {Ex.Message}");
            }

            return result;
        }

        public bool Set(string key, string value)
        {
            bool result = false;

            try
            {
                using (var redis = ConnectionMultiplexer.Connect(_configurationOptions))
                {
                    var db = redis.GetDatabase();
                    db.StringSet(key, value);
                    string confirmVariable = db.StringGet(key)!;

                    if (confirmVariable != null)
                        result = true;
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError($"Erro workflow api Set (RedisService): {Ex.Message}");
            }

            return result;
        }

        public void Delete()
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(_configurationOptions))
                {
                    var endpoints = redis.GetEndPoints();
                    var server = redis.GetServer(endpoints[0]);
                    server.FlushAllDatabases();
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError($"Erro workflow api Delete (RedisService): {Ex.Message}");
            }
        }
    }
}
