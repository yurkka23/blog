using System;
using AutoMapper;
using Blog.Application.Common.Mappings;
using Blog.Domain.Enums;
using Blog.Domain.Models;

namespace Blog.Application.Users.Queries.GetArticleContent
{
    //view model will return to client
    public class ArticleContentVm : IMapWith<Article>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public State State { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public User User { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Article, ArticleContentVm>()
                .ForMember(artVm => artVm.Title,
                    opt => opt.MapFrom(art => art.Title))
                .ForMember(artVm => artVm.Content,
                    opt => opt.MapFrom(art => art.Content))
                .ForMember(artVm => artVm.CreatedTime,
                    opt => opt.MapFrom(art => art.CreatedTime))
                .ForMember(artVm => artVm.UpdatedTime,
                    opt => opt.MapFrom(art => art.UpdatedTime))
                .ForMember(artVm => artVm.State,
                    opt => opt.MapFrom(art => art.State))
                .ForMember(artVm => artVm.CreatedBy,
                    opt => opt.MapFrom(art => art.CreatedBy))
                .ForMember(artVm => artVm.UpdatedBy,
                    opt => opt.MapFrom(art => art.UpdatedBy));
        }
    }
}
