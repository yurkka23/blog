using MediatR;

namespace Blog.Application.Ratings.Queries.GetRatingListByUser;

public class GetRatingListByUserQuery:  IRequest<RatingListVm>
{
    public Guid UserId { get; set; }
}
