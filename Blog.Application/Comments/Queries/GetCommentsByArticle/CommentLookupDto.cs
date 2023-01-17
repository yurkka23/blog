using AutoMapper;
using Blog.Application.Common.Mappings;
using Blog.Domain.Models;

namespace Blog.Application.Comments.Queries.GetCommentsByArticle;

public class CommentLookupDto : IMapWith<Comment>
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public string AuthorUserName { get; set; } = string.Empty;
    public string AuthorImgUrl { get; set; } = string.Empty;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Comment, CommentLookupDto>().ForMember(x => x.Id , x => x.MapFrom(src => src.EntityId));
    }
}
