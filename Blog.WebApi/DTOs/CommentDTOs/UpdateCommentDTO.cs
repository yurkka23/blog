namespace Blog.WebApi.DTOs.CommentDTOs;

public class UpdateCommentDTO : IMapWith<UpdateCommentCommand>
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Message { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateCommentDTO, UpdateCommentCommand>();
    }
}
