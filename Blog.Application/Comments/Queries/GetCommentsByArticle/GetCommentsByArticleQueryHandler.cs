using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq;

namespace Blog.Application.Comments.Queries.GetCommentsByArticle;

public class GetCommentsByArticleQueryHandler : IRequestHandler<GetCommentsByArticleQuery, CommentList>
{
    private readonly IMongoCollection<Comment> _entitiesCollection;
    private readonly IMapper _mapper;
    private readonly IMongoCollection<User> _userCollection;

    public GetCommentsByArticleQueryHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, IMapper mapper, IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<Comment>(
            entitiesStoreDatabaseSettings.Value.CollectionName);

        _mapper = mapper;

        var mongoClient1 = new MongoClient(
          userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase1 = mongoClient1.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase1.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<CommentList> Handle(GetCommentsByArticleQuery request, CancellationToken cancellationToken)
    {
        var comments = (await _entitiesCollection
           .FindAsync(Builders<Comment>.Filter.Eq("_t", "Comment") & Builders<Comment>.Filter.Eq("ArticleId", request.ArticleId),null,cancellationToken))
           .ToEnumerable()
           .OrderByDescending(c => c.CreatedTime)
           .Select(com => new CommentLookupDto
           {
               Id = com.EntityId,
               Message = com.Message,
               AuthorUserName = _userCollection.Find(user => user.Id == com.UserId).FirstOrDefault().UserName,
               AuthorImgUrl = _userCollection.Find(user => user.Id == com.UserId).FirstOrDefault().ImageUserUrl
           }).ToList();

        return new CommentList { Comments = comments };
    }
}
