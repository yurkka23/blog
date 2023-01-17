using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Application.Caching;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq;
using Blog.Application.Settings;

namespace Blog.Application.Articles.Queries.SearchArticlesByTitle;

public class SearchArticlesByTitleQueryHandler : IRequestHandler<SearchArticlesByTitleQuery, ArticleList>
{
    private readonly IMapper _mapper;
    private readonly IMongoCollection<Article> _entitiesCollection;
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<Rating> _ratingCollection;

    public SearchArticlesByTitleQueryHandler(IMapper mapper, IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
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
    public async Task<ArticleList> Handle(SearchArticlesByTitleQuery request, CancellationToken cancellationToken)
    {
        // var cachedEntity = await _cacheService.GetAsync<ArticleList>($"ArticleListSearch {request.PartTitle}");

        //if (cachedEntity != default)
        //{
        //    return cachedEntity;
        //}
        
        var articleQuery = (await _entitiesCollection
          .FindAsync(Builders<Article>.Filter.Eq("_t", "Article") & Builders<Article>.Filter.Eq("State", request.State) & Builders<Article>.Filter.AnyEq("Title", request.PartTitle.Trim()), null, cancellationToken))
          .ToEnumerable()
          .OrderByDescending(art => art.CreatedTime)
          .Select(ent => new ArticleLookupDto
          {
              Id = ent.EntityId,
              Title = ent.Title,
              Content = ent.Content,
              Genre = ent.Genre,
              CreatedBy = ent.CreatedBy,
              CreatedTime = (DateTime)ent.CreatedTime,
              ArticleImageUrl = ent.ArticleImageUrl,
              AuthorFullName = _userCollection.Find(x => x.Id == ent.UserId).First().FirstName + ' ' + _userCollection.Find(x => x.Id == ent.UserId).First().LastName,
              AverageRating = _ratingCollection.Find(Builders<Rating>.Filter.Eq("_t", "Rating") & Builders<Rating>.Filter.Eq("ArticleId", ent.EntityId)).CountDocuments() > 0 ? _ratingCollection.Find(Builders<Rating>.Filter.Eq("_t", "Rating") & Builders<Rating>.Filter.Eq("ArticleId", ent.EntityId)).ToEnumerable().Select(r => (int)r.Score).AsQueryable().Average() : 0
          }).ToList();

        var result = new ArticleList { Articles = articleQuery };

        //await _cacheService.CreateAsync($"ArticleListSearch {request.PartTitle}", result);

        return result;
    }
}

