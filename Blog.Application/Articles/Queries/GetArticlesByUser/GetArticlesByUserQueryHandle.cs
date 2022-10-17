using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using Blog.Domain.Helpers;

namespace Blog.Application.Articles.Queries.GetArticlesByUser;

public class GetArticlesByUserQueryHandle : IRequestHandler<GetArticlesByUserQuery, ArticleListByUser>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetArticlesByUserQueryHandle(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<ArticleListByUser> Handle(GetArticlesByUserQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            throw new NotFoundException(nameof(User), request.UserId);
        }

        var articleQuery = await _dbContext.Articles
            .Include(a => a.Ratings)
            .AsNoTracking()
            .Where(article => article.UserId == request.UserId)
            .OrderByDescending(article => article.CreatedTime)
            .ToListAsync(cancellationToken);

        var articesList = new List<ArticleByUserLookupDto>();
        foreach (var article in articleQuery)
        {
            var temp = _mapper.Map<ArticleByUserLookupDto>(article);
            temp.AverageRating = ArticleHelper.GetAverageRating(article);
            articesList.Add(temp);

        }

        return new ArticleListByUser { Articles = articesList };
    }
}
