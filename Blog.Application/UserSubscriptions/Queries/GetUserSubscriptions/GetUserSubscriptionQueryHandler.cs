using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Interfaces;
using Blog.Application.Users.Queries.GetUsersByRole;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UserSubscriptions.Queries.GetUserSubscriptions;

public class GetUserSubscriptionQueryHandler : IRequestHandler<GetUserSubscriptionQuery, UserList>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetUserSubscriptionQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<UserList> Handle(GetUserSubscriptionQuery request, CancellationToken cancellationToken)
    {
        var userToSubscribeIdQuery = await _dbContext.UserSubscriptions
            .Where(user => user.UserToSubscribeId == request.UserId)
            .Select(u => u.UserId)
            .ToListAsync(cancellationToken);

        var usersQuery = await _dbContext.Users
            .Where(u => userToSubscribeIdQuery.Contains(u.Id))
            .Select(u => new UserLookUpDto
            {
                UserName = u.UserName,
                ImageUserUrl = u.ImageUserUrl,
                AboutMe = u.AboutMe,
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role
            })
            .ToListAsync(cancellationToken);

        return new UserList { Users = usersQuery };
    }
}