using Blog.Application.Caching;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
namespace Blog.Application.Users.Commands.ChangeRoleToAdmin;

public class ChangeRoleToAdminCommandHandler : AsyncRequestHandler<ChangeRoleToAdminCommand>
{
    private readonly ICacheService _cacheService;
    private readonly IMongoCollection<User> _userCollection;

    public ChangeRoleToAdminCommandHandler(IOptions<MongoUserDBSettings> userStoreDatabaseSettings, ICacheService cacheService)
    {
        _cacheService = cacheService;
        var mongoClient = new MongoClient(
           userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);
    }
    protected override async Task Handle(ChangeRoleToAdminCommand request, CancellationToken cancellationToken)
    {
        var entity = (await _userCollection
            .FindAsync(Builders<User>.Filter.Eq("_id", request.UserId), null, cancellationToken))
            .FirstOrDefault(); 

        if (entity == null)
        {
            throw new NotFoundException(nameof(User), request.UserId);
        }

        if (request.Role != Role.Admin)
        {
            throw new NotRightsException(request.UserId);
        }

        entity.Role = Role.Admin;

        await _userCollection.ReplaceOneAsync(Builders<User>.Filter.Eq("_id", request.UserId), entity, new ReplaceOptions { IsUpsert = false }, cancellationToken);

        await _cacheService.DeleteAsync("UserListSearch");

    }
}
