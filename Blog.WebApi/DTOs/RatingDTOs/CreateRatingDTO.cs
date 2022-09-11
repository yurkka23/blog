using Blog.Application.Common.Mappings;
using AutoMapper;
using Blog.Application.Ratings.Commands.CreateRating;

namespace Blog.WebApi.DTOs.RatingDTOs;

public class CreateRatingDTO  : IMapWith<CreateRatingCommand>
{
   // public int Id { get; set; }
    public Guid ArticleId { get; set; }
    public byte Score { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateRatingDTO, CreateRatingCommand>();
    }
}
