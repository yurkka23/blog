using Blog.Application.Interfaces;
using Blog.Domain.Models;
using MediatR;

namespace Blog.Application.UserSubscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand>
{
    private readonly IBlogDbContext _dbContext;
    public CreateSubscriptionCommandHandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Unit> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = new UserSubscription { 
            UserId = request.UserId , 
            UserToSubscribeId = request.UserToSubscribeId
        };

        await _dbContext.UserSubscriptions.AddAsync(subscription, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
