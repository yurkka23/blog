using MediatR;

namespace Blog.Application.Articles.Queries.GetArticleContent;

public class GetArticleContentQuery: IRequest<ArticleContent>
{
    public Guid Id { get; set; }
}
