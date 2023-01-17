using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using Blog.Domain.Enums;
using Blog.Application.Caching;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.Application.Articles.Commands.DeleteArticle;

public class DeleteArticleCommandHandler : AsyncRequestHandler<DeleteArticleCommand>
{
    private readonly ICacheService _cacheService;
    private readonly IMongoCollection<Article> _entitiesCollection;

    public DeleteArticleCommandHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, ICacheService cacheService)
    {
        _cacheService = cacheService;
        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<Article>(
            entitiesStoreDatabaseSettings.Value.CollectionName);
    }
    protected override async Task Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var entity = (await _entitiesCollection
           .FindAsync(x => x.EntityId == request.Id, null, cancellationToken))
           .FirstOrDefault();

        if (entity == null)
        {
            throw new NotFoundException(nameof(Article), request.Id);
        }
        if (entity.UserId != request.UserId && request.Role != Role.Admin)
        {
            throw new NotRightsException(request.UserId);
        }

        await _entitiesCollection.DeleteOneAsync(x => x.EntityId == request.Id, cancellationToken);

        var t1 = _cacheService.DeleteAsync($"ArticleListByGenre {entity.Genre}");
        var t2 = _cacheService.DeleteAsync("ArticleListSearch");
        var t3 = _cacheService.DeleteAsync($"Article {entity.EntityId}");

        await Task.WhenAll(t1, t2, t3);
    }
}


