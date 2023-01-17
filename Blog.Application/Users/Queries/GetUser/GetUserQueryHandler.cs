using MediatR;
using MongoDB.Driver;
using Blog.Domain.Models;
using Microsoft.Extensions.Options;
using Blog.Application.Settings;
using Blog.Application.Users.Queries.GetUser;

namespace Blog.Application.Common.Exceptions;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
{
    private readonly IMongoCollection<User> _userCollection;
    public GetUserQueryHandler(
        IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);
    }

    public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userQuery = (await _userCollection.FindAsync(x => x.UserName == request.UserName, null, cancellationToken)).FirstOrDefault();

        if (userQuery == null)
        {
            throw new NotFoundException(nameof(User), request.UserName);
        }

        return userQuery;
    }
}
