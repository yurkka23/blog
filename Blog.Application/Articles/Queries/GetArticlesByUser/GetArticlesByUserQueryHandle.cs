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
            .Include(a => a.User)
            .AsNoTracking()
            .Where(article => article.UserId == request.UserId)
            .OrderByDescending(article => article.CreatedTime)
            .ProjectTo<ArticleByUserLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
      
        return new ArticleListByUser { Articles = articleQuery };
       
    }
    
}
