namespace Blog.Application.Users.Queries.GetStatistics;

public class StatisticsDTO
{
    public int CountAdmins { get; set; }
    public int CountUsers { get; set; }
    public int ApprovedArticles { get; set; }
    public int WaitingArticles { get; set; }
    public int DeclinedArticles { get; set; }
    public int CountComments { get; set; }
    public int CountRating { get; set; }

}
