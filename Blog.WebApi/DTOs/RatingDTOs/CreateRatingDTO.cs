namespace Blog.WebApi.DTOs.RatingDTOs;

public class CreateRatingDTO  : IMapWith<CreateRatingCommand>
{
    
    public Guid ArticleId { get; set; }
    public byte Score { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateRatingDTO, CreateRatingCommand>();
    }
}
