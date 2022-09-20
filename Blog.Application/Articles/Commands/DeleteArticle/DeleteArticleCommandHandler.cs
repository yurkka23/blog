using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using Blog.Domain.Enums;

namespace Blog.Application.Articles.Commands.DeleteArticle;

public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
{
    private readonly IBlogDbContext _dbContext;
    public DeleteArticleCommandHandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Unit> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        //var entity = await _dbContext.Articles.FindAsync(new object[] { request.Id }, cancellationToken);
         var entity = await _dbContext.Articles.FirstOrDefaultAsync(ent => ent.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Article), request.Id);
        }
        if (entity.UserId != request.UserId && request.Role != Role.Admin)
        {
            throw new NotRightsException(request.UserId);
        }
        

        _dbContext.Articles.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}


