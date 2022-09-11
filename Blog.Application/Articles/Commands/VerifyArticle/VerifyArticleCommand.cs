using System;
using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Articles.Commands.VerifyArticle
{
    public class VerifyArticleCommand : IRequest
    {
        public Guid Id { set; get; }
        public State state { set; get; }
        public Guid UserId { set; get; }
        public Role Role { set; get; }
        
    }
}
