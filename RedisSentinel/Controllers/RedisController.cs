using Microsoft.AspNetCore.Mvc;
using RedisSentinel.Services;
using StackExchange.Redis;

namespace RedisSentinel.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RedisController : ControllerBase
{
    //localhost:4200/api/redis/setvalue/name/safak
    [HttpGet("[action]/{key}/{value}")]
    public async Task<IActionResult> SetValue(string key, string value)
    {
        IDatabase redis = await RedisService.RedisMasterDatabase();
        await redis.StringSetAsync(key, value);
        return Ok();
    }

    //localhost:4200/api/redis/getvalue/name
    [HttpGet("[action]/{key}")]
    public async Task<IActionResult> GetValue(string key)
    {
        IDatabase redis = await RedisService.RedisMasterDatabase();
        RedisValue data = await redis.StringGetAsync(key);
        return Ok();
    }
}
