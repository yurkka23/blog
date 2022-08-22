using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using Blog.Domain.Enums;

namespace Blog.Application.Users.Commands.UpdateArticle
{
    public class UpdateArticleHandler: IRequestHandler<UpdateArticleCommand>
    {
        private readonly IBlogDbContext _dbContext;
        public UpdateArticleHandler(IBlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Articles.FirstOrDefaultAsync(article => article.Id == request.Id, cancellationToken);

            if (entity == null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Article), request.Id);
            }

            entity.Title = request.Title;
            entity.Content = request.Content;
            entity.UpdatedTime = DateTime.Now;
            entity.State = State.Waiting;
            entity.UpdatedBy = request.UserId;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
