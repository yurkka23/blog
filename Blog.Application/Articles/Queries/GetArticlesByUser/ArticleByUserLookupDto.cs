using AutoMapper;
using Blog.Application.Common.Mappings;
using Blog.Domain.Enums;
using Blog.Domain.Helpers;
using Blog.Domain.Models;

namespace Blog.Application.Articles.Queries.GetArticlesByUser;

public class ArticleByUserLookupDto : IMapWith<Article>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string ArticleImageUrl { get; set; } = string.Empty;
    public double AverageRating { get; set; }
    public State State { get; set; } 
    public DateTime CreatedTime { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Article, ArticleByUserLookupDto>();
    }
}
