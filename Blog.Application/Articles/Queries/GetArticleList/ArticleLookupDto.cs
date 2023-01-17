using AutoMapper;
using Blog.Application.Common.Mappings;
using Blog.Domain.Helpers;
using Blog.Domain.Models;

namespace Blog.Application.Articles.Queries.GetArticleList;

public class ArticleLookupDto : IMapWith<Article>
{
    
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string ArticleImageUrl { get; set; } = string.Empty;
    public double AverageRating { get; set; }
    public string AuthorFullName { get; set; } = string.Empty;  
    public DateTime CreatedTime { get; set; }
    public Guid CreatedBy { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Article, ArticleLookupDto>();
            //.ForMember(art => art.AuthorFullName, art => art.MapFrom(map => map.User.FirstName + ' ' + map.User.LastName));
           //.ForMember(art => art.AverageRating, art => art.MapFrom(map => ArticleHelper.GetAverageRating(map)));
    }
}
