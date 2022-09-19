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

namespace Blog.Persistence.Services;

public class UserService : IUserService
{
    public Guid GetUserId(HttpContext context)
    {
        //if (!context.User.Identity.IsAuthenticated)
        //{
        //     throw new Exception("Not authenticated");
        //}
        var identity = context.User.Identity as ClaimsIdentity;
        var id = identity!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        return Guid.Parse(id);
    }

    public bool IsAdmin(HttpContext context)
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
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now
        };

        return refreshToken;
    }
    public async Task SetRefreshToken(RefreshToken token, User user, HttpContext context, IBlogDbContext blogDbContext , CancellationToken cancellationToken)
    {
      
        var cookieOpts = new CookieOptions
        {
            HttpOnly = true,
            Expires = token.Expires,
            IsEssential = true,
            Secure = true,
        };

        context.Response.Cookies.Append("refreshToken", token.Token, cookieOpts);

        context.Response.Headers.Add("token", token.Token);
        context.Response.Headers.Add("tokenExp", token.Expires.ToString());

        user.RefreshToken = token.Token;
        user.TokenCreated = token.Created;
        user.TokenExpires = token.Expires;
        await blogDbContext.SaveChangesAsync(cancellationToken);
    }
    public string CreateToken(User user, IConfiguration _configuration)
    {
        List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
               
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
    
}
