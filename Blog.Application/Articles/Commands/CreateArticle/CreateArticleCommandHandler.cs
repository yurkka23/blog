using System;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using MediatR;
using Blog.Domain;
using Blog.Application.Caching;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.Application.Articles.Commands.CreateArticle;

public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, Guid>
{
    private readonly ICacheService _cacheService;   
    private readonly IMongoCollection<MongoEntity> _entitiesCollection;

    public CreateArticleCommandHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, ICacheService cacheService)
    {
        _cacheService = cacheService;
        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<MongoEntity>(
            entitiesStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<Guid> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var article = new Article
        {
            EntityId = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            Content = request.Content,
            Genre = request.Genre,
            ArticleImageUrl = request.ArticleImageUrl,
            State = State.Waiting,
            CreatedTime = DateTime.UtcNow,
            UpdatedTime = null,
            CreatedBy = request.UserId,
            UpdatedBy = null
        };

        await _entitiesCollection.InsertOneAsync(article, cancellationToken);

        await _cacheService.DeleteAsync($"ArticleListByGenre {request.Genre}");
        await _cacheService.DeleteAsync("ArticleListSearch");

        return article.EntityId;
    }
}
