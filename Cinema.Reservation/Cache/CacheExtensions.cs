using System.Text.Json;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace Cinema.Reservation.Cache;

internal static class CacheExtensions
{
    public static IServiceCollection AddCache(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddFusionCache()
            .WithDefaultEntryOptions(new FusionCacheEntryOptions
            {
                Duration = TimeSpan.FromMinutes(1),

                IsFailSafeEnabled = true,
                FailSafeMaxDuration = TimeSpan.FromHours(2),
                FailSafeThrottleDuration = TimeSpan.FromSeconds(30),

                // FactorySoftTimeout = TimeSpan.FromMilliseconds(100),
                // FactoryHardTimeout = TimeSpan.FromMilliseconds(1500)
            })
            .WithSerializer(new FusionCacheSystemTextJsonSerializer(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }))
            // ADD REDIS DISTRIBUTED CACHE SUPPORT
            .WithDistributedCache(
                new RedisCache(new RedisCacheOptions
                {
                    Configuration = configuration.GetConnectionString("redis"),
                    InstanceName = "cinema_"
                })
            );

        return services;
    }
}