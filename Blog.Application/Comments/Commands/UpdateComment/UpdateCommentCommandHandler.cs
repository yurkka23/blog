using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.Application.Comments.Commands.UpdateComment;

public class UpdateCommentCommandHandler : AsyncRequestHandler<UpdateCommentCommand>
{
    private readonly IMongoCollection<Comment> _commentCollection;
    private readonly IMongoCollection<MongoEntity> _entitiesCollection;
    public UpdateCommentCommandHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _commentCollection = mongoDatabase.GetCollection<Comment>(
            entitiesStoreDatabaseSettings.Value.CollectionName);

        _entitiesCollection = mongoDatabase.GetCollection<MongoEntity>(
           entitiesStoreDatabaseSettings.Value.CollectionName);
    }
    protected override async Task Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = (await _commentCollection
          .FindAsync(x => x.EntityId == request.Id, null, cancellationToken))
          .FirstOrDefault();

        if (entity == null)
        {
            throw new NotFoundException(nameof(Comment), request.Id);
        }
        if (entity.UserId != request.UserId)
        {
            throw new NotRightsException(request.Id);
        }

        entity.Message = request.Message;

        await _entitiesCollection.ReplaceOneAsync(x => x.EntityId == request.Id, entity, new ReplaceOptions { IsUpsert = false }, cancellationToken);
    }
}
