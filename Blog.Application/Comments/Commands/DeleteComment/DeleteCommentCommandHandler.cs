using Blog.Application.Common.Exceptions;
using Blog.Application.Interfaces;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler : AsyncRequestHandler<DeleteCommentCommand>
{
    private readonly IBlogDbContext _dbContext;
    public DeleteCommentCommandHandler( IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    protected override async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Comments.FirstOrDefaultAsync(ent => ent.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Comment), request.Id);
        }
        if(entity.UserId != request.UserId && request.Role != Role.Admin)
        {
            throw new NotRightsException(request.UserId);
        }
        
        _dbContext.Comments.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

    }
}
