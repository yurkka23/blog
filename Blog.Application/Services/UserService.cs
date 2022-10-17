using Blog.Application.Interfaces;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Application.Services;

public class UserService : IUserService
{
    private IConfiguration _config;
    public UserService(IConfiguration configuration)
    {
        _config = configuration;
    }
    public bool IsAdmin(HttpContext context)//delete check
    {
        var identity = context.User.Identity as ClaimsIdentity;
        var role = identity!.FindFirst(ClaimTypes.Role)!.Value;

        return role == Role.Admin.ToString();
    }

    public RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow
        };

        return refreshToken;
    }
    public async Task SetRefreshToken(RefreshToken token, User user, HttpContext context, IBlogDbContext blogDbContext, CancellationToken cancellationToken)
    {
        user.RefreshToken = token.Token;
        user.TokenCreated = token.Created;
        user.TokenExpires = token.Expires;
        await blogDbContext.SaveChangesAsync(cancellationToken);
    }
    public string CreateToken(User user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _config.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds) ;

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
