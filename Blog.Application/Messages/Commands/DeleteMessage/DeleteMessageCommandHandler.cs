using Blog.Application.Common.Exceptions;
using Blog.Application.Interfaces;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Messages.Commands.DeleteMessage;

public class DeleteMessageCommandHandler : AsyncRequestHandler<DeleteMessageCommand>
{
    private readonly IBlogDbContext _dbContext;
    public DeleteMessageCommandHandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    protected override async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _dbContext.Messages.FirstOrDefaultAsync(ent => ent.Id == request.MessageId, cancellationToken);
        if (message.SenderId == request.UserId) message.SenderDeleted = true;
        if (message.RecipienId == request.UserId) message.RecipientDeleted = true;

        if (message.SenderDeleted && message.RecipientDeleted)
        {
            _dbContext.Messages.Remove(message);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

    }
}
