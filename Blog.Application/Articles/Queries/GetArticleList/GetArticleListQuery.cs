using System;
using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Users.Queries.GetArticleList
{
    public class GetArticleListQuery : IRequest<ArticleListVm>
    {
        public State State { get; set; }

    }
}
