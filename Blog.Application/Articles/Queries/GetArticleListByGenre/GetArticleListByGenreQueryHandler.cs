using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Domain.Helpers;
using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Application.Common.Exceptions;
using Blog.Application.Caching;

namespace Blog.Application.Articles.Queries.GetArticleListByGenre;

public class GetArticleListByGenreQueryHandler : IRequestHandler<GetArticleListByGenreQuery, ArticleList>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public GetArticleListByGenreQueryHandler(IBlogDbContext dbContext, IMapper mapper, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _cacheService = cacheService;
    }
    public async Task<ArticleList> Handle(GetArticleListByGenreQuery request, CancellationToken cancellationToken)
    {
        var cachedEntity = await _cacheService.GetAsync<ArticleList>($"ArticleListByGenre {request.Genre}");

        if (cachedEntity != default)
        {
            return cachedEntity;
        }

        var articleQuery = await _dbContext.Articles
            .Include(a => a.Ratings)
            .Include(a => a.User)
            .AsNoTracking()
            .Where(article => article.State == request.State && article.Genre == request.Genre.Trim())
            .OrderByDescending(article => article.CreatedTime)
            .ProjectTo<ArticleLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var result = new ArticleList { Articles = articleQuery };
        await _cacheService.CreateAsync($"ArticleListByGenre {request.Genre}", result);

        return result;
        
    }
}

