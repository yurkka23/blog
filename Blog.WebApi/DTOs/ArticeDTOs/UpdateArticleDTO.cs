namespace Blog.WebApi.DTOs.ArticeDTOs;

public class UpdateArticleDTO : IMapWith<UpdateArticleCommand>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string ArticleImageUrl { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateArticleDTO, UpdateArticleCommand>();
    }
}
