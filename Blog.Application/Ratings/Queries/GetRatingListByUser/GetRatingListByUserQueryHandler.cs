using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.Application.Ratings.Queries.GetRatingListByUser;

public class GetRatingListByUserQueryHandler : IRequestHandler<GetRatingListByUserQuery, RatingList>
{
    private readonly IMongoCollection<Rating> _ratingCollection;
    private readonly IMongoCollection<Article> _articleCollection;
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMapper _mapper;

    public GetRatingListByUserQueryHandler(IMapper mapper, IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
    {
        _mapper = mapper;

        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _ratingCollection = mongoDatabase.GetCollection<Rating>(
            entitiesStoreDatabaseSettings.Value.CollectionName);

        _articleCollection = mongoDatabase.GetCollection<Article>(
           entitiesStoreDatabaseSettings.Value.CollectionName);

        var mongoClientUser = new MongoClient(
          userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabaseUser = mongoClientUser.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabaseUser.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<RatingList> Handle(GetRatingListByUserQuery request, CancellationToken cancellationToken)
    {
       
        var ratingQuery = _ratingCollection
           .Find(Builders<Rating>.Filter.Eq("_t", "Rating") & Builders<Rating>.Filter.Eq("UserId", request.UserId))
           .SortByDescending(x => x.CreatedTime)
           .ToEnumerable()
           .Select(ent => new RatingLookupDto
           {
               Id = ent.EntityId,
               ArticleId = ent.ArticleId,
               ArticleImage = _articleCollection.Find(Builders<Article>.Filter.Eq("_t", "Article") & Builders<Article>.Filter.Eq("_id", ent.ArticleId)).First().ArticleImageUrl,
               ArticleTitle = _articleCollection.Find(Builders<Article>.Filter.Eq("_t", "Article") & Builders<Article>.Filter.Eq("_id", ent.ArticleId)).First().Title,
               Score = ent.Score
           })
           .ToList();

        return new RatingList { Ratings = ratingQuery };
    }

}
