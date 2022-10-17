namespace Blog.Application.Comments.Queries.GetCommentsByArticle;

public class CommentList
{
    public IList<CommentLookupDto> Comments { get; set; } = new List<CommentLookupDto>();
}
