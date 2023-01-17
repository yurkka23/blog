using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Blog.Application.Settings;
using Blog.Domain.Models;

namespace Blog.Application.Users.Queries.GetUsersByRole;

public class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQuery, UserList>
{
    private readonly IMapper _mapper;
    private readonly IMongoCollection<User> _userCollection;

    public GetUsersByRoleQueryHandler(IOptions<MongoUserDBSettings> userStoreDatabaseSettings, IMapper mapper)
    {
        _mapper = mapper;
        var mongoClient = new MongoClient(
           userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<UserList> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
    {
        var userQuery = await _userCollection.Find(x => x.Role == request.Role)
            .SortBy(user => user.UserName)
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

        return new UserList { Users = userQuery };
    }
}
