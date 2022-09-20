using Blog.Application.Common.Mappings;
using Blog.Domain.Models;
using AutoMapper;

namespace Blog.Application.Ratings.Queries;

public class RatingLookupDto : IMapWith<Rating>
{
    public int Id { get; set; }
    public byte Score { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Rating, RatingLookupDto>();
    }
}
