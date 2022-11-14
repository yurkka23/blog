using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using Blog.Domain.Enums;
using Blog.Application.Caching;

namespace Blog.Application.Articles.Commands.DeleteArticle;

public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
{
    private readonly IBlogDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public DeleteArticleCommandHandler(IBlogDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }
    public async Task<Unit> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
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

        await _cacheService.DeleteAsync($"ArticleListByGenre {entity.Genre}");
        await _cacheService.DeleteAsync("ArticleListSearch");
        await _cacheService.DeleteAsync($"Article {entity.Id}");

        return Unit.Value;
    }
}


