using MediatR;
using AutoMapper;
using Blog.Domain.Models;
using Blog.Application.Caching;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.Application.Articles.Queries.GetArticleList;

public class GetArticleListQueryHandle : IRequestHandler<GetArticleListQuery, ArticleList>
{
    private readonly IMongoCollection<Article> _entitiesCollection;
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<Rating> _ratingCollection;

    public GetArticleListQueryHandle( IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
    {
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
    public async Task<ArticleList> Handle(GetArticleListQuery request, CancellationToken cancellationToken)
    {
        var articleQuery =(await _entitiesCollection
            .FindAsync(Builders<Article>.Filter.Eq("_t", "Article") & Builders<Article>.Filter.Eq("State", request.State), null, cancellationToken))
            .ToEnumerable()
            .OrderByDescending(art => art.CreatedTime)
            .Select( ent => new ArticleLookupDto
            {
                Id = ent.EntityId,
                Title = ent.Title,
                Content = ent.Content,
                Genre = ent.Genre,
                CreatedBy = ent.CreatedBy,
                CreatedTime = (DateTime)ent.CreatedTime,
                ArticleImageUrl = ent.ArticleImageUrl,
                AuthorFullName =_userCollection.Find(x => x.Id == ent.UserId).FirstOrDefault().FirstName + ' ' + _userCollection.Find(x => x.Id == ent.UserId).FirstOrDefault().LastName,
                AverageRating =_ratingCollection.Find(Builders<Rating>.Filter.Eq("_t", "Rating") & Builders<Rating>.Filter.Eq("ArticleId", ent.EntityId)).CountDocuments() > 0 ? _ratingCollection.Find(Builders<Rating>.Filter.Eq("_t", "Rating") & Builders<Rating>.Filter.Eq("ArticleId", ent.EntityId)).ToEnumerable().Select(r => (int)r.Score).AsQueryable().Average() : 0
            }).ToList();
        
        return new ArticleList { Articles = articleQuery };
    }
}
