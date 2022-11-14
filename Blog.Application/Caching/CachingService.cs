using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Text.Json;

namespace Blog.Application.Caching;

public class CachingService : ICacheService
{
    private readonly IMongoCollection<CacheModel> _cacheCollection;

    public CachingService(
        IOptions<CacheStoreDatabaseSettings> cacheStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            cacheStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            cacheStoreDatabaseSettings.Value.DatabaseName);

        _cacheCollection = mongoDatabase.GetCollection<CacheModel>(
            cacheStoreDatabaseSettings.Value.CollectionName);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var result = await _cacheCollection.Find(x => x.Key == key).FirstOrDefaultAsync();
        if(result == default)
        {
            return default;
        }
        var decompressed = result.Value.DecompressGZip();
        return JsonSerializer.Deserialize<T>(decompressed);
    }
    
    public async Task CreateAsync<T>(string key, T data)
    {
        var jsonString = JsonSerializer.Serialize(data);
        var model = new CacheModel
        {
            Key = key,  
            Value = jsonString.CompressGZip()
        };
        var check = await GetAsync<T>(key);
        if (check == null)
        {
            await _cacheCollection.InsertOneAsync(model);
        }
       
    }

    public async Task DeleteAsync(string key)
    {
        await _cacheCollection.DeleteManyAsync(x => x.Key.Contains(key));
    }
    //public async Task DeleteAsync(Expression<Func<object, string>> predicate = null)
    //{
    //    await _cacheCollection.DeleteManyAsync(predicate);
    //}

}
