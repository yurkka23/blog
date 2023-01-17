using MediatR;
using MongoDB.Driver;
using Blog.Domain.Models;
using Microsoft.Extensions.Options;
using Blog.Application.Settings;

namespace Blog.Application.Users.Queries.CheckUser;

public class CheckUserQueryHandler : IRequestHandler<CheckUserQuery, bool>
{
    private readonly IMongoCollection<User> _userCollection;
    public CheckUserQueryHandler(
        IOptions<MongoUserDBSettings> cacheStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            cacheStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            cacheStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>(
            cacheStoreDatabaseSettings.Value.CollectionName);
    }
  
    public async Task<bool> Handle(CheckUserQuery request, CancellationToken cancellationToken)
    {
        var userQuery = (await _userCollection
            .FindAsync(Builders<User>.Filter.Eq("UserName", request.UserName)))
            .FirstOrDefault();

        if (userQuery == null)
        {
            return false;
        }

        return true;
    }
}
