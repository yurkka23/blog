
using AutoMapper;
using Blog.Application.Common.Mappings;
using Blog.Domain.Models;

namespace Blog.Application.Users.Queries.GetArticleList
{
    public class ArticleLookupDto : IMapWith<Article>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Article, ArticleLookupDto>()
                .ForMember(artDto => artDto.Id,
                    opt => opt.MapFrom(art => art.Id))
                .ForMember(artDto => artDto.Title,
                    opt => opt.MapFrom(art => art.Title));
        }
    }
}
