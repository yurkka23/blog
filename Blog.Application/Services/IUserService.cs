using Blog.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Blog.Application.Services;

public interface IUserService
{
    public bool IsAdmin(HttpContext context);
    public RefreshToken GenerateRefreshToken();
    public Task SetRefreshToken(RefreshToken token, User user, HttpContext context, CancellationToken cancellationToken);
    public string CreateToken(User user);
}
