using AutoMapper;
using Blog.Application.Articles.Commands.VerifyArticle;
using Blog.Application.Common.Mappings;
using Blog.Domain.Enums;

namespace Blog.WebApi.DTOs.ArticeDTOs
{
    public class VerifyArticleDTO : IMapWith<VerifyArticleCommand>
    {
        public Guid Id { get; set; }
        public State State { get; set; }
        public Role Role { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VerifyArticleDTO, VerifyArticleCommand>();
        }
    }
}
