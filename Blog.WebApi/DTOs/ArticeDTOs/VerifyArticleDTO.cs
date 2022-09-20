namespace Blog.WebApi.DTOs.ArticeDTOs;

public class VerifyArticleDTO : IMapWith<VerifyArticleCommand>
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public State State { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<VerifyArticleDTO, VerifyArticleCommand>();
    }
}
