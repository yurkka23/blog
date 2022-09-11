using Blog.Application.Common.Exceptions;
using Blog.Application.Interfaces;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Comments.Commands.UpdateComment;

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand>
{
    private readonly IBlogDbContext _dbContext;
    public UpdateCommentCommandHandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Unit> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Comments.FirstOrDefaultAsync(comment => comment.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Comment), request.Id);
        }
        if (entity.UserId != request.UserId)
        {
            throw new NotRightsException(request.Id);
        }

        entity.Message = request.Message;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
