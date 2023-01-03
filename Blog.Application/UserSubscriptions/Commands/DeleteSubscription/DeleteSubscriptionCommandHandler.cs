

using Blog.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UserSubscriptions.Commands.DeleteSubscription;

public class DeleteSubscriptionCommandHandler : IRequestHandler<DeleteSubscriptionCommand>
{
    private readonly IBlogDbContext _dbContext;
    public DeleteSubscriptionCommandHandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Unit> Handle(DeleteSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.UserSubscriptions.FirstOrDefaultAsync(s => s.UserId == request.UserId && s.UserToSubscribeId == request.UserToSubscribeId, cancellationToken);

        _dbContext.UserSubscriptions.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
