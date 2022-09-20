using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Blog.Application.Articles.Queries.GetArticleList;

public class GetArticleListQueryHandle : IRequestHandler<GetArticleListQuery, ArticleListVm>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetArticleListQueryHandle(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<ArticleListVm> Handle(GetArticleListQuery request, CancellationToken cancellationToken)
    {
        var articleQuery = await _dbContext.Articles
            .Include(a => a.Ratings)
            .Where(article => article.State == request.State)
            .ProjectTo<ArticleLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new ArticleListVm { Articles = articleQuery};
    }
}
