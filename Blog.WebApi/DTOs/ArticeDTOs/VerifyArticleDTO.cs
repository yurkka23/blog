namespace Blog.WebApi.DTOs.ArticeDTOs;

public class VerifyArticleDTO : IMapWith<VerifyArticleCommand>
{
    public Guid Id { get; set; }
    public State State { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<VerifyArticleDTO, VerifyArticleCommand>();
    }
}
