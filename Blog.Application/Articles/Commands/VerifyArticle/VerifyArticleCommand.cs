using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Articles.Commands.VerifyArticle;

public class VerifyArticleCommand : IRequest
{
    public Guid Id { set; get; }
    public State State { set; get; }
    public Role Role { set; get; }
    
}
