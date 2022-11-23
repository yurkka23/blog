using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using AutoMapper;
using Blog.Domain.Helpers;
using Blog.Application.Caching;

namespace Blog.Application.Articles.Queries.GetArticleContent;

public class GetArticleDetailsQueryHandler : IRequestHandler<GetArticleContentQuery, ArticleContent>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    public GetArticleDetailsQueryHandler(IBlogDbContext dbContext, IMapper mapper, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _cacheService = cacheService;
    }
    public async Task<ArticleContent> Handle(GetArticleContentQuery request , CancellationToken cancellationToken)
    {
        var cachedEntity = await _cacheService.GetAsync<ArticleContent>($"Article {request.Id}");

        if (cachedEntity != default)
        {
            return cachedEntity;
        }
       
        var entity = await _dbContext.Articles
                       .Include(art => art.Ratings)
                       .AsNoTracking()
                       .FirstOrDefaultAsync(article => article.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Article), request.Id);
        }
        
        var result = _mapper.Map<ArticleContent>(entity);

        var getAuthorName = await _dbContext.Users
            .Where(user => user.Id == result.CreatedBy)
            .ToListAsync(cancellationToken);

        result.AverageRating = ArticleHelper.GetAverageRating(entity);
        result.AuthorImageUrl = getAuthorName[0].ImageUserUrl;
        result.AuthorFullName = getAuthorName[0].FirstName + ' ' + getAuthorName[0].LastName;
        result.IsRatedByCurrentUser = entity.Ratings.Any(u => u.UserId == request.UserId);

        await _cacheService.CreateAsync($"Article {request.Id}", result);

        return result;
    }
}
