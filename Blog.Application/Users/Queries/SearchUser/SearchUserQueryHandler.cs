using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Caching;
using Blog.Application.Interfaces;
using Blog.Application.Users.Queries.GetUsersByRole;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Users.Queries.SearchUser;

public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, UserList>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    public SearchUserQueryHandler(IBlogDbContext dbContext, IMapper mapper, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _cacheService = cacheService;
    }
    public async Task<UserList> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        var cachedEntity = await _cacheService.GetAsync<UserList>($"UserListSearch {request.PartUsername}");

        if (cachedEntity != default)
        {
            return cachedEntity;
        }

        var userQuery = await _dbContext.Users
            .Where(user => user.Role == request.Role)
            .Where(user => user.UserName.Contains(request.PartUsername.Trim()))
            .ProjectTo<UserLookUpDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var result = new UserList { Users = userQuery };

        await _cacheService.CreateAsync($"UserListSearch {request.PartUsername}", result);

        return result;
    }
}

