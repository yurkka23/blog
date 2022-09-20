namespace Blog.WebApi.DTOs.CommentDTOs;

public class CreateCommentDTO : IMapWith<CreateCommentCommand>
{
    [Required]
    public string Message { get; set; } = null!;
    [Required]
    public Guid ArticleId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateCommentDTO, CreateCommentCommand>();
    }
}
