using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using Blog.Domain.Helpers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq;
using Blog.Application.Settings;

namespace Blog.Application.Articles.Queries.GetArticlesByUser;

public class GetArticlesByUserQueryHandle : IRequestHandler<GetArticlesByUserQuery, ArticleListByUser>
{
    private readonly IMapper _mapper;
    private readonly IMongoCollection<Article> _entitiesCollection;
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<Rating> _ratingCollection;

    public GetArticlesByUserQueryHandle(IMapper mapper, IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
    {
        _mapper = mapper;
        var mongoClient = new MongoClient(
          entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<Article>(
            entitiesStoreDatabaseSettings.Value.CollectionName);

        _userCollection = mongoDatabase.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);

        var mongoClient1 = new MongoClient(
         entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase1 = mongoClient1.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);
        _ratingCollection = mongoDatabase1.GetCollection<Rating>(
            entitiesStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<ArticleListByUser> Handle(GetArticlesByUserQuery request, CancellationToken cancellationToken)
    {
        var articleQuery = (await _entitiesCollection
             .FindAsync(Builders<Article>.Filter.Eq("_t", "Article") & Builders<Article>.Filter.Eq("UserId", request.UserId), null,cancellationToken))
             .ToEnumerable()
           .OrderByDescending(art => art.CreatedTime)
           .Select(ent => new ArticleByUserLookupDto
           {
               Id = ent.EntityId,
               Title = ent.Title,
               Content = ent.Content,
               Genre = ent.Genre,
               CreatedTime = (DateTime)ent.CreatedTime,
               ArticleImageUrl = ent.ArticleImageUrl,
               State = ent.State,
               AverageRating = _ratingCollection.Find(Builders<Rating>.Filter.Eq("_t", "Rating") & Builders<Rating>.Filter.Eq("ArticleId", ent.EntityId)).CountDocuments() > 0 ? _ratingCollection.Find(Builders<Rating>.Filter.Eq("_t", "Rating") & Builders<Rating>.Filter.Eq("ArticleId", ent.EntityId)).ToEnumerable().Select(r => (int)r.Score).AsQueryable().Average() : 0
           }).ToList();
        
        return new ArticleListByUser { Articles = articleQuery };

    }

}
