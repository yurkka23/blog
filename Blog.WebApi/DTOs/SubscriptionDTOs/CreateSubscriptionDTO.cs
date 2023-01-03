using Blog.Application.UserSubscriptions.Commands.CreateSubscription;

namespace Blog.WebApi.DTOs.SubscriptionDTOs;

public class CreateSubscriptionDTO : IMapWith<CreateSubscriptionCommand>
{
    public Guid UserToSubscribeId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateSubscriptionDTO, CreateSubscriptionCommand>();
    }
}