using Blog.Application.Common.Exceptions;
using Blog.Application.Interfaces;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Domain.Enums;

namespace Blog.Application.Articles.Commands.VerifyArticle;

public class VerifyArticleCommandHandler : AsyncRequestHandler<VerifyArticleCommand>
{
    private readonly IBlogDbContext _dbContext;
    public VerifyArticleCommandHandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    protected override async Task Handle(VerifyArticleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Articles.FirstOrDefaultAsync(article => article.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Article), request.Id);
        }

        if (request.Role != Role.Admin)
        {
            throw new NotRightsException(request.Id);
        }
    
        entity.State = request.State;
        
        await _dbContext.SaveChangesAsync(cancellationToken);

       

    }
}
