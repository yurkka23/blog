using System;
using MediatR;

namespace Blog.Application.Articles.Commands.CreateArticle;

public class CreateArticleCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string ArticleImageUrl { get; set; } = string.Empty;
}
