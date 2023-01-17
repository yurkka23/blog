using Blog.Application.Common.Exceptions;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler : AsyncRequestHandler<DeleteCommentCommand>
{
    private readonly IMongoCollection<Comment> _entitiesCollection;

    public DeleteCommentCommandHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<Comment>(
            entitiesStoreDatabaseSettings.Value.CollectionName);
    }
    protected override async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = (await _entitiesCollection
          .FindAsync(x => x.EntityId == request.Id, null, cancellationToken))
          .FirstOrDefault();

        if (entity == null)
        {
            throw new NotFoundException(nameof(Comment), request.Id);
        }
        if (entity.UserId != request.UserId && request.Role != Role.Admin)
        {
            throw new NotRightsException(request.UserId);
        }

        await _entitiesCollection.DeleteOneAsync(x => x.EntityId == request.Id, cancellationToken);
    }
}
