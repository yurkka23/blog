namespace Blog.WebApi.DTOs.RatingDTOs;

public class CreateRatingDTO  : IMapWith<CreateRatingCommand>
{
    [Required]
    public Guid ArticleId { get; set; }
    [Required]
    public byte Score { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateRatingDTO, CreateRatingCommand>();
    }
}
