using System;
using AutoMapper;
using Blog.Application.Common.Mappings;
using Blog.Domain.Enums;
using Blog.Domain.Models;

namespace Blog.Application.Articles.Queries.GetArticleContent
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
            profile.CreateMap<Article, ArticleContentVm>();
        }
    }
}
