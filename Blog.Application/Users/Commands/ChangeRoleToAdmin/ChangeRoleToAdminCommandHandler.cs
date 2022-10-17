using Blog.Application.Common.Exceptions;
using Blog.Application.Interfaces;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Users.Commands.ChangeRoleToAdmin;

public class ChangeRoleToAdminCommandHandler : IRequestHandler<ChangeRoleToAdminCommand>
{
    private readonly IBlogDbContext _dbContext;
    public ChangeRoleToAdminCommandHandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Unit> Handle(ChangeRoleToAdminCommand request, CancellationToken cancellationToken)
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
        return Unit.Value;
    }
}
