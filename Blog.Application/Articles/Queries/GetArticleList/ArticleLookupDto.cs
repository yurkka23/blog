using AutoMapper;
using Blog.Application.Common.Mappings;
using Blog.Domain.Models;

namespace Blog.Application.Articles.Queries.GetArticleList;

public class ArticleLookupDto : IMapWith<Article>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Article, ArticleLookupDto>();
    }
}
