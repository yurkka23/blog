using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Application.Interfaces;
using Blog.Domain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Articles.Queries.SearchArticlesByTitle;

public class SearchArticlesByTitleQueryHandler : IRequestHandler<SearchArticlesByTitleQuery, ArticleList>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;

    public SearchArticlesByTitleQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<ArticleList> Handle(SearchArticlesByTitleQuery request, CancellationToken cancellationToken)
    {
       

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

        return result;
    }
}

