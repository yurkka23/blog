using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Domain.Helpers;

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
            .AsNoTracking()
            .Where(article => article.State == request.State)
            .OrderByDescending(article => article.CreatedTime)
            .ToListAsync(cancellationToken);

        var articesList = new List<ArticleLookupDto>();

        if (articleQuery.Count > 0)
        {
            var index = 0;
            foreach (var article in articleQuery)
            {
                var getAuthorName = await _dbContext.Users
                .Where(user => user.Id == articleQuery[index].CreatedBy)
                .ToListAsync(cancellationToken);

                var temp = _mapper.Map<ArticleLookupDto>(article);
                temp.AverageRating = ArticleHelper.GetAverageRating(article);
                temp.AuthorFullName = getAuthorName[0].FirstName + ' ' + getAuthorName[0].LastName;
                articesList.Add(temp);
                index++;
            }

       

        }
       
        return new ArticleList { Articles = articesList };
        
            
    }
}
