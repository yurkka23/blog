using Blog.Domain.Models;
using MediatR;
using Blog.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Common.Exceptions;

namespace Blog.Application.Messages.Commands.CreateMessage;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Guid>
{
    private readonly IBlogDbContext _dbContext;

    public CreateMessageCommandHandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Guid> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {

        var sender = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.SenderId, cancellationToken);
        var recipient = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.RecipientId, cancellationToken);

        var message = new Message
        { 
            RecipienId = recipient.Id,
            SenderId = sender.Id,
            Content = request.Content.Trim(),
            SenderUsername = sender.UserName,
            RecipienUsername = recipient.UserName,
            Sender = sender,
            Recipient = recipient,
            MessageSent = DateTime.UtcNow
        };

        await _dbContext.Messages.AddAsync(message, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return message.Id;
    }

}
