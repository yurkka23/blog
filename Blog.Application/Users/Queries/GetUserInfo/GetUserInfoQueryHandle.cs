using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Blog.Application.Settings;

namespace Blog.Application.Users.Queries.GetUserInfo;

public class GetUserInfoQueryHandle : IRequestHandler<GetUserInfoQuery, UserInfo>
{
    private readonly IMapper _mapper;
    private readonly IMongoCollection<User> _userCollection;

    public GetUserInfoQueryHandle(IOptions<MongoUserDBSettings> userStoreDatabaseSettings, IMapper mapper)
    {
        _mapper = mapper;
        var mongoClient = new MongoClient(
           userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<UserInfo> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var userQuery = (await _userCollection.FindAsync(x => x.Id == request.Id, null, cancellationToken)).FirstOrDefault();

        if (userQuery == null)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        return _mapper.Map<UserInfo>(userQuery);
    }
}
