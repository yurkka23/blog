using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using Blog.Application.Caching;


namespace Blog.Application.Users.Commands.EditUserInfo;

public class EditUserInfoCommandHandler : IRequestHandler<EditUserInfoCommand>
{
    private readonly IBlogDbContext _dbContext;
    private readonly ICacheService _cacheService;
    public EditUserInfoCommandHandler(IBlogDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }
    public async Task<Unit> Handle(EditUserInfoCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == request.Id, cancellationToken);
        
        
        if (entity == null || entity.Id != request.Id)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        if (entity.UserName != request.UserName)
        {
            var checkIfUserExist = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == request.UserName, cancellationToken);
            if (checkIfUserExist != null)
            {
                throw new Exception("User with uch username already exists");
            }
        }

        if(entity.UserName != request.UserName)
        {
            await _cacheService.DeleteAsync("UserListSearch");
        }

        entity.UserName = request.UserName;
        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.AboutMe = request.AboutMe;
        entity.ImageUserUrl = request.ImageUserUrl;
      
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
