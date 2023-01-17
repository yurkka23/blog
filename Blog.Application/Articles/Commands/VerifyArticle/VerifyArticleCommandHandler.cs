using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Domain.Enums;
using Blog.Application.Caching;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.Application.Articles.Commands.VerifyArticle;

public class VerifyArticleCommandHandler : AsyncRequestHandler<VerifyArticleCommand>
{
    private readonly ICacheService _cacheService;
    private readonly IMongoCollection<Article> _articleCollection;
    private readonly IMongoCollection<MongoEntity> _entitiesCollection;
    public VerifyArticleCommandHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, ICacheService cacheService)
    {
        _cacheService = cacheService;
        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<MongoEntity>(
            entitiesStoreDatabaseSettings.Value.CollectionName);

        _articleCollection = mongoDatabase.GetCollection<Article>(
            entitiesStoreDatabaseSettings.Value.CollectionName);
    }
    protected override async Task Handle(VerifyArticleCommand request, CancellationToken cancellationToken)
    {
        var entity = (await _articleCollection
                  .FindAsync(x => x.EntityId == request.Id, null, cancellationToken))
                  .FirstOrDefault();

        if (entity == null)
        {
            throw new NotFoundException(nameof(Article), request.Id);
        }

        if (request.Role != Role.Admin)
        {
            throw new NotRightsException(request.Id);
        }

        entity.State = request.State;

        await _entitiesCollection.ReplaceOneAsync(x => x.EntityId == request.Id, entity, new ReplaceOptions { IsUpsert = false }, cancellationToken);

        var t1 = _cacheService.DeleteAsync($"ArticleListByGenre {entity.Genre}");
        var t2 = _cacheService.DeleteAsync("ArticleListSearch");
        var t3 = _cacheService.DeleteAsync($"Article {entity.EntityId}");

        await Task.WhenAll(t1, t2, t3);

    }
}
