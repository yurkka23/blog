using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using AutoMapper;
using Blog.Domain.Helpers;

namespace Blog.Application.Articles.Queries.GetArticleContent;

public class GetArticleDetailsQueryHandler : IRequestHandler<GetArticleContentQuery, ArticleContent>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetArticleDetailsQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<ArticleContent> Handle(GetArticleContentQuery request , CancellationToken cancellationToken)
    {
       
        var entity = await _dbContext.Articles
                       .Include(art => art.Ratings)
                       .AsNoTracking()
                       .FirstOrDefaultAsync(article => article.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Article), request.Id);
        }
        
        var result = _mapper.Map<ArticleContent>(entity);

        var getAuthorName = await _dbContext.Users
            .Where(user => user.Id == result.CreatedBy)
            .ToListAsync(cancellationToken);

        result.AverageRating = ArticleHelper.GetAverageRating(entity);
        result.AuthorImageUrl = getAuthorName[0].ImageUserUrl;
        result.AuthorFullName = getAuthorName[0].FirstName + ' ' + getAuthorName[0].LastName;
        result.IsRatedByCurrentUser = entity.Ratings.Any(u => u.UserId == request.UserId);
        result.AuthorId = result.CreatedBy;

        return result;
    }
}
