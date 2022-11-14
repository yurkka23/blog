namespace Blog.Application.Articles.Queries.GetArticlesByUser;

public class ArticleListByUser 
{  
    public IList<ArticleByUserLookupDto>? Articles { get; set; } = new List<ArticleByUserLookupDto>();
}
