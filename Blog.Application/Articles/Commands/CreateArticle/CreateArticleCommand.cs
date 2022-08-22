using System;
using MediatR;

namespace Blog.Application.Users.Commands.CreateArticle
{
    public class CreateArticleCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

    }
}
