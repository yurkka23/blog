using Blog.Application.Common.Exceptions;
using Blog.Application.Interfaces;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using MediatR;

namespace Blog.Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
{
    private readonly IBlogDbContext _dbContext;
    public DeleteCommentCommandHandler( IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Comments.FindAsync(new object[] { request.Id }, cancellationToken);

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

        return Unit.Value;
    }
}
