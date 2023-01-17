using MediatR;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using Blog.Application.Caching;
using MongoDB.Driver;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;

namespace Blog.Application.Users.Commands.EditUserInfo;

public class EditUserInfoCommandHandler : AsyncRequestHandler<EditUserInfoCommand>
{
    private readonly ICacheService _cacheService;
    private readonly IMongoCollection<User> _userCollection;

    public EditUserInfoCommandHandler(IOptions<MongoUserDBSettings> userStoreDatabaseSettings, ICacheService cacheService)
    {
        _cacheService = cacheService;
        var mongoClient = new MongoClient(
          userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);
    }
    protected override async Task Handle(EditUserInfoCommand request, CancellationToken cancellationToken)
    {
        var entity = (await _userCollection
           .FindAsync(Builders<User>.Filter.Eq("_id", request.Id), null, cancellationToken))
           .FirstOrDefault();

        if (entity == null || entity.Id != request.Id)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        if (entity.UserName != request.UserName)
        {
            var checkIfUserExist = (await _userCollection
            .FindAsync(Builders<User>.Filter.Eq("UserName", request.UserName)))
            .FirstOrDefault();

            if (checkIfUserExist != null)
            {
                throw new Exception("User with such username already exists");
            }
        }

        if (entity.UserName != request.UserName)
        {
            await _cacheService.DeleteAsync("UserListSearch");
        }

        entity.UserName = request.UserName;
        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.AboutMe = request.AboutMe;
        entity.ImageUserUrl = request.ImageUserUrl;

        await _userCollection.ReplaceOneAsync(Builders<User>.Filter.Eq("_id", request.Id), entity, new ReplaceOptions { IsUpsert = false });

    }
}
