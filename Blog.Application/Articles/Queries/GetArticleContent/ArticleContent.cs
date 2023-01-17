using AutoMapper;
using Blog.Application.Common.Mappings;
using Blog.Domain.Enums;
using Blog.Domain.Models;

namespace Blog.Application.Articles.Queries.GetArticleContent;
public class ArticleContent : IMapWith<Article>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public double AverageRating { get; set; }
    public string Genre { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public string ArticleImageUrl { get; set; } = string.Empty;
    public string AuthorImageUrl { get; set; } = string.Empty;
    public string AuthorFullName { get; set; } = string.Empty;
    public bool IsRatedByCurrentUser { get; set; }
    public State State { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public DateTime? UpdatedTime { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Article, ArticleContent>();
    }
}
