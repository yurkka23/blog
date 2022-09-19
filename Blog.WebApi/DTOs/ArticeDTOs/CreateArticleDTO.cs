using Blog.Application.Articles.Commands.CreateArticle;
using Blog.Application.Common.Mappings;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Blog.WebApi.DTOs.ArticeDTOs;

public class CreateArticleDTO : IMapWith<CreateArticleCommand>
{
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public string Content { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateArticleDTO, CreateArticleCommand>();
    }
}
