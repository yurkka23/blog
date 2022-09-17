using Blog.Domain.Enums;
using Blog.Domain.Models;
using MediatR;
using Blog.Domain;
using Blog.Application.Interfaces;

namespace Blog.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandhandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IBlogDbContext _dbContext;
    public RegisterUserCommandhandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            PasswordHash = request.PasswordHash,
            PasswordSalt = request.PasswordSalt,
            UserName = request.UserName,
            AboutMe = string.Empty,
            FirstName = string.Empty,
            LastName = string.Empty,    
            Role = Role.User,
            RefreshToken = string.Empty,
            TokenCreated = DateTime.Now,
            TokenExpires = DateTime.Now
        };
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
