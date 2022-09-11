using System;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using MediatR;
using Blog.Domain;
using Blog.Application.Interfaces;

namespace Blog.Application.Articles.Commands.CreateArticle
{
    //logic to create article
    public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, Guid>//1 request, 2 response
    {
        private readonly IBlogDbContext _dbContext;
        public CreateArticleCommandHandler(IBlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Guid> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            var article = new Article
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Title = request.Title,
                Content = request.Content,
                State = State.Waiting,
                CreatedTime = DateTime.Now,
                UpdatedTime = null,
                CreatedBy = request.UserId,
                UpdatedBy = null
            };
            await _dbContext.Articles.AddAsync(article, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return article.Id;
        }
    }
}
