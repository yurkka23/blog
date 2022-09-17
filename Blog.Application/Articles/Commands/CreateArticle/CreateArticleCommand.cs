using System;
using MediatR;

namespace Blog.Application.Articles.Commands.CreateArticle;

//this class contain what need to create article
public class CreateArticleCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
}
