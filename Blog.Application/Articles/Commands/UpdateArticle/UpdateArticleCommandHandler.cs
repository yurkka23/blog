using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using Blog.Domain.Enums;

namespace Blog.Application.Articles.Commands.UpdateArticle;

public class UpdateArticleCommandHandler: AsyncRequestHandler<UpdateArticleCommand>
{
    private readonly IBlogDbContext _dbContext;
    public UpdateArticleCommandHandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    protected override async Task Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Articles.FirstOrDefaultAsync(article => article.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Article), request.Id);
        }
        if(entity.UserId != request.UserId)
        {
            throw new NotRightsException(request.Id);
        }

        entity.Title = request.Title;
        entity.Content = request.Content;
        entity.Genre = request.Genre;
        entity.ArticleImageUrl = request.ArticleImageUrl;
        entity.UpdatedTime = DateTime.UtcNow;
        entity.State = State.Waiting;
        entity.UpdatedBy = request.UserId;

        await _dbContext.SaveChangesAsync(cancellationToken);


    }
}
