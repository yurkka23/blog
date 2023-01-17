
using Blog.Application.Common.Exceptions;
using Blog.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Blog.Domain.Models;

namespace Blog.Application.Users.Queries.GetStatistics;

public class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, StatisticsDTO>
{
    private readonly IMongoCollection<Article> _articleCollection;
    private readonly IMongoCollection<UserSubscription> _subscriptionCollection;
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<Rating> _ratingCollection;
    private readonly IMongoCollection<Comment> _commentCollection;


    public GetStatisticsQueryHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
    {

        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _articleCollection = mongoDatabase.GetCollection<Article>(
            entitiesStoreDatabaseSettings.Value.CollectionName);
        _commentCollection = mongoDatabase.GetCollection<Comment>(
            entitiesStoreDatabaseSettings.Value.CollectionName);
        _ratingCollection = mongoDatabase.GetCollection<Rating>(
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
    public async Task<StatisticsDTO> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
    {
        if (request.Role != Role.Admin)
        {
            throw new NotRightsException(request.Role);
        }
        var statistics = new StatisticsDTO
        {
            CountAdmins = (await _userCollection.FindAsync(Builders<User>.Filter.Eq("Role", Role.Admin), null, cancellationToken))
            .ToEnumerable()
            .Count(),
            CountUsers = (await _userCollection.FindAsync(Builders<User>.Filter.Eq("Role", Role.User), null, cancellationToken))
            .ToEnumerable()
            .Count(),
            CountComments = (await _commentCollection.FindAsync(Builders<Comment>.Filter.Eq("_t", "Comment"), null, cancellationToken)).ToEnumerable().Count(),
            CountRating = (await _ratingCollection.FindAsync(Builders<Rating>.Filter.Eq("_t", "Rating"), null, cancellationToken)).ToEnumerable().Count(),
            ApprovedArticles = (await _articleCollection.FindAsync(Builders<Article>.Filter.Eq("_t", "Article") & Builders<Article>.Filter.Eq("State", State.Approved), null, cancellationToken)).ToEnumerable().Count(),
            DeclinedArticles = (await _articleCollection.FindAsync(Builders<Article>.Filter.Eq("_t", "Article") & Builders<Article>.Filter.Eq("State", State.Declined), null, cancellationToken)).ToEnumerable().Count(),
            WaitingArticles = (await _articleCollection.FindAsync(Builders<Article>.Filter.Eq("_t", "Article") & Builders<Article>.Filter.Eq("State", State.Waiting), null, cancellationToken)).ToEnumerable().Count(),
        };

        return statistics;
    }
}
