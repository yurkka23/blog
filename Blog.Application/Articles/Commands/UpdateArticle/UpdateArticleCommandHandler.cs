using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using Blog.Domain.Enums;
using Blog.Application.Caching;

namespace Blog.Application.Articles.Commands.UpdateArticle;

public class UpdateArticleCommandHandler: IRequestHandler<UpdateArticleCommand>
{
    private readonly IBlogDbContext _dbContext;
    private readonly ICacheService _cacheService;
    public UpdateArticleCommandHandler(IBlogDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }
    public async Task<Unit> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Articles.FirstOrDefaultAsync(article => article.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Article), request.Id);
        }
        if(entity.UserId != request.UserId)
        {
            throw new NotRightsException(request.Id);
        }

        entity.Title = request.Title;
        entity.Content = request.Content;
        entity.Genre = request.Genre;
        entity.ArticleImageUrl = request.ArticleImageUrl;
        entity.UpdatedTime = DateTime.UtcNow;
        entity.State = State.Waiting;
        entity.UpdatedBy = request.UserId;

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cacheService.DeleteAsync($"ArticleListByGenre {entity.Genre}");
        await _cacheService.DeleteAsync("ArticleListSearch");
        await _cacheService.DeleteAsync($"Article {entity.Id}");

        return Unit.Value;
    }
}
