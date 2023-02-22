using Blog.Domain.Models;

namespace Blog.Domain.Helpers;

public static class ArticleHelper
{
    public static double GetAverageRating(Article article)
    {
        double averageRating = article.Ratings.Count > 0 ? article.Ratings.Average(r => r.Score) : 0;
        return averageRating;
    }
}
