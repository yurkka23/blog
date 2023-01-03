using MediatR;

namespace Blog.Application.UserSubscriptions.Commands.DeleteSubscription;

public class DeleteSubscriptionCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid UserToSubscribeId { get; set; }
}
