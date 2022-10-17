namespace Blog.WebApi.DTOs.CommentDTOs;

public class CreateCommentDTO : IMapWith<CreateCommentCommand>
{
    public string Message { get; set; } = string.Empty;
   
    public Guid ArticleId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateCommentDTO, CreateCommentCommand>();
    }
}
