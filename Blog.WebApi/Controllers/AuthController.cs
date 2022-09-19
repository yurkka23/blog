using AutoMapper;
using Blog.Application.Interfaces;
using Blog.Domain.Models;
using Blog.Persistence.Services;
using Blog.WebApi.DTOs.UserDTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cors;
using Blog.Domain.Enums;
using System.Data.Entity;

namespace Blog.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController : BaseController
{
   
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IBlogDbContext _blogContext;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public AuthController(IConfiguration configuration, IUserService userService, IBlogDbContext blogContext, IMapper mapper,
        SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _configuration = configuration;
        _userService = userService;
        _blogContext = blogContext;
        _mapper = mapper;
        _signInManager = signInManager;
        _userManager = userManager;
    }

   

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody] UserRegisterDTO request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
       
        if (_blogContext.Users.FirstOrDefault(u => u.UserName == request.UserName) != null)
        {
            return BadRequest("User already exists");
        }

        User newUser = new User
        {
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName,
            AboutMe = request.AboutMe,
            Role = Role.User
        };
        
        var result = await _userManager.CreateAsync(newUser, request.Password);


        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(newUser, false);//false - if we close browser, cookie delete 
            return Ok(newUser);//need to return another view model
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login/")]
    public async Task<ActionResult<string>> Login([FromBody] UserLoginDTO request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = _blogContext.Users.FirstOrDefault(u => u.UserName == request.UserName);

        if (user == null)
        {
            return BadRequest("User with such username doesn't exist");
        }

        var res = _signInManager.PasswordSignInAsync(request.UserName, request.Password,false,false);
        var result = _signInManager.CanSignInAsync(user);

        if (!result.Result)
        {
            return BadRequest("Wrong password");
        }
            

        var token = _userService.CreateToken(user , _configuration);
       
        var refreshToken = _userService.GenerateRefreshToken();
        await _userService.SetRefreshToken(refreshToken, user, HttpContext, _blogContext, CancellationToken.None);
        
        return Ok(token);
    }
    [HttpGet]
    public async Task<IActionResult> Logout()//check if work properly
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }


    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken([FromBody] Guid id)//redo
    {
        var user =  _blogContext.Users.FirstOrDefault(u => u.Id == id);
       

        var refreshToken = Request.Cookies["refreshToken"];
        if(user == null)
        {
            return NotFound("Not found such user");
        }
        if (!user.RefreshToken.Equals(refreshToken))
        {
            return Unauthorized("Invalid Refresh Token.");
        }
        else if (user.TokenExpires < DateTime.Now)
        {
            return Unauthorized("Token expired.");
        }

        string token = _userService.CreateToken(user, _configuration);
        var newRefreshToken = _userService.GenerateRefreshToken();
        
        await _userService.SetRefreshToken(newRefreshToken,user,HttpContext, _blogContext, CancellationToken.None);

        return Ok(token);
    }

}