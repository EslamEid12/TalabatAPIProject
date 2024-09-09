using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class ResponseCasheService : IResponseCasheService
    {
        private readonly IDatabase _database;
        public ResponseCasheService(IConnectionMultiplexer redis) 
        {
            _database = redis.GetDatabase();
        }
        public async Task CasheResponseAsync(string CashKey, object response, TimeSpan timeToLive)
        {
            if (response is null) return;
            var SerializeOption=new JsonSerializerOptions() { PropertyNamingPolicy= JsonNamingPolicy.CamelCase };
            var SerializedResponse = JsonSerializer.Serialize(response,SerializeOption);
            await _database.StringSetAsync(CashKey, SerializedResponse, timeToLive);
        }

        public async Task<string?> GetCashedResponseAsync(string CashKey)
        {
            var CashedResponse = await _database.StringGetAsync(CashKey);

            if (CashedResponse.IsNullOrEmpty) return null;
            return CashedResponse;
        }
    }
}
