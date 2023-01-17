using AutoMapper;
using Blog.Application.Common.Mappings;
using Blog.Domain.Helpers;
using Blog.Domain.Models;

namespace Blog.Application.Messages.Queries.GetMessagesFromGroup;

public class MessageDTO : IMapWith<Message>
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public string SenderUsername { get; set; }
    public string SenderPhotoUrl { get; set; }

    public Guid RecipienId { get; set; }
    public string RecipienUsername { get; set; }
    public string RecipientPhotoUrl { get; set; }
    public string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Message, MessageDTO>().ForMember(x => x.Id, o => o.MapFrom(s => s.EntityId));
            //.ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.ImageUserUrl))
           // .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s => s.Recipient.ImageUserUrl));
    }
}
