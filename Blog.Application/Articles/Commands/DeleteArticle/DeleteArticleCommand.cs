using System;
using Blog.Domain.Enums;
using MediatR;
namespace Blog.Application.Articles.Commands.DeleteArticle;

public class DeleteArticleCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid Id { get; set; }
    public Role Role { get; set; }
}
