using System;
using Blog.Application.Common.Helpers;
using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Articles.Queries.GetArticleList;

public class GetArticleListQuery : PaginationParams, IRequest<PagedList<ArticleLookupDto>>
{
    public State State { get; set; }
}
