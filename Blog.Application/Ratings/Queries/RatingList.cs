
namespace Blog.Application.Ratings.Queries;

public class RatingList
{
    public IList<RatingLookupDto> Ratings { get; set; } = new List<RatingLookupDto>();
}
