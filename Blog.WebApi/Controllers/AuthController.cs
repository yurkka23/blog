using Blog.Application.Users.Queries.GetUser;

namespace Blog.WebApi.Controllers;

[Route("auth/")]
[ApiController]

public class AuthController : BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IUserService _userService;
    private readonly ICacheService _cacheService;

    public AuthController(IUserService userService,
        SignInManager<User> signInManager,ICacheService cacheService, UserManager<User> userManager , IMediator mediator) : base(mediator)
    {
        _userService = userService;
        _signInManager = signInManager;
        _userManager = userManager;
        _cacheService = cacheService;   
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<Guid>> Register([FromBody] UserRegisterDTO request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var query = new CheckUserQuery
        {
            UserName = request.UserName
        };
        var existUser = await Mediator.Send(query);

        if (existUser)
        {
            return BadRequest("User with such username already exists");
        }

        var newUser = new User
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
            await _signInManager.SignInAsync(newUser, false);
            await _cacheService.DeleteAsync("UserListSearch");
            return Ok(newUser.Id);
        }

        return BadRequest(result.Errors);
    }

    [AllowAnonymous]
    [HttpPost("login/")]
    public async Task<ActionResult<AuthRefreshDTO>> Login([FromBody] UserLoginDTO request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var query = new CheckUserQuery
        {
            UserName = request.UserName
        };
        var existUser = await Mediator.Send(query);

        if (!existUser)
        {
            return BadRequest("User with such username doesn't exists");
        }


        var res = _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);

        if (!res.Result.Succeeded)
        {
            return BadRequest("Wrong password");
        }

        var queryUser = new GetUserQuery
        {
            UserName = request.UserName
        };

        var user =  await Mediator.Send(queryUser);

        var token = _userService.CreateToken(user);

        var refreshToken = _userService.GenerateRefreshToken();
        await _userService.SetRefreshToken(refreshToken, user, HttpContext, CancellationToken.None);
        var response = new AuthResponseDTO
        {
            JwtToken = token,
            RefreshToken = refreshToken.Token,
            User = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AboutMe = user.AboutMe,
                ImageUserUrl = user.ImageUserUrl,
                Role = user.Role
            }
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("login-with-facebook/")]
    public async Task<ActionResult<AuthRefreshDTO>> LoginWithFacebook([FromBody] FacebookLoginDTO request)
    {
        string UserNameDTO = request.Email?.Split('@')[0] ?? request.Id;

        var queryUser = new GetUserQuery
        {
            UserName = UserNameDTO
        };

        var user = await Mediator.Send(queryUser);


        if (user == null)
        {
            user = new User
            {
                UserName = UserNameDTO,
                FirstName = request.FirstName,
                LastName = request.LastName,
                ImageUserUrl = request.AvatarUrl,
                AboutMe = null,
                Role = Role.User
            };

            var result = await _userManager.CreateAsync(user);


            if (result.Succeeded)
            {
                 await _cacheService.DeleteAsync("UserListSearch");
            }
        }

        await _signInManager.SignInAsync(user, false);

        var token = _userService.CreateToken(user);

        var refreshToken = _userService.GenerateRefreshToken();
        await _userService.SetRefreshToken(refreshToken, user, HttpContext, CancellationToken.None);
        var response = new AuthResponseDTO
        {
            JwtToken = token,
            RefreshToken = refreshToken.Token,
            User = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AboutMe = user.AboutMe,
                ImageUserUrl = user.ImageUserUrl,
                Role = user.Role
            }
        };
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthRefreshDTO>> RefreshToken([FromBody] AuthRequestDTO request)
    {
        var queryUser = new GetUserQuery
        {
            UserName = UserName
        };

        var user = await Mediator.Send(queryUser);

        if (user == null)
        {
            return NotFound("Not found such user");
        }

        if (!user.RefreshToken.Equals(request.RefreshToken))
        {
            return BadRequest("Invalid Refresh Token.");
        }
        if (user.TokenExpires < DateTime.UtcNow)
        {
            return BadRequest("Token expired.");
        }

        string token = _userService.CreateToken(user);
        var newRefreshToken = _userService.GenerateRefreshToken();

        await _userService.SetRefreshToken(newRefreshToken, user, HttpContext, CancellationToken.None);

        var response = new AuthRefreshDTO
        {
            jwtToken = token,
            refreshToken = newRefreshToken.Token
        };
        return Ok(response);
    }

}