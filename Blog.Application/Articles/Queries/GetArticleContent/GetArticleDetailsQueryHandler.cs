using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using AutoMapper;
using Blog.Domain.Helpers;
using Blog.Application.Caching;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.Application.Articles.Queries.GetArticleContent;

public class GetArticleDetailsQueryHandler : IRequestHandler<GetArticleContentQuery, ArticleContent>
{
    private readonly IMapper _mapper;
    private readonly IMongoCollection<Article> _entitiesCollection;
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<Rating> _ratingCollection;

    public GetArticleDetailsQueryHandler( IMapper mapper, IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
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

        _ratingCollection = mongoDatabase.GetCollection<Rating>(
            entitiesStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<ArticleContent> Handle(GetArticleContentQuery request, CancellationToken cancellationToken)
    {
       
        var entity = (await _entitiesCollection.FindAsync(x => x.EntityId == request.Id,null, cancellationToken)).FirstOrDefault();

        if (entity == null)
        {
            throw new NotFoundException(nameof(Article), request.Id);
        }

        var result = _mapper.Map<ArticleContent>(entity);

        var author =await _userCollection.Find(u => u.Id == result.CreatedBy).FirstOrDefaultAsync(cancellationToken);
        

        result.AverageRating = _ratingCollection.Find(Builders<Rating>.Filter.Eq("_t", "Rating") & Builders<Rating>.Filter.Eq("ArticleId", request.Id)).CountDocuments() > 0 ? _ratingCollection.Find(Builders<Rating>.Filter.Eq("_t", "Rating") & Builders<Rating>.Filter.Eq("ArticleId", request.Id)).ToEnumerable().Average(r => r.Score) : 0;
        result.AuthorImageUrl = author.ImageUserUrl;
        result.AuthorFullName = author.FirstName + ' ' + author.LastName;
        result.AuthorId = entity.CreatedBy;
        result.IsRatedByCurrentUser = _ratingCollection.Find(Builders<Rating>.Filter.Eq("_t", "Rating") & Builders<Rating>.Filter.Eq("ArticleId", request.Id) & Builders<Rating>.Filter.Eq("UserId", request.UserId)).CountDocuments() > 0 ? true : false;

        return result;
    }
}
