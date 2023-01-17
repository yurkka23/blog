using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using Blog.Domain.Enums;
using Blog.Application.Caching;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.Application.Articles.Commands.UpdateArticle;

public class UpdateArticleCommandHandler : AsyncRequestHandler<UpdateArticleCommand>
{
    private readonly ICacheService _cacheService;
    private readonly IMongoCollection<Article> _articleCollection;
    private readonly IMongoCollection<MongoEntity> _entitiesCollection;


    public UpdateArticleCommandHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, ICacheService cacheService)
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
    protected override async Task Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        var entity = (await _articleCollection
          .FindAsync(x => x.EntityId == request.Id, null,cancellationToken))
          .FirstOrDefault();

        if (entity == null)
        {
            throw new NotFoundException(nameof(Article), request.Id);
        }
        if (entity.UserId != request.UserId)
        {
            throw new NotRightsException(request.Id);
        }

        entity.Title = request.Title;
        entity.Content = request.Content;
        entity.Genre = request.Genre;
        entity.ArticleImageUrl = request.ArticleImageUrl;
        entity.UpdatedTime = DateTime.UtcNow;
        entity.State = State.Waiting;
        entity.UpdatedBy = request.UserId;

        await _entitiesCollection.ReplaceOneAsync(x => x.EntityId == request.Id, entity, new ReplaceOptions { IsUpsert = false }, cancellationToken);

        var t1 = _cacheService.DeleteAsync($"ArticleListByGenre {entity.Genre}");
        var t2 = _cacheService.DeleteAsync("ArticleListSearch");
        var t3 = _cacheService.DeleteAsync($"Article {entity.EntityId}");
        await Task.WhenAll(t1, t2, t3);

    }
}
