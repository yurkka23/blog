using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Caching;
using Blog.Application.Users.Queries.GetUsersByRole;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Blog.Application.Settings;
using Blog.Domain.Models;
using Microsoft.Extensions.Options;

namespace Blog.Application.Users.Queries.SearchUser;

public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, UserList>
{
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly IMongoCollection<User> _userCollection;

    public SearchUserQueryHandler(IOptions<MongoUserDBSettings> userStoreDatabaseSettings, IMapper mapper, ICacheService cacheService)
    {
        _mapper = mapper;
        _cacheService = cacheService;
        _mapper = mapper;
        var mongoClient = new MongoClient(
           userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<UserList> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        //var cachedEntity = await _cacheService.GetAsync<UserList>($"UserListSearch {request.PartUsername}");

        //if (cachedEntity != default)
        //{
        //    return cachedEntity;
        //}
        var userQuery = await _userCollection
           .Find(x => x.Role == request.Role && x.UserName.Contains(request.PartUsername.Trim()))
           .Project(user => new UserLookUpDto
           {
               FirstName = user.FirstName,
               LastName = user.LastName,
               AboutMe = user.AboutMe,
               Id = user.Id,
               ImageUserUrl = user.ImageUserUrl,
               Role = user.Role,
               UserName = user.UserName
           })
           .ToListAsync(cancellationToken);

        var result = new UserList { Users = userQuery };

       // await _cacheService.CreateAsync($"UserListSearch {request.PartUsername}", result);

        return result;
    }
}

