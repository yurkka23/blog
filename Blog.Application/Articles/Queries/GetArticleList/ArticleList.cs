
namespace Blog.Application.Articles.Queries.GetArticleList;

public class ArticleList
{
    public IList<ArticleLookupDto> Articles { get; set; } = new List<ArticleLookupDto>();
}
