using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using Blog.Domain.Enums;
using AutoMapper;

namespace Blog.Application.Users.Queries.GetArticleContent
{
    public class GetArticleDetailsQueryHandler : IRequestHandler<GetArticleContentQuery, ArticleContentVm>
    {
        private readonly IBlogDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetArticleDetailsQueryHandler(IBlogDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ArticleContentVm> Handle(GetArticleContentQuery request , CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Articles
                .FirstOrDefaultAsync(article => article.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Article), request.Id);
            }
            return _mapper.Map<ArticleContentVm>(entity);
        }
    }
}
