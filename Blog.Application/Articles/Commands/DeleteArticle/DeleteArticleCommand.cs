using System;
using MediatR;
namespace Blog.Application.Users.Commands.DeleteArticle
{
    public class DeleteArticleCommand : IRequest
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}
