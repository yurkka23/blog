using MediatR;

namespace Blog.Application.Articles.Commands.UpdateArticle;

public class UpdateArticleCommand: IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

}
