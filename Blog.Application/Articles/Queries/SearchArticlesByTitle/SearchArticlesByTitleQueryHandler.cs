using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Application.Caching;
using Blog.Application.Common.Helpers;
using Blog.Application.Interfaces;
using Blog.Domain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Articles.Queries.SearchArticlesByTitle;

public class SearchArticlesByTitleQueryHandler : IRequestHandler<SearchArticlesByTitleQuery, PagedList<ArticleLookupDto>>
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
    public async Task<PagedList<ArticleLookupDto>> Handle(SearchArticlesByTitleQuery request, CancellationToken cancellationToken)
    {
        //var cachedEntity = await _cacheService.GetAsync<ArticleList>($"ArticleListSearch {request.PartTitle}");

        //if (cachedEntity != default)
        //{
        //    return cachedEntity;
        //}
        var articleQuery = _dbContext.Articles
           .Include(a => a.Ratings)
           .Include(a => a.User)
           .AsNoTracking()
           .Where(article => article.State == request.State && article.Title.Contains(request.PartTitle.Trim()))
           .OrderByDescending(article => article.CreatedTime);     
        //  await _cacheService.CreateAsync($"ArticleListSearch {request.PartTitle}", result);

        return await PagedList<ArticleLookupDto>.CreateAsync(articleQuery.ProjectTo<ArticleLookupDto>(_mapper
               .ConfigurationProvider),
                   request.PageNumber, request.PageSize);
    }
}

