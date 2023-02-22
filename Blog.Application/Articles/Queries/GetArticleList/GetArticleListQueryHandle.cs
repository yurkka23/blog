using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Domain.Helpers;
using Blog.Application.Caching;
using Blog.Application.Common.Helpers;

namespace Blog.Application.Articles.Queries.GetArticleList;

public class GetArticleListQueryHandle : IRequestHandler<GetArticleListQuery, PagedList<ArticleLookupDto>>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetArticleListQueryHandle(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<PagedList<ArticleLookupDto>> Handle(GetArticleListQuery request, CancellationToken cancellationToken)
    {
        var articleQuery = _dbContext.Articles
            .Include(a => a.Ratings)
            .Include(a => a.User)
            .AsNoTracking()
            .Where(article => article.State == request.State)
            .OrderByDescending(article => article.CreatedTime);
            //.ProjectTo<ArticleLookupDto>(_mapper.ConfigurationProvider)
            //.ToListAsync(cancellationToken);

       // return new ArticleList { Articles = articleQuery };
        return await PagedList<ArticleLookupDto>.CreateAsync(articleQuery.ProjectTo<ArticleLookupDto>(_mapper
               .ConfigurationProvider),
                   request.PageNumber, request.PageSize);
    }
}
