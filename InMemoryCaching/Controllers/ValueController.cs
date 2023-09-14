using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCaching.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValueController : ControllerBase
{
    readonly IMemoryCache _memoryCache;

    public ValueController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    [HttpGet("setName/{name}")]
    public void SetName(string name)
    {
        _memoryCache.Set("name", name);
    }

    [HttpGet("getName")]
    public string GetName()
    {
        if (_memoryCache.TryGetValue<string>("name", out string name))
        {
            return name;
        }
        return "name is empty";
    }


    [HttpGet("setDate")]
    public void SetDate()
    {
        _memoryCache.Set<DateTime>("date", DateTime.Now, options: new()
        {
            AbsoluteExpiration = DateTime.Now.AddSeconds(30),
            SlidingExpiration = TimeSpan.FromSeconds(5)
        });
    }

    [HttpGet("getDate")]
    public DateTime GetDate()
    {
        return _memoryCache.Get<DateTime>("date");
    }

}
