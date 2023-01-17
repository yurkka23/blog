using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Users.Queries.GetUsersByRole;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Blog.Domain.Models;

namespace Blog.Application.UserSubscriptions.Queries.GetUserSubscriptions;

public class GetUserSubscriptionQueryHandler : IRequestHandler<GetUserSubscriptionQuery, UserList>
{
    private readonly IMongoCollection<UserSubscription> _subscriptionCollection;
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMapper _mapper;

    public GetUserSubscriptionQueryHandler(IMapper mapper, IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
    {
        _mapper = mapper;

        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _subscriptionCollection = mongoDatabase.GetCollection<UserSubscription>(
           entitiesStoreDatabaseSettings.Value.CollectionName);

        var mongoClientUser = new MongoClient(
          userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabaseUser = mongoClientUser.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabaseUser.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<UserList> Handle(GetUserSubscriptionQuery request, CancellationToken cancellationToken)
    {
        var userToSubscribeIdQuery = (await _subscriptionCollection
            .FindAsync(Builders<UserSubscription>.Filter.Eq("_t", "UserSubscription") & Builders<UserSubscription>.Filter.Eq("UserSubscribedToId", request.UserId),null, cancellationToken))
            .ToEnumerable()
            .Select(u => u.UserId)
            .ToList();

        var usersQuery = (await _userCollection
            .FindAsync(u => userToSubscribeIdQuery.Contains(u.Id), null, cancellationToken))
            .ToEnumerable()
            .Select(u => new UserLookUpDto
            {
                UserName = u.UserName,
                ImageUserUrl = u.ImageUserUrl,
                AboutMe = u.AboutMe,
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role
            })
            .ToList();

        return new UserList { Users = usersQuery };
    }
}