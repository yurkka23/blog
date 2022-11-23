namespace Blog.Application.Caching;

public interface ICacheService
{
    public Task<T> GetAsync<T>(string key);

    public Task CreateAsync<T>(string key, T data);

    public Task DeleteAsync(string key);
      
}
