using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Users.Queries.GetArticleList
{
    public class ArticleListVm
    {
        public IList<ArticleLookupDto> Articles { get; set; }

    }
}
