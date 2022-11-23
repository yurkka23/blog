using Blog.Application.Caching;
using Blog.Application.Common.Exceptions;
using Blog.Application.Interfaces;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Users.Commands.ChangeRoleToAdmin;

public class ChangeRoleToAdminCommandHandler : AsyncRequestHandler<ChangeRoleToAdminCommand>
{
    private readonly IBlogDbContext _dbContext;
    private readonly ICacheService _cacheService;
    public ChangeRoleToAdminCommandHandler(IBlogDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }
    protected override async Task Handle(ChangeRoleToAdminCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(User), request.UserId);
        }

        if (request.Role != Role.Admin)
        {
            throw new NotRightsException(request.UserId);
        }

        entity.Role = Role.Admin;

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cacheService.DeleteAsync("UserListSearch");

    }
}
