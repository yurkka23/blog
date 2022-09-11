using MediatR;
namespace Blog.Application.Ratings.Queries.GetRatingByArticle;

public class GetRatingListByArticleQuery : IRequest<RatingListVm>
{
    public Guid ArticleId { get; set; }
}
