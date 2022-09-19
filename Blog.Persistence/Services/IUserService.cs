﻿using Blog.Application.Interfaces;
using Blog.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Blog.Persistence.Services;

public interface IUserService
{
    public Guid GetUserId(HttpContext context);
    public bool IsAdmin(HttpContext context);
    public RefreshToken GenerateRefreshToken();
   public  Task SetRefreshToken(RefreshToken token, User user, HttpContext context,  IBlogDbContext blogDbContext, CancellationToken cancellationToken);
    public string CreateToken(User user, IConfiguration _configuration);
  
}
