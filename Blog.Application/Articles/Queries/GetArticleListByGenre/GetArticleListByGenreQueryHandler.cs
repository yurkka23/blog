using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Domain.Helpers;
using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Application.Common.Exceptions;

namespace Blog.Application.Articles.Queries.GetArticleListByGenre;

public class GetArticleListByGenreQueryHandler : IRequestHandler<GetArticleListByGenreQuery, ArticleList>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetArticleListByGenreQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<ArticleList> Handle(GetArticleListByGenreQuery request, CancellationToken cancellationToken)
    {

       

        var articleQuery = await _dbContext.Articles
            .Include(a => a.Ratings)
            .Include(a => a.User)
            .AsNoTracking()
            .Where(article => article.State == request.State && article.Genre == request.Genre.Trim())
            .OrderByDescending(article => article.CreatedTime)
            .ProjectTo<ArticleLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var result = new ArticleList { Articles = articleQuery };

        return result;
        
    }
}

