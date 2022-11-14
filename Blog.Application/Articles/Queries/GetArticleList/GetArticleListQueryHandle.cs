using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Domain.Helpers;
using Blog.Application.Caching;

namespace Blog.Application.Articles.Queries.GetArticleList;

public class GetArticleListQueryHandle : IRequestHandler<GetArticleListQuery, ArticleList>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetArticleListQueryHandle(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<ArticleList> Handle(GetArticleListQuery request, CancellationToken cancellationToken)
    {
        var articleQuery = await _dbContext.Articles
            .Include(a => a.Ratings)
            .Include(a => a.User)
            .AsNoTracking()
            .Where(article => article.State == request.State)
            .OrderByDescending(article => article.CreatedTime)
            .ProjectTo<ArticleLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new ArticleList { Articles = articleQuery }; 
    }
}
