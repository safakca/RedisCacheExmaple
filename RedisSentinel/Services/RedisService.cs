using StackExchange.Redis;

namespace RedisSentinel.Services;
public sealed class RedisService
{
    static ConfigurationOptions sentinelOptions => new()
    {
        EndPoints =
        {
            { "localhost", 6380 },
            { "localhost", 6381 },
            { "localhost", 6382 },
        },
        CommandMap = CommandMap.Sentinel,
        AbortOnConnectFail = false
    };

    static ConfigurationOptions masterOptions => new()
    {
        AbortOnConnectFail = false
    };

    public static async Task<IDatabase> RedisMasterDatabase()
    {
        ConnectionMultiplexer sentinelConnection = await ConnectionMultiplexer.SentinelConnectAsync(sentinelOptions);

        System.Net.EndPoint masterEndpoint = null;
        foreach (System.Net.EndPoint endPoint in sentinelConnection.GetEndPoints())
        {
            IServer server = sentinelConnection.GetServer(endPoint);
            if (!server.IsConnected)
                continue;
            masterEndpoint = await server.SentinelGetMasterAddressByNameAsync("mymaster");
            break;
        }

        string localMasterIP = masterEndpoint.ToString() switch
        {
            "172.18.0.2:6379" => "6379",
            "172.18.0.3:6380" => "6380",
            "172.18.0.4:6381" => "6381",
            "172.18.0.5:6382" => "6382"
        };

        ConnectionMultiplexer masterConnection = await ConnectionMultiplexer.ConnectAsync(localMasterIP);
        IDatabase database = masterConnection.GetDatabase();
        return database;
    }
}
