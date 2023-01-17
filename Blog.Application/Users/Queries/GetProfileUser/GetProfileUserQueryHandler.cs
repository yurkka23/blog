using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using AutoMapper;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.Application.Users.Queries.GetProfileUser;

public class GetProfileUserQueryHandler : IRequestHandler<GetProfileUserQuery, ProfileUser>
{
    private readonly IMongoCollection<Article> _articleCollection;
    private readonly IMongoCollection<UserSubscription> _subscriptionCollection;
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMapper _mapper;

    public GetProfileUserQueryHandler(IMapper mapper, IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
    {
        _mapper = mapper;

        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _articleCollection = mongoDatabase.GetCollection<Article>(
            entitiesStoreDatabaseSettings.Value.CollectionName);

        _subscriptionCollection = mongoDatabase.GetCollection<UserSubscription>(
           entitiesStoreDatabaseSettings.Value.CollectionName);

        var mongoClientUser = new MongoClient(
          userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabaseUser = mongoClientUser.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabaseUser.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<ProfileUser> Handle(GetProfileUserQuery request, CancellationToken cancellationToken)
    {
        var userQuery = (await _userCollection
            .FindAsync(Builders<User>.Filter.Eq("_id", request.Id), null, cancellationToken))
            .FirstOrDefault();

        if (userQuery == null)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        var result = _mapper.Map<ProfileUser>(userQuery);

        result.CountArticles = (await _articleCollection.FindAsync(Builders<Article>.Filter.Eq("_t", "Article") & Builders<Article>.Filter.Eq("CreatedBy", request.Id),null,cancellationToken)).ToEnumerable().Count();

        result.Followers = (await _subscriptionCollection
            .FindAsync(Builders<UserSubscription>.Filter.Eq("_t", "UserSubscription") & Builders<UserSubscription>.Filter.Eq("UserSubscribedToId", request.Id),null,cancellationToken))
            .ToEnumerable()
            .Count();

        result.Following = (await _subscriptionCollection
            .FindAsync(Builders<UserSubscription>.Filter.Eq("_t", "UserSubscription") & Builders<UserSubscription>.Filter.Eq("UserId", request.Id),null,cancellationToken))
            .ToEnumerable()
            .Count();

        result.IsCurrentUserSubscribed = (await  _subscriptionCollection
            .FindAsync(Builders<UserSubscription>.Filter.Eq("_t", "UserSubscription") 
               & Builders<UserSubscription>.Filter.Eq("UserId", request.CurrentUserId)
               & Builders<UserSubscription>.Filter.Eq("UserSubscribedToId", request.Id), null,cancellationToken))
            .FirstOrDefault() == null ? false : true;

        return result;
    }
}
