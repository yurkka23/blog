using MediatR;

namespace Blog.Application.Ratings.Queries.GetRatingListByUser;

public class GetRatingListByUserQuery:  IRequest<RatingList>
{
    public Guid UserId { get; set; }
}
