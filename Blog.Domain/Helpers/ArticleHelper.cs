using Blog.Domain.Models;

namespace Blog.Domain.Helpers;

public static class ArticleHelper
{
    public static double GetAverageRating(IList<Rating> ratings)
    {
        double averageRating = ratings.Count() > 0 ? ratings.Average(r => r.Score) : 0;
        return averageRating;
    }
}
