namespace Blog.WebApi.DTOs.ArticeDTOs;

public class UpdateArticleDTO : IMapWith<UpdateArticleCommand>
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateArticleDTO, UpdateArticleCommand>();
    }
}
