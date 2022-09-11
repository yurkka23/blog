using AutoMapper;
using Blog.Application.Articles.Commands.UpdateArticle;
using Blog.Application.Common.Mappings;

namespace Blog.WebApi.DTOs.ArticeDTOs
{
    public class UpdateArticleDTO : IMapWith<UpdateArticleCommand>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateArticleDTO, UpdateArticleCommand>();
        }
    }
}
