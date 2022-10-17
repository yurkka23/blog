using System;
using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Articles.Queries.GetArticleList;

public class GetArticleListQuery : IRequest<ArticleList>
{
    public State State { get; set; }
}
