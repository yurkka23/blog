using AutoMapper;
using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Application.Interfaces;
using Blog.Domain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Articles.Queries.GetTopArticles;

public class GetTopArticlesQueryHandler : IRequestHandler<GetTopArticlesQuery, ArticleList>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetTopArticlesQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<ArticleList> Handle(GetTopArticlesQuery request, CancellationToken cancellationToken)
    {
        var articleQuery = await _dbContext.Articles
            .Include(a => a.Ratings)
            .AsNoTracking()
            .Where(article => article.State == request.State)
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
            articesList = articesList.OrderByDescending(art => art.AverageRating).Take(4).ToList();

        }



        return new ArticleList { Articles = articesList };
               
    }
}
