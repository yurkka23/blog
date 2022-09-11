using Blog.Domain.Models;
using MediatR;
using Blog.Application.Interfaces;

namespace Blog.Application.Comments.Commands.CreateComment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, int>
{
    private readonly IBlogDbContext _dbContext;
    public CreateCommentCommandHandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<int> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new Comment
        {
            UserId = request.UserId,
            Message = request.Message,
            ArticleId = request.ArticleId,        
        };
        await _dbContext.Comments.AddAsync(comment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return comment.Id;
    }
}
