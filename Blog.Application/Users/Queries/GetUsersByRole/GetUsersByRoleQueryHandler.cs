using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Blog.Application.Users.Queries.GetUsersByRole;

public class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQuery, UserList>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetUsersByRoleQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<UserList> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
    {
        var userQuery = await _dbContext.Users
            .Where(user => user.Role == request.Role)
            .OrderBy(user => user.UserName)
            .ProjectTo<UserLookUpDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);


        return new UserList { Users = userQuery };


    }
}
