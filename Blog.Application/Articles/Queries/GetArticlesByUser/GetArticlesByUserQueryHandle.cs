using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;

namespace Blog.Application.Articles.Queries.GetArticlesByUser;

public class GetArticlesByUserQueryHandle : IRequestHandler<GetArticlesByUserQuery, ArticleListVm>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetArticlesByUserQueryHandle(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<ArticleListVm> Handle(GetArticlesByUserQuery request, CancellationToken cancellationToken)
    {
        
        var articleQuery = await _dbContext.Articles
            .Where(article => article.UserId == request.UserId)
            .ProjectTo<ArticleLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (request.UserId == Guid.Empty)
        {
            throw new NotFoundException(nameof(User), request.UserId);
        }

        return new ArticleListVm { Articles = articleQuery };
    }
}
