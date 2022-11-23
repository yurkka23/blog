using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Application.Caching;
using Blog.Application.Interfaces;
using Blog.Domain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Articles.Queries.SearchArticlesByTitle;

public class SearchArticlesByTitleQueryHandler : IRequestHandler<SearchArticlesByTitleQuery, ArticleList>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public SearchArticlesByTitleQueryHandler(IBlogDbContext dbContext, IMapper mapper, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _cacheService = cacheService;
    }
    public async Task<ArticleList> Handle(SearchArticlesByTitleQuery request, CancellationToken cancellationToken)
    {
        var cachedEntity = await _cacheService.GetAsync<ArticleList>($"ArticleListSearch {request.PartTitle}");

        if (cachedEntity != default)
        {
            return cachedEntity;
        }

        var articleQuery = await _dbContext.Articles
            .Include(a => a.Ratings)
            .Include(a => a.User)
            .AsNoTracking()
            .Where(article => article.State == request.State)
            .Where(article => article.Title.Contains(request.PartTitle.Trim()))
            .OrderByDescending(article => article.CreatedTime)
            .ProjectTo<ArticleLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
         
        var result = new ArticleList { Articles = articleQuery };

        await _cacheService.CreateAsync($"ArticleListSearch {request.PartTitle}", result);

        return result;
    }
}

