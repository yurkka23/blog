using MediatR;

namespace Blog.Application.UserSubscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid UserToSubscribeId {  get; set; }

}
