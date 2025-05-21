using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Infrastructure.Redis
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IDatabase _database;

        public RedisRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public async Task SetAsync(string key, string value) =>
            await _database.StringSetAsync(key, value);

        public async Task<string?> GetAsync(string key) =>
            await _database.StringGetAsync(key);
    }
}
