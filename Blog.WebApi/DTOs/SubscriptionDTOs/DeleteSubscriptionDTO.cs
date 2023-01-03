using Blog.Application.UserSubscriptions.Commands.DeleteSubscription;

namespace Blog.WebApi.DTOs.SubscriptionDTOs;

public class DeleteSubscriptionDTO : IMapWith<DeleteSubscriptionCommand>
{
    public Guid UserToSubscribeId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<DeleteSubscriptionDTO, DeleteSubscriptionCommand>();
    }
}