using Blog.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Users.Queries.CheckUser;

public class CheckUserQueryHandler : IRequestHandler<CheckUserQuery, bool>
{
    private readonly IBlogDbContext _dbContext;
    public CheckUserQueryHandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> Handle(CheckUserQuery request, CancellationToken cancellationToken)
    {
        var userQuery = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == request.UserName, cancellationToken);

        if (userQuery == null)
        {
            return false;
        }

        return true;
    }
}
