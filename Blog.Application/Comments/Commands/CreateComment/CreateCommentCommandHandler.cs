using Blog.Domain.Models;
using MediatR;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.Application.Comments.Commands.CreateComment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
{
    private readonly IMongoCollection<MongoEntity> _entitiesCollection;

    public CreateCommentCommandHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<MongoEntity>(
            entitiesStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new Comment
        {
            EntityId = Guid.NewGuid(),
            UserId = request.UserId,
            Message = request.Message,
            ArticleId = request.ArticleId,
            CreatedTime = DateTime.UtcNow
        };

        await _entitiesCollection.InsertOneAsync(comment, cancellationToken);

        return comment.EntityId;
    }
}
