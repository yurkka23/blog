using Blog.Application.Common.Exceptions;
using Blog.Application.Interfaces;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Domain.Enums;
using Blog.Application.Caching;

namespace Blog.Application.Articles.Commands.VerifyArticle;

public class VerifyArticleCommandHandler : IRequestHandler<VerifyArticleCommand>
{
    private readonly IBlogDbContext _dbContext;
    private readonly ICacheService _cacheService;
    public VerifyArticleCommandHandler(IBlogDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;   
    }
    public async Task<Unit> Handle(VerifyArticleCommand request, CancellationToken cancellationToken)
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

        await _cacheService.DeleteAsync($"ArticleListByGenre {entity.Genre}");
        await _cacheService.DeleteAsync("ArticleListSearch");
        await _cacheService.DeleteAsync($"Article {entity.Id}");

        return Unit.Value;
    }
}
