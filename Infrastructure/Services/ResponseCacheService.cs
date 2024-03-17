using System.Text.Json;
using Core.Interfaces;
using Microsoft.VisualBasic;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class ResponseCacheService : IResponseCacheService
{
    private readonly IDatabase _database;
    public ResponseCacheService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
    {
        if (response == null)
        {
            return;
        }

        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var serialisedResponse = JsonSerializer.Serialize(response, options);

        await _database.StringSetAsync(cacheKey, serialisedResponse);

    }

    public async Task<string> GetCachedResponseAsync(string cacheKey)
    {
        var cachedResponse = await _database.StringGetAsync(cacheKey);

        if (cachedResponse.IsNullOrEmpty)
        {
            return null;
        }

        return cachedResponse;
    }
}