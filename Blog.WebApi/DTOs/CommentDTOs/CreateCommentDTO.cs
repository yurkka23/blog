using Blog.Application.Common.Mappings;
using AutoMapper;
using Blog.Application.Comments.Commands.CreateComment;

namespace Blog.WebApi.DTOs.CommentDTOs;

public class CreateCommentDTO : IMapWith<CreateCommentCommand>
{
    //public int Id { get; set; }
    public string Message { get; set; } = null!;
    public Guid ArticleId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateCommentDTO, CreateCommentCommand>();
    }
}
