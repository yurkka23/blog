using System;
using MediatR;

namespace Blog.Application.Users.Queries.GetArticleContent
{
    public class GetArticleContentQuery: IRequest<ArticleContentVm>
    {
        public Guid Id { get; set; }
    }
}
