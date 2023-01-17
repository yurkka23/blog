using Blog.Domain.Models;
using MediatR;
using Blog.Application.Caching;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.Application.Ratings.Commands.CreateRating;

public class CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand, Guid>
{
    private readonly IMongoCollection<MongoEntity> _entitiesCollection;
    private readonly ICacheService _cacheService;

    public CreateRatingCommandHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, ICacheService cacheService)
    {
        _cacheService = cacheService;
        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<MongoEntity>(
            entitiesStoreDatabaseSettings.Value.CollectionName);

    }
    public async Task<Guid> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
    {
        var rating = new Rating
        {
            EntityId = Guid.NewGuid(),
            UserId = request.UserId,
            Score = request.Score,
            ArticleId = request.ArticleId,
            CreatedTime = DateTime.UtcNow
        };

        await _entitiesCollection.InsertOneAsync(rating, cancellationToken);
        await _cacheService.DeleteAsync($"Article {request.ArticleId}");
        return rating.EntityId;
    }

}
