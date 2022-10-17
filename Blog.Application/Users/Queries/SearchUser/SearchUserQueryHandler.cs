using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Interfaces;
using Blog.Application.Users.Queries.GetUsersByRole;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Users.Queries.SearchUser;

public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, UserList>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public SearchUserQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<UserList> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        var userQuery = await _dbContext.Users
            .Where(user => user.Role == request.Role)
            .Where(user => user.UserName.Contains(request.PartUsername.Trim()))
            .ProjectTo<UserLookUpDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new UserList { Users = userQuery };
    }
}

