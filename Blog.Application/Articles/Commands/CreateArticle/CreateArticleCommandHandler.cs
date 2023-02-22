using System;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using MediatR;
using Blog.Domain;
using Blog.Application.Interfaces;
using Blog.Application.Caching;

namespace Blog.Application.Articles.Commands.CreateArticle;

//logic to create article
public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, Guid>//1 request, 2 response
{
    private readonly IBlogDbContext _dbContext;
    private readonly ICacheService _cacheService;
    public CreateArticleCommandHandler(IBlogDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }
    public async Task<Guid> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var article = new Article
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            Content = request.Content,
            Genre = request.Genre,
            ArticleImageUrl = request.ArticleImageUrl,
            State = State.Waiting,
            CreatedTime = DateTime.UtcNow,
            UpdatedTime = null,
            CreatedBy = request.UserId,
            UpdatedBy = null
        };
        await _dbContext.Articles.AddAsync(article, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cacheService.DeleteAsync($"ArticleListByGenre {request.Genre}");
        await _cacheService.DeleteAsync("ArticleListSearch");

        return article.Id;
    }
}
