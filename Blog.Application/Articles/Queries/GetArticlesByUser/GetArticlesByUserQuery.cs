
using Blog.Application.Users.Queries.GetArticleList;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Articles.Queries.GetArticlesByUser
{
    public class GetArticlesByUserQuery : IRequest<ArticleListVm>
    {
        public Guid UserId { get; set; }
    }
}
