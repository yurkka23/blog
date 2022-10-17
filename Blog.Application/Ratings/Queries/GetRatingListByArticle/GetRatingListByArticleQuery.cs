using MediatR;
namespace Blog.Application.Ratings.Queries.GetRatingByArticle;

public class GetRatingListByArticleQuery : IRequest<RatingList>
{
    public Guid ArticleId { get; set; }
}
