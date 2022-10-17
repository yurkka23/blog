namespace Blog.WebApi.DTOs.CommentDTOs;

public class UpdateCommentDTO : IMapWith<UpdateCommentCommand>
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Message { get; set; } = null!;
    [Required]
    public Guid ArticleId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateCommentDTO, UpdateCommentCommand>();
    }
}
