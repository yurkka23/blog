﻿using Blog.Application.Common.Mappings;
using Blog.Domain.Models;
using AutoMapper;

namespace Blog.Application.Ratings.Queries;

public class RatingLookupDto : IMapWith<Rating>
{
    public Guid Id { get; set; }
    public byte Score { get; set; }
    public string ArticleTitle { get; set; }
    public string ArticleImage { get; set; }
    public Guid ArticleId { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Rating, RatingLookupDto>().ForMember(x => x.Id, o => o.MapFrom(s => s.EntityId));
    }
}
