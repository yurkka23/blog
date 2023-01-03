using Blog.Application.Messages.Commands.CreateMessage;

namespace Blog.WebApi.DTOs.MessageDTOs;

public class CreateMessageDTO : IMapWith<CreateMessageCommand>
{
    public Guid RecipientId { get; set; }
    public string Content { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateMessageDTO, CreateMessageCommand>();
    }
}
